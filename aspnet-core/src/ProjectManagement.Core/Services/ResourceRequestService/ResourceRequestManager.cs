using Abp.Application.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.IoC;
using NccCore.Paging;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceRequestService.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Expression = System.Linq.Expressions.Expression;

namespace ProjectManagement.Services.ResourceRequestService
{
    public class ResourceRequestManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public ResourceRequestManager(IWorkScope workScope)
        {
            _workScope = workScope;

        }

        // sort list resource request by multiple condition
        // key: priority, projectName, level, creationTime, timeNeed
        public IQueryable<GetResourceRequestDto> ApplyOrders(IQueryable<GetResourceRequestDto> query,
            IDictionary<string, SortDirection> sortParams)
        {
            if (query == null || sortParams == null) return query;
            var isOrder = true;
            var queryOrder = (IOrderedQueryable<GetResourceRequestDto>)query;
            foreach (var param in sortParams)
            {
                queryOrder = OrderResourceRequest(queryOrder, param.Key, param.Value, isOrder);
                isOrder = false;
            }
            return queryOrder;
        }

        private IOrderedQueryable<GetResourceRequestDto> OrderResourceRequest(IQueryable<GetResourceRequestDto> query,
            string propertyName, SortDirection sortDirection, bool anotherLevel)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(GetResourceRequestDto), string.Empty);
            MemberExpression memberExpression = Expression.PropertyOrField(parameterExpression, propertyName);
            LambdaExpression lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);
            MethodCallExpression methodCallExpression = Expression.Call(
                typeof(Queryable),
                 (anotherLevel ? "OrderBy" : "ThenBy") + (sortDirection == SortDirection.DESC ? "Descending" : string.Empty),
                 new[] { typeof(GetResourceRequestDto), memberExpression.Type },
                 query.Expression,
                 Expression.Quote(lambdaExpression)
                );
            var result = (IOrderedQueryable<GetResourceRequestDto>)query.Provider.CreateQuery(methodCallExpression);
            return result;
        }

        public IQueryable<GetResourceRequestDto> IQGetResourceRequest()
        {
            // get all user skill
            var userSkills = _workScope.GetAll<UserSkill>()
                .Select(x => new UserSkillDto
                {
                    UserId = x.UserId,
                    SkillId = x.SkillId,
                    SkillName = x.Skill.Name,
                    SkillRank = x.SkillRank,
                    SkillNote = x.Note
                }).ToList().GroupBy(u => u.UserId)
                .ToDictionary(group => group.Key, group => group.ToList());

            var query = from request in _workScope.GetAll<ResourceRequest>()
                        orderby request.IsNewBillAccount descending, request.Code ascending
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
                            CVName = request.CVName,
                            LinkCv = request.LinkCV,
                            ResCV = request.ResourceRequestCVs.Select(r => new ResourceRequestCvDto()
                            {
                                UserId = r.UserId,
                                User = new UserBaseDto
                                {
                                    EmailAddress = r.User.EmailAddress,
                                    AvatarPath = r.User.AvatarPath,
                                    UserType = r.User.UserType,
                                    BranchColor = r.User.Branch.Color,
                                    BranchDisplayName = r.User.Branch.DisplayName,
                                    PositionColor = r.User.Position.Color,
                                    PositionName = r.User.Position.Name,
                                    PositionId = r.User.Position.Id,
                                    UserLevel = r.User.UserLevel,
                                    FullName = r.User.FullName,
                                    Branch = r.User.BranchOld,
                                    Id = r.User.Id,
                                },
                                ResourceRequestId = r.ResourceRequestId,
                                Status = r.Status,
                                CVName = r.CVName,
                                CVPath = r.CVPath,
                                Note = r.Note,
                                KpiPoint = r.KpiPoint,
                                InterviewDate = r.InterviewDate,
                                SendCVDate = r.SendCVDate,
                                Id = r.Id,
                                CvStatusId = r.CvStatusId,
                                CvStatus = new CvStatusDto
                                {
                                    Id = r.CvStatus.Id,
                                    Name = r.CvStatus.Name,
                                    Color = r.CvStatus.Color
                                },
                            }).ToList(),
                            Skills = request.ResourceRequestSkills.Select(p => new ResourceRequestSkillDto() { Id = p.SkillId, Name = p.Skill.Name }).ToList(),

                            PlanUserInfo = request.ProjectUsers.Where(x => x.Status == ProjectUserStatus.Future)
                                                               .OrderByDescending(q => q.CreationTime)
                                                               .Select(s => new PlanUserInfoDto
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
                                                                   Role = s.ProjectRole,
                                                                   PlannedDate = s.StartTime,
                                                                   UserSkill = !userSkills.ContainsKey(s.UserId) ? null : userSkills[s.UserId]
                                                               }).FirstOrDefault(),

                            BillUserInfo = request.User != null ? new PlanUserInfoDto
                            {
                                Employee = new UserBaseDto
                                {
                                    PositionId = request.User.PositionId,
                                    PositionName = request.User.Position.ShortName,
                                    PositionColor = request.User.Position.Color,
                                    Branch = request.User.BranchOld,
                                    BranchColor = request.User.Branch.Color,
                                    BranchDisplayName = request.User.Branch.DisplayName,
                                    UserLevel = request.User.UserLevel,
                                    UserType = request.User.UserType,
                                    FullName = request.User.FullName,
                                    EmailAddress = request.User.EmailAddress,
                                    Id = request.BillAccountId ?? default,
                                    AvatarPath = request.User.AvatarPath
                                },
                                PlannedDate = request.BillStartDate ?? null,
                                UserSkill = request.BillAccountId.HasValue &&
                                                                       userSkills.ContainsKey(request.BillAccountId.Value) ? userSkills[request.BillAccountId.Value] : null
                            } : null,

                            BillCVEmail = request.User.EmailAddress,
                            PlanUserEmail = request.ProjectUsers.FirstOrDefault() != null ? request.ProjectUsers.FirstOrDefault().User.EmailAddress : null,
                            Code = request.Code,
                            UserRequestName = _workScope.Get<User>((long)request.CreatorUserId).Name,
                            CreateAt = request.CreationTime,
                            IsNewBillAccount = request.IsNewBillAccount,
                            IsRequiredPlanResource = request.IsRequiredPlanResource
                        };

            return query;
        }

        public async Task RemoveResourceRequestPlan(long requestId)
        {
            var request = await _workScope.GetAll<ResourceRequest>()
               .Where(s => s.Id == requestId)
               .Select(s => new
               {
                   s.Status,
                   PUs = s.ProjectUsers.Where(s => s.Status == ProjectUserStatus.Future && s.AllocatePercentage > 0).ToList()
               }).FirstOrDefaultAsync();

            if (request == default)
            {
                throw new UserFriendlyException("Not found request with Id " + requestId);
            }

            if (request.Status == ResourceRequestStatus.DONE)
            {
                throw new UserFriendlyException("Request already DONE. You can't delete Planned Resource");
            }

            request.PUs.ForEach(p => { p.IsDeleted = true; });

        }
    }
}
