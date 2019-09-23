using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Configuration
{
    public interface IWritableOptions
    {
        object ValueObject { get; }
        void Update(Action<object> applyChanges);

        void RegisterChangeListener(Action onChange);
    }

    public interface IWritableOptions<out T>
        : IWritableOptions, IOptionsSnapshot<T> where T : class, new()
    {
        new T Value { get; }
        void Update(Action<T> applyChanges);
    }
}
