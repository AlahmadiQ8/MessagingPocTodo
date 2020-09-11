using System;

namespace TodoSubscriber
{
    public class TodoItemResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime DueDate { get; set; }
    }
}