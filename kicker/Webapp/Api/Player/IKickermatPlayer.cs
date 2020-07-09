using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;

namespace Webapp.Api.Player
{
    public interface IKickermatPlayer
    {
        void Start();

        void Stop();

        void Pause();

        void Resume();
    }
}
