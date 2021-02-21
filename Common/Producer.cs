using System;
using System.Threading.Channels;
using MessagingChannels.Example.Common;
using Microsoft.Extensions.Logging;

namespace MessagingChannels.Example
{
    public class Producer
    {
        private readonly ILogger<Producer> _logger;
        private readonly Channel<object> _channel;

        public Producer(
            ILogger<Producer> logger,
            Channel<object> channel)
        {
            _logger = logger;
            _channel = channel;
        }

        public void Send<T>(Message<T> message)
        {
            if (_channel.Writer.TryWrite(message))
            {
                _logger.LogDebug($"Message {message.Headers[MessageHeaders.CorrelationId]} sent at {DateTime.Now:hh:mm:ss.fff}");
            }
            else
            {
                throw new Exception("Error while sending the message");
            }
        }
    }
}
