using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.OffboardUserManager.Dto
{
    public class OffboardChecklistItemDto
    {
        public long ProjectAssetId { get; set; }
        public string AssetName { get; set; }
        public bool IsChecked { get; set; }
    }
}
