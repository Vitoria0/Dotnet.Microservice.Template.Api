using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MicroserviceTemplate.Application.Products.Queries.GetProductById;
using MicroserviceTemplate.Application.Products.Dtos;
using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Domain.Repositories;

namespace MicroserviceTemplate.Tests.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<GetProductByIdQueryHandler>> _loggerMock;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetProductByIdQueryHandler>>();
        _handler = new GetProductByIdQueryHandler(
            _productRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ProductExists_ReturnsProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Test Product", "Test Description", 99.99m, 10);
        
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(product.Stock, result.Stock);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}

