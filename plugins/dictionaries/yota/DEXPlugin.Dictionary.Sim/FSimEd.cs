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

namespace DEXPlugin.Dictionary.Yota.Sim
{
    public partial class FSimEd : Form
    {
        DataRow row;
        Object toolbox;
        int jid;
        string journalName;
        bool update = false;

        public FSimEd()
        {
            InitializeComponent();
        }

        public void InitForm(Object AToolBox, string journalName, DataRow ARow)
        {
            toolbox = AToolBox;
            this.journalName = journalName; 
            row = ARow;
            if ( row != null )
                update = true; // если true, то 
            //DataTable t = ((IDEXData)toolbox).getQuery("select * from `um_regions` order by title");
            //StringTagItem.UpdateCombo(cbRegion_id, t, null, "region_id", "title", false);

            //t = ((IDEXData)toolbox).getQuery("select * from `um_plans` order by title");
            //StringTagItem.UpdateCombo(cbPlan_id, t, null, "plan_id", "title", false);

            DataTable t = ( (IDEXData)toolbox ).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit_id, t, null, "uid", "title", false);


            if (row == null)
            {
                jid = -1;
                //tbMSISDN.Text = "";
                tbICC.Text = "";
                //cbRegion_id.SelectedItem = null;

                try
                {
                    t = ((IDEXData)toolbox).getQuery("select max(party_id) as mpid from `{0}`", journalName);
                    nudParty_id.Value = int.Parse(t.Rows[0]["mpid"].ToString());
                }
                catch (Exception)
                {
                    nudParty_id.Value = 1;
                }

                //cbPlan_id.SelectedItem = null;
                dtpDate_in.Value = DateTime.Now;
                cbStatus.SelectedIndex = 0;
                cbUnit_id.SelectedItem = null;
                dtpDate_own.Text = "";
                dtpDate_sold.Text = "";
                cbTypeSim.SelectedIndex = 0;
                //tbBalance.Text = "";
                tbComment.Text = "";
            }
            else
            {
                jid = int.Parse(row["id"].ToString());
                //tbMSISDN.Text = row["msisdn"].ToString();
                tbICC.Text = row["icc"].ToString();

                cbTypeSim.SelectedIndex = int.Parse(row["type_sim"].ToString());

                tbBalance.Text = row["balance"].ToString();
                //StringTagItem.SelectByTag(cbRegion_id, row["region_id"].ToString(), true);
                try
                {
                    nudParty_id.Value = int.Parse(row["party_id"].ToString());
                }
                catch (Exception)
                {
                }

                //cbFS.Checked = Convert.ToBoolean(row["fs"]);

                //StringTagItem.SelectByTag(cbPlan_id, row["plan_id"].ToString(), true);
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

                //tbBalance.Text = row["balance"].ToString();
                tbComment.Text = row["comment"].ToString();
            }

            //tbMSISDN.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            //if (tbMSISDN.Text.Trim().Length != 10)
            //{
            //    er += "* Некорректный MSISDN\n";
            //}

            //if (cbRegion_id.SelectedIndex < 0 || cbRegion_id.SelectedItem == null)
            //{
            //    er += "* Не указан регион\n";
            //}

            if (!nudParty_id.Text.Equals(nudParty_id.Value.ToString()))
            {
                er += "* Некорректно указана партия\n";
            }

            //if (cbPlan_id.SelectedIndex < 0 || cbPlan_id.SelectedItem == null)
            //{
            //    er += "* Не указан ТП\n";
            //}

            if (!dtpDate_in.Text.Equals(dtpDate_in.Value.ToString("dd.MM.yyyy")))
            {
                er += "* Некорректная дата поступления\n";
            }

            if (cbStatus.SelectedIndex < 0)
            {
                er += "* Не указан статус SIM-карты\n";
            }

