using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class GetTimesheetProjectDto : EntityDto<long>
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TimesheetFile { get; set; }
        public string TimeSheetName { get; set; }
        public string Note { get; set; }
        public string ProjectBillInfomation { get; set; }
        public string HistoryFile { get; set; }
    }

    public class GetTimeSheetProjectToBackgroundJobDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectCode { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        [ApplySearchAttribute]
        public string PMName { get; set; }
        [ApplySearchAttribute]
        public string ClientCode { get; internal set; }
        [ApplySearchAttribute]
        public string ClientName { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public long TimeSheetId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ReactiveTimesheetProjectDto
    {
        public DateTime? CloseDate { get; set; }
        public List<long> TimesheetProjectIds { get; set; }
    }
}