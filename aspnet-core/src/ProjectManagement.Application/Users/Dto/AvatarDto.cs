using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Users.Dto
{
    public class AvatarDto
    {
        public IFormFile File { get; set; }
        public long UserId { get; set; }
    }
}
