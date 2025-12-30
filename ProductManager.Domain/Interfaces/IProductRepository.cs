using ProductManager.Domain.Entities;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product>, int)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string sortColumn,
            string sortDirection,
            string? filterName = null,
            string? filterDescription = null,
            decimal? filterMinQuantity = null,
            decimal? filterMaxQuantity = null,
            bool? filterActive = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default!);
        Task<Product> GetById(string id, CancellationToken cancellationToken = default!);
        Task AddAsync(Product product, CancellationToken cancellationToken = default!);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default!);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default!);

        Task SaveChangesAsync(CancellationToken cancellationToken = default!);
    }
}
