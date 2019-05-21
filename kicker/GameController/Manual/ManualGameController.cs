namespace GameController.Manual
{
    using System;
    using System.Collections.Generic;
    using Calibration;
    using Coach;
    using GlobalDataTypes;
    using ObjectDetection;
    using PluginSystem;

    /// <summary>
    /// This class implements the <c>ManualGameController</c> to control the Kicker manually.
    /// </summary>
    public sealed class ManualGameController : BasicGameController
    {
        /// <summary>
        /// The user control of this game controller.
        /// </summary>
        private readonly ManualGameControllerUserControl Control;
        
        /// <summary>
        /// The last sent positions of all players.
        /// </summary>
        private readonly Dictionary<Player, int> lastPos;

        /// <summary>
        /// The last set angles of all bars.
        /// </summary>
        private readonly Dictionary<Bar, int> lastAngle;

        /// <summary>
        /// The current own bar detection.
        /// </summary>
        private IOwnBarDetection ownBarDetection;

        /// <summary>
        /// The current correction parameters from calibration.
        /// </summary>
        private ICorrectionParams correctionParams;

        /// <summary>
        /// Inidicates whether this is the first run of this game controller.
        /// </summary>
        private bool firstRun = true;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualGameController"/> class.
        /// </summary>
        public ManualGameController()
            : base()
        {
            this.lastPos = new Dictionary<Player, int>();
            this.lastAngle = new Dictionary<Bar, int>();
            this.Control = new ManualGameControllerUserControl();
            this.SettingsUserControl = this.Control;
            
            ServiceLocator.RegisterUpdateCallback<ICoach>(this.UpdateCoach);
            ServiceLocator.RegisterUpdateCallback<IOwnBarDetection>(this.UpdateDetection);
            ServiceLocator.RegisterUpdateCallback<ICorrectionParams>(this.UpdateCorrection);

            this.ownBarDetection = ServiceLocator.LocateService<IOwnBarDetection>();
            this.correctionParams = ServiceLocator.LocateService<ICorrectionParams>();

            this.UpdateCoach(this.Coach);
            this.UpdateDetection(this.ownBarDetection);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        protected override void Play(Position ballpos)
        {            
            this.Control.UpdateDynamicValues(this.ownBarDetection, ballpos);

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                int pos = this.Control.GetPlayerYPos(player);
                if ((pos != this.lastPos[player] || this.firstRun) &&
                    this.Coach.IsYPositionValid(player, pos))
                {
                    this.Coach.MovePlayerToYPosition(player, pos);
                }

                this.lastPos[player] = pos;
            }

            foreach (Bar bar in Enum.GetValues(typeof(Bar)))
            {
                if (bar == Bar.All)
                {
                    continue;
                }
                
                int angle = this.Control.GetBarAngle(bar);
                if (angle != this.lastAngle[bar] || this.firstRun)
                {
                    this.Coach.SetPlayerAngle(bar, (short)angle);
                }
            }

            this.firstRun = false;
        }
        
        /// <summary>
        /// Updates the own bar detection.
        /// </summary>
        /// <param name="newDetection">The new own bar detection.</param>
        private void UpdateDetection(IOwnBarDetection newDetection)
        {
            this.ownBarDetection = newDetection;
            this.Control.UpdateDynamicValues(this.ownBarDetection, null);
        }
        
        /// <summary>
        /// Initializes the position labels with values from the current coach.
        /// </summary>
        private void InitPosFromCoach()
        {
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                if (this.Coach != null)
                {
                    this.lastPos[player] = this.Coach.GetPlayerMinYPosition(player);
                }
                else
                {
                    this.lastPos[player] = 0;
                }
            }

            foreach (Bar bar in Enum.GetValues(typeof(Bar)))
            {
                this.lastAngle[bar] = 0;
            }
        }

        /// <summary>
        /// Updates the coach.
        /// </summary>
        /// <param name="newCoach">The new coach.</param>
        private void UpdateCoach(ICoach newCoach)
        {
            this.InitPosFromCoach();            
            this.Control.UpdateStaticValues(newCoach, this.correctionParams);
        }

        /// <summary>
        /// Updates the correction parameters.
        /// </summary>
        /// <param name="corrParams">The new correction parameters.</param>
        private void UpdateCorrection(ICorrectionParams corrParams)
        {
            this.correctionParams = corrParams;
            this.Control.UpdateStaticValues(this.Coach, corrParams);
        }
    }
}
