using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Api.Settings.Parameter
{
    /// <summary>
    /// A base attribute for all parameters. These attributes are used to enrich the properties
    /// in <see cref="ISettings"/> implementations with human-friendly information to be displayed
    /// in the webapp. Only properties annotated with this attribute can be adjusted in the webapp.
    /// </summary>
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

        /// <summary>
        /// The human-friendly name of the parameter.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// A description explaining the effect of the parameter and possibly how to adjust it.
        /// </summary>
        public string Description { get; protected set; }

        [JsonIgnore]
        public object Value { get; set; }

        [JsonIgnore]
        public object DefaultValue { get; protected set; }
    }
}
