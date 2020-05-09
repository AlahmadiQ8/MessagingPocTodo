using System;
using MessagingService.Interfaces;
using MessagingService.Model;
using Microsoft.Extensions.Options;

namespace MessagingService.Services
{
    public class SnsMessagingService<T> : IMessagingService<T>
    {
        private readonly IOptions<MessagingOptions> _messagingOptions;

        public SnsMessagingService(IOptions<MessagingOptions> messagingOptions1)
        {
            _messagingOptions = messagingOptions1;
        }

        public void Publish(T payload)
        {
            Console.WriteLine($"Published on {_messagingOptions.Value.TopicArn}");
        }
    }
}