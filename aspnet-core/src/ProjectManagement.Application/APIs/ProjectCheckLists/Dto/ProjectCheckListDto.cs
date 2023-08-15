
using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectCheckLists.Dto
{
    [AutoMapTo(typeof(ProjectCheckList))]
    public class ProjectCheckListDto: Entity<long>
    {
        public long ProjectId { get; set; }
        public long CheckListItemId { get; set; }
        public bool IsActive { get; set; }
    }
}
