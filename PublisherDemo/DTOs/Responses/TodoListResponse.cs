using System.Collections;
using System.Collections.Generic;

namespace PublisherDemo.DTOs.Responses
{
    public class TodoListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TodoItemResponse> TodoItems { get; set; }
    }
}