using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.OffboardUserManager.Dto
{
    public class SaveOffboardChecklistDto
    {
        public long OffboardHistoryId { get; set; }
        public List<long> CheckedProjectAssetIds { get; set; } = new List<long>();
        public List<long> CheckedAccountAssetIds { get; set; } = new List<long>();
    }
}
