using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Settings.Parameter
{
    /// <summary>
    /// A parameter for a boolean value.
    /// </summary>
    public class BooleanParameterAttribute : BaseParameterAttribute
    {
        public BooleanParameterAttribute(string name, string description, bool value,
            bool defaultValue)
            : base(name, description)
        {
            Value = value;
            DefaultValue = defaultValue;
        }

        public new bool Value { get; set; }

        public new bool DefaultValue { get; set; }
    }
}
