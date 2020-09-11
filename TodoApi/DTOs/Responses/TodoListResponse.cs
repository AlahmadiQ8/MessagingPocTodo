using System.Collections;
using System.Collections.Generic;

namespace TodoApi.DTOs.Responses
{
    public class TodoListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TodoItemResponse> TodoItems { get; set; }
    }
}