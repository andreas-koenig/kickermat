﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class KickerOptionsAttribute : Attribute
    {
        public KickerOptionsAttribute(string[] path, Type kickerComponentImpl)
        {
            Path = path;
            KickerComponentType = kickerComponentImpl;
        }

        public string[] Path { get; private set; }

        public Type KickerComponentType { get; private set; }
    }
}
