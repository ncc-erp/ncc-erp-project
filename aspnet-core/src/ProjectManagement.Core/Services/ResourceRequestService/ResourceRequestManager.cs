using Abp.Application.Services;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using NccCore.IoC;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.ResourceRequestService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagement.Services.ResourceRequestService
{
    public class ResourceRequestManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public ResourceRequestManager(IWorkScope workScope)
        {
            _workScope = workScope;

        }

        public IQueryable<GetResourceRequestDto> IQGetResourceRequest()
        {
            var query = from request in _workScope.GetAll<ResourceRequest>()
                        orderby request.Priority descending, request.TimeNeed ascending
                        select new GetResourceRequestDto
                        {
                            DMNote = request.DMNote,
                            Id = request.Id,
                            IsRecruitmentSend = request.IsRecruitmentSend,
                            ProjectName = request.Project.Name,
                            ProjectId = request.ProjectId,
                            ProjectType = request.Project.ProjectType,
                            ProjectStatus = request.Project.Status,

                            Name = request.Name,

                            PMNote = request.PMNote,
                            Priority = request.Priority,

                            PathRecruitment = request.RecruitmentUrl,
                            TimeNeed = request.TimeNeed,
                            TimeDone = request.TimeDone,
                            Status = request.Status,
                            Level = request.Level,
                            CreationTime = request.CreationTime,

                            Quantity = request.Quantity,
                            ProjectCode = request.Project.Code,

                            Skills = request.ResourceRequestSkills.Select(p => new ResourceRequestSkillDto() { Id = p.SkillId, Name = p.Skill.Name }).ToList(),
                            PlanUserInfo = request.ProjectUsers.OrderByDescending(q => q.CreationTime).Select(s => new PlanUserInfoDto
                            {
                                ProjectUserId = s.Id,
                                Employee = new UserBaseDto
                                {
                                    PositionId = s.User.PositionId,
                                    PositionName = s.User.Position.ShortName,
                                    PositionColor = s.User.Position.Color,
                                    Branch = s.User.BranchOld,
                                    BranchColor = s.User.Branch.Color,
                                    BranchDisplayName = s.User.Branch.DisplayName,
                                    UserLevel = s.User.UserLevel,
                                    UserType = s.User.UserType,
                                    FullName = s.User.FullName,
                                    EmailAddress = s.User.EmailAddress,
                                    Id = s.UserId,
                                    AvatarPath = s.User.AvatarPath,
                                },

                                PlannedDate = s.StartTime,

                            }).FirstOrDefault(),
                        };
            return query;
        }


    }
}
