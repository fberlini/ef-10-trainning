using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniBlog.Api.Data;

public static class DependencyInitialization
{
    public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        
        services.AddDbContext<MiniBlogDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("SqlServerDbContext"), x => x.MigrationsAssembly("MiniBlog.Api.Data"))
        );

        return services;
    }
}