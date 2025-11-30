using GishnizApp.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Extentions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        AddRepositories(services);
        return services;
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));
    }
}
