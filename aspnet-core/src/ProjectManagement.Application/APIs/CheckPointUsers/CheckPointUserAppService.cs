using Abp.Application.Services.Dto;
using NccCore.Paging;
using ProjectManagement.APIs.CheckPointUsers.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NccCore.Extension;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Authorization.Users;

namespace ProjectManagement.APIs.CheckPointUsers
{
    public class CheckPointUserAppService : ProjectManagementAppServiceBase
    {
        //paging của trang kết quả đánh giá
        [HttpPost]
        public async Task<GridResult<CheckPointUserDto>> GetAllPagingSub(GridParam input,long phaseId)
        {
            var isPhaseSub = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id == phaseId && x.Type == PhaseType.Sub);
            if (!isPhaseSub)
            {
                throw new UserFriendlyException(String.Format("Không phải phase sub"));
            }
            var result = WorkScope.GetAll<CheckPointUser>()
                         .Include(x => x.Reviewer)
                         .Include(x => x.User)
                         .Where(x => x.PhaseId == phaseId)
                         .Select(x => new CheckPointUserDto
                         {
                             Id = x.Id,
                             ReviewerId = x.ReviewerId,
                             ReviewerName = x.Reviewer.FullName,
                             UserId = x.UserId,
                             UserName = x.User.FullName,
                             Type = x.Type,
                             Score = x.Score,
                             Note = x.Note,
                             Status = x.Status,
                         });

            return await result.GetGridResult(result, input);
        }
        [HttpPost]
        public async Task<GridResult<CheckPointUserDto>> GetAllPagging(GridParam input, long phaseId)
        {

            var result = WorkScope.GetAll<CheckPointUser>()
                         .Include(x => x.Reviewer)
                         .Include(x => x.User)
                         .Where(x => x.PhaseId == phaseId)
                         .Select(x => new CheckPointUserDto
                         {
                             Id = x.Id,
                             ReviewerId = x.ReviewerId,
                             ReviewerName = x.Reviewer.FullName,
                             ReviewerEmail = x.Reviewer.EmailAddress,
                             ReviewerAvatar = x.Reviewer.AvatarPath,
                             UserId = x.UserId,
                             UserName = x.User.FullName,
                             UserEmail = x.User.EmailAddress,
                             UserAvatar = x.User.AvatarPath,
                             Type = x.Type,
                             Status = x.Status,
                         });

            return await result.GetGridResult(result, input);
        }

