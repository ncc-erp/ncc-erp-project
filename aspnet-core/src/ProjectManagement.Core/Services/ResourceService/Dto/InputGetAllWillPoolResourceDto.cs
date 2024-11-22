using System;
using System.Collections.Generic;
using NccCore.Paging;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class InputGetAllWillPoolResourceDto : GridParam
    {
        public string UserName { get; set; }
        public List<long> BranchIds { get; set; }
        public List<UserType> UserTypes { get; set; }
        public DateTime EndChargeDateFrom { get; set; }
        public DateTime EndChargeDateTo { get; set; }
        public bool SortContribute { get; set; }
        public bool SortEndChargeDate { get; set; }
    }
}