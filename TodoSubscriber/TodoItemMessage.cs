namespace TodoSubscriber
{
    public class TodoItemMessage
    {
        public string Action { get; set; }
        public TodoItemResponse TodoItem { get; set; }
    }
}