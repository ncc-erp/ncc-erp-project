using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.APIs.Public.Dto;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    public class SyncProjectWeeklyReportRequestDto
    {
        public long ProjectId { get; set; }
    }

    public class SyncProjectWeeklyReportResponseDto
    {
        public bool Success { get; set; }
    }
}

