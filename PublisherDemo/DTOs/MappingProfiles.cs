using AutoMapper;
using PublisherDemo.DTOs.Requests;
using PublisherDemo.DTOs.Responses;
using PublisherDemo.Models;

namespace PublisherDemo.DTOs
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TodoItem, TodoItemResponse>();
            CreateMap<TodoList, TodoListResponse>();
            
            CreateMap<TodoList, TodoListRequest>();
            CreateMap<TodoItem, TodoItemRequest>();
            
            CreateMap<TodoItemRequest, TodoItem>()
                .ForAllMembers(opt => 
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}