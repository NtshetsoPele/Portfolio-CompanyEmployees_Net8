using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors((CorsOptions options) =>
        {
            options.AddPolicy("CorsPolicy", (CorsPolicyBuilder builder) =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    public static void ConfigureIisIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>((IISOptions options) =>
        {
        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>((DbContextOptionsBuilder opts) =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    // From .NET 6 RC2
    //public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
    //    services.AddSqlServer<RepositoryContext>((configuration.GetConnectionString("sqlConnection")));

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();
}