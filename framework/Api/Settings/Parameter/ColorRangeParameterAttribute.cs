using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Settings.Parameter
{
    /// <summary>
    /// A parameter for an HSV color range which can be used for classic image processing.
    /// </summary>
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

    /// <summary>
    /// An HSV color range with an upper and a lower bound.
    /// </summary>
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

        /// <summary>
        /// The lower bound.
        /// </summary>
        public HsvColor Lower { get; set; }

        /// <summary>
        /// The upper bound.
        /// </summary>
        public HsvColor Upper { get; set; }
    }

    /// <summary>
    /// A color value in the HSV color space.
    /// </summary>
    public class HsvColor
    {
        public HsvColor() { }

        public HsvColor(int hue, int saturation, int value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        /// <summary>
        /// The hue.
        /// </summary>
        public int Hue { get; set; }

        /// <summary>
        /// The value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The saturation.
        /// </summary>
        public int Saturation { get; set; }
    }
}

