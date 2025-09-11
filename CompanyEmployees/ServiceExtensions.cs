using Contracts;
using LoggerService;
using CompanyEmployees;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
        => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        });

    public static void ConfigureIisIntegration(this IServiceCollection services)
        => services.Configure<IISOptions>(options => { });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositories(this IServiceCollection services)
        => services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServices(this IServiceCollection services)
        => services.AddScoped<IServiceManager, ServiceManager>();


    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSqlServer<RepositoryContext>(configuration.GetConnectionString("sqlConnection"));
}