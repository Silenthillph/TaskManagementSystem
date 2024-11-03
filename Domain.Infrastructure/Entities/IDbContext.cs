using Microsoft.EntityFrameworkCore;

namespace Domain.Infrastructure.Entities;

public interface IAppDbContext
{
    DbSet<T> Set<T>() where T : class;
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}