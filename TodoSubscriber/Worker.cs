using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessagingService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TodoSubscriber
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISubscriberService<TodoItemMessage> _todoItemSubscriber;

        public Worker(ILogger<Worker> logger, ISubscriberService<TodoItemMessage> subscriber)
        {
            _logger = logger;
            _todoItemSubscriber = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _todoItemSubscriber.ReceiveMessageAsync(message =>
                {
                    Console.WriteLine("--- INCOMING MESSAGE ---");
                    Console.WriteLine("Processed Message: ");
                    Console.WriteLine($"\tAction:        {message.Action}");
                    Console.WriteLine($"\tId:            {message.TodoItem.Id}");
                    Console.WriteLine($"\tDescription:   {message.TodoItem.Description}\n");
                    return Task.FromResult(true);
                });
            }
        }
    }
}