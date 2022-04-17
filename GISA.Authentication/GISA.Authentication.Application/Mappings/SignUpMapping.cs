using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class SignUpMapping : Profile
    {
        public SignUpMapping()
        {
            CreateMap<SignUpRequestViewModel, SignUp>();
            CreateMap<SignUp, SignUpResponseViewModel>();
        }
    }
}