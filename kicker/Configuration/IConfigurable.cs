using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public interface IConfigurable<T> where T : class, new()
    {
        IWritableOptions<T> Options { get; }
    }
}
