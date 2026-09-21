using System.Collections.Generic;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public sealed class AcceptanceReportDto
    {
        public string Role { get; set; }

        public List<string> Descriptions { get; set; } = new List<string>();
    }

    public sealed class AcceptanceReportResponseDto
    {
        public List<AcceptanceReportDto> Items { get; set; } = new List<AcceptanceReportDto>();
    }

    public sealed class AcceptanceReportRequest
    {
        public long TimesheetProjectId { get; set; }
    }
}
