using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Phases.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Phases
{
    [AbpAuthorize]
    public class PhaseAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        public async Task<GridResult<PhaseDto>> GetAllPaging(GridParam input)
        {
            var query = from p in WorkScope.GetAll<Phase>()
                        join pt in WorkScope.GetAll<Phase>()
                        on p.ParentId equals pt.Id
                        select new PhaseDto()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Year = p.Year,
                            Type = p.Type,
                            ParentId = pt.Id,
                            Status = p.Status,
                            IsCriteria = p.IsCriteria,
                            Index = p.Index,
                        };
            return await query.GetGridResult(query, input);
        }
        [HttpGet]
        public async Task<object> GetAllParent(int year)
        {
            var query = WorkScope.GetAll<Phase>().Where(x => x.Year == year && x.Type == PhaseType.Main)
                .Select(x => new
                {
                    ParentId = x.Id,
                    ParentName = x.Name,
                });
            return await query.ToListAsync();
        }
        [HttpGet]
        public async Task<object> GetAllPhase(int year)
        {
            var query = WorkScope.GetAll<Phase>().Where(x => x.Year == year)
                 .Select(x => new
                 {
                     PhaseId = x.Id,
                     PhaseName = x.Name,
                     Status = x.Status,
                     Type = x.Type,
                 });
            return await query.ToListAsync();
        }
        [HttpPost]
        public async Task<PhaseDto> Create(PhaseDto input)
        {
            var isExist = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Name == input.Name && x.Year == input.Year && x.Type == input.Type);

            if (isExist)
                throw new UserFriendlyException(String.Format("Phase already exist"));

            if (input.Type == PhaseType.Sub)
            {
                var isParentExist = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id == input.ParentId && x.Type == PhaseType.Main);
                if (!isParentExist)
                    throw new UserFriendlyException(String.Format("Parent not exist"));

                var parent = WorkScope.GetAsync<Phase>(input.ParentId);
                input.Year = parent.Result.Year;
            }

            long phaseid = await WorkScope.InsertAndGetIdAsync<Phase>(ObjectMapper.Map<Phase>(input));
            var phase = await WorkScope.GetAsync<Phase>(phaseid);
            if (input.Type == PhaseType.Main)
            {
                phase.ParentId = phase.Id;
                await WorkScope.UpdateAsync<Phase>(phase);
            }

            return input;
        }
        [HttpPut]
        public async Task<PhaseDto> Update(PhaseDto input)
        {
            var phase = await WorkScope.GetAsync<Phase>(input.Id);

            var isExist = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name && x.Year == input.Year);
            if (isExist)
                throw new UserFriendlyException(String.Format("Name already exist"));

            if (input.Type == PhaseType.Main)
                input.ParentId = input.Id;

            if (input.Type == PhaseType.Sub)
            {
                var isParentExist = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Id == input.ParentId && x.Type == PhaseType.Main);
                if (!isParentExist)
                    throw new UserFriendlyException(String.Format("Parent not exist"));

                var parent = WorkScope.GetAsync<Phase>(input.ParentId);
                input.Year = parent.Result.Year;
            }

            await WorkScope.UpdateAsync(ObjectMapper.Map<PhaseDto, Phase>(input, phase));

            return input;
        }
        [HttpDelete]
        public async Task Delete(long phaseId)
        {
            var phase = WorkScope.GetAsync<Phase>(phaseId);

            if (phase.Result.Type == PhaseType.Main)
            {
                var phasechilds = WorkScope.GetAll<Phase>().Where(x => x.ParentId == phaseId);
                var isExist = await WorkScope.GetAll<CheckPointUser>().AnyAsync(x => phasechilds.Select(x => x.Id).Contains(x.PhaseId) && x.Status == CheckPointUserStatus.Draft);

                if (!isExist)
                    foreach (var item in phasechilds)
                    {
                        await WorkScope.DeleteAsync<Phase>(item.Id);
                    }
            }
            else
            {
                await WorkScope.DeleteAsync<Phase>(phaseId);
            }

        }
        [HttpPut]
        public async Task Active(long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);
            var isExistActive = await WorkScope.GetAll<Phase>().AnyAsync(x => x.Status == PhaseStatus.Active && x.Year==phase.Year);

            if (isExistActive)
            {
                throw new UserFriendlyException(String.Format("Đã có phase active trước đó nên ko thể active"));
            }

            if (phase.Status == PhaseStatus.Done)
            {
                throw new UserFriendlyException(String.Format("Đợt này đã xong rồi, không thể active nữa"));
            }

            phase.Status = PhaseStatus.Active;
            await WorkScope.UpdateAsync<Phase>(phase);
        }
        [HttpPut]
        public async Task DeActive(long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);

            if (phase.Status == PhaseStatus.Done)
            {
                throw new UserFriendlyException(String.Format("Đợt này đã xong rồi, không thể deactive nữa"));
            }

            phase.Status = PhaseStatus.DeActive;
            await WorkScope.UpdateAsync<Phase>(phase);
        }
        [HttpPut]
        public async Task Done(long phaseId)
        {
            var phase = await WorkScope.GetAsync<Phase>(phaseId);

            if (phase.Status != PhaseStatus.Done)
            {
                phase.Status = PhaseStatus.Done;

                var maxIndex = WorkScope.GetAll<Phase>().Where(x => x.ParentId == phase.ParentId).Max(x => x.Index);
                phase.Index = maxIndex + 1;

                await WorkScope.UpdateAsync<Phase>(phase);
            }
        }
    }
}
