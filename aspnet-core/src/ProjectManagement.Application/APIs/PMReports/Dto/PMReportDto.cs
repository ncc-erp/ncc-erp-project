using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReports.Dto
{
    [AutoMapTo(typeof(PMReport))]
    public class PMReportDto : EntityDto<long>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Year { get; set; }
        public PMReportType Type { get; set; }
        public PMReportStatus PMReportStatus { get; set; }
        public string Note { get; set; }

    }
}
