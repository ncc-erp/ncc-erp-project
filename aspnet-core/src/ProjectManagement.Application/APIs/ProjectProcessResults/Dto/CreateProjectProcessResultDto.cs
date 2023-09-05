using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    [AutoMapTo(typeof(ProjectProcessResult))]
    public class CreateProjectProcessResultDto : EntityDto<long>
    {
        [Required(ErrorMessage ="Please enter Project Id")]
        public long ProjectId { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
