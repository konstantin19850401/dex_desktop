using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Journalhook.Beeline.AssignDPCode
{
    public partial class JHFormDPCode : Form
    {
        public string resultCode = "";

        public JHFormDPCode()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            try
            {
                string s = tbCode.Text.Trim();
                //if (s.Length > 0) int.Parse(s);
                resultCode = s;
                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show("Можно указывать только цифры");
            }
            catch (OverflowException)
            {
                MessageBox.Show("Код точки должен быть семизначным");
            }
                
        }

        private void JHFormDPCode_Shown(object sender, EventArgs e)
        {
            tbCode.Focus();
        }

    }
}
