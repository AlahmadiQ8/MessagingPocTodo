using System;
using System.ComponentModel.Design;
using MessagingService.Interfaces;
using MessagingService.Model;
using MessagingService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingService.Bootstrap
{
    public static class MessagingExtension
    {
        public static IServiceCollection AddSnsPublishService<T>(this IServiceCollection services, Action<SnsOptions> options = null)
        {
            services.Configure(options);
            services.AddSingleton<IPublishingService<T>, SnsPublishingService<T>>();
            return services;
        }        
        
        public static IServiceCollection AddSqsSubscriberService<T>(this IServiceCollection services, Action<SqsOptions> options = null)
        {
            services.Configure(options);
            services.AddSingleton<ISubscriberService<T>, SqsProcessorService<T>>();
            return services;
        }
    }
}