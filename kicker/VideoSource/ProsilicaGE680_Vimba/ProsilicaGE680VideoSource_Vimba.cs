namespace VideoSource.ProsilicaGE680_Vimba
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using AVT.VmbAPINET;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using GlobalDataTypes;
    using Game;
    using System.Threading;

    /// <summary>
    /// The Program class 
    /// </summary>
    public class ProsilicaGE680CVideoSource_Vimba : IVideoSource, IDisposable
    {                             // The camera ID
        //System.Drawing.Image img;
        IImage imgRgb = new Image<Rgb, byte>(640, 480);
        ProsilicaGE680_VimbaUserControl _Control;
        private Vimba _Vimba = null;
        private Camera _Camera = null;
        /// <summary>
        /// Flag to remember if acquisition is running
        /// </summary>
        private bool _Acquiring = false;

        public ProsilicaGE680CVideoSource_Vimba()
        {

            // Create a new Vimba entry object
            try
            {
                _Vimba = new Vimba();
                _Vimba.Startup(); // Startup API
                _Control = new ProsilicaGE680_VimbaUserControl();
                Game.GameStarted += Game_GameStarted;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void _Camera_OnFrameReceived(Frame frame)
        {
            if (_Acquiring)
            {
                try
                {
                    // Check if the image is valid
                    if (VmbFrameStatusType.VmbFrameStatusComplete != frame.ReceiveStatus)
                    {
                        throw new Exception("Invalid frame received. Reason: " + frame.ReceiveStatus.ToString());
                    }
                    Bitmap bmp = null;
                    frame.Fill(ref bmp);
                    this.imgRgb = new Image<Rgb, byte>(bmp);
                    if (NewImage != null)
                        NewImage(this, new NewImageEventArgs(this.imgRgb));
                    if (!_Control.ImageUpdateInProgress)
                        ThreadPool.QueueUserWorkItem(new WaitCallback((p) => { _Control.SetImage(this.imgRgb); }));
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (_Acquiring)
                    {
                        try
                        {
                            //Return Frame to Camera;
                            _Camera.QueueFrame(frame);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        private void Game_GameStarted(object sender, EventArgs e)
        {
            StartAcquisition();
        }

        public IImage RawImage
        {
            get
            {
                return imgRgb;
            }
        }

        public IImage RgbImage
        {
            get
            {
                return imgRgb;
            }
        }

        public UserControl SettingsUserControl => _Control;

        public bool Acquiring
        {
            get
            {
                return _Acquiring;
            }
        }

        public event EventHandler<NewImageEventArgs> NewImage;

        public void Dispose()
        {
            try
            {
                Game.GameStarted -= Game_GameStarted;
                try
                {
                    releaseCamera();
                }
                finally
                {
                    if (_Vimba != null)
                        _Vimba.Shutdown();
                }
            }
            finally
            {
                _Vimba = null;
            }
        }

        public void GetNewImage()
        {
            if (!_Acquiring)
            {
                StartAcquisition();
                //put Thread to sleep in order for the camera to actually grab an image.
                Thread.Sleep(200);
            }
            return;
        }

        public void InitUserControl()
        {
            _Control = new ProsilicaGE680_VimbaUserControl();
        }

        public void LoadConfiguration(string xmlFileName)
        {

        }

        public bool LoadVideoSource(string fileName)
        {
            return true;
        }

        public void SaveConfiguration(string xmlFileName)
        {
            ;
        }

        public void StartAcquisition()
        {
            try
            {
                if (_Acquiring)
                    return;
                if (_Vimba.Cameras.Count > 0)
                    _Camera = _Vimba.Cameras[0];
                if (_Camera == null)
                    throw new ArgumentException("No Cameras Available!");
                _Camera.Open(VmbAccessModeType.VmbAccessModeFull);
                _Camera.OnFrameReceived += _Camera_OnFrameReceived;
                var features = _Camera.Features;
                var feature = features["PixelFormat"];
                feature.EnumValue = "RGB8Packed";
                _Acquiring = true;
                // Set the GeV packet size to the highest possible value
                // (In this example we do not test whether this cam actually is a GigE cam)
                try
                {
                    this._Camera.Features["GVSPAdjustPacketSize"].RunCommand();
                    while (false == this._Camera.Features["GVSPAdjustPacketSize"].IsCommandDone())
                    {
                        // Do Nothing
                    }
                }
                catch
                {
                    // Do Nothing
                }
                try
                {
                    _Camera.StartContinuousImageAcquisition(3);
                }
                catch (Exception ex)
                {
                    releaseCamera();
                }

            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void releaseCamera()
        {
            if (_Camera != null)
            {
                try
                {
                    try
                    {
                        try
                        {
                            _Camera.OnFrameReceived -= _Camera_OnFrameReceived;
                        }
                        finally
                        {
                            if (_Acquiring)
                            {
                                _Acquiring = false;
                                Thread.Sleep(100);
                                //_Camera.EndCapture();
                                _Camera.StopContinuousImageAcquisition();
                            }
                        }
                    }
                    finally
                    {
                        _Camera.Close();
                    }
                }
                finally
                {
                    _Camera = null;
                }
            }
        }


        public void StopAcquisition()
        {
            if (_Acquiring)
            {
                releaseCamera();
                _Acquiring = false;
            }
        }
    }
}