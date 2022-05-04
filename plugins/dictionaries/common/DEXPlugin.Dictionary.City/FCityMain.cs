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

namespace DEXPlugin.Dictionary.City
{
    public partial class FCityMain : Form
    {
        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public FCityMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvCity.AutoGenerateColumns = false;
            dgvCity.DataSource = bs;
            GetCityDataAdapter();
            RefreshCity();
        }

        void GetCityDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `city`");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshCity()
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
                    GetCityDataAdapter();
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
                dgvCity.Columns.Clear();

                dgvCity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        zip,
                        city,
                        region
                });

                zip.DataPropertyName = "zip";
                city.DataPropertyName = "city";
                region.DataPropertyName = "region";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу населенных пунктов.");
            }
        }

        void UpdateCity()
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
            RefreshCity();
        }

        private void dgvCity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FCityEd CityEd = new FCityEd();
            CityEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (CityEd.ShowDialog() == DialogResult.OK)
            {
                UpdateCity();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FCityEd CityEd = new FCityEd();
            CityEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (CityEd.ShowDialog() == DialogResult.OK)
            {
                UpdateCity();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ((DataRowView)bs.Current).Row.Delete();
                    UpdateCity();
                }
            }
            catch (Exception)
            {
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            dgvCity_CellDoubleClick(null, null);
        }
    }
}
