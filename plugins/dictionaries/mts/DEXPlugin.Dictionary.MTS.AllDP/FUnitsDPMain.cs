using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.MTS.AllDP
{
    public partial class FUnitsDPMain : Form
    {
        public Object toolbox;
        BindingSource bs;


        public FUnitsDPMain(Object ptoolbox)
        {
            InitializeComponent();
            toolbox = ptoolbox;
            bs = new BindingSource();
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = bs;
            refresh();
        }


        public void refresh()
        {
            ListSortDirection lastSortDirection = ListSortDirection.Ascending;
            string lastSortName = null;
            if (dgv.SortedColumn != null)
            {
                lastSortName = dgv.SortedColumn.Name;
                if (dgv.SortOrder != SortOrder.Ascending) lastSortDirection = ListSortDirection.Descending;
            }
            int id = -1;
            try
            {
                id = Convert.ToInt32(((DataRowView)bs.Current).Row["id"]);
            }
            catch (Exception) { }

            // select mud.*, u.title from `mts_units_dp` mud, `units` u where mud.uid = u.uid
            // select  FROM `mts_units_dp` 

            bs.DataSource = ((IDEXData)toolbox).getQuery("select * from `mts_dp_all`");

            try
            {
                if (lastSortName != null)
                {
                    dgv.Sort(dgv.Columns[lastSortName], lastSortDirection);
                }

                if (id > -1)
                {
                    for (int i = 0; i < dgv.Rows.Count; ++i)
                    {
                        if (Convert.ToInt32(dgv.Rows[i].Cells["id"].Value) == id)
                        {
                            dgv.CurrentCell = dgv.Rows[i].Cells[0];
                            break;
                        }
                    }
                }
            }
            catch (Exception) { }

        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FUnitsDPEd UnitsDPEd = new FUnitsDPEd(toolbox, ((DataRowView)bs.Current).Row);
            if (UnitsDPEd.ShowDialog() == DialogResult.OK)
            {
                refresh();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FUnitsDPEd UnitsDPEd = new FUnitsDPEd(toolbox, null);
            if (UnitsDPEd.ShowDialog() == DialogResult.OK)
            {
                refresh();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string er = "";
                IDEXData d = (IDEXData)toolbox;

                if (er == "")
                {
                    if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ((IDEXData)toolbox).runQuery("delete from `mts_dp_all` where id = {0}", ((DataRowView)bs.Current).Row["id"].ToString());
                        refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Удаление записи невозможно по следующим причинам:\n\n" + er);
                }
            }
            catch (Exception)
            {
            }

        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            dgv_CellDoubleClick(null, null);
        }

        private void bCheck_Click(object sender, EventArgs e)
        {
            FCheckDP CheckDp = new FCheckDP(toolbox);
            if ( CheckDp.ShowDialog() == DialogResult.OK )
            {
                refresh();
            }
        }
    }
}
