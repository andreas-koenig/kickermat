using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;

namespace Webapp
{
    public interface IKickermatPlayer
    {
        void Startup();

        void Play();

        void PauseGame();

        void ResumeGame();

        void ShutDown();
    }
}
