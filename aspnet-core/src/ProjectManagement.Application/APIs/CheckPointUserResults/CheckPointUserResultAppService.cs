using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CheckPointUserResults.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.RequestLevel
{
    public class CheckPointUserResultAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        public async Task<GridResult<CheckPointUserResultDto>> GetAllPagingMain(GridParam input,long phaseId)
        {
            var isPhaseMain = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id == phaseId && x.Type == PhaseType.Main);
            var cput = WorkScope.GetAll<CheckPointUserResultTag>()
                .Select(x => new ResultTagDto
                {
                    Id=x.Id,
                    TagId=x.TagId,
                    TagName = x.Tag.Name,
                    CheckPointUserResultId=x.CheckPointUserResultId,
                });
            if (!isPhaseMain)
            {
                throw new UserFriendlyException(String.Format("Không phải phase main"));
            }
            var resultMain = from cpur in WorkScope.GetAll<CheckPointUserResult>()
                             where cpur.PhaseId == phaseId
                             select new CheckPointUserResultDto
                             {
                                 Id = cpur.Id,
                                 PhaseId = cpur.PhaseId,
                                 UserId = cpur.User.Id,
                                 UserName = cpur.User.FullName,
                                 ReviewerId = cpur.PM.Id,
                                 ReviewerName = cpur.PM.FullName,
                                 UserNote = cpur.UserNote,
                                 PMNote = cpur.PMNote,
                                 FinalNote = cpur.FinalNote,
                                 CurrentLevel = cpur.User.UserLevel,
                                 ExpectedLevel = cpur.User.UserLevel,
                                 NowLevel = cpur.NewLevel,
                                 PMScore = cpur.PMScore.Value,
                                 TeamScore = cpur.TeamScore.Value,
                                 ClientScore = cpur.ClientScore.Value,
                                 ExamScore = cpur.ExamScore.Value,
                                 Status = cpur.Status,
                                 Tags = cput.Where(x=>x.CheckPointUserResultId==cpur.Id).ToList(),
                             };
            return await resultMain.GetGridResult(resultMain, input);
        }
        [HttpGet]
        public async Task<object> GetAll(long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);
            if (phase.Type == PhaseType.Sub)
            {
                var resultSub = WorkScope.GetAll<CheckPointUser>()
                    .Where(x => x.PhaseId == phaseId)
                    .Select(x => new
                    {
                        Id = x.Id,
                        PhaseId = x.Phase.Name,
                        Note = x.Note,
                        ReviewerName = x.Reviewer.FullName,
                        ReviewerId = x.ReviewerId,
                        Score = x.Score.Value,
                        Status = x.Status,
                        Type = x.Type,
                        UserName = x.User.Name,
                        UserId = x.UserId
                    });
                return await resultSub.ToListAsync();
            }

            var cput = from a in WorkScope.GetAll<CheckPointUserResultTag>()
                       join b in WorkScope.GetAll<Tag>() on a.TagId equals b.Id
                       select new
                       {
                           Id = a.Id,
                           CPURId = a.CheckPointUserResultId,
                           TagId = b.Id,
                           TagName = b.Name,
                       };

            var resultMain = from cpur in WorkScope.GetAll<CheckPointUserResult>()
                             where cpur.PhaseId == phaseId
                             select new
                             {
                                 Id = cpur.Id,
                                 PhaseId = cpur.PhaseId,
                                 UserId = cpur.User.Id,
                                 UserName = cpur.User.FullName,
                                 ReviewerId = cpur.PM.Id,
                                 ReviewerName = cpur.PM.FullName,
                                 UserNote = cpur.UserNote,
                                 PMNote = cpur.PMNote,
                                 FinalNote = cpur.FinalNote,
                                 CurrentLevel = cpur.User.UserLevel,
                                 ExpectedLevel = cpur.User.UserLevel,
                                 NowLevel = cpur.NewLevel,
                                 PMScore = cpur.PMScore.Value,
                                 TeamScore = cpur.TeamScore.Value,
                                 ClientScore = cpur.ClientScore.Value,
                                 ExamScore = cpur.ExamScore.Value,
                                 Status = cpur.Status,
                                 Tag = cput.Where(x => x.CPURId == cpur.Id).ToList(),
                             };
            return await resultMain.ToListAsync();
        }
        [HttpPut]
        public async Task EditMain(EditMainDto input)
        {
            var checkPointUserResultId = input.CheckPointUserResultId;
            var tagIds = input.TagIds;
            //edit final note với userlever now
            var checkPointUserResult = await WorkScope.GetAsync<CheckPointUserResult>(checkPointUserResultId);
            checkPointUserResult.FinalNote = input.FinalNote;
            checkPointUserResult.NewLevel = input.Now;
            await WorkScope.UpdateAsync<CheckPointUserResult>(checkPointUserResult);

            //edit tags
            var oldTags = WorkScope.GetAll<CheckPointUserResultTag>().Where(x => x.CheckPointUserResultId == checkPointUserResultId).ToList();

            var oldTagIds = oldTags.Select(x => x.TagId);
            var insertTags = tagIds.Select(x => x).Except(oldTagIds);
            var deleteTags = oldTagIds.Except(tagIds.Select(x => x));
            var deleteTagIds = oldTags.Where(x => deleteTags.Contains(x.TagId));

            foreach (var item in insertTags)
            {
                ResultTagDto temp = new ResultTagDto() { Id = 0, TagId = item, CheckPointUserResultId = checkPointUserResultId };
                await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<CheckPointUserResultTag>(temp));
            }

            foreach (var item in deleteTagIds)
            {
                await WorkScope.DeleteAsync<CheckPointUserResultTag>(item);
            }

            var cpuResult = await WorkScope.GetAsync<CheckPointUserResult>(checkPointUserResultId);

            cpuResult.Status = CheckPointUserResultStatus.FinalDone;
            await WorkScope.UpdateAsync<CheckPointUserResult>(cpuResult);
        }
        [HttpGet]
        public async Task<object> GetAllTagNotSelect(long checkPointUserResultId)
        {
            var tagIdSelects = WorkScope.GetAll<CheckPointUserResultTag>()
            .Where(x => x.CheckPointUserResultId == checkPointUserResultId)
            .Select(x => x.TagId).ToList();
            var tagIdNotSelects = WorkScope.GetAll<Tag>().Where(x => !tagIdSelects.Contains(x.Id));

            return tagIdNotSelects.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }
        [HttpGet]
        public async Task<object> ShowShortDetails(long checkPointUserResultId)
        {
            var cpur = await WorkScope.GetAsync<CheckPointUserResult>(checkPointUserResultId);
            var cpus = WorkScope.GetAll<CheckPointUser>().Where(x => x.PhaseId == cpur.PhaseId && x.UserId == cpur.UserId)
                .Select(x => new
                {
                    Reviewer = x.Reviewer.FullName,
                    PhaseType = x.Phase.Type,
                    Note = x.Note,
                }).ToList();
            return cpus;
        }
        [HttpPost]
        public async Task Done(long checkPointUserResultId)
        {
            var checkPointUserResult = await WorkScope.GetAsync<CheckPointUserResult>(checkPointUserResultId);
            var isPhaseMain = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id == checkPointUserResult.PhaseId && x.Type == PhaseType.Main);
            if (!isPhaseMain)
                throw new UserFriendlyException(String.Format("Không phải đợt main"));

            if (checkPointUserResult.Status == CheckPointUserResultStatus.FinalDone)
                throw new UserFriendlyException(String.Format("Đã final done rồi"));

            checkPointUserResult.Status = CheckPointUserResultStatus.FinalDone;
            await WorkScope.UpdateAsync<CheckPointUserResult>(checkPointUserResult);
        }
    }
}
