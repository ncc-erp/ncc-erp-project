using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectIssues.Dto
{
    [AutoMapTo(typeof(PMReportProjectIssue))]
    public class PMReportProjectIssueDto : EntityDto<long>
    {
        public long PMReportProjectId { get; set; }
        public string Description { get; set; }
        public string Impact { get; set; }
        public IssueCritical Critical { get; set; }
        public ProjectIssueSource Source { get; set; }
        public string Solution { get; set; }
        public string MeetingSolution { get; set; }
        public PMReportProjectIssueStatus Status { get; set; }
    }
}
