using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ReleaseUserToPoolDto
    {
        public long ProjectUserId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Note { get; set; }
    }
}
