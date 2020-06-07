using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Configuration;

namespace Webapp.Services
{
    public class SettingsService
    {
        private readonly KickermatService _kickermatService;
        private readonly IServiceProvider _services;

        public SettingsService(KickermatService kickermatService, IServiceProvider services)
        {
            _kickermatService = kickermatService;
            _services = services;

            PlayerSettings = CollectPlayerSettings();
        }

        public Dictionary<string, IEnumerable<IWriteable>> PlayerSettings { get; private set; }

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
            foreach (var player in _kickermatService.Players)
            {
                settingsDict.Add(player.Key, GetSettings(player.Value));
            }

            return settingsDict;
        }
    }
}
