using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Implementations.Mediator;
using ProductManager.Infrastructure.Persistance;
using ProductManager.Infrastructure.Repositories;

namespace ProductManager.Infrastructure
{
    public static class Extensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString") ?? throw new ArgumentNullException("Connection string is empty");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IMediator, Mediator>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineBehavior<,>));

            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static IServiceCollection AddSwaggerInfra(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ProductManager API",
                    Version = "v1",
                    Description = "API ProductManager"
                });
            });

            return services;
        }
    }
}
