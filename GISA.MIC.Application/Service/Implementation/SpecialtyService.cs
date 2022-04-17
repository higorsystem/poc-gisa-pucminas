using System;
using System.Collections.Generic;
using Amazon.Extensions.CognitoAuthentication;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service.Handler;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.Domain.Model.MIC;
using GISA.Domain.Repositories.MIC;

namespace GISA.MIC.Application.Service.Implementation
{
    public class SpecialtyService : ServiceBaseHandler, ISpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        public SpecialtyService(UserManager<CognitoUser> userManager, ISpecialtyRepository specialtyRepository) : base(userManager)
        {
            _specialtyRepository = specialtyRepository ?? throw new ArgumentNullException(nameof(specialtyRepository));
        }

        public async Task<Response<IList<Specialty>>> GetAllAsync()
        {
            var specialties = await _specialtyRepository.GetAllAsync();

            return await ReturnResponseMessageAsync
            (
                response: specialties,
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.Created
            );
        }

        public async Task<Response<Specialty>> GetByIdAsync(long specialtyId)
        {
            var specialty = await _specialtyRepository.GetByIdAsync(specialtyId);

            return await ReturnResponseMessageAsync
            (
                response: specialty,
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.Created
            );
        }
    }
}