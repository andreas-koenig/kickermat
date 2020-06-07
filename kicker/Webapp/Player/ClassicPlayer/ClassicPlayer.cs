using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using VideoSource.Dalsa;
using Webapp.Player.Api;

namespace Webapp.Player
{
    [KickermatPlayer(
        "Classic Player",
        "This player uses the image processing mechanism and GameController provided by the framework",
        new string[] { "Dominik Hagenauer", "Andreas König" },
        '⚽')
    ]
    public class ClassicPlayer : IKickermatPlayer
    {
        private readonly IWriteable<ClassicPlayerSettings> _settings;

        public ClassicPlayer(IWriteable<ClassicPlayerSettings> settings,
            IWriteable<DalsaSettings> dalsaSettings)
        {
            _settings = settings;
        }

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
