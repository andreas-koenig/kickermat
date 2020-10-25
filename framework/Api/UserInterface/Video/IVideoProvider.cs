using System;
using System.Collections.Generic;
using System.Text;

namespace Api.UserInterface.Video
{
    [UserInterface(UserInterfaceType.Video)]
    public interface IVideoProvider
    {
        public IVideoInterface VideoInterface { get; }
    }
}
