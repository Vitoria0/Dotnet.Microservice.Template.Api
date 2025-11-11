using Microsoft.EntityFrameworkCore;
using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateTimestamp();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdateTimestamp();
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

