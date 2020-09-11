using System;
using System.Threading.Tasks;

namespace MessagingService.Interfaces
{
    public interface ISubscriberService<T>
    {
        Task ReceiveMessageAsync(Func<T, Task<bool>> handler);
    }
}