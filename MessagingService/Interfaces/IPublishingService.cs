using System.Threading.Tasks;

namespace MessagingService.Interfaces
{
    public interface IPublishingService<T>
    {
        Task PublishAsync(T payload);
    }
}