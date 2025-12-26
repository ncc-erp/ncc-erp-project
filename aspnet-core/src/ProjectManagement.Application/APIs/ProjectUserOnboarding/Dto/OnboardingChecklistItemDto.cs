using System.Collections.Generic;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class OnboardingChecklistItemDto
    {
        public string Key { get; set; } 
        public string Label { get; set; } 
        public bool IsChecked { get; set; } = false;
        public List<string> Details { get; set; } = new List<string>();
    }
}
