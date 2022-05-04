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
    public partial class UserEdForm : Form
    {
        Tools tools;

        User user;
        OpRuleEdForm ruleEd;

        bool isNewUser;

        public UserEdForm()
        {
            InitializeComponent();
            tools = Tools.instance;
            ruleEd = new OpRuleEdForm();
        }

        ~UserEdForm()
        {
            ruleEd = null;
        }

        public void prepareEditForm(int pid, bool active, string login, string password, string data, bool marknew)
        {
            UserPrefs prefs = new UserPrefs();
            prefs.LoadFromXml(data);
            user = new User(pid, active, login, password, prefs);
            fillFieldsFromUser();

            cbChangePassword.Visible = true;
            cbMarkNewRec.Checked = marknew;
            isNewUser = false;
        }

        public void prepareNewForm()
        {
            UserPrefs prefs = new UserPrefs();
            prefs.createdTime = DateTime.Now;
            user = new User(-1, false, "", "", prefs);
            fillFieldsFromUser();
            cbChangePassword.Visible = false;
            cbMarkNewRec.Checked = false;
            isNewUser = true;
        }

        void fillFieldsFromUser()
        {
            tbLogin.Text = user.Login;
            cbActive.Checked = user.active;
            tbPassword.Text = "";
            
            cbDicUsers.SelectedIndex = (int)user.prefs.dicUsers;
            cbDicCurrencies.SelectedIndex = (int)user.prefs.dicCurrency;
            cbDicClients.SelectedIndex = (int)user.prefs.dicClients;
            cbDicOps.SelectedIndex = (int)user.prefs.dicOps;
            cbDicFirmAcc.SelectedIndex = (int)user.prefs.dicFirmAcc;

            cbAppSettings.SelectedIndex = (int)user.prefs.appSettings;
            cbFieldsEdit.SelectedIndex = (int)user.prefs.fieldsEdit;

            cbGlobalRule.SelectedIndex = (int)user.prefs.globalRule;

            lbOpRules.Items.Clear();
            lbOpRules.Items.AddRange(user.prefs.opRules.ToArray());
        }

        private void UserEdForm_Shown(object sender, EventArgs e)
        {
            tbLogin.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            try
            {
                string tLogin = tbLogin.Text.Trim();
                string tPassword = cbChangePassword.Checked || isNewUser ? Crypt.StringToMD5(tbPassword.Text) : user.Password;

                string er = "";
                if (tLogin.Equals("")) er += "Имя пользователя не может быть пустым.\n";
                else
                {
                    /*
                    MySqlCommand cmd = new MySqlCommand("select count(id) cid from users where login = @login and id <> @id", tools.connection);
                    tools.SetDbParameter(cmd, "login", tLogin);
                    tools.SetDbParameter(cmd, "id", user.PID);
                    object sc = cmd.ExecuteScalar();
                    if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0) er += "Пользователь с таким именем уже имеется.\n";
                     */
                    using (DbCommand cmd = Db.command("select count(id) cid from users where login = @login and id <> @id"))
                    {
                        Db.param(cmd, "login", tLogin);
                        Db.param(cmd, "id", user.PID);
                        object sc = cmd.ExecuteScalar();
                        if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0) er += "Пользователь с таким именем уже имеется.\n";
                    }
                }

                if (er.Equals(""))
                {
                    /*
                    MySqlCommand cmd;

                    if (isNewUser)
                    { // Новый пользователь
                        cmd = new MySqlCommand(
                            "insert into users (active, login, pass, prefs, mark_new) " +
                            "values (@active, @login, @pass, @prefs, @marknew)", tools.connection
                            );
                    }
                    else
                    { // Имеющийся пользователь
                        cmd = new MySqlCommand(
                            "update users set active = @active, login = @login, pass = @pass, prefs = @prefs, mark_new = @marknew where id = @id", tools.connection
                            );
                        tools.SetDbParameter(cmd, "id", user.PID);
                    }

                    tools.SetDbParameter(cmd, "active", cbActive.Checked);
                    tools.SetDbParameter(cmd, "login", tLogin);
                    tools.SetDbParameter(cmd, "pass", tPassword);
                    tools.SetDbParameter(cmd, "prefs", user.prefs.SaveToXml());
                    tools.SetDbParameter(cmd, "marknew", cbMarkNewRec.Checked);
                    cmd.ExecuteNonQuery();
                    */

                    string sql = (isNewUser) ?
                        "insert into users (active, login, pass, prefs, mark_new) values (@active, @login, @pass, @prefs, @marknew)" :
                        "update users set active = @active, login = @login, pass = @pass, prefs = @prefs, mark_new = @marknew where id = @id";

                    using (DbCommand cmd = Db.command(sql))
                    {
                        if (!isNewUser) Db.param(cmd, "id", user.PID);
                        Db.param(cmd, "active", cbActive.Checked);
                        Db.param(cmd, "login", tLogin);
                        Db.param(cmd, "pass", tPassword);
                        Db.param(cmd, "prefs", user.prefs.SaveToXml());
                        Db.param(cmd, "marknew", cbMarkNewRec.Checked);
                        cmd.ExecuteNonQuery();
                    }

                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();

                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Ошибки:\n\n" + er);
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cbDicUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.dicUsers = (AccessMode)((ComboBox)sender).SelectedIndex;
        }

        private void cbDicCurrencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.dicCurrency = (AccessMode)((ComboBox)sender).SelectedIndex;
        }

        private void cbAppSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.appSettings = (SimplePermission)((ComboBox)sender).SelectedIndex;
        }

        private void cbFieldsEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.fieldsEdit = (SimplePermission)( (ComboBox)sender ).SelectedIndex;
        }

        private void cbDicClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.dicClients = (AccessMode)((ComboBox)sender).SelectedIndex;
        }

        private void cbDicOps_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.dicOps = (AccessMode)((ComboBox)sender).SelectedIndex;
        }

        private void cbDicFirmAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.dicFirmAcc = (AccessMode)((ComboBox)sender).SelectedIndex;
        }

        private void cbGlobalRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            user.prefs.globalRule = (SimplePermission)((ComboBox)sender).SelectedIndex;
        }

        private void bNewRule_Click(object sender, EventArgs e)
        {
            OpRuleItem newRule = new OpRuleItem(OpRuleType.FIRM, -1, "", false);
            ruleEd.initForm(newRule);
            if (ruleEd.ShowDialog() == DialogResult.OK)
            {
                lbOpRules.Items.Add(newRule);
                lbOpRules.SelectedItem = newRule;
                user.prefs.opRules.Add(newRule);
            }
        }

        private void bMoveUp_Click(object sender, EventArgs e)
        {
            if (lbOpRules.Items.Count > 1 && lbOpRules.SelectedIndex > 0)
            {
                int src = lbOpRules.SelectedIndex, dst = src - 1;
                OpRuleItem b = (OpRuleItem)lbOpRules.Items[src];
                lbOpRules.SuspendLayout();
                lbOpRules.Items.Remove(b);
                lbOpRules.Items.Insert(dst, b);
                lbOpRules.SelectedItem = b;
                lbOpRules.Invalidate();
                lbOpRules.ResumeLayout();

                b = user.prefs.opRules[src];
                user.prefs.opRules.Remove(b);
                user.prefs.opRules.Insert(dst, b);
            }
        }

        private void bMoveDown_Click(object sender, EventArgs e)
        {
            if (lbOpRules.Items.Count > 1 && lbOpRules.SelectedIndex > -1 && lbOpRules.SelectedIndex < lbOpRules.Items.Count - 1)
            {
                int src = lbOpRules.SelectedIndex, dst = src + 1;
                OpRuleItem b = (OpRuleItem)lbOpRules.Items[dst];
                lbOpRules.SuspendLayout();
                lbOpRules.Items.Remove(b);
                lbOpRules.Items.Insert(src, b);
                lbOpRules.SelectedIndex = dst;
                lbOpRules.Invalidate();
                lbOpRules.ResumeLayout();

                b = user.prefs.opRules[dst];
                user.prefs.opRules.Remove(b);
                user.prefs.opRules.Insert(src, b);
            }
        }

        private void bEditRule_Click(object sender, EventArgs e)
        {
            if (lbOpRules.Items.Count > 0 && lbOpRules.SelectedItem != null)
            {
                OpRuleItem item = (OpRuleItem)lbOpRules.SelectedItem;
                ruleEd.initForm(item);
                if (ruleEd.ShowDialog() == DialogResult.OK)
                {
                    int si = lbOpRules.SelectedIndex;
                    lbOpRules.Items.Remove(item);
                    lbOpRules.Items.Insert(si, item);
                    lbOpRules.SelectedIndex = si;
                    lbOpRules.Invalidate();
                }
            }
        }

        private void bDeleteRule_Click(object sender, EventArgs e)
        {
            if (lbOpRules.Items.Count > 0 && lbOpRules.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить правило?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OpRuleItem item = (OpRuleItem)lbOpRules.SelectedItem;
                    lbOpRules.Items.Remove(item);
                    user.prefs.opRules.Remove(item);
                }
            }
        }

        private void bRuleSet_Click(object sender, EventArgs e)
        {
            try
            {
                user.prefs.buildRuleMapping();
                string s = user.prefs.getRuleMappingText();
                using (TextViewer tv = new TextViewer("RuleMapping", "Карта допуска", s))
                {
                    tv.ShowDialog();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось построить карту допуска");
            }
        }


    }
}
