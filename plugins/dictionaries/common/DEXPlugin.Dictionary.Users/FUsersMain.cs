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

namespace DEXPlugin.Dictionary.Users
{
    public partial class FUsersMain : Form
    {

        public Object toolbox;
        MySqlDataAdapter da;
        MySqlCommandBuilder cb;
        BindingSource bs;

        public FUsersMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bs = new BindingSource();
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.DataSource = bs;
            GetUsersDataAdapter();
            RefreshUsers();
        }

        void GetUsersDataAdapter()
        {
            da = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `users` where login <> 'SYSTEM'");
            cb = new MySqlCommandBuilder(da);
        }

        public void RefreshUsers()
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
                    GetUsersDataAdapter();
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
                dgvUsers.Columns.Clear();

                dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                        status, login, title, datecreated});
                status.DataPropertyName = "status";
                login.DataPropertyName = "login";
                title.DataPropertyName = "title";
                datecreated.DataPropertyName = "datecreated";

                Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно обновить таблицу пользователей.");
            }
        }

        void UpdateUsers()
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
                MessageBox.Show("Невозможно обновить таблицу пользователей.");
            }
        }

        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns[e.ColumnIndex].Name == "datecreated")
            {
                if (e.Value != null)
                {
                    string old = e.Value.ToString();
                    e.Value = string.Format("{0}.{1}.{2} {3}:{4}:{5}",
                        old.Substring(6, 2), old.Substring(4, 2), old.Substring(0, 4),
                        old.Substring(8, 2), old.Substring(10, 2), old.Substring(12, 2)
                        );
                }
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            FUsersEd UsersEd = new FUsersEd();
            UsersEd.InitForm(toolbox, (DataTable)bs.DataSource, ((DataRowView)bs.Current).Row);
            if (UsersEd.ShowDialog() == DialogResult.OK)
            {
                IDEXUserData dud = (IDEXUserData)toolbox;
                if (dud.UID.Equals(((DataRowView)bs.Current).Row["uid"].ToString()))
                {
                    MessageBox.Show("Внимание!\nДля того, чтобы настройки текущего пользователя вступили в силу,\nнеобходимо перезагрузить приложение.");
                }
                UpdateUsers();
            }
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            FUsersEd UsersEd = new FUsersEd();
            UsersEd.InitForm(toolbox, (DataTable)bs.DataSource, null);
            if (UsersEd.ShowDialog() == DialogResult.OK)
            {
                UpdateUsers();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string er = "";
                IDEXData d = (IDEXData)toolbox;
                if (d.getDataReference("users", ((DataRowView)bs.Current).Row["uid"].ToString()) > 0)
                {
                    er = "* На данного пользователя ссылаются записи в базе данных.\n";
                }

                if (er == "")
                {
                    if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ((DataRowView)bs.Current).Row.Delete();
                        UpdateUsers();
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

        private void bStatus_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow r = ((DataRowView)bs.Current).Row;
                if (r != null)
                {
                    r.BeginEdit();
                    bool v = !bool.Parse(r["status"].ToString());
                    r["status"] = v.ToString();
                    r.EndEdit();
                    UpdateUsers();
                }
            }
            catch (Exception)
            {
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshUsers();
        }

        private void dgvUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bEdit_Click(null, null);
        }
    }
}
