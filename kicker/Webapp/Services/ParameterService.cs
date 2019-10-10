﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;

namespace Webapp.Services
{
    public class ParameterService
    {
        private IServiceProvider _services;

        public ParameterService(IServiceProvider services)
        {
            _services = services;
            KickerOptions = CollectKickerOptions();
        }

        public Dictionary<Type, IWritableOptions> KickerOptions { get; private set; }

        private Dictionary<Type, IWritableOptions> CollectKickerOptions()
        {
            // Collect all KickerOptionsAttributes and their annotated settings classes
            var kickerOptionsAttrs = new Dictionary<KickerOptionsAttribute, Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attrs = type.GetCustomAttributes(typeof(KickerOptionsAttribute), false);
                    if (attrs.Length == 1)
                    {
                        kickerOptionsAttrs.Add(attrs[0] as KickerOptionsAttribute, type);
                    }
                }
            }

            // Add the WritableOptions instance to every kickerComponentType
            var kickerOptions = new Dictionary<Type, IWritableOptions>();
            foreach (var attr in kickerOptionsAttrs.Keys)
            {
                var kickerComponentType = attr.KickerComponentType;

                // Throw error in case of multiple settings classes for a single component
                if (kickerOptions.ContainsKey(kickerComponentType))
                {
                    var componentName = kickerComponentType.FullName;
                    var settingsClass = kickerOptions[kickerComponentType]
                        .GetType().GetGenericArguments()[0].FullName;
                    var newSettingsClass = kickerOptionsAttrs[attr].FullName;
                    throw new Exception($"The kicker component {componentName} is already " +
                        $"associated with the settings class {settingsClass} and therefore " +
                        $"cannot also be associated with the settings class {newSettingsClass}!");
                }

                // Get WritableOptions instance
                var settingsType = kickerOptionsAttrs[attr];
                var writableOptionsType = typeof(IWritableOptions<>)
                    .MakeGenericType(new Type[] { settingsType });
                var writableOptions = (IWritableOptions)_services.GetService(writableOptionsType);

                kickerOptions[attr.KickerComponentType] = writableOptions;
            }

            return kickerOptions;
        }
    }
}