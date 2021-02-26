using AutoMapper;
using TodoList.Data.Models;
using TodoList.Shared.Contract.Responses;
using TodoList.Shared.Domain;
using TodoList.Shared.Extensions;

namespace TodoList.Api.Mappers
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