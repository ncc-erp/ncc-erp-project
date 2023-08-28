using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.AuditLogs.Dto
{
    public class GetAllEmailAddressInAuditLogDto
    {
        public long UserId { get; set; }
        public string EmailAddress { get; set; }
    }
}
