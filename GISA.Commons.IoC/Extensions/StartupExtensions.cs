using System;
using GISA.Commons.SDK.AWS;
using GISA.Commons.SDK.AWS.Implementation;
using GISA.Domain.Repositories.MIC;
using GISA.MIC.Application.Service;
using GISA.MIC.Application.Service.Implementation;
using GISA.MIC.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GISA.Commons.IoC.Extensions
{
    public static class StartupExtensions
    {
        public static void AddConfigurationDatabaseInitialize(this IServiceCollection services, IConfiguration configuration)
        {
            // Config. do banco de dados e inicialização do contexto para executar as rotinas automáticas.
            services.AddDbContext<MICDbContext>
            (options =>
                options.UseSqlServer
                (
                    configuration
                        .GetEnvironmentVariableOrConfigurantion("CONNECTION_STRING", "ConnectionStrings:ConnectionString")
                )
            );

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            services
                .BuildServiceProvider()
                .GetService<MICDbContext>()?.Database
                .EnsureCreated();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            // Config. dos repositórios.
            services.AddScoped<IAssociateRepository, AssociateRepository>();
            services.AddScoped<IConsultRepository, ConsultRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
        }

        public static void AddGisaMicServices(this IServiceCollection services)
        {
            // Config. dos Serviços.
            services.AddScoped<IConsultService, ConsultService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IProviderService, ProviderService>();
        }

        public static void AddCloudConfiguration(this IServiceCollection services)
        {
            // Config. do SDK
            services.AddScoped<ICloudConfigurationService, AwsConfigurationService>();
        }

        public static void AddDaprClientConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // Config. do Dapr. 
            var daprHttpPort = configuration.GetEnvironmentVariableOrConfigurantion("DAPR_HTTP_PORT", "DaprClient:HttpPort");
            var daprGrpcPort = configuration.GetEnvironmentVariableOrConfigurantion("DAPR_GRPC_PORT", "DaprClient:GrpcPort");

            services
                .AddDaprClient(builder => builder
                    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
                    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
        }

        public static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });
        }

        public static string GetEnvironmentVariableOrConfigurantion(this IConfiguration configuration, string environVariable, string configSectionValue)
        {
            return Environment.GetEnvironmentVariable(environVariable) ?? configuration[configSectionValue];
        }

        /// <summary>
        /// Shorthand for GetSection("ApplicationInsights")["InstrumentationKey"].
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The instrumentation key.</returns>
        public static string GetAppInsightInstrumentationKey(this IConfiguration configuration)
        {
            return Environment.GetEnvironmentVariable("APP_INSIGHTS_INSTR_KEY") ?? configuration?.GetSection("ApplicationInsights")?["InstrumentationKey"];
        }

        public static void AddSwaggerApplication(this IApplicationBuilder app,
            IApiVersionDescriptionProvider versionProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                foreach (var versionDescription in versionProvider.ApiVersionDescriptions)
                    swaggerUiOptions.SwaggerEndpoint($"../swagger/{versionDescription.GroupName}/swagger.json",
                        versionDescription.GroupName.ToUpperInvariant());

                swaggerUiOptions.DocExpansion(DocExpansion.List);
            });
        }

        public static void AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title =
                        $"GISA - {configuration.GetEnvironmentVariableOrConfigurantion("API_DESCRIPTION","API:Description")} ({configuration.GetEnvironmentVariableOrConfigurantion("ASPNETCORE_ENVIRONMENT", "Environment")})",
                    Contact = new OpenApiContact
                    {
                        Name = "Jorge Higor Mendes dos Santos",
                        Email = "jorge.higor@gmail.com"
                    },
                    Description = "API de integração do módulo de informações cadastrais - MIC"
                });
            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options
                .AddPolicy("CORS", option =>
                {
                    option
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(configuration.GetSection("ALLOWED_CORS").Get<string[]>());
                }));
        }

        public static void UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("CORS");
        }
    }
}