using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateRequestNoteDto
    {
        public long ResourceRequestId { get; set; }

        public string Note { get; set; }

    }
}
