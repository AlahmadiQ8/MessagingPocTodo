using System.Collections.Generic;

namespace PublisherDemo.Models
{
    public class TodoList
    {
        public TodoList()
        {
            TodoItems = new List<TodoItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TodoItem> TodoItems { get; set; }
    }
}