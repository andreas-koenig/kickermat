namespace PositionPrediction.Default
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using GlobalDataTypes;
    using PluginSystem;
    using PluginSystem.Configuration;

    /// <summary>
    /// Implements the default distance prediction which doesn't predict any values.
    /// </summary>
    public sealed class DefaultPositionPrediction : IPositionPrediction, IXmlConfigurableKickerPlugin, IDisposable
    {
        /// <summary>
        /// Indicates if the instance is already initialized or not.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// The settings used by the instance.
        /// </summary>
        private DefaultPositionPredictionSettings settings;

        /// <summary>
        /// Internal predicted position.
        /// </summary>        
        private PredictedBallPosition predictedPosition;

        /// <summary>
        /// Internal last detected position.
        /// </summary>
        private PredictedBallPosition lastDetectedPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPositionPrediction"/> class.
        /// </summary>
        public DefaultPositionPrediction()
        {
            this.predictedPosition = new PredictedBallPosition();
            this.lastDetectedPosition = new PredictedBallPosition();
        }

        /// <summary>
        /// Gets the predicted position.
        /// </summary>
        public Position PredictedPosition
        {
            get
            {
                return this.predictedPosition;
            }
        }

        /// <summary>
        /// Gets the user control of the plugin.
        /// </summary>
        public UserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Executes the position prediction
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        /// <param name="currentFrame">The current frame.</param>
        public void Run(Position ballPosition, int currentFrame, Rectangle playingarea = new Rectangle())
        {
            if (this.settings.PredictionEnabled == true)
            {
                PredictedBallPosition operatingPosition;
                double multiplier;

                // Initialize values
                if (this.isInitialized == false)
                {
                    if (ballPosition.Valid == false)
                    {
                        return;
                    }

                    this.predictedPosition.SetValues(ballPosition, currentFrame, 1);
                    this.lastDetectedPosition.SetValues(ballPosition, currentFrame, 1);
                    this.isInitialized = true;
                    return;
                }

                // Ball hat sich nicht bewegt
                if ((ballPosition.XPosition == this.lastDetectedPosition.XPosition) &&
                    (ballPosition.YPosition == this.lastDetectedPosition.YPosition) && (ballPosition.Valid == true))
                {
                    predictedPosition.SetValues(ballPosition, currentFrame, predictedPosition.FrameDifference);
                    // keine Bewegung
                    this.predictedPosition.Direction.XPosition = 0;
                    this.predictedPosition.Direction.YPosition = 0;

                    return;
                }

                /* Mit welchen Wert wird gerechnet(Rechenposition)
                 * Falls der Zeitunterschied vom aktuellen Frame zum Frame der vorletzten(also nicht der aktuellen) erkannten Ballposition größer als die Maximum Difference ist wird mit dem letzen vorhergesagten Wert gerechnet  
                 * sonst mit der vorletzten erkannten Ballpositio
                 */

                if (currentFrame - this.lastDetectedPosition.FrameNumber > this.settings.MaximumDifference)
                {
                    operatingPosition = (PredictedBallPosition)this.predictedPosition.Clone();
                }
                else
                {
                    operatingPosition = (PredictedBallPosition)this.lastDetectedPosition.Clone();
                }

                // Ball wurde von Ballerkennung erkannt
                if (ballPosition.Valid == true)
                {
                    // Framunterschied
                    operatingPosition.FrameDifference = currentFrame - operatingPosition.FrameNumber;

                    // Streckung / Stauchung
                    multiplier = (double)this.settings.FramesToPredict / operatingPosition.FrameDifference;

                    // Berechnung des Richtungsvektors aus Position der Ballerkennung und Rechenposition
                    operatingPosition.Direction.XPosition = ballPosition.XPosition - operatingPosition.XPosition;
                    operatingPosition.Direction.YPosition = ballPosition.YPosition - operatingPosition.YPosition;

                    try
                    {
                        // vorhergesagte Position = Richtungsvektor * Streckung bzw. Stauchung + Ballposition
                        var predictedX =
                            System.Convert.ToInt32(operatingPosition.Direction.XPosition * multiplier) +
                            ballPosition.XPosition;
                        var predictedY =
                            System.Convert.ToInt32(operatingPosition.Direction.YPosition * multiplier) +
                            ballPosition.YPosition;
                        //Wenn die vorhergesagte Position den Spielbereich verlassen würde wird diese korrigiert.
                        if (!playingarea.Contains(predictedX, predictedY))
                        {
                            if (predictedY > playingarea.Bottom)
                            {
                                predictedY -= Math.Abs(predictedY - playingarea.Bottom);
                            }
                            else if (predictedY < playingarea.Top)
                            {
                                predictedY += Math.Abs(predictedY - playingarea.Top);
                            }
                            if (predictedX > playingarea.Right)
                            {
                                predictedX -= Math.Abs(predictedX - playingarea.Right);
                            }
                            else if (predictedX < playingarea.Left)
                            {
                                predictedX += Math.Abs(predictedX - playingarea.Left);
                            }
                            operatingPosition.XPosition = predictedX;
                            operatingPosition.YPosition = predictedY;
                        }
                        else
                        {
                            operatingPosition.XPosition = predictedX;
                            operatingPosition.YPosition = predictedY;
                        }
                    }
#pragma warning disable 168
                    catch (OverflowException e)
#pragma warning restore 168
                    {
                        operatingPosition.XPosition = ballPosition.XPosition;
                        operatingPosition.YPosition = ballPosition.YPosition;
                    }

                    operatingPosition.Valid = true;
                    this.lastDetectedPosition.ResetValues(ballPosition, operatingPosition.Direction, operatingPosition.FrameDifference);
                    this.lastDetectedPosition.FrameNumber = operatingPosition.FrameNumber = currentFrame;
                }
                else
                {
                    // Ball wurde nicht erkannt
                    multiplier = ((double)this.settings.FramesToPredict + currentFrame - operatingPosition.FrameNumber) /
                                 operatingPosition.FrameDifference;
                    try
                    {
                        // vorhergesagte Position = Richtungsvektor * Streckung bzw. Stauchung + Ballposition
                        var predictedX =
                            System.Convert.ToInt32(operatingPosition.Direction.XPosition * multiplier) +
                            ballPosition.XPosition;
                        var predictedY =
                            System.Convert.ToInt32(operatingPosition.Direction.YPosition * multiplier) +
                            ballPosition.YPosition;
                        //Wenn die vorhergesagte Position den Spielbereich verlassen würde wird diese korrigiert.
                        if (!playingarea.Contains(predictedX, predictedY))
                        {
                            if (predictedY > playingarea.Bottom)
                            {
                                predictedY -= Math.Abs(predictedY - playingarea.Bottom);
                            }
                            else if (predictedY < playingarea.Top)
                            {
                                predictedY += Math.Abs(predictedY - playingarea.Top);
                            }
                            if (predictedX > playingarea.Right)
                            {
                                predictedX -= Math.Abs(predictedX - playingarea.Right);
                            }
                            else if (predictedX < playingarea.Left)
                            {
                                predictedX += Math.Abs(predictedX - playingarea.Left);
                            }
                            operatingPosition.XPosition = predictedX;
                            operatingPosition.YPosition = predictedY;
                        }
                        else
                        {
                            operatingPosition.XPosition = predictedX;
                            operatingPosition.YPosition = predictedY;
                        }
                    }
#pragma warning disable 168
                    catch (OverflowException e)
#pragma warning restore 168
                    {
                        operatingPosition.XPosition = ballPosition.XPosition;
                        operatingPosition.YPosition = ballPosition.YPosition;
                    }

                    operatingPosition.Valid = true;
                    operatingPosition.FrameNumber = currentFrame;
                }

                this.predictedPosition = (PredictedBallPosition)operatingPosition.Clone();
            }
            else
            {
                this.predictedPosition.Valid = ballPosition.Valid;
                this.predictedPosition.InPlayingArea = ballPosition.InPlayingArea;
                this.predictedPosition.XPosition = ballPosition.XPosition;
                this.predictedPosition.YPosition = ballPosition.YPosition;
            }
        }

        /// <summary>
        /// Loads the configuration from a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void LoadConfiguration(string xmlFileName)
        {
            this.settings = SettingsSerializer.LoadSettingsFromXml<DefaultPositionPredictionSettings>(xmlFileName);
        }

        /// <summary>
        /// Saves the configuration to a XML file.
        /// </summary>
        /// <param name="xmlFileName">Name of the XML file.</param>
        public void SaveConfiguration(string xmlFileName)
        {
            SettingsSerializer.SaveSettingsToXml(this.settings, xmlFileName);
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public void InitUserControl()
        {
            this.SettingsUserControl = new DefaultPositionPredictionUserControl(this.settings);
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}