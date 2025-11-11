using MediatR;
using MicroserviceTemplate.Application.Products.Dtos;

namespace MicroserviceTemplate.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;

