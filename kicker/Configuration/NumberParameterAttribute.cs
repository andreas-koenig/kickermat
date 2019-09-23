using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public class NumberParameterAttribute : KickerParameterAttribute
    {
        public new double Value { get => (double)base.Value; set => base.Value = value; }
        public new double DefaultValue { get; protected set; }
        public double Min { get; protected set; }
        public double Max { get; protected set; }
        public double Step { get; protected set; }

        public NumberParameterAttribute(string name, string description, double defaultValue,
            double min, double max, double step) : base(name, description)
        {
            DefaultValue = defaultValue;
            Min = min;
            Max = max;
            Step = step;
            Value = default;
        }
    }
}
