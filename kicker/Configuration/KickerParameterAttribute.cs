using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class KickerParameterAttribute : Attribute
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public KickerDataType DataType { get; protected set; }
        public object Value { get; set; }
        public object DefaultValue { get; protected set; }
        public object Min { get; protected set; }
        public object Max { get; protected set; }
        public object Step { get; protected set; }

        public KickerParameterAttribute(string name, string description, KickerDataType dataType,
            object defaultValue, object min = null, object max = null, object step = null)
        {
            Name = name;
            Description = description;
            DataType = dataType;
            DefaultValue = defaultValue;
            Min = min;
            Max = max;
            Step = step;
        }
    }
}
