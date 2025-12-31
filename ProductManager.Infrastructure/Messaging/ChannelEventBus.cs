using System.Threading.Channels;

namespace ProductManager.Infrastructure.Messaging
{
    public class ChannelEventBus : IEventBus
    {
        private readonly Channel<IntegrationEvent> _channel;

        public ChannelEventBus(Channel<IntegrationEvent> channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(@event, cancellationToken).ConfigureAwait(false);
        }
    }
}
