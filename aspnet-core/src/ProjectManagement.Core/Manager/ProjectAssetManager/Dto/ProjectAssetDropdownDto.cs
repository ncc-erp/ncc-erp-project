using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Manager.ProjectAssetManager.Dto
{
    public class ProjectAssetDropdownDto : EntityDto<long>
    {
        public string AssetName { get; set; }
    }
}