using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Services.Talent;
using ProjectManagement.Services.Talent.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Talent
{
    [AbpAuthorize]
    public class TalentAppService : ProjectManagementAppServiceBase
    {
        private readonly TalentManager _talentManager;
        public TalentAppService(TalentManager talentManager)
        {
            _talentManager = talentManager;
        }
        [HttpPost]
        public async Task<IActionResult> SendRecruitmentToTalent(RecruitmentTalentDto input)
        {
            var result = await _talentManager.SendRecruitmentToTalent(input);
            return new OkObjectResult(result);
        }
        [HttpGet]
        public async Task<List<DropdownPositionDto>> GetPositions()
        {
            return await _talentManager.GetPositions();
        }
        [HttpGet]
        public async Task<List<BranchDto>> GetBranches()
        {
            return await _talentManager.GetBranches();
        }
    }
}
