using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VideoSource.ProsilicaGE680_Vimba
{
    public partial class ProsilicaGE680_VimbaUserControl : UserControl
    {
        public ProsilicaGE680_VimbaUserControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        public bool ImageUpdateInProgress { get { return _ImageUpdateInProgress; } }

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
            else if (!_ImageUpdateInProgress && !(this.IsDisposed || this.Disposing))
            {
                try
                {
                    this.BeginInvoke(new MethodInvoker(() => { SetImage(Image); }));
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine("Control has been disposed. Catch exception and do nothing. {}", ex);
                    //Control has been disposed. Catch exception and do nothing.
                }
            }               
        }
    }
}
