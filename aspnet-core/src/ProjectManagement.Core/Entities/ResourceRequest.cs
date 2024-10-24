﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ResourceRequest : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(1000)]
        public string Name { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }
        public DateTime TimeNeed { get; set; }
        public ResourceRequestStatus Status { get; set; }
        public DateTime? TimeDone { get; set; }
        [MaxLength(10000)]
        public string CVName { get; set; }
        [MaxLength(10000)]
        public string PMNote { get; set; }
        public string DMNote { get; set; }
        public string Code { get; set; }
        [ForeignKey(nameof(BillAccountId))]
        public User User { get; set; }
        public long? BillAccountId { get; set; }
        public DateTime? BillStartDate { get; set; }
        public bool IsRecruitmentSend { get; set; }
        public string RecruitmentUrl { get; set; }

        public int Quantity { get; set; }
        public UserLevel Level { get; set; }
        public Priority Priority { get; set; }
        public bool IsNewBillAccount { get; set; }
        public bool IsRequiredPlanResource { get; set; }

        public ICollection<ResourceRequestSkill> ResourceRequestSkills { get; set; }
        public string LinkCV { get; set; }
        public ICollection<ResourceRequestCV> ResourceRequestCVs { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
