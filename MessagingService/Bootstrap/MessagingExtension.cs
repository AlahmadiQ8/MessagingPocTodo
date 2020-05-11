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
            services.AddSingleton<IMessagingService<T>, SnsMessagingService<T>>();
            return services;
        }
    }
}