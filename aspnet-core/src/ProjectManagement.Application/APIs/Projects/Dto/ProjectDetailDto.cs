using Abp.AutoMapper;
using ProjectManagement.APIs.Technologys.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Projects.Dto
{
    [AutoMapTo(typeof(Project))]
    public class ProjectDetailDto
    {
        public long ProjectId { get; set; }
        public string BriefDescription { get; set; }
        public string DetailDescription { get; set; }
        public string TechnologyUsed { get; set; }
        public string TechnicalProblems { get; set; }
        public string OtherProblems { get; set; }
        public string NewKnowledge { get; set; }
        public List<TechnologyDto> ProjectTechnologies { get; set; }
        public List<long> ProjectTechnologiesInput { get; set; }
    }
}
