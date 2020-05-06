using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public class ColorRangeParameterAttribute : BaseParameterAttribute
    {
        public ColorRangeParameterAttribute(string name, string description, int hueLower,
            int saturationLower, int valueLower, int hueUpper, int saturationUpper, int valueUpper)
            : base(name, description)
        {
            DefaultValue = new ColorRange(hueLower, saturationLower, valueLower, hueUpper,
                saturationUpper, valueUpper);
            Value = DefaultValue;
        }

        public new ColorRange Value { get => (ColorRange)base.Value; set => base.Value = value; }

        public new ColorRange DefaultValue { get; protected set; }
    }

    public class ColorRange
    {
        public ColorRange() { }

        public ColorRange(HsvColor upper, HsvColor lower)
        {
            Lower = lower;
            Upper = upper;
        }

        public ColorRange(int hueLower, int saturationLower, int valueLower, int hueUpper,
            int saturationUpper, int valueUpper)
        {
            Lower = new HsvColor(hueLower, saturationLower, valueLower);
            Upper = new HsvColor(hueUpper, saturationUpper, valueUpper);
        }

        public HsvColor Lower { get; set; }

        public HsvColor Upper { get; set; }
    }

    public class HsvColor
    {
        public HsvColor(int hue, int saturation, int value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        public int Hue { get; set; }

        public int Value { get; set; }

        public int Saturation { get; set; }
    }
}
