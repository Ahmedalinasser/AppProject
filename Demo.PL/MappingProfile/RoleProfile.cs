using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.MappingProfile
{
    public class RoleProfile : Profile
    {


        public RoleProfile()
        {
            CreateMap<RoleViewModel ,IdentityRole>()
                .ForMember(role => role.Name,option => option.MapFrom(roleVm => roleVm.RoleName) )
                .ReverseMap();

            //role is the destination //then option to get MApFrom // MapFrom roleViewModel Is the source 
            
        }
    }
}
