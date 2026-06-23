using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.IoC;
using NccCore.Paging;
using NccCore.Uitls;
using Newtonsoft.Json;
using ProjectManagement.Entities;
using ProjectManagement.Manager;
using ProjectManagement.Manager.OffboardUserManager.Dto;
using ProjectManagement.Manager.ProjectAssetManager;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;


namespace ProjectManagement.Manager.OffboardUserManager
{
    public class OffboardUserManager : BaseManager
    {
        private KomuService _komuService;
        public OffboardUserManager(IWorkScope workScope, KomuService komuService) : base(workScope)
        {
            _komuService = komuService;
        }

        public async Task<GridResult<OffboardHistoryDto>> GetAllOffboardHistory(InputGetAllOffboardHistoryDto input)
        {
            var query = WorkScope.GetAll<Entities.OffboardUser>()
                .AsNoTracking()
                .Select(x => new OffboardHistoryDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    EmailAddress = x.User.EmailAddress,
                    AvatarPath = x.User.AvatarPath,
                    UserType = x.User.UserType,
                    Branch = x.User.BranchOld,
                    FullName = x.User.Name + " " + x.User.Surname,
                    BranchColor = x.User.Branch != null ? x.User.Branch.Color : null,
                    BranchDisplayName = x.User.Branch != null ? x.User.Branch.DisplayName : null,
                    PositionId = x.User.PositionId,
                    PositionColor = x.User.Position != null ? x.User.Position.Color : null,
                    PositionName = x.User.Position != null ? x.User.Position.Name : null,
                    UserLevel = x.User.UserLevel,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project != null ? x.Project.Name : null,
                    ProjectType = x.Project != null ? x.Project.ProjectType : 0,
                    ProjectCode = x.Project != null ? x.Project.Code : null,
                    ProjectRole = x.ProjectRole,
                    ProjectPM = x.Project.PM != null ? x.Project.PM.FullName : null,
                    PMEmail = x.Project.PM.EmailAddress != null ? x.Project.PM.EmailAddress : null,
                    PMId = x.Project.PMId,
                    HistoryAsset = x.HistoryAsset,
                    HistoryAccountAsset = x.HistoryAccountAsset,
                    CheckOffboardStatus = x.CheckOffboardStatus,
                    OffboardDate = x.OffboardDate,
                    OffboardStatus = x.OffboardStatus
                });

            if (input.ProjectId.HasValue && input.ProjectId.Value > 0)
            {
                query = query.Where(x => x.ProjectId == input.ProjectId.Value);
            }

            if (input.OffboardStatus.HasValue)
            {
                query = query.Where(x => x.OffboardStatus == input.OffboardStatus.Value);
            }

            if (input.PMId.HasValue && input.PMId.Value > 0)
            {
                query = query.Where(x => x.PMId == input.PMId.Value);
            }

            if (!string.IsNullOrEmpty(input.SearchText))
            {
                query = query.Where(x =>
                    x.EmailAddress.ToLower().Contains(input.SearchText.ToLower()) ||
                    x.FullName.ToLower().Contains(input.SearchText.ToLower()));
            }

            var total = await query.CountAsync();
            var list = await query.TakePage(input).ToListAsync();
            return new GridResult<OffboardHistoryDto>(list, total);
        }

        public async Task<OffboardUser> UpdateOffboardStatus(long offboardHistoryId, bool needOffboard)
        {
            var offboardUser = await WorkScope.All<OffboardUser>()
                .Where(x => x.Id == offboardHistoryId)
                .FirstOrDefaultAsync();

            if (offboardUser == null)
            {
                throw new UserFriendlyException($"Not found OffboardUser with Id {offboardHistoryId}");
            }

            var hasProjectAssets = await WorkScope.GetAll<ProjectUserAsset>()
                .Include(x => x.ProjectAsset)
                .Where(x => x.UserId == offboardUser.UserId && x.ProjectAsset.ProjectId == offboardUser.ProjectId)
                .AnyAsync();

            var hasAccountAssets = await WorkScope.GetAll<AccountAsset>()
                .Include(x => x.ProjectUserBill)
                .Where(x => x.ProjectUserBill.ProjectId == offboardUser.ProjectId && x.ProjectUserBill.UserId == offboardUser.UserId)
                .AnyAsync();

            var hasAnyAssets = hasProjectAssets || hasAccountAssets;

            offboardUser.OffboardStatus = needOffboard ? OffboardStatus.PMOffboard : OffboardStatus.Complete;

            if (!needOffboard)
            {
                offboardUser.CheckOffboardStatus = CheckOffboardStatus.Done;
            }
            if (!hasAnyAssets)
            {
                offboardUser.CheckOffboardStatus = CheckOffboardStatus.PMAccept;
            }

            await WorkScope.UpdateAsync(offboardUser);

            return offboardUser;
        }

