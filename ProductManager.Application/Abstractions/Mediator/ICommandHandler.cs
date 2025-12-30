namespace ProductManager.Application.Abstractions.Mediator
{
    public interface ICommandHandler<TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    { }

    public interface ICommand<TResponse> : IRequest<TResponse> { }
}
