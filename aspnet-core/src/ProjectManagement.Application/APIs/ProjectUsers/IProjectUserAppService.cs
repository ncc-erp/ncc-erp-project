using Abp.Application.Services;
using Abp.Dependency;
using ProjectManagement.APIs.ProjectUsers.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectUsers
{
    public interface IProjectUserAppService: IApplicationService, ITransientDependency
    {
        Task<List<GetProjectUserDto>> GetAllByProject(long projectId, bool viewHistory);
        Task<ProjectUserDto> Create(ProjectUserDto input);
    }
}
