using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityDev.Data;
using IdentityDev.Models;
using IdentityDev.Services;
using System.IO.Compression;
using System.Linq;
using Boilerplate.AspNetCore;
// $Start-RedirectToCanonicalUrl$
using Boilerplate.AspNetCore.Filters;
// $End-RedirectToCanonicalUrl$
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
// $Start-HttpsEverywhere-On$
using Microsoft.AspNetCore.Rewrite;
// $End-HttpsEverywhere-On$
// $Start-CORS$
using IdentityDev.Constants;
// $End-CORS$
using IdentityDev.Settings;
// $Start-JsonSerializerSettings$
using Newtonsoft.Json.Serialization;
// $End-JsonSerializerSettings$


namespace IdentityDev
{
    public partial class Startup
    {
        #region Fields

        /// <summary>
        /// Gets or sets the application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html
        /// </summary>
        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// The environment the application is running under. This can be Development, Staging or Production by default.
        /// To set the hosting environment on Windows:
        /// 1. On your server, right click 'Computer' or 'My Computer' and click on 'Properties'.
        /// 2. Go to 'Advanced System Settings'.
        /// 3. Click on 'Environment Variables' in the Advanced tab.
        /// 4. Add a new System Variable with the name 'ASPNETCORE_ENVIRONMENT' and value of Production, Staging or
        /// whatever you want. See http://docs.asp.net/en/latest/fundamentals/environments.html
        /// </summary>
        private readonly IHostingEnvironment hostingEnvironment;
        // $Start-HttpsEverywhere-On$

        /// <summary>
        /// Gets or sets the port to use for HTTPS. Only used in the development environment.
        /// </summary>
        private readonly int? sslPort;
        // $End-HttpsEverywhere-On$

        #endregion

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
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets();
            //}

            //builder.AddEnvironmentVariables();
            //Configuration = builder.Build();
        }

        //public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                // $Start-ApplicationInsights$
                // Add Azure Application Insights data collection services to the services container.
                .AddApplicationInsightsTelemetry(this.configuration)
                // $End-ApplicationInsights$
                .AddAntiforgerySecurely()
                .AddCaching()
                .AddOptions(this.configuration)
                .AddRouting(
                    options =>
                    {
                        // Improve SEO by stopping duplicate URL's due to case differences or trailing slashes.
                        // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
                        // All generated URL's should append a trailing slash.
                        options.AppendTrailingSlash = true;
                        // All generated URL's should be lower-case.
                        //options.LowercaseUrls = true;
                    })
                // $Start-CORS$
                // Add cross-origin resource sharing (CORS) services. See https://docs.asp.net/en/latest/security/cors.html
                .AddCors(
                    options =>
                    {
                        // Create named CORS policies here which you can consume using
                        // application.UseCors("PolicyName") or a [EnableCors("PolicyName")] attribute on your
                        // controller or action.
                        options.AddPolicy(
                            CorsPolicyName.AllowAny,
                            builder => builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
                    })
                // $End-CORS$
                .AddResponseCaching()
                // Add response compression to enable GZIP compression.
                .AddResponseCompression(
                    options =>
                    {
                        // $Start-HttpsEverywhere-On$
                        // options.EnableForHttps = true;
                        // $End-HttpsEverywhere-On$
                        // Add additional MIME types (other than the built in defaults) to enable GZIP compression for.
                        var responseCompressionSettings = configuration.GetSection<ResponseCompressionSettings>(
                            nameof(ResponseCompressionSettings));
                        options.MimeTypes = ResponseCompressionDefaults
                            .MimeTypes
                            .Concat(responseCompressionSettings.MimeTypes);
                    })
                .Configure<GzipCompressionProviderOptions>(
                    options => options.Level = CompressionLevel.Optimal)
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the HttpContext outside a controller.
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped<IUrlHelper>(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
                // $Start-RedirectToCanonicalUrl$
                // Adds a filter which help improve search engine optimization (SEO).
                .AddSingleton<RedirectToCanonicalUrlAttribute>();
                            // $End-RedirectToCanonicalUrl$




            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(
                options =>
                {
                        // Controls how controller actions cache content from the config.json file.
                        var cacheProfileSettings = this.configuration.GetSection<CacheProfileSettings>();
                    foreach (var keyValuePair in cacheProfileSettings.CacheProfiles)
                    {
                        options.CacheProfiles.Add(keyValuePair);
                    }
                        // $Start-RedirectToCanonicalUrl$

                        // Adds a filter which help improve search engine optimization (SEO).
                        options.Filters.AddService(typeof(RedirectToCanonicalUrlAttribute));
                        // $End-RedirectToCanonicalUrl$
                    })
            // $Start-JsonSerializerSettings$
            // Configures the JSON output formatter to use camel case property names like 'propertyName' instead of
            // pascal case 'PropertyName' as this is the more common JavaScript/JSON style.
            .AddJsonOptions(
                x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
            // $End-JsonSerializerSettings$
            // $Start-XmlFormatter-DataContractSerializer$
            // Adds the XML input and output formatter using the DataContractSerializer.
            .AddXmlDataContractSerializerFormatters()
            // $End-XmlFormatter-DataContractSerializer$
            // $Start-XmlFormatter-XmlSerializer$
            // Adds the XML input and output formatter using the XmlSerializer.
            .AddXmlSerializerFormatters()
            // $End-XmlFormatter-XmlSerializer$
            .Services
            .AddCustomServices();


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Retrieve SendGrid Configuration
            services.Configure<AuthMessageSenderOptions>(this.configuration.GetSection("MessageSenderOptions"));

            // Retrieve Twilio Configuration
            services.Configure<AuthMessageSMSSenderOptions>(this.configuration.GetSection("SMSSenderOptions"));

            // Retrieve Azure Table Configuration
            services.Configure<AuthStorageTableOptions>(this.configuration.GetSection("TableConnectionStrings"));

            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app
                // Removes the Server HTTP header from the HTTP response for marginally better security and performance.
                .UseNoServerHttpHeader()
                // $Start-HttpsEverywhere-On$
                // Require HTTPS to be used across the whole site. Also set a custom port to use for SSL in
                // Development. The port number to use is taken from the launchSettings.json file which Visual
                // Studio uses to start the application.
                //.UseRewriter(
                //    new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, this.sslPort))
                // $End-HttpsEverywhere-On$
                // $Start-CORS$
                .UseCors(CorsPolicyName.AllowAny)
                // $End-CORS$
                .UseResponseCaching()
                .UseResponseCompression()
                .UseStaticFilesWithCacheControl(this.configuration)
                .UseCookiePolicy()
                .UseIfElse(
                    this.hostingEnvironment.IsDevelopment(),
                    x => x
                        .UseDebugging()
                        .UseDeveloperErrorPages(),
                    x => x.UseErrorPages())
                // $Start-NWebSec$
                // $Start-HttpsEverywhere-On$
                //.UseStrictTransportSecurityHttpHeader()
                //.UsePublicKeyPinsHttpHeader()
                //.UseContentSecurityPolicyHttpHeader(this.sslPort, this.hostingEnvironment)
                // $End-HttpsEverywhere-On$
                // $Start-HttpsEverywhere-Off$
                // .UseContentSecurityPolicyHttpHeader(this.hostingEnvironment)
                // $End-HttpsEverywhere-Off$
                .UseSecurityHttpHeaders();
                // $End-NWebSec$



            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                SignInScheme = "Identity.External", // this is the name of the cookie middleware registered by UseIdentity()
                ClientId = "apps.googleusercontent.com",
                ClientSecret = "",
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
