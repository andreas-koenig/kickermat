using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Settings;
using Api.Settings.Parameter;

namespace Webapp.Services
{
    public class SettingsService
    {
        private readonly PlayerService _playerService;
        private readonly IServiceProvider _services;

        private readonly Dictionary<string, Type> _settingsDict;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions();

        public SettingsService(PlayerService playerService, IServiceProvider services)
        {
            _playerService = playerService;
            _services = services;

            PlayerSettings = CollectPlayerSettings();
            _settingsDict = CollectSettings();
            _jsonOptions.PropertyNameCaseInsensitive = true;
        }

        public Dictionary<string, IEnumerable<IWriteable>> PlayerSettings { get; private set; }

        public object UpdateParameter(string settingsName, string paramName, object value)
        {
            if (!_settingsDict.TryGetValue(settingsName, out var writeableType))
            {
                throw new KickermatException($"There are no settings with the name {settingsName}");
            }

            var writeable = _services.GetService(writeableType) as IWriteable;
            var properties = writeable.ValueObject
                .GetType()
                .GetProperties()
                .ToList();

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttributes<BaseParameterAttribute>(true)
                        .Where(attr => attr.Name.Equals(paramName))
                        .FirstOrDefault();

                if (attr == null)
                {
                    continue; // Not the right parameter
                }

                try
                {
                    var val = JsonSerializer.Deserialize(
                        ((JsonElement)value).GetRawText(), prop.PropertyType, _jsonOptions);
                    writeable.Update(changes => prop.SetValue(changes, val));
                    return value;
                }
                catch (Exception ex)
                {
                    // TODO: Log exception

                    var oldValue = prop.GetValue(writeable.ValueObject);
                    throw new UpdateSettingsException(
                        @$"Failed to update {settingsName}.{paramName}", oldValue);
                }
            }

            throw new UpdateSettingsException(
                $"There is no parameter {settingsName}.{paramName}", null);
        }

        public IEnumerable<IWriteable> GetSettings(Type type)
        {
            var settings = new List<IWriteable>();

            // Only one constructor can exist (demanded by ASP.NET Core DI mechanism)
            var constructor = type.GetConstructors().FirstOrDefault();
            if (constructor == default)
            {
                return settings;
            }

            foreach (var param in constructor.GetParameters())
            {
                // Add to list if param is of type IWriteable<TOptions>
                if (IsIWriteable(param))
                {
                    var setting = _services.GetService(param.ParameterType) as IWriteable;
                    settings.Add(setting);
                }

                // Call this method recursively on the type
                else
                {
                    settings.AddRange(GetSettings(param.ParameterType));
                }
            }

            return settings;
        }

        private static bool IsIWriteable(ParameterInfo param)
        {
            var isIWriteable = param.ParameterType
                .GetGenericTypeDefinition()
                .Equals(typeof(IWriteable<>));

            var typeParamIsISettingsImpl = param.ParameterType
                .GetGenericArguments()
                .First()
                .GetInterfaces()
                .Any(i => i.Equals(typeof(ISettings)));

            return isIWriteable && typeParamIsISettingsImpl;
        }

        private Dictionary<string, IEnumerable<IWriteable>> CollectPlayerSettings()
        {
            var settingsDict = new Dictionary<string, IEnumerable<IWriteable>>();
            foreach (var player in _playerService.Players)
            {
                settingsDict.Add(player.Key, GetSettings(player.Value.GetType()));
            }

            return settingsDict;
        }

        private Dictionary<string, Type> CollectSettings()
        {
            var settingsDict = new Dictionary<string, Type>();

            AppDomain.CurrentDomain.GetAssemblies()
                .ToList()
                .ForEach(asm => asm.GetTypes()
                    .ToList()
                    .ForEach(type =>
                    {
                        if (type.GetInterfaces().Contains(typeof(ISettings)))
                        {
                            var name = (Activator.CreateInstance(type) as ISettings).Name;
                            var iWriteableType = typeof(IWriteable<>).MakeGenericType(type);
                            settingsDict.Add(name, iWriteableType);
                        }
                    }));

            return settingsDict;
        }

        private IWriteable FindSettings(string settingsName)
        {
            return _services.GetService(_settingsDict[settingsName]) as IWriteable;
        }
    }
}
