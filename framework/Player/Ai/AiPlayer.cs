using System;
using System.Collections.Generic;
using System.Text;
using Api.Player;
using Microsoft.Extensions.Logging;

namespace Player.Ai
{
    public class AiPlayer : IKickermatPlayer
    {
        private readonly ILogger _logger;
        
        public AiPlayer(ILogger<AiPlayer> logger)
        {
            _logger = logger;
        }

        public string Name => "AI Player";

        public string Description => @"This is a skeleton for a player that uses some sort of
machine learning technology for image processing or move calculation.
Feel free to start development.";

        public string[] Authors => new string[] { "Max Mustermann" };

        public Rune Emoji => new Rune(0x1f9e0); // Brain Emoji

        public void Start()
        {
            _logger.LogInformation("Ai Player started...");
        }

        public void Stop()
        {
            _logger.LogInformation("Ai Player stopped...");
        }
    }
}
