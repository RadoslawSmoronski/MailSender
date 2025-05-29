using AutoMapper;
using MailSender.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.Mappers
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
