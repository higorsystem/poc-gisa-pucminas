using GISA.Commons.IoC.Extensions;
using GISA.MIC.Application.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO.Compression;
using System.Text;

namespace GISA.MIC.WebApi
{
    /// <summary />
    public class Startup
    {
        /// <summary />
        protected IWebHostEnvironment WebEnvironment { get; }

        /// <summary />
        /// <param name="configuration"></param>
        /// <param name="webEnvironment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webEnvironment)
        {
            Configuration = configuration;
            WebEnvironment = webEnvironment;
        }

        /// <summary />
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Configurações de base de dados e cache para minimiza a carga de requisições ao banco.
            services.AddConfigurationDatabaseInitialize(Configuration);

            // Configuração do cache do Redis.
            services
                .AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration.GetEnvironmentVariableOrConfigurantion("REDIS_HOST", "GISA.Redis");
                    options.InstanceName = "GISA.MIC.";
                });

            services.AddCognitoIdentity();

            //Repository, Service e configuracao da AWS.
            services.AddInfrastructure();
            services.AddGisaMicServices();
            services.AddCloudConfiguration();

            //Config. da inicialização do DAPR Client.
            services.AddDaprClientConfig(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = Configuration.GetEnvironmentVariableOrConfigurantion("AWS_JWT_ISSUER", "AWS:Issuer"), // TODO: Adicionar na variavel de ambiente
                    ValidAudience = Configuration.GetEnvironmentVariableOrConfigurantion("AWS_JWT_AUDIENCE", "AWS:Audience"),// TODO: Adicionar na variavel de ambiente
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetEnvironmentVariableOrConfigurantion("AWS_JWT_KEY", "AWS:Key")))// TODO: Adicionar na variavel de ambiente
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });

            // Config. da documentação do Swagger.
            services.AddSwaggerDocumentation(Configuration);

            services.ConfigureCors(Configuration);
            services.AddApiVersion();
            services.AddHealthChecks();

            // Config. de compressão..
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            // Config. do Application Insights para metricas de telemetria e monitoramento dos endpooints.
            if (WebEnvironment.IsDevelopment())
                services.AddApplicationInsightsTelemetry(options => options.DeveloperMode = true);
            else
                services.AddApplicationInsightsTelemetry(Configuration.GetAppInsightInstrumentationKey());

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="versionProvider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHealthChecks("/health");
            app.AddSwaggerApplication(versionProvider);
            app.UseApiVersioning();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}