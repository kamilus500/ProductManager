namespace ProductManager.Application.Abstractions.Validation
{
    public record ValidationFailure(string PropertyName, string ErrorMessage);
}
