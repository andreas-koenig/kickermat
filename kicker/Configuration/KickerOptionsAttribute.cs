using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class KickerOptionsAttribute : Attribute
    {
        public KickerOptionsAttribute(string[] path)
        {
            Path = path;
        }

        public string[] Path { get; private set; }
    }
}
