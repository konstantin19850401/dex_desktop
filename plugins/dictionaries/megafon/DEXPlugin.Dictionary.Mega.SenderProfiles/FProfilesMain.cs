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

namespace DEXPlugin.Dictionary.Mega.SenderProfiles
{
    public partial class FProfilesMain : Form
    {

        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public FProfilesMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvProfiles.AutoGenerateColumns = false;
            dgvProfiles.DataSource = bs;
            GetProfilesDataAdapter();
            RefreshProfiles();
        }

        void GetProfilesDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `sendprofiles`");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshProfiles()
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
                    GetProfilesDataAdapter();
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
                dgvProfiles.Columns.Clear();

                dgvProfiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        pname,
                        plogin,
                        ppassword,
                        plink,
                        psubscribers
                });

                pname.DataPropertyName = "pname";
                plogin.DataPropertyName = "plogin";
                ppassword.DataPropertyName = "ppassword";
                plink.DataPropertyName = "plink";
                psubscribers.DataPropertyName = "psubscribers";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу профилей отправки.");
            }
        }

        void UpdateProfiles()
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
                MessageBox.Show("Невозможно обновить таблицу профилей отправки.");
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshProfiles();
        }

        private void dgvProfiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FProfilesEd ProfilesEd = new FProfilesEd();
            ProfilesEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (ProfilesEd.ShowDialog() == DialogResult.OK)
            {
                UpdateProfiles();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FProfilesEd ProfilesEd = new FProfilesEd();
            ProfilesEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (ProfilesEd.ShowDialog() == DialogResult.OK)
            {
                UpdateProfiles();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ((DataRowView)bs.Current).Row.Delete();
                    UpdateProfiles();
                }
            }
            catch (Exception)
            {
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            dgvProfiles_CellDoubleClick(null, null);
        }
    }
}