        public async Task<CheckPointUserDto> Get(long Id)
        {
            return await WorkScope.GetAll<CheckPointUser>()
                         .Include(x => x.Reviewer)
                         .Include(x => x.User)
                         .Where(x => x.Id == Id)
                         .Select(x => new CheckPointUserDto
                         {
                             ReviewerId = x.ReviewerId,
                             ReviewerName = x.Reviewer.FullName,
                             ReviewerEmail = x.Reviewer.EmailAddress,
                             ReviewerAvatar = x.Reviewer.AvatarPath,
                             UserId = x.UserId,
                             UserName = x.User.FullName,
                             UserEmail = x.User.EmailAddress,
                             UserAvatar = x.User.AvatarPath,
                             Type = x.Type,
                             Status = x.Status
                         }).FirstOrDefaultAsync();
        }
        [HttpPost]
        //Set up ai đánh giá ai
        public async Task<CheckPointUserInputDto> Create(CheckPointUserInputDto input, bool isAdmin)
        {
            var isExist = await WorkScope.GetAll<CheckPointUser>().AnyAsync(x => x.ReviewerId == input.ReviewerId && x.UserId == input.UserId && x.PhaseId == input.PhaseId);
            if (isExist)
            {
                throw new UserFriendlyException("This user has been evaluated by this reviewer!");
            }
            if (!isAdmin)
            {
                var checkPointUser = new CheckPointUser
                {
                    UserId = AbpSession.UserId.Value,
                    ReviewerId = input.ReviewerId,
                    Type = CheckPointUserType.Team,
                    Status = CheckPointUserStatus.Draft,
                    PhaseId = input.PhaseId
                };
                await WorkScope.InsertAndGetIdAsync(checkPointUser);
                await GenerateDetail(checkPointUser.Id, input.PhaseId);
                //nếu type là PM tự động tạo bản ghi trong table CheckPointUserResult
                //if (checkPointUser.Type == CheckPointUserType.PM)
                //{
                //    var checkPointResult = new CheckPointUserResult
                //    {
                //        UserId = checkPointUser.UserId.Value,
                //        PMId = checkPointUser.ReviewerId.Value,
                //        Status = CheckPointUserResultStatus.Draft,
                //        PhaseId = checkPointUser.PhaseId
                //    };
                //    await WorkScope.InsertAndGetIdAsync(checkPointResult);
                //}    
            }
            else
            {
                var map = ObjectMapper.Map<CheckPointUser>(input);
                input.Id = await WorkScope.InsertAndGetIdAsync(map);
                //Tạo tiêu chí đánh giá 

                await GenerateDetail(input.Id, input.PhaseId);
            }

            return input;
        }
        [HttpPost]
        //Setup mình đánh giá ai
        public async Task<CheckPointUserInputDto> Update(CheckPointUserInputDto input)
        {
            var checkPointUser = await WorkScope.GetAsync<CheckPointUser>(input.Id);

            if (checkPointUser.Status == CheckPointUserStatus.Reviewed)
            {
                throw new UserFriendlyException("Reviewed. Can not edit!");
            }
            await WorkScope.UpdateAsync(ObjectMapper.Map<CheckPointUserInputDto, CheckPointUser>(input, checkPointUser));

            return input;
        }
        //edit by self
        public async Task EditBySelf(EditBySelfDto input)
        {
            var checkPointUser = await WorkScope.GetAsync<CheckPointUser>(input.CheckPointUserId);

            checkPointUser.Note = input.Note;
            checkPointUser.Score = input.Score;

            await WorkScope.UpdateAsync(checkPointUser);

        }
        public async Task Delete(long Id)
        {
            var checkPointUser = await WorkScope.GetAsync<CheckPointUser>(Id);
            if (checkPointUser.Status != CheckPointUserStatus.Draft)
            {
                throw new UserFriendlyException("Reviewed. Can not delete!");
            }
            if (checkPointUser.CreatorUserId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("Can not delete review of other reviewer!");
            }
            var details = WorkScope.GetAll<CheckPointUserDetail>().Where(x => x.CheckPointUserId == Id);
            foreach (var detail in details)
            {
                await WorkScope.DeleteAsync(detail);
            }

            await WorkScope.DeleteAsync(checkPointUser);

        }
        [HttpGet]
        public async Task GenerateReviewer(long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);
            var userProjects = await (from p in WorkScope.GetAll<ProjectUser>().Include(x => x.Project).Where(x => x.Status == ProjectUserStatus.Present)
                                      group p by new { p.UserId, p.ProjectId } into g
                                      select new
                                      {
                                          UserId = g.Key.UserId,
                                          ProjectId = g.Key.ProjectId
                                      }).ToListAsync();
            var mapPMProjects = await WorkScope.GetAll<Project>()
                                        .Select(x => new
                                        {
                                            ProjectId = x.Id,
                                            PMId = x.PMId
                                        }).ToDictionaryAsync(x => x.ProjectId, x => x.PMId);

