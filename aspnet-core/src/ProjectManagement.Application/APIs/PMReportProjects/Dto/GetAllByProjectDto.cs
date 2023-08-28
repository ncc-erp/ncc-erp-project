
using Abp.Domain.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjects.Dto
{
    public class GetAllByProjectDto
    {
        public long ReportId { get; set; }
        public long PmReportProjectId { get; set; }
        public string PMReportName { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string ProjectHealth { get; set; }
    }
}
