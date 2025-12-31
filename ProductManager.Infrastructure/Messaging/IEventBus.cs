namespace ProductManager.Infrastructure.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
