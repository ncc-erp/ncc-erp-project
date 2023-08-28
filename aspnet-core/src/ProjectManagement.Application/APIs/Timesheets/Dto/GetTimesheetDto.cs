using Abp.Application.Services.Dto;
using NccCore.Anotations;
using ProjectManagement.APIs.TimesheetProjects.Dto;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Timesheets.Dto
{
    public class GetTimesheetDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public bool CreatedInvoice { get; set; }
        public long TotalProject { get; set; }
        public long TotalTimesheet { get; set; }
        public float? TotalWorkingDay { get; set; }
        public int TotalIsRequiredFile { get; set; }
        public int TotalHasFile { get; set; }
        public double WorkingDayOfUser { get; set; }
        public double ManMonth { 
            get {
                return Math.Round((double) (WorkingDayOfUser/(TotalWorkingDay.HasValue ? TotalWorkingDay.Value : 22)), 2);
            }
        }

    }
}
