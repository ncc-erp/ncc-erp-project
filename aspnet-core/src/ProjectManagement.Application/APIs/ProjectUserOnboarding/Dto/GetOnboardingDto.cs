using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class GetOnboardingDto
    {
        public long ProjectUserId { get; set; }
        public ProjectUserOnboardingStatus Status { get; set; }
        public List<OnboardingChecklistItemDto> Checklist { get; set; }
    }
}