        public async Task<List<OffboardChecklistItemDto>> GetOffboardChecklist(long offboardHistoryId)
        {
            var offboard = await WorkScope.GetAll<Entities.OffboardUser>()
                .FirstOrDefaultAsync(x => x.Id == offboardHistoryId);

            if (offboard == null)
                return new List<OffboardChecklistItemDto>();

            var assets = await WorkScope.GetAll<ProjectUserAsset>()
                   .Where(x =>
                       x.UserId == offboard.UserId &&
                       x.ProjectAsset.ProjectId == offboard.ProjectId)
                   .Select(x => new
                   {
                       x.ProjectAssetId,
                       x.ProjectAsset.AssetName,
                       ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                       AccountType = x.ProjectAsset.AccountType.Name,
                       Creator = x.ProjectAsset.AccountAssetCreator.Name,
                       TypeLogin = x.ProjectAsset.TypeLogin.Name
                   })
                   .ToListAsync();

            var accountAssets = await WorkScope.GetAll<AccountAsset>()
                .Where(x =>
                    x.ProjectUserBill.ProjectId == offboard.ProjectId &&
                    x.ProjectUserBill.UserId == offboard.UserId)
                .Select(x => new
                {
                    x.Id,
                    x.ProjectAssetId,
                    x.ProjectAsset.AssetName,
                    ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                    AccountType = x.ProjectAsset.AccountType.Name,
                    Creator = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLogin = x.ProjectAsset.TypeLogin.Name
                })
                .ToListAsync();

            var checkedItems = DeserializeCheckedItems(offboard.OffboardChecklistJson);

            var checklist = assets
                    .Select(x => new OffboardChecklistItemDto
                    {
                        ProjectAssetId = x.ProjectAssetId,
                        AccountAssetId = null,
                        ItemType = "ProjectAsset",
                        AssetName = FormatProjectAssetName(
                            x.AssetName,
                            x.ProjectAssetType,
                            x.AccountType,
                            x.Creator,
                            x.TypeLogin),
                        IsChecked =
                            checkedItems.ProjectAssetIds.Contains(x.ProjectAssetId)
                    })
                    .ToList();

            checklist.AddRange(accountAssets.Select(x =>
                new OffboardChecklistItemDto
                {
                    ProjectAssetId = null,
                    AccountAssetId = x.Id,
                    ItemType = "AccountAsset",
                    AssetName = FormatProjectAssetName(
                        x.AssetName,
                        x.ProjectAssetType,
                        x.AccountType,
                        x.Creator,
                        x.TypeLogin),
                    IsChecked =
                        checkedItems.AccountAssetIds.Contains(x.Id)
                }));

            return checklist;
        }

        public async Task SaveOffboardChecklist(SaveOffboardChecklistDto input)
        {
            var offboard = await WorkScope.GetAll<Entities.OffboardUser>()
                .FirstOrDefaultAsync(x => x.Id == input.OffboardHistoryId);

            if (offboard == null)
                throw new UserFriendlyException("Offboard history not found");

            var assets = await WorkScope.GetAll<ProjectUserAsset>()
                .Include(x => x.ProjectAsset)
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .ToListAsync();

            var accountAssets = await WorkScope.GetAll<AccountAsset>()
                .Include(x => x.ProjectUserBill)
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .ToListAsync();

            var total = assets.Count + accountAssets.Count;
            var checkedAssetCount = input.CheckedProjectAssetIds?.Count ?? 0;
            var checkedAccountAssetCount = input.CheckedAccountAssetIds?.Count ?? 0;
            var checkedCount = checkedAssetCount + checkedAccountAssetCount;

            offboard.OffboardChecklistJson = JsonConvert.SerializeObject(new
            {
                ProjectAssetIds = input.CheckedProjectAssetIds ?? new List<long>(),
                AccountAssetIds = input.CheckedAccountAssetIds ?? new List<long>()
            });

            if (total == 0)
            {
                offboard.CheckOffboardStatus = GetCompletedStatus(offboard.OffboardStatus)
                    ?? CheckOffboardStatus.NotStarted;
            }
            else if (checkedCount == 0)
            {
                offboard.CheckOffboardStatus = CheckOffboardStatus.NotStarted;
            }
            else if (checkedCount < total)
            {
                offboard.CheckOffboardStatus = CheckOffboardStatus.InProgress;
            }
            else
            {
                offboard.CheckOffboardStatus = GetCompletedStatus(offboard.OffboardStatus)
                    ?? CheckOffboardStatus.Done;
            }

            await WorkScope.UpdateAsync(offboard);
        }

