using ProductManager.Application.Abstractions.Mediator;
using ProductManager.Application.Abstractions.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : IValidator<UpdateProductCommand>
    {
        public IEnumerable<ValidationFailure> Validate(UpdateProductCommand instance)
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

            if (string.IsNullOrWhiteSpace(instance.Name))
                yield return new ValidationFailure(nameof(instance.Name), "Name is required.");

            if (instance.Price <= 0)
                yield return new ValidationFailure(nameof(instance.Price), "Price must be greater than zero.");

            if (instance.Quantity < 0)
                yield return new ValidationFailure(nameof(instance.Quantity), "Quantity cannot be negative.");

            if (instance.Description != null && instance.Description.Length > 1000)
                yield return new ValidationFailure(nameof(instance.Description), "Description must be at most 1000 characters long.");
        }
    }
}
