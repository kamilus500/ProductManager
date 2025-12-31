using ProductManager.MVC.Models;

namespace ProductManager.MVC.Services
{
    public interface IProductApiService
    {
        Task<PageResult<ProductDto>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? sortColumn = "Name",
            string? sortDirection = "asc",
            string? name = null,
            string? description = null,
            bool? isActive = null,
            decimal? minQuantity = null,
            decimal? maxQuantity = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default);

        Task<ProductDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<string> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(DeleteProductRequest request, CancellationToken cancellationToken = default);
    }
}
