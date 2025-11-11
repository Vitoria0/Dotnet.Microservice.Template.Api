using MediatR;
using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MicroserviceTemplate.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product with name: {ProductName}", request.Name);

        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock
        );

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product created with ID: {ProductId}", product.Id);

        return product.Id;
    }
}

