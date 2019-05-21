using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalDataTypes;
using System.ComponentModel;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VideoSource.StaticImageVideoSource
{
    public class StaticImageVideoSource : IVideoSource
    {
        private readonly StaticImageVideoSourceUserControl _UserControl;
        private bool _Running = false;

        public StaticImageVideoSource()
        {
            _UserControl = new StaticImageVideoSourceUserControl();
            Game.Game.GameStarted += Game_GameStarted;
        }

        private void Game_GameStarted(object sender, EventArgs e)
        {
            StartAcquisition();
        }

        private System.Threading.Timer _Timer;

        private void timerCallback(object info)
        {
            if (_Running)
            {
                _UserControl.SetImage(RgbImage);
                if (this.NewImage != null)
                    this.NewImage(this, new NewImageEventArgs(RgbImage));
            }
        }

        public IImage RawImage
        {
            get; private set;
        }

        public IImage RgbImage
        {
            get; private set;
        }

        public UserControl SettingsUserControl
        {
            get
            {
                return _UserControl;
            }
        }

        public bool Acquiring
        {
            get
            {
                return _Running;
            }
        }

        public event EventHandler<NewImageEventArgs> NewImage;

        public void Dispose()
        {
            Game.Game.GameStarted -= Game_GameStarted;
        }

        public void GetNewImage()
        {
            return;
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
                var bmp = (Bitmap)Bitmap.FromFile(fileName);
                RawImage = new Image<Rgb, byte>(bmp);
                RgbImage = RawImage;
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
            if (!_Running)
            {
                _Running = true;
                //Settings for around 30 fps.
                _Timer = new System.Threading.Timer(timerCallback, null, 0, 21);
            }
        }

        public void StopAcquisition()
        {
            if (_Running)
            {
                _Running = false;
                if (_Timer != null)
                    _Timer.Dispose();
                _Timer = null;
                _Running = false;
            }
        }
    }
}
