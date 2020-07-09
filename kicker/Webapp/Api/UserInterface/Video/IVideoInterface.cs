using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapp.Api.Video;

namespace Webapp.Api.UserInterface.Video
{
    [UserInterface(UserInterfaceType.Video)]
    public interface IVideoInterface<T>
    {
        IVideoSource<T> GetVideoSource();
    }
}
