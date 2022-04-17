using System;
using System.Collections.Generic;
using Amazon.Extensions.CognitoAuthentication;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service.Handler;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;
using GISA.Domain.Repositories.MIC;

namespace GISA.MIC.Application.Service.Implementation
{
    public class ProviderService : ServiceBaseHandler, IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(UserManager<CognitoUser> userManager, IProviderRepository providerRepository) : base(userManager)
        {
            _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
        }

        public async Task<Response<Provider>> GetByIdAsync(long providerId)
        {
            var responseProvider = await _providerRepository.GetByIdAsync(providerId);

            return await ReturnResponseMessageAsync
            (
                response: responseProvider,
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.OK
            );
        }

        public async Task<Response<IList<Provider>>> GetByStateAsync(string state)
        {
            var providers = await _providerRepository.GetByStateAsync(state);

            return await ReturnResponseMessageAsync
            (
                response: providers,
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.OK
            );
        }

        public async Task<Response<IList<Provider>>> GetProviderBySpecialtyIdAsync(long specialtyId)
        {
            var providers = await _providerRepository.GetProviderBySpecialtyIdAsync(specialtyId);

            return await ReturnResponseMessageAsync
            (
                response: providers,
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.OK
            );
        }
    }
}