using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Dapr.Client;
using GISA.Domain.Model.DTO;
using GISA.Domain.Repositories.MIC;
using GISA.MIC.Application.Helper;
using GISA.MIC.Application.Service.Handler;
using GISA.MIC.Application.Service.Parser;
using Microsoft.AspNetCore.Identity;

namespace GISA.MIC.Application.Service.Implementation
{
    public class ConsultService : ServiceBaseHandler, IConsultService
    {
        private readonly IConsultRepository _consultRepository;
        private readonly DaprClient _daprClient;

        public ConsultService(UserManager<CognitoUser> userManager, DaprClient daprClient,
            IConsultRepository consultRepository) : base(userManager)
        {
            _consultRepository = consultRepository ?? throw new ArgumentNullException(nameof(consultRepository));
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<Response<ConsultDto>> SaveConsultScheduleAsync(ConsultDto request, long personId = default)
        {
            if (!string.IsNullOrEmpty(value: Environment.GetEnvironmentVariable(variable: "DAPR")))
                await _daprClient.PublishEventAsync(pubsubName: "pubsub", topicName: "request-consult", data: request);
            else
                await _consultRepository.SaveAsync(consult: request.ToConsultBo(personId: personId));

            return await ReturnResponseMessageAsync
            (
                response: request,
                message: "Consulta agendada com sucesso.",
                isSuccess: true,
                statusCode: HttpStatusCode.Created
            );
        }

        public async Task<Response<IList<ConsultDto>>> CheckStatusByProviderAsync(string checkStatus, long personId)
        {
            var businessObject = await _consultRepository.CheckStatusByProviderAsync(consultStatus: checkStatus, providerId: personId);

            return await ReturnResponseMessageAsync
            (
                response: businessObject.ToListConsultDto(),
                message: string.Empty,
                isSuccess: true,
                statusCode: HttpStatusCode.OK
            );
        }
    }
}