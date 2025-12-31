using Microsoft.Extensions.DependencyInjection;
using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Abstractions.Validation;
using ProductManager.Application.Features.Products.Commands.Create;
using ProductManager.Application.Features.Products.Commands.Update;
using ProductManager.Domain.Interfaces;
using System.Reflection;

namespace ProductManager.Application
{
    public static class Extensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var handlers = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Select(i => new { Implementation = t, Interface = i }))
                .Where(x => x.Interface.IsGenericType &&
                            (x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                             x.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
                .ToList();

            foreach (var h in handlers)
            {
                services.AddScoped(h.Interface, h.Implementation);
            }

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
            services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        }
    }
}
