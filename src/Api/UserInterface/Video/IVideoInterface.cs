using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Camera;

namespace Api.UserInterface.Video
{
    public interface IVideoInterface : IObservable<IFrame>
    {
        /// <summary>
        /// The different channels that an <see cref="IVideoInterface"/> provides.
        /// </summary>
        public IEnumerable<IVideoChannel> Channels { get; }

        /// <summary>
        /// The channel that is currently selected.
        /// </summary>
        public IVideoChannel Channel { get; set; }
    }
}
