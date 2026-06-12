using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.IoC;
using NccCore.Paging;
using Newtonsoft.Json;
using ProjectManagement.Entities;
using ProjectManagement.Manager;
using ProjectManagement.Manager.OffboardUserManager.Dto;
using ProjectManagement.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Manager.ProjectAssetManager;

namespace ProjectManagement.Manager.OffboardUserManager
{
    public class OffboardUserManager : BaseManager
    {
        public OffboardUserManager(IWorkScope workScope) : base(workScope)
        {
        }

        public async Task<GridResult<OffboardHistoryDto>> GetAllOffboardHistory(InputGetAllOffboardHistoryDto input)
        {
            IQueryable<OffboardUser> baseQuery = WorkScope.All<OffboardUser>()
                .Include(x => x.User)
                    .ThenInclude(u => u.Branch)
                .Include(x => x.User)
                    .ThenInclude(u => u.Position)
                .Include(x => x.Project)
                    .ThenInclude(x => x.PM);

            var query = baseQuery.Select(x => new OffboardHistoryDto
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
                ProjectPM = x.Project.PM.FullName != null ? x.Project.PM.FullName : null,
                PMEmail = x.Project.PM.EmailAddress != null ? x.Project.PM.EmailAddress : null,
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

            if (!string.IsNullOrEmpty(input.SearchText))
            {
                query = query.Where(x =>
                    x.EmailAddress.ToLower().Contains(input.SearchText.ToLower()) ||
                    x.FullName.ToLower().Contains(input.SearchText.ToLower()));
            }

            var list = await query.TakePage(input).ToListAsync();
            var total = await query.CountAsync();
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
                .Include(x => x.ProjectAsset)
                    .ThenInclude(x => x.ProjectAssetType)
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .ToListAsync();

            var accountAssets = await WorkScope.GetAll<AccountAsset>()
                .Include(x => x.AccountType)
                .Include(x => x.AccountAssetCreator)
                .Include(x => x.ProjectUserBill)
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .ToListAsync();

            var checkedItems = DeserializeCheckedItems(offboard.OffboardChecklistJson);

            var checklist = assets
                .Select(x => new OffboardChecklistItemDto
                {
                    ProjectAssetId = x.ProjectAssetId,
                    AccountAssetId = null,
                    ItemType = "ProjectAsset",
                    AssetName = !string.IsNullOrWhiteSpace(x.ProjectAsset?.AssetName)
                        ? x.ProjectAsset.AssetName : string.Empty,
                    IsChecked = checkedItems.ProjectAssetIds.Contains(x.ProjectAssetId)
                })
                .ToList();

            checklist.AddRange(accountAssets.Select(x => new OffboardChecklistItemDto
            {
                ProjectAssetId = null,
                AccountAssetId = x.Id,
                ItemType = "AccountAsset",
                AssetName = $"{x.AssetName ?? "Unnamed"} [{x.AccountAssetCreator?.Name ?? "Unknown creator"}] [{x.AccountType?.Name ?? "Unknown type"}] [{x.TypeLogin ?? string.Empty}]",
                IsChecked = checkedItems.AccountAssetIds.Contains(x.Id)
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
                .Include(x => x.ProjectAsset)
                    .ThenInclude(x => x.ProjectAssetType)
                .Where(x => x.UserId == offboard.UserId && x.ProjectAsset.ProjectId == offboard.ProjectId)
                .ToListAsync();

            var remainAccountAssets = await WorkScope.All<AccountAsset>()
                .Include(x => x.AccountType)
                .Include(x => x.AccountAssetCreator)
                .Include(x => x.ProjectUserBill)
                .Where(x => x.ProjectUserBill.ProjectId == offboard.ProjectId && x.ProjectUserBill.UserId == offboard.UserId)
                .ToListAsync();

            var assetHistory = JsonConvert.SerializeObject(remainAsset.Select(x => new
            {
                x.ProjectAssetId,
                AssetName = !string.IsNullOrWhiteSpace(x.ProjectAsset?.AssetName)
                    ? x.ProjectAsset.AssetName
                    : x.ProjectAsset?.ProjectAssetType != null
                        ? x.ProjectAsset.ProjectAssetType.Name
                        : string.Empty
            }));

            var accountAssetHistory = JsonConvert.SerializeObject(remainAccountAssets.Select(x => new
            {
                x.Id,
                Value = $"{x.AssetName ?? "Unnamed"} [{x.AccountAssetCreator?.Name ?? "Unknown creator"}] [{x.AccountType?.Name ?? "Unknown type"}] [{x.TypeLogin ?? string.Empty}]"
            }));

            offboard.HistoryAsset = assetHistory;
            offboard.HistoryAccountAsset = accountAssetHistory;

            foreach (var asset in remainAsset)
            {
                await WorkScope.DeleteAsync(asset);
            }

            foreach (var accountResource in remainAccountAssets)
            {
                await WorkScope.DeleteAsync(accountResource);
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
    }
}
