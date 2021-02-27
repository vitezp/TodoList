using AutoMapper;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;
using TodoList.Domain.Enums;
using TodoList.Domain.Extensions;

namespace TodoList.Application.Mappers
{
    public class ResponseToDomainProfile : Profile
    {
        public ResponseToDomainProfile()
        {
            CreateMap<TodoResponse, TodoItem>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ParseEnum<Status>()));
            ;
        }
    }
}