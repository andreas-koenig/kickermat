using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public interface IKickerParameter
    {
        string Name { get; }
        string Description { get; }
    }
}
