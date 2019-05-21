namespace Coach
{
    using System;
    using System.Collections.Generic;
    using Communication.Manager;
    using Communication.PlayerControl;
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Verwaltet die Stangen und Positionen der Männchen, welche Positionen angefahren
    /// werden können, und das Anfahren der Positionen und Winkel.
    /// </summary>
    public class DefaultCoach : ICoachInit
    {
        /// <summary>
        /// Contains the max player positions.
        /// </summary>
        private readonly Dictionary<Player, int> playerMaxPositions = new Dictionary<Player, int>();

        /// <summary>
        /// Contains the min player positions.
        /// </summary>
        private readonly Dictionary<Player, int> playerMinPositions = new Dictionary<Player, int>();

        /// <summary>
        /// Contains the own bar positions.
        /// </summary>
        private readonly Dictionary<Bar, int> barPositions = new Dictionary<Bar, int>();

        /// <summary>
        /// The used player control
        /// </summary>
        private IPlayerControl playerControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCoach"/> class.
        /// </summary>
        public DefaultCoach()
        {
            CommunicationManager currentManger = ServiceLocator.LocateService<CommunicationManager>();
            this.playerControl = currentManger.PlayerControl;
            currentManger.CommunicationSetChanged += this.CommunicationSetChanged;
            ServiceLocator.RegisterService<ICoach>(this);
        }

        /// <summary>
        /// Gets the width of the playing field.
        /// </summary>
        /// <value>The width of the playing field.</value>
        public int PlayingFieldWidth { get; private set; }

        /// <summary>
        /// Gets the height of the playing field.
        /// </summary>
        public int PlayingFieldHeight { get; private set; }

        /// <summary>
        /// Gets the goal top.
        /// </summary>
        /// <value>The goal top.</value>
        public int GoalTop { get; private set; }

        /// <summary>
        /// Gets the goal bottom.
        /// </summary>
        public int GoalBottom { get; private set; }

        /// <summary>
        /// Gets the goal area top.
        /// </summary>
        public int GoalAreaTop { get; private set; }

        /// <summary>
        /// Gets the goal area bottom.
        /// </summary>
        public int GoalAreaBottom { get; private set; }

        /// <summary>
        /// Gets the penalty box top.
        /// </summary>
        public int PenaltyBoxTop { get; private set; }

        /// <summary>
        /// Gets the penalty box bottom.
        /// </summary>
        public int PenaltyBoxBottom { get; private set; }

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        public Position PlayingFieldOffset { get; private set; }

        /// <summary>
        /// Gets the position of the center of the playing field.
        /// </summary>
        public Position PlayingFieldCenter { get; private set; }

        public int CameraLongZ { get; private set; }

        public int PlayerShortZ { get; private set; }

        public int BallShortZ { get; private set; }

        /// <summary>
        /// Moves the player to position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The y position.</param>
        public void MovePlayerToYPosition(Player player, int position)
        {
            this.MovePlayerToYPosition(player, position, false);
        }

        /// <summary>
        /// Verschiebt einen bestimmten <see cref="Player"/> zur Position.
        /// </summary>
        /// <param name="player">Der <see cref="Player"/> der an die Position soll.</param>
        /// <param name="position">Die Position die angefahren wird.</param>
        /// <param name="wait">Gibt an, ob auf die Bestaetigung der Positionierung gewartet werden soll</param>
        public void MovePlayerToYPosition(Player player, int position, bool wait)
        {
            if (this.IsYPositionValid(player, position))
            {
                this.playerControl.MovePlayer(player.GetBar(), Convert.ToUInt16(position - this.GetPlayerMinYPosition(player)), wait);
            }
        }

        /// <summary>
        /// Setz den Spielerwinkel für die entsprechende Stange.
        /// </summary>
        /// <param name="bar">Die Stange deren Winkel gesetzt werden soll.</param>
        /// <param name="angle">Der zu setzende Winkel</param>
        public void SetPlayerAngle(Bar bar, short angle)
        {
            this.SetPlayerAngle(bar, angle, false);
        }

        /// <summary>
        /// Setz den Spielerwinkel für die entsprechende Stange.
        /// </summary>
        /// <param name="bar">Die Stange deren Winkel gesetzt werden soll.</param>
        /// <param name="angle">Der zu setzende Winkel</param>
        /// <param name="wait">Blocking auf Antwort warten.</param>
        public void SetPlayerAngle(Bar bar, short angle, bool wait)
        {
            this.playerControl.SetAngle(bar, angle, wait);
        }

        /// <summary>
        /// Stellt alle Spieler senkrecht hin, um den Ball zu blocken.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die Blocken soll.</param>
        public void SetPlayerAngleBlock(Bar bar)
        {
            this.SetPlayerAngleBlock(bar, false);
        }

        /// <summary>
        /// Stellt alle Spieler senkrecht hin, um den Ball zu blocken.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die Blocken soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        public void SetPlayerAngleBlock(Bar bar, bool wait)
        {
            this.playerControl.SetAngle(bar, 0, wait);
        }

        /// <summary>
        /// Stellt alle Spieler waagrecht hin, um den Ball passieren zu lassen.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die passieren lassen soll.</param>
        public void SetPlayerAnglePass(Bar bar)
        {
            this.SetPlayerAnglePass(bar, false);
        }

        /// <summary>
        /// Stellt alle Spieler waagrecht hin, um den Ball passieren zu lassen.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die passieren lassen soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        public void SetPlayerAnglePass(Bar bar, bool wait)
        {
            this.playerControl.SetAngle(bar, -90, wait);
        }

        /// <summary>
        /// Determines whether [is position valid] [the specified bar].
        /// </summary>
        /// <param name="bar">The bar which is checked.</param>
        /// <param name="position">The y position for the check.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified bar]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(Bar bar, int position)
        {
            return position >= this.playerMinPositions[bar.FirstPlayer()] &&
                   position <= this.playerMaxPositions[bar.LastPlayer()];
        }

        /// <summary>
        /// Determines whether [is position valid] [the specified player].
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified player]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(Player player, int position)
        {
            return position >= this.playerMinPositions[player] &&
                   position <= this.playerMaxPositions[player];
        }

        /// <summary>
        /// Gets the bar x position.
        /// </summary>
        /// <param name="bar">The bar whose x position is requested.</param>
        /// <returns>The bar x position.</returns>
        public int GetBarXPosition(Bar bar)
        {
            return this.barPositions[bar];
        }

        /// <summary>
        /// Sets the bar x position.
        /// </summary>
        /// <param name="bar">The bar whose x position is set.</param>
        /// <param name="position">The x position.</param>
        public void SetBarXPosition(Bar bar, int position)
        {
            this.barPositions[bar] = position;
        }

        /// <summary>
        /// Gets the player minimum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The minimum y position of the player.</returns>
        public int GetPlayerMinYPosition(Player player)
        {
            return this.playerMinPositions[player];
        }

        /// <summary>
        /// Sets the player minimum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The y position.</param>
        public void SetPlayerMinYPosition(Player player, int position)
        {
            this.playerMinPositions[player] = position;
        }

        /// <summary>
        /// Gets the player maximum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The maximum y position of the player.</returns>
        public int GetPlayerMaxYPosition(Player player)
        {
            return this.playerMaxPositions[player];
        }

        /// <summary>
        /// Sets the player maximum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The y position.</param>
        public void SetPlayerMaxYPosition(Player player, int position)
        {
            this.playerMaxPositions[player] = position;
        }

        /// <summary>
        /// Gets the y distance between two players.
        /// </summary>
        /// <param name="firstPlayer">The first player.</param>
        /// <param name="secondPlayer">The second player.</param>
        /// <returns>The y distance between first and second player.</returns>
        public int GetYDistanceBetweenPlayers(Player firstPlayer, Player secondPlayer)
        {
            return this.GetPlayerMinYPosition(firstPlayer) - this.GetPlayerMinYPosition(secondPlayer);
        }

        /// <summary>
        /// Sets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        public void SetFieldDimensions(int width, int height)
        {
            this.PlayingFieldWidth = width;
            this.PlayingFieldHeight = height;
        }

        /// <summary>
        /// Sets the offset of the playing field on the image.
        /// </summary>
        /// <param name="fieldOffset">The position of the top left corner of the playing field.</param>
        public void SetFieldOffset(Position fieldOffset)
        {
            this.PlayingFieldOffset = fieldOffset;
        }

        /// <summary>
        /// Sets the center of the playing field on the image.
        /// </summary>
        /// <param name="centerPosition">The position of the center of the playing field.</param>
        public void SetFieldCenter(Position centerPosition)
        {
            this.PlayingFieldCenter = centerPosition;
        }

        /// <summary>
        /// Sets the values of vertial markings on the playing field. 
        /// </summary>
        /// <param name="goalTop">The y position of the goal top.</param>
        /// <param name="goalBottom">The y position of the goal bottom.</param>
        /// <param name="goalAreaTop">The y position of the goal area top.</param>
        /// <param name="goalAreaBottom">The y position of the goal area bottom.</param>
        /// <param name="penaltyBoxTop">The y position of the penalty box top.</param>
        /// <param name="penaltyBoxBottom">The y position of the penalty area bottom.</param>
        public void SetVerticalValues(int goalTop, int goalBottom, int goalAreaTop, int goalAreaBottom, int penaltyBoxTop, int penaltyBoxBottom)
        {
            this.GoalTop = goalTop;
            this.GoalBottom = goalBottom;
            this.GoalAreaTop = goalAreaTop;
            this.GoalAreaBottom = goalAreaBottom;
            this.PenaltyBoxTop = penaltyBoxTop;
            this.PenaltyBoxBottom = penaltyBoxBottom;
        }

        public void SetParallaxCorrectionParams(int cameraLongZ, int playerShortZ, int ballShortZ)
        {
            CameraLongZ = cameraLongZ;
            PlayerShortZ = playerShortZ;
            BallShortZ = ballShortZ;
        }

        /// <summary>
        /// Handles the CommunicationSetChanged event of the CommunicationManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NewCommunicationSetEventArgs"/> instance containing the event data.</param>
        private void CommunicationSetChanged(object sender, NewCommunicationSetEventArgs e)
        {
            this.playerControl = ServiceLocator.LocateService<CommunicationManager>().PlayerControl;
        }
    }
}