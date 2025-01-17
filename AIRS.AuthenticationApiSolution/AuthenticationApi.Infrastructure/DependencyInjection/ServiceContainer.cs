using AIRS.SharedLibrary.DependencyInjection;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Messaging;
using AuthenticationApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection;
public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SharedServiceContainer.AddSharedService<AuthenticationDbContext>(services, configuration, configuration["MySerilog:FileName"]!);
        services.AddScoped<IUser, UserRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // rabbit mq dependency injection
        services.AddSingleton<IMessageBus, MessageBusClient>();
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
