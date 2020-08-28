using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Api;
using Api.Periphery;
using Api.Player;
using Api.Settings;
using Api.Settings.Parameter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kickermat.Settings
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string[] _projects = new string[]
        {
            "Player", "Video", "Motor", "Api",
        };

        // Make sure all assemblies referenced by this project are loaded
        // so no type is missing when searching for interfaces etc.
        private static readonly IEnumerable<Assembly> _assemblies = _projects.ToList()
            .Select(project => Assembly.Load(project))
            .Append(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Register all classes implementing <see cref="IKickermatPlayer" /> as a singleton
        /// service that can be injected into other classes. An implementation can be accessed
        /// like so: <code>services.GetService(typeof(KickermatPlayerImpl))</code>
        /// </summary>
        public static IServiceCollection RegisterKickermatPlayers(
            this IServiceCollection services, out IEnumerable<Type> playerTypes)
        {
            var players = new Dictionary<string, Type>();

            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.GetInterfaces().Contains(typeof(IKickermatPlayer)))
                    {
                        continue;
                    }

                    players.Add(type.FullName, type);
                }
            }

            players.Values.ToList().ForEach(playerType => services.AddSingleton(playerType));

            playerTypes = players.Values;

            var names = string.Join('\n', playerTypes.Select(p => $"\t{p.FullName}"));
            Console.WriteLine($"Registered {playerTypes.Count()} players:\n{names}");

            return services;
        }

        public static IServiceCollection RegisterPeripherals(
            this IServiceCollection services, out IEnumerable<Type> peripheralTypes)
        {
            var peripherals = new List<Type>();

            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IPeripheral))
                        && !type.IsInterface
                        && !type.IsAbstract
                        && type.IsClass)
                    {
                        services.AddSingleton(type);
                        peripherals.Add(type);
                    }
                }
            }

            var names = string.Join('\n', peripherals.Select(p => $"\t{p.FullName}"));
            Console.WriteLine($"Registered {peripherals.Count} peripherals:\n{names}");

            peripheralTypes = peripherals;

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

            var registeredTypes = new List<Type>();

            _assemblies.ToList()
                .ForEach(asm => asm.GetTypes()
                    .ToList()
                    .ForEach(type =>
                        {
                            if (type.GetInterfaces().Contains(typeof(ISettings))
                                && type.IsClass
                                && !type.IsAbstract)
                            {
                                CheckDefaultValues(services, type);
                                ConfigureSettingsType(services, type, config, configureMethod);
                                registeredTypes.Add(type);
                            }
                        }));

            var names = string.Join('\n', registeredTypes.Select(t => $"\t{t.FullName}"));
            Console.WriteLine($"Registered {registeredTypes.Count} settings:\n{names}");

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
                var environment = provider.GetService<IHostEnvironment>();

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
            where TOptions : class, ISettings, new()
        {
            services.Configure<TOptions>(section);
            services.AddTransient<IWriteable<TOptions>>(provider =>
            {
                var environment = provider.GetService<IHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<TOptions>>();
                return new Writable<TOptions>(environment, options, section.Path, file);
            });

            return services;
        }

        private static void ValidateOptions(ISettings options)
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
