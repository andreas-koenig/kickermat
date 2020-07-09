using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Api.Settings.Parameter
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class BaseParameterAttribute : Attribute
    {
        public BaseParameterAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        [JsonIgnore]
        public override object TypeId => base.TypeId;

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        [JsonIgnore]
        public object Value { get; set; }

        [JsonIgnore]
        public object DefaultValue { get; protected set; }
    }
}
