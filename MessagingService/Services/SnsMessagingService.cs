using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using MessagingService.Interfaces;
using MessagingService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagingService.Services
{
    public class SnsMessagingService<T> : IMessagingService<T>
    {
        private readonly AmazonSimpleNotificationServiceClient _snsClient;
        private readonly string _topicArn;
        private readonly IOptions<SnsOptions> _messagingOptions;
        private readonly ILogger<SnsMessagingService<T>> _logger;
        private JsonSerializerOptions _serializerOptions;

        public SnsMessagingService(IOptions<SnsOptions> messagingOptions, ILogger<SnsMessagingService<T>> logger)
        {
            _messagingOptions = messagingOptions;
            _logger = logger;
            if (messagingOptions.Value.TopicArn != null)
            {
                _topicArn = messagingOptions.Value.TopicArn;
            } else
            {
                throw new ArgumentException("Either TopicArn or Topic Name must be provided");
            }
            _snsClient = new AmazonSimpleNotificationServiceClient();
            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task PublishAsync(T payload)
        {
            var message = JsonSerializer.Serialize(payload, _serializerOptions);
            await _snsClient.PublishAsync(new PublishRequest
            {
                TopicArn = _topicArn,
                Message = message
            });
            _logger.LogDebug($"Successfully Published to SNS Topic: {_topicArn}");
            _logger.LogDebug($"Payload: {message}");
        }
    }
}