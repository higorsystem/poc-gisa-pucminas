using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class ForgotPasswordMapping : Profile
    {
        public ForgotPasswordMapping()
        {
            CreateMap<ForgotPasswordRequestViewModel, ForgotPassword>();
        }
    }
}