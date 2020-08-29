using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Settings.Parameter
{
    /// <summary>
    /// A number parameter. Make sure that you only annotate double properties.
    /// </summary>
    public class NumberParameterAttribute : BaseParameterAttribute
    {
        public NumberParameterAttribute(string name, string description, double defaultValue,
            double min, double max, double step)
            : base(name, description)
        {
            DefaultValue = defaultValue;
            Min = min;
            Max = max;
            Step = step;
            Value = default;
        }

        public new double Value { get => (double)base.Value; set => base.Value = value; }

        public new double DefaultValue { get; protected set; }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public double Min { get; protected set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public double Max { get; protected set; }

        /// <summary>
        /// The step size used to jump a certain interval when the value range is large.
        /// </summary>
        public double Step { get; protected set; }
    }
}

