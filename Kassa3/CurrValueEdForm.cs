using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Kassa3
{
    public partial class CurrValueEdForm : Form
    {
        public double returnValue;

        public CurrValueEdForm()
        {
            InitializeComponent();
        }

        private void tbCurrValue_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string value = ((TextBox)sender).Text;
                double v = double.Parse(value, NumberStyles.Float);
                ((TextBox)sender).Text = v.ToString("F2");
                e.Cancel = false;
            }
            catch (Exception)
            {
                e.Cancel = true;
            }

        }

        public void init(string currTitle, double oldValue)
        {
            lCurrTitle.Text = currTitle;
            tbCurrValue.Text = oldValue.ToString();
            returnValue = oldValue;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";
            try
            {
                returnValue = double.Parse(tbCurrValue.Text, NumberStyles.Float);
                if (returnValue <= 0) er = "Стоимость не может быть менее или равной нулю";
                else DialogResult = DialogResult.OK;
            }
            catch (Exception) {
                er = "Неверный формат числа";
            }

            if (er != "") MessageBox.Show(er);
        }
    }
}
