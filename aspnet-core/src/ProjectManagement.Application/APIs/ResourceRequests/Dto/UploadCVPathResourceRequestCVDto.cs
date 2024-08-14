using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UploadCVPathResourceRequestCVDto
    {
        public long resourceRequestCVId { get; set; }
        public IFormFile file { get; set; }
    }
}
