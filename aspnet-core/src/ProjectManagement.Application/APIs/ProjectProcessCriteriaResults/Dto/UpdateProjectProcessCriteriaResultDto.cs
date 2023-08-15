using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessCriteriaResults.Dto
{
    public class UpdateProjectProcessCriteriaResultDto : EntityDto<long>
    {
        public NCStatus Status { get; set; }
        public string Note { get; set; }
    }
}
