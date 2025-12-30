using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Mapping;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Features.Products.Queries.GetAll
{
    public record GetAllProductsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? SortColumn = "Name",
        string? SortDirection = "asc",
        string? Name = null,
        string? Description = null,
        bool? IsActive = null,
        decimal? MinQuantity = null,
        decimal? MaxQuantity = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null)
    : IRequest<PageResult<ProductDto>>;

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PageResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }
        public async Task<PageResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _productRepository.GetAllAsync(
                                request.PageNumber,
                                request.PageSize,
                                request.SortColumn ?? "Name",
                                request.SortDirection ?? "asc",
                                request.Name,
                                request.Description,
                                request.MinQuantity,
                                request.MaxQuantity,
                                request.IsActive,
                                request.MinPrice,
                                request.MaxPrice,
                                cancellationToken);

            var productDtos = products.ToDto();

            return new PageResult<ProductDto>
            {
                Items = productDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortColumn = request.SortColumn,
                SortDirection = request.SortDirection,
                TotalCount = totalCount
            };
        }
    }
}
