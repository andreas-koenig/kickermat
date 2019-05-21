using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;

namespace VideoSource.AviFile
{
    public partial class AviFileVideoSourceUserControl : UserControl
    {
        public AviFileVideoSourceUserControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private bool _ImageUpdateInProgress = false;
        public void SetImage(IImage Image)
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
                    imageBox1.Image = Image;
                }
                finally
                {
                    _ImageUpdateInProgress = false;
                }
            }
            else if (!_ImageUpdateInProgress)
                this.BeginInvoke(new MethodInvoker(() => { SetImage(Image); }));
        }

        private int _FPS = 60;

        public int FPS { get { return _FPS; } }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _FPS = (int)this.numericUpDown1.Value;
        }
    }
}
