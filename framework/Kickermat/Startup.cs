﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Motor;
using Video.Dalsa;
using Kickermat.Controllers;
using Kickermat.Hubs;
using Kickermat.Services;
using Kickermat.Services.Game;
using Kickermat.Services.Settings;
using Kickermat.Settings;

namespace Kickermat
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
            services.AddControllers(options => options.EnableEndpointRouting = true)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            // Add Angular frontend
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Services
            services.ConfigureKickermatSettings(Configuration)
                .RegisterKickermatPlayers(out var players)
                .AddSingleton((services) => new PlayerService(services, players))
                .RegisterPeripherals(out var peripheralTypes)
                .AddSingleton((services) => new PeripheralsService(services, peripheralTypes))
                .AddSingleton<GameService>()
                .AddSingleton<SettingsService>()
                .AddSingleton<MotorController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(); // Call before useRouting()
            app.UseSpaStaticFiles();

            app.UseRouting();

            // Configure CORS (before middleware using CORS, such as useEndpoints())
            app.UseCors(CorsPolicy);

            app.UseEndpoints(config =>
            {
                config.MapHub<CalibrationHub>(SignalrBasePath + "/calibration");
                config.MapDefaultControllerRoute();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configure proxy to Angular frontend if in development mode
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "Webapp";
                    if (env.IsDevelopment())
                    {
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    }
                });
            }
            else if (env.IsStaging() || env.IsProduction())
            {
                // Use internal SinglePageApp mechanism to start Angular in production mode
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseAngularCliServer("start");
                });
            }

            // Warm up services (configure cameras and start motor calibration)
            app.ApplicationServices.GetService(typeof(PeripheralsService));
            app.ApplicationServices.GetService(typeof(PlayerService));
            app.ApplicationServices.GetService(typeof(MotorController));
        }
    }
}
