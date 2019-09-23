using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConfigurableOptionsAttribute : Attribute
    {
        public Type OptionsType { get; protected set; }

        public ConfigurableOptionsAttribute(Type optionsType)
        {
            OptionsType = optionsType;
        }
    }
}
