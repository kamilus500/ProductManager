using ProductManager.Application.Abstractions.Mediator;

namespace ProductManager.Application.Abstractions.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            var validatorsType = typeof(IEnumerable<IValidator<TRequest>>);
            var validatorsObj = _serviceProvider.GetService(validatorsType) as IEnumerable<IValidator<TRequest>>;
            var validators = validatorsObj?.ToList() ?? new List<IValidator<TRequest>>();

            var failures = new List<ValidationFailure>();

            foreach (var validator in validators)
            {
                var result = validator.Validate(request);
                if (result != null)
                    failures.AddRange(result);
            }

            if (failures.Any())
            {
                var dict = failures
                    .GroupBy(f => string.IsNullOrWhiteSpace(f.PropertyName) ? "_" : f.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray(), StringComparer.OrdinalIgnoreCase);

                throw new ValidationException(dict);
            }

            return next();
        }
    }
}
