using Abp.Application.Services;
using NccCore.Extension;
using NccCore.IoC;
using NccCore.Paging;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceRequestService.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var source = query.OrderBy(q => true);
            if (sortParams.ContainsKey("priority"))
                source = OrderResourceRequest(source, "Priority", sortParams["priority"], false);
            if (sortParams.ContainsKey("projectName"))
                source = OrderResourceRequest(source, "projectName", sortParams["projectName"], false);
            if (sortParams.ContainsKey("level"))
                source = OrderResourceRequest(source, "level", sortParams["level"], false);
            if (sortParams.ContainsKey("creationTime"))
                source = OrderResourceRequest(source, "creationTime", sortParams["creationTime"], false);
            if (sortParams.ContainsKey("timeNeed"))
                source = OrderResourceRequest(source, "timeNeed", sortParams["timeNeed"], false);
            return source;
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
