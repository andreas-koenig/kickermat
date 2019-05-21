namespace GameController
{
    using System;
    using GlobalDataTypes;

    /// <summary>
    /// This class implements the SmoothingAdvancedGameController used for the playing logic.
    /// </summary>
    public sealed class SmoothingAdvancedGameController : BasicGameController
    {
        /// <summary>
        /// The user control of this game controller.
        /// </summary>
        private readonly SmoothingAdvancedGameControllerUserControl Control;

        /// <summary>
        /// An instance of a random number generator.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The width of the playing field.
        /// </summary>
        private int e;

        /// <summary>
        /// The goal top Y position.
        /// </summary>
        private int y;

        /// <summary>
        /// The height if the playing field.
        /// </summary>
        private int z;

        /// <summary>
        /// Stores the current Y position of the keeper.
        /// </summary>
        private int aktKeeperPos = 0;

        /// <summary>
        /// Stores the current Y position of the defense.
        /// </summary>
        private int aktDefensePos = 0;

        /// <summary>
        /// Stores the current Y position of the midfield.
        /// </summary>
        private int aktMidfieldPos = 0;

        /// <summary>
        /// Stores the current Y position of the strikers.
        /// </summary>
        private int aktStrikerPos = 0;

        /// <summary>
        /// Counter for the shooting state of the strikers.
        /// </summary>
        private int strikercounter = 0;

        /// <summary>
        /// Counter for the shooting state of the midfield.
        /// </summary>
        private int midfieldcounter = 0;

        /// <summary>
        /// A saved random value for the next run.
        /// </summary>
        private int randnext = 2;



        /// <summary>
        /// Initializes a new instance of the <see cref="SmoothingAdvancedGameController"/> class.
        /// </summary>
        public SmoothingAdvancedGameController()
            : base()
        {
            this.Control = new SmoothingAdvancedGameControllerUserControl();
            this.SettingsUserControl = this.Control;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        protected override void Play(Position ballpos)
        {
            this.e = Coach.PlayingFieldWidth;
            this.y = Coach.GoalTop;
            this.z = Coach.PlayingFieldHeight;

            this.SetKeeperPosition(ballpos);
            this.SetDefensePosition(ballpos);
            this.SetMidfieldPosition(ballpos);
            this.SetStrikerPosition(ballpos);
        }

        /// <summary>
        /// Calculates the movement threshold for the given bar.
        /// </summary>
        /// <param name="bar">the bar of which the threshold should be calculated</param>
        /// <param name="ballPosition">the positionof the ball</param>
        /// <returns>the threshold of the bar movement</returns>
        private int calculateMovementThreshold(Bar bar, Position ballPosition)
        {
            int barXPos = Coach.GetBarXPosition(bar);
            int ballXPos = ballPosition.XPosition;
            int barSpacing = Math.Abs(Coach.GetBarXPosition(Bar.Keeper) - Coach.GetBarXPosition(Bar.Defense));
            int maxThresholdLeft = this.Control.ThresholdLeft.Value;
            int maxThresholdRight = this.Control.ThresholdRight.Value;
            int minDistRight = barXPos + this.Control.preciseDistanceRight.Value * barSpacing;
            int minDistLeft = barXPos - this.Control.preciseDistanceLeft.Value * barSpacing;

            // falls Ball im Bereich von zwei Stangenabständen vor oder einem hinter der aktuell zu steuerenden
            // Stange liegt, dann muss eine genaue Bewegung erfolgen
            if (ballXPos <= minDistRight && ballXPos >= minDistLeft)
            {
                return 0;
            }
            //falls Ball links der Stange ist.
            else if (ballXPos < minDistLeft)
            {
                float sloapLeft = -maxThresholdLeft / (Math.Abs(-7 * barSpacing - minDistLeft));
                return (int)(sloapLeft * (Math.Abs(barXPos - ballXPos) + minDistLeft));
            }
            //falls Ball rechts der Stange ist.
            float sloapRight = maxThresholdRight / (Math.Abs(7 * barSpacing - minDistRight));
            return (int)(sloapRight * (Math.Abs(barXPos - ballXPos) - minDistRight));
        }
        /// <summary>
        /// Sets the keeper position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetKeeperPosition(Position ballpos)
        {
            // Blocken, wenn der Ball nicht direkt hinter dem Keeper liegt 
            if (!(ballpos.YPosition >= this.aktKeeperPos - 20 &&
                ballpos.YPosition <= this.aktKeeperPos + 20))
            {
                Coach.SetPlayerAngleBlock(Bar.Keeper);
            }

            // Wenn Ball auf gleicher Ebene wie Keeper, dann soll er den Ball seitlich wegkicken
            if (ballpos.XPosition > (Coach.GetBarXPosition(Bar.Keeper) - 10) &&
                ballpos.XPosition < (Coach.GetBarXPosition(Bar.Keeper) + 30))
            {
                double transpositionsgrad = 50;

                Coach.SetPlayerAngle(Bar.Keeper, (short)(((-1.0 * (ballpos.XPosition - Coach.GetBarXPosition(Bar.Keeper))) / 30.0) * transpositionsgrad));

                // Wenn Ball oberhalb des Tormanns liegt, dann schieß den Ball nach oben weg
                if (this.aktKeeperPos > ballpos.YPosition)
                {
                    Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMinYPosition(Player.Keeper));
                }
                else
                {
                    Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMaxYPosition(Player.Keeper));
                }

                return;
            }

            int newKeeperPosition = 0;

            // Wenn Ball hinter Defense-Bar, dann soll der Keeper dem Ball folgen
            if (ballpos.XPosition > Coach.GetBarXPosition(Bar.Defense))
            {
                newKeeperPosition = ballpos.YPosition;
            }

            // Wenn Ball zwischen Midfield and Striker, dann nimmt dynamische Defense-Position ein
            else if (ballpos.XPosition > Coach.GetBarXPosition(Bar.Striker))
            {
                // Punkt B
                double px, py;
                int bx = ballpos.XPosition;
                int by = ballpos.YPosition;

                double deltamax = 15.0;
                double delta = ((this.z / 2.0) - by) / (this.z / 2.0) * deltamax;
                delta = delta < 0 ? -1 * delta : delta;

                // Punkt P
                px = this.e + delta;
                if (by < (this.z / 2))
                {
                    py = ((5.0 / 6.0) * this.z) - ((2.0 / 3.0) * this.y);
                }
                else
                {
                    py = ((1.0 / 6.0) * this.z) + ((2.0 / 3.0) * this.y);
                }

                int a = this.e - Coach.GetBarXPosition(Bar.Keeper);

                // Geradengleichung
                // g: y = ((py-by)/(py-bx))*x + ((px*by-bx*py)/(px-bx))
                // Tormannstange: x = (e-a)
                // Schnittpunkt K: K((e-a)|(py-by)/(py-bx))*(e-a) + ((px*by-bx*py)/(px-bx))

                // Wenn Ball zwischen Defense and Midfield, dann nimmt Keeper modifizierte dynamische Defense-Position ein
                if (ballpos.XPosition > Coach.GetBarXPosition(Bar.Midfield))
                {
                    // Bessere Keeperposition um schräge Schüsse zu verhindern
                    newKeeperPosition = (int)((((py - by) / (px - bx)) * (this.e - a)) + (((px * by) - (bx * py)) / (px - bx)) * 0.96);
                }
                // Andernfalls
                else
                {
                    newKeeperPosition = (int)((((py - by) / (px - bx)) * (this.e - a)) + (((px * by) - (bx * py)) / (px - bx)));
                }
            }

            // Wenn der Ball vor Striker, dann nimmt Keeper eine feste Position ein
            else
            {
                /*if (ballpos.YPosition < this.z / 2)
                {
                    newKeeperPosition = (int)((Coach.GetPlayerMaxYPosition(Player.Keeper) + Coach.GetPlayerMinYPosition(Player.Keeper)) * (0.48)); 
                }
                else
                {
                    newKeeperPosition = (int)((Coach.GetPlayerMaxYPosition(Player.Keeper) + Coach.GetPlayerMinYPosition(Player.Keeper)) * (0.52));
                }*/
                newKeeperPosition = (int)((Coach.GetPlayerMaxYPosition(Player.Keeper) + Coach.GetPlayerMinYPosition(Player.Keeper)) * (0.5));
            }

            if (newKeeperPosition > Coach.GetPlayerMaxYPosition(Player.Keeper))
            {
                newKeeperPosition = Coach.GetPlayerMaxYPosition(Player.Keeper);
            }
            else if (newKeeperPosition < Coach.GetPlayerMinYPosition(Player.Keeper))
            {
                newKeeperPosition = Coach.GetPlayerMinYPosition(Player.Keeper);
            }

            if (newKeeperPosition <= Coach.GoalTop + 5)
            {
                newKeeperPosition = Coach.GoalTop + 5;
            }
            else if (newKeeperPosition >= Coach.GoalBottom - 5)
            {
                newKeeperPosition = Coach.GoalBottom - 5;
            }

            int threshold = calculateMovementThreshold(Bar.Keeper, ballpos);
            if (Math.Abs(this.aktKeeperPos - newKeeperPosition) >= threshold)
            {
                Coach.MovePlayerToYPosition(Player.Keeper, newKeeperPosition);
                this.aktKeeperPos = newKeeperPosition;
            }

            // Ball nach vorne schießen
            if (ballpos.XPosition >= Coach.GetBarXPosition(Bar.Keeper) - this.ShootingRange &&
                ballpos.XPosition < Coach.GetBarXPosition(Bar.Keeper) + 10)
            {
                // Nur schießen, wenn Ball direkt vor dem Tormann liegt
                if (ballpos.YPosition >= this.aktKeeperPos - 10 &&
                    ballpos.YPosition <= this.aktKeeperPos + 10)
                {
                    Coach.SetPlayerAngle(Bar.Keeper, 90);
                }
            }
        }

        /// <summary>
        /// Sets the defense position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetDefensePosition(Position ballpos)
        {
            // Passieren lassen wenn der Ball hinter dem Verteidiger ist; Blocken wenn er davor liegt
            if (ballpos.XPosition > (Coach.GetBarXPosition(Bar.Defense) + this.ShootingRange))
            {
                // Nur Passieren lassen wenn Ball nicht direkt dahinter liegt(-> kein Eigentor)
                if (!(ballpos.YPosition >= this.aktDefensePos - 15 &&
                    ballpos.YPosition <= this.aktDefensePos + 15))
                {
                    // Passieren lassen
                    Coach.SetPlayerAnglePass(Bar.Defense);
                }
            }
            else
            {
                // Blocken
                Coach.SetPlayerAngleBlock(Bar.Defense);
            }

            int newDefensePosition = 0;


            // Befindet sich der Ball im äußeren Bereich des Spielfeldes und ist er genügend nahe an der Stange, kann der Ball seitlich geschossen werden
            if ((ballpos.YPosition < (0.3 * this.z) || ballpos.YPosition > (0.7 * this.z)) &&
                ballpos.XPosition >= Coach.GetBarXPosition(Bar.Defense) - this.ShootingRange &&
                ballpos.XPosition < Coach.GetBarXPosition(Bar.Defense))
            {
                newDefensePosition = ballpos.YPosition;
            }
            // Defense verteidigt dynamisch
            else if (ballpos.XPosition > Coach.GetBarXPosition(Bar.Striker))
            {
                // Punkt B
                double px, py;
                int bx = ballpos.XPosition;
                int by = ballpos.YPosition;

                double deltamax = 15.0;
                double delta = ((this.z / 2.0) - by) / (this.z / 2.0) * deltamax;
                delta = delta < 0 ? -1 * delta : delta;

                // Punkt P
                px = this.e + delta;
                py = by < (this.z / 2) ? (this.z + this.y) / 3.0 : ((2.0 / 3.0) * this.z) - ((1.0 / 3.0) * this.y);

                int b = this.e - Coach.GetBarXPosition(Bar.Defense);

                // Geradengleichung
                // g: y = ((py-by)/(py-bx))*x + ((px*by-bx*py)/(px-bx))
                // Verteidigerstange: x = (e-b)
                // Schnittpunkt K: K((e-b)|(py-by)/(py-bx))*(e-b) + ((px*by-bx*py)/(px-bx))                
                newDefensePosition = (int)((((py - by) / (px - bx)) * (this.e - b)) + (((px * by) - (bx * py)) / (px - bx)));
            }
            else
            {
                // Defense nimmt feste Position ein
                if (ballpos.YPosition < (this.z / 2))
                {
                    newDefensePosition = (int)((Coach.GetPlayerMaxYPosition(Player.DefenseOne) + Coach.GetPlayerMinYPosition(Player.DefenseOne)) * (0.61));
                }
                else
                {
                    newDefensePosition = (int)((Coach.GetPlayerMaxYPosition(Player.DefenseOne) + Coach.GetPlayerMinYPosition(Player.DefenseOne)) * (0.39));
                }
            }

            // den zu bewegenden Verteidiger wählen
            Player playerToMove = Player.DefenseOne;
            if (Coach.IsYPositionValid(Player.DefenseOne, newDefensePosition))
            {
                playerToMove = Player.DefenseOne;
            }
            else if (Coach.IsYPositionValid(Player.DefenseTwo, newDefensePosition))
            {
                playerToMove = Player.DefenseTwo;
            }

            // Spieler bewegen falls notwendig
            int threshold = calculateMovementThreshold(Bar.Defense, ballpos);
            if (Math.Abs(this.aktDefensePos - newDefensePosition) >= threshold)
            {
                Coach.MovePlayerToYPosition(playerToMove, newDefensePosition);
                this.aktDefensePos = newDefensePosition;
            }

            // Ball schießen, wenn in Reichweite
            if (ballpos.XPosition >= Coach.GetBarXPosition(Bar.Defense) - this.ShootingRange &&
                ballpos.XPosition < Coach.GetBarXPosition(Bar.Defense) + 5)
            {
                // Nur schießen, wenn Ball direkt vor dem Verteidiger liegt
                if (ballpos.YPosition >= this.aktDefensePos - 20 &&
                    ballpos.YPosition <= this.aktDefensePos + 20)
                {
                    Coach.SetPlayerAngle(Bar.Defense, 90);
                }
            }
        }

        /// <summary>
        /// Sets the midfield position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetMidfieldPosition(Position ballpos)
        {
            this.midfieldcounter--;

            // Blocken wenn Ball vor Stange; Passieren lassen wenn Ball hinter Stange
            if (ballpos.XPosition > (Coach.GetBarXPosition(Bar.Midfield) + 10))
            {
                // Nur Passieren lassen wenn Ball nicht direkt dahinter liegt
                // Damit er nicht den Ball Richtung eigenes Tor schießt
                if (!(ballpos.YPosition >= this.aktMidfieldPos - 15 &&
                    ballpos.YPosition <= this.aktMidfieldPos + 15))
                {
                    Coach.SetPlayerAnglePass(Bar.Midfield);
                    ////return;
                }
            }
            else
            {
                if (this.midfieldcounter == 4 || this.midfieldcounter < 0)
                {
                    Coach.SetPlayerAngle(Bar.Midfield, -20);
                }
            }

            int newMidfieldPosition = ballpos.YPosition;
            Player playerToMove = Player.MidfieldOne;

            if (ballpos.XPosition > Coach.GetBarXPosition(Bar.Striker))
            {
                // Versatz nach oben(0), unten(1) oder keinen Versatz(2), je nach Zufallszahl            
                switch (this.randnext)
                {
                    case 0:
                        newMidfieldPosition += 5;
                        break;
                    case 1:
                        newMidfieldPosition -= 5;
                        break;
                    case 2:
                        break;
                }

                if (Coach.IsYPositionValid(Player.MidfieldOne, newMidfieldPosition))
                {
                    playerToMove = Player.MidfieldOne;
                }
                else if (Coach.IsYPositionValid(Player.MidfieldTwo, newMidfieldPosition))
                {
                    playerToMove = Player.MidfieldTwo;
                }
                else if (Coach.IsYPositionValid(Player.MidfieldThree, newMidfieldPosition))
                {
                    playerToMove = Player.MidfieldThree;
                }
                else if (Coach.IsYPositionValid(Player.MidfieldFour, newMidfieldPosition))
                {
                    playerToMove = Player.MidfieldFour;
                }
                else if (Coach.IsYPositionValid(Player.MidfieldFive, newMidfieldPosition))
                {
                    playerToMove = Player.MidfieldFive;
                }
            }
            else
            {
                playerToMove = Player.MidfieldThree;
                // Defense nimmt feste Position ein
                if (ballpos.YPosition < (this.z / 2))
                {
                    newMidfieldPosition = (int)((Coach.GetPlayerMaxYPosition(playerToMove)));
                }
                else
                {
                    newMidfieldPosition = (int)((Coach.GetPlayerMinYPosition(playerToMove)));
                }

            }
            // Schwelle

            int threshold = calculateMovementThreshold(Bar.Midfield, ballpos);
            if (Math.Abs(this.aktMidfieldPos - newMidfieldPosition) >= threshold)
            {
                Coach.MovePlayerToYPosition(playerToMove, newMidfieldPosition);
                this.aktMidfieldPos = newMidfieldPosition;
            }

            if (this.midfieldcounter <= 0)
            {
                if (ballpos.XPosition >= Coach.GetBarXPosition(Bar.Midfield) - this.ShootingRange &&
                    ballpos.XPosition < Coach.GetBarXPosition(Bar.Midfield) + 10)
                {
                    Coach.SetPlayerAngle(Bar.Midfield, 90);
                    this.midfieldcounter = 8;
                    this.randnext = this.random.Next(3);
                }
            }
        }

        /// <summary>
        /// Sets the striker position.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        private void SetStrikerPosition(Position ballpos)
        {
            this.strikercounter--;

            // Blocken wenn Ball vor Stange; Passieren lassen wenn Ball hinter Stange
            if (ballpos.XPosition > (Coach.GetBarXPosition(Bar.Striker) + 10))
            {
                // Nur Passieren lassen wenn Ball nicht direkt dahinter liegt
                // Damit er nicht den Ball Richtung eigenes Tor schießt
                if (!(ballpos.YPosition >= this.aktStrikerPos - 10 &&
                    ballpos.YPosition <= this.aktStrikerPos + 10))
                {
                    Coach.SetPlayerAnglePass(Bar.Striker);
                    ////return;
                }
            }
            else
            {
                if (this.strikercounter == 4 || this.strikercounter < 0)
                {
                    Coach.SetPlayerAngle(Bar.Striker, -20);
                }
            }

            int newStrikerPosition = ballpos.YPosition;

            // Befindet sich der Ball im ausserhalb des Torbereichs
            if (ballpos.YPosition < this.y)
            {
                newStrikerPosition -= (int)(15 - ((ballpos.YPosition * 15) / this.y));
            }
            else if (ballpos.YPosition > (this.z - this.y))
            {
                newStrikerPosition += (int)(((ballpos.YPosition - (this.z - this.y)) * 25) / this.y);
            }

            if (newStrikerPosition > Coach.GetPlayerMaxYPosition(Player.StrikerThree))
            {
                newStrikerPosition = Coach.GetPlayerMaxYPosition(Player.StrikerThree);
            }
            else if (newStrikerPosition < Coach.GetPlayerMinYPosition(Player.StrikerOne))
            {
                newStrikerPosition = Coach.GetPlayerMinYPosition(Player.StrikerOne);
            }

            Player playerToMove = Player.StrikerOne;

            if (Coach.IsYPositionValid(Player.StrikerOne, newStrikerPosition))
            {
                playerToMove = Player.StrikerOne;
            }
            else if (Coach.IsYPositionValid(Player.StrikerTwo, newStrikerPosition))
            {
                playerToMove = Player.StrikerTwo;
            }
            else if (Coach.IsYPositionValid(Player.StrikerThree, newStrikerPosition))
            {
                playerToMove = Player.StrikerThree;
            }

            int threshold = calculateMovementThreshold(Bar.Striker, ballpos);
            if (Math.Abs(this.aktStrikerPos - newStrikerPosition) >= threshold)
            {
                Coach.MovePlayerToYPosition(playerToMove, newStrikerPosition);
                this.aktStrikerPos = newStrikerPosition;
            }

            if (this.strikercounter <= 0)
            {
                if (ballpos.XPosition >= Coach.GetBarXPosition(Bar.Striker) - this.ShootingRange &&
                    ballpos.XPosition < Coach.GetBarXPosition(Bar.Striker) + 10)
                {
                    Coach.SetPlayerAngle(Bar.Striker, 110);
                    this.strikercounter = 8;
                }
            }
        }
    }
}
