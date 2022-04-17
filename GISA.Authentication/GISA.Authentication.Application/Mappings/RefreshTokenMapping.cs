using AutoMapper;
using GISA.Authentication.Application.ViewModels;
using GISA.Authentication.Domain.Entities;

namespace GISA.Authentication.Application.Mappings
{
    public class RefreshTokenMapping : Profile
    {
        public RefreshTokenMapping()
        {
            CreateMap<RefreshTokenRequestViewModel, RefreshToken>();
        }
    }
}