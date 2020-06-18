using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public ClassicPlayer(
            IWriteable<ClassicPlayerSettings> settings,
            IWriteable<DalsaSettings> dalsaSettings,
            ILogger<ClassicPlayer> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("ClassicPlayer started");
        }

        public void Stop()
        {
            _logger.LogInformation("ClassicPlayer stopped");
        }

        public void Pause()
        {
            _logger.LogInformation("ClassicPlayer paused");
        }

        public void Resume()
        {
            _logger.LogInformation("ClassicPlayer resumed");
        }
    }
}
