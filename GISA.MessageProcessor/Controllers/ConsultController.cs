using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using GISA.Domain.Model;
using GISA.Domain.Model.DTO;
using GISA.Domain.Model.MIC;
using GISA.Domain.Repositories.MIC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GISA.MessageProcessor.Controllers
{
    [ApiController]
    [Route("api/v1.0/consult")]
    public class ConsultController : ControllerBase
    {
        private readonly IAssociateRepository _associateRepository;
        private readonly IConsultRepository _consultRepository;
        private readonly DaprClient _daprClient;
        private readonly ILogger<ConsultController> _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly ISpecialtyRepository _specialtyRepository;

        public ConsultController(DaprClient daprClient,
            ILogger<ConsultController> logger,
            IAssociateRepository associateRepository,
            IConsultRepository consultRepository,
            ISpecialtyRepository specialtyRepository,
            IProviderRepository providerRepository)
        {
            _daprClient = daprClient;

            _logger = logger;

            _associateRepository = associateRepository;
            _consultRepository = consultRepository;
            _specialtyRepository = specialtyRepository;
            _providerRepository = providerRepository;
        }

        [Topic("pubsub", "request-consult")]
        [Route("schedule-consult")]
        [HttpPost]
        public async Task<ActionResult> ScheduleMedicalAppointmentAsync(ConsultDto consult)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(consult));

            //TODO: Refatorar para usar o contrato de Request/Response
            Associate associate = null;
            Provider provider = null;

            try
            {
                associate = await _associateRepository.GetByIdAsync(consult.AssociateId);
                _logger.LogInformation("Associate: " + JsonConvert.SerializeObject(associate));

                var specialty = await _specialtyRepository.GetByIdAsync(consult.SpecialtyId);
                _logger.LogInformation("Specialty: " + JsonConvert.SerializeObject(specialty));

                provider = await _providerRepository.GetByIdAsync(consult.ProviderId);
                _logger.LogInformation("Provider: " + JsonConvert.SerializeObject(provider));

                var appointmentScheduled = await _consultRepository.SaveAsync(new Consult(consult));

                var detailEmail = new Dictionary<string, string>
                {
                    ["emailFrom"] = "atendimento@boasaude.com.br",
                    ["emailTo"] = associate.Email.EmailAddress,
                    ["subject"] =
                        $"[Boa Saúde] Sua consulta em {consult.ConsultDate:dd/MM} às {consult.ConsultDate:HH:mm} foi agendada."
                };

                await _daprClient.PublishEventAsync("pubsub", "request-consult",
                    new
                    {
                        metadata = new
                        {
                            emailFrom = "atendimento@boasaude.com.br",
                            emailTo = associate.Email.EmailAddress,
                            subject =
                                $"[Boa Saúde] Sua consulta em {consult.ConsultDate:dd/MM} às {consult.ConsultDate:HH:mm} foi agendada"
                        },
                        data =
                            $"<span style='font-family: Verdana'><b>{associate.Name.Split(' ')[0]}</b>,</br></br>Sua consulta com o(a) Dr(a) {provider.Name}, especialidade {specialty.Name}, às {consult.ConsultDate:HH:mm} do dia {consult.ConsultDate:dd/MM} está marcada.</br></br>Até lá!</br></br>Equipe Boa Saúde</span>"
                    });
            }
            catch (Exception)
            {
                await _daprClient.PublishEventAsync("pubsub", "canceled-consult",
                    new
                    {
                        metadata = new
                        {
                            emailFrom = "atendimento@boasaude.com.br",
                            emailTo = associate.Email.EmailAddress,
                            subject = "[Boa Saúde] Ops! Não foi possível agendar a sua consulta :("
                        },
                        data =
                            $"<span style='font-family: Verdana'><b>{associate.Name.Split(' ')[0]}</b>,</br></br>Sua consulta com o(a) Dr(a) {provider.Name} às {consult.ConsultDate:HH:mm} do dia {consult.ConsultDate:dd/MM} NÃO FOI AGENDADA.</br></br>Equipe Boa Saúde</span>"
                    });
            }

            return Ok();
        }
    }
}