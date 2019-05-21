namespace ObjectDetection
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Coach;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using GlobalDataTypes;
    using ObjectDetection;
    using PluginSystem;
    using Utilities;

    /// <summary>
    /// Default user control for controlling an image processor.
    /// </summary>
    public sealed partial class BasicObjectDetectionUserControl : UserControl
    {
        /// <summary>
        /// Reference to the used object detection.
        /// </summary>
        private readonly IBasicObjectDetection objectDetection;

        /// <summary>
        /// Mapping of the label handles of the local display to the main display.
        /// </summary>
        private readonly Dictionary<int, int> mainLabels = new Dictionary<int, int>();

        /// <summary>
        /// Stores if the set state of the controls for consistency between events.
        /// </summary>
        private bool enableControls = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicObjectDetectionUserControl"/> class.
        /// </summary>
        /// <param name="managedInstance">The managed instance.</param>
        /// <param name="objectSearchUserControl">The object search user control.</param>
        internal BasicObjectDetectionUserControl(IBasicObjectDetection managedInstance, UserControl objectSearchUserControl)
        {
            this.InitializeComponent();
            this.objectDetection = managedInstance;
            objectSearchUserControl.Dock = DockStyle.Fill;
            this.panelModuleSpecificControls.Controls.Add(objectSearchUserControl);
            this.ApplySettingsToUserInterface(true);
        }
        private bool _ImageUpdateInProgress = false;
        /// <summary>
        /// Updates the image.
        /// </summary>
        /// <param name="image">The image.</param>
        public void UpdateImage(IImage image)
        {
            var parent = this.Parent;
            bool visible = true;
            while (parent != null)
            {
                if (parent.Visible == false)
                {
                    visible = false;
                    break;
                }
                parent = parent.Parent;
            }
            //if the Control isnt even visible to the user dont update the image in order to conserve cpu resources.
            if (!visible)
                return;
            if (!InvokeRequired)
            {
                if (_ImageUpdateInProgress)
                    return;
                try
                {
                    _ImageUpdateInProgress = true;
                    updateimage(image);
                }
                finally
                {
                    _ImageUpdateInProgress = false;
                }
            }
            else if (!_ImageUpdateInProgress && !(this.IsDisposed || this.Disposing))
            {
                try
                {
                    this.BeginInvoke(new MethodInvoker(() => { UpdateImage(image); }));
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine("Control has been disposed. Catch exception and do nothing. {}", ex);
                    //Control has been disposed. Catch exception and do nothing.
                }
            }
        }

        private void updateimage(IImage image)
        {
            try
            {
                if (this.objectDetection is IBallDetection)
                {
                    IBallDetection balldetection = ServiceLocator.LocateService<IBallDetection>();
                    //var coach = ServiceLocator.LocateService<ICoach>();
                    if (balldetection != null)
                    {
                        Image<Bgr, byte> bgrImage = new Image<Bgr, byte>(image.Bitmap);
                        var ballPos = balldetection.BallPosition.Clone();
                        //if (coach != null)
                        //{
                        //    var paraBall = ballPos.Clone();
                        //    SwissKnife.ParallaxCorrection(coach.CameraLongZ, coach.PlayerShortZ, coach.PlayingFieldCenter, paraBall);
                        //    bgrImage.Draw(paraBall.BoundingBox, new Bgr(0, 255, 255));
                        //}
                        bgrImage.Draw(ballPos.BoundingBox, new Bgr(0, 0, 255));
                        bgrImage.Draw(balldetection.ObjectSearch.AreaOfInterestForNextSearch, new Bgr(0, 255, 0));
                        bgrImage.Draw(balldetection.PlayingFieldArea, new Bgr(255, 0, 0));
                        this.imageBox1.Image = bgrImage;
                    }
                }
                else if (this.objectDetection is IOwnBarDetection)
                {
                    IOwnBarDetection ownBarDetection = ServiceLocator.LocateService<IOwnBarDetection>();
                    var coach = ServiceLocator.LocateService<ICoach>();

                    if (ownBarDetection != null || coach != null)
                    {
                        Image<Bgr, byte> bgrImage = new Image<Bgr, byte>(image.Bitmap);
                        if (coach != null)
                        {
                            int x = coach.GetBarXPosition(Bar.Keeper);
                            bgrImage.Draw(new LineSegment2D(new Point(x, 0), new Point(x, bgrImage.Height)), new Bgr(255, 255, 0), 2);
                            x = coach.GetBarXPosition(Bar.Defense);
                            bgrImage.Draw(new LineSegment2D(new Point(x, 0), new Point(x, bgrImage.Height)), new Bgr(255, 255, 0), 2);
                            x = coach.GetBarXPosition(Bar.Midfield);
                            bgrImage.Draw(new LineSegment2D(new Point(x, 0), new Point(x, bgrImage.Height)), new Bgr(255, 255, 0), 2);
                            x = coach.GetBarXPosition(Bar.Striker);
                            bgrImage.Draw(new LineSegment2D(new Point(x, 0), new Point(x, bgrImage.Height)), new Bgr(255, 255, 0), 2);
                        }
                        if (ownBarDetection != null)
                        {
                            int i = 0;
                            foreach (Player player in Enum.GetValues(typeof(Player)))
                            {
                                Position pos = ownBarDetection.GetPlayerPosition(player).Clone();
                                //var rect = ownBarDetection.GetPlayerBoundingBox(player);
                                bgrImage.Draw(pos.BoundingBox, new Bgr(0, 0, 255));
                                if (pos != null)
                                {
                                    bgrImage.Draw(player.ShortName(), new Point(pos.BoundingBox.Right, pos.BoundingBox.Bottom), Emgu.CV.CvEnum.FontFace.HersheyPlain, 2, new Bgr(0, 0, 255));
                                }
                                i++;
                            }
                            bgrImage.Draw(ownBarDetection.ObjectSearch.AreaOfInterestForNextSearch, new Bgr(0, 255, 0));
                            bgrImage.Draw(ownBarDetection.PlayingFieldArea, new Bgr(255, 0, 0));
                        }
                        this.imageBox1.Image = bgrImage;
                    }
                    else
                        this.imageBox1.Image = image;
                }
                else if (this.objectDetection is IOpponentBarDetection)
                {
                    IOpponentBarDetection opponentBarDetection = ServiceLocator.LocateService<IOpponentBarDetection>();
                    if (opponentBarDetection != null)
                    {
                        Image<Bgr, byte> bgrImage = new Image<Bgr, byte>(image.Bitmap);
                        int i = 0;
                        foreach (Player player in Enum.GetValues(typeof(Player)))
                        {
                            var rect = opponentBarDetection.ObjectSearch.GetObjectBounds(i);
                            bgrImage.Draw(rect, new Bgr(0, 0, 255));
                            Position pos = opponentBarDetection.GetPlayerPosition(player).Clone();
                            if (pos != null)
                            {
                                bgrImage.Draw(player.ShortName(), new Point(pos.BoundingBox.Right, pos.BoundingBox.Bottom), Emgu.CV.CvEnum.FontFace.HersheyPlain, 2, new Bgr(0, 0, 255));
                            }
                            i++;
                        }
                        bgrImage.Draw(opponentBarDetection.ObjectSearch.AreaOfInterestForNextSearch, new Bgr(0, 255, 0));
                        bgrImage.Draw(opponentBarDetection.PlayingFieldArea, new Bgr(255, 0, 0));
                        this.imageBox1.Image = bgrImage;
                    }
                    else
                        this.imageBox1.Image = image;
                }
                else
                {
                    this.imageBox1.Image = image;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// Lädt alle Einstellungen der verwalteten Objekterkennung in die Benutzeroberfläche.
        /// </summary>
        /// <param name="enableControls">If the controls should be enabled at all.</param>
        internal void ApplySettingsToUserInterface(bool enableControls)
        {
            this.enableControls = enableControls;
            this.checkBoxEnableDetection.Checked = this.objectDetection.Settings.DetectionEnabled;
            this.checkBoxUpdateDisplay.Checked = this.objectDetection.Settings.UpdateDisplay;

            this.checkBoxEnableDetection.Enabled = this.enableControls;
            this.buttonExecute.Enabled = this.enableControls;
            this.checkBoxUpdateDisplay.Enabled = this.enableControls;

            // Muss zusätzlich aufgerufen werden, da kein CheckedChanged-Event aufgerufen wird
            // wenn der vorherige Checked-Status gleich dem neuen Checked-Status ist.
            this.ApplyChkEnableDetection();
            this.ApplyChkUpdateDisplay();
        }

        /// <summary>
        /// Sets the enabled state of the user control and the object detection depending on the state of the check box.
        /// </summary>
        private void ApplyChkEnableDetection()
        {
            this.panelModuleSpecificControls.Enabled = this.checkBoxEnableDetection.Checked && this.enableControls;
            this.objectDetection.Settings.DetectionEnabled = this.checkBoxEnableDetection.Checked;
        }

        /// <summary>
        /// Applies the value of checkBoxUpdateDisplay control.
        /// </summary>
        private void ApplyChkUpdateDisplay()
        {
            if (this.checkBoxUpdateDisplay.Checked)
            {
                this.objectDetection.ObjectSearch.NewBinaryImageCallback = this.UpdateImage;
            }
            else
            {
                this.objectDetection.ObjectSearch.NewBinaryImageCallback = null;
            }

            this.objectDetection.Settings.UpdateDisplay = this.checkBoxUpdateDisplay.Checked;
        }

        /// <summary>
        /// Handles the Click event of the buttonExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ButtonExecute_Click(object sender, EventArgs e)
        {
            this.objectDetection.Execute(null);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxEnableDetection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxEnableDetection_CheckedChanged(object sender, EventArgs e)
        {
            this.ApplyChkEnableDetection();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxUpdateDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxUpdateDisplay_CheckedChanged(object sender, EventArgs e)
        {
            this.ApplyChkUpdateDisplay();
        }
    }
}