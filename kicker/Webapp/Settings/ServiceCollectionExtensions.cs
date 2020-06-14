using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Configuration;
using Configuration.Parameter;
using ImageProcessing;
using ImageProcessing.Calibration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VideoSource;
using Webapp.Player;
using Webapp.Player.Api;

namespace Webapp.Settings
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all classes implementing <see cref="IKickermatPlayer" /> as a singleton
        /// service that can be injected into other classes. An implementation can be accessed
        /// like so: <code>services.GetService(typeof(KickermatPlayerImpl))</code>
        /// </summary>
        public static IServiceCollection RegisterKickermatPlayers(this IServiceCollection services)
        {
            var players = new Dictionary<string, Type>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IKickermatPlayer)))
                {
                    var playerAttr = type.GetCustomAttribute<KickermatPlayerAttribute>();
                    if (playerAttr == null)
                    {
                        throw new KickermatException(
                            $@"Please register the IKickermatPlayer implementation '{type.FullName}'
                            by using annotating it with {typeof(KickermatPlayerAttribute).FullName}");
                    }

                    string playerName = playerAttr.Name;
                    if (players.ContainsKey(playerName))
                    {
                        throw new KickermatException(
                            $@"The IKickermatPlayer implementations {type.FullName} and
                            {players[playerName].FullName} are both registered under the name
                            {playerName}. Please choose distinct names!");
                    }

                    players.Add(playerAttr.Name, type);
                }
            }

            foreach (var playerType in players.Values)
            {
                services.AddSingleton(playerType);
            }

            return services;
        }

        /// <summary>
        /// Register all implementations of <see cref="ISettings" /> as a service wrapped into
        /// <see cref="IWriteable{TSettings}" />. The instance can be injected into a class
        /// by adding the following parameter to its constructor:
        /// <code>
        /// public class Example
        /// {
        ///     public Example(IWritable{TSettings} settings)
        ///     {
        ///         // ...
        ///     }
        /// }
        /// </code>
        /// </summary>
        public static IServiceCollection ConfigureKickermatSettings(
            this IServiceCollection services,
            IConfiguration config)
        {
            var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethods()
                .First(m => m.Name.Equals(nameof(OptionsConfigurationServiceCollectionExtensions.Configure)));

            var tmpSettingsDict = new Dictionary<string, Type>();

            AppDomain.CurrentDomain.GetAssemblies()
                .ToList()
                .ForEach(asm => asm.GetTypes()
                    .ToList()
                    .ForEach(type =>
                        {
                            if (type.GetInterfaces().Contains(typeof(ISettings)))
                            {
                                CheckDefaultValues(services, type);
                                ConfigureSettingsType(services, type, config, configureMethod);
                            }
                        }));

            return services;
        }

        private static void CheckDefaultValues(this IServiceCollection services, Type settingsType)
        {
            var settings = Activator.CreateInstance(settingsType);

            settingsType.GetProperties()
                .ToList()
                .ForEach(prop =>
                {
                    var value = prop.GetValue(settings);
                    if (value == default || value == null)
                    {
                        throw new KickermatException(@$"Please define a default value for the
                            property '{prop.Name}' of the 'ISettings' implementation
                            '{settingsType.FullName}'");
                    }
                });
        }

        private static void ConfigureSettingsType(
            this IServiceCollection services,
            Type settingsType,
            IConfiguration config,
            MethodInfo configureMethod)
        {
            var name = (Activator.CreateInstance(settingsType) as ISettings).Name;
            var iWriteableType = typeof(IWriteable<>).MakeGenericType(settingsType);

            // Call services.Configure<TOptions>(section) with settingsType as TOptions
            configureMethod
                .MakeGenericMethod(settingsType)
                .Invoke(null, new object[] { services, config.GetSection(name) });

            // Register IWriteable<TOptions> as transient service
            services.AddTransient(iWriteableType, provider =>
            {
                var writableType = typeof(Writable<>).MakeGenericType(settingsType);
                var monitorType = typeof(IOptionsMonitor<>).MakeGenericType(settingsType);
                var optionsMonitor = provider.GetService(monitorType);
                var environment = provider.GetService<IHostingEnvironment>();

                var writableOptions = Activator.CreateInstance(
                    writableType, environment, optionsMonitor, name, "appsettings.json");

                return writableOptions;
            });
        }

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
            services.AddTransient<IWriteable<TOptions>>(provider =>
            {
                var environment = provider.GetService<IHostingEnvironment>();
                var options = provider.GetService<IOptionsMonitor<TOptions>>();
                return new Writable<TOptions>(environment, options, section.Path, file);
            });

            return services;
        }

        /// <summary>
        /// Add the implementations for the 4 different components of the kicker.
        /// </summary>
        /// <typeparam name="TVideoSource">The type of the implementation for IVideoSource.</typeparam>
        /// <typeparam name="TCameraCalibration">The type of the implementation for ICameraCalibration.</typeparam>
        /// <typeparam name="TImageProcessor">The type of the implementation for IImageProcessor.</typeparam>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <returns>The IServiceCollection.</returns>
        public static IServiceCollection AddKickerServices<TVideoSource, TCameraCalibration,
            TImageProcessor>(
            this IServiceCollection services)
            where TVideoSource : class, IVideoSource
            where TCameraCalibration : class, ICameraCalibration
            where TImageProcessor : class, IImageProcessor
        {
            services.AddSingleton<IVideoSource, TVideoSource>();
            services.AddSingleton<ICameraCalibration, TCameraCalibration>();
            services.AddSingleton<IImageProcessor, TImageProcessor>();

            return services;
        }

        /// <summary>
        /// Configure the Kicker by registering the settings needed by the implementations of the
        /// Kicker components.
        /// </summary>
        /// <typeparam name="TVideoSourceSettings">The type of the settings for the IVideoSource.</typeparam>
        /// <typeparam name="TCameraCalibrationSettings">The type of the settings for the ICameraCalibration.</typeparam>
        /// <typeparam name="TImageProcessorSettings">The type of the settings for the IImageProcessor.</typeparam>
        /// <param name="services">The IServiceCollection to be extended.</param>
        /// <param name="config">The configuration instance containing the appsettings.json file.</param>
        public static void ConfigureKicker<TVideoSourceSettings, TCameraCalibrationSettings,
            TImageProcessorSettings>(
            this IServiceCollection services, IConfiguration config)
            where TVideoSourceSettings : class, new()
            where TCameraCalibrationSettings : class, new()
            where TImageProcessorSettings : class, new()
        {
            services.ConfigureKickerOptions<TVideoSourceSettings>(config);
            services.ConfigureKickerOptions<TCameraCalibrationSettings>(config);
            services.ConfigureKickerOptions<TImageProcessorSettings>(config);
        }

        private static void ConfigureKickerOptions<TKickerOptions>(
            this IServiceCollection services, IConfiguration config)
            where TKickerOptions : class, new()
        {
            var optionsAttrs = typeof(TKickerOptions)
                .GetCustomAttributes(typeof(KickermatSettingsAttribute), false);

            if (optionsAttrs == null || optionsAttrs.Length == 0)
            {
                var msg = string.Format(
                    "The Options class {0} has to be configured via the KickerOptionsAttribute!",
                    typeof(TKickerOptions).FullName);
                throw new Exception(msg);
            }

            var kickerOptionsAttr = (KickermatSettingsAttribute)optionsAttrs[0];
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
