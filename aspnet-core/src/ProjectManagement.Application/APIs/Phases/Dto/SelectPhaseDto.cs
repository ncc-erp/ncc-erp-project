using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Phases.Dto
{
    [AutoMapTo(typeof(Phase))]
    public class SelectPhaseDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        [ApplySearchAttribute]
        public int Year { get; set; }
        public string ParentName { get; set; }
        public string Type { get; set; }
        public PhaseStatus Status { get; set; }
        public bool IsCriteria { get; set; }
        public int Index { get; set; }
    }
}
