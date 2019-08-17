using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class KickerParameterAttribute : Attribute
    {
        public string Name { get; protected set; }
        public Type ParameterType { get; protected set; }
        public string Description { get; protected set; }
        public object Value { get; protected set; }
        public object DefaultValue { get; protected set; }
        public object Min { get; protected set; }
        public object Max { get; protected set; }

        public KickerParameterAttribute(string name, Type parameterType, string description,
            object value, object defaultValue, object min, object max)
        {
            if (!value.GetType().Equals(parameterType) ||
                !defaultValue.GetType().Equals(parameterType) ||
                !min.GetType().Equals(parameterType) ||
                !max.GetType().Equals(parameterType))
            {
                string msg = String.Format("The parameter {0} has a datatype mismatch!", name);
                throw new Exception(msg);
            }

            Name = name;
            ParameterType = parameterType;
            Description = description;
            Value = value;
            DefaultValue = defaultValue;
            Min = min;
            Max = max;
        }

        public static IEnumerable<IKickerParameter> GetKickerParameters(Type kickerComponentType)
        {
            var attrs = (KickerParameterAttribute[])Attribute
                .GetCustomAttributes(kickerComponentType, typeof(KickerParameterAttribute));

            if (attrs.Length == 0)
            {
                return null;
            }

            var parameters = new IKickerParameter[attrs.Length];

            for (int i = 0; i < attrs.Length; i++)
            {
                var attr = attrs[i];

                Type type = attr.ParameterType;
                Type kickerParamType = typeof(KickerParameter<>).MakeGenericType(type);
                var param = (IKickerParameter) Activator.CreateInstance(kickerParamType,
                    new object[] { attr.Name, attr.Value, attr.DefaultValue, attr.Description,
                        attr.Min, attr.Max});

                parameters[i] = param;
            }

            return parameters;
        }
    }
}
