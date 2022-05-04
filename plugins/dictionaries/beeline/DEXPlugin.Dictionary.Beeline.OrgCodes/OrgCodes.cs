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

namespace DEXPlugin.Dictionary.Beeline.OrgCodes
{
    public partial class OrgCodes : Form
    {
        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public OrgCodes()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvOrgCodes.AutoGenerateColumns = false;
            dgvOrgCodes.DataSource = bs;
            GetOrgCodesDataAdapter();
            RefreshOrgCodes();
        }

        void GetOrgCodesDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `org_codes`");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshOrgCodes()
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
                    GetOrgCodesDataAdapter();
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
                dgvOrgCodes.Columns.Clear();

                dgvOrgCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        code,
                        title,
                });

                code.DataPropertyName = "code";
                title.DataPropertyName = "title";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу населенных пунктов.");
            }
        }

        void UpdateOrgCodes()
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
                MessageBox.Show("Невозможно обновить таблицу населенных пунктов.");
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshOrgCodes();
        }

        private void dgvOrgCodes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OrgCodesEd CityEd = new OrgCodesEd();
            CityEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (CityEd.ShowDialog() == DialogResult.OK)
            {
                UpdateOrgCodes();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            OrgCodesEd CityEd = new OrgCodesEd();
            CityEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (CityEd.ShowDialog() == DialogResult.OK)
            {
                UpdateOrgCodes();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ((DataRowView)bs.Current).Row.Delete();
                    UpdateOrgCodes();
                }
            }
            catch (Exception)
            {
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            dgvOrgCodes_CellDoubleClick(null, null);
        }

      
    }
}
