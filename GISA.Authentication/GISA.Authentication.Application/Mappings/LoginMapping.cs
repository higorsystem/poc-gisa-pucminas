using Amazon.CognitoIdentityProvider.Model;
using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class LoginMapping : Profile
    {
        public LoginMapping()
        {
            CreateMap<AuthenticationResultType, LoginResponseViewModel>();
            CreateMap<LoginRequestViewModel, Login>();
        }
    }
}