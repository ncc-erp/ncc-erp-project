using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.OffboardUserManager.Dto
{
    public class UpdateOffboardNoteDto
    {
        public long OffboardUserId { get; set; }
        public string Note { get; set; }
    }
}
