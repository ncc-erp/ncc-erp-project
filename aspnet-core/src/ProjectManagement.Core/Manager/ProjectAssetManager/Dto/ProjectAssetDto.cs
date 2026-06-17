using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.ProjectAssetManager.Dto
{
    public class ProjectAssetDto : EntityDto<long>
    {
        public long ProjectAssetTypeId { get; set; }
        public string ProjectAssetTypeName { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public long AccountAssetCreatorId { get; set; }
        public string AccountAssetCreatorName { get; set; }
        public long TypeLoginId { get; set; }
        public string TypeLoginName { get; set; }
        public string AssetName { get; set; }
    }
}
