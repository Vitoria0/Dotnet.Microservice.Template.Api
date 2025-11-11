using Xunit;
using FluentValidation.TestHelper;
using MicroserviceTemplate.Application.Products.Commands.CreateProduct;

namespace MicroserviceTemplate.Tests.Products.Commands;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            10
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_ShouldFail()
    {
        // Arrange
        var command = new CreateProductCommand(
            string.Empty,
            "Test Description",
            99.99m,
            10
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_PriceZero_ShouldFail()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            0m,
            10
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_NegativeStock_ShouldFail()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            "Test Description",
            99.99m,
            -1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Stock);
    }
}

