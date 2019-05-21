using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameController
{
    public partial class EmguControllerUserControl : UserControl
    {
        public EmguControllerUserControl()
        {
            InitializeComponent();
        }
        public void SetSettings(EmguGameControllerSettings Settings)
        {
            this.propertyGrid1.SelectedObject = Settings;
        }
    }
}
