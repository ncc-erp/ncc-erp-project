using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.ProjectAssetManager.Dto
{
    public class ProjectAssetDto : EntityDto<long>
    {
        public string AssetName { get; set; }
    }
}
