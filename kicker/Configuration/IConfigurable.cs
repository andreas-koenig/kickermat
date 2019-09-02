using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public interface IConfigurable
    {
        void ApplyOptions();
    }

    public interface IConfigurable<T> : IConfigurable where T : class, new()
    {
        IWritableOptions<T> Options { get; set; }
    }
}
