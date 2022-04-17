using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class ConfirmSignUpMapping : Profile
    {
        public ConfirmSignUpMapping()
        {
            CreateMap<ConfirmSignUpRequestViewModel, ConfirmSignUp>();
            CreateMap<ResendConfirmationCodeRequestViewModel, ResendConfirmationCode>();
        }
    }
}