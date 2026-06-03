using Abp.Authorization;
using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NccCore.Paging;
using ProjectManagement.Manager.OffboardUserManager;
using ProjectManagement.Manager.OffboardUserManager.Dto;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagement.Authorization;

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
    }
}
