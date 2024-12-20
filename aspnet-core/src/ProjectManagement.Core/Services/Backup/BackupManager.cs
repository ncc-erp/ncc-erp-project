using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.EntityFrameworkCore;
using NccCore.IoC;
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