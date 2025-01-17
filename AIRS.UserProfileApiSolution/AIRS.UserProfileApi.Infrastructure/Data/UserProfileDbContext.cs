
using global::AIRS.SharedLibrary.DependencyInjection;
using global::AIRS.UserProfileApi.Domain.Entities;
using MongoDB.Driver;

namespace AIRS.UserProfileApi.Infrastructure.Data
{
    public class UserProfileDbContext : SharedServiceContainer.MongoDbContext
    {
        public UserProfileDbContext(IMongoDatabase database) : base(database)
        {
        }

        public IMongoCollection<UserInfo> UserInfos => GetCollection<UserInfo>("UserInfoDb");
    }
}

