
using AIRS.SharedLibrary.Responses;
using AIRS.UserProfileApi.Application.Dtos;
using AIRS.UserProfileApi.Application.Interfaces;
using AIRS.UserProfileApi.Domain.Entities;
using AIRS.UserProfileApi.Infrastructure.Data;
using MongoDB.Driver;


namespace AIRS.UserProfileApi.Infrastructure.Repository;
internal class UserInfoRepository(UserProfileDbContext dbContext) : IUserInfoRepository
{
    private readonly UserProfileDbContext _dbContext = dbContext;


    public async Task AddUser(UserInfo userInfo)
    {
        var _ = new UserInfo
        {
            UserId = userInfo.UserId,
            MbtiType = userInfo.MbtiType
        };
        await _dbContext.UserInfos.InsertOneAsync(userInfo);
    }

    public async Task<Response<UserInfo>> GetUserProfileInfo(int userId)
    {
        var filter = Builders<UserInfo>.Filter.Eq(u => u.UserId, userId);
        var userInfo = await _dbContext.UserInfos.Find(filter).FirstOrDefaultAsync();

        if (userInfo == null)
        {
            return new Response<UserInfo>(false, "User not found");
        }

        return new Response<UserInfo>(true, "User profile retrieved successfully", userInfo);
    }

    public async Task<Response<string>> UpdateUserInfo(int userId, UserProfleUpdateDto userProfleUpdateDto)
    {
        var filter = Builders<UserInfo>.Filter.Eq(u => u.UserId, userId);
        var update = Builders<UserInfo>.Update
            .Set(u => u.Age, userProfleUpdateDto.Age)
            .Set(u => u.City, userProfleUpdateDto.City)
            .Set(u => u.StudyField, userProfleUpdateDto.StudyField)
            .Set(u => u.Income, userProfleUpdateDto.Income)
            .Set(u => u.Hobbies, userProfleUpdateDto.Hobbies)
            .Set(u => u.Interests, userProfleUpdateDto.Interests);

        var result = await _dbContext.UserInfos.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
        {
            return new Response<string>(false, "User not found");
        }

        return new Response<string>(true, "User profile updated successfully");
    }
    public async Task<IEnumerable<UserInfo>> GetAllUsers()
    {
        return await _dbContext.UserInfos.Find(_ => true).ToListAsync();
    }

}
