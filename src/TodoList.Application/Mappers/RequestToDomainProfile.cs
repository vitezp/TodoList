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
                    opt => opt.MapFrom(src => src.Status.ParseEnum<Status>()));
            CreateMap<UpdateTodoRequest, TodoItem>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ParseEnum<Status>()));
        }
    }
}