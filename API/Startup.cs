using Fingers10.EnterpriseArchitecture.API.Utils;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Fingers10.EnterpriseArchitecture.Infrastructure.Data;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Fingers10.EnterpriseArchitecture.API
{
    /// <summary>
    /// Startup that gets called on application start to setup services and request middlewares
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor for Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Appsettings Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(new ProducesDefaultResponseTypeAttribute());
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                options.ReturnHttpNotAcceptable = true;
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "https://enterprisearchitecture.com/modelvalidationproblem",
                            Title = "One or more model validation errors occurred.",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "See the errors property for details.",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV";
                //options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //options.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            IApiVersionDescriptionProvider apiVersionDescriptionProvider = GetApiVersionDescriptionProvider(services);

            services.AddSwaggerGen(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        $"EnterpriseArchitectureOpenAPISpecification{description.GroupName}",
                        new Microsoft.OpenApi.Models.OpenApiInfo
                        {
                            Title = "EnterpriseArchitecture API",
                            Version = description.ApiVersion.ToString(),
                            Description = "Through this API you can access architecture reference and knowledge",
                            Contact = new Microsoft.OpenApi.Models.OpenApiContact
                            {
                                Email = "abdulrahman.smsi@gmail.com",
                                Name = "Abdul Rahman",
                                Url = new Uri("https://www.linkedin.com/in/fingers10")
                            },
                            // Need to change the license in future
                            License = new Microsoft.OpenApi.Models.OpenApiLicense
                            {
                                Name = "MIT License",
                                Url = new Uri("https://opensource.org/licenses/MIT")
                            },
                            //TermsOfService = new Uri("")
                        });
                }

                //options.AddSecurityDefinition("token", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OpenIdConnect,
                //    OpenIdConnectUrl = new Uri(Configuration.GetValue<string>("IDP"))
                //});

                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "token" }
                //        }, Array.Empty<string>()
                //    }
                // });

                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "Bearer",
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
                //});

                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer" }
                //        }, new List<string>() }
                //});

                options.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }

                    if (actionApiVersionModel.DeclaredApiVersions.Count > 0)
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"EnterpriseArchitectureOpenAPISpecificationv{v}" == documentName);
                    }

                    return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                        $"EnterpriseArchitectureOpenAPISpecificationv{v}" == documentName);
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddHttpContextAccessor();
            var config = new Config(Configuration.GetValue<int>("DatabaseConnectRetryAttempts"));
            services.AddSingleton(config);
            var consoleLogging = new ConsoleLogging(Configuration.GetValue<bool>("EnableConsoleLogging"));
            services.AddSingleton(consoleLogging);

            var commandsConnectionString = new CommandsConnectionString(Configuration["ConnectionString"]);
            var queriesConnectionString = new QueriesConnectionString(Configuration["ConnectionString"]);
            services.AddSingleton(commandsConnectionString);
            services.AddSingleton(queriesConnectionString);

            services.AddTransient<IBus, Bus>();
            services.AddTransient<MessageBus>();
            services.AddTransient<EventDispatcher>();
            services.AddTransient<Messages>();
            services.AddTransient<Fingers10Context>();
            services.AddTransient<IAsyncRepository<Student>, StudentRepository>();
            services.AddTransient<IStudentReadonlyRepository, StudentReadonlyRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddHandlers();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="apiVersionDescriptionProvider"></param>
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/EnterpriseArchitectureOpenAPISpecification{description.GroupName}/swagger.json",
                        $"EnterpriseArchitecture API - {description.GroupName.ToUpperInvariant()}");
                }

                options.RoutePrefix = string.Empty;
                //options.OAuthClientId("enterprisearchitectureapi");
                options.DefaultModelExpandDepth(2);
                options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                options.DisplayRequestDuration();
                options.EnableValidator();
                options.EnableFilter();
                options.EnableDeepLinking();
                options.DisplayOperationId();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static IApiVersionDescriptionProvider GetApiVersionDescriptionProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
