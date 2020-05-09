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
        public static IServiceCollection AddMessagingService<T>(this IServiceCollection services, Action<MessagingOptions> options = null)
        {
            services.Configure<MessagingOptions>(options);
            services.AddSingleton<IMessagingService<T>, SnsMessagingService<T>>();
            return services;
        }
    }
}