using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Forge.Diet.Application.Common.Interfaces;
using Forge.Diet.Infrastructure.Persistence;

namespace Forge.Diet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DietDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDietDbContext>(provider => provider.GetRequiredService<DietDbContext>());

        return services;
    }
}
