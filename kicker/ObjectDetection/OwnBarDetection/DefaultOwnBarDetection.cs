namespace ObjectDetection
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using GlobalDataTypes;
    using ObjectSearch.BlobSearch;
    using PluginSystem;
    using Emgu.CV;
    using Emgu.Util;
    using Emgu.CV.Structure;
    using System.Drawing;

    /// <summary>
    /// Default algorithm for own bar detection.
    /// </summary>
    public sealed class DefaultOwnBarDetection : BasicObjectDetection<DefaultOwnBarDetectionSettings>, IOwnBarDetection
    {
        /// <summary>
        /// Stores the current player positions.
        /// </summary>
        private readonly Dictionary<Player, Position> currentPositions;

        private readonly Dictionary<Player, Rectangle> currentboundingBoxes;

        /// <summary>
        /// Stores a configured value during calibration.
        /// </summary>
        private bool oldLabelsOnMain;

        /// <summary>
        /// Stores a configured value during calibration.
        /// </summary>
        private bool oldDetectionEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOwnBarDetection"/> class.
        /// </summary>
        public DefaultOwnBarDetection()
            : base(1)
        {
            this.currentPositions = new Dictionary<Player, Position>();
            this.currentboundingBoxes = new Dictionary<Player, Rectangle>();
            ServiceLocator.RegisterService<IOwnBarDetection>(this);
        }

        /// <summary>
        /// Gets the selcted player position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The position of the selected player.</returns>
        public Position GetPlayerPosition(Player player)
        {
            if (this.currentPositions.ContainsKey(player))
            {
                return this.currentPositions[player];
            }

            return null;
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public override void InitUserControl()
        {
            base.InitUserControl();
        }

        /// <summary>
        /// Calibration classes call this method at their start.
        /// </summary>
        public void CalibrationStart()
        {
            if (this.Control.InvokeRequired)
            {
                this.Control.BeginInvoke(new MethodInvoker(this.CalibrationStart));
            }
            else
            {
                this.oldDetectionEnabled = this.Settings.DetectionEnabled;
                this.oldLabelsOnMain = this.Settings.LabelsOnMain;
                this.Settings.LabelsOnMain = true;
                this.Settings.DetectionEnabled = true;

                this.Control.ApplySettingsToUserInterface(false);
            }
        }

        /// <summary>
        /// Calibration classes call this method as they actually start using the detection.
        /// </summary>
        public void CalibrationDetection()
        {
            if (this.Control.InvokeRequired)
            {
                this.Control.BeginInvoke(new MethodInvoker(this.CalibrationDetection));
            }
            else
            {
                this.Settings.LabelsOnMain = false;
                this.Control.ApplySettingsToUserInterface(false);
            }
        }

        /// <summary>
        /// Calibration classes call this method as they actually start using the detection.
        /// </summary>
        public void CalibrationFinished()
        {
            if (this.Control.InvokeRequired)
            {
                this.Control.BeginInvoke(new MethodInvoker(this.CalibrationFinished));
            }
            else
            {
                this.Settings.LabelsOnMain = this.oldLabelsOnMain;
                this.Settings.DetectionEnabled = this.oldDetectionEnabled;
                this.Control.ApplySettingsToUserInterface(true);
            }
        }

        /// <summary>
        /// Detects the objects.
        /// </summary>
        /// <param name="frameCounter">The frame counter.</param>
        protected override void DetectObjects(ulong frameCounter)
        {
            Position[] ownBarPositions;
            int playerCount = Enum.GetValues(typeof(Player)).Length;

            if (this.ObjectSearch.NumberOfFoundObjects > playerCount)
            {
                ownBarPositions = this.GetBestPositions(playerCount);
            }
            else if (this.ObjectSearch.NumberOfFoundObjects == playerCount)
            {
                ownBarPositions = this.GetBestPositions(playerCount);
            }
            else
            {
                ownBarPositions = GetInvalidPositions(playerCount);
            }

            SortOwnBarPositions(ownBarPositions);

            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                this.currentPositions[player] = ownBarPositions[(int)player];
                this.currentboundingBoxes[player] = ownBarPositions[(int)player].BoundingBox;
            }
        }

        /// <summary>
        /// Gets the invalid positions.
        /// </summary>
        /// <param name="playerCount">The player count.</param>
        /// <returns>Array containing dummy positions with invalid flag.</returns>
        private static Position[] GetInvalidPositions(int playerCount)
        {
            Position[] currentPositions = new Position[playerCount];
            for (int playerIndex = 0; playerIndex < currentPositions.Length; playerIndex++)
            {
                currentPositions[playerIndex] = new Position(0, 0, false, false);
            }

            return currentPositions;
        }

        /// <summary>
        /// Sorts the own bar positions.
        /// </summary>
        /// <param name="positionValues">The position values.</param>
        private static void SortOwnBarPositions(Position[] positionValues)
        {
            // Tormann-Position befindet sich immer an Stelle 0, muss nicht sortiert werden

            // Es gibt 2 Verteidigermännchen, das untere kommt zuerst
            SortPositionInformation(positionValues, 1, 2);

            // Es gibt 5 Mittelfeldmännchen, das unterste kommt zuerst
            SortPositionInformation(positionValues, 3, 7);

            // Es gibt 3 Stürmermännchen, das unterste kommt zuerst
            SortPositionInformation(positionValues, 8, 10);
        }

        /// <summary>
        /// Sorts the position information.
        /// </summary>
        /// <param name="positionValues">The position values.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        private static void SortPositionInformation(Position[] positionValues, int startIndex, int stopIndex)
        {
            bool paarSortiert;
            do
            {
                paarSortiert = true;
                for (int position = startIndex; position < stopIndex; position++)
                {
                    if (positionValues[position].YPosition > positionValues[position + 1].YPosition)
                    {
                        var temp = positionValues[position];
                        positionValues[position] = positionValues[position + 1];
                        positionValues[position + 1] = temp;
                        paarSortiert = false;
                    }
                }
            }
            while (paarSortiert == false);
        }

        /// <summary>
        /// Gets the best positions.
        /// </summary>
        /// <param name="playerCount">The player count.</param>
        /// <returns>
        /// Array with the best positons sorted by the x position (descending).
        /// </returns>
        private Position[] GetBestPositions(int playerCount)
        {
            // Get all blobs and their size, x position and y position
            BlobData[] currentBlobs = new BlobData[this.ObjectSearch.NumberOfFoundObjects];
            for (int blobIndex = 0; blobIndex < this.ObjectSearch.NumberOfFoundObjects; blobIndex++)
            {
                currentBlobs[blobIndex] = new BlobData(
                    this.ObjectSearch.GetObjectCenter(blobIndex).XPosition,
                    this.ObjectSearch.GetObjectCenter(blobIndex).YPosition,
                    this.ObjectSearch.ObjectSize(blobIndex),
                    this.ObjectSearch.GetObjectBounds(blobIndex));
            }

            Array.Sort(currentBlobs, (x, y) => y.BlobSize.CompareTo(x.BlobSize));

            Position[] bestPositions = new Position[playerCount];
            for (int positionIndex = 0; positionIndex < bestPositions.Length; positionIndex++)
            {
                bestPositions[positionIndex] = new Position(
                    currentBlobs[positionIndex].BlobCenterX,
                    currentBlobs[positionIndex].BlobCenterY,
                    true,
                    true,
                    currentBlobs[positionIndex].Rectangle);
            }

            Array.Sort(bestPositions, (x, y) => y.XPosition.CompareTo(x.XPosition));
            return bestPositions;
        }

        /// <summary>
        /// Sorts the found bar positions.
        /// </summary>
        /// <returns>Array containing all found blob positions.</returns>
        private Position[] GetAllPositions()
        {
            // Get Blob positions
            Position[] blobPositions = new Position[Enum.GetValues(typeof(Player)).Length];
            for (int blobIndex = 0; blobIndex < this.ObjectSearch.NumberOfFoundObjects; blobIndex++)
            {
                blobPositions[blobIndex] = new Position(
                    this.ObjectSearch.GetObjectCenter(blobIndex).XPosition,
                    this.ObjectSearch.GetObjectCenter(blobIndex).YPosition,
                    true,
                    true,
                    this.ObjectSearch.GetObjectBounds(blobIndex));
            }

            return blobPositions;
        }

        public Rectangle GetPlayerBoundingBox(Player player)
        {
            if (this.currentboundingBoxes.ContainsKey(player))
            {
                return currentboundingBoxes[player];
            }
            else
                return new Rectangle();
        }
    }
}