using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GISA.Authentication.Infra.IoC;

namespace GISA.Authentication.WebApi
{
    /// <summary />
    public class Startup
    {
        /// <summary />
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary />
        public IConfiguration Configuration { get; }

        /// <summary />
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddDaprClientConfig();
            services.ConfigureCors(Configuration);
            services.AddAutoMapper();
            services.AddIdentity();
            services.AddHealthChecks();
            services.AddApplicationInsightsConfiguration(Configuration);
            services.AddSwaggerDocumentation(Configuration);
            services.AddApiVersion();
            services.AddControllers();
        }

        /// <summary />
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="versionProvider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseAppCors();
            app.UseHealthChecks("/health");
            app.AddSwaggerApplication(versionProvider);
            app.UseApiVersioning();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}