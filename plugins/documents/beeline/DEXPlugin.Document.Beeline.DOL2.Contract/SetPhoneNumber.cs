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
    public partial class SetPhoneNumber : Form
    {
        public SetPhoneNumber()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (tbNewNumber.Text.Length == 10 && tbDocNum.Text.Length > 10)
            {
                this.Close();
            }
        }
    }
}
