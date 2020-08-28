using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Settings
{
    /// <summary>
    /// Implement this interface to register a settings class with the framework. You can inject
    /// the settings into your player via the <see cref="IWriteable{}"/> interface that wraps your
    /// instance and provides methods for querying and updating the individual parameters.
    /// </summary>
    public interface ISettings : INamed
    {
        // Marker interface
    }
}
