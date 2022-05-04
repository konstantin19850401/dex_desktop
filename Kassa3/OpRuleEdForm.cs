using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using DEXExtendLib;

namespace Kassa3
{
    public partial class OpRuleEdForm : Form
    {
        Tools tools;
        OpRuleItem item;

        public OpRuleEdForm()
        {
            InitializeComponent();
            tools = Tools.instance;
        }

        public void initForm(OpRuleItem item)
        {
            cbFirm.Items.Clear();
//            DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, title from firms order by title", tools.connection));
            using (DataTable dt = Db.fillTable(Db.command("select id, title from firms order by title")))
            {
                StringTagItem.UpdateCombo(cbFirm, dt, null, "id", "title", false);
            }
            /*
            dt = tools.MySqlFillTable(new MySqlCommand(
                "select ac.id, concat('[', fi.title, '] ', ac.title) as account_title  from accounts as ac, firms as fi where ac.firm_id = fi.id order by account_title", 
                tools.connection));
             */

            string SCONCAT = Db.isMysql ? "concat('[', fi.title, '] ', ac.title)" : "'[' || fi.title || '] ' || ac.title";

            using(DataTable dt = Db.fillTable(Db.command(
                "select ac.id, " + SCONCAT + " as account_title  from accounts as ac, firms as fi where ac.firm_id = fi.id order by account_title"
                ))) {
                StringTagItem.UpdateCombo(cbAcc, dt, null, "id", "account_title", false);
            }

//            dt = tools.MySqlFillTable(new MySqlCommand("select id, title from ops order by title", tools.connection));

            using (DataTable dt = Db.fillTable(Db.command("select id, title from ops order by title")))
            {
                StringTagItem.UpdateCombo(cbOperation, dt, null, "id", "title", false);
            }

            initCategoryClient();

            this.item = item;

            rbProhibit.Checked = !item.permit;
            rbPermit.Checked = item.permit;

            rbFirm.Checked = item.RuleType == OpRuleType.FIRM;
            rbAcc.Checked = item.RuleType == OpRuleType.ACCOUNT;
            rbOp.Checked = item.RuleType == OpRuleType.OPERATION;
            rbCategory.Checked = item.RuleType == OpRuleType.CATEGORY;
            rbClient.Checked = item.RuleType == OpRuleType.CLIENT;

            if (rbFirm.Checked) StringTagItem.SelectByTag(cbFirm, item.paramId.ToString(), true);
            if (rbAcc.Checked) StringTagItem.SelectByTag(cbAcc, item.paramId.ToString(), true);
            if (rbOp.Checked) StringTagItem.SelectByTag(cbOperation, item.paramId.ToString(), true);
            if (rbCategory.Checked) StringTagItem.SelectByTag(cbCategory, item.paramId.ToString(), true);
            if (rbClient.Checked) StringTagItem.SelectByTag(cbClient, item.paramId.ToString(), true);
        }

        DataTable dtcat, dtdata;

        void processCategory(long cat_id, string indent)
        {
            DataRow[] catrows = dtcat.Select("parent_id = " + cat_id.ToString());
            if (catrows != null && catrows.Length > 0)
            {
                foreach (DataRow catrow in catrows)
                {
                    cbCategory.Items.Add(new StringTagItem(indent + catrow["cat_title"].ToString(), catrow["id"].ToString()));
                    cbClient.Items.Add(new StringTagItem(indent + catrow["cat_title"].ToString(), "-1"));
                    DataRow[] clirows = dtdata.Select("cat_id = " + catrow["id"].ToString());
                    if (clirows != null && clirows.Length > 0)
                    {
                        foreach (DataRow clirow in clirows)
                        {
                            cbClient.Items.Add(new StringTagItem("* " + clirow["title"].ToString(), clirow["id"].ToString()));
                        }
                    }

                    processCategory(Convert.ToInt64(catrow["id"]), indent + " ");
                }
            }
        }

        void initCategoryClient()
        {
//            dtcat = tools.MySqlFillTable(new MySqlCommand("select * from client_cat", tools.connection));
            dtcat = Db.fillTable(Db.command("select * from client_cat"));
//            dtdata = tools.MySqlFillTable(new MySqlCommand("select id, cat_id, title from client_data order by title", tools.connection));
            dtdata = Db.fillTable(Db.command("select id, cat_id, title from client_data order by title"));
            cbCategory.Items.Clear();
            cbClient.Items.Clear();
            processCategory(-1, "");
        }

        private void rbFirm_CheckedChanged(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32(((RadioButton)sender).Tag);
            bool Checked = ((RadioButton)sender).Checked;

            cbFirm.Visible = tag == 0 && Checked;
            cbAcc.Visible = tag == 1 && Checked;
            cbOperation.Visible = tag == 2 && Checked;
            cbCategory.Visible = tag == 3 && Checked;
            cbClient.Visible = tag == 4 && Checked;
        }

        private void cbClient_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbClient.SelectedItem != null)
            {
                try
                {
                    string s = ((StringTagItem)cbClient.SelectedItem).Text;
                    if (!s[0].Equals('*') && cbClient.SelectedIndex < cbClient.Items.Count - 1)
                    {
                        cbClient.SelectedIndex++;
                    }
                }
                catch (Exception) { }
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            if (!rbAcc.Checked && !rbCategory.Checked && !rbClient.Checked && !rbFirm.Checked && !rbOp.Checked)
            {
                er += "* Не выделен ни один тип правила\n";
            }
            else
            {

                if (rbAcc.Checked && cbAcc.SelectedItem == null) er += "* Не выбран счёт\n";
                if (rbCategory.Checked && cbCategory.SelectedItem == null) er += "* Не выбрана категория контрагентов\n";

                if (rbClient.Checked)
                {
                    try
                    {
                        long i = long.Parse(((StringTagItem)cbClient.SelectedItem).Tag);
                        if (i < 0) throw new Exception();
                    }
                    catch (Exception)
                    {
                        er += "* Не выбран контрагент\n";
                    }
                }

                if (rbFirm.Checked && cbFirm.SelectedItem == null) er += "* Не выбрана фирма\n";
                if (rbOp.Checked && cbOperation.SelectedItem == null) er += "* Не выбрана операция\n";
            }

            if (!rbPermit.Checked && !rbProhibit.Checked) er += "* Не выбрано ни одно действие\n";

            long val = -1;
            OpRuleType rt = OpRuleType.FIRM;

            if (er == "")
            {
                if (rbAcc.Checked && long.TryParse(((StringTagItem)cbAcc.SelectedItem).Tag, out val)) rt = OpRuleType.ACCOUNT;
                if (rbCategory.Checked && long.TryParse(((StringTagItem)cbCategory.SelectedItem).Tag, out val)) rt = OpRuleType.CATEGORY;
                if (rbClient.Checked && long.TryParse(((StringTagItem)cbClient.SelectedItem).Tag, out val)) rt = OpRuleType.CLIENT;
                if (rbFirm.Checked && long.TryParse(((StringTagItem)cbFirm.SelectedItem).Tag, out val)) rt = OpRuleType.FIRM;
                if (rbOp.Checked && long.TryParse(((StringTagItem)cbOperation.SelectedItem).Tag, out val)) rt = OpRuleType.OPERATION;

                if (val < 0) er += "* Не указано значение правила\n";
            }

            if (er == "") 
            {
                item.RuleType = rt;
                item.paramId = val;
                item.permit = rbPermit.Checked;
                tools.ValidateRule(item);
                tools.currentUser.prefs.needRebuildRuleMapping = true;
                tools.tmDataChanges.markTableChanged();
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Ошибки:\n\n" + er);
        }
    }
}
