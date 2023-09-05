using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class FileInputDto
    {
        public IFormFile File { get; set; }
        public long TimesheetProjectId { get; set; }
    }
}
