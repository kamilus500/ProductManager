using ProductManager.MVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ProductManager.MVC.Services
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PageResult<ProductDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? sortColumn = "Name", string? sortDirection = "asc", string? name = null, string? description = null, bool? isActive = null, bool? isDeleted = null, decimal? minQuantity = null, decimal? maxQuantity = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken cancellationToken = default)
        {
            var query = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}",
                $"sortColumn={Uri.EscapeDataString(sortColumn ?? string.Empty)}",
                $"sortDirection={Uri.EscapeDataString(sortDirection ?? string.Empty)}"
            };

            if (!string.IsNullOrEmpty(name)) query.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrEmpty(description)) query.Add($"description={Uri.EscapeDataString(description)}");
            if (isActive.HasValue) query.Add($"isActive={isActive.Value.ToString().ToLowerInvariant()}");
            if (isDeleted.HasValue) query.Add($"isDeleted={isDeleted.Value.ToString().ToLowerInvariant()}");
            if (minQuantity.HasValue) query.Add($"minQuantity={minQuantity.Value}");
            if (maxQuantity.HasValue) query.Add($"maxQuantity={maxQuantity.Value}");
            if (minPrice.HasValue) query.Add($"minPrice={minPrice.Value}");
            if (maxPrice.HasValue) query.Add($"maxPrice={maxPrice.Value}");

            var url = $"/getall?{string.Join("&", query)}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<PageResult<ProductDto>>(stream, _jsonOptions, cancellationToken)
                         ?? new PageResult<ProductDto>();

            return result;
        }

        public async Task<string> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("/create", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return body.Trim('"');
        }

        public async Task UpdateAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PutAsync("/update", content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(DeleteProductRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/delete")
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<ProductDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            var url = $"/api/Product/{Uri.EscapeDataString(id)}";

            using var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var dto = await JsonSerializer.DeserializeAsync<ProductDto>(stream, _jsonOptions, cancellationToken);

            return dto;
        }
    }
}
