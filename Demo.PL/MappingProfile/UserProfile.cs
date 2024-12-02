using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfile
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UsersViewModel>().ReverseMap();
        }
    }
}
