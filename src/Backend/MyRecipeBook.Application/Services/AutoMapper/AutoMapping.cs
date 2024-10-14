using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
        }
    }
}
