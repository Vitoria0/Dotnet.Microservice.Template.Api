using MediatR;

namespace MicroserviceTemplate.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;

