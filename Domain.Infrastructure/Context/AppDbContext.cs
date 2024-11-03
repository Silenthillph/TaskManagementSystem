using Domain.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Infrastructure.Context;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskEntity> Tasks { get; set; }
    
    public async Task<int> SaveAsync(CancellationToken ct = default)
    {
        return await this.SaveChangesAsync(ct);
    }
}