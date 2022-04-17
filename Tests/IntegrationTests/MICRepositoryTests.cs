using GISA.Domain.Model;

using NUnit.Framework;

using System;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using GISA.Domain.Model.MIC;

namespace IntegrationTests
{
    [TestFixture]
    public class MICRepositoryTests : RepositoryTests
    {
        private static readonly Bogus.DataSets.Name nameGenerator = new Bogus.DataSets.Name("pt_BR");
        private static readonly Bogus.DataSets.Address addressGenerator = new Bogus.DataSets.Address("pt_BR");

        #region Associate

        [TestCase]
        public async Task DeveSalvar_UmAssociado()
        {
            await _associateRepository.SaveAsync(
                Associate("12346578909", new Random(1).Next(999999999).ToString()));

            Assert.IsTrue(context.SaveChanges() > 0);
        }

        [Test]
        public async Task DeveAlterar_OAssociado()
        {
            var associado = await _associateRepository.SaveAsync(
                Associate("99988877765", new Random(2).Next(999999999).ToString()));

            context.SaveChanges();

            associado.CPF = "88877766655";
            associado = await _associateRepository.UpdateAsync(associado);

            Assert.IsTrue(associado.CPF == "88877766655");
        }

        [Test]
        public async Task DeveDeletar_OAssociado()
        {
            var associado = await _associateRepository.SaveAsync(
                Associate("77766655544", new Random(3).Next(999999999).ToString()));

            await context.SaveChangesAsync();

            await _associateRepository.DeleteAsync(associado);

            await context.SaveChangesAsync();

            associado = await _associateRepository.GetByCpfAsync("77766655544");

            Assert.IsTrue(associado == null);
        }

        [Test]
        public async Task DeveBuscar_UmAssociadoPorCpf()
        {
            var associado = await _associateRepository.SaveAsync(
                Associate("66655544433", new Random(4).Next(999999999).ToString()));

            context.SaveChanges();

            associado = await _associateRepository.GetByCpfAsync("66655544433");

            Assert.IsTrue(associado != null);
        }

        #endregion

        #region Specialty

        [TestCase]
        public async Task DeveRecuperarTodas_AsEspecialidades()
        {
            var specialties = await _specialtyRepository.GetAllAsync();

            Assert.IsTrue(specialties != null);
        }

        #endregion

        #region Procedure

        [TestCase]
        public async Task ProcedimentoRecuperarTodos()
        {
            var procedure = await _procedureRepository.GetAllProcedureAsync();

            Assert.IsTrue(procedure != null);
        }

        #endregion

        #region Helpers

        internal static Associate Associate(string cpf, string cardNumber)
        {
            return new Associate()
            {
                CPF = cpf,
                RG = "999999999",
                IssuedAt = DateTime.UtcNow,
                IssuedBy = 1,
                JoiningDate = DateTime.UtcNow,
                BirthDate = DateTime.UtcNow.AddYears(-30),
                Email = new Email("email@email.com"),
                Address = new Address( addressGenerator.StreetAddress(),
                "",
                int.Parse(addressGenerator.BuildingNumber()),
                addressGenerator.ZipCode().Replace("-", ""),
                addressGenerator.City(),
                addressGenerator.StateAbbr()),
                Genre = EGenderPerson.Masculino,
                CardNumber = cardNumber,
                HealthInsurance = new HealthInsurance
                {
                    Id = (long)1, 
                    AnsNumber = "123456",
                    CategoryPlan = ECategoryPlan.VIP, 
                    CommercialName = "VIP BLACK - TOP INTERNATIONAL",
                    DentalPlan = true
                },
                Name = $"{nameGenerator.FirstName()} {nameGenerator.LastName()}",
                PlanStatus = EPlanStatus.Suspenso
            };
        }

        #endregion
    }
}