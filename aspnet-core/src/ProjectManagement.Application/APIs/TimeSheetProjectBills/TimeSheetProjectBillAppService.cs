using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.ProjectUserBills.Dto;
using ProjectManagement.APIs.TimeSheetProjectBills.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimeSheetProjectBills
{
    [AbpAuthorize]
    public class TimeSheetProjectBillAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize]
        public async Task<List<GetTimeSheetProjectBillDto>> GetAll(long timesheetId, long projectId)
        {
            var isViewRate = PermissionChecker.IsGranted(PermissionNames.Timesheets_TimesheetDetail_ViewBillRate);

            var query = WorkScope.GetAll<TimesheetProjectBill>()
                .Where(x => x.TimesheetId == timesheetId && x.ProjectId == projectId)
                .OrderBy(x => x.User.EmailAddress)
                         .Select(x => new GetTimeSheetProjectBillDto
                         {
                             Id = x.Id,
                             UserId = x.UserId,
                             UserName = x.User.Name,
                             ProjectId = x.ProjectId,
                             ProjectName = x.Project.Name,
                             AccountName = x.AccountName,
                             BillRole = x.BillRole,
                             BillRate = isViewRate ? x.BillRate : -1,
                             StartTime = x.StartTime.Date,
                             EndTime = x.EndTime.Value.Date,
                             Note = x.Note,
                             ShadowNote = x.ShadowNote,
                             IsActive = x.IsActive,
                             AvatarPath = x.User.AvatarPath,
                             FullName = x.User.FullName,
                             Branch = x.User.BranchOld,
                             BranchColor = x.User.Branch.Color,
                             BranchDisplayName = x.User.Branch.DisplayName,
                             PositionId = x.User.PositionId,
                             PositionName = x.User.Position.ShortName,
                             PositionColor = x.User.Position.Color,
                             EmailAddress = x.User.EmailAddress,
                             UserType = x.User.UserType,
                             WorkingTime = x.WorkingTime,
                             UserLevel = x.User.UserLevel,
                             Currency = x.CurrencyId == null ? x.Project.Currency.Name : x.Currency.Name,
                             ChargeType = x.ChargeType == null ? x.Project.ChargeType : x.ChargeType,
                             //ProjectBillInfomation = $"<b>{x.User.FullName}</b> - {x.BillRole} - {x.BillRate} - {x.Note} - {x.ShadowNote} <br>"
                         });
            return await query.ToListAsync();
        }

        private void checkValidData(List<TimeSheetProjectBillDto> input)
        {
            if (input.IsEmpty())
            {
                throw new UserFriendlyException("List empty");
            }
            foreach (var item in input)
            {
                checkValidInput(item);
            }
            if (input.Count == 1)
            {
                return;
            }

            var isDuplicate = (from i in input
                               group i by i.UserId into g
                               select new
                               {
                                   UserId = g.Key,
                                   Count = g.Count()
                               }).Any(s => s.Count > 1);


            if (isDuplicate)
            {
                throw new UserFriendlyException($"Duplicated user bill!");
            }

        }

        private void checkValidInput(TimeSheetProjectBillDto dto)
        {
            if (string.IsNullOrEmpty(dto.BillRole))
            {
                throw new UserFriendlyException($"BillRole can't be null or empty. Id {dto.Id}");
            }
        }

        [HttpPut]
        public async Task<List<TimeSheetProjectBillDto>> Update(List<TimeSheetProjectBillDto> input)
        {
            checkValidData(input);
            var ids = input.Select(s => s.Id);
            var entities = await WorkScope.GetAll<TimesheetProjectBill>()
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var item in entities)
            {
                var dto = input.Where(s => s.Id == item.Id).FirstOrDefault();
                if (dto != default)
                {
                    item.AccountName = dto.AccountName;
                    item.BillRate = dto.BillRate;
                    item.BillRole = dto.BillRole;
                    item.Note = dto.Note;
                    item.WorkingTime = dto.WorkingTime;
                    item.IsActive = dto.IsActive;
                    item.ChargeType = dto.ChargeType;
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            return input;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTSOfPM(List<UpdateTimesheetPMDto> input)
        {
            var ids = input.Select(x => x.Id).ToList();
            var entities = await WorkScope.GetAll<TimesheetProjectBill>()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            foreach (var entity in entities)
            {
                var dto = input.Where(s => s.Id == entity.Id).FirstOrDefault();
                if (dto != default)
                {
                    entity.WorkingTime = dto.WorkingTime;
                    entity.Note = dto.Note;
                    entity.IsActive = dto.IsActive;
                    entity.AccountName = dto.AccountName;
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return new OkObjectResult("Save Success!");
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_RemoveAccount)]
        public async Task RemoveAccountTS(long id)
        {
            await WorkScope.DeleteAsync<TimesheetProjectBill>(id);
        }

        public async Task<List<GetUserForTimesheetProjectBillDto>> GetUserForTimesheetProjectBill(long timesheetId, long projectId, bool isEdited)
        {
            var currentUserIds = await WorkScope.GetAll<TimesheetProjectBill>()
                .Where(x => x.TimesheetId == timesheetId && x.ProjectId == projectId)
                .Select(x => x.UserId).ToListAsync();

            var users = WorkScope.GetAll<User>()
                                .Where(x => !isEdited ? !currentUserIds.Contains(x.Id) : true)
                                .Select(x => new GetUserForTimesheetProjectBillDto
                                {
                                    UserId = x.Id,
                                    FullName = x.FullName,
                                    Email = x.EmailAddress
                                }).ToList();
            return users;
        }
    }
}
