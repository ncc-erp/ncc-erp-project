using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore;
using NccCore.IoC;
using NccCore.Paging;
using ProjectManagement.Authorization.Users;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Entities;
using ProjectManagement.Services.Backup.Dto;

namespace ProjectManagement.Services.Backup
{
    public class BackupManager: ApplicationService
    {
        private readonly IWorkScope _workScope;

        public BackupManager(IWorkScope workScope)
        {
            _workScope = workScope;
        }

        public async Task BackupMonthlyUserContribution(DateTime monthYearTime)
        {
            byte month = (byte)monthYearTime.Month;
            int year = monthYearTime.Year;
            await DeleteExistMonthlyUserContribution(month, year);
            var monthlyUserContributionList = await GetAllMonthlyUserContribution(month, year);
            var mappedMonthlyUserContributions = ObjectMapper.Map<IEnumerable<MonthlyUserContribution>>(monthlyUserContributionList);
            await _workScope.InsertRangeAsync(mappedMonthlyUserContributions);
        }

        public async Task<GridResult<InfoMonthlyUserContributionDto>> GetAllBackupMonthlyUserContribution(FilterMonthlyUserContributionDto input)
        {
            var qUserIds = _workScope.GetAll<User>()
                .AsNoTracking()
                .Where(u => u.UserType != UserType.FakeUser).AsEnumerable()
                .WhereIf(input.BranchIds != null && input.BranchIds.Any(),
                    u => u.BranchId != null && input.BranchIds.Contains(u.BranchId.Value))
                .WhereIf(input.UserTypes != null && input.UserTypes.Any(),
                    u => input.UserTypes.Contains(u.UserType))
                .WhereIf(input.UserLevels != null && input.UserLevels.Any(),
                    u => input.UserLevels.Contains(u.UserLevel))
                .Select(u => u.Id);

            var qMonthlyUserContribution = _workScope.GetAll<MonthlyUserContribution>()
                .Where(muc => muc.MonthTime == input.MonthTime && muc.YearTime == input.YearTime)
                .Where(muc => qUserIds.Contains(muc.UserId));

            var selectMonthlyUserContribution = await qMonthlyUserContribution.Select(muc => new
            {
                EmployeeId = muc.UserId,
                Employee = new UserInfo
                {
                    Id = muc.User.Id,
                    EmailAddress = muc.User.EmailAddress,
                    AvatarPath = muc.User.AvatarPath,
                    UserType = muc.UserType,
                    UserLevel = muc.UserLevel,
                    FullName = muc.User.FullName,
                    UserName = muc.User.UserName,
                    BranchId = muc.BranchId,
                    BranchColor = muc.Branch.Color,
                    BranchDisplayName = muc.Branch.DisplayName,
                },
                ProjectContributes = new ProjectContributeDto()
                {
                    Id = muc.ProjectId,
                    ProjectType = muc.Project.ProjectType,
                    ProjectName = muc.Project.Name,
                    ProjectCode = muc.Project.Code,
                    Contribute = muc.Contribute
                }
            }).ToListAsync();
            
            var groupResult = selectMonthlyUserContribution.GroupBy(gr => gr.EmployeeId)
                .Select(gr => new InfoMonthlyUserContributionDto
                {
                    Employee = gr.First().Employee,
                    ProjectContributes = gr.Select(pc => pc.ProjectContributes).ToList()
                }).ToList();

            return new GridResult<InfoMonthlyUserContributionDto>(groupResult, groupResult.Count);
        }

        private async Task DeleteExistMonthlyUserContribution(byte month, int year)
        {
            var existMonthlyUserContributionIds = await _workScope.GetAll<MonthlyUserContribution>()
                .Where(x => x.MonthTime == month && x.YearTime == year)
                .Select(x => x.Id)
                .ToListAsync();
            await _workScope.DeleteRangeAsync<MonthlyUserContribution>(existMonthlyUserContributionIds);
        }

        private async Task<List<MonthlyUserContributionDto>> GetAllMonthlyUserContribution(byte month, int year)
        {
            var qLinkedResource = _workScope.GetAll<LinkedResource>()
                .Where(lr => lr.CreationTime.Month == month && lr.CreationTime.Year == year)
                .Where(lr => lr.ProjectUserBill.Project.Status != ProjectStatus.Closed);
            var qUser = qLinkedResource.Select(u => new MonthlyUserContributionDto()
            {
                UserId = u.UserId,
                UserLevel = u.User.UserLevel,
                UserType = u.User.UserType,
                BranchId = u.User.BranchId,
                Contribute = u.Contribute,
                ProjectId = u.ProjectUserBill.ProjectId,
                MonthTime = month,
                YearTime= year
            });
            var result = await qUser.ToListAsync();
            return result;
        }
    }
}