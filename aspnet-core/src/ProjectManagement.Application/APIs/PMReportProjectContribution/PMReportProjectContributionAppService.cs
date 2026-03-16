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

                var usersQuery = WorkScope.GetAll<User>().AsNoTracking()
                    .Where(u => WorkScope.GetAll<WeeklyContributionHistory>()
                        .Any(h => h.PMReportId == pmReportId && h.UserId == u.Id));

                if (hasBranchFilter)
                {
                    usersQuery = usersQuery.Where(u => u.BranchId.HasValue && branchIds.Contains(u.BranchId.Value));
                }
                else
                {
                    usersQuery = usersQuery.Where(u => u.BranchId.HasValue && u.Branch.IsDeleted == false);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(search)
                                                 || u.FullName.ToLower().Contains(search)
                                                 || u.EmailAddress.ToLower().Contains(search));
                }

                var totalUserCount = await usersQuery.CountAsync();

                if (totalUserCount == 0)
                {
                    return new GridResult<UserGroupContributionDto>(new List<UserGroupContributionDto>(), 0);
                }

                var filteredUserIds = await usersQuery.Select(u => u.Id).ToListAsync();

                var historyData = await WorkScope.GetAll<WeeklyContributionHistory>()
                    .AsNoTracking()
                    .Where(x => x.PMReportId == pmReportId && filteredUserIds.Contains(x.UserId))
                    .Select(g => new
                    {
                        g.UserId,
                        UserName = g.User.UserName,
                        UserFullName = g.User.FullName,
                        AvatarPath = g.User.AvatarPath,
                        BranchDisplayName = g.User.Branch != null ? g.User.Branch.DisplayName : "No Branch",
                        BranchColor = g.User.Branch != null ? g.User.Branch.Color : "#ccc",
                        PositionName = g.User.Position != null ? g.User.Position.Name : "",
                        PositionColor = g.User.Position != null ? g.User.Position.Color : "",
                        g.ProjectId,
                        ProjectName = g.Project.Name,
                        PMName = g.Project.PM != null ? g.Project.PM.FullName : "No PM",
                        AccountName = g.ProjectUserBill.AccountName ?? g.ProjectUserBill.User.FullName,
                        BillRole = g.ProjectUserBill.BillRole,
                        g.Contribute,
                        HeadCount = g.ProjectUserBill.HeadCount
                    })
                    .ToListAsync();

                var resultItems = historyData
                    .GroupBy(u => new { u.UserId, u.UserName, u.UserFullName, u.AvatarPath, u.BranchDisplayName, u.BranchColor, u.PositionName, u.PositionColor })
                    .Select(g => new UserGroupContributionDto
                    {
                        UserId = g.Key.UserId,
                        UserName = g.Key.UserName,
                        UserFullName = g.Key.UserFullName,
                        AvatarPath = g.Key.AvatarPath,
                        BranchDisplayName = g.Key.BranchDisplayName,
                        BranchColor = g.Key.BranchColor,
                        PositionName = g.Key.PositionName,
                        PositionColor = g.Key.PositionColor,
                        TotalHeadCount = g.Sum(x => x.HeadCount * x.Contribute),
                        Projects = g.GroupBy(p => new { p.ProjectId, p.ProjectName, p.PMName })
                            .Select(pg => new ProjectUserContributionDto
                            {
                                ProjectId = pg.Key.ProjectId,
                                ProjectName = pg.Key.ProjectName,
                                PMName = pg.Key.PMName,
                                TotalContribute = pg.Sum(x => x.Contribute),
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
                
                var historyQuery = WorkScope.GetAll<WeeklyContributionHistory>()
                    .AsNoTracking()
                    .Where(x => x.PMReportId == pmReportId);

                var userFilterQuery = WorkScope.GetAll<User>().AsNoTracking();

                if (hasBranchFilter)
                {
                    userFilterQuery = userFilterQuery.Where(u => u.BranchId.HasValue && branchIds.Contains(u.BranchId.Value));
                }
                else
                {
                    userFilterQuery = userFilterQuery.Where(u => u.BranchId.HasValue && u.Branch.IsDeleted == false);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    userFilterQuery = userFilterQuery.Where(u => u.UserName.ToLower().Contains(search)
                                                             || u.FullName.ToLower().Contains(search)
                                                             || u.EmailAddress.ToLower().Contains(search));
                }

                var totalContribution = await historyQuery
                    .Where(h => userFilterQuery.Any(u => u.Id == h.UserId))
                    .SumAsync(x => x.Contribute);

                return totalContribution;
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
