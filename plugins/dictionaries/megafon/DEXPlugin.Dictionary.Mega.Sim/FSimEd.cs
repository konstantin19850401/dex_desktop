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

namespace DEXPlugin.Dictionary.Mega.Sim
{
    public partial class FSimEd : Form
    {
        DataRow row;
        Object toolbox;
        int jid;
        string journalName;

        public FSimEd()
        {
            InitializeComponent();
        }

        public void InitForm(Object AToolBox, string journalName, DataRow ARow)
        {
            toolbox = AToolBox;
            this.journalName = journalName;
            row = ARow;

            DataTable t = ((IDEXData)toolbox).getQuery("select * from `um_regions` order by title");
            StringTagItem.UpdateCombo(cbRegion_id, t, null, "region_id", "title", false);

            t = ((IDEXData)toolbox).getQuery("select * from `um_plans` order by title");
            StringTagItem.UpdateCombo(cbPlan_id, t, null, "plan_id", "title", false);

            t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit_id, t, null, "uid", "title", false);


            if (row == null)
            {
                jid = -1;
                tbMSISDN.Text = "";
                tbICC.Text = "";
                cbRegion_id.SelectedItem = null;

                try
                {
                    t = ((IDEXData)toolbox).getQuery("select max(party_id) as mpid from `{0}`", journalName);
                    nudParty_id.Value = int.Parse(t.Rows[0]["mpid"].ToString());
                }
                catch (Exception)
                {
                    nudParty_id.Value = 1;
                }

                cbPlan_id.SelectedItem = null;
                dtpDate_in.Value = DateTime.Now;
                cbStatus.SelectedIndex = 0;
                cbUnit_id.SelectedItem = null;
                dtpDate_own.Text = "";
                dtpDate_sold.Text = "";
                tbBalance.Text = "";
                tbComment.Text = "";
            }
            else
            {
                jid = int.Parse(row["id"].ToString());
                tbMSISDN.Text = row["msisdn"].ToString();
                tbICC.Text = row["icc"].ToString();
                StringTagItem.SelectByTag(cbRegion_id, row["region_id"].ToString(), true);
                try
                {
                    nudParty_id.Value = int.Parse(row["party_id"].ToString());
                }
                catch (Exception)
                {
                }

                cbFS.Checked = Convert.ToBoolean(row["fs"]);
                cbNewBilling.Checked = Convert.ToBoolean(row["billing"]);

                StringTagItem.SelectByTag(cbPlan_id, row["plan_id"].ToString(), true);
                string dt = row["date_in"].ToString();
                try
                {
                    dtpDate_in.Value = new DateTime(
                        int.Parse(dt.Substring(0, 4)), int.Parse(dt.Substring(4, 2)),
                        int.Parse(dt.Substring(6, 2)));
                }
                catch (Exception)
                {
                    dtpDate_in.Text = "";
                }

                cbStatus.SelectedIndex = int.Parse(row["status"].ToString());
                StringTagItem.SelectByTag(cbUnit_id, row["owner_id"].ToString(), true);

                dt = row["date_own"].ToString();
                try
                {
                    dtpDate_own.Value = new DateTime(
                        int.Parse(dt.Substring(0, 4)), int.Parse(dt.Substring(4, 2)),
                        int.Parse(dt.Substring(6, 2)));
                }
                catch (Exception)
                {
                    dtpDate_own.Text = "";
                }

                dt = row["date_sold"].ToString();
                try
                {
                    dtpDate_sold.Value = new DateTime(
                        int.Parse(dt.Substring(0, 4)), int.Parse(dt.Substring(4, 2)),
                        int.Parse(dt.Substring(6, 2)));
                }
                catch (Exception)
                {
                    dtpDate_sold.Text = "";
                }

                tbBalance.Text = row["balance"].ToString();
                tbComment.Text = row["comment"].ToString();
            }

            tbMSISDN.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            if (tbMSISDN.Text.Trim().Length != 10)
            {
                er += "* Некорректный MSISDN\n";
            }

            if (cbRegion_id.SelectedIndex < 0 || cbRegion_id.SelectedItem == null)
            {
                er += "* Не указан регион\n";
            }

            if (!nudParty_id.Text.Equals(nudParty_id.Value.ToString()))
            {
                er += "* Некорректно указана партия\n";
            }

            if (cbPlan_id.SelectedIndex < 0 || cbPlan_id.SelectedItem == null)
            {
                er += "* Не указан ТП\n";
            }

            if (!dtpDate_in.Text.Equals(dtpDate_in.Value.ToString("dd.MM.yyyy")))
            {
                er += "* Некорректная дата поступления\n";
            }

            if (cbStatus.SelectedIndex < 0)
            {
                er += "* Не указан статус SIM-карты\n";
            }

            if (cbStatus.SelectedIndex > 0)
            {
                if (cbUnit_id.SelectedIndex < 0 || cbUnit_id.SelectedItem == null)
                {
                    er += "* Не указано отделение-владелец SIM-карты\n";
                }

                if (!dtpDate_own.Text.Equals(dtpDate_own.Value.ToString("dd.MM.yyyy")))
                {
                    er += "* Некорректная дата распределения\n";
                }
            }

            if (cbStatus.SelectedIndex > 1)
            {
                if (!dtpDate_sold.Text.Equals(dtpDate_sold.Value.ToString("dd.MM.yyyy")))
                {
                    er += "* Некорректная дата продажи\n";
                }
            }

            if (er.Equals(""))
            {

                if ("um_data".Equals(journalName))
                {
                    string sql = string.Format(
                        "select count(id) as cid from `{2}` where id <> {0} and msisdn = '{1}'",
                        jid, MySqlHelper.EscapeString(tbMSISDN.Text), journalName
                    );
                    IDEXData d = (IDEXData)toolbox;
                    DataTable t = d.getQuery(sql);
                    if (t != null && t.Rows.Count > 0 && int.Parse(t.Rows[0]["cid"].ToString()) > 0)
                    {
                        er += "* MSISDN уже имеется в базе\n";
                    }

                    sql = string.Format(
                        "select count(id) as cid from `{2}` where id <> {0} and icc = '{1}'",
                        jid, MySqlHelper.EscapeString(tbICC.Text), journalName
                    );
                    t = d.getQuery(sql);
                    if (t != null && t.Rows.Count > 0 && int.Parse(t.Rows[0]["cid"].ToString()) > 0)
                    {
                        er += "* ICC уже имеется в базе\n";
                    }
                }
            }

            if (er.Equals(""))
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }

        }

    }
}
