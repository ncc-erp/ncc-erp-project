using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Services.ProjectUserBill.Dto;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class GetAllWillPoolResourceDto
    {
        public GetUserInfo Resource { get; set; }
        public string ResourceNote { get; set; }
        public List<AccountDto> Accounts { get; set; }
        public List<ShortInfoProjectDto> Projects {get; set;}
        public int TotalContribute => Accounts.Sum(s => s.Contribute);    }

    public class AccountDto
    {
        public long Id { get; set; }
        public ShortInfoProjectDto Project {get; set;}
        public string ChargeName { get; set; }
        public float HeadCount { get; set; }
        public DateTime? EndChargeDate { get; set; }
        public byte Contribute { get; set; }
    }

    public class ShortInfoProjectDto: EntityDto<long>
    {
        public ProjectType ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
    }
}