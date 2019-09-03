namespace PositionPrediction.Default
{
    using System;
    using System.Drawing;
    using Game;

    /// <summary>
    /// Implements the default distance prediction which doesn't predict any values.
    /// </summary>
    public sealed class DefaultPositionPrediction : IPositionPrediction, IDisposable
    {
        /// <summary>
        /// Indicates if the instance is already initialized or not.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Internal predicted position.
        /// </summary>        
        private PredictedBallPosition predictedPosition;

        /// <summary>
        /// Internal last detected position.
        /// </summary>
        private PredictedBallPosition lastDetectedPosition;

        private bool PredictionEnabled { get; set; }

        private int MaximumDifference { get; set; }

        private int FramesToPredict { get; set; }





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
        /// Executes the position prediction
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        /// <param name="currentFrame">The current frame.</param>
        public void Run(Position ballPosition, int currentFrame, Rectangle playingarea = new Rectangle())
        {
            if (this.PredictionEnabled == true)
            {
                PredictedBallPosition operatingPosition;
                double multiplier;

                // Initialize values
                if (this.isInitialized == false)
                {
                    if (ballPosition.valid == false)
                    {
                        return;
                    }

                    this.predictedPosition.SetValues(ballPosition, currentFrame, 1);
                    this.lastDetectedPosition.SetValues(ballPosition, currentFrame, 1);
                    this.isInitialized = true;
                    return;
                }

                // Is a delta because of image-noise needed here ?
                // Ball hat sich nicht bewegt
                if ((ballPosition.xPosition == this.lastDetectedPosition.xPosition) &&
                    (ballPosition.yPosition == this.lastDetectedPosition.yPosition) && (ballPosition.valid == true))
                {
                    predictedPosition.SetValues(ballPosition, currentFrame, predictedPosition.FrameDifference);
                    // keine Bewegung
                    this.predictedPosition.Direction.XPosition = 0;
                    this.predictedPosition.Direction.YPosition = 0;

                    return;
                }

                /* Mit welchen Wert wird gerechnet(Rechenposition)
                 * Falls der Zeitunterschied vom aktuellen Frame zum Frame der vorletzten(also nicht der aktuellen) erkannten Ballposition größer als die Maximum Difference ist wird mit dem letzen vorhergesagten Wert gerechnet  
                 * sonst mit der vorletzten erkannten Ballposition
                 */

                if (currentFrame - this.lastDetectedPosition.FrameNumber > this.MaximumDifference)
                {
                    operatingPosition = (PredictedBallPosition)this.predictedPosition.Clone();
                }
                else
                {
                    operatingPosition = (PredictedBallPosition)this.lastDetectedPosition.Clone();
                }

                // Ball wurde von Ballerkennung erkannt
                if (ballPosition.valid == true)
                {
                    // Frameunterschied
                    operatingPosition.FrameDifference = currentFrame - operatingPosition.FrameNumber;

                    // Streckung / Stauchung
                    multiplier = (double)this.FramesToPredict / operatingPosition.FrameDifference;

                    // Berechnung des Richtungsvektors aus Position der Ballerkennung und Rechenposition
                    operatingPosition.Direction.XPosition = ballPosition.xPosition - operatingPosition.xPosition;
                    operatingPosition.Direction.YPosition = ballPosition.yPosition - operatingPosition.yPosition;

                    try
                    {
                        // vorhergesagte Position = Richtungsvektor * Streckung bzw. Stauchung + Ballposition
                        var predictedX =
                            System.Convert.ToInt32(operatingPosition.Direction.XPosition * multiplier) +
                            ballPosition.xPosition;
                        var predictedY =
                            System.Convert.ToInt32(operatingPosition.Direction.YPosition * multiplier) +
                            ballPosition.yPosition;
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
                            operatingPosition.xPosition = predictedX;
                            operatingPosition.yPosition = predictedY;
                        }
                        else
                        {
                            operatingPosition.xPosition = predictedX;
                            operatingPosition.yPosition = predictedY;
                        }
                    }
#pragma warning disable 168
                    catch (OverflowException e)
#pragma warning restore 168
                    {
                        operatingPosition.xPosition = ballPosition.xPosition;
                        operatingPosition.yPosition = ballPosition.yPosition;
                    }

                    operatingPosition.valid = true;
                    this.lastDetectedPosition.ResetValues(ballPosition, operatingPosition.Direction, operatingPosition.FrameDifference);
                    this.lastDetectedPosition.FrameNumber = operatingPosition.FrameNumber = currentFrame;
                }
                else
                {
                    // Ball wurde nicht erkannt
                    multiplier = ((double)this.FramesToPredict + currentFrame - operatingPosition.FrameNumber) /
                                 operatingPosition.FrameDifference;
                    try
                    {
                        // vorhergesagte Position = Richtungsvektor * Streckung bzw. Stauchung + Ballposition
                        var predictedX =
                            System.Convert.ToInt32(operatingPosition.Direction.XPosition * multiplier) +
                            ballPosition.xPosition;
                        var predictedY =
                            System.Convert.ToInt32(operatingPosition.Direction.YPosition * multiplier) +
                            ballPosition.yPosition;
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
                            operatingPosition.xPosition = predictedX;
                            operatingPosition.yPosition = predictedY;
                        }
                        else
                        {
                            operatingPosition.xPosition = predictedX;
                            operatingPosition.yPosition = predictedY;
                        }
                    }
#pragma warning disable 168
                    catch (OverflowException e)
#pragma warning restore 168
                    {
                        operatingPosition.xPosition = ballPosition.xPosition;
                        operatingPosition.yPosition = ballPosition.yPosition;
                    }

                    operatingPosition.valid = true;
                    operatingPosition.FrameNumber = currentFrame;
                }

                this.predictedPosition = (PredictedBallPosition)operatingPosition.Clone();
            }
            else
            {
                this.predictedPosition.valid = ballPosition.valid;
                this.predictedPosition.inPlayingArea = ballPosition.inPlayingArea;
                this.predictedPosition.xPosition = ballPosition.xPosition;
                this.predictedPosition.yPosition = ballPosition.yPosition;
            }
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
