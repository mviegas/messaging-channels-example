using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MessagingChannels.Example.Common
{
    public class Message<T>
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>
        {
            { MessageHeaders.CorrelationId, Activity.Current.Id },
            { MessageHeaders.MessageId, Guid.NewGuid().ToString() }
        };

        public T Body { get; set; }
    }
}