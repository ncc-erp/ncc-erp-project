using System;
using System.Collections.Generic;
using ProjectManagement.Services.ProjectUserBill.Dto;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class GetAllWillPoolResourceDto
    {
        public GetUserInfo Resource { get; set; }
        public List<AccountDto> Accounts { get; set; }
        public int TotalContribute { get; set; }
    }

    public class AccountDto
    {
        public string ChargeName { get; set; }
        public float HeadCount { get; set; }
        public DateTime? EndChargeDate { get; set; }
        public byte Contribute { get; set; }
        public string Note { get; set; }
    }
}