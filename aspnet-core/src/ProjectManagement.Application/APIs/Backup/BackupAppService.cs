using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Services.Backup;

namespace ProjectManagement.APIs.Backup
{
    [AbpAuthorize]
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
    }
}