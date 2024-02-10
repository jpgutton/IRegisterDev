using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//#if (CORS)
    using APIDev.Constants;
//#endif
using Boilerplate.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
//#if (HttpsEverywhere)
//    using Microsoft.AspNetCore.Rewrite;
//#endif
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace APIDev
{
    public partial class Startup
    {
        /// <summary>
        /// Gets or sets the application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html
        /// </summary>
        private readonly IConfigurationRoot configuration;

        private readonly IHostingEnvironment hostingEnvironment;


        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.configuration = new ConfigurationBuilder()
                .SetBasePath(this.hostingEnvironment.ContentRootPath)
                // Add configuration from the config.json file.
                .AddJsonFile("appsettings.json")
                // Add configuration from an optional config.development.json, config.staging.json or
                // config.production.json file, depending on the environment. These settings override the ones in the
                // config.json file.
                .AddJsonFile($"appsettings.{this.hostingEnvironment.EnvironmentName}.json", optional: true)
                // This reads the configuration keys from the secret store. This allows you to store connection strings
                // and other sensitive settings, so you don't have to check them into your source control provider.
                // Only use this in Development, it is not intended for Production use. See
                // http://docs.asp.net/en/latest/security/app-secrets.html
                //.AddIf(
                //    this.hostingEnvironment.IsDevelopment(),
                //    x => x.AddUserSecrets())
                // Add configuration specific to the Development, Staging or Production environments. This config can
                // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                // override the ones in all of the above config files.
                // Note: To set environment variables for debugging navigate to:
                // Project Properties -> Debug Tab -> Environment Variables
                // Note: To get environment variables for the machine use the following command in PowerShell:
                // [System.Environment]::GetEnvironmentVariable("[VARIABLE_NAME]", [System.EnvironmentVariableTarget]::Machine)
                // Note: To set environment variables for the machine use the following command in PowerShell:
                // [System.Environment]::SetEnvironmentVariable("[VARIABLE_NAME]", "[VARIABLE_VALUE]", [System.EnvironmentVariableTarget]::Machine)
                // Note: Environment variables use a colon separator e.g. You can override the site title by creating a
                // variable named AppSettings:SiteTitle. See http://docs.asp.net/en/latest/security/app-secrets.html
                .AddEnvironmentVariables()
                // $Start-ApplicationInsights$
                // Push telemetry data through the Azure Application Insights pipeline faster in the development and
                // staging environments, allowing you to view results immediately.
                .AddApplicationInsightsSettings(developerMode: !this.hostingEnvironment.IsProduction())
                // $End-ApplicationInsights$
                .Build();
            // $Start-HttpsEverywhere-On$

            //if (this.hostingEnvironment.IsDevelopment())
            //{
            //    var launchConfiguration = new ConfigurationBuilder()
            //        .SetBasePath(this.hostingEnvironment.ContentRootPath)
            //        .AddJsonFile(@"Properties\launchSettings.json")
            //        .Build();
            //    this.sslPort = launchConfiguration.GetValue<int>("iisSettings:iisExpress:sslPort");
            //}
            // $End-HttpsEverywhere-On$



            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services
#if (ApplicationInsights)
                // Add Azure Application Insights data collection services to the services container.
                .AddApplicationInsightsTelemetry(this.configuration)
#endif
                .AddCaching()
                .AddCustomOptions(this.configuration)
                .AddCustomRouting()
                .AddResponseCaching()
                .AddCustomResponseCompression(this.configuration)
#if (Swagger)
                .AddSwagger()
#endif
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the HttpContext outside a controller.
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped<IUrlHelper>(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
                .AddCustomMvc(this.configuration, this.hostingEnvironment)
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddDataAnnotations()
                .AddJsonFormatters()
                .AddCustomJsonOptions()
                //#if (CORS)
                .AddCustomCors();
//#endif
#if (DataContractSerializer)
                // Adds the XML input and output formatter using the DataContractSerializer.
                .AddXmlDataContractSerializerFormatters()
#elif (XmlSerializer)
                // Adds the XML input and output formatter using the XmlSerializer.
                .AddXmlSerializerFormatters()
#endif
                //services.Services
                //.AddCommands()
                //.AddRepositories()
                //.AddTranslators();




            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder application, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            application.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:45100",
                RequireHttpsMetadata = false,

                ApiName = "IRegisterBackEndAPI"
            });


            application.UseNoServerHttpHeader();
            // Removes the Server HTTP header from the HTTP response for marginally better security and performance.
            //.UseNoServerHttpHeader()
            //#if (HttpsEverywhere)
            //                // Require HTTPS to be used across the whole site. Also set a custom port to use for SSL in
            //                // Development. The port number to use is taken from the launchSettings.json file which Visual
            //                // Studio uses to start the application.
            //                .UseRewriter(
            //                    new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, this.sslPort))
            //#endif
            application.UseResponseCaching();
            application.UseResponseCompression();
            application.UseStaticFilesWithCacheControl(this.configuration);
            //#if (CORS)
            application.UseCors(CorsPolicyName.AllowAny)
                //#endif
                .UseIf(
                    this.hostingEnvironment.IsDevelopment(),
                    x => x
                        .UseDebugging()
                        .UseDeveloperErrorPages());
//#if (HttpsEverywhere)
//                .UseStrictTransportSecurityHttpHeader()
//#if (PublicKeyPinning)
//                .UsePublicKeyPinsHttpHeader()
//#endif
//#endif
                // Add MVC to the request pipeline.
//#if (Swagger)
//                .UseMvc()
//                // Add Swagger to the request pipeline.
//                .UseSwagger()
//                .UseSwaggerUi(
//                    options =>
//                    {
//                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Version 1");
//                    });
//#else
//                .UseMvc();
//#endif




            application.UseMvc();
        }
    }
}
