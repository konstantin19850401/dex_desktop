using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using DEXExtendLib;


namespace DEXPlugin.Dictionary.Beeline.Sim
{
    public partial class FSimMain : Form
    {
        public Object toolbox;
        string[] simJournalName = new string[2] { "um_data", "um_data_out" };
        
        MySqlDataAdapter daSim;
        MySqlCommandBuilder cbSim;
        BindingSource bsSim;

        public List<int> selectedParties = new List<int>();

        int simType;

        public FSimMain()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            bsSim = new BindingSource();
            dgvSim.DataSource = bsSim;
            dgvSim.AutoGenerateColumns = false;
            this.msisdn.DataPropertyName = "msisdn";
            this.icc.DataPropertyName = "icc";
            this.fs.DataPropertyName = "fs";
            this.dynamic.DataPropertyName = "dynamic";
            this.status.DataPropertyName = "status";
            
            
            //this.status_j.DataPropertyName = "status_j";
            this.owner_id.DataPropertyName = "owner_id";
            this.date_in.DataPropertyName = "date_in";
            this.date_own.DataPropertyName = "date_own";
            this.date_sold.DataPropertyName = "date_sold";
            this.party_id.DataPropertyName = "party_id";
            this.region_id.DataPropertyName = "region_id";
            this.plan_id.DataPropertyName = "plan_id";
            this.balance.DataPropertyName = "balance";
            this.comment.DataPropertyName = "comment";
            this.auto_distr.DataPropertyName = "auto_distr";

            

            IDEXConfig cfg = (IDEXConfig)toolbox;
            foreach (DataGridViewColumn col in dgvSim.Columns)
            {
                try
                {
                    int wh = cfg.getInt("dgvSim", col.Name + "_width", -1);
                    if (wh > -1) col.Width = wh;

                    wh = cfg.getInt("dgvSim", col.Name + "_order", -1);
                    if (wh > -1) col.DisplayIndex = wh;
                }
                catch (Exception)
                {
                }
            }

            int p = cfg.getInt("SimMain", "width", -1);
            if (p > -1) this.Width = p;

            p = cfg.getInt("SimMain", "height", -1);
            if (p > -1) this.Height = p;

            cbClipboardExcel.Checked = cfg.getBool("SimMain", "cbClipboardExcel", false);

            simType = 0;

            if (simType.ToString() == tsmiSimTypeActive.Tag.ToString())
            {
                tsddbSimType.Text = tsmiSimTypeActive.Text;
            }

            if (simType.ToString() == tsmiSimTypeInactive.Tag.ToString())
            {
                tsddbSimType.Text = tsmiSimTypeInactive.Text;
            }

            cbFS.SelectedIndex = 2;

            cbStatus0.Checked = true;
            cbStatus1.Checked = true;
            cbStatus2.Checked = true;

            UpdateFilterPanel(false);
            _clearFilter();
            dgvSim.Visible = false;

            tbFilterMSISDN.Focus();
        }

        public void _saveLayout()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            foreach (DataGridViewColumn col in dgvSim.Columns)
            {
                cfg.setInt("dgvSim", col.Name + "_width", col.Width);
                cfg.setInt("dgvSim", col.Name + "_order", col.DisplayIndex);
            }

