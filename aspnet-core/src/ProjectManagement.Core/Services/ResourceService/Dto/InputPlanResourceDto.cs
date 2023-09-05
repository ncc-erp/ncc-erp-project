using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class InputPlanResourceDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public bool IsPool { get; set; }
        public byte AllocatePercentage { get; set; }
        public ProjectUserRole ProjectRole { get; set; }

        public DateTime StartTime { get; set; }
        public string Note { get; set; }
    

    }
}
