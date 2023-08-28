using Microsoft.AspNetCore.Mvc;
using ProjectManagement.APIs.CheckPointUserDetails.Dto;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Abp.UI;
using System;

namespace ProjectManagement.APIs.CheckPointUserDetails
{
    public class CheckPointUserDetailAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        public async Task<object> GetAll(long phaseId, long memberId)
        {
            var checkPointUser = await WorkScope.GetAll<CheckPointUser>().Where(x => x.PhaseId == phaseId && x.ReviewerId == memberId && x.UserId == memberId).FirstOrDefaultAsync();
            var phase = await WorkScope.GetAsync<Phase>(checkPointUser.PhaseId);
            var checkPointUserId = checkPointUser.Id;

            var listDetail = WorkScope.GetAll<CheckPointUserDetail>().Where(x => x.CheckPointUserId == checkPointUserId).Select(x => new
            {
                Id = x.Id,
                CriteriaId = x.CriteriaId,
                Note = x.Note,
                CheckPointUserId = x.CheckPointUserId,
                Score = x.Score,
            }).ToList();

            if (listDetail == null)
            {
                throw new UserFriendlyException(String.Format("Chưa được đánh giá"));
            }

            if (phase.Type == PhaseType.Main)
            {
                return new
                {
                    checkPointUser.Id,
                    checkPointUser.Note,
                    checkPointUser.Score,
                    listDetail
                };
            }
            else
            {
                return new
                {
                    checkPointUser.Id,
                    checkPointUser.Note,
                    checkPointUser.Score,
                };
            }
        }
        [HttpGet]
        public async Task<List<CheckPointUserDetailBeforeDto>> GetAllBefore(long phaseId, long memberId)
        {
            var checkPointUser = WorkScope.GetAll<CheckPointUser>()
                .Where(x => x.PhaseId == phaseId && x.ReviewerId == memberId && x.UserId == memberId)
                .FirstOrDefault();

            var parentPhase = await WorkScope.GetAsync<Phase>(checkPointUser.Id);
            var phases = WorkScope.GetAll<Phase>().Where(x => x.Type == PhaseType.Main && x.Status == PhaseStatus.Done);
            if (parentPhase.Index != 0)
            {
                phases.Where(x => x.Index < parentPhase.Index);
            }
            var phaseBefore = phases.OrderBy(x => x.Index).LastOrDefault();

            var query = from cpud in WorkScope.GetAll<CheckPointUserDetail>()
                        join cpu in WorkScope.GetAll<CheckPointUser>() on cpud.CheckPointUserId equals cpu.Id
                        where cpu.UserId == checkPointUser.UserId && cpu.PhaseId == phaseBefore.Id
                        && AbpSession.UserId == checkPointUser.UserId ? cpu.ReviewerId == checkPointUser.UserId : true && cpu.IsPublic == true
                        select new CheckPointUserDetailBeforeDto
                        {
                            Id = cpud.Id,
                            CriteriaId = cpud.CriteriaId,
                            Score = cpu.Score,
                        };
            return await query.ToListAsync();
        }
        [HttpPut]
        public async Task Update(InputCriteriaDto input)
        {
            //var isMain = await WorkScope.GetAsync<Phase>(input.Id);
            var criterias = WorkScope.GetAll<Criteria>().ToList();
            var weightSum = criterias.Sum(x => x.Weight);

            var checkPointUserId = WorkScope.GetAll<CheckPointUser>().FirstOrDefault(x => x.UserId == input.MemberId && x.PhaseId == input.PhaseId && x.ReviewerId == input.MemberId).Id;

            var score = 0;
            foreach (var cpudNew in input.CheckPointUserDetails)
            {
                var cpudOld = await WorkScope.GetAsync<CheckPointUserDetail>(cpudNew.Id);

                var weight = criterias.Where(x => x.Id == cpudNew.CriteriaId).Select(x => x.Weight).FirstOrDefault();
                if (cpudNew.Score != null)
                    score += (cpudNew.Score * weight).Value;
                //if (!string.IsNullOrEmpty(cpudNew.Note))
                await WorkScope.UpdateAsync(ObjectMapper.Map<CheckPointUserDetailDto,CheckPointUserDetail>(cpudNew,cpudOld));
            }

            //tính điểm để nhập vào checkpointuser
            var checkpointuser = await WorkScope.GetAsync<CheckPointUser>(input.Id);
            checkpointuser.Note = input.Note;
            checkpointuser.Score = (int)score / weightSum;
            checkpointuser.Status = CheckPointUserStatus.Reviewed;

            await WorkScope.UpdateAsync<CheckPointUser>(checkpointuser);

            var cpuTypeTeam = WorkScope.GetAll<CheckPointUser>().Where(x => x.PhaseId == checkpointuser.PhaseId && x.UserId == checkpointuser.UserId && x.Type == CheckPointUserType.Team);

            //cập nhật điểm vào checkpointuserresult
            var cpuResult = WorkScope.GetAll<CheckPointUserResult>().Where(x => x.PhaseId == checkpointuser.PhaseId && x.UserId == checkpointuser.UserId).FirstOrDefault();
            if (checkpointuser.Type == CheckPointUserType.PM)
            {
                cpuResult.PMId = checkpointuser.ReviewerId.Value;
                cpuResult.PMNote = checkpointuser.Note;
                cpuResult.PMScore = checkpointuser.Score;
            }
            else if (checkpointuser.Type == CheckPointUserType.Self)
            {
                cpuResult.UserNote = checkpointuser.Note;
                cpuResult.SelfScore = checkpointuser.Score;
            }
            else if (checkpointuser.Type == CheckPointUserType.Exam)
            {
                cpuResult.ExamScore = checkpointuser.Score;
            }
            else if (checkpointuser.Type == CheckPointUserType.Client)
            {
                cpuResult.ClientScore = checkpointuser.Score;
            }
            else
            {
                var scoreTeam = cpuTypeTeam.Average(x => x.Score);
                cpuResult.TeamScore = checkpointuser.Score;
            }

            await WorkScope.UpdateAsync<CheckPointUserResult>(cpuResult);

            //đánh giá bản thân mới được requestlevel
            if (AbpSession.UserId == checkpointuser.UserId)
            {
                await RequestLevel(input.RequestLevel, cpuResult.PhaseId);
            }
        }
        public async Task RequestLevel(UserLevel requestLevel, long PhaseId)
        {
            var cpur = WorkScope.GetAll<CheckPointUserResult>().Where(x => x.PhaseId == PhaseId && x.UserId == AbpSession.UserId).FirstOrDefault();
            cpur.NewLevel = requestLevel;
            await WorkScope.UpdateAsync<CheckPointUserResult>(cpur);
        }
        [HttpGet]
        public async Task<List<HistoryDto>> ShowHistory(long checkPointUserDetailId)
        {
            var checkpointuserdetail = await WorkScope.GetAsync<CheckPointUserDetail>(checkPointUserDetailId);

            var query = from cpud in WorkScope.GetAll<CheckPointUserDetail>()
                        join cpu in WorkScope.GetAll<CheckPointUser>() on cpud.CheckPointUserId equals cpu.Id
                        where cpud.CheckPointUserId == checkpointuserdetail.CheckPointUserId && cpud.CriteriaId == checkpointuserdetail.CriteriaId
                        && cpu.IsPublic == true && cpu.Status == CheckPointUserStatus.Reviewed
                        select new HistoryDto
                        {
                            Id = cpud.Id,
                            PhaseName = cpu.Phase.Name,
                            Reviewer = cpu.Reviewer.FullName,
                            Note = cpud.Note,
                            Score = cpud.Score,
                            Type = cpu.Type,
                        };
            return await query.OrderBy(x => x.PhaseName).ToListAsync();
        }
    }
}
