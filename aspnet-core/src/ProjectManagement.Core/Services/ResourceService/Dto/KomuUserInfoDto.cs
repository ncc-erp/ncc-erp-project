using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class KomuUserInfoDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long? KomuUserId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string KomuAccountInfo { get
            {
                return KomuUserId.HasValue ? $"<@{KomuUserId}>" : $"**{FullName}**";
            } 
        }
    }
}
