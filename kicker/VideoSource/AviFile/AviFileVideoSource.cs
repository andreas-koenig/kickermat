using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using GlobalDataTypes;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace VideoSource.AviFile
{
    public class AviFileVideoSource : IVideoSource
    {
        private VideoCapture _VideoCapture;
        private readonly BackgroundWorker _BackgroundWorker;
        private bool _Acquiring = false;
        private string _FileName = string.Empty;
        private readonly Stopwatch _LastFrameStopwatch;
        private readonly AviFileVideoSourceUserControl _UserControl;
        public AviFileVideoSource()
        {
            _LastFrameStopwatch = new Stopwatch();
            _LastFrameStopwatch.Restart();
            _BackgroundWorker = new BackgroundWorker();
            _BackgroundWorker.WorkerSupportsCancellation = true;
            _BackgroundWorker.DoWork += _BackgroundWorker_DoWork;
            _UserControl = new AviFileVideoSourceUserControl();
            Game.Game.GameStarted += Game_GameStarted;
        }

        private void Game_GameStarted(object sender, EventArgs e)
        {
            StartAcquisition();
        }

        private void _BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (_Acquiring && !e.Cancel)
            {
                if (_VideoCapture != null)
                {
                    //Wait until next frame is allowed to be taken in order to get correct FPS
                    while (_LastFrameStopwatch.ElapsedMilliseconds < 1000 / _UserControl.FPS)
                        Thread.Sleep(1);
                    var frame = _VideoCapture.QueryFrame();
                    if (frame == null)
                    {
                        _VideoCapture.Dispose();
                        _VideoCapture = new VideoCapture(_FileName);
                        continue;
                    }
                    _LastFrameStopwatch.Restart();
                    _Image = frame;
                    _UserControl.SetImage(_Image);
                    if (NewImage != null)
                        NewImage(this, new NewImageEventArgs(_Image));
                }
            }
        }

        private Mat _Image = null;
        public IImage RawImage
        {
            get
            {
                return _Image;
            }
        }

        public IImage RgbImage
        {
            get
            {
                return _Image;
            }
        }

        public UserControl SettingsUserControl
        {
            get { return _UserControl; }
        }

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
            Game.Game.GameStarted -= Game_GameStarted;
            StopAcquisition();

            if (_VideoCapture != null)
            {
                _VideoCapture.Dispose();
                _VideoCapture = null;
            }
            if (_Image != null)
            {
                _Image.Dispose();
                _Image = null;
            }
        }

        public void GetNewImage()
        {
            if (!_Acquiring)
            {
                if (_VideoCapture != null)
                {
                    _Image = _VideoCapture.QueryFrame();
                }
            }
        }

        public void InitUserControl()
        {
        }

        public void LoadConfiguration(string xmlFileName)
        {
        }

        public bool LoadVideoSource(string fileName)
        {
            try
            {
                if (_VideoCapture != null)
                {
                    if (_Acquiring)
                        StopAcquisition();
                    _VideoCapture.Dispose();
                }
                _FileName = fileName;
                _VideoCapture = new VideoCapture(_FileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void SaveConfiguration(string xmlFileName)
        {
        }

        public void StartAcquisition()
        {
            if (!_Acquiring)
            {
                _Acquiring = true;
                if (!_BackgroundWorker.IsBusy)
                    _BackgroundWorker.RunWorkerAsync();
            }
        }

        public void StopAcquisition()
        {
            if (_Acquiring)
            {
                _Acquiring = false;
                _BackgroundWorker.CancelAsync();
                while (_BackgroundWorker.IsBusy)
                    Application.DoEvents();
            }
        }
    }
}
