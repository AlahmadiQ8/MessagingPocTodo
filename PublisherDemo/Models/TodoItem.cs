using System;

namespace PublisherDemo.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime DueDate { get; set; }
        public TodoList TodoList { get; set; }
        public int TodoListId { get; set; }
    }
}