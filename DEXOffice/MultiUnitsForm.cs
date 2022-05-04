using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXOffice
{
    public partial class MultiUnitsForm : Form
    {
        DEXToolBox toolbox;

        public static List<int> selectedIds = new List<int>();

        public MultiUnitsForm()
        {
            InitializeComponent();
            toolbox = DEXToolBox.getToolBox();

            IDEXData d = (IDEXData)toolbox;
            clbUnits.Items.Clear();

            DataTable dt = d.getQuery("select uid, title from `units` order by title");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    StringObjTagItem sti = new StringObjTagItem(row["title"].ToString(), Convert.ToInt32(row["uid"]));
                    clbUnits.Items.Add(sti, selectedIds.Contains(Convert.ToInt32(row["uid"])));
                }
            }

        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (clbUnits.CheckedItems.Count < 1)
            {
                MessageBox.Show("Не выделено ни одного отделения");
                return;
            }

            selectedIds.Clear();
            foreach (StringObjTagItem soti in clbUnits.CheckedItems)
            {
                selectedIds.Add((int)soti.Tag);
            }

            DialogResult = DialogResult.OK;
        }

        private void clbUnits_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool nstate = clbUnits.CheckedItems.Count < clbUnits.Items.Count;
                clbUnits.BeginUpdate();
                try
                {
                    for (int i = 0; i < clbUnits.Items.Count; ++i)
                    {
                        clbUnits.SetItemChecked(i, nstate);
                    }
                }
                finally
                {
                    clbUnits.EndUpdate();
                }
            }
        }
    }
}
