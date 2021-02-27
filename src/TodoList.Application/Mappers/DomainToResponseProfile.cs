﻿using System.Collections.Generic;
using AutoMapper;
using TodoList.Domain.Contract.Responses;
using TodoList.Domain.Entities;

namespace TodoList.Application.Mappers
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<TodoItem, TodoResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ErrorResponse, TodoResponse>()
                .ForMember(m => m.ErrorResponse, opt => opt.MapFrom(src => src.Errors));


            CreateMap<TodoItem, TodoResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}