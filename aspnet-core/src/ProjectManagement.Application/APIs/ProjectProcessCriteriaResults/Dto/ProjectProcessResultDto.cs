using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.ProjectProcessCriteriaResults.Dto
{
    public class ProjectProcessResultDto : Entity<long>
    {
        public long ProjectId { get; set; }
    }
  
}
