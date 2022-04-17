using GISA.Domain.Repositories.MIC;
using GISA.MIC.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace IntegrationTests
{
    [SetUpFixture]
    public abstract class RepositoryTests
    {
        private IConfiguration _config;

        protected MICDbContext context;

        protected IAssociateRepository _associateRepository;
        protected ISpecialtyRepository _specialtyRepository;
        protected IProcedureRepository _procedureRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(Configuration);

            context = new MICDbContext(new DbContextOptionsBuilder<MICDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("ConnectionString"))
                .Options);

            context.Database.EnsureCreated();

            _associateRepository = new AssociateRepository(context);
            _specialtyRepository = new SpecialtyRepository(context);
            _procedureRepository = new ProcedureRepository(context);
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            context.Database.EnsureDeleted();
        }

        internal IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testsettings.json", optional: false);

                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}
