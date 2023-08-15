using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Phases.Dto
{
    [AutoMapTo(typeof(Phase))]
    public class PhaseDto : EntityDto<long>
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public long ParentId { get; set; }
        public PhaseType Type { get; set; }
        [DefaultValue("false")]
        public PhaseStatus Status { get; set; }
        [DefaultValue("false")]
        public bool IsCriteria { get; set; }
        public int Index { get; set; }
    }
}
