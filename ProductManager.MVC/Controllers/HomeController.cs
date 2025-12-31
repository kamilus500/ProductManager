
using Microsoft.AspNetCore.Mvc;
using ProductManager.MVC.Models;
using ProductManager.MVC.Services;
using System.Diagnostics;

namespace ProductManager.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductApiService _productApiService;

        public HomeController(IProductApiService productApiService)
        {
            _productApiService = productApiService ?? throw new ArgumentNullException(nameof(productApiService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(
                    int pageNumber = 1,
                    int pageSize = 5,
                    string? sortColumn = "Name",
                    string? sortDirection = "asc",
                    string? name = null,
                    string? description = null,
                    bool? isActive = null,
                    bool? isDeleted = null,
                    decimal? minQuantity = null,
                    decimal? maxQuantity = null,
                    decimal? minPrice = null,
                    decimal? maxPrice = null,
                    CancellationToken cancellationToken = default)
        {
            var page = await _productApiService.GetAllAsync(
                pageNumber,
                pageSize,
                sortColumn,
                sortDirection,
                name,
                description,
                isActive,
                isDeleted,
                minQuantity,
                maxQuantity,
                minPrice,
                maxPrice,
                cancellationToken);

            return View(page);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest createProduct, CancellationToken cancellationToken = default)
        {
            await _productApiService.CreateAsync(createProduct, cancellationToken);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var product = await _productApiService.GetByIdAsync(id, cancellationToken);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(DeleteProductRequest request, CancellationToken cancellationToken = default)
        {
            await _productApiService.DeleteAsync(request, cancellationToken);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken = default)
        {
            var product = await _productApiService.GetByIdAsync(id, cancellationToken);
            if (product == null)
                return NotFound();

            var model = new UpdateProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.StockQuantity,
                IsActive = product.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            await _productApiService.UpdateAsync(request, cancellationToken);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
