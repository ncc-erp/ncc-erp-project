using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class ProjectInvoiceForExportDto
    {
        public string ClientName { get; set; }
        public List<ProjectInfoForExportDto> ProjectInfor { get; set; }
        public double Total => ProjectInfor.Sum(x => x.ProjectBill);
    }
}