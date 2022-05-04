using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class SelectUnit : Form
    {
        public SelectUnit(DataTable dt, string owner_id)
        {
            InitializeComponent();
            StringTagItem.UpdateCombo(cbUnits, dt, null, "uid", "title", false);
            StringTagItem.SelectByTag(cbUnits, owner_id, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
