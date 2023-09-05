
using Abp.Domain.Entities;
using ProjectManagement.APIs.AuditResultPeoples.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectCheckLists.Dto
{
    public class CheckListItemByProjectDto : Entity<long>
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public string Code { get; set; }
        public string CategoryName { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string AuditTarget { get; set; }
        [MaxLength(255)]
        public string PersonInCharge { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<GetAuditResultPeopleDto> people { get; set; }
        public List<ProjectType> Mandatories { get; set; }
    }
}
