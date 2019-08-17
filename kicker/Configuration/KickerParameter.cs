using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public class KickerParameter<T> : IKickerParameter
    {
        public string Name { get; }
        public T Value { get; }
        public T Default { get; }
        public T Min { get; }
        public T Max { get; }
        public string Description { get; }

        public KickerParameter(string name, T value, T defaultValue, string help, T min = default,
            T max = default)
        {
            Name = name;
            Value = value;
            Default = defaultValue;
            Min = min;
            Max = max;
            Description = help;
        }

        public override string ToString()
        {
            return String.Format(
                "[Name: {0}, Description: {1}, Default: {2}, Value: {3}, Min: {4}, Max: {5}]",
                Name, Description, Default, Value, Min, Max);
        }
    }
}
