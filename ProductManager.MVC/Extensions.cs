using ProductManager.MVC.Services;

namespace ProductManager.MVC
{
    public static class Extensions
    {
        public static IServiceCollection AddApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ProductApi:BaseUrl"] ?? throw new ArgumentNullException("ProductApi:BaseUrl configuration is missing");
            services.AddHttpClient<IProductApiService, ProductApiService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}