            foreach (var item in userProjects)
            {
                bool isExist = await WorkScope.GetAll<CheckPointUser>().AnyAsync(x => x.UserId == item.UserId && x.ReviewerId == mapPMProjects[item.ProjectId]);
                //bỏ Pm đánh giá chính mình
                if (item.UserId != mapPMProjects[item.ProjectId] && !isExist)
                {
                    var checkPointUser = new CheckPointUser
                    {
                        UserId = item.UserId,
                        ReviewerId = mapPMProjects[item.ProjectId],
                        Type = CheckPointUserType.PM,
                        PhaseId = phaseId,
                        Status = CheckPointUserStatus.Draft
                    };
                    await WorkScope.InsertAndGetIdAsync(checkPointUser);
                    await GenerateDetail(checkPointUser.Id, phaseId);

                    //phaseType == Main tự động thêm vào bảng CheckPointUserResult
                    if (phase.Type == PhaseType.Main)
                    {
                        var checkPointResult = new CheckPointUserResult
                        {
                            UserId = checkPointUser.UserId.Value,
                            PMId = checkPointUser.ReviewerId.Value,
                            Status = CheckPointUserResultStatus.Draft,
                            PhaseId = checkPointUser.PhaseId
                        };
                        await WorkScope.InsertAndGetIdAsync(checkPointResult);
                    }

                }

            }
        }


        public async Task<object> GetUserUnreview(long phaseId)
        {
            var userCheckPointIds = await WorkScope.GetAll<CheckPointUser>()
                                        .Include(x => x.User)
                                        .Where(x => x.PhaseId == phaseId)
                                        .Select(x => x.UserId).ToListAsync();
            var userCurrent = await WorkScope.GetAll<User>()
                                        .Where(x => x.IsActive)
                                        .Select(x => new
                                        {
                                            UserId = x.Id,
                                            UserName = x.FullName,

                                        }).ToListAsync();

            var userUnrevieweds = userCurrent.Where(x => !userCheckPointIds.Contains(x.UserId)).ToList();

            return userUnrevieweds;
        }

        public async Task<object> GetAllPhase()
        {
            return await WorkScope.GetAll<Phase>()
                        .Select(x => new
                        {
                            x.Id,
                            x.Name
                        }).ToListAsync();
        }
        [HttpPost]
        public async Task<GridResult<CheckPointUserDto>> GetAllReviewForSelf(GridParam input)
        {
            var result = WorkScope.GetAll<CheckPointUser>()
                                .Where(x => x.UserId == AbpSession.UserId.Value)
                                .Where(x => x.Status == CheckPointUserStatus.Reviewed)
                                .Select(x => new CheckPointUserDto
                                {
                                    ReviewerId = x.ReviewerId,
                                    ReviewerName = x.Reviewer.FullName,
                                    ReviewerEmail = x.Reviewer.EmailAddress,
                                    ReviewerAvatar = x.Reviewer.AvatarPath,
                                    Status = x.Status,
                                    Type = x.Type,
                                    Score = x.Score,
                                    Note = x.Note,
                                    UpdateAt = x.LastModificationTime
                                });

            return await result.GetGridResult(result, input);
        }
        [HttpPost]
        public async Task<GridResult<CheckPointUserDto>> GetAllReviewBySelf(GridParam input)
        {
            var result = WorkScope.GetAll<CheckPointUser>()
                                .Where(x => x.ReviewerId == AbpSession.UserId.Value)
                                .Select(x => new CheckPointUserDto
                                {
                                    Id = x.Id,
                                    UserId = x.UserId,
                                    UserName = x.User.FullName,
                                    UserEmail = x.User.EmailAddress,
                                    UserAvatar = x.User.AvatarPath,
                                    Status = x.Status,
                                    Type = x.Type,
                                    Score = x.Score,
                                    Note = x.Note
                                });

            return await result.GetGridResult(result, input);
        }

        //public async Task<GridResult<CheckPointUserDto>> GetAllReviewResult(GridParam input, long phaseId)
        //{

        //}
        private async Task GenerateDetail(long checkpointUserId, long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);
            var criterias = WorkScope.GetAll<Criteria>();
            if (phase.IsCriteria)
            {
                foreach (var criteria in criterias)
                {
                    var detail = new CheckPointUserDetail
                    {
                        CriteriaId = criteria.Id,
                        CheckPointUserId = checkpointUserId
                    };

                    await WorkScope.InsertAsync(detail);
                }
            }
        }
        //private async Task AutoInsertCheckPointUserResult(CheckPointUserResultInputDto input)
        //{
        //    var checkPointResult = new CheckPointUserResult
        //    {
        //        UserId = input.UserId,
        //        PMId = input.PMId,
        //        Status = CheckPointUserResultStatus.Draft,
        //        PhaseId = input.PhaseId
        //    };
        //    await WorkScope.InsertAndGetIdAsync(checkPointResult);
        //}
    }
}
