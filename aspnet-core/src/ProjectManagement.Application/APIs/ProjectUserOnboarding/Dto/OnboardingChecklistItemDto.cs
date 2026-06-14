using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class OnboardingChecklistItemDto : EntityDto<long>
    {
        public string Label { get; set; }

        public string Details { get; set; }

        public bool IsChecked { get; set; }
    }
}
