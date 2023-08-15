using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects.Dto
{
    [AutoMapTo(typeof(Entities.Project))]
    public class TrainingProjectDto : Entity<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectStatus Status { get; set; }
        public long PmId { get; set; }
        public long? ClientId { get; set; }
        public string BriefDescription { get; set; }
        public string DetailDescription { get; set; }
        public string TechnologyUsed { get; set; }
        public string TechnicalProblems { get; set; }
        public string OtherProblems { get; set; }
        public string NewKnowledge { get; set; }
        public string Evaluation { get; set; }
        public bool IsRequiredWeeklyReport { get; set; }
    }
}
