using TodoApi.DTOs.Responses;

namespace TodoApi.Models
{
    public class TodoItemNotification
    {
        public ItemAction Action { get; set; }
        public TodoItemResponse TodoItem { get; set; }
    }
    
    public enum ItemAction
    {
        Created,
        Updated,
        Deleted
    }
}