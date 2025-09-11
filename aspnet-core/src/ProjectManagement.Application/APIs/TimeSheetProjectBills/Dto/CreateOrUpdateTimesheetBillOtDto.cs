using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class CreateOrUpdateTimesheetBillOtDto
    {
        public long? TimesheetProjectBillOtTypesId { get; set; }
        public long ProjectOtTypeId { get; set; }
        public long TimesheetProjectBillId { get; set; }
        public decimal Hours { get; set; }
        public TimesheetBillOtActionMode Mode { get; set; }
    }
}