        private CheckOffboardStatus? GetCompletedStatus(OffboardStatus offboardStatus)
        {
            return offboardStatus switch
            {
                OffboardStatus.PMOffboard => CheckOffboardStatus.PMAccept,
                OffboardStatus.ITOffboard => CheckOffboardStatus.Done,
                _ => null
            };
        }

        private static (List<long> ProjectAssetIds, List<long> AccountAssetIds) DeserializeCheckedItems(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return (new List<long>(), new List<long>());

            try
            {
                var payload = JsonConvert.DeserializeObject<dynamic>(json);

                if (payload != null && payload.ProjectAssetIds != null && payload.AccountAssetIds != null)
                {
                    return (
                        JsonConvert.DeserializeObject<List<long>>(payload.ProjectAssetIds.ToString()) ?? new List<long>(),
                        JsonConvert.DeserializeObject<List<long>>(payload.AccountAssetIds.ToString()) ?? new List<long>()
                    );
                }
            }
            catch
            {
            }

            try
            {
                return (JsonConvert.DeserializeObject<List<long>>(json) ?? new List<long>(), new List<long>());
            }
            catch
            {
                return (new List<long>(), new List<long>());
            }
        }

        public async Task MoveToComplete(long offboardHistoryId)
        {
            var offboard = await WorkScope.GetAll<Entities.OffboardUser>()
                .FirstOrDefaultAsync(x => x.Id == offboardHistoryId);

            if (offboard == null)
                throw new UserFriendlyException("Offboard history not found");

            offboard.OffboardStatus = OffboardStatus.Complete;
            offboard.OffboardChecklistJson = null;

            var remainAsset = await WorkScope.All<ProjectUserAsset>()
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .Select(x => new
                {
                    x.Id,
                    x.ProjectAssetId,
                    x.ProjectAsset.AssetName,
                    ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                    AccountType = x.ProjectAsset.AccountType.Name,
                    Creator = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLogin = x.ProjectAsset.TypeLogin.Name
                })
                .ToListAsync();

            var remainAccountAssets = await WorkScope.All<AccountAsset>()
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .Select(x => new
                {
                    x.Id,
                    x.ProjectAssetId,
                    x.ProjectAsset.AssetName,
                    ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                    AccountType = x.ProjectAsset.AccountType.Name,
                    Creator = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLogin = x.ProjectAsset.TypeLogin.Name,
                })
                .ToListAsync();

            foreach (var asset in remainAsset)
            {
                await WorkScope.DeleteAsync<ProjectUserAsset>(asset.Id);
            }

            foreach (var accountAsset in remainAccountAssets)
            {
                await WorkScope.DeleteAsync<AccountAsset>(accountAsset.Id);
            }

            await WorkScope.UpdateAsync(offboard);
        }

        public async Task MoveToIT(long offboadHistoryId)
        {
            var offboard = await WorkScope.GetAll<Entities.OffboardUser>()
                .FirstOrDefaultAsync(x => x.Id == offboadHistoryId);

            if (offboard == null)
                throw new UserFriendlyException("Offboard history not found");

            var hasProjectAssets = await WorkScope.GetAll<ProjectUserAsset>()
                .Include(x => x.ProjectAsset)
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .AnyAsync();

            var hasAccountAssets = await WorkScope.GetAll<AccountAsset>()
                .Include(x => x.ProjectUserBill)
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .AnyAsync();

            var hasAnyAssets = hasProjectAssets || hasAccountAssets;

            offboard.OffboardStatus = OffboardStatus.ITOffboard;
            
            if (!hasAnyAssets)
            {
                offboard.CheckOffboardStatus = CheckOffboardStatus.Done;
            }
            else
            {
                offboard.CheckOffboardStatus = CheckOffboardStatus.NotStarted;
            }

            offboard.OffboardChecklistJson = null;

            await WorkScope.UpdateAsync(offboard);

            await SendNoticeMessageToIT(offboadHistoryId);
        }

