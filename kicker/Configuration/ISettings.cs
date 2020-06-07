using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public interface ISettings
    {
        string Name { get; }
    }
}
