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
    public partial class FirmAccDicForm : Form
    {
        public Tools tools;
        FormState fs;
        FirmEdForm firmEd;
        AccEdForm accEd;

        public FirmAccDicForm()
        {
            InitializeComponent();
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\FirmAccDicForm.fs");
            firmEd = new FirmEdForm();
            accEd = new AccEdForm();
        }

        ~FirmAccDicForm()
        {
            firmEd = null;
            accEd = null;
        }

        private void FirmAccDicForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
            //            fs.LoadValue("pCat", pCat);
            RefreshFirms();
        }

        private void FirmAccDicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            //            fs.SaveValue("pCat", pCat);
            fs.SaveToFile(tools.dataDir + @"\FirmAccDicForm.fs");
            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        public void RefreshFirms()
        {
            FirmItem fiOldSel = (FirmItem)cbFirm.SelectedItem;

            cbFirm.SuspendLayout();
            cbFirm.Enabled = false;
            try
            {
                using (DataTable dt = Db.fillTable(Db.command("select * from firms order by title")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        try
                        {
                            FirmItem fi = new FirmItem(Convert.ToInt32(r["id"]), r["title"].ToString(), r["shortcut"].ToString());
                            cbFirm.Items.Add(fi);
                            if (fiOldSel.id == fi.id) cbFirm.SelectedItem = fiOldSel;
                        }
                        catch (Exception) { }
                    }
                }
                cbFirm.Enabled = cbFirm.Items.Count > 0;
            }
            catch (Exception) { }
            cbFirm.ResumeLayout();
        }

        private void cbFirm_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountItem aiOldSel = (AccountItem)lbAcc.SelectedItem;


            lbAcc.Enabled = false;
            bAccAdd.Enabled = false;
            bAccEdit.Enabled = false;
            bAccDelete.Enabled = false;
            lbAcc.SuspendLayout();
            lbAcc.Items.Clear();
            try
            {
                if (cbFirm.SelectedItem != null)
                {
                    //clname, id, firm_id, curr_id, title, shortcut

                    DbCommand cmd = Db.command("select ac.*, cl.name clname from accounts ac, curr_list cl where cl.id = ac.curr_id and ac.firm_id = @firm_id");
                    Db.param(cmd, "firm_id", ((FirmItem)cbFirm.SelectedItem).id);

                    using (DataTable dt = Db.fillTable(cmd))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            try
                            {
                                AccountItem ai = new AccountItem(
                                    Convert.ToInt64(r["id"]), r["title"].ToString(), r["shortcut"].ToString(), 
                                    Convert.ToInt64(r["firm_id"]), Convert.ToInt64(r["curr_id"]), r["clname"].ToString(),
                                    r["banktag"].ToString()
                                    );

                                lbAcc.Items.Add(ai);
                                if (aiOldSel.id == ai.id) lbAcc.SelectedItem = aiOldSel;
                            }
                            catch (Exception) { }
                        }
                    }

                    lbAcc.Enabled = lbAcc.Items.Count > 0;
                    bAccAdd.Enabled = true;
                    bAccEdit.Enabled = lbAcc.Items.Count > 0;
                    bAccDelete.Enabled = lbAcc.Items.Count > 0;
                }
            }
            catch (Exception) { }

            lbAcc.ResumeLayout();
        }

        private void bFirmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                firmEd.initForm(true, -1, "", "");
                if (firmEd.ShowDialog() == DialogResult.OK)
                {
                    FirmItem fi = new FirmItem(firmEd.id, firmEd.tbTitle.Text.Trim(), firmEd.tbShortcut.Text.Trim());
                    cbFirm.Items.Add(fi);
                    cbFirm.SelectedItem = fi;
                    cbFirm.Enabled = true;
                    MainForm.mainForm.RefreshMenuFirms();
                }
            }
            catch (Exception) { }

        }

        private void bFirmEdit_Click(object sender, EventArgs e)
        {
            if (cbFirm.SelectedItem != null)
            {
                FirmItem ci = (FirmItem)cbFirm.SelectedItem;
                try
                {
                    firmEd.initForm(false, ci.id, ci.Shortcut, ci.Text);
                    if (firmEd.ShowDialog() == DialogResult.OK)
                    {
                        ci.Text = firmEd.tbTitle.Text.Trim();
                        ci.Shortcut = firmEd.tbShortcut.Text.Trim();

                        cbFirm.SuspendLayout();
                        cbFirm.Items.Remove(ci);
                        cbFirm.Items.Add(ci);
                        cbFirm.SelectedItem = ci;
                        cbFirm.ResumeLayout();
                        MainForm.mainForm.RefreshMenuFirms();
                    }
                }
                catch (Exception) { }
            }
        }

        private void bFirmDelete_Click(object sender, EventArgs e)
        {
            if (cbFirm.SelectedItem != null)
            {
                FirmItem ci = (FirmItem)cbFirm.SelectedItem;

                if (!Db.isMysql)
                {
                    // Проверка, есть ли у фирмы счета
                    using (DbCommand cmd = Db.command("select count(id) from accounts where firm_id = @firm_id"))
                    {
                        Db.param(cmd, "firm_id", ci.id);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Невозможно удалить фирму, пока у неё имеются счета");
                            return;
                        }
                    }
                }

                if (MessageBox.Show("Удалить фирму?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (DbCommand cmd = Db.command("delete from firms where id = @id")) //OK
                        {
                            Db.param(cmd, "id", ci.id);
                            cmd.ExecuteNonQuery();
                        }

                        if (!Db.isMysql)
                        {
                            using (DbCommand cmd = Db.command("delete from rules where firm_id = @firm_id"))
                            {
                                Db.param(cmd, "firm_id", ci.id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        cbFirm.Items.Remove(ci);
                        cbFirm.Visible = cbFirm.Items.Count > 0;
                        MainForm.mainForm.RefreshMenuFirms();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить запись."));
                    }
                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
            }
        }

        private void bAccAdd_Click(object sender, EventArgs e)
        {
            if (cbFirm.SelectedItem != null)
            {
                FirmItem ci = (FirmItem)cbFirm.SelectedItem;
                try
                {
                    accEd.initForm(true, -1, ci.id, "", "", -1, "");
                    if (accEd.ShowDialog() == DialogResult.OK)
                    {
                        AccountItem ai = new AccountItem(
                            accEd.id, accEd.tbTitle.Text, accEd.tbShortcut.Text, accEd.firmId, 
                            accEd.currId, accEd.cbCurrency.SelectedItem.ToString(), accEd.cbBankTag.Text
                            );
                        lbAcc.Items.Add(ai);
                        lbAcc.SelectedItem = ai;
                        lbAcc.Enabled = true;
                        bAccEdit.Enabled = true;
                        bAccDelete.Enabled = true;
                        tools.currentUser.prefs.needRebuildRuleMapping = true;
                        tools.tmDataChanges.markTableChanged();
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось создать запись."));
                }
            }
        }

        private void bAccEdit_Click(object sender, EventArgs e)
        {
            if (cbFirm.SelectedItem != null && lbAcc.SelectedItem != null)
            {
                AccountItem ai = (AccountItem)lbAcc.SelectedItem;
                try
                {
                    accEd.initForm(false, ai.id, ai.firmId, ai.Shortcut, ai.Text, ai.currId, ai.bankTag);
                    if (accEd.ShowDialog() == DialogResult.OK)
                    {
                        ai.Text = accEd.tbTitle.Text;
                        ai.Shortcut = accEd.tbShortcut.Text;
                        ai.currId = accEd.currId;
                        ai.CurrencyTitle = accEd.cbCurrency.SelectedItem.ToString();
                        ai.bankTag = accEd.cbBankTag.Text;

                        lbAcc.SuspendLayout();
                        lbAcc.Items.Remove(ai);
                        lbAcc.Items.Add(ai);
                        lbAcc.SelectedItem = ai;
                        lbAcc.ResumeLayout();
                        tools.currentUser.prefs.needRebuildRuleMapping = true;
                        tools.tmDataChanges.markTableChanged();
                    }
                }
                catch (Exception) { }
            }
        }

        private void bAccDelete_Click(object sender, EventArgs e)
        {
            if (cbFirm.SelectedItem != null && lbAcc.SelectedItem != null)
            {
                AccountItem ai = (AccountItem)lbAcc.SelectedItem;

                if (!Db.isMysql)
                {
                    // Проверка, есть ли операции по счёту
                    using (DbCommand cmd = Db.command("select count(id) from journal where src_acc_id = @acc_id or dst_acc_id = @acc_id"))
                    {
                        int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                        if (cnt > 0)
                        {
                            MessageBox.Show("Невозможно удалить счёт, так как на него ссылаются записи в журнале операций (" + cnt + ")");
                            return;
                        }
                    }
                }

                if (MessageBox.Show("Удалить счёт?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (DbCommand cmd = Db.command("delete from accounts where id = @id")) //OK
                        {
                            Db.param(cmd, "id", ai.id);
                            cmd.ExecuteNonQuery();
                        }

                        if (!Db.isMysql)
                        {
                            using (DbCommand cmd = Db.command("delete from import_rules where src_acc_id = @acc_id or dst_acc_id = @acc_id")) //OK
                            {
                                Db.param(cmd, "acc_id", ai.id);
                                cmd.ExecuteNonQuery();
                            }

                            using (DbCommand cmd = Db.command("delete from rules where acc_id = @acc_id")) //OK
                            {
                                Db.param(cmd, "acc_id", ai.id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        lbAcc.SuspendLayout();
                        lbAcc.Items.Remove(ai);
                        lbAcc.ResumeLayout();
                        lbAcc.Enabled = lbAcc.Items.Count > 0;
                        bAccEdit.Enabled = lbAcc.Items.Count > 0;
                        bAccDelete.Enabled = lbAcc.Items.Count > 0;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить запись."));
                    }
                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
            }
        }
    }
}
