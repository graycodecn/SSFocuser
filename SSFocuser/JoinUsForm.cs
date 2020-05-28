using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.SSFocuser
{
    public partial class JoinUsForm : Form
    {
        public JoinUsForm()
        {
            InitializeComponent();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.fhxy.com");
            System.Diagnostics.Process.Start("http://www.graycode.cn");
        }

    }
}
