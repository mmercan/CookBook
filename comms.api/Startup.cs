using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Comms.Api.Helpers;
using CookBook.Common;
using CookBook.Models;
using Comms.Api.CustomFeatureFilter;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.FeatureManagement.FeatureFilters;
using AutoMapper;
using System.Net.Http.Headers;
using Microsoft.FeatureManagement;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Serilog;
using Serilog.Events;

namespace Comms.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IServiceCollection>(services);
            services.AddSingleton<IConfiguration>(Configuration);
            // services.AddSingleton<PushNotificationService>();
            services.AddHealthChecks();

            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights"]);
            services.AddApplicationInsightsKubernetesEnricher();


            services.ConfigureJwtAuthService(Configuration);
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                // builder.AllowAnyOrigin()
                // .AllowAnyMethod()
                // .AllowAnyHeader()
                // .SetIsOriginAllowedToAllowWildcardSubdomains();
                // //.AllowCredentials();

                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowCredentials()
                .WithOrigins("http://localhost:4300", "https://app-health-ui.dev.myrcan.com");

            }));

            services.AddApiVersioning(options =>
           {
               options.ReportApiVersions = true;
               options.AssumeDefaultVersionWhenUnspecified = true;
               options.DefaultApiVersion = new ApiVersion(1, 0);
               options.ApiVersionReader = new HeaderApiVersionReader("api-version");
           });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.IncludeXmlComments(XmlCommentsFilePath);

                options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "Bearer "
                });

            });
            services.AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]);
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisConnection"];
                options.InstanceName = "ApiComms";
            });
            services.AddMangoRepo<Recipe>(Configuration.GetSection("Mongodb"));

            services.AddHttpClient("run_with_try", options =>
           {
               options.Timeout = new TimeSpan(0, 2, 0);
               options.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
               options.DefaultRequestHeaders.Add("OData-Version", "4.0");
               options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           }).ConfigurePrimaryHttpMessageHandler<CertMessageHandler>();
            // ConfigurePrimaryHttpMessageHandler((ch) =>
            // ConfigurePrimaryHttpMessageHandler((ch) =>
            // ConfigurePrimaryHttpMessageHandler((ch) =>
            // {
            //     var handler = new HttpClientHandler();
            //     handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //     handler.ClientCertificates.Add(HttpClientHelpers.GetCert());
            //     return handler;

            // })
            // .AddHttpMessageHandler()
            // .AddHttpMessageHandler<OAuthTokenHandler>()
            //.AddHttpMessageHandler(*)Helpers.GetRetryPolicy())Policy());

            services.AddHttpContextAccessor();

            services.AddFeatureManagement()
            .AddFeatureFilter<PercentageFilter>()
            .AddFeatureFilter<HeadersFeatureFilter>();
            // services.AddHostedService<HealthCheckSubscribeService>();

            services.AddSignalR();
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider, IHostApplicationLifetime lifetime, IDistributedCache cache)
        {

            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Enviroment", Environment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", "Api App")
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File("Logs/logs.txt");
            //.WriteTo.Elasticsearch()
            logger.WriteTo.Console();
            loggerFactory.AddSerilog();
            Log.Logger = logger.CreateLogger();
            app.UseExceptionLogger();


            app.UseSwagger(e => { e.AddHealthCheckSwaggerOptions(); });
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });

            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAllAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var basePath = AppContext.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
