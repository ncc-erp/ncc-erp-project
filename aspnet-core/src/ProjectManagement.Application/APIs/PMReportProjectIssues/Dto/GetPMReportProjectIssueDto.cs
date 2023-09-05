using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectIssues.Dto
{
    public class GetResultpmReportProjectIssue
    {
        public long PmReportProjectId { get; set; }

        public ProjectHealth ProjectHealth { get; set; }

        public string Status { get; set; }

        public string ProjectHealthString { get; set; }

        public List<GetPMReportProjectIssueDto> Result { get; set; }

        public DateTime? TimeSendReport { get; set; }
    }

    public class GetPMReportProjectIssueDto : EntityDto<long>
    {
        public long PMReportProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string Impact { get; set; }

        public string Critical { get; set; }

        public string Source { get; set; }

        public string Solution { get; set; }

        public string MeetingSolution { get; set; }

        public ProjectHealth ProjectHealth { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int TotalWeekAgo { get; set; }
    }
}