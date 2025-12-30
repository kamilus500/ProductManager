using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Features.Products.Commands.Delete
{
    public record DeleteProductCommand(
        string Id
    ) : ICommand<bool>;

    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                throw new ArgumentNullException(nameof(request.Id));

            await _productRepository.DeleteAsync(request.Id, cancellationToken);

            await _productRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
