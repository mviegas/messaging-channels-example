using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using MessagingChannels.Example.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessagingChannels.Example
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly Channel<object> _channel;

        public Consumer(ILogger<Consumer> logger, Channel<object> channel)
        {
            _logger = logger;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Consumer started");

                while (await _channel.Reader.WaitToReadAsync(stoppingToken))
                {
                    if (_channel.Reader.TryRead(out object message))
                    {
                        if (message is Message<string> typedMessage)
                        {
                            _logger.LogDebug($"Message {typedMessage.Headers[MessageHeaders.CorrelationId]} received at {DateTime.Now:f}");
                        }
                    }
                }
            }

            _logger.LogInformation("Consumer stopped");
        }
    }
}
