
using AIRS.SharedLibrary.Responses;
using AIRS.UserProfileApi.Application.Dtos;
using AIRS.UserProfileApi.Domain.Entities;

namespace AIRS.UserProfileApi.Application.Interfaces;
public interface IUserInfoRepository
{
    Task AddUser(UserInfo userInfo);
    Task<Response<UserInfo>> GetUserProfileInfo(int userId);
    Task<Response<string>> UpdateUserInfo(int userId, UserProfleUpdateDto userProfleUpdateDto);
    Task<IEnumerable<UserInfo>> GetAllUsers();
}
