using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Persistance;

namespace ProductManager.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var productToDelete = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id), cancellationToken);

            if (productToDelete != null)
            {
                productToDelete.MarkAsDelete();
                productToDelete.Activate(false);
                _dbContext.Products.Update(productToDelete);
            }
        }

        public async Task<(IEnumerable<Product>, int)> GetAllAsync(
            int pageNumber, 
            int pageSize,
            string sortColumn, 
            string sortDirection, 
            string? filterName = null,
            string? filterDescription = null,
            decimal? filterMinQuantity = null,
            decimal? filterMaxQuantity = null,
            bool? filterActive = null,
            bool? filterIsDeleted = null,
            decimal? minPrice = null,
            decimal? maxPrice = null, 
            CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            sortColumn = string.IsNullOrWhiteSpace(sortColumn) ? "Name" : sortColumn;
            sortDirection = string.IsNullOrWhiteSpace(sortDirection) ? "asc" : sortDirection.ToLowerInvariant();

            var query = _dbContext.Products
                                    .AsNoTracking()
                                    .AsQueryable();

            var totalCount = query.Count();

            if (!string.IsNullOrWhiteSpace(filterName))
            {
                var fn = filterName.Trim();
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{fn}%"));
            }

            if (!string.IsNullOrWhiteSpace(filterDescription))
            {
                var fd = filterDescription.Trim();
                query = query.Where(p => EF.Functions.Like(p.Description, $"%{fd}%"));
            }

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (filterMinQuantity.HasValue)
                query = query.Where(p => p.StockQuantity >= filterMinQuantity.Value);

            if (filterMaxQuantity.HasValue)
                query = query.Where(p => p.StockQuantity <= filterMaxQuantity.Value);

            if (filterActive.HasValue)
                query = query.Where(p => p.IsActive == filterActive.Value);

            if (filterIsDeleted.HasValue)
                query = query.Where(p => p.IsDeleted == filterIsDeleted.Value);

            var column = sortColumn.Trim().ToLowerInvariant();
            var desc = sortDirection == "desc";

            switch (column)
            {
                case "description":
                    query = desc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                    column = "description";
                    break;
                case "price":
                    query = desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    column = "Price";
                    break;
                case "quantity":
                    query = desc ? query.OrderByDescending(p => p.StockQuantity) : query.OrderBy(p => p.StockQuantity);
                    column = "quantity";
                    break;
                case "isactive":
                    query = desc ? query.OrderByDescending(p => p.IsActive) : query.OrderBy(p => p.IsActive);
                    column = "isActive";
                    break;
                case "isdeleted":
                    query = desc ? query.OrderByDescending(p => p.IsDeleted) : query.OrderBy(p => p.IsDeleted);
                    column = "isDeleted";
                    break;
                case "createdat":
                    query = desc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt);
                    column = "createdat";
                    break;
                default:
                    query = desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    column = "Name";
                    break;
            }

            return (await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken), totalCount);
        }

        public async Task<Product> GetById(string id, CancellationToken cancellationToken = default)
            => await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id), cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            _dbContext.Products.Update(product);
            await Task.CompletedTask;
        }
    }
}
