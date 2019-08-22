using ImageProcessing.Calibration;
using ImageProcessing.Preprocessing;
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
        private const string CORS_POLICY = "KickermatCorsPolicy";
        internal const string URL = "http://localhost:5001";
        internal const string PROXY_URL = "http://localhost:4200";
        private static readonly string[] CORS_URLS = { URL, PROXY_URL };
        private const string SIGNALR_BASE_PATH = "/signalr";
        private readonly IWritableOptions<KickerSettings> _settings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Cors Policy to allow different origins
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY, policy =>
                {
                    policy.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(CORS_URLS);
                });
            });

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add Angular frontend
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Kicker services
            services.AddSingleton<IVideoSource, DalsaVideoSource>();
            services.AddSingleton<ICameraCalibration, CameraCalibration>();
            services.AddSingleton<IPreprocessor, Preprocessor>();
            services.AddSingleton<ICameraConnectionHandler, CameraConnectionHandler>();

            // Persistent configuration
            services.ConfigureWritable<KickerSettings>(Configuration.GetSection("MySection"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(CORS_POLICY);

                var wsOptions = new WebSocketOptions();
                wsOptions.AllowedOrigins.Add(URL);
                wsOptions.AllowedOrigins.Add(PROXY_URL);
                app.UseWebSockets(wsOptions);

                // Configure SignalR hubs
                app.UseSignalR(route =>
                {
                    route.MapHub<CameraHub>(SIGNALR_BASE_PATH + "/camera");
                    route.MapHub<CalibrationHub>(SIGNALR_BASE_PATH + "/calibration");
                });

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

            app.UseMvc();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
        }
    }
}
