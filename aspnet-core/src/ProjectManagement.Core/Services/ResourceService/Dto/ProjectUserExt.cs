using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ProjectUserExt
    {
        public ProjectUser  PU { get; set; }
        public KomuProjectInfoDto Project { get; set; }
        public KomuUserInfoDto Employee { get; set; }
    }
}
