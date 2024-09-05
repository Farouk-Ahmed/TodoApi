using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap();
        }
    }
}
