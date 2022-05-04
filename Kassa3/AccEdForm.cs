using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Data.Common;

namespace Kassa3
{
    public partial class AccEdForm : Form
    {
        Tools tools;

        public long id, firmId, currId;
        string curShortcut, curTitle, curBanktag;
        bool shortcutChanged, isNew;

        public AccEdForm()
        {
            InitializeComponent();
            tools = Tools.instance;
        }

        public void initForm(bool isNew, long id, long firmId, string shortcut, string title, long currId, string bankTag)
        {
            shortcutChanged = false;
            lShortcutStatus.Text = "";
            this.isNew = isNew;
            this.id = id;
            this.firmId = firmId;
            this.currId = currId;
            curShortcut = shortcut;
            curTitle = title;
            curBanktag = bankTag;
            tbShortcut.Text = shortcut;
            tbTitle.Text = title;
            tbShortcut.Focus();

            lOwner.Text = "-";
            try
            {
                using (DbCommand cmd = Db.command("select title from firms where id = " + firmId.ToString())) //OK
                {
                    lOwner.Text = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception) { }

            using (DataTable dt = Db.fillTable(Db.command("select id, name from curr_list where active = 1"))) //OK
            {

                StringTagItem.UpdateCombo(cbCurrency, dt, null, "id", "name", false);
                StringTagItem.SelectByTag(cbCurrency, currId.ToString(), true);
            }

            using (DataTable dt = Db.fillTable(Db.command("select banktag from accounts group by banktag"))) //OK
            {
                StringTagItem.UpdateCombo(cbBankTag, dt, null, "banktag", "banktag", false);
                cbBankTag.Text = bankTag;
            }

        }

        private object cmdShortcut(string shortcut) //OK
        {
            using (DbCommand ret = Db.command("select count(id) cpid from accounts where shortcut = @shortcut and firm_id = @firm_id and id <> @id"))
            {
                Db.param(ret, "id", id);
                Db.param(ret, "firm_id", firmId);
                Db.param(ret, "shortcut", shortcut);
                return ret.ExecuteScalar();
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

        private void AccEdForm_Activated(object sender, EventArgs e)
        {
            tShortcutChanged.Enabled = true;
        }

        private void AccEdForm_Deactivate(object sender, EventArgs e)
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

            if (
                cbCurrency.SelectedItem == null ||
                !long.TryParse(((StringTagItem)cbCurrency.SelectedItem).Tag, out currId)
                ) 
                er += "Валюта счёта не указана\n";

            if (er != "") MessageBox.Show("Ошибки:\n\n" + er);
            else
            {
                if (isNew || (!tbShortcut.Text.Trim().Equals(curShortcut) || !tbTitle.Text.Trim().Equals(curTitle) || !cbBankTag.Text.Trim().Equals(curBanktag)))
                {
                    string sql = (isNew) ?
                        "insert into accounts (title, shortcut, firm_id, curr_id, banktag) values (@title, @shortcut, @firm_id, @curr_id, @banktag)" :
                        "update accounts set title = @title, shortcut = @shortcut, firm_id = @firm_id, curr_id = @curr_id, banktag = @banktag where id = @id";

                    using (DbCommand cmd = Db.command(sql))
                    {
                        if (!isNew) Db.param(cmd, "id", id);

                        Db.param(cmd, "title", tbTitle.Text.Trim());
                        Db.param(cmd, "shortcut", tbShortcut.Text.Trim());
                        Db.param(cmd, "firm_id", firmId);
                        Db.param(cmd, "curr_id", currId);
                        Db.param(cmd, "banktag", cbBankTag.Text.Trim());
                        cmd.ExecuteNonQuery();

                        if (isNew) id = Db.LastInsertedId(cmd, "accounts");
                    }

                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
                DialogResult = DialogResult.OK;
            }

        }

    }


}
