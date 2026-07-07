using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class UpdateOnboardHistoryNoteDto
    {
        public long ProjectUserOnboardingId { get; set; }
        public string Note { get; set; }
    }
}
