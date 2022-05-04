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

namespace DEXPlugin.Dictionary.Regions
{
    public partial class FRegionsMain : Form
    {
        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public FRegionsMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvRegions.AutoGenerateColumns = false;
            dgvRegions.DataSource = bs;
            GetRegionsDataAdapter();
            RefreshRegions();
        }

        void GetRegionsDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `um_regions`");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshRegions()
        {
            Cursor = Cursors.WaitCursor;

            DataTable t = new DataTable();
            try
            {
                da.Fill(t);
            }
            catch (Exception)
            {
                try
                {
                    GetRegionsDataAdapter();
                    da.Fill(t);
                }
                catch (Exception)
                {
                    t = null;
                }

            }

            bs.DataSource = t;

            if (t != null)
            {
                dgvRegions.Columns.Clear();

                dgvRegions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        region_id,
                        title
                });

                title.DataPropertyName = "title";
                region_id.DataPropertyName = "region_id";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу регионов.");
            }
        }

        void UpdateRegions()
        {
            Cursor = Cursors.WaitCursor;
            DataTable t = (DataTable)bs.DataSource;
            try
            {
                DataTable ch = t.GetChanges();
                if (ch != null)
                {
                    da.Update(ch);
                    t.AcceptChanges();
                }
                Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                t.RejectChanges();
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу регионов.");
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshRegions();
        }

        private void dgvRegions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FRegionsEd RegionsEd = new FRegionsEd();
            RegionsEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (RegionsEd.ShowDialog() == DialogResult.OK)
            {
                UpdateRegions();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FRegionsEd RegionsEd = new FRegionsEd();
            RegionsEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (RegionsEd.ShowDialog() == DialogResult.OK)
            {
                UpdateRegions();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string er = "";
                IDEXData d = (IDEXData)toolbox;
                if (d.getDataReference("regions", ((DataRowView)bs.Current).Row["region_id"].ToString()) > 0)
                {
                    er = "* На данный регион ссылаются записи в базе данных.\n";
                }

                if (er == "")
                {
                    if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ((DataRowView)bs.Current).Row.Delete();
                        UpdateRegions();
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
            dgvRegions_CellDoubleClick(null, null);
        }

    }
}
