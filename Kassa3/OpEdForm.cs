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
    public partial class OpEdForm : Form
    {
        Tools tools;

        public long id;
        string curShortcut, curTitle;
        bool shortcutChanged, isNew;
//        MySqlCommand cmdShortcut;

        public OpEdForm()
        {
            InitializeComponent();
            tools = Tools.instance;
        }

        public void initForm(bool isNew, long id, string shortcut, string title)
        {
            shortcutChanged = false;
            lShortcutStatus.Text = "";
            this.isNew = isNew;
            this.id = id;
            curShortcut = shortcut;
            curTitle = title;
            tbShortcut.Text = shortcut;
            tbTitle.Text = title;
            /*
            cmdShortcut = new MySqlCommand("select count(id) cpid from ops where shortcut = @shortcut and id <> @id", tools.connection);
            tools.SetDbParameter(cmdShortcut, "id", id);
             */
        }

        object cmdShortcut(string shortcut)
        {
            using (DbCommand cmd = Db.command("select count(id) cpid from ops where shortcut = @shortcut and id <> @id"))
            {
                Db.param(cmd, "id", id);
                Db.param(cmd, "shortcut", shortcut);
                return cmd.ExecuteScalar();
            }
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

                        /*
                        tools.SetDbParameter(cmdShortcut, "shortcut", skt);

                        if (Convert.ToInt64(cmdShortcut.ExecuteScalar()) > 0)
                         */
                        if (Convert.ToInt64(cmdShortcut(skt)) > 0)
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

        private void OpEdForm_Activated(object sender, EventArgs e)
        {
            tShortcutChanged.Enabled = true;
        }

        private void OpEdForm_Deactivate(object sender, EventArgs e)
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
                object cs = cmdShortcut.ExecuteScalar();
                 */
                object cs = cmdShortcut(tbShortcut.Text.Trim());
                if (cs != System.DBNull.Value && Convert.ToInt64(cs) > 0) er += "* Код занят\n";
            }

            if (tbTitle.Text.Trim().Length < 1) er += "Наименование не указано\n";

            if (er != "") MessageBox.Show("Ошибки:\n\n" + er);
            else
            {
                if (isNew || (!tbShortcut.Text.Trim().Equals(curShortcut) || !tbTitle.Text.Trim().Equals(curTitle)))
                {
                    /*
                    MySqlCommand cmd;
                    if (isNew)
                        cmd = new MySqlCommand("insert into ops (title, shortcut) values (@title, @shortcut)", tools.connection);
                    else
                    {
                        cmd = new MySqlCommand("update ops set title = @title, shortcut = @shortcut where id = @id", tools.connection);
                        tools.SetDbParameter(cmd, "id", id);
                    }

                    tools.SetDbParameter(cmd, "title", tbTitle.Text.Trim());
                    tools.SetDbParameter(cmd, "shortcut", tbShortcut.Text.Trim());
                    cmd.ExecuteNonQuery();
                     */

                    string sql = (isNew) ?
                        "insert into ops (title, shortcut) values (@title, @shortcut)" :
                        "update ops set title = @title, shortcut = @shortcut where id = @id";

                    using (DbCommand cmd = Db.command(sql))
                    {
                        if (!isNew) Db.param(cmd, "id", id);
                        Db.param(cmd, "title", tbTitle.Text.Trim());
                        Db.param(cmd, "shortcut", tbShortcut.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
                DialogResult = DialogResult.OK;
            }
        }

        private void OpEdForm_Shown(object sender, EventArgs e)
        {
            tbShortcut.Focus();
        }

    }
}
