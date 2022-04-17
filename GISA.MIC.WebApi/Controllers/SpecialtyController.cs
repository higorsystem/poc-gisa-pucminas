using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Domain.Model;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace GISA.MIC.WebApi.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/specialty")]
    public class SpecialtyController : BaseController
    {
        private readonly ISpecialtyService _specialtyService;

        /// <summary>
        ///     Controller.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="userManager"></param>  
        /// <param name="specialtyRepository"></param>
        public SpecialtyController(IDistributedCache cache, ISpecialtyService specialtyService) : base(cache)
        {
            _specialtyService = specialtyService;
        }

        /// <summary />
        /// <param name="specialtyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{specialtyId}")]
        public async Task<Response<Specialty>> GetByIdAsync(long specialtyId)
        {
            var response = await _specialtyService.GetByIdAsync(specialtyId);
            return response;
        }

        /// <summary />
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<IList<Specialty>>> GetAllAsync()
        {
            var cachedData = GetCacheData("SpecialtyCollection");

            if (cachedData == null)
            {
                var response = await _specialtyService.GetAllAsync();

                await SetCacheDataAsync("SpecialtyCollection",
                    JsonConvert.SerializeObject(response,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                return response;
            }

            return JsonConvert.DeserializeObject<Response<IList<Specialty>>>(cachedData);
        }
    }
}