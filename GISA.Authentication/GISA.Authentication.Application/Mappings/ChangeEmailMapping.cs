using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class ChangeEmailMapping : Profile
    {
        public ChangeEmailMapping()
        {
            CreateMap<ChangeEmailRequestViewModel, ChangeEmail>();
            CreateMap<ChangeEmailRequestViewModel, SignUpRequestViewModel>().ReverseMap();
        }
    }
}