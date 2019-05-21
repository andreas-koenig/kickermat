using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV;

namespace VideoSource.StaticImageVideoSource
{
    public partial class StaticImageVideoSourceUserControl : UserControl
    {
        public StaticImageVideoSourceUserControl()
        {
            InitializeComponent();
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
    }
}
