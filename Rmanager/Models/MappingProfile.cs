using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Rmanager.Dto;
namespace Rmanager.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<RmanagerUser, UserDetailDto>();
            CreateMap<UserDetailDto, RmanagerUser>();
        }
    }
}
