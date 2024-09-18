using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateResourceRequestCVNote
    {
        public long ResourceRequestCVId { get; set; }
        public string Note { get; set; }
    }
}
