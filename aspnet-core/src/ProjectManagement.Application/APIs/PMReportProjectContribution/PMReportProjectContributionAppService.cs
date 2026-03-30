using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.PMReportProjectContribution.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Migrations;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectManagement.APIs.PMReportProjectContribution
{
    [AbpAuthorize]
    public class PMReportProjectContributionAppService : ProjectManagementAppServiceBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly TimesheetService _timesheetService;

        private readonly ResourceManager _resourceManager;

        public PMReportProjectContributionAppService(IBackgroundJobManager backgroundJobManager,
            TimesheetService timesheetService,
            ResourceManager resourceManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _timesheetService = timesheetService;
            _resourceManager = resourceManager;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<long> CreateOrUpdate(CreateOrEditWeeklyContributionDto input)
        {
            try
            {
                var resource = await WorkScope.GetAll<LinkedResource>()
                .FirstOrDefaultAsync(lr => lr.ProjectUserBillId == input.ProjectUserBillId && lr.UserId == input.UserId);

                if (resource == null)
                    throw new UserFriendlyException("Linked Resource not found");

                if (await CheckTotalContribute(input.ProjectUserBillId, input.Contribute, input.UserId))
                    throw new UserFriendlyException("Total contribution cannot exceed 100%.");

                resource.Contribute = input.Contribute;
                await WorkScope.UpdateAsync(resource);

                WeeklyContributionHistory history = null;

                if (input.Id.HasValue && input.Id > 0)
                {
                    history = await WorkScope.GetAsync<WeeklyContributionHistory>(input.Id.Value);
                }
                else
                {
                    history = await WorkScope.GetAll<WeeklyContributionHistory>()
                        .FirstOrDefaultAsync(x => x.PMReportId == input.PMReportId
                                               && x.UserId == input.UserId && x.ProjectUserBillId == input.ProjectUserBillId);
                }

                if (history != null)
                {
                    history.Contribute = input.Contribute;
                    await WorkScope.UpdateAsync(history);
                }
                else
                {

                    history = new WeeklyContributionHistory
                    {
                        ProjectUserBillId = input.ProjectUserBillId,
                        PMReportId = input.PMReportId,
                        Contribute = input.Contribute,
                        UserId = input.UserId,
                        ProjectId = input.ProjectId,
                        TenantId = AbpSession.TenantId
                    };
                    input.Id = await WorkScope.InsertAndGetIdAsync(history);
                }

                return history.Id;

            }
            catch (Exception ex)
            {
                Logger.Error("Error in CreateOrUpdate Weekly Contribution", ex);
                throw new UserFriendlyException("An error occurred while saving the contribution.");

            }
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<GridResult<UserGroupContributionDto>> GetAllPagingContributions([FromBody] ContributionInputDto input, [FromQuery] long pmReportId)
        {
            try
            {
                var branchIds = input.BranchIds?.ToList() ?? new List<long>();
                var hasBranchFilter = branchIds.Any();
                var search = !string.IsNullOrWhiteSpace(input.SearchText) ? input.SearchText.Trim().ToLower() : "";

                // Get all active staff
                var activeUsersQuery = WorkScope.GetAll<User>()
                    .AsNoTracking()
                    .Where(u => u.IsActive);

                // Apply branch filter
                if (hasBranchFilter)
                {
                    activeUsersQuery = activeUsersQuery
                        .Where(u => u.BranchId.HasValue && branchIds.Contains(u.BranchId.Value));
                }
                else
                {
                    var activeBranchIds = await WorkScope.GetAll<Branch>()
                        .AsNoTracking()
                        .Where(b => !b.IsDeleted)
                        .Select(b => b.Id)
                        .ToListAsync();

                    activeUsersQuery = activeUsersQuery
                        .Where(u => u.BranchId.HasValue && activeBranchIds.Contains(u.BranchId.Value));
                }

                // Get user basic info (query db)
                var usersQuery = await activeUsersQuery
                    .Select(u => new
                    {
                        u.Id,
                        UserName = u.UserName.Trim().ToLower(),
                        FullName = u.FullName.Trim().ToLower(),
                        EmailAddress = u.EmailAddress.Trim().ToLower(),
                        u.AvatarPath,

                        BranchDisplayName = u.Branch.DisplayName,
                        BranchColor = u.Branch.Color,
                        PositionName = u.Position.Name,
                        PositionColor = u.Position.Color
                    })
                    .ToListAsync();

                // Apply search
                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery
                        .Where(u => u.UserName.Contains(search)
                                 || u.FullName.Contains(search)
                                 || u.EmailAddress.Contains(search))
                        .ToList();
                }

                var totalUserCount = usersQuery.Count;

                if (totalUserCount == 0)
                {
                    return new GridResult<UserGroupContributionDto>(new List<UserGroupContributionDto>(), 0);
                }

                var filteredUserIds = usersQuery.Select(u => u.Id).ToList();

                // JOIN with WeeklyContributionHistory => contribution data
                var historyData = await WorkScope.GetAll<WeeklyContributionHistory>()
                    .AsNoTracking()
                    .Where(x => x.PMReportId == pmReportId && filteredUserIds.Contains(x.UserId))
                    .Select(g => new
                    {
                        g.UserId,
                        g.ProjectId,
                        ProjectName = g.Project.Name,
                        PMName = g.Project.PM != null ? g.Project.PM.FullName : "No PM",
                        AccountName = g.ProjectUserBill.AccountName ?? g.ProjectUserBill.User.FullName,
                        BillRole = g.ProjectUserBill.BillRole,
                        g.Contribute,
                        HeadCount = g.ProjectUserBill.HeadCount
                    })
                    .ToListAsync();

                // Group and aggregate with default values for users without contribution
                var resultItems = usersQuery
                    .Select(user => new UserGroupContributionDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserFullName = user.FullName,
                        AvatarPath = user.AvatarPath,
                        BranchDisplayName = user.BranchDisplayName,
                        BranchColor = user.BranchColor,
                        PositionName = user.PositionName,
                        PositionColor = user.PositionColor,
                        TotalHeadCount = historyData
                            .Where(h => h.UserId == user.Id)
                            .Sum(x => x.HeadCount * x.Contribute),
                        Projects = historyData
                            .Where(h => h.UserId == user.Id)
                            .GroupBy(p => new { p.ProjectId, p.ProjectName, p.PMName })
                            .Select(pg => new ProjectUserContributionDto
                            {
                                ProjectId = pg.Key.ProjectId,
                                ProjectName = pg.Key.ProjectName,
                                PMName = pg.Key.PMName,
                                TotalContribute = pg.Sum(x => x.Contribute * x.HeadCount),
                                BillDetails = pg.Select(detail => new ProjectBillDetailDto
                                {
                                    AccountName = detail.AccountName,
                                    BillRole = detail.BillRole,
                                    Contribute = detail.Contribute,
                                    HeadCount = detail.HeadCount
                                }).ToList()
                            }).ToList()
                    })
                    .ToList();

                var sortColumn = input.Sort?.Trim().ToLower();

                var isDesc = input.SortDirection == SortDirection.DESC;

                IEnumerable<UserGroupContributionDto> sortedQuery;

                switch (sortColumn)
                {
                    case "totalheadcount":
                        sortedQuery = isDesc ? resultItems.OrderByDescending(x => x.TotalHeadCount) : resultItems.OrderBy(x => x.TotalHeadCount);
                        break;
                    case "username":
                        sortedQuery = isDesc ? resultItems.OrderByDescending(x => x.UserName) : resultItems.OrderBy(x => x.UserName);
                        break;
                    case "userfullname":
                        sortedQuery = isDesc ? resultItems.OrderByDescending(x => x.UserFullName) : resultItems.OrderBy(x => x.UserFullName);
                        break;
                    case "branchdisplayname":
                        sortedQuery = isDesc ? resultItems.OrderByDescending(x => x.BranchDisplayName) : resultItems.OrderBy(x => x.BranchDisplayName);
                        break;
                    default:
                        sortedQuery = resultItems.OrderBy(x => x.UserName);
                        break;
                }
                var finalItems = sortedQuery
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                return new GridResult<UserGroupContributionDto>(finalItems, totalUserCount);
            }
            catch (Exception ex)
            {
                Logger.Error("Error in GetAllPagingContributions", ex);
                throw new UserFriendlyException(ex.Message);
            }
        }


        [HttpPost]
        [AbpAuthorize]
        public async Task<float> GetTotalContribution([FromBody] ContributionInputDto input, [FromQuery] long pmReportId)
        {
            try
            {
                var branchIds = input.BranchIds?.ToList() ?? new List<long>();
                var hasBranchFilter = branchIds.Any();
                var search = !string.IsNullOrWhiteSpace(input.SearchText) ? input.SearchText.Trim().ToLower() : "";

                var userIdsInReport = await WorkScope.GetAll<WeeklyContributionHistory>()
                    .Where(h => h.PMReportId == pmReportId)
                    .Select(h => h.UserId)
                    .Distinct()
                    .ToListAsync();

                if (!userIdsInReport.Any())
                    return 0;

                var usersQuery = await WorkScope.GetAll<User>()
                    .AsNoTracking()
                    .Where(u => userIdsInReport.Contains(u.Id))
                    .Select(u => new
                    {
                        Id = u.Id,
                        UserName = u.UserName.Trim().ToLower(),
                        FullName = u.FullName.Trim().ToLower(),
                        EmailAddress = u.EmailAddress.Trim().ToLower(),
                        u.BranchId,
                    })
                    .ToListAsync();

                if (hasBranchFilter)
                {
                    usersQuery = usersQuery
                        .Where(u => u.BranchId.HasValue && branchIds.Contains(u.BranchId.Value))
                        .ToList();
                }
                else
                {
                    var activeBranchIds = await WorkScope.GetAll<Branch>()
                        .AsNoTracking()
                        .Where(b => !b.IsDeleted)
                        .Select(b => b.Id)
                        .ToListAsync();

                    usersQuery = usersQuery
                        .Where(u => u.BranchId.HasValue && activeBranchIds.Contains(u.BranchId.Value))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery.Where(u => u.UserName.Contains(search)
                                                 || u.FullName.Contains(search)
                                                 || u.EmailAddress.Contains(search)).ToList();
                }

                var totalUserCount = usersQuery.Count;

                if (totalUserCount == 0)
                    return 0;

                var filteredUserIds = usersQuery.Select(u => u.Id).ToList();

                var totalContribution = await WorkScope.GetAll<WeeklyContributionHistory>()
                    .AsNoTracking()
                    .Where(x => x.PMReportId == pmReportId && filteredUserIds.Contains(x.UserId))
                    .SumAsync(x => x.Contribute * x.ProjectUserBill.HeadCount);

                return (float)Math.Round(totalContribution / 100.0, 3);
            }
            catch (Exception ex)
            {
                Logger.Error("Error in GetTotalContribution", ex);
                throw new UserFriendlyException(ex.Message);
            }

        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.WeeklyContributionReport)]
        public async Task<GridResult<GetWeeklyContributionDto>> GetAllPaging(GridParam input)
        {
            try
            {
                var pmReportProject = WorkScope.GetAll<WeeklyContributionHistory>();

                var query = pmReportProject
                    .GroupBy(x => new {
                        x.PMReportId,
                        PMReportName = x.PMReport.Name,
                    })
                    .Select(g => new GetWeeklyContributionDto
                    {
                        PMReportId = g.Key.PMReportId,
                        PMReportName = g.Key.PMReportName,
                    });
                return await query.GetGridResult(query, input);
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        #region API Helper
        private async Task<bool> CheckTotalContribute(long projectUserBillId, byte contribute, long userId)
        {
            var totalContribute = await WorkScope.GetAll<LinkedResource>()
                .Where(lr => lr.ProjectUserBillId == projectUserBillId && lr.UserId != userId)
                .SumAsync(lr => lr.Contribute);

            if (totalContribute + contribute > 100)
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
