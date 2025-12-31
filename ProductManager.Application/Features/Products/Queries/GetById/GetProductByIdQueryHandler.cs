using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Mapping;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Features.Products.Queries.GetById
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductDto>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                throw new ArgumentNullException(nameof(request.Id));

            var product = await _productRepository.GetById(request.Id, cancellationToken);

            if (product == null)
                throw new KeyNotFoundException($"Product with id {request.Id} not found.");

            return product.ToDto();
        }
    }
}
