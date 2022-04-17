using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerUI;
using GISA.Authentication.Application;
using GISA.Authentication.Application.Interfaces;
using GISA.Authentication.Application.Mappings;
using GISA.Authentication.Application.Notifications;
using GISA.Authentication.Application.Services;
using GISA.Commons.IoC.Extensions;
using GISA.Commons.SDK.AWS;
using GISA.Commons.SDK.AWS.Implementation;

namespace GISA.Authentication.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<NotificationContext>();
            services.AddSingleton<ICloudConfigurationService, AwsConfigurationService>();

            return services;
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(LoginMapping),
                typeof(RefreshTokenMapping),
                typeof(SignUpMapping),
                typeof(SignOutMapping),
                typeof(ForgotPasswordMapping),
                typeof(ResetPasswordMapping),
                typeof(ChangePasswordMapping));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddCognitoIdentity();
        }

        public static void AddDaprClientConfig(this IServiceCollection services)
        {
            var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3600";
            var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60000";

            services.AddDaprClient(builder => builder
                .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
                .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
        }

        public static void AddApplicationInsightsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration.GetAppInsightInstrumentationKey());
        }

        public static void AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"GISA Authentication API ({configuration.GetSection("Environment").Value})",
                    Contact = new OpenApiContact
                    {
                        Name = "Boa Saude GISA",
                        Email = string.Empty
                    }
                });
            });
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

        public static void AddSwaggerApplication(this IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                foreach (var versionDescription in versionProvider.ApiVersionDescriptions)
                    swaggerUiOptions.SwaggerEndpoint($"../swagger/{versionDescription.GroupName}/swagger.json", versionDescription.GroupName.ToUpperInvariant());

                swaggerUiOptions.DocExpansion(DocExpansion.List);
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