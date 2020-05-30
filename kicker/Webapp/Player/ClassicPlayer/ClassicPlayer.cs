using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapp.Player.Api;

namespace Webapp.Player.ClassicPlayer
{
    [KickermatPlayer(
        "Classic Player",
        "This player uses the image processing mechanism and GameController provided by the framework",
        new string[] { "Dominik Hagenauer", "Andreas König" })
    ]
    public class ClassicPlayer : IKickermatPlayer
    {
        public void PauseGame()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void ResumeGame()
        {
            throw new NotImplementedException();
        }

        public void ShutDown()
        {
            throw new NotImplementedException();
        }

        public void Startup()
        {
            throw new NotImplementedException();
        }
    }
}
