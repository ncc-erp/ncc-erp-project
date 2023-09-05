using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class SetTimesheetProjectCompleteDto : EntityDto<long>
    {
        public bool IsComplete { get; set; }
    }
}
