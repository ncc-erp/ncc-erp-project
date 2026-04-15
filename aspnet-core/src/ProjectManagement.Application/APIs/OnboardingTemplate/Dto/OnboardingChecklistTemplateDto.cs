using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.OnboardingTemplate.Dto
{
    public class OnboardingChecklistTemplateDto : EntityDto<long>
    {
        [Required]
        [ApplySearchAttribute]
        public string Label { get; set; }

        public int Order { get; set; }

        public string Details { get; set; }
    }
}
