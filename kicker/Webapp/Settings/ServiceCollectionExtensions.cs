using System;
using Communication;
using Configuration;
using ImageProcessing;
using ImageProcessing.Calibration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VideoSource;

namespace Webapp.Settings
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add an OptionsMonitor whose updates are written back to its underlying JSON file.
        /// </summary>
        /// <typeparam name="TOptions">The type of the options.</typeparam>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <param name="section">The configuration section within the JSON file.</param>
        /// <param name="file">The JSON file.</param>
        /// <returns>The IServiceCollection.</returns>
        public static IServiceCollection ConfigureWritable<TOptions>(
            this IServiceCollection services,
            IConfigurationSection section,
            string file = "appsettings.json")
            where TOptions : class, new()
        {
            services.Configure<TOptions>(section);
            services.AddTransient<IWritableOptions<TOptions>>(provider =>
            {
                var environment = provider.GetService<IHostingEnvironment>();
                var options = provider.GetService<IOptionsMonitor<TOptions>>();
                return new WritableOptions<TOptions>(environment, options, section.Path, file);
            });

            return services;
        }

        /// <summary>
        /// Add the implementations for the 4 different components of the kicker.
        /// </summary>
        /// <typeparam name="TVideoSource">The type of the implementation for IVideoSource.</typeparam>
        /// <typeparam name="TCameraCalibration">The type of the implementation for ICameraCalibration.</typeparam>
        /// <typeparam name="TImageProcessor">The type of the implementation for IImageProcessor.</typeparam>
        /// <typeparam name="TKickerControl">The type of the implementation for IKickerControl.</typeparam>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <returns>The IServiceCollection.</returns>
        public static IServiceCollection AddKickerServices<TVideoSource, TCameraCalibration,
            TImageProcessor, TKickerControl>(
            this IServiceCollection services)
            where TVideoSource : class, IVideoSource
            where TCameraCalibration : class, ICameraCalibration
            where TImageProcessor : class, IImageProcessor
            where TKickerControl : class, ICommunication
        {
            services.AddSingleton<IVideoSource, TVideoSource>();
            services.AddSingleton<ICameraCalibration, TCameraCalibration>();
            services.AddSingleton<IImageProcessor, TImageProcessor>();
            services.AddSingleton<ICommunication, TKickerControl>();

            return services;
        }

        /// <summary>
        /// Configure the Kicker by registering the settings needed by the implementations of the
        /// Kicker components.
        /// </summary>
        /// <typeparam name="TVideoSourceSettings">The type of the settings for the IVideoSource.</typeparam>
        /// <typeparam name="TCameraCalibrationSettings">The type of the settings for the ICameraCalibration.</typeparam>
        /// <typeparam name="TImageProcessorSettings">The type of the settings for the IImageProcessor.</typeparam>
        /// <typeparam name="TKickerControlSettings">The type of the settings for the IKickerControl.</typeparam>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <param name="config">The configuration instance containing the appsettings.json file.</param>
        public static void ConfigureKicker<TVideoSourceSettings, TCameraCalibrationSettings,
            TImageProcessorSettings, TKickerControlSettings>(
            this IServiceCollection services, IConfiguration config)
            where TVideoSourceSettings : class, new()
            where TCameraCalibrationSettings : class, new()
            where TImageProcessorSettings : class, new()
            where TKickerControlSettings : class, new()
        {
            services.ConfigureKickerOptions<TVideoSourceSettings>(config);
            services.ConfigureKickerOptions<TCameraCalibrationSettings>(config);
            services.ConfigureKickerOptions<TImageProcessorSettings>(config);
            services.ConfigureKickerOptions<TKickerControlSettings>(config);
        }

        private static void ConfigureKickerOptions<TKickerOptions>(
            this IServiceCollection services, IConfiguration config)
            where TKickerOptions : class, new()
        {
            var optionsAttrs = typeof(TKickerOptions)
                .GetCustomAttributes(typeof(KickerOptionsAttribute), false);

            if (optionsAttrs == null || optionsAttrs.Length == 0)
            {
                var msg = string.Format(
                    "The Options class {0} has to be configured via the KickerOptionsAttribute!",
                    typeof(TKickerOptions).FullName);
                throw new Exception(msg);
            }

            var kickerOptionsAttr = (KickerOptionsAttribute)optionsAttrs[0];
            var key = string.Join(":", kickerOptionsAttr.Path);
            services.ConfigureWritable<TKickerOptions>(config.GetSection(key));
            services.PostConfigure<TKickerOptions>(o => ValidateOptions<TKickerOptions>(o));
        }

        private static void ValidateOptions<TKickerOptions>(TKickerOptions options)
        {
            foreach (var property in options.GetType().GetProperties())
            {
                // Check if configurable in frontend
                var attrs = property.GetCustomAttributes(typeof(BaseParameterAttribute), true);
                if (attrs?.Length == 0)
                {
                    continue;
                }

                // Check if property.datatype == attribute.datatype
                var parameterType = ((BaseParameterAttribute)attrs[0]).Value.GetType();
                var propertyType = property.PropertyType;
                if (!parameterType.Equals(propertyType))
                {
                    var msg = string.Format(
                        "Datatype of ParameterAttribute does not match " +
                        "datatype of property {0} of class {1}: {2} != {3}", property.Name,
                        options.GetType().FullName, parameterType.Name, propertyType.Name);
                    throw new Exception(msg);
                }

                // Set value to default if it is null
                if (property.GetValue(options) == null)
                {
                    var defaultValue = ((BaseParameterAttribute)attrs[0]).DefaultValue;
                    property.SetValue(options, defaultValue);
                }
            }
        }
    }
}
