using AutoMapper;
using HandyManAPI.Inputs;
using HandyManAPI.Models;
 
namespace HandyManAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRecord, UserDto>();
            CreateMap<UserDto, UserRecord>();
        }
    }
}