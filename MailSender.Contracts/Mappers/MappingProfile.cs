using AutoMapper;
using MailSender.Contracts.DTOs;

namespace MailSender.Contracts.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterAppDto, SimpleClientAppDto>();
            CreateMap<SimpleClientAppDto, RegisterAppDto>();
            CreateMap<RegisterDto, RegisterAppDto>().ForMember(dest => dest.SigningJwtKey, opt => opt.Ignore());
            CreateMap<SimpleClientAppDto, RegisteredDto>().ForMember(dest => dest.Key, opt => opt.Ignore());
        }
    }
}
