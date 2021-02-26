using AutoMapper;
using TodoList.Data.Models;
using TodoList.Shared.Contract.Responses;
using TodoList.Shared.Domain;
using TodoList.Shared.Extensions;

namespace TodoList.Api.Mappers
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<TodoItem, TodoResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}