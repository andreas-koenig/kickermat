using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Configuration
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        new T Value { get; }
        void Update(Action<T> applyChanges);
    }
}
