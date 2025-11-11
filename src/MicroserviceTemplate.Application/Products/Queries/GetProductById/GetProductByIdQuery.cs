using MediatR;
using MicroserviceTemplate.Application.Products.Dtos;

namespace MicroserviceTemplate.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

