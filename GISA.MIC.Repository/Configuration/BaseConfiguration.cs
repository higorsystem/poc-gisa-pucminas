using System.Collections.Generic;
using GISA.Commons.SDK.Extensions;
using GISA.Domain.Model;

namespace GISA.MIC.Repository.Configuration
{
    public abstract class BaseConfiguration
    {
        private static readonly Bogus.DataSets.Address AddressGenerator = new Bogus.DataSets.Address("pt_BR");

        protected List<object> Emails { get; } = new List<object>();

        protected List<object> Address { get; } = new List<object>();

        //TODO: Ajustar as entradas mockadas dos dados para não saltar com 1000.
        public BaseConfiguration(EPersonType ePersonType)
        {
            for (long personId = 1; personId < 11; personId += 1)
            {
                Emails.Add(new { AssociateId = personId * 1000, ProviderId = personId * 1000, EmailAddress = $"{ePersonType.ToDescription().ToLower()}-{personId * 1000}@boasaude.com.br" });
                Address.Add(new
                {
                    AssociateId = personId * 1000,
                    ProviderId = personId * 1000,
                    Area = AddressGenerator.County(),
                    Street = AddressGenerator.StreetAddress(),
                    Number = int.Parse(AddressGenerator.BuildingNumber()),
                    ZipCode = AddressGenerator.ZipCode().Replace("-", ""),
                    City = AddressGenerator.City(),
                    State = AddressGenerator.StateAbbr()
                });
            }
        }
    }
}
