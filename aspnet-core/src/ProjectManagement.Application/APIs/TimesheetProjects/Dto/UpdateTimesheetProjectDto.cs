using ProjectManagement.APIs.ProjectUserBills.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class UpdateTimesheetProjectDto : BaseInvoiceSettingDto
    {
        public long Id { get; set; }
        public float WorkingDay { get; set; }    
        public float TransferFee { get; set; }

    }
}
