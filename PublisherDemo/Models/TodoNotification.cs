using PublisherDemo.DTOs.Responses;

namespace PublisherDemo.Models
{
    public class TodoNotification
    {
        public Action Action { get; set; }
        public TodoItemResponse TodoItem { get; set; }
    }
    
    public enum Action
    {
        Created,
        Updated,
        Deleted
    }
}