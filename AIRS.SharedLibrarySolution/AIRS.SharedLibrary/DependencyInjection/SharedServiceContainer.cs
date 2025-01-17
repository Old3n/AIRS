using AIRS.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;

namespace AIRS.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>(this IServiceCollection services, IConfiguration configuration, string fileName) where TContext : DbContext
        {
            // Add Generic DbContext
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AIRSConnection"), sqlServerOption => sqlServerOption.EnableRetryOnFailure());
            });
            //serilog config 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Adding JWT Authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);
            return services;
        }

        public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, string fileName) where TContext : MongoDbContext
        {
            // MongoDB Configuration
            var connectionString = configuration.GetConnectionString("MongoDBConnection");
            var databaseName = configuration["MongoDBSettings:DatabaseName"];
            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("MongoDB connection string or database name is missing in configuration.");
            }

            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            // Register the DbContext
            services.AddScoped<TContext>();

            // Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.log",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Adding JWT Authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);

            return services;
        }


        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //using global exceptions 
            app.UseMiddleware<GlobalException>();
            //refistering middle ware for api calls
            app.UseMiddleware<GatewayConfiguration>();
            return app;
        }

        public abstract class MongoDbContext
        {
            protected readonly IMongoDatabase Database;

            protected MongoDbContext(IMongoDatabase database)
            {
                Database = database;
            }

            protected IMongoCollection<T> GetCollection<T>(string collectionName)
            {
                return Database.GetCollection<T>(collectionName);
            }
        }

    }
}
