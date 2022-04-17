using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model.MIC;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace GISA.MIC.WebApi.Controllers
{
    /// <summary />
    [Authorize]
    [ApiController]
    [Route("api/v1/provider")]
    [Produces("application/json")]
    public class ProviderController : BaseController
    {
        private readonly IProviderService _providerService;

        /// <summary />
        /// <param name="cache"></param>
        /// <param name="providerService"></param>
        public ProviderController(IDistributedCache cache, IProviderService providerService) : base(cache)
        {
            _providerService = providerService;
        }

        /// <summary>
        ///     Retorna um prestador por identificador.
        /// </summary>
        /// <param name="providerId">O id do prestador.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{providerId}")]
        public async Task<Response<Provider>> GetByIdAsync(long providerId)
        {
            var response = await _providerService.GetByIdAsync(providerId);
            return response;
        }

        /// <summary>
        ///     Retorna todos os prestadores por estado.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("country-state/{state}/providers")]
        public async Task<Response<IList<Provider>>> GetByStateAsync(string state)
        {
            var response = await _providerService.GetByStateAsync(state);
            return response;
        }

        /// <summary>
        ///     Retorna todos os prestadores com a especialidade informada.
        /// </summary>
        /// <param name="specialtyId">O id da especialidade.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("specialty/{specialtyId}/providers")]
        public async Task<Response<IList<Provider>>> GetProviderBySpecialtyIdAsync(long specialtyId)
        {
            var response = await _providerService.GetProviderBySpecialtyIdAsync(specialtyId);
            return response;
        }
    }
}