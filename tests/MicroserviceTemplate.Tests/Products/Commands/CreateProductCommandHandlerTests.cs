using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MicroserviceTemplate.Application.Products.Commands.CreateProduct;
using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Domain.Repositories;

namespace MicroserviceTemplate.Tests.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateProductCommandHandler>> _loggerMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateProductCommandHandler>>();
        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsProductId()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            10
        );

        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken ct) => p);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesProductWithCorrectValues()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            10
        );

        Product? capturedProduct = null;
        _productRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken ct) =>
            {
                capturedProduct = p;
                return p;
            });

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedProduct);
        Assert.Equal(command.Name, capturedProduct.Name);
        Assert.Equal(command.Description, capturedProduct.Description);
        Assert.Equal(command.Price, capturedProduct.Price);
        Assert.Equal(command.Stock, capturedProduct.Stock);
    }
}

