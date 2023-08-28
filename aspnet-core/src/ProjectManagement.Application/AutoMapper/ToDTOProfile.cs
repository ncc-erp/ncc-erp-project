using AutoMapper;
using AutoMapper.Configuration.Conventions;
using AutoMapper.Mappers;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceManager.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.AutoMapper
{
    public class ToDTOProfile : Profile
    {
        public ToDTOProfile()
        {
            AddMemberConfiguration().AddMember<NameSplitMember>().AddName<PrePostfixName>(
                    _ => _.AddStrings(p => p.Postfixes, "Dto"));
    
        }
    }
}
