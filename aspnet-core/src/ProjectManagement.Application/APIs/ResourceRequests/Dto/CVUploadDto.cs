using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class CVUploadDto
    {
        public long ResourceRequestId { get; set; }
        public IFormFile file { get; set; }
    }
}
