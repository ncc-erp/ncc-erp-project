using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using ProjectManagement.Configuration;
using ProjectManagement.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Json;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ProjectManagement.Services.Finance;
using ProjectManagement.Services.Timesheet;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.HRM;
using ProjectManagement.Constants;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon;
using ProjectManagement.UploadFilesService;
using ProjectManagement.Services.Talent;
using ProjectManagement.Services;

namespace ProjectManagement.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private const string _apiVersion = "v1";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });



            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            services.AddHttpClient<FinfastService>();
            services.AddHttpClient<TimesheetService>();
            services.AddHttpClient<KomuService>();
            services.AddHttpClient<HRMService>();
            services.AddHttpClient<TalentService>();
            services.AddHttpClient<BaseWebService>();

            RegisterFileService(services);

            ProjectManagement.Constants.AppConsts.FE_TALENT_ADDRESS = _appConfiguration.GetValue<string>("TalentService:FEAddress");

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "ProjectManagement API",
                    Description = "ProjectManagement",
                    // uncomment if needed TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "ProjectManagement",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/aspboilerplate"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
                    }
                });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<ProjectManagementWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app,  ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                // specifying the Swagger JSON endpoint.
                options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"ProjectManagement API {_apiVersion}");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("ProjectManagement.Web.Host.wwwroot.swagger.ui.index.html");
                options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.  
            }); // URL: /swagger
        }
        void CreateAWSCredentialProfile()
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = ConstantAmazonS3.AccessKeyId,
                SecretKey = ConstantAmazonS3.SecretKeyId
            };
            var profile = new CredentialProfile(ConstantAmazonS3.Profile, options);
            profile.Region = RegionEndpoint.GetBySystemName(ConstantAmazonS3.Region);

            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);
        }

        private void LoadUploadFileConfig()
        {
            ConstantAmazonS3.Profile = _appConfiguration.GetValue<string>("AWS:Profile");
            ConstantAmazonS3.AccessKeyId = _appConfiguration.GetValue<string>("AWS:AccessKeyId");
            ConstantAmazonS3.SecretKeyId = _appConfiguration.GetValue<string>("AWS:SecretKeyId");
            ConstantAmazonS3.Region = _appConfiguration.GetValue<string>("AWS:Region");
            ConstantAmazonS3.BucketName = _appConfiguration.GetValue<string>("AWS:BucketName");
            ConstantAmazonS3.Prefix = _appConfiguration.GetValue<string>("AWS:Prefix");
            ConstantAmazonS3.CloudFront = _appConfiguration.GetValue<string>("AWS:CloudFront");

            ConstantUploadFile.AvatarFolder = _appConfiguration.GetValue<string>("UploadFile:AvatarFolder");

            ConstantUploadFile.Provider = _appConfiguration.GetValue<string>("UploadFile:Provider");
            var strAllowImageFileType = _appConfiguration.GetValue<string>("UploadFile:AllowImageFileTypes");
            ConstantUploadFile.AllowImageFileTypes = strAllowImageFileType.Split(",");

            var strAllowTimesheetFileType = _appConfiguration.GetValue<string>("UploadFile:AllowTimesheetFileTypes");
            if (string.IsNullOrEmpty(strAllowTimesheetFileType))
            {
                strAllowTimesheetFileType = "xlsx,xltx,docx";
            }
            ConstantUploadFile.AllowTimesheetFileTypes = strAllowTimesheetFileType.Split(",");

            ConstantInternalUploadFile.RootUrl = _appConfiguration.GetValue<string>("App:ServerRootAddress");
        }

        private void RegisterFileService(IServiceCollection services)
        {
            LoadUploadFileConfig();

            if (ConstantUploadFile.Provider == ConstantUploadFile.AMAZONE_S3)
            {
                CreateAWSCredentialProfile();
                services.AddAWSService<IAmazonS3>();
                services.AddTransient<IUploadFileService, AmazonS3Service>();
            }
            else
            {
                services.AddTransient<IUploadFileService, InternalUploadFileService>();
            }

        }
    }
}
