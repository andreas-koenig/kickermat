using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class KickerOptionsAttribute : Attribute
    {
        public string[] Path { get; private set; }

        public KickerOptionsAttribute(string[] path)
        {
            Path = path;
        }
    }
}
