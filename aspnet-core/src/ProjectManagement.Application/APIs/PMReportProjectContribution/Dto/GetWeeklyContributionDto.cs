using NccCore.Anotations;
namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class GetWeeklyContributionDto
    {
      
        public long PMReportId { get; set; }
        [ApplySearch]
        public string PMReportName { get; set; }
    }
}
