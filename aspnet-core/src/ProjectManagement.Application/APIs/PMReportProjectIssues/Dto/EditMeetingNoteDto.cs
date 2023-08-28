using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PMReportProjectIssues.Dto
{
    public class EditMeetingNoteDto : EntityDto
    {
        public string Note { get; set; }
    }
}
