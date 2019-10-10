using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConfigurableOptionsAttribute : Attribute
    {
        public ConfigurableOptionsAttribute(Type optionsType)
        {
            OptionsType = optionsType;
        }

        public Type OptionsType { get; protected set; }
    }
}
