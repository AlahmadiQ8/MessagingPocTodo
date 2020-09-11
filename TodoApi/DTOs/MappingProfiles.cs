using AutoMapper;
using TodoApi.DTOs.Requests;
using TodoApi.DTOs.Responses;
using TodoApi.Models;

namespace TodoApi.DTOs
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