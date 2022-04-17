using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class ChangePasswordMapping : Profile
    {
        public ChangePasswordMapping()
        {
            CreateMap<ChangePasswordRequestViewModel, ChangePassword>();
            CreateMap<ChangePasswordRequestViewModel, SignUpRequestViewModel>().ReverseMap();
        }
    }
}