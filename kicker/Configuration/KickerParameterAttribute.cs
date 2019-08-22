using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class KickerParameterAttribute : Attribute
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public object Value { get; set; }
        

        public KickerParameterAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
