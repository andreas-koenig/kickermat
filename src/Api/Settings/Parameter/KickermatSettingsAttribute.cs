using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Settings.Parameter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class KickermatSettingsAttribute : Attribute
    {
        public KickermatSettingsAttribute(Type kickerComponentImpl, params string[] path)
        {
            Path = path;
            KickerComponentType = kickerComponentImpl;
        }

        public string[] Path { get; private set; }

        public Type KickerComponentType { get; private set; }
    }
}
