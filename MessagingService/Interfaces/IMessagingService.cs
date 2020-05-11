using System.Threading.Tasks;

namespace MessagingService.Interfaces
{
    public interface IMessagingService<T>
    {
        Task PublishAsync(T payload);
    }
}