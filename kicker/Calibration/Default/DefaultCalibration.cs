namespace Calibration.Default
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;
    using Calibration.Base;
    using Communication.NetworkLayer.Packets.Udp.Enums;
    using GlobalDataTypes;
    using ObjectDetection;
    using PluginSystem;
    using Utilities;
    using VideoSource;
    using Communication.PlayerControl;
    using Communication.Manager;

    /// <summary>
    /// Default class for calibrating the image processing.
    /// </summary>
    public sealed class DefaultCalibration : BasicCalibration<DefaultCalibrationSettings>
    {
        /// <summary>
        /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
        /// to do the actual calibration.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        /// <param name="minPositions">Out parameter for the minimum positions of the players.</param>
        /// <param name="maxPositions">Out parameter for the maximum positions of the players.</param>
        /// <returns>if the calibration has been successful.</returns>
        protected override bool DoCalibration(object sender, DoWorkEventArgs e, Dictionary<Player, Position> minPositions, Dictionary<Player, Position> maxPositions)
        {
            Dictionary<Player, int> labelHandles = new Dictionary<Player, int>();
            IVideoSource videoSource = ServiceLocator.LocateService<IVideoSource>();
            IOwnBarDetection ownBarDetection = ServiceLocator.LocateService<IOwnBarDetection>();

            // Tell detection that calibration is starting
            ownBarDetection.CalibrationStart();

            // Wait for controller calibration to be finished
            while (Controller.InitStatus != ControllerStatus.Ok)
            {
                videoSource.GetNewImage();
                Thread.Sleep(100);

                // Ab und zu mal schauen, ob der Thread überhaupt noch laufen darf...
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    // ...wenn nicht, dann raus hier
                    e.Cancel = true;
                    return false;
                }
            }

            this.Controller.SetAllAnglesAndPositionsToZero();
            Thread.Sleep(500);

            // Prepare own bar detection for calibration
            ownBarDetection.CalibrationDetection();

            try
            {
                // Eigene Stangenerkennung initialisieren
                // initStep 1: an maximale Position fahren
                // initStep 2: an minimale Position fahren
                // in jedem Schritt jeweils die eigenen Spieler suchen
                //TODO: Exception Handling
                for (uint initStep = 1; initStep <= 2; initStep++)
                {
                    if (initStep == 1)
                    {
                        // Alle Stangen auf maximale Position fahren
                        Controller.MoveAllBarsToMaximumPosition();
                        Thread.Sleep(500);
                    }
                    else
                    {
                        // Alle Stangen auf minimale Position fahren
                        Controller.MoveAllBarsToMinimumPosition();
                        Thread.Sleep(500);
                    }
                }

                for (int runCounter = 0; runCounter < 20; runCounter++)
                {
                    bool allPlayersFound = true;

                    // get a new image, this triggers the image processing and object detection
                    videoSource.GetNewImage();

                    // wait until current detection is finished
                    while (ownBarDetection.IsRunning)
                    {
                        Application.DoEvents();
                    }

                    foreach (Player player in Enum.GetValues(typeof(Player)))
                    {
                        Position pos = ownBarDetection.GetPlayerPosition(player);
                        if (pos == null || !pos.Valid)
                        {
                            allPlayersFound = false;
                        }
                    }

                    if (allPlayersFound)
                    {
                        break;
                    }
                }

                foreach (Player player in Enum.GetValues(typeof(Player)))
                {
                    Position pos = ownBarDetection.GetPlayerPosition(player);
                    //FormImageDisplay.Instance.Labels.Update(labelHandles[player], pos);

                    if (pos == null || !pos.Valid)
                    {
                        //SwissKnife.ShowError(this, "Position of " + player + " not found, please check configuration and try again.");
                        //return false;
                    }
                }

                foreach (Player playerName in Enum.GetValues(typeof(Player)))
                {
                    if (initStep == 1)
                    {
                        var currpos = ownBarDetection.GetPlayerPosition(playerName);
                        maxPositions[playerName] = currpos;
                    }
                    else
                    {
                        var currpos = ownBarDetection.GetPlayerPosition(playerName);
                        minPositions[playerName] = currpos;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO: Logging or rethrow ?
            }
            finally
            {
                // Tell detection that calibration is finished
                ownBarDetection.CalibrationFinished();

            }
            return true;
        }


    }


    /// <summary>
    /// Called by the handler method for the DoWork event of the <c>calibrationThread</c>
    /// after evaluating the calibration values.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
    protected override void AfterCalibration(object sender, DoWorkEventArgs e)
    {
        // Save the calculated values in the settings
        this.Settings.KeeperBarPosition = this.Settings.PlayingFieldWidth - Coach.GetBarXPosition(Bar.Keeper) + this.Settings.PlayingFieldXOffset;
        this.Settings.DefenseBarPosition = this.Settings.PlayingFieldWidth - Coach.GetBarXPosition(Bar.Defense) + this.Settings.PlayingFieldXOffset;
        this.Settings.MidfieldBarPosition = this.Settings.PlayingFieldWidth - Coach.GetBarXPosition(Bar.Midfield) + this.Settings.PlayingFieldXOffset;
        this.Settings.OpponentStrikerBarPosition = Coach.GetBarXPosition(Bar.Striker) - this.Settings.PlayingFieldXOffset;
        this.SettingsUserControl.Refresh();
    }
}
}
