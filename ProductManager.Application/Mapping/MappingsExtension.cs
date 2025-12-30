using ProductManager.Application.Features.Products;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Mapping
{
    public static class MappingsExtension
    {
        public static ProductDto ToDto(this Product product)
            => new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                Price = product.Price,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                IsDeleted = product.IsDeleted
            };

        public static List<ProductDto> ToDto(this IEnumerable<Product> products)
            => products.Select(p => p.ToDto()).ToList();
    }
}
