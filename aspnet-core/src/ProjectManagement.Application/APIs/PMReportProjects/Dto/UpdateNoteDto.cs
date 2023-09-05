

using Abp.Domain.Entities;

namespace ProjectManagement.APIs.PMReportProjects.Dto
{
    public class UpdateNoteDto : Entity<long>
    {
        public string Note { get; set; }
    }
}
