using Domain.Infrastructure.Context;
using Domain.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
}