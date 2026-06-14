using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Creators.Dto
{
    [AutoMapTo(typeof(AccountAssetCreator))]
    public class AccountAssetCreatorDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
    }
}
