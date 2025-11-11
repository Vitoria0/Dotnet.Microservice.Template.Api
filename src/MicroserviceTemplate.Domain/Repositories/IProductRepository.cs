using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

