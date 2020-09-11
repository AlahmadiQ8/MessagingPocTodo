using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using MessagingService.Interfaces;
using MessagingService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessagingService.Services
{
    public class SqsProcessorService<T> : ISubscriberService<T>
    {
        private readonly AmazonSQSClient _sqs;
        private readonly ILogger<SqsProcessorService<T>> _logger;
        private readonly string _queueUrl;

        public SqsProcessorService(ILogger<SqsProcessorService<T>> logger, IOptions<SqsOptions> sqsOptions)
        {
            _logger = logger;

            if (sqsOptions.Value.QueueUrl == null)
            {
                throw new ArgumentException("QueueUrl is required");
            }

            _queueUrl = sqsOptions.Value.QueueUrl;
            _sqs = new AmazonSQSClient();
        }

        public async Task ReceiveMessageAsync(Func<T, Task<bool>> handler)
        {
            var messageResponse = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                WaitTimeSeconds = 20
            });
            if (messageResponse.Messages.Any())
            {
                var processedMessages = await ProcessMessages(messageResponse.Messages, handler);
                await DeleteProcessedMessages(processedMessages);
            }
            else
            {
                _logger.LogDebug("No Messages in the queue");
            }
        }

        private async Task<List<(string, string)>> ProcessMessages(IList<Message> messages, Func<T, Task<bool>> handler)
        {
            var processedMessages = new List<(string, string)>();
            var id = 0;
            foreach (var message in messages)
            {
                var snsMessage = Amazon.SimpleNotificationService.Util.Message.ParseMessage(message.Body);

                var processedSuccessfully = await handler(JsonSerializer.Deserialize<T>(snsMessage.MessageText));
                if (processedSuccessfully)
                {
                    processedMessages.Add((id.ToString(), message.ReceiptHandle));
                    id++;
                }
            }

            return processedMessages;
        }

        private async Task DeleteProcessedMessages(List<(string, string)> processedMessages)
        {
            if (!processedMessages.Any())
                return;

            var entries = new List<DeleteMessageBatchRequestEntry>();
            foreach (var (id, receiptHandle) in processedMessages)
            {
                entries.Add(new DeleteMessageBatchRequestEntry(id, receiptHandle));
            }

            var deleteRequest = new DeleteMessageBatchRequest {QueueUrl = _queueUrl, Entries = entries};

            var response = await _sqs.DeleteMessageBatchAsync(deleteRequest);
            if (response.Failed.Any())
            {
                _logger.LogError("Some messages failed to be deleted");
                foreach (var failed in response.Failed)
                {
                    _logger.LogError(failed.Message);
                }
            }
            _logger.LogDebug("Deleted processed Messages");
        }
    }
}