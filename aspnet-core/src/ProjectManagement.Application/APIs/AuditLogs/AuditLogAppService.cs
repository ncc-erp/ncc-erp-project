using Abp.Auditing;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.AuditLogs.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.AuditLogs
{
    [AbpAuthorize]
    public class AuditLogAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_AuditLogs_View)]
        public async Task<GridResult<GetAuditLogDto>> GetAllPagging(GridParam input)
        {
            var qEmailAddress = WorkScope.GetAll<User>().Select(s => new { s.Id, s.EmailAddress });
            var query = WorkScope.GetAll<AuditLog>()
                .Select(s => new GetAuditLogDto
                {
                    ExecutionDuration = s.ExecutionDuration,
                    ExecutionTime = s.ExecutionTime,
                    MethodName = s.MethodName,
                    Parameters = s.Parameters,
                    ServiceName = s.ServiceName,
                    UserId = s.UserId,
                    UserIdString = s.UserId.ToString(),
                    EmailAddress = qEmailAddress.Where(x => x.Id == s.UserId).Select(x => x.EmailAddress).FirstOrDefault()
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<GetAllEmailAddressInAuditLogDto>> GetAllEmailAddressInAuditLog()
        {
            var userIdInAuditLog = await WorkScope.GetAll<AuditLog>().Where(s => s.UserId != null)
                .Select(s => s.UserId).Distinct().ToListAsync();

            var emailAddressByUserId = WorkScope.GetAll<User>().Where(s => userIdInAuditLog.Contains(s.Id)).Select(s => new GetAllEmailAddressInAuditLogDto
            {
                EmailAddress = s.EmailAddress,
                UserId = s.Id
            }).ToListAsync();

            return await emailAddressByUserId;
        }
    }
}
