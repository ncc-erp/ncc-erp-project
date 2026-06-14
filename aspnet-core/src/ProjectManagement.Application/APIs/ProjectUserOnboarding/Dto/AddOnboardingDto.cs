using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class AddOnboardingDto
    {
        [Required]
        public long ProjectUserId { get; set; }

        [Required]
        public List<OnboardingChecklistItemDto> Checklist { get; set; }
    }
}
