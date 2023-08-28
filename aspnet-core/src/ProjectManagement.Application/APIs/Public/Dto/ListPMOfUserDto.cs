using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Public.Dto
{
    public class ListPMOfUserDto
    {
        public string EmailAddress { get; set; }
        public List<PMOfUserDto> ListPMOfUser { get; set; }
    }
}
