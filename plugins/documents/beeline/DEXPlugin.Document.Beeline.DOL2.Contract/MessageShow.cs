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
    public partial class MessageShow : Form
    {
        public MessageShow(string message, string color, string statusMessage)
        {
            InitializeComponent();
            if (color.Equals("red")) {
                this.Text = statusMessage;
                lbText.Text = message;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
