

namespace ProductManager.Application.Abstractions.Validation
{
    public interface IValidator<T>
    {
        IEnumerable<ValidationFailure> Validate(T instance);
    }
}
