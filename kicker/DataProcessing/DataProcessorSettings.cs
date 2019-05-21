namespace DataProcessing
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using Calibration.Default;
    using GameController;
    using ObjectDetection;
    using PositionPrediction.Default;

    /// <summary>
    /// Class for Settings of default data processor.
    /// </summary>
    public class DataProcessorSettings
    {
        /// <summary>
        /// Gets or sets the type of the position prediction algorithm.
        /// </summary>
        /// <value>The position prediction algorithm.</value>
        [XmlIgnore]
        public Type PositionPredictionAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of the game controller algorithm.
        /// </summary>
        /// <value>The game controller algorithm.</value>
        [XmlIgnore]
        public Type GameControllerAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of the ball detection image processor.
        /// </summary>
        /// <value>The type of the ball detection image processor.</value>
        [XmlIgnore]
        public Type BallDetectionAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the calibration algorithm.
        /// </summary>
        /// <value>The calibration algorithm.</value>
        [XmlIgnore]
        public Type CalibrationAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of opponent bar detection image processor.
        /// </summary>
        /// <value>The type of opponent bar detection image processor.</value>
        [XmlIgnore]
        public Type OpponentBarDetectionAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the type of the own bar detection image processor.
        /// </summary>
        /// <value>The type of the own bar detection image processor.</value>
        [XmlIgnore]
        public Type OwnBarDetectionAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the ball detection image processor string.
        /// </summary>
        /// <value>The ball detection image processor string.</value>
        public string BallDetectionAlgorithmString
        {
            get
            {
                return this.BallDetectionAlgorithm.AssemblyQualifiedName;
            }
         
            set
            {
                this.BallDetectionAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the own bar detection image processor string.
        /// </summary>
        /// <value>The own bar detection image processor string.</value>
        public string OwnBarDetectionAlgorithmString
        {
            get
            {
                return this.OwnBarDetectionAlgorithm.AssemblyQualifiedName;
            }
          
            set
            {
                this.OwnBarDetectionAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the opponent bar detection image processor string.
        /// </summary>
        /// <value>The opponent bar detection image processor string.</value>
        public string OpponentBarDetectionAlgorithmString
        {
            get
            {
                return this.OpponentBarDetectionAlgorithm.AssemblyQualifiedName;
            }
           
            set
            {
                this.OpponentBarDetectionAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the game controller algorithm string.
        /// </summary>
        /// <value>The game controller algorithm string.</value>
        public string GameControllerAlgorithmString
        {
            get
            {
                return this.GameControllerAlgorithm.AssemblyQualifiedName;
            }

            set
            {
                this.GameControllerAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the position prediction algorithm string.
        /// </summary>
        /// <value>The position prediction algorithm string.</value>
        public string PositionPredictionAlgorithmString
        {
            get
            {
                return this.PositionPredictionAlgorithm.AssemblyQualifiedName;
            }

            set
            {
                this.PositionPredictionAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the calibration algorithm string.
        /// </summary>
        /// <value>The calibration algorithm string.</value>
        public string CalibrationAlgorithmString
        {
            get
            {
                return this.CalibrationAlgorithm.AssemblyQualifiedName;
            }

            set
            {
                this.CalibrationAlgorithm = Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the selected tab in the data processor.
        /// </summary>
        [DefaultValueAttribute(0)]
        public int SelectedTab { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            if (this.BallDetectionAlgorithm == null)
            {
                this.BallDetectionAlgorithm = typeof(DefaultBallDetection);
            }

            if (this.OpponentBarDetectionAlgorithm == null)
            {
                this.OpponentBarDetectionAlgorithm = typeof(DefaultOpponentBarDetection);
            }

            if (this.OwnBarDetectionAlgorithm == null)
            {
                this.OwnBarDetectionAlgorithm = typeof(DefaultOwnBarDetection);
            }

            if (this.CalibrationAlgorithm == null)
            {
                this.CalibrationAlgorithm = typeof(DefaultCalibration);
            }

            if (this.GameControllerAlgorithm == null)
            {
                this.GameControllerAlgorithm = typeof(EmguGameController);
            }

            if (this.PositionPredictionAlgorithm == null)
            {
                this.PositionPredictionAlgorithm = typeof(DefaultPositionPrediction);
            }
        }     
    }
}