using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Mapping;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Features.Products.Commands.Update
{
    public record UpdateProductCommand(
        string Id,
        string Name,
        decimal Price,
        int Quantity,
        string Description,
        bool IsActive
    ) : ICommand<ProductDto>;

    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existedProduct = await _productRepository.GetById(request.Id, cancellationToken);

            existedProduct.UpdateName(request.Name);
            existedProduct.Activate(request.IsActive);
            existedProduct.SetQuantity(request.Quantity);
            existedProduct.UpdateDescription(request.Description);

            await _productRepository.UpdateAsync(existedProduct, cancellationToken);

            await _productRepository.SaveChangesAsync(cancellationToken);

            return existedProduct.ToDto();
        }
    }
}
