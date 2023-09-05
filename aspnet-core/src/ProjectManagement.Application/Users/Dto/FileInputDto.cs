using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Users.Dto
{
    public class FileInputDto
    {
        public IFormFile File { get; set; }
    }
}
