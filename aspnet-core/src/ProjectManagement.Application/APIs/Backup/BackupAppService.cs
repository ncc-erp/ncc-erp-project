using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using NccCore.Paging;
using ProjectManagement.Authorization;
using ProjectManagement.Services.Backup;
using ProjectManagement.Services.Backup.Dto;

namespace ProjectManagement.APIs.Backup
{
    [AbpAuthorize(PermissionNames.Admin_Backup)]
    public class BackupAppService : ProjectManagementAppServiceBase
    {
        private readonly BackupManager _backupManager;

        public BackupAppService(BackupManager backupManager)
        {
            _backupManager = backupManager;
        }
        
        [HttpPost]
        public async Task BackupMonthlyUserContribution([FromForm] DateTime monthYearTime)
        {
             await _backupManager.BackupMonthlyUserContribution(monthYearTime);
        }
        
        [HttpPost]
        public async Task<GridResult<InfoMonthlyUserContributionDto>> GetAllBackupMonthlyUserContribution(FilterMonthlyUserContributionDto input)
        {
            return await _backupManager.GetAllBackupMonthlyUserContribution(input);
        }
    }
}