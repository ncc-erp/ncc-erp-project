using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.Timesheets.Dto
{
    [AutoMapTo(typeof(Timesheet))]
    public class TimesheetDto : EntityDto<long>
    {
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
        public bool CreatedInvoice { get; set; }
        public float? TotalWorkingDay { get; set; }
    }
}
