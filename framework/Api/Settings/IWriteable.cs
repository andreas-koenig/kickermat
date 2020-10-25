using System;
using Microsoft.Extensions.Options;

namespace Api.Settings
{
    /// <summary>
    /// Base interface for <see cref="IWriteable{T}"/> which is only needed for reflection.
    /// </summary>
    public interface IWriteable
    {
        object ValueObject { get; }

        void Update(Action<object> applyChanges);

        /// <summary>
        /// Registers an change listener that for notification about external updates to the
        /// wrapped settings instance.
        /// </summary>
        /// <param name="onChange">The callback action.</param>
        void RegisterChangeListener(Action onChange);
    }

    /// <summary>
    /// Serves as a wrapper for the <see cref="ISettings"/> interface. It provides functionality
    /// to read and update the wrapped settings value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriteable<out T>
        : IWriteable, IOptionsSnapshot<T>
        where T : class, ISettings, new()
    {
        /// <summary>
        /// The current value the wrapped settings instance.
        /// </summary>
        new T Value { get; }

        /// <summary>
        /// Updates the wrapped settings instance and writes back the changes to the settings file.
        /// </summary>
        /// <param name="applyChanges">The update action.</param>
        void Update(Action<T> applyChanges);
    }
}

