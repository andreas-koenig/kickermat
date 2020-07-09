using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Video;

namespace Api.UserInterface.Video
{
    [UserInterface(UserInterfaceType.Video)]
    public interface IVideoInterface<T>
    {
        IVideoSource<T> GetVideoSource();
    }
}
