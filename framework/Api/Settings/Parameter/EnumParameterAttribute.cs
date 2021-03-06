﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Settings.Parameter
{
    /// <summary>
    /// An enum parameter.
    /// </summary>
    public class EnumParameterAttribute : BaseParameterAttribute
    {
        public EnumParameterAttribute(string name, string description, Type enumType)
            : base(name, description)
        {
            Options = new List<object>();
            Enum.GetValues(enumType)
                .Cast<int>()
                .ToList()
                .ForEach(key => Options.Add(new
                {
                    Key = key,
                    Value = Enum.GetName(enumType, key),
                }));
        }

        public new int Value { get => (int)base.Value; set => base.Value = value; }

        public new int DefaultValue { get; protected set; }

        /// <summary>
        /// The options of the enumeration.
        /// </summary>
        public List<object> Options { get; }

        // Dictionary<int, string> and KeyValuePair<int, string> are not properly serialized to
        // Json with System.Text.Json
    }
}
