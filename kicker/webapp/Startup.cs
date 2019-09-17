using System.IO;
using Configuration;
using ImageProcessing.BallSearch;
using ImageProcessing.BarSearch;
using ImageProcessing.Calibration;
using ImageProcessing.Preprocessing;
using ImageProcessingNew.BarSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoSource;
using VideoSource.Dalsa;
using Webapp.Hubs;
using Webapp.Settings;

namespace webapp
{
    public class Startup
    {
        internal const string URL = "http://localhost:5001/";
        internal const string PROXY_URL = "http://localhost:4200/";
        private static readonly string[] CORS_URLS = { URL, PROXY_URL };
        private const string SIGNALR_BASE_PATH = "/signalr";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Angular frontend
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Kicker services
            services.AddSingleton<IVideoSource, DalsaCamera>();
            services.AddSingleton<ICameraCalibration, CameraCalibration>();
            services.AddSingleton<IPreprocessor, Preprocessor>();
            services.AddSingleton<IBallSearch, BallSearch>();
            services.AddSingleton<IBarSearch, BarSearch>();

            // Configuration
            var cameraSection = Configuration.GetSection("Camera");
            services.ConfigureWritable<DalsaSettings>(cameraSection.GetSection("Dalsa"));
            services.ConfigureWritable<CalibrationSettings>(cameraSection.GetSection("Calibration"));

            var imgProcSection = Configuration.GetSection("ImageProcessing");
            services.ConfigureWritable<BarSearchSettings>(imgProcSection.GetSection("BarSearch"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure SignalR hubs
            app.UseSignalR(route =>
            {
                route.MapHub<CalibrationHub>(SIGNALR_BASE_PATH + "/calibration");
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

            // Configure Cors
            app.UseCors(policy =>
            {
                policy.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(CORS_URLS);
            });
        }
    }
}
