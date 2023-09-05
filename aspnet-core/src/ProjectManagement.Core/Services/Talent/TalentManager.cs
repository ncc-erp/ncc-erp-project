using Abp.Domain.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NccCore.IoC;
using ProjectManagement.Entities;
using ProjectManagement.Services.Talent.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.Talent
{
    public class TalentManager : DomainService
    {
        private readonly IWorkScope _workScope;
        private readonly TalentService _talentService;
        public TalentManager(IWorkScope workScope, TalentService talentService)
        {
            _workScope = workScope;
            _talentService = talentService;
        }
        public async Task<RecruitmentTalentResultDto> SendRecruitmentToTalent(RecruitmentTalentDto input)
        {
            var resourceRequest = await _workScope.GetAll<ResourceRequest>()
                .Where(q => q.Id == input.ResourceRequestId)
                .Select(s => new
                {
                    Id = s.Id,
                    Level = s.Level,
                    SkillNames = s.ResourceRequestSkills.Select(s => s.Skill.Name).ToList(),
                    TimeNeed = s.TimeNeed,
                    Priority = s.Priority,
                    Url = s.RecruitmentUrl,
                    Quantity = s.Quantity,
                    IsRecruitmentSend = s.IsRecruitmentSend
                }).FirstOrDefaultAsync();

            if (resourceRequest == null)
                throw new UserFriendlyException("Not Found Resource Request!");

            var recruitment = new SendRecruitmentTalentDto
            {
                BranchId = input.BranchId,
                Level = resourceRequest.Level,
                ResourceRequestId = input.ResourceRequestId,
                SubPositionId = input.SubPositionId,
                Quantity = resourceRequest.Quantity,
                Priority = resourceRequest.Priority,
                SkillNames = resourceRequest.SkillNames,
                TimeNeed = resourceRequest.TimeNeed,
                Note = input.Note,
            };

            var response = await _talentService.SendRecruitmentToTalent(recruitment);
            if (response == default || !response.Success)
                throw new UserFriendlyException("Created Fail Request to Talent");

            var model = await _workScope.GetAsync<ResourceRequest>(input.ResourceRequestId);
            model.IsRecruitmentSend = true;
            model.RecruitmentUrl = response.Result;
            await _workScope.UpdateAsync(model);

            return new RecruitmentTalentResultDto
            {
                IsRecruitmentSend = true,
                PathRecruitment = model.RecruitmentUrl
            };
        }
        public async Task CancelRequest(CloseResourceRequestDto input)
        {
            await _talentService.CancelRequest(input);
        }
        public async Task<List<DropdownPositionDto>> GetPositions()
        {
            return await _talentService.GetPositions();
        }
        public async Task<List<BranchDto>> GetBranches()
        {
            return await _talentService.GetBranches();
        }
    }
}
