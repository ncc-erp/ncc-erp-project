using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateUserPoolNoteDto
    {
        public long UserId { get; set; }
        public string Note { get; set; }
    }
}
