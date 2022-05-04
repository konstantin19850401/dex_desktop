using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Document.Mega.EFD
{
    public partial class SetupForm : Form
    {
        public SetupForm()
        {
            InitializeComponent();
        }

        private void SetupForm_Shown(object sender, EventArgs e)
        {
            cbExpressInput.Focus();
        }
    }
}
