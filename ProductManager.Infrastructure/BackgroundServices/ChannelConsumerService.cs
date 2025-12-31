using Microsoft.Extensions.Hosting;
using ProductManager.Infrastructure.Messaging;
using System.Threading.Channels;

namespace ProductManager.Infrastructure.BackgroundServices
{
    public class ChannelConsumerService : BackgroundService
    {
        private readonly Channel<IntegrationEvent> _channel;
        private readonly IServiceProvider _provider;

        public ChannelConsumerService(Channel<IntegrationEvent> channel, IServiceProvider provider)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var reader = _channel.Reader;

            while (await reader.WaitToReadAsync(stoppingToken))
            {
                while (reader.TryRead(out var @event))
                {
                    //Implementacja logiki przetwarzania zdarzenia
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}
