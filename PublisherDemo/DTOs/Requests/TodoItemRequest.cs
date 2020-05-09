using System;
using PublisherDemo.Models;

namespace PublisherDemo.DTOs.Requests
{
    public class TodoItemRequest
    {
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime DueDate { get; set; }
    }
}