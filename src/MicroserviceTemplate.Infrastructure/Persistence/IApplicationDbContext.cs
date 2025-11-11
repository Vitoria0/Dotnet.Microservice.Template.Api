using Microsoft.EntityFrameworkCore;
using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Infrastructure.Persistence;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

