using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;

namespace Kassa3
{
    public partial class FirmEdForm : Form
    {
        Tools tools;

        public long id;
        string curShortcut, curTitle;
        bool shortcutChanged, isNew;

        public FirmEdForm()
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
        }

        object cmdShortcut(string shortcut)
        {
            using (DbCommand cmd = Db.command("select count(id) cpid from firms where shortcut = @shortcut and id <> @id"))
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

        private void FirmEdForm_Activated(object sender, EventArgs e)
        {
            tShortcutChanged.Enabled = true;
        }

        private void FirmEdForm_Deactivate(object sender, EventArgs e)
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
                        "insert into firms (title, shortcut) values (@title, @shortcut)" :
                        "update firms set title = @title, shortcut = @shortcut where id = @id";

                    using (DbCommand cmd = Db.command(sql))
                    {
                        if (!isNew) Db.param(cmd, "id", id);
                        Db.param(cmd, "title", tbTitle.Text.Trim());
                        Db.param(cmd, "shortcut", tbShortcut.Text.Trim());
                        cmd.ExecuteNonQuery();

                        if (isNew) id = Db.LastInsertedId(cmd, "firms");
                    }

//                    if (isNew) id = cmd.LastInsertedId;

                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
                DialogResult = DialogResult.OK;
            }
        }

    }
}
