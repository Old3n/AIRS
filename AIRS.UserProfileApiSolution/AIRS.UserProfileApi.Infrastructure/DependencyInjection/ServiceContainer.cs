using AIRS.SharedLibrary.DependencyInjection;
using AIRS.UserProfileApi.Application.Interfaces;
using AIRS.UserProfileApi.Infrastructure.Data;
using AIRS.UserProfileApi.Infrastructure.Messaging;
using AIRS.UserProfileApi.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AIRS.UserProfileApi.Infrastructure.DependencyInjection;
public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SharedServiceContainer.AddMongoDbContext<UserProfileDbContext>(services, configuration, configuration["MySerilog:FileName"]!);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        // rabbit mq dependency injection
        services.AddHostedService<TestResultSubscriber>();


        return services;
    }

    public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
    {
        // Registering middleware 
        // refistering global exception handler
        // Adding gateway configuration(rate limitier etc)
        SharedServiceContainer.UseSharedPolicies(app);
        return app;
    }
}

