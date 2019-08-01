using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    interface IVideoSource
    {
        void StartAcquisition();
        void StopAcquisition();
    }
}
