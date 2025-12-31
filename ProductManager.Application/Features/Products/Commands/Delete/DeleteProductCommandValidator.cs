using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Abstractions.Validation;

namespace ProductManager.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandValidator : IValidator<DeleteProductCommand>
    {
        public IEnumerable<ValidationFailure> Validate(DeleteProductCommand instance)
        {
            if (instance == null)
            {
                yield return new ValidationFailure(string.Empty, "Request is required.");
                yield break;
            }

            if (string.IsNullOrWhiteSpace(instance.Id))
                yield return new ValidationFailure(nameof(instance.Id), "Id is required.");
            else if (!Guid.TryParse(instance.Id, out _))
                yield return new ValidationFailure(nameof(instance.Id), "Id must be a valid GUID.");
        }
    }
}
