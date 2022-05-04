using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace dexol
{
    public partial class SelectSchemeForm : Form
    {
        public int selected;

        public SelectSchemeForm(SimpleXML[] schemes)
        {
            InitializeComponent();

            selected = -1;

            lbSchemeId.Items.Clear();

            int hk = 1;
            
            foreach (SimpleXML scheme in schemes)
            {
                lbSchemeId.Items.Add(string.Format("F{0} - {1}", hk, scheme["Title"].Text));
                hk++;
            }
        }

        private void SelectSchemeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F10)
            {
                int sel = e.KeyCode - Keys.F1;
                if (sel >= 0 && sel < lbSchemeId.Items.Count)
                {
                    selected = sel;
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
