using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectFiles.Dto
{
    public class ProjectInputFileDto
    {
        public List<IFormFile> Files { get; set; }
        //public IFormCollection Filess { get; set; }
        public long ProjectId { get; set; }
        //public List<string> FileNames { get; set; }
    }
}
