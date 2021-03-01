using AutoMapper;
using TodoList.Domain.Contract.Requests;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Domain.Extensions;

namespace TodoList.Application.Mappers
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<TodoRequest, TodoItem>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.Status) ? Status.NotStarted : src.Status.ParseEnum<Status>()))
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.Priority) ? 0 : int.Parse(src.Priority)));  

            CreateMap<UpdateTodoRequest, TodoItem>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ParseEnum<Status>()));
        }
    }
}