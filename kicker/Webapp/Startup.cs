using Communication;
using ImageProcessing;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoSource.Dalsa;
using Webapp.Controllers;
using Webapp.Hubs;
using Webapp.Services;
using Webapp.Settings;

namespace Webapp
{
    public class Startup
    {
        internal const string CorsPolicy = "KickermatCorsPolicy";
        internal const string BackendUrl = "http://localhost:5001"; // No trailing slash!
        internal const string FrontendProxyUrl = "http://localhost:4200";
        private const string SignalrBasePath = "/signalr";
        private static readonly string[] CorsUrls = { BackendUrl, FrontendProxyUrl };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(CorsUrls)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Angular frontend
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Services
            services.AddSingleton<ParameterService>();
            services.AddKickerServices<DalsaCamera, CameraCalibration, ImageProcessor, Communication.Communication>();
            services.ConfigureKicker<DalsaSettings, CalibrationSettings,
                ImageProcessorSettings, CommunicationSettings>(Configuration);

            // RouteConstraints
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("videoSourceType", typeof(VideoSourceTypeRouteConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure Cors
            app.UseCors(CorsPolicy);

            // Configure SignalR hubs
            app.UseSignalR(route =>
            {
                route.MapHub<CalibrationHub>(SignalrBasePath + "/calibration");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configure proxy to Angular frontend if in development mode
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    if (env.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    }
                });
            }
            else
            {
                // Use internal SinglePageApp mechanism to start Angular in production mode
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseAngularCliServer("start");
                });
            }
        }
    }
}
