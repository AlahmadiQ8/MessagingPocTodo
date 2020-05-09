namespace MessagingService.Interfaces
{
    public interface IMessagingService<T>
    {
        void Publish(T payload);
    }
}