            cfg.setInt("SimMain", "width", this.Width);
            cfg.setInt("SimMain", "height", this.Height);
            cfg.setBool("SimMain", "cbClipboardExcel", cbClipboardExcel.Checked);
        }

        private void UpdatePartyFilter(bool saveCurrent)
        {
            clbParties.Items.Clear();
            DataTable t = ((IDEXData)toolbox).getQuery(
                string.Format("select party_id from `{0}` group by party_id", simJournalName[simType])
                );
            StringTagItem.UpdateCombo(cbFilterParty, t, "Любая", "party_id", "party_id", saveCurrent);

            if (t != null && t.Rows.Count > 0)
            {
                foreach (DataRow row in t.Rows)
                {
                    StringObjTagItem sti = new StringObjTagItem(row["party_id"].ToString(), Convert.ToInt32(row["party_id"]));
                    clbParties.Items.Add(sti, selectedParties.Contains(Convert.ToInt32(row["party_id"])));
                }
            }
        }

        private void UpdateFilterPanel(bool saveCurrent)
        {
            DataTable t = ((IDEXData)toolbox).getQuery("select * from `um_plans` order by title");
            StringTagItem.UpdateCombo(cbFilterPlan, t, "Любой", "plan_id", "title", saveCurrent);

            if (t != null && t.Rows.Count > 0)
            {
                tsmiChangePlan.DropDownItems.Clear();
                tsmiSChangePlan.DropDownItems.Clear();
                foreach (DataRow r in t.Rows)
                {
                    ToolStripMenuItem tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Tag = r["plan_id"].ToString();
                    tsmir.Click += tsmiPlan_Click;
                    tsmiChangePlan.DropDownItems.Add(tsmir);

                    tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Tag = r["plan_id"].ToString();
                    tsmir.Click += tsmiSPlan_Click;
                    tsmiSChangePlan.DropDownItems.Add(tsmir);
                }
            }

            t = ((IDEXData)toolbox).getQuery("select * from `um_regions` order by title");
            StringTagItem.UpdateCombo(cbFilterRegion, t, "Любой", "region_id", "title", saveCurrent);

            if (t != null && t.Rows.Count > 0)
            {
                tsmiChangeRegion.DropDownItems.Clear();
                foreach (DataRow r in t.Rows)
                {
                    ToolStripMenuItem tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Tag = r["region_id"].ToString();
                    tsmir.Click += tsmiRegion_Click;
                    tsmiChangeRegion.DropDownItems.Add(tsmir);
                }
            }

            t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbFilterUnit, t, "Любое", "uid", "title", saveCurrent);

            tsmiChangePartyBalance.DropDownItems.Clear();
            tsmiSChangeBalance.DropDownItems.Clear();
            tsmiChangePartyBalance.Enabled = false;
            tsmiSChangeBalance.Enabled = false;
            t = ((IDEXData)toolbox).getQuery("select * from `um_balances` order by title");
            if (t != null && t.Rows.Count > 0)
            {
                tsmiChangePartyBalance.Enabled = true;
                tsmiSChangeBalance.Enabled = true;
                foreach (DataRow r in t.Rows)
                {
                    ToolStripMenuItem tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Click += tsmiBalance_Click;
                    tsmiChangePartyBalance.DropDownItems.Add(tsmir);

                    tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Click += tsmiSBalance_Click;
                    tsmiSChangeBalance.DropDownItems.Add(tsmir);
                }
            }


            // на всякий случай - массовая замена отделения
            /* 
            tsmiSimChangeUid.DropDownItems.Clear();
            tsmiSimChangeUid.Enabled = false;
            t = ((IDEXData)toolbox).getQuery("select uid, title from `units` order by title");
            if (t != null && t.Rows.Count > 0)
            {
                tsmiSimChangeUid.Enabled = true;
                foreach (DataRow r in t.Rows)
                {
                    ToolStripMenuItem tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Tag = r["uid"].ToString();
                    tsmir.Click += tsmiSimUidChange_Click;
                    tsmiSimChangeUid.DropDownItems.Add(tsmir);
                }

            }
            */

            UpdatePartyFilter(saveCurrent);
            _resetButtonsColors();
        }

        private void tsmiBalance_Click(object sender, EventArgs e)
        {
            try
            {
                string bal_title = ((ToolStripMenuItem)sender).Text;

                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;

                if (MessageBox.Show(
                    string.Format("Изменить баланс партии на <{0}>?", bal_title), "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    d.runQuery(string.Format(
                        "update `um_data` set balance='{0}' where party_id={1}",
                        d.EscapeString(bal_title), scpid
                        ));
                    MessageBox.Show("Присвоение баланса партии ТП завершено");
                    UpdateFilterPanel(true);
                    _resetButtonsColors();
                    _sim();

                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiSBalance_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    string bal_title = ((ToolStripMenuItem)sender).Text;

                    if (MessageBox.Show(
                        string.Format("Изменить баланс выделенных карт на <{0}>?", bal_title), "Подтверждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        cpmsg = "Изменение баланса выделенных записей";
                        
                        cpfield = "balance";
                        cpvalue = "'" + ((IDEXData)toolbox).EscapeString(bal_title) + "'";
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Изменение баланса успешно произведено";
                        MessageBox.Show(s);
                        _sim();
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        private void tsmiSimTypeActive_Click(object sender, EventArgs e)
        {
            string[] archSt = { "Переместить в архив", "Переместить в активные SIM-карты" };
            
            tsddbSimType.Text = ((ToolStripMenuItem)sender).Text;
            //TODO Выбор хранилища типа карт
            simType = int.Parse(((ToolStripMenuItem)sender).Tag.ToString());
            tsmiMoveFromToArchive.Text = archSt[simType];
            _sim();                
        }

        private void _clearFilter()
        {
            UpdateFilterPanel(false);
            tbFilterMSISDN.Text = "";
            tbFilterICC.Text = "";
        }

        private void _sim()
        {
            pFilter.Enabled = false;
            tsTools.Enabled = false;

            
            string sqlr = "select sim.* from `" + simJournalName[simType] + "` as sim ";
            string sqlc = "select count(sim.id) as sim_count from `" + simJournalName[simType] + "` as sim ";

            string whr = "";

            if (cbSearchBySimList.Checked)
            {
                DataTable dt = (DataTable)dgvSim.DataSource;
                List<string> msisdnList = new List<string>();
                string msisdnListStr = "";
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msisdnList.Add(dr["msisdn"].ToString());
                    }
                    msisdnListStr = String.Join(",", msisdnList);
                    whr += ((whr != "") ? "and " : "") + "sim.msisdn in (" + msisdnListStr + ") ";
                }
                else
                {
                    cbSearchBySimList.Checked = false;
                    if (tbFilterMSISDN.Text.Trim() != "")
                    {
                        whr += ((whr != "") ? "and " : "") + "sim.msisdn like '%" +
                            MySqlHelper.EscapeString(tbFilterMSISDN.Text.Trim()) +
                            "%' ";
                    }

                    if (tbFilterICC.Text.Trim() != "")
                    {
                        whr += ((whr != "") ? "and " : "") + "sim.icc like '%" +
                            MySqlHelper.EscapeString(tbFilterICC.Text.Trim()) +
                            "%' ";
                    }
                }
                
                /*
                for ()
                if (tbFilterMSISDN.Text.Trim() != "")
                {
                    whr += ((whr != "") ? "and " : "") + "sim.msisdn like '%" +
                        MySqlHelper.EscapeString(tbFilterMSISDN.Text.Trim()) +
                        "%' ";
                }

                if (tbFilterICC.Text.Trim() != "")
                {
                    whr += ((whr != "") ? "and " : "") + "sim.icc like '%" +
                        MySqlHelper.EscapeString(tbFilterICC.Text.Trim()) +
                        "%' ";
                }
                */
            }
            else
            {
                if (tbFilterMSISDN.Text.Trim() != "")
                {
                    whr += ((whr != "") ? "and " : "") + "sim.msisdn like '%" +
                        MySqlHelper.EscapeString(tbFilterMSISDN.Text.Trim()) +
                        "%' ";
                }

                if (tbFilterICC.Text.Trim() != "")
                {
                    whr += ((whr != "") ? "and " : "") + "sim.icc like '%" +
                        MySqlHelper.EscapeString(tbFilterICC.Text.Trim()) +
                        "%' ";
                }
            }

            if (cbFS.SelectedIndex < 2 && cbFS.SelectedIndex > -1)
            {
                whr += ((whr != "") ? "and " : "") + "sim.fs = " + cbFS.SelectedIndex.ToString() + " ";
            }


            string simst = "";
            if (cbStatus0.Checked)
            {
                simst += "sim.status = '0'";
            }


            if (cbStatus1.Checked)
            {
                simst += (simst.Equals("") ? "" : " or ") + "sim.status = '1'";
            }

            if (cbStatus2.Checked)
            {
                simst += (simst.Equals("") ? "" : " or ") + "sim.status = '2'";
            }

            if (cbStatus0.Checked && cbStatus1.Checked && cbStatus2.Checked) simst = "";

            // в случае чего раскомментировать
            if (!simst.Equals(""))
            {
                if (whr.Equals("")) whr += "(" + simst + ") ";
                else whr += "and (" + simst + ") ";
            }
            //if (!simst.Equals("")) whr += "and (" + simst + ") ";

            StringTagItem sfi = (StringTagItem)cbFilterParty.SelectedItem;
            try
            {
                if (cbPartyFilter.Checked)
                {
                    string parties = "";
                    selectedParties.Clear();
                    foreach (StringObjTagItem soti in clbParties.CheckedItems)
                    {
                        selectedParties.Add((int)soti.Tag);
                    }
                    parties = String.Join(",", selectedParties);

                    whr += ((whr != "") ? "and " : "") + string.Format("sim.party_id IN ({0}) ", parties);

                }
                else
                {
                    if (!sfi.Tag.Equals(StringTagItem.VALUE_ANY))
                    {
                        whr += ((whr != "") ? "and " : "") + string.Format("sim.party_id = {0} ", sfi.Tag);
                    }
                }

            }
            catch (Exception) { }

            try
            {
                if (cbDateFilter.Checked)
                {
                    string date = deDateSold.Value.ToString("yyyyMMdd");
                    whr += string.Format("and sim.status = '2' and sim.date_sold = '{0}' ", date);
                }
            }
            catch (Exception)
            {
            }

            sfi = (StringTagItem)cbFilterRegion.SelectedItem;
            if (!sfi.Tag.Equals(StringTagItem.VALUE_ANY))
            {
                whr += ((whr != "") ? "and " : "") + string.Format("sim.region_id = '{0}' ", MySqlHelper.EscapeString(sfi.Tag));
            }

            sfi = (StringTagItem)cbFilterPlan.SelectedItem;
            if (!sfi.Tag.Equals(StringTagItem.VALUE_ANY))
            {
                whr += ((whr != "") ? "and " : "") + string.Format("sim.plan_id = '{0}' ", MySqlHelper.EscapeString(sfi.Tag));
            }

            sfi = (StringTagItem)cbFilterUnit.SelectedItem;
            if (!sfi.Tag.Equals(StringTagItem.VALUE_ANY))
            {
                whr += ((whr != "") ? "and " : "") + string.Format("sim.owner_id = {0} ", MySqlHelper.EscapeString(sfi.Tag));
            }

            if (whr != "") whr = "where " + whr;

            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery(sqlc + whr);
            if (t != null && t.Rows.Count > 0)
            {
                tsslItemsCount.Text = string.Format("Записей: {0}", t.Rows[0]["sim_count"].ToString());

                daSim = ((IDEXMySqlData)toolbox).getDataAdapter(sqlr + whr);
                cbSim = new MySqlCommandBuilder(daSim);

                t = new DataTable();
                daSim.Fill(t);

                /*
                int p = 0;
                DataTable pp = new DataTable();
                if (cbOnlyEmpty.Checked && t != null && t.Rows.Count > 0)
                {
                    int cnt = 0;
                    foreach (DataRow r in t.Rows)
                    {
                        string sql = "select count(id) as count_id from journal where digest like '%"+r["msisdn"].ToString()+"%' and status != 5";
                        DataTable dt = d.getQuery(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string dd = dt.Rows[0]["count_id"].ToString();
                            if (dd == "0")
                            {
                                //DataRow dr = ;
                                pp.Rows.Add(r);
                                p++;
                            }
                        }
                        cnt++;
                        
                    }
                }
                */

                
                

                bsSim.DataSource = t;


                dgvSim.Visible = (t != null && t.Rows.Count > 0);
            }
            else
            {
                tsslItemsCount.Text = "Нет записей";
                dgvSim.Visible = false;
            }
            

            pFilter.Enabled = true;
            tsTools.Enabled = true;

//            MessageBox.Show("sqlr:\n\n" + sqlr + "\n\nsqlc:\n\n" + sqlc + "\n\nwhr:\n\n" + whr);

        }

        private void _resetButtonsColors()
        {
            tbFilterMSISDN.BackColor = SystemColors.Window;
            tbFilterICC.BackColor = SystemColors.Window;
            cbStatus0.BackColor = SystemColors.Window;
            cbStatus1.BackColor = SystemColors.Window;
            cbStatus2.BackColor = SystemColors.Window;
            cbFilterParty.BackColor = SystemColors.Window;
            cbFilterRegion.BackColor = SystemColors.Window;
            cbFilterPlan.BackColor = SystemColors.Window;
            cbFilterUnit.BackColor = SystemColors.Window;
        }

        private void bClearFilter_Click(object sender, EventArgs e)
        {
            _clearFilter();
        }

        private void bSetFilter_Click(object sender, EventArgs e)
        {
            _resetButtonsColors();
            _sim();
        }

        private void dgvSim_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string fcol = dgvSim.Columns[e.ColumnIndex].Name;
            if (fcol.StartsWith("date"))
            {
                try
                {
                    string v = e.Value.ToString();
                    if (!v.Equals(""))
                    {
                        e.Value = v.Substring(6, 2) + "." + v.Substring(4, 2) + "." + v.Substring(0, 4);
                    }
                    else
                    {
                        e.Value = "-";
                    }
                }
                catch (Exception)
                {
                    e.Value = "-";
                }
            }
            else if (fcol.Equals("owner_id"))
            {
                // еще регион получим
                string reg = "";
                DataTable dt = null;
                try
                {
                    IDEXData d = (IDEXData)toolbox;
                    dt = d.getQuery("SELECT * FROM units");
                } catch(Exception) {}



                string nval = "-";
                foreach (StringTagItem sfi in cbFilterUnit.Items)
                {
                    if (e.Value.ToString().Equals(sfi.Tag))
                    {
                        string str = sfi.ToString();
                        if (dt != null && dt.Rows.Count > 0) 
                        {
                            foreach(DataRow dr in dt.Rows) 
                            {
                                if (dr["uid"].ToString().Equals(sfi.Tag))
                                {
                                    if (!dr["region"].ToString().Equals(""))
                                    {
                                        str += " [" + dr["region"].ToString() + "]";
                                    }
                                    break;
                                }
                            }
                        }
                        nval = str;//sfi.ToString();
                        break;
                    }
                }

                e.Value = nval;
            }
            else if (fcol.Equals("region_id"))
            {
                string nval = "-";
                foreach (StringTagItem sfi in cbFilterRegion.Items)
                {
                    if (e.Value.Equals(sfi.Tag))
                    {
                        nval = sfi.ToString();
                        break;
                    }
                }

                e.Value = nval;
            }
            else if (fcol.Equals("plan_id"))
            {
                string nval = "-";
                foreach (StringTagItem sfi in cbFilterPlan.Items)
                {
                    if (e.Value.Equals(sfi.Tag))
                    {
                        nval = sfi.ToString();
                        break;
                    }
                }

                e.Value = nval;
            }
            else if (fcol.Equals("status"))
            {
                string[] st = { "Поступила", "Распределена", "Продана" };
                try
                {
                    int sst = int.Parse(e.Value.ToString());
                    if (sst > -1 && sst < 3) e.Value = st[sst];
                }
                catch (Exception)
                {
                }
            }
            else if (fcol.Equals("fs"))
            {
                e.Value = Convert.ToBoolean(e.Value) ? "ФС" : "";
            }
            else if (fcol.Equals("dynamic"))
            {
                e.Value = Convert.ToBoolean(e.Value) ? "Да" : "Нет";
            }
            else if (fcol.Equals("auto_distr")) 
            {
                try
                {
                    string[] st = { "Запрещено", "Разрешено" };
                    int sst = int.Parse(e.Value.ToString());
                    if (sst > -1 && sst < 2) e.Value = st[sst];
                }
                catch (Exception ee) { }
            }
        }

        string _setCmdParams2(string cmd, FSimEd simEd)
        {
            IDEXData d = (IDEXData)toolbox;

            int sstatus = simEd.cbStatus.SelectedIndex;
            // Поступила = 0
            // Распределена = 1
            // Продана = 2

            string ret = cmd.Replace("@status", sstatus.ToString());
            ret = ret.Replace("@msisdn", "'" + d.EscapeString(simEd.tbMSISDN.Text) + "'");
            ret = ret.Replace("@icc", "'" + d.EscapeString(simEd.tbICC.Text) + "'");
            ret = ret.Replace("@date_in", "'" + d.EscapeString(simEd.dtpDate_in.Value.ToString("yyyyMMdd")) + "'");
            ret = ret.Replace("@fs", simEd.cbFS.Checked ? "1" : "0");

            if (simEd.cb_typeSim.SelectedIndex < 0)
            {
                ret = ret.Replace("@dynamic", "'0'");
            }
            else
            {
                ret = ret.Replace("@dynamic", "'" + simEd.cb_typeSim.SelectedIndex + "'");
            }

            int owner_id = -1;

            try
            {
                if (sstatus > 0) owner_id = int.Parse(((StringTagItem)simEd.cbUnit_id.SelectedItem).Tag);
            }
            catch (Exception)
            {
                owner_id = -1;
            }
            ret = ret.Replace("@owner_id", owner_id.ToString());

            string dateown = "";
            try
            {
                if (owner_id > -1) dateown = simEd.dtpDate_own.Value.ToString("yyyyMMdd");
            } catch (Exception) { }
            ret = ret.Replace("@date_own", "'" + d.EscapeString(dateown) + "'");

            string datesold = "";
            try
            {
                if (sstatus == 2) datesold = simEd.dtpDate_sold.Value.ToString("yyyyMMdd");
            } catch (Exception) { }
            ret = ret.Replace("@date_sold", "'" + d.EscapeString(datesold) + "'");

            string region_id = "";
            try
            {
                region_id = ((StringTagItem)simEd.cbRegion_id.SelectedItem).Tag;
            } catch (Exception) { }
            ret = ret.Replace("@region_id", "'" + d.EscapeString(region_id) + "'");

            ret = ret.Replace("@party_id", simEd.nudParty_id.Value.ToString());

            string plan_id = "";
            try
            {
                plan_id = ((StringTagItem)simEd.cbPlan_id.SelectedItem).Tag;
            } catch (Exception) { }
            ret = ret.Replace("@plan_id", "'" + d.EscapeString(plan_id) + "'");

            ret = ret.Replace("@balance", "'" + d.EscapeString(simEd.tbBalance.Text) + "'");

            ret = ret.Replace("@comment", "'" + d.EscapeString(simEd.tbComment.Text) + "'");

            try
            {
                ret = ret.Replace("@auto_distr", "'" + d.EscapeString(simEd.tbComment.Text) + "'");
            }
            catch (Exception e) { }

            return ret;
        }

        private void tsbNewSim_Click(object sender, EventArgs e)
        {
            FSimEd simEd = new FSimEd();
            simEd.InitForm(toolbox, simJournalName[simType], null);
            if (simEd.ShowDialog() == DialogResult.OK)
            {
                IDEXData d = (IDEXData)toolbox;

                string sql =
                    "insert into `" + simJournalName[simType] + "` (status, msisdn, icc, fs, dynamic, date_in, owner_id, " +
                    "date_own, date_sold, region_id, party_id, plan_id, balance, comment, auto_distr) values (" +
                    "@status, @msisdn, @icc, @fs, @dynamic, @date_in, @owner_id, @date_own, " +
                    "@date_sold, @region_id, @party_id, @plan_id, @balance, @comment, @auto_distr)";

                sql = _setCmdParams2(sql, simEd);

                d.runQuery(sql);

                try
                {
                    d.setDataReference("units", ((StringTagItem)simEd.cbUnit_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                try
                {
                    d.setDataReference("regions", ((StringTagItem)simEd.cbRegion_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                try
                {
                    d.setDataReference("plans", ((StringTagItem)simEd.cbPlan_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                UpdatePartyFilter(true);
                _resetButtonsColors();
                _sim();
                bsSim.Position = bsSim.Find("msisdn", simEd.tbMSISDN.Text);

            }
        }

        private void dgvSim_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsbEditSim_Click(null, null);
        }

        private void tsbEditSim_Click(object sender, EventArgs e)
        {
            if (dgvSim.DataSource == null || dgvSim.Rows.Count < 1) return;
            
            DataRow rr = ((DataRowView)bsSim.Current).Row;
            string oldplan = rr["plan_id"].ToString();
            string oldregion = rr["region_id"].ToString();
            int oldunit = int.Parse(rr["owner_id"].ToString());

            FSimEd simEd = new FSimEd();
            simEd.InitForm(toolbox, simJournalName[simType], ((DataRowView)bsSim.Current).Row);
            if (simEd.ShowDialog() == DialogResult.OK)
            {
                IDEXData d = (IDEXData)toolbox;

                string sql =
                    "update `" + simJournalName[simType] + "` set status = @status, msisdn = @msisdn, " +
                    "icc = @icc, fs = @fs, dynamic = @dynamic, date_in = @date_in, owner_id = @owner_id, " +
                    "date_own = @date_own, date_sold = @date_sold, region_id = @region_id, " +
                    "party_id = @party_id, plan_id = @plan_id, balance = @balance, comment = @comment, auto_distr = @auto_distr where id = @id";

                sql = _setCmdParams2(sql, simEd);
                
                string rid = ((DataRowView)bsSim.Current).Row["id"].ToString();
                sql = sql.Replace("@id", rid);

                d.runQuery(sql);


                if (oldunit > -1) d.setDataReference("units", oldunit.ToString(), false);
                if (oldregion != "") d.setDataReference("regions", oldregion, false);
                if (oldplan != "") d.setDataReference("plans", oldplan, false);

                try
                {
                    d.setDataReference("units", ((StringTagItem)simEd.cbUnit_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                try
                {
                    d.setDataReference("regions", ((StringTagItem)simEd.cbRegion_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                try
                {
                    d.setDataReference("plans", ((StringTagItem)simEd.cbPlan_id.SelectedItem).Tag, true);
                }
                catch (Exception)
                {
                }

                UpdatePartyFilter(true);
                _resetButtonsColors();
                _sim();

                bsSim.Position = bsSim.Find("id", rid);
                
            }
        }

        public string DeleteSelectedCards(IWaitMessageEventArgs wmea)
        {
            try
            {
                wmea.canAbort = true;
                wmea.textMessage = "Удаление выделенных SIM-карт";
                wmea.minValue = 0;
                wmea.maxValue = dgvSim.SelectedRows.Count;
                wmea.progressVisible = true;
                int cv = 0;
                wmea.progressValue = cv;
                wmea.DoEvents();

                IDEXData d = (IDEXData)toolbox;

                foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                {
                    DataRowView drv = bsSim[dgvr.Index] as DataRowView;
                    int id = int.Parse(drv.Row["id"].ToString());

                    string oldplan = drv.Row["plan_id"].ToString();
                    string oldregion = drv.Row["region_id"].ToString();
                    int oldunit = int.Parse(drv.Row["owner_id"].ToString());

                    d.runQuery(string.Format(
                        "delete from `{0}` where id = {1}",
                        simJournalName[simType], id
                        ));

                    if (oldunit > -1) d.setDataReference("units", oldunit.ToString(), false);
                    if (oldregion != "") d.setDataReference("regions", oldregion, false);
                    if (oldplan != "") d.setDataReference("plans", oldplan, false);

                    cv++;
                    if (cv % 10 == 0)
                    {
                        wmea.progressValue = cv;
                    }
                    if (wmea.isAborted)
                    {
                        return "Операция прервана";
                    }
                }
            }
            catch (Exception)
            {
                return "В ходе выполнения операции возникла ошибка";
            }

            return "";
        }

        private void tsbDeleteCard_Click(object sender, EventArgs e)
        {
            if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 1)
            {
                if (MessageBox.Show("Удалить выделенные SIM-карты?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string s = WaitMessage.Execute(new WaitMessageEvent(DeleteSelectedCards));
                    if (s == null || s == "") s = "Удаление выделенных SIM-карт произведено";
                    MessageBox.Show(s);
                    UpdatePartyFilter(true);
                    _resetButtonsColors();
                    _sim();
                }
            }
            else
            {
                if (dgvSim.DataSource == null || dgvSim.Rows.Count < 1) return;

                DataRow r = ((DataRowView)bsSim.Current).Row;
                string oldplan = r["plan_id"].ToString();
                string oldregion = r["region_id"].ToString();
                int oldunit = int.Parse(r["owner_id"].ToString());

                if (MessageBox.Show(string.Format(
                    "Удалить SIM-карту <{0}>?", r["msisdn"].ToString()
                    ), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IDEXData d = (IDEXData)toolbox;

                    d.runQuery("delete from `{0}` where id = {1}", simJournalName[simType], r["id"].ToString());

                    if (oldunit > -1) d.setDataReference("units", oldunit.ToString(), false);
                    if (oldregion != "") d.setDataReference("regions", oldregion, false);
                    if (oldplan != "") d.setDataReference("plans", oldplan, false);

                    UpdatePartyFilter(true);
                    _resetButtonsColors();
                    _sim();
                }
            }
        }

        private void tbFilterMSISDN_TextChanged(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = SystemColors.Info;
        }

        private void tsmiDictionaryPlans_Click(object sender, EventArgs e)
        {
            FPlansMain main = new FPlansMain();
            main.toolbox = toolbox;
            main.InitForm();
            main.ShowDialog();
            main.toolbox = null;
            main = null;
            GC.Collect();
            UpdateFilterPanel(true);
        }

        private void tsmiSimImport_Click(object sender, EventArgs e)
        {
            if (simType > 0)
            {
                MessageBox.Show("Внимание!\n\nЗагрузка SIM-карт производится " +
                    "только в справочник активных SIM-карт!\nПеред загрузкой " +
                    "необходимо выбрать активный справочник SIM-карт.");
                return;
            }

            FSimImport simImport = new FSimImport();
            simImport.InitForm(toolbox);
            simImport.ShowDialog();
            simImport.SaveFormParams();
            UpdatePartyFilter(true);
            _sim();
        }

        private void tsmiPartyDelete_Click(object sender, EventArgs e)
        {
            if (simType != 0)
            {
                MessageBox.Show("Данная функция доступна только в режиме справочника активных SIM-карт");
                return;
            }

            try
            {
                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;
                DataTable dtcnt = d.getQuery("select count(party_id) as cpid from `" + simJournalName[simType] +
                    "` where party_id = " + scpid);
                if (dtcnt != null && dtcnt.Rows.Count > 0)
                {
                    int pcount = int.Parse(dtcnt.Rows[0]["cpid"].ToString());
                    if (pcount > 0)
                    {
                        if (MessageBox.Show(
                            string.Format("Удалить партию {0} (Кол-во карт: {1})?", scpid, pcount),
                            "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            d.runQuery(string.Format("delete from `{0}` where party_id = {1}",
                                simJournalName[simType], scpid));

                            UpdateFilterPanel(true);
                            _resetButtonsColors();
                            _sim();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiPartyCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (simType != 0)
            {
                MessageBox.Show("Данная функция доступна только в режиме справочника активных SIM-карт");
                return;
            }

            try
            {
                bool toExcel = cbClipboardExcel.Checked;

                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery("select * from `" + simJournalName[simType] +
                    "` where party_id = " + scpid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int opt = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        int cst = int.Parse(r["status"].ToString());
                        if (cst > 0)
                        {
                            opt = -1;
                            break;
                        }
                    }

                    if (opt != 0)
                    {
                        SimPartyCpClipInfo spcci = new SimPartyCpClipInfo();
                        DialogResult dr = spcci.ShowDialog();
                        if (dr == DialogResult.OK) opt = 1;
                        else if (dr == DialogResult.Yes) opt = 0;
                        else opt = -1;
                    }

                    if (opt > -1)
                    {
                        int problemcnt = 0;

                        string dst = "";
                        int tcnt = 0;
                        foreach (DataRow r in dt.Rows)
                        {
                            int cst = int.Parse(r["status"].ToString());
                            if (cst == 0 || opt == 0)
                            {
                                string nmsi = r["msisdn"].ToString(), nicc = r["icc"].ToString();
                                if (toExcel)
                                {
                                    nmsi = "'" + nmsi;
                                    nicc = "'" + nicc;
                                }
                                try
                                {
//                                    nmsi = "+7 " + nmsi.Substring(0, 3) + " " + nmsi.Substring(3, 3) + "-" + nmsi.Substring(6, 2) + "-" + nmsi.Substring(8, 2);
                                    dst += string.Format("{0}|{1}\r\n", nicc, nmsi);
                                    tcnt++;
                                }
                                catch (Exception)
                                {
                                    problemcnt++;
                                }
                            }
                        }
                        if (problemcnt > 0)
                        {
                            MessageBox.Show(string.Format("Внимание!\n\nВ списке имеются карты ({0}) с неправильным количеством символов." +
                                "\nПеред выгрузкой в буфер, проверьте правильность всех выгружаемых SIM-карт.", problemcnt));
                        }
                        else
                        {
                            if (!dst.Equals(""))
                            {
                                Clipboard.SetText("ID|1|" + tcnt.ToString() + "|Партия: " + scpid + "\r\n" + dst);
                                MessageBox.Show("Информация о партии скопирована в буфер");
                            }
                            else
                            {
                                MessageBox.Show("В буфер обмена не было загружено ни одной карты.");
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiAppendToParty_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery(string.Format(
                    "select distinct party_id from `um_data` where party_id <> {0}", scpid
                    ));
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<int> pids = new List<int>();
                    foreach (DataRow r in dt.Rows)
                    {
                        pids.Add(int.Parse(r["party_id"].ToString()));
                    }
                    if (pids.Count > 0)
                    {
                        FAddToPartyForm pf = new FAddToPartyForm(pids);
                        if (pf.ShowDialog() == DialogResult.OK)
                        {
                            int dpid = int.Parse(pf.cbParties.SelectedItem.ToString());
                            d.runQuery(string.Format(
                                "update `um_data` set party_id={0} where party_id={1}",
                                dpid, scpid
                                ));
                            MessageBox.Show("Перенос завершен");
                            UpdateFilterPanel(true);
                            _resetButtonsColors();
                            _sim();
                        }
                    }
                    else
                    {
                        MessageBox.Show("В таблице SIM-карт менее двух партий.");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiRegion_Click(object sender, EventArgs e)
        {
            try
            {
                string rgn_id = ((ToolStripMenuItem)sender).Tag.ToString();
                string rgn_title = ((ToolStripMenuItem)sender).Text;

                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;

                if (MessageBox.Show(
                    string.Format("Изменить регион партии на <{0}>?", rgn_title), "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    d.runQuery(string.Format(
                        "update `um_data` set region_id='{0}' where party_id={1}",
                        d.EscapeString(rgn_id) , scpid
                        ));
                    MessageBox.Show("Присвоение партии региона завершено");
                    UpdateFilterPanel(true);
                    _resetButtonsColors();
                    _sim();

                }
            }
            catch(Exception)
            {
            }
        }

        private void tsmiPlan_Click(object sender, EventArgs e)
        {
            try
            {
                string pln_id = ((ToolStripMenuItem)sender).Tag.ToString();
                string pln_title = ((ToolStripMenuItem)sender).Text;

                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                string scpid = drv.Row["party_id"].ToString();

                IDEXData d = (IDEXData)toolbox;

                if (MessageBox.Show(
                    string.Format("Изменить ТП партии на <{0}>?", pln_title), "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    d.runQuery(string.Format(
                        "update `um_data` set plan_id='{0}' where party_id={1}",
                        d.EscapeString(pln_id), scpid
                        ));
                    MessageBox.Show("Присвоение партии ТП завершено");
                    UpdateFilterPanel(true);
                    _resetButtonsColors();
                    _sim();

                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiChangePartyId_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                int scpid = int.Parse(drv.Row["party_id"].ToString());

                FChangePartyIdForm form = new FChangePartyIdForm(toolbox, scpid, false);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery(string.Format(
                        "update `um_data` set party_id = {0} where party_id = {1}",
                        form.nudPartyId.Value.ToString(), scpid
                        ));
                    MessageBox.Show("Изменение номера партии завершено");
                    UpdateFilterPanel(true);
                    StringTagItem.SelectByTag(cbFilterParty, form.nudPartyId.Value.ToString(), true);
                    _resetButtonsColors();
                    _sim();
                }
            }
            catch (Exception)
            {
            }
        }

        string cpmsg;
        string cpfield;
        string cpvalue;

        public string ChangeSimProp(IWaitMessageEventArgs wmea)
        {
            try
            {
                wmea.canAbort = false;
                wmea.textMessage = cpmsg;
                wmea.minValue = 0;
                int cv = 0;
                wmea.maxValue = dgvSim.SelectedRows.Count;
                wmea.progressVisible = true;
                wmea.DoEvents();

                IDEXData d = (IDEXData)toolbox;

                foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                {
                    DataRowView drv = bsSim[dgvr.Index] as DataRowView;
                    int id = int.Parse(drv.Row["id"].ToString());

                    d.runQuery("update `um_data` set {0} = {1} where id = {2}", cpfield, cpvalue, id.ToString());
                    cv++;
                    if (cv % 50 == 0)
                    {
                        wmea.progressValue = cv;
                        wmea.DoEvents();
                    }
                }
            }
            catch (Exception)
            {
                return "В ходе выполнения операции возникла ошибка";
            }
            return "";
        }

        private void tsmiChangePartyBalance_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                int scpid = int.Parse(drv.Row["party_id"].ToString());

                FChangeBalance form = new FChangeBalance();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery(string.Format(
                        "update `um_data` set balance = '{0}' where party_id = {1}",
                        d.EscapeString(form.tbBalance.Text), scpid
                        ));
                    MessageBox.Show("Изменение баланса партии завершено");
                    _sim();
                }
            }
            catch (Exception)
            {
            }

        }


        private void tsmiSPlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    string pln_id = ((ToolStripMenuItem)sender).Tag.ToString();
                    string pln_title = ((ToolStripMenuItem)sender).Text;

                    if (MessageBox.Show(
                        string.Format("Изменить ТП выделенных карт на <{0}>?", pln_title), "Подтверждение",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        cpmsg = "Изменение ТП выделенных записей";
                        cpfield = "plan_id";
                        cpvalue = "'" + ((IDEXData)toolbox).EscapeString(pln_id) + "'";
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Изменение ТП успешно произведено";
                        MessageBox.Show(s);
                        _sim();
                    }                    
                }
            }
            catch (Exception)
            {
            }
        }

        private void dgvSim_SelectionChanged(object sender, EventArgs e)
        {
            tsmiSelectedCards.Enabled = dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0;
            tsslSelectedCount.Text = "";
            if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
            {
                tsslSelectedCount.Text = string.Format("Выделено: {0}", dgvSim.SelectedRows.Count);
            }
        }

        private void tsmiSChangePartyId_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    FChangePartyIdForm form = new FChangePartyIdForm(toolbox, 0, true);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        cpmsg = "Изменение номера партии выделенных записей";
                        cpfield = "party_id";
                        cpvalue = form.nudPartyId.Value.ToString();
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Изменение партии успешно произведено";
                        MessageBox.Show(s);
                        UpdateFilterPanel(true);
                        _resetButtonsColors();
                        _sim();
                        
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiSChangeBalance_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    FChangeBalance form = new FChangeBalance();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        cpmsg = "Изменение баланса выделенных записей";
                        cpfield = "balance";
                        cpvalue = "'" + ((IDEXData)toolbox).EscapeString(form.tbBalance.Text) + "'";
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Изменение баланса успешно произведено";
                        MessageBox.Show(s);
                        UpdateFilterPanel(true);
                        _resetButtonsColors();
                        _sim();

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dgvSim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                tsbDeleteCard_Click(null, null);
            }
            else if (e.KeyCode == Keys.Insert)
            {
                tsbNewSim_Click(null, null);
            }
            else if (e.KeyCode == Keys.Space)
            {
                tsbEditSim_Click(null, null);
            }
        }

        string _gd(string src)
        {
            try
            {
                return src.Substring(6, 2) + "." + src.Substring(4, 2) + "." + src.Substring(0, 4);
            }
            catch (Exception)
            {
            }
            return "-";
        }

        private void tsmiSCopyToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                bool toExcel = cbClipboardExcel.Checked;

                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    IDEXData d = (IDEXData)toolbox;
                    DataTable dtunits = d.getQuery("select uid, title from `units`");
                    DataTable dtplans = d.getQuery("select plan_id, title from `um_plans`");
                    DataTable dtregions = d.getQuery("select region_id, title from `um_regions`");

                    //string dta = "Карта продана\tMSISDN\tICC\tФС\tПартия\tОтделение\tПоступление\tВыдача\tПродажа\tТП\tБаланс\tРегион\tПримечание\tСтатусВжурнале\n";
                    string dta = "Карта продана\tMSISDN\tICC\tФС\tПартия\tОтделение\tПоступление\tВыдача\tПродажа\tТП\tБаланс\tРегион\n";
                    
                    foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                    {
                        DataRowView drv = bsSim[dgvr.Index] as DataRowView;
                        
                        string sold = (int.Parse(drv.Row["status"].ToString())) == 2 ? "+" : "-";
                        
                        string unit = "-";
                        if (dtunits != null && dtunits.Rows.Count > 0)
                        {
                            DataRow[] rr = dtunits.Select("uid=" + drv.Row["owner_id"].ToString());
                            if (rr != null && rr.Length > 0)
                            {
                                unit = rr[0]["title"].ToString();
                            }
                        }

                        string plan = "-";
                        if (dtplans != null && dtplans.Rows.Count > 0)
                        {
                            DataRow[] rr = dtplans.Select("plan_id='" + drv.Row["plan_id"].ToString() + "'");
                            if (rr != null && rr.Length > 0)
                            {
                                plan = rr[0]["title"].ToString();
                            }
                        }

                        string region = "-";
                        if (dtregions != null && dtregions.Rows.Count > 0)
                        {
                            DataRow[] rr = dtregions.Select("region_id='" + drv.Row["region_id"].ToString() + "'");
                            if (rr != null && rr.Length > 0)
                            {
                                region = rr[0]["title"].ToString();
                            }
                        }

                        string sfs = Convert.ToBoolean(drv.Row["fs"]) ? "ФС" : "";

                        string msisdn = drv.Row["msisdn"].ToString(), icc = drv.Row["icc"].ToString();

                        if (toExcel)
                        {
                            msisdn = "'" + msisdn;
                            icc = "'" + icc;
                        }

                        string comment = drv.Row["comment"].ToString().Replace('\t', ' ');

                        dta += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\n",
                            sold, msisdn, icc, sfs,
                            drv.Row["party_id"].ToString(), unit, _gd(drv.Row["date_in"].ToString()),
                            _gd(drv.Row["date_own"].ToString()), _gd(drv.Row["date_sold"].ToString()),
                            plan, drv.Row["balance"].ToString(), region);
                    }

                    Clipboard.SetText(dta);
                    MessageBox.Show("Информация о картах скопирована в буфер обмена");
                }
            }
            catch (Exception)
            {
            }

        }

        DataTable dtOstatki;
        bool doDetailedOstatki, doFsSeparated, doDetailedNull, doLessThree, doRegions, doZeroParty;
        int ostatkiCnt;

        public string PrepareOstatki(IWaitMessageEventArgs wmea)
        {
            dtOstatki = null;
            try
            {
                wmea.textMessage = "Подготовка таблицы остатков";
                wmea.canAbort = false;
                wmea.DoEvents();

                IDEXData d = (IDEXData)toolbox;

                string sql;
                if (!doZeroParty)
                {
                    sql =
                       "select count(ud.id) as cnt, ud.plan_id as plan_id, un.title as title, un.region as region, ud.fs " +
                       "from `um_data` as ud, `units` as un " +
                       "where ud.status = 1 and un.uid = ud.owner_id and ud.party_id != '0' and ud.party_id != 1 " +
                       "group by ud.plan_id, un.title";
                }
                else
                {
                    sql =
                      "select count(ud.id) as cnt, ud.plan_id as plan_id, un.title as title, un.region as region, ud.fs " +
                      "from `um_data` as ud, `units` as un " +
                      "where ud.status = 1 and un.uid = ud.owner_id " +
                      "group by ud.plan_id, un.title";
                }
                if (doFsSeparated) sql += ", ud.fs";

                dtOstatki = d.getQuery(sql);

                if (dtOstatki != null && dtOstatki.Rows.Count > 0)
                {
                    Dictionary<string, int> dost = new Dictionary<string, int>();
                    
                    foreach (DataRow r in dtOstatki.Rows)
                    {
                        bool status = true;
                        /*
                        if (doLessThree)
                        {
                            if (Convert.ToInt32(r["cnt"].ToString()) < 3)
                            {
                                int ss = Convert.ToInt32(r["cnt"].ToString());
                                if (ss > 3) 
                                {
                                    string fff = "";
                                }
                                status = true;
                            }
                        } 
                        else if (!doLessThree) status = true;
                        */
                        if (status)
                        {
                            if (r["plan_id"].ToString().Trim().Equals("")) r["plan_id"] = "-";
                            string title = r["title"].ToString();

                            if (doFsSeparated && Convert.ToInt32(r["fs"]) == 1)
                            {
                                title += " (ФС)";
                                r["title"] = title;
                            }

                            if (!dost.ContainsKey(title)) dost[title] = 0;

                            try
                            {
                                dost[title] += Convert.ToInt32(r["cnt"]);
                                if (Convert.ToInt32(r["cnt"]) == 0)
                                {
                                    string fff = "";
                                }
                            }
                            catch (Exception) { }
                        }
                    }

                    if (!doDetailedOstatki) dtOstatki.Rows.Clear();
                    ostatkiCnt = 0;

                    foreach (KeyValuePair<string, int> kvp in dost)
                    {
                        DataRow r = dtOstatki.NewRow();
                        r["plan_id"] = "Всего";
                        r["title"] = kvp.Key;

                        ostatkiCnt += kvp.Value;
                        r["cnt"] = kvp.Value;
                        dtOstatki.Rows.Add(r);
                    }

                    

                    // если нужно показать нулевые остатки, запросим список отделений и добавим те, что отсутствуют в созданном справочнике, поставив им 0
                    if (doDetailedNull)
                    {
                        sql = "SELECT * FROM units";
                        DataTable dtUnits = d.getQuery(sql);
                        if (dtUnits != null && dtUnits.Rows.Count > 0)
                        {
                            foreach (DataRow r in dtUnits.Rows)
                            {
                                if (!dost.ContainsKey(r["title"].ToString()))
                                {
                                    DataRow dr = dtOstatki.NewRow();
                                    dr["plan_id"] = "Всего";
                                    dr["title"] = r["title"].ToString();
                                    dr["region"] = r["region"].ToString();
                                    dr["cnt"] = 0;
                                    dtOstatki.Rows.Add(dr);
                                }
                            }
                        }
                    }

                    // если нужно указать регион отделения
                    if (doRegions)
                    {
                        if (!doZeroParty)
                        {
                            sql = "SELECT * FROM units WHERE party_id != '0' and party_id != '1'";
                        }
                        else
                        {
                            sql = "SELECT * FROM units";
                        }
                        DataTable dtUnits = d.getQuery(sql);
                        if (dtUnits != null && dtUnits.Rows.Count > 0)
                        {
                            foreach (DataRow r in dtUnits.Rows)
                            {
                                foreach (DataRow dr1 in dtOstatki.Rows)
                                {
                                    if (dr1["title"].ToString().Equals(r["title"].ToString())) 
                                    { 
                                        dr1["region"] = r["region"].ToString();
                                        break;
                                    }
                                }

                                
                            }
                        }
                    }
                }
                else
                {
                    return "Нет данных для отчёта";
                }
            }
            catch (Exception)
            {
                dtOstatki = null;   
                return "Операция была прервана в результате внутренней ошибки";
            }

            return "";
        }

        private void tsmiReportOstatki_Click(object sender, EventArgs e)
        {
            doDetailedOstatki = MessageBox.Show("Детализировать остатки по тарифным планам?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            doFsSeparated = MessageBox.Show("Разделять ФС и обычные карты?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            doDetailedNull = MessageBox.Show("Отображать нулевые остатки?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            doRegions = MessageBox.Show("Отображать регион отделения?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            doZeroParty = MessageBox.Show("Брать нулевую и первую партии?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;

            //doLessThree = MessageBox.Show("Выводить только тех, на ком менее 3 карт?", "Подтверждение",
            //    MessageBoxButtons.YesNo) == DialogResult.Yes;
            

            string ret = WaitMessage.Execute(new WaitMessageEvent(PrepareOstatki));
            if (ret.Equals(""))
            {
                Form frm = new Form();
                frm.WindowState = FormWindowState.Maximized;

                ReportViewer rv = new ReportViewer();
                rv.Dock = DockStyle.Fill;
                frm.Controls.Add(rv);


                if (doRegions)
                {
                    rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Dictionary.Beeline.Sim.OstatkiRegions.rdlc";

                }
                else
                {
                    rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Dictionary.Beeline.Sim.Ostatki.rdlc";

                }


                //rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Dictionary.Beeline.Sim.Ostatki.rdlc";
                rv.ProcessingMode = ProcessingMode.Local;

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("ds_ostatki", dtOstatki));

                List<ReportParameter> paraml = new List<ReportParameter>();
                paraml.Add(new ReportParameter("repdate", DateTime.Now.ToString("dd.MM.yyyy")));
                paraml.Add(new ReportParameter("ito_cnt", ostatkiCnt.ToString()));

                rv.LocalReport.SetParameters(paraml);

                rv.SetDisplayMode(DisplayMode.Normal);
                rv.ZoomMode = ZoomMode.PageWidth;

                rv.RefreshReport();

                frm.ShowDialog();
                
            }
            else
            {
                MessageBox.Show(ret);
            }
        }

        public string DoMoveSimToFromArchive(IWaitMessageEventArgs wmea)
        {
            if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
            {
                IDEXData d = (IDEXData)toolbox;

                if (simType == 0) // Из активных в архив
                {
                    wmea.canAbort = true;
                    wmea.textMessage = "Перенос активных SIM-карт в архив";
                    wmea.minValue = 0;
                    wmea.maxValue = dgvSim.SelectedRows.Count;
                    int cnt = 0;
                    wmea.progressValue = cnt;
                    wmea.progressVisible = true;
                    wmea.DoEvents();

                    foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                    {
                        DataRow dr = (bsSim[dgvr.Index] as DataRowView).Row;
                        try
                        {
                            string sql = string.Format(
                                "delete from `{0}` where msisdn = '{1}' and icc = '{2}'",
                                simJournalName[1], d.EscapeString(dr["msisdn"].ToString()),
                                d.EscapeString(dr["icc"].ToString()));
                            d.runQuery(sql);

                            sql = string.Format(
                                "insert into `{0}` (status, msisdn, icc, date_in, owner_id, date_own, " +
                                "date_sold, region_id, party_id, plan_id, balance, comment, data, fs, dynamic, auto_distr) values ({1}, " +
                                "'{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}', '{13}', {14}, {15})",
                                simJournalName[1], dr["status"].ToString(), d.EscapeString(dr["msisdn"].ToString()),
                                d.EscapeString(dr["icc"].ToString()), d.EscapeString(dr["date_in"].ToString()),
                                dr["owner_id"].ToString(), d.EscapeString(dr["date_own"].ToString()),
                                d.EscapeString(dr["date_sold"].ToString()), d.EscapeString(dr["region_id"].ToString()),
                                dr["party_id"].ToString(), d.EscapeString(dr["plan_id"].ToString()),
                                d.EscapeString(dr["balance"].ToString()), d.EscapeString(dr["comment"].ToString()),
                                d.EscapeString(dr["data"].ToString()), Convert.ToBoolean(dr["fs"]) ? 1 : 0, dr["dynamic"].ToString(), dr["auto_distr"].ToString());
                            d.runQuery(sql);

                            sql = string.Format("delete from `{0}` where id = {1}", simJournalName[0], dr["id"].ToString());
                            d.runQuery(sql);
                        }
                        catch (Exception)
                        {
                        }
                        cnt++;
                        if (cnt % 10 == 0)
                        {
                            wmea.progressValue = cnt;
                        }
                        if (wmea.isAborted) return "Операция прервана";
                    }

                    return "";
                }
                else if (simType == 1)
                {
                    wmea.canAbort = true;
                    wmea.textMessage = "Перенос архивных SIM-карт в справочник активных SIM-карт";
                    wmea.minValue = 0;
                    wmea.maxValue = dgvSim.SelectedRows.Count;
                    int cnt = 0;
                    wmea.progressValue = cnt;
                    wmea.progressVisible = true;
                    wmea.DoEvents();

                    foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                    {
                        DataRow dr = (bsSim[dgvr.Index] as DataRowView).Row;
                        try
                        {
                            string sql = string.Format(
                                "select * from `{0}` where msisdn = '{1}' and icc = '{2}'",
                                simJournalName[0], d.EscapeString(dr["msisdn"].ToString()),
                                d.EscapeString(dr["icc"].ToString()));
                            DataTable t = d.getQuery(sql);
                            if (t == null || t.Rows.Count < 1)
                            {
                                sql = string.Format(
                                    "insert into `{0}` (status, msisdn, icc, date_in, owner_id, date_own, " +
                                    "date_sold, region_id, party_id, plan_id, balance, comment, data, fs, dynamic, auto_distr) values ({1}, " +
                                    "'{2}', '{3}', '{4}', {5}, '{6}', '{7}', '{8}', {9}, '{10}', '{11}', '{12}', '{13}', {14}, {15})",
                                    simJournalName[0], dr["status"].ToString(), d.EscapeString(dr["msisdn"].ToString()),
                                    d.EscapeString(dr["icc"].ToString()), d.EscapeString(dr["date_in"].ToString()),
                                    dr["owner_id"].ToString(), d.EscapeString(dr["date_own"].ToString()),
                                    d.EscapeString(dr["date_sold"].ToString()), d.EscapeString(dr["region_id"].ToString()),
                                    dr["party_id"].ToString(), d.EscapeString(dr["plan_id"].ToString()),
                                    d.EscapeString(dr["balance"].ToString()), d.EscapeString(dr["comment"].ToString()),
                                    d.EscapeString(dr["data"].ToString()), Convert.ToBoolean(dr["fs"]) ? 1 : 0, dr["dynamic"].ToString(), dr["auto_distr"].ToString());
                                d.runQuery(sql);

                                sql = string.Format("delete from `{0}` where id = {1}", simJournalName[1], dr["id"].ToString());
                                d.runQuery(sql);
                                
                            }
                            
                        }
                        catch (Exception)
                        {
                        }
                        cnt++;
                        if (cnt % 10 == 0)
                        {
                            wmea.progressValue = cnt;
                        }
                        if (wmea.isAborted) return "Операция прервана";
                    }

                    return "";
                }

                return "Не определен тип журнала";
            }
            else
            {
                return "Нет выделенных записей";
            }
        }

        private void tsmiMoveFromToArchive_Click(object sender, EventArgs e)
        {
            if (simType == 0) // Из активных в архив
            {
                if (MessageBox.Show("Внимание!\n\nВ архив будут перемещены все указанные записи.\n" +
                    "Если в архиве имеются дублирующие записи, они будут удалены.\n\nПереместить?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string ret = WaitMessage.Execute(new WaitMessageEvent(DoMoveSimToFromArchive));
                    if (!ret.Equals("")) MessageBox.Show(ret);
                    _sim();
                }
            }
            else if (simType == 1) // Из архива в активные
            {
                if (MessageBox.Show("Внимание!\n\nВ справочник активных SIM-карт будут перемещены только записи,\n" +
                    "не имеющие дубликатов.\n\nПереместить?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string ret = WaitMessage.Execute(new WaitMessageEvent(DoMoveSimToFromArchive));
                    if (!ret.Equals("")) MessageBox.Show(ret);
                    _sim();
                }
            }
        }

        private void tsmiSimUidChange_Click(object sender, EventArgs e)
        {
            if (simType != 0)
            {
                MessageBox.Show("Данная функция доступна только в режиме справочника активных SIM-карт");
                return;
            }
            tsmiSimChangeUid.DropDownItems.Clear();
            tsmiSimChangeUid.Enabled = false;
            DataTable t = ((IDEXData)toolbox).getQuery("select uid, title from `units` order by title");
            if (t != null && t.Rows.Count > 0)
            {
                tsmiSimChangeUid.Enabled = true;
                foreach (DataRow r in t.Rows)
                {
                    ToolStripMenuItem tsmir = new ToolStripMenuItem(r["title"].ToString());
                    tsmir.Tag = r["uid"].ToString();
                    tsmir.Click += tsmiSimUidChange_Click;
                    tsmiSimChangeUid.DropDownItems.Add(tsmir);
                }
            }
        }

        private void tsmiSIMToArchive_Click(object sender, EventArgs e)
        {
            dSim = new Dictionary<string, string>();
            try
            {
                string clb = Clipboard.GetText();

                string[] cards = clb.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                foreach (string card in cards)
                {
                    try {
                        string[] ccomps = card.Split(';', (char)0x09);
                        string msisdn = ccomps[0].Trim();
                        string icc = ccomps[1].Trim();
                        if (icc.StartsWith("'")) icc = icc.Substring(1).Trim();
                        dSim[msisdn] = icc;
                    } catch (Exception) { }
                }
            }
            catch (Exception)
            {
            }

            if (dSim.Count > 0)
            {
                if (MessageBox.Show(string.Format("Количество записей в буфере обмена: {0}.\nПереместить в архив все карты из буфера обмена?", dSim.Count), 
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string ret = WaitMessage.Execute(new WaitMessageEvent(MoveSimToArchive));
                    if (!ret.Equals(""))
                    {
                        MessageBox.Show("Сообщения:\n\n" + ret);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("В буфере обмена не найдено ни одной записи.\nПоместите в буфер обмена записи о SIM-картах в виде \"MSISDN;ICC\".");
            }
        }


        Dictionary<string, string> dSim;

        public string MoveSimToArchive(IWaitMessageEventArgs wmea)
        {
            string ret = "";

            int simMoved = 0, simErrors = 0, simNotFound = 0;
            List<string> slog = new List<string>();

            try
            {
                wmea.canAbort = true;
                wmea.minValue = 0;
                wmea.maxValue = dSim.Count;
                wmea.textMessage = "Перенос SIM-карт в архив";
                wmea.progressValue = 0;
                wmea.progressVisible = true;

                int pv = 0;

                IDEXData d = (IDEXData)toolbox;

                foreach (KeyValuePair<string, string> sim in dSim)
                {
                    DataTable t = d.getQuery(
                        "select * from `um_data` where msisdn = '{0}'",
                        d.EscapeString(sim.Key));
                    string smessage = "";
                    if (t != null && t.Rows.Count > 0)
                    {
                        foreach (DataRow row in t.Rows)
                        {
                            try
                            {
                                string ricc = row["icc"] as string;
                                if (ricc.StartsWith(sim.Value) || sim.Value.StartsWith(ricc))
                                {
                                    d.runQuery(
                                        "delete from `um_data_out` where msisdn = '{0}' and icc = '{1}'",
                                        d.EscapeString(row["msisdn"] as string),
                                        d.EscapeString(row["icc"] as string));

                                    d.runQuery(
                                        "insert into `um_data_out` (status, msisdn, icc, date_in, owner_id, " +
                                        "date_own, date_sold, region_id, party_id, plan_id, balance, comment, data, fs, auto_distr) " +
                                        "values ({0}, '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', {8}, " +
                                        "'{9}', '{10}', '{11}', '{12}', {13})",
                                        row["status"], d.EscapeString(row["msisdn"] as string),
                                        d.EscapeString(row["icc"] as string),
                                        d.EscapeString(row["date_in"] as string), row["owner_id"],
                                        d.EscapeString(row["date_own"] as string),
                                        d.EscapeString(row["date_sold"] as string),
                                        d.EscapeString(row["region_id"] as string),
                                        row["party_id"], d.EscapeString(row["plan_id"] as string),
                                        d.EscapeString(row["balance"] as string),
                                        d.EscapeString(row["comment"] as string),
                                        d.EscapeString(row["data"] as string),
                                        Convert.ToBoolean(row["fs"]) ? 1 : 0, row["auto_distr"]);

                                    d.runQuery("delete from `um_data` where id = {0}", row["id"]);
                                    simMoved++;
                                    smessage = "(+) Успешно перенесена в архив";
                                }
                                else
                                {
                                    smessage = "(?) Найден MSISDN, но не найден ICC";
                                }
                            }
                            catch (Exception)
                            {
                                simErrors++;
                                smessage = "(!) Ошибка при архивации";
                            }
                        }
                    }
                    else
                    {
                        simNotFound++;
                        smessage = "(-) Не найдена в справочнике SIM";
                    }

                    slog.Add(string.Format("{0};{1};{2}\n", sim.Key, sim.Value, smessage));

                    wmea.progressValue = pv++;
                    if (wmea.isAborted) break;
                }

                string sr = "";
                foreach (string sr2 in slog) sr += sr2;
                Clipboard.SetText(sr);

                ret = string.Format(
                    "Записей перенесено в архив: {0}\n" +
                    "Ошибок переноса: {1}\n" +
                    "Не найдено в справочнике (возможно, уже в архиве): {2}\n" +
                    "Полный отчёт в буфере обмена в формате CSV", 
                    simMoved, simErrors, simNotFound);
            }
            catch (NullReferenceException)
            {
                ret += "Не инициализирован список SIM-карт\n";
            }
            catch (Exception)
            {
                ret += "Ошибка выполнения процедуры переноса SIM-карт в архив.\n";
            }



            return ret;
        }

        private void tsmiDictionaryBalance_Click(object sender, EventArgs e)
        {
            FBalanceMain bm = new FBalanceMain(toolbox);
            bm.ShowDialog();
            bm = null;
            GC.Collect();
            UpdateFilterPanel(true);
        }

        private void changeFsStatus(bool fs)
        {
            try
            {
                DataRowView drv = bsSim[dgvSim.CurrentRow.Index] as DataRowView;
                int scpid = int.Parse(drv.Row["party_id"].ToString());

                string nst = fs ? "ФС" : "Обычная";
                if (MessageBox.Show("Изменить статус всех карт в партии на \"" + nst + "\"?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery(string.Format("update `um_data` set fs = {0} where party_id = {1}", (fs ? 1 : 0), scpid));
                    MessageBox.Show("Изменение статуса карт партии завершено");
                    UpdateFilterPanel(true);
                    _resetButtonsColors();
                    _sim();
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiSetFS_Click(object sender, EventArgs e)
        {
            changeFsStatus(true);
        }

        private void tsmiSetNonFS_Click(object sender, EventArgs e)
        {
            changeFsStatus(false);
        }

        private void msMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsmiSChangeType_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    FChangeSimType form = new FChangeSimType(toolbox, 0, true);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        cpmsg = "Изменение типа сим-карты";
                        cpfield = "dynamic";
                        cpvalue = form.cbTypeSim.SelectedIndex.ToString();
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Изменение типа сим-карты произошло успешно";
                        MessageBox.Show(s);
                        UpdateFilterPanel(true);
                        _resetButtonsColors();
                        _sim();

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tsmiSUnitsEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    FSimUnitsEd form = new FSimUnitsEd(toolbox);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        cpvalue = "'" + ((StringTagItem)form.cbUnit.SelectedItem).Tag.ToString() + "'";
                        foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                        {
                            DataRow dr = (bsSim[dgvr.Index] as DataRowView).Row;
                            IDEXData d = (IDEXData)toolbox;
                            d.runQuery(string.Format("update `um_data` set owner_id = {0} where id = {1}", cpvalue, dr["id"].ToString()));
                        }
                        MessageBox.Show("Изменение отделения у выделенных карт завершено");
                        UpdateFilterPanel(true);
                        _resetButtonsColors();
                        _sim();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void tbFilterMSISDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                tbFilterICC.Focus();
            }
        }

        private void tbFilterICC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                bSetFilter.PerformClick();
            }
        }

        private void returnSimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Вы уверены, что хотите вернуть SIM-карты на склад? Информаия о распределении будет стерта!", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        cpmsg = "Возврат выделенных SIM на склад";
                        IDEXData d = (IDEXData)toolbox;
                        foreach (DataGridViewRow dgvr in dgvSim.SelectedRows)
                        {
                            DataRow dr = (bsSim[dgvr.Index] as DataRowView).Row;
                            string id = dr["id"].ToString();

                            DataTable t = d.getQuery(
                                "select * from `um_data` where id = '{0}'",
                                d.EscapeString(id));

                            if (t != null && t.Rows.Count > 0)
                            {
                                string ssss = t.Rows[0]["date_sold"].ToString();
                                if (t.Rows[0]["date_sold"].ToString().Equals(""))
                                {
                                    d.runQuery(string.Format(
                                    "update `um_data` set owner_id = '-1', date_own = '', region_id='' where id = {0}",
                                     id
                                    ));
                                }
                            }


                        }
                        MessageBox.Show("Возврат выделенных SIM-карт осуществлен успешно");
                        _sim();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void cbPartyFilter_CheckedChanged(object sender, EventArgs e)
        {
            clbParties.Enabled = !clbParties.Enabled;
        }

        private void tsmiAdvancedSearch_Click(object sender, EventArgs e)
        {
            pFilter.Enabled = false;
            tsTools.Enabled = false;
            try
            {
                if (cbSearchBySimList.Checked)
                {
                    FAdvancedSearchList simSearch = new FAdvancedSearchList();
                    simSearch.InitForm(toolbox);
                    simSearch.ShowDialog();
                    DataTable dt = (DataTable)simSearch.dgvPreview.DataSource;
                    dgvSim.DataSource = null;
                    bsSim.DataSource = null;
                    IDEXData d = (IDEXData)toolbox;

                    DataTable newDt = new DataTable();
                    foreach (DataRow r in dt.Rows)
                    {
                        string whr = "";
                        string sqlr = "select sim.* from `" + simJournalName[simType] + "` as sim ";
                        //whr += string.Format("WHERE sim.msisdn ='{0}'", r["msisdn"].ToString()) + string.Format(" AND sim.icc='{0}'", r["icc"].ToString());
                        whr += string.Format("WHERE sim.msisdn ='{0}'", r["msisdn"].ToString());
                        DataTable t = d.getQuery(sqlr + whr);
                        if (newDt.Rows.Count == 0)
                        {
                            foreach (DataColumn column in t.Columns)
                            {
                                DataColumn col = new DataColumn();
                                col.DataType = column.DataType;
                                col.ColumnName = column.ColumnName;
                                newDt.Columns.Add(col);
                            }
                        }
                        DataRow dr = newDt.NewRow();
                        foreach (DataRow drw in t.Rows)
                        {
                            foreach (DataColumn column in t.Columns)
                            {
                                dr[column.ColumnName] = drw[column.ColumnName];
                            }
                        }
                        newDt.Rows.Add(dr);
                    }

                    dgvSim.DataSource = newDt;
                    dgvSim.Visible = true;
                    bsSim.DataSource = newDt;
                    tsslItemsCount.Text = string.Format("Записей: {0}", newDt.Rows.Count.ToString());
                }
                else
                {
                    MessageBox.Show("Включите поиск по списку сим!");
                }
            } 
            catch (Exception) {
            }
            pFilter.Enabled = true;
            tsTools.Enabled = true;
        }

        private void clbParties_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool nstate = clbParties.CheckedItems.Count < clbParties.Items.Count;
                clbParties.BeginUpdate();
                try
                {
                    for (int i = 0; i < clbParties.Items.Count; ++i)
                    {
                        clbParties.SetItemChecked(i, nstate);
                    }
                }
                finally
                {
                    clbParties.EndUpdate();
                }
            }
        }

        private void tsimSDistr_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSim.SelectedRows != null && dgvSim.SelectedRows.Count > 0)
                {
                    FChangeDistrType form = new FChangeDistrType(toolbox, 0, true);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        cpmsg = "Блокировка|разблокировка автоматического распределения";
                        cpfield = "auto_distr";
                        cpvalue = form.cbDistrSim.SelectedIndex.ToString();
                        string s = WaitMessage.Execute(new WaitMessageEvent(ChangeSimProp));
                        if (s == null || s == "") s = "Операция осуществлена успешно";
                        MessageBox.Show(s);
                        UpdateFilterPanel(true);
                        _resetButtonsColors();
                        _sim();

                    }
                }
            }
            catch (Exception)
            {
            }
        }

       
    }

}