            if (er.Equals(""))
            {

                if ( "um_data".Equals(journalName)  )
                {
                    if ( !update ) 
                    {
                        string sql = string.Format(
                            "select count(id) as cid from `{2}` where id <> {0} and icc = '{1}'",
                            jid, MySqlHelper.EscapeString(tbICC.Text), journalName
                        );
                        IDEXData d = (IDEXData)toolbox;
                        DataTable t = d.getQuery(sql);
                        if ( t != null && t.Rows.Count > 0 && int.Parse(t.Rows[0]["cid"].ToString()) > 0 )
                        {
                            er += "* ICC уже имеется в базе\n";
                        }
                        else
                        {
                            try
                            {
                                string uid;
                                if ( cbUnit_id.SelectedIndex < 0 || cbUnit_id.SelectedItem == null ) 
                                {
                                    uid = "-1";
                                } else 
                                {
                                    uid = ((StringTagItem)cbUnit_id.SelectedItem ).Tag;
                                }
                                
                                sql = string.Format(
                                        "insert into `{0}` (status, msisdn, icc, type_sim, date_in, owner_id, date_own, " +
                                        "date_sold, region_id, party_id, plan_id, balance, comment, data, fs) values ({1}, " +
                                        "'{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}', '{13}', {14})",
                                        journalName, cbStatus.SelectedIndex, "",
                                        tbICC.Text, cbTypeSim.SelectedIndex, d.EscapeString(dtpDate_in.Value.ToString("yyyyMMdd")),
                                        uid, d.EscapeString(dtpDate_own.Value.ToString("yyyyMMdd")),
                                        "", "",
                                        nudParty_id.Text, "",
                                        tbBalance.Text, tbComment.Text,
                                        "", 0);
                                t = d.getQuery(sql);
                                MessageBox.Show("Информация о номере была внесена в базу!");
                            }
                            catch ( Exception )
                            {
                                MessageBox.Show("Неудачное внесение информации о номере в базу!");
                            }

                        }
                    } else 
                    {
                        if ( !dtpDate_own.Text.Equals(dtpDate_own.Value.ToString("dd.MM.yyyy")) )
                        {
                            er += "* Некорректная дата распределения\n";
                        }

                        if ( cbUnit_id.SelectedIndex < 0 || cbUnit_id.SelectedItem == null )
                        {
                            er += "* Не указано отделение-владелец SIM-карты\n";
                        }

                        if ( cbStatus.SelectedIndex > 1 )
                        {
                            if ( !dtpDate_sold.Text.Equals(dtpDate_sold.Value.ToString("dd.MM.yyyy")) )
                            {
                                er += "* Некорректная дата продажи\n";
                            }
                        }

                        if ( er.Equals("") ) {
                            try
                            {
                                IDEXData d = (IDEXData)toolbox;

                                string sql = "update `" + journalName + "` set status='" + cbStatus.SelectedIndex + "', msisdn='', icc='" + tbICC.Text +
                                            "', type_sim ='" +cbTypeSim.SelectedIndex+ "', date_in='" + d.EscapeString(dtpDate_in.Value.ToString("yyyyMMdd")) + "', owner_id='" + ((StringTagItem)cbUnit_id.SelectedItem).Tag +
                                            "', date_own='" + d.EscapeString(dtpDate_own.Value.ToString("yyyyMMdd")) +
                                            "', date_sold='" + d.EscapeString(dtpDate_sold.Value.ToString("yyyyMMdd")) +
                                            "', region_id='', party_id='" + nudParty_id.Text + "', plan_id='', balance='" + tbBalance.Text +
                                            "', comment='" + tbComment.Text + "', data='', fs='0' where icc = " + tbICC.Text;

                                d.runQuery(sql);
                                MessageBox.Show("Информация о номере была обновлена в базе!");
                            }
                            catch ( Exception )
                            {
                                MessageBox.Show("Неудачное обновление информации о номере в базе!");
                            }
                        } else 
                        {
                            
                        }
                    }
                    

                    /*
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
                    */
                    
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
