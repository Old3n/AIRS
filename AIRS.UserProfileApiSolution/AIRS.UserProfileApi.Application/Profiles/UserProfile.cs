using AIRS.UserProfileApi.Application.Dtos;
using AIRS.UserProfileApi.Domain.Entities;
using AutoMapper;

namespace AIRS.UserProfileApi.Application.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserInfo, UserProfileReadDto>().ReverseMap();
        CreateMap<TestPublishedDto, UserInfo>()
            .ForMember(dest => dest.MbtiType, opt => opt.MapFrom(src => src.Result))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
    }
}
