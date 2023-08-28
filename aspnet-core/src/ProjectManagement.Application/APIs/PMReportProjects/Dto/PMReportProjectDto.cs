using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjects.Dto
{
    [AutoMapTo(typeof(PMReportProject))]
    public class PMReportProjectDto : EntityDto<long>
    {
        public long PMReportId { get; set; }
        public long ProjectId { get; set; }
        public PMReportProjectStatus Status { get; set; }
        public ProjectHealth ProjectHealth { get; set; }
        public long PMId { get; set; }
        public string Note { get; set; }
        public bool Seen { get; set; }
    }

    public class PMUnsentWeeklyReportDto
    {
        public int Index { get; set; }
        public string ProjectName { get; set; }
        public string WeeklyName { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }

    public class PMDiscordChanelIdDto
    {
        public List<PMUnsentWeeklyReportDto> list { get; set; }
        public string DiscordChannelId { get; set; }
    }

    public class CheckDateTimeDto
    {
        public bool IsCheck { get; set; }
        public DateTime Time { get; set; }
        public Days Day { get; set; }

    }

    public class InformPmDto
    {
        public string ChannelId { get; set; }
        public List<CheckDateTimeDto> CheckDateTimes { get; set; }
       
    }
}
