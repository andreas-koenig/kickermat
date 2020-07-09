using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameControllers
{
    public class EmguGameControllerSettings
    {
        [Category("Shoot-/Blockbehavior")]
        [Description("If enabled players only shoot when the ball is in their vicinity")]
        [DisplayName("Use ownbardetection to determine playerpositions")]
        public bool UseOwnBarDetectionToDeterminePlayerPositions { get; set; } = true;
        [Category("ShootingRange")]
        public int KeeperShootingRange { get; set; } = 25;
        [Category("ShootingRange")]
        public int DefenseShootingRange { get; set; } = 25;
        [Category("ShootingRange")]
        public int MidFieldShootingRange { get; set; } = 25;
        [Category("ShootingRange")]
        public int StrikerShootingRange { get; set; } = 25;
        [Category("ShootingRange")]
        public int VerticalShootingRange { get; set; } = 25;
        [Category("Shoot-/Blockbehavior")]
        public int ShootingMilisecs { get; set; } = 150;
        [Category("Shoot-/Blockbehavior")]
        public int BlockingMilisecs { get; set; } = 60;
    }
}
