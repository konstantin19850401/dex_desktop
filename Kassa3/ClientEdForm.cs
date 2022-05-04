using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public partial class ClientEdForm : Form
    {
        Tools tools;

        public long id, cat_id;
        string curShortcut, curTitle;
        bool shortcutChanged, isNew;
//        MySqlCommand cmdShortcut;

        public ClientEdForm()
        {
            InitializeComponent();
            tools = Tools.instance;
        }

        private object cmdShortcut(string shortcut)
        {
            using (DbCommand ret = Db.command("select count(id) cpid from client_data where shortcut = @shortcut and id <> @id"))
            {
                Db.param(ret, "id", id);
                Db.param(ret, "shortcut", shortcut);
                return ret.ExecuteScalar();
            }
        }

        public void initForm(bool isNew, long id, long cat_id, string shortcut, string title)
        {
            shortcutChanged = false;
            lShortcutStatus.Text = "";
            this.isNew = isNew;
            this.id = id;
            this.cat_id = cat_id;
            curShortcut = shortcut;
            curTitle = title;
            tbShortcut.Text = shortcut;
            tbTitle.Text = title;
            //cmdShortcut = new MySqlCommand("select count(id) cpid from client_data where shortcut = @shortcut and id <> @id", tools.connection);
            //tools.SetDbParameter(cmdShortcut, "id", id);
        }

        bool tickBusy = false;

        private void tShortcutChanged_Tick(object sender, EventArgs e)
        {
            if (tickBusy) return;
            if (shortcutChanged)
            {
                shortcutChanged = false;
                lShortcutStatus.SuspendLayout();
                lShortcutStatus.Text = "";
                tickBusy = true;
                try
                {
                    string skt = tbShortcut.Text.Trim();

                    if (skt.CompareTo(curShortcut) != 0)
                    {
                        lShortcutStatus.Text = "Код свободен";
                        lShortcutStatus.ForeColor = Color.Green;

                        //tools.SetDbParameter(cmdShortcut, "shortcut", skt);

                        //object sc = cmdShortcut.ExecuteScalar();

                        object sc = cmdShortcut(skt);

                        if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0)
                        {
                            lShortcutStatus.Text = "Код занят";
                            lShortcutStatus.ForeColor = Color.Red;
                        }
                    }
                }
                catch (Exception) { }
                lShortcutStatus.ResumeLayout();
                tickBusy = false;
            }
        }

        private void ClientEdForm_Activated(object sender, EventArgs e)
        {
            tShortcutChanged.Enabled = true;
        }

        private void ClientEdForm_Deactivate(object sender, EventArgs e)
        {
            tShortcutChanged.Enabled = false;
        }

        private void tbShortcut_TextChanged(object sender, EventArgs e)
        {
            shortcutChanged = true;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            if (tbShortcut.Text.Trim().Length < 1) er += "* Код не указан\n";
            else
            {
                /*
                tools.SetDbParameter(cmdShortcut, "shortcut", tbShortcut.Text.Trim());
                object sc = cmdShortcut.ExecuteScalar();
                 */
                object sc = cmdShortcut(tbShortcut.Text.Trim());
                if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0)
                {
                    er += "* Код занят\n";
                }
            }

            if (tbTitle.Text.Trim().Length < 1) er += "Наименование не указано\n";

            if (er != "") MessageBox.Show("Ошибки:\n\n" + er);
            else
            {
                if (isNew || (!tbShortcut.Text.Trim().Equals(curShortcut) || !tbTitle.Text.Trim().Equals(curTitle)))
                {
                    string sql = (isNew) ?
                        "insert into client_data (cat_id, title, shortcut) values (@cat_id, @title, @shortcut)" :
                        "update client_data set title = @title, shortcut = @shortcut where id = @id";

                    using (DbCommand cmd = Db.command(sql))
                    {
                        if (isNew)
                        {
                            /*
                            cmd = new MySqlCommand("insert into client_data (cat_id, title, shortcut) values (@cat_id, @title, @shortcut)", tools.connection);
                            tools.SetDbParameter(cmd, "cat_id", cat_id);
                             */
                            Db.param(cmd, "cat_id", cat_id);
                        }
                        else
                        {
                            /*
                            cmd = new MySqlCommand("update client_data set title = @title, shortcut = @shortcut where id = @id", tools.connection);
                            tools.SetDbParameter(cmd, "id", id);
                             */
                            Db.param(cmd, "id", id);
                        }
                        /*
                        tools.SetDbParameter(cmd, "title", tbTitle.Text.Trim());
                        tools.SetDbParameter(cmd, "shortcut", tbShortcut.Text.Trim());
                         */
                        Db.param(cmd, "title", tbTitle.Text.Trim());
                        Db.param(cmd, "shortcut", tbShortcut.Text.Trim());
                        cmd.ExecuteNonQuery();

                        //if (isNew) id = cmd.LastInsertedId;
                        if (isNew) id = Db.LastInsertedId(cmd, "client_data");
                    }
                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
                DialogResult = DialogResult.OK;
            }
        }
    }
}
