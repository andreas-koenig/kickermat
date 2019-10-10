﻿using Communication.KickerControl;
using ImageProcessing;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoSource.Dalsa;
using Webapp.Hubs;
using Webapp.Settings;

namespace Webapp
{
    public class Startup
    {
        internal const string Url = "http://localhost:5001/";
        internal const string ProxyUrl = "http://localhost:4200/";
        private const string SignalrBasePath = "/signalr";
        private static readonly string[] CorsUrls = { Url, ProxyUrl };

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
            services.AddKickerServices<DalsaCamera, CameraCalibration, ImageProcessor, KickerControl>();
            services.ConfigureKicker<DalsaSettings, CalibrationSettings,
                ImageProcessorSettings, KickerControlSettings>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            // Configure Cors
            app.UseCors(policy =>
            {
                policy.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(CorsUrls);
            });
        }
    }
}
