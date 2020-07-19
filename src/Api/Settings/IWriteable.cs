using System;
using Microsoft.Extensions.Options;

namespace Api.Settings
{
    public interface IWriteable
    {
        object ValueObject { get; }

        void Update(Action<object> applyChanges);

        void RegisterChangeListener(Action onChange);
    }

    public interface IWriteable<out T>
        : IWriteable, IOptionsSnapshot<T>
        where T : class, new()
    {
        new T Value { get; }

        void Update(Action<T> applyChanges);
    }
}
