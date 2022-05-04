using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class SetupForm : Form
    {
        public SetupForm()
        {
            InitializeComponent();
        }

        private void SetupForm_Shown(object sender, EventArgs e)
        {
            cbSuggestFIO.Focus();
        }
    }
}
