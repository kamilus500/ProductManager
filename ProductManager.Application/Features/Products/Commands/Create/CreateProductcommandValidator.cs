using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Abstractions.Validation;
namespace ProductManager.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : IValidator<CreateProductCommand>
    {
        public IEnumerable<ValidationFailure> Validate(CreateProductCommand instance)
        {
            if (instance == null)
            {
                yield return new ValidationFailure(string.Empty, "Request is required.");
                yield break;
            }

            if (string.IsNullOrWhiteSpace(instance.Name))
                yield return new ValidationFailure(nameof(instance.Name), "Name is required.");

            if (!string.IsNullOrWhiteSpace(instance.Name) && instance.Name.Length > 200)
                yield return new ValidationFailure(nameof(instance.Name), "Name must be at most 200 characters long.");

            if (instance.Price <= 0)
                yield return new ValidationFailure(nameof(instance.Price), "Price must be greater than zero.");

            if (instance.Quantity < 0)
                yield return new ValidationFailure(nameof(instance.Quantity), "Quantity cannot be negative.");

            if (instance.Description != null && instance.Description.Length > 1000)
                yield return new ValidationFailure(nameof(instance.Description), "Description must be at most 1000 characters long.");
        }
    }
}
