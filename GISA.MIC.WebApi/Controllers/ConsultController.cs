using System.Collections.Generic;
using System.Threading.Tasks;
using GISA.Commons.SDK.Extensions;
using GISA.Domain.Model.DTO;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service;
using GISA.MIC.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace GISA.MIC.WebApi.Controllers
{
    /// <summary />
    [Authorize]
    [ApiController]
    [Route("api/v1/consult")]
    [Produces("application/json")]
    public class ConsultController : BaseController
    {
        private readonly IConsultService _consultService;

        /// <inheritdoc />
        public ConsultController(IDistributedCache cache, IConsultService consultService) : base(cache)
        {
            _consultService = consultService;
        }

        /// <summary />
        /// <param name="checkStatus"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("check-status/{checkStatus}")]
        [UserClaimCognito(UserTypeValue = nameof(EClaimRoleType.Provider))]
        public async Task<Response<IList<ConsultDto>>> GetByStatusAsync(string checkStatus)
        {
            var response = await _consultService.CheckStatusByProviderAsync(checkStatus, PersonId);
            return response;
        }

        /// <summary />
        /// <param name="consult"></param>
        /// <returns></returns>
        [HttpPost]
        [UserClaimCognito(UserTypeValue = nameof(EClaimRoleType.Associate))]
        public async Task<Response<ConsultDto>> PostAsync(ConsultDto consult)
        {
            var response = await _consultService.SaveConsultScheduleAsync(consult, PersonId);
            return response;
        }
    }
}