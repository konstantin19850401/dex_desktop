using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using MySql.Data.MySqlClient;

namespace DEXPlugin.Dictionary.Units
{
    public partial class FUnitsMain : Form
    {
        public Object toolbox;
        BindingSource bs;
        string[] documentstates;

        public FUnitsMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            documentstates = ((IDEXSysData)toolbox).DocumentStatesText;
            bs = new BindingSource();
            dgvUnits.AutoGenerateColumns = false;
            dgvUnits.DataSource = bs;
            RefreshUnits();
        }

        public void RefreshUnits()
        {
            ListSortDirection lastSortDirection = ListSortDirection.Ascending;
            string lastSortName = null;
            if (dgvUnits.SortedColumn != null)
            {
                lastSortName = dgvUnits.SortedColumn.Name;
                if (dgvUnits.SortOrder != SortOrder.Ascending) lastSortDirection = ListSortDirection.Descending;
            }
            int id = -1;
            try
            {
                id = Convert.ToInt32(((DataRowView)bs.Current).Row["id"]);
            }
            catch (Exception) { }
            
            //bs.DataSource = ((IDEXData)toolbox).getTable("units");
            string search = "";
            if (!tbSearch.Text.Equals("")) search = " where title like '%"+tbSearch.Text+"%'";
            bs.DataSource = ((IDEXData)toolbox).getQuery("select * from units" + search);


            try
            {
                if (lastSortName != null)
                {
                    dgvUnits.Sort(dgvUnits.Columns[lastSortName], lastSortDirection);
                }

                if (id > -1)
                {
                    for (int i = 0; i < dgvUnits.Rows.Count; ++i)
                    {
                        if (Convert.ToInt32(dgvUnits.Rows[i].Cells["id"].Value) == id)
                        {
                            dgvUnits.CurrentCell = dgvUnits.Rows[i].Cells[0];
                            break;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshUnits();
        }

        private void bStatus_Click(object sender, EventArgs e)
        {

            try
            {
                DataRow r = ((DataRowView)bs.Current).Row;
                if (r != null)
                {
                    bool v = !bool.Parse(r["status"].ToString());
                    ((IDEXData)toolbox).runQuery("update `units` set status = {0} where id = {1}", v ? 1 : 0, r["id"].ToString());
                    RefreshUnits();
                }
            }
            catch (Exception)
            {
            }
        }



        private void dgvUnits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FUnitsEd UnitsEd = new FUnitsEd();
            UnitsEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (UnitsEd.ShowDialog() == DialogResult.OK)
            {
                RefreshUnits();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            
            FUnitsEd UnitsEd = new FUnitsEd();
            UnitsEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (UnitsEd.ShowDialog() == DialogResult.OK)
            {
                RefreshUnits();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string er = "";
                IDEXData d = (IDEXData)toolbox;
                if (d.getDataReference("units", ((DataRowView)bs.Current).Row["uid"].ToString()) > 0)
                {
                    er = "* На данное отделение ссылаются записи в базе данных.\n";
                }

                if (er == "")
                {
                    if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ((IDEXData)toolbox).runQuery("delete from `units` where id = {0}", ((DataRowView)bs.Current).Row["id"].ToString());
                        RefreshUnits();
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
            dgvUnits_CellDoubleClick(null, null);
        }

        private void dgvUnits_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUnits.Columns[e.ColumnIndex].Name == "documentstate")
            {
                try
                {
                    e.Value = documentstates[int.Parse(e.Value.ToString())];
                }
                catch(Exception)
                {
                    e.Value = "-";
                }
            }

        }


        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                RefreshUnits();
            }
        }

        private void dgvUnits_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            {
                
                String id = ((DataRowView)bs.Current).Row["id"].ToString();
                bool status = !bool.Parse(((DataRowView)bs.Current).Row["status"].ToString());


                ((IDEXData)toolbox).runQuery("update `units` set status = {0} where id = {1}", status ? 1 : 0, id);

                ((DataRowView)bs.Current).Row["status"] = status;
               
             
            }
        }
    }
}
