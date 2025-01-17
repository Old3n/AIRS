using AIRS.PersonalityTest.Application.Interfaces;
using AIRS.PersonalityTest.Infrastructure.Data;
using AIRS.PersonalityTest.Infrastructure.Repositories;
using AIRS.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Infrastructure.DependencyInjection;
public static  class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        SharedServiceContainer.AddSharedService<PersonalityTestDbContext>(services, configuration, configuration["MySerilog:FileName"]!);
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IQuestionRepository, QuestionRepository>();
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
