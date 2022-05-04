using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Mega.Sim
{
    public partial class FPlansMain : Form
    {
        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public FPlansMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvPlans.AutoGenerateColumns = false;
            dgvPlans.DataSource = bs;
            GetPlansDataAdapter();
            RefreshPlans();
        }

        void GetPlansDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `um_plans`");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshPlans()
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
                    GetPlansDataAdapter();
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
                dgvPlans.Columns.Clear();

                dgvPlans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        plan_id, title});
                plan_id.DataPropertyName = "plan_id";
                title.DataPropertyName = "title";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу тарифных планов.");
            }
        }

        void UpdatePlans()
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
                MessageBox.Show("Невозможно обновить таблицу тарифных планов.");
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {

            FPlansEd PlansEd = new FPlansEd();
            PlansEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (PlansEd.ShowDialog() == DialogResult.OK)
            {
                UpdatePlans();
            }
            
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FPlansEd PlansEd = new FPlansEd();
            PlansEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (PlansEd.ShowDialog() == DialogResult.OK)
            {
                UpdatePlans();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string er = "";
                IDEXData d = (IDEXData)toolbox;
                if (d.getDataReference("plans", ((DataRowView)bs.Current).Row["plan_id"].ToString()) > 0)
                {
                    er = "* На данный тарифный план ссылаются записи в базе данных.\n";
                }

                if (er == "")
                {
                    if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ((DataRowView)bs.Current).Row.Delete();
                        UpdatePlans();
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

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshPlans();
        }

        private void dgvPlans_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bEdit_Click(null, null);
        }

    }
}
