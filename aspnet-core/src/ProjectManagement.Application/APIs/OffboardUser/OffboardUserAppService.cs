using Abp.Application.Services;
using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Paging;
using Newtonsoft.Json;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Manager.OffboardUserManager;
using ProjectManagement.Manager.OffboardUserManager.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.OffboardUser
{
    [AbpAuthorize]
    public class OffboardUserAppService : ProjectManagementAppServiceBase
    {
        private readonly OffboardUserManager _offboardUserManager;

        public OffboardUserAppService(OffboardUserManager offboardUserManager)
        {
            _offboardUserManager = offboardUserManager;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<GridResult<OffboardHistoryDto>> GetAllOffboardHistory(InputGetAllOffboardHistoryDto input)
        {
            return await _offboardUserManager.GetAllOffboardHistory(input);
        }

        [HttpPut]
        [AbpAuthorize]
        public async Task UpdateOffboardStatus(UpdateOffboardStatusDto input)
        {
            await _offboardUserManager.UpdateOffboardStatus(input.OffboardHistoryId, input.NeedOffboard);
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<OffboardChecklistItemDto>> GetOffboardChecklist(long offboardHistoryId)
        {
            return await _offboardUserManager.GetOffboardChecklist(offboardHistoryId);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task SaveOffboardChecklist(SaveOffboardChecklistDto input)
        {
            await _offboardUserManager.SaveOffboardChecklist(input);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task MoveToComplete(long offboardHistoryId)
        {
            await _offboardUserManager.MoveToComplete(offboardHistoryId);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task MoveToIT(long offboardHistoryId)
        {
            await _offboardUserManager.MoveToIT(offboardHistoryId);
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<bool> CheckOffboardHistory(long projectUserId)
        {
            return await _offboardUserManager.CheckOffboardHistory(projectUserId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPM()
        {
            var pms = await WorkScope.GetAll<Project>()
                .Where(u => u.PM != null)
                .Select(u => new PMDto
                {
                    Id = u.PMId,
                    EmailAddress = u.PM != null ? u.PM.EmailAddress : null,
                    FullName = u.PM != null ? u.PM.FullName : null,
                    AvatarPath = u.PM != null ? u.PM.AvatarPath : null,
                    UserType = u.PM != null ? u.PM.UserType : 0,
                    UserLevel = u.PM != null ? u.PM.UserLevel : 0,
                    Branch = u.PM.BranchOld,
                })
                .Distinct()
                .ToListAsync();
            return new OkObjectResult(pms);
        }

        [HttpDelete]
        public async Task Delete(long offboardHistoryId)
        {
            var offboard = await WorkScope.GetAsync<Entities.OffboardUser>(offboardHistoryId);
            if (offboard == null)
                throw new UserFriendlyException("Offboard History not exist !");

            await WorkScope.DeleteAsync(offboard);
        }
    }
}

