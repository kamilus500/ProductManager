using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Features.Products.Commands.Create
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        bool Active
    ): ICommand<string>;

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, string>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product(request.Name, request.Price, request.Quantity, request.Active, request.Description);

            await _productRepository.AddAsync(newProduct, cancellationToken);

            await _productRepository.SaveChangesAsync(cancellationToken);

            return newProduct.Id.ToString();
        }
    }
}
