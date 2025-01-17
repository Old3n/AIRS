using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AutoMapper;
namespace AuthenticationApi.Application.Profiles;
public class UsersProfile : Profile
{
    public UsersProfile()
    {
        // Source -> Target
        CreateMap<AppUser, UserReadDto>();
        CreateMap<UserCreateDto, AppUser>();
        CreateMap<UserReadDto, UserPublishedDto>();
        CreateMap<UserReadDto, UserInfoDto>();
    }
}
