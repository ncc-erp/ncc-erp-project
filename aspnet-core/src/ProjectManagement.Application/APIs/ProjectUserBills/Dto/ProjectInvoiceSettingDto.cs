using ProjectManagement.APIs.Timesheets.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class ProjectInvoiceSettingDto
    {
        public string CurrencyName { get; set; }        
        public long InvoiceNumber { get; set; }
        public float Discount { get; set; }

        public bool IsMainProjectInvoice { get; set; } = true;
        public long? MainProjectId { get; set; }
        public string MainProjectName { get; set; }
        public List<IdNameDto> SubProjects { get; set; }
        public List<string> SubProjectNames => SubProjects.Select(s => s.Name).ToList();
        public List<long> SubProjectIds => SubProjects.Select(s => s.Id).ToList();


    }
}
