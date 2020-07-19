using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.UserInterface
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class UserInterfaceAttribute : Attribute
    {
        public UserInterfaceAttribute(UserInterfaceType type)
        {
            Type = type;
        }

        public UserInterfaceType Type { get; }
    }
}
