using AutoMapper;
using TodoList.Application.Mappers;

namespace TodoList.Application
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToResponseProfile());
                cfg.AddProfile(new RequestToDomainProfile());
                cfg.AddProfile(new ResponseToDomainProfile());
            });

            return config;
        }
    }
}