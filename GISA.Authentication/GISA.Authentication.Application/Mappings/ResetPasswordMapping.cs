using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class ResetPasswordMapping : Profile
    {
        public ResetPasswordMapping()
        {
            CreateMap<ResetPasswordRequestViewModel, ResetPassword>();
        }
    }
}