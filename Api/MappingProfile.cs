using AutoMapper;
using Domain.Models;

namespace Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, DataAccessLayer.Entities.User>();
            CreateMap<DataAccessLayer.Entities.User, User>();
        }
	}
}