        public async Task<bool> CheckOffboardHistory(long projectUserId)
        {
            var projectUser = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.Id == projectUserId)
                .Select(s => new
                {
                    s.UserId,
                    s.ProjectId,
                })
                .FirstOrDefaultAsync();

            if (projectUser == null)
            {
                return false;
            }

            return await WorkScope.All<OffboardUser>()
                .AnyAsync(x => x.UserId == projectUser.UserId
                    && x.ProjectId == projectUser.ProjectId
                    && x.OffboardStatus != OffboardStatus.Complete);
        }

        private async Task SendNoticeMessageToIT(long offboardHistoryId)
        {
            var noticeIT = await WorkScope.GetAll<ConfigIT>()
                .Include(x => x.User)
                .ToListAsync();

            var offboard = await WorkScope.GetAll<Entities.OffboardUser>()
                .Include(x => x.User)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == offboardHistoryId);

            if (offboard == null)
                throw new UserFriendlyException("Offboard history not found");

            var projectAssets = await WorkScope.GetAll<ProjectUserAsset>()
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .Select(x => new
                {
                    AssetName = x.ProjectAsset.AssetName,
                    ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                    AccountType = x.ProjectAsset.AccountType.Name,
                    Creator = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLogin = x.ProjectAsset.TypeLogin.Name,
                })
                .ToListAsync();

            var accountAssets = await WorkScope.GetAll<AccountAsset>()
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .Select(x => new
                {
                    AssetName = x.ProjectAsset.AssetName,
                    ProjectAssetType = x.ProjectAsset.ProjectAssetType.Name,
                    AccountType = x.ProjectAsset.AccountType.Name,
                    Creator = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLogin = x.ProjectAsset.TypeLogin.Name,
                })
                .ToListAsync();

            var sb = new StringBuilder();

            sb.AppendLine("📦 **OFFBOARD REQUEST**");
            sb.AppendLine($"Project: **{offboard.Project.Name}**");
            sb.AppendLine($"User: **{offboard.User.FullName}**");
            sb.AppendLine("--------------------------------");

            if (accountAssets.Any())
            {
                sb.AppendLine();
                sb.AppendLine("🔐 **Asset with Bill Account**");

                foreach (var item in accountAssets)
                {
                    sb.AppendLine(
                        $"- {item.AssetName ?? "Unnamed"} " +
                        $"[{item.ProjectAssetType ?? "Unknown project type"}] " +
                        $"[{item.AccountType ?? "Unknown account type"}] " +
                        $"[{item.Creator ?? "Unknown creator"}]" +
                        $"[{item.TypeLogin ?? string.Empty}]"
                    );
                }
            }

            if (projectAssets.Any())
            {
                sb.AppendLine();
                sb.AppendLine("🗂️ **Asset with Project User**");

                foreach (var item in projectAssets)
                {
                    sb.AppendLine(
                        $"- {item.AssetName ?? "Unnamed"} " +
                        $"[{item.ProjectAssetType ?? "Unknown project type"}] " +
                        $"[{item.AccountType ?? "Unknown account type"}] " +
                        $"[{item.Creator ?? "Unknown creator"}]" +
                        $"[{item.TypeLogin ?? string.Empty}]"
                    );
                }
            }

            if (!accountAssets.Any() && !projectAssets.Any())
            {
                sb.AppendLine();
                sb.AppendLine("> No remaining assets.");
            }

            sb.AppendLine();
            sb.AppendLine("--------------------------------");

            sb.AppendLine("🗣️ IT Notice:");
            sb.AppendLine("> Hãy xác nhận đầy đủ các mục trong Offboard Checklist.");

            var messsage = sb.ToString();

            foreach (var it in noticeIT)
            {
                var komuUserName = it.User.UserName?.Split('@')[0];
                await _komuService.NotifyToKomuUserAwait(new KomuMessage
                {
                    UserName = komuUserName,
                    Message = messsage,
                    CreateDate = DateTimeUtils.GetNow()
                });
            }
        }

        private static string FormatProjectAssetName(
            string assetName,
            string assetType,
            string accountType,
            string creator,
            string typeLogin)
        {
            return
                $"{assetName ?? "Unnamed"} " +
                $"[{assetType ?? "Unknown project type"}] " +
                $"[{accountType ?? "Unknown account type"}] " +
                $"[{creator ?? "Unknown creator"}] " +
                $"[{typeLogin ?? "Unknown type login"}]";
        }
    }
}
