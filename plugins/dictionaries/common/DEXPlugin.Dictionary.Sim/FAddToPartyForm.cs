using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.Sim
{
    public partial class FAddToPartyForm : Form
    {
        public FAddToPartyForm(List<int> parties)
        {
            InitializeComponent();
            cbParties.Items.Clear();
            foreach (int p in parties)
            {
                cbParties.Items.Add(p.ToString());
            }
            cbParties.SelectedIndex = -1;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (cbParties.SelectedIndex > -1 && cbParties.SelectedItem != null)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Укажите партию.");
            }
        }
    }
}
