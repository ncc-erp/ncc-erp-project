using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ProjectUserExt
    {
        public ProjectUser  PU { get; set; }
        public KomuProjectInfoDto Project { get; set; }
        public KomuUserInfoDto Employee { get; set; }
        public ProjectType ProjectType { set; get; }
        public string PMEmail { set; get; }
    }
}
