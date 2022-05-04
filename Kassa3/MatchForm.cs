using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace Kassa3
{
    public partial class MatchForm : Form
    {

        // Справка по регулярным выражениям - одеть в кнопку на форме
        // http://msdn.microsoft.com/ru-ru/library/az24scfc(v=vs.110).aspx

        ImportMatch match;

        public MatchForm(ImportMatch match, Dictionary<string, string> dkeys)
        {
            InitializeComponent();
            this.match = match;

            int sel = -1;

            cbField.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in dkeys)
            {
                int cur = cbField.Items.Add(new StringTagItem(kvp.Value, kvp.Key));
                if (kvp.Key.Equals(match.field)) sel = cur;
            }

            cbField.SelectedIndex = sel;
            cbOp.SelectedIndex = match.operation;
            tbValue.Text = match.match;

            cbField.Focus();
        }

        private void cbField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                if (sender == bOk)
                {
                    bOk.PerformClick();
                }
                else
                {
                    SelectNextControl((Control)sender, true, true, true, false);
                }
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";
            if (cbField.SelectedIndex < 0) er += "* Не указано поле\n";
            if (cbOp.SelectedIndex < 0) er += "* Не указана операция\n";

            if (er == "")
            {
                match.field = ((StringTagItem)cbField.SelectedItem).Tag;
                match.field_title = ((StringTagItem)cbField.SelectedItem).Text;
                match.operation = cbOp.SelectedIndex;
                match.match = tbValue.Text;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }

        }
    }
}
