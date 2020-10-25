using System;
using System.Collections.Generic;
using System.Text;

namespace Api
{
    /// <summary>
    /// For all resources/entities that need a human-friendly name.
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// A human-friendly name. It can contain spaces.
        /// </summary>
        public string Name { get; }
    }
}

