using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using DEXExtendLib;

namespace DEXPlugin.Function.Yota.DistributeSIM
{
    public partial class DistributeSimMain : Form
    {
        object toolbox;
        //Dictionary<string, string> plans;

        int markedFreeSim, markedAssignedSim;

        #region Инициализация
        public DistributeSimMain(object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;

            /*
            plans = new Dictionary<string, string>();
            try
            {
                DataTable dt = ((IDEXData)toolbox).getQuery("select * from `um_plans`");

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        plans[r["plan_id"].ToString()] = r["title"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
            */

            IDEXConfig cfg = (IDEXConfig)toolbox;
            tsmiDate.Checked = cfg.getBool(this.Name, "tsmiDate", true);
            tsmiICC.Checked = cfg.getBool(this.Name, "tsmiICC", true);
            //tsmiMsisdn.Checked = cfg.getBool(this.Name, "tsmiMsisdn", true);
            tsmiParty.Checked = cfg.getBool(this.Name, "tsmiParty", true);
            //tsmiPlan.Checked = cfg.getBool(this.Name, "tsmiPlan", true);

            cbAssignToClipboard.Checked = cfg.getBool(this.Name, "cbAssignToClipboard", false);
            ofd.FileName = cfg.getStr(this.Name, "ofd", "");
            sfd.FileName = cfg.getStr(this.Name, "sfd", "");

            cbSortField.SelectedIndex = cfg.getInt(this.Name, "cbSortField", 0);
            this.Size = new Size(cfg.getInt(this.Name, "Width", this.Size.Width), cfg.getInt(this.Name, "Height", this.Size.Height));
            panel1.Width = cfg.getInt(this.Name, "panel1.Width", this.panel1.Width);

            UpdateFilterPanel(false);

            StringTagItem.SelectByTag(cbParty, cfg.getStr(this.Name, "PartyId", ""), true);
            StringTagItem.SelectByTag(cbUnit, cfg.getStr(this.Name, "UnitId", ""), true);

            markedFreeSim = 0;
            markedAssignedSim = 0;

            _select();
        }

        public void SaveParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setBool(this.Name, "tsmiDate", tsmiDate.Checked);
            cfg.setBool(this.Name, "tsmiICC", tsmiICC.Checked);
            //cfg.setBool(this.Name, "tsmiMsisdn", tsmiMsisdn.Checked);
            cfg.setBool(this.Name, "tsmiParty", tsmiParty.Checked);
            //cfg.setBool(this.Name, "tsmiPlan", tsmiPlan.Checked);

            cfg.setBool(this.Name, "cbAssignToClipboard", cbAssignToClipboard.Checked);
            cfg.setStr(this.Name, "ofd", ofd.FileName);
            cfg.setStr(this.Name, "sfd", sfd.FileName);

            cfg.setInt(this.Name, "cbSortField", cbSortField.SelectedIndex);
            cfg.setInt(this.Name, "Width", this.Size.Width);
            cfg.setInt(this.Name, "Height", this.Size.Height);
            cfg.setInt(this.Name, "panel1.Width", panel1.Width);

            if (cbParty.SelectedItem != null)
            {
                cfg.setStr(this.Name, "PartyId", ((StringTagItem)cbParty.SelectedItem).Tag);
            }

            if (cbUnit.SelectedItem != null)
            {
                cfg.setStr(this.Name, "UnitId", ((StringTagItem)cbUnit.SelectedItem).Tag);
            }
        }

        private void bFields_Click(object sender, EventArgs e)
        {
            cmsFields.Show((Control)sender, new Point(0, ((Control)sender).Size.Height));
        }
        #endregion

        private void UpdatePartyFilter(bool saveCurrent)
        {
            DataTable t = ((IDEXData)toolbox).getQuery("select party_id from `um_data` group by party_id");
            StringTagItem.UpdateCombo(cbParty, t, "Все карты", "party_id", "party_id", saveCurrent);
        }

        private void UpdateFilterPanel(bool saveCurrent)
        {
            UpdatePartyFilter(saveCurrent);
            
            DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, t, null, "uid", "title", saveCurrent);
        }

        string FormatRow(DataRow r)
        {
            string ret = "";
            try
            {
                //if (tsmiMsisdn.Checked) ret += r["msisdn"].ToString();
                if (tsmiICC.Checked) ret += (ret != "" ? ", " : "") + r["icc"].ToString();
                if (tsmiParty.Checked) ret += (ret != "" ? ", " : "") + r["party_id"].ToString();
                if (tsmiDate.Checked) 
                {
                    string _d = r["date_in"].ToString();
                    ret += (ret != "" ? ", " : "") + _d.Substring(6, 2) + "." + _d.Substring(4, 2) + "." + _d.Substring(0, 4);
                }
                
                /*
                if (tsmiPlan.Checked)
                {
                    ret += (ret != "" ? ", " : "");
                    if (plans.ContainsKey(r["plan_id"].ToString()))
                    {
                        ret += plans[r["plan_id"].ToString()];
                    }
                    else
                    {
                        ret += r["plan_id"].ToString();
                    }
                }
                */
            }
            catch(Exception)
            {
            }
            return ret;
        }

        void _select()
        {
            lbUnassigned.Visible = false;
            lbUnsold.Visible = false;
            //todo

            lbUnassigned.Items.Clear();
            lbUnsold.Items.Clear();

            string[] ordt = { "icc", "party_id", "date_in" };

            try
            {
                IDEXData d = (IDEXData)toolbox;
                
                string sql = string.Format("select * from `um_data` where status = 1 and " +
                    "owner_id = {0}",
                    ((StringTagItem)cbUnit.SelectedItem).Tag
                    );

                if (!((StringTagItem)cbParty.SelectedItem).Tag.Equals(StringTagItem.VALUE_ANY))
                {
                    sql += " and party_id = " + ((StringTagItem)cbParty.SelectedItem).Tag;
                }

                sql += " order by " + ordt[cbSortField.SelectedIndex];

                DataTable dt = d.getQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        string dp = r["date_own"].ToString();
                        dp = dp.Substring(6, 2) + "." + dp.Substring(4, 2) + "." + dp.Substring(0, 4);

                        //string pln = r["plan_id"].ToString();
                        //if (plans.ContainsKey(pln)) pln = plans[pln];


                        SimItem snew = new SimItem(
                            r["id"].ToString(), FormatRow(r), r["icc"].ToString(),
                            dp, r["party_id"].ToString(), 
                            false);
                        lbUnsold.Items.Add(snew);
                    }
                }

                sql = "select * from `um_data` where status = 0";

                if (!((StringTagItem)cbParty.SelectedItem).Tag.Equals(StringTagItem.VALUE_ANY))
                {
                    sql += " and party_id = " + ((StringTagItem)cbParty.SelectedItem).Tag;
                }

                sql += " order by " + ordt[cbSortField.SelectedIndex];

                dt = d.getQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        string dp = r["date_in"].ToString();
                        dp = dp.Substring(6, 2) + "." + dp.Substring(4, 2) + "." + dp.Substring(0, 4);

                       // string pln = r["plan_id"].ToString();
                        //if (plans.ContainsKey(pln)) pln = plans[pln];

                        SimItem snew = new SimItem(
                            r["id"].ToString(), FormatRow(r), r["icc"].ToString(), 
                            dp, r["party_id"].ToString(),
                            false
                            );
                        lbUnassigned.Items.Add(snew);
                    }
                }
            }
            catch(Exception)
            {
            }

            markedFreeSim = 0;
            markedAssignedSim = 0;

            lbUnassigned.Visible = lbUnassigned.Items.Count > 0;
            lbUnsold.Visible = lbUnsold.Items.Count > 0;

        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            _select();
        }

        private void cmsFields_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _select();
        }

        private void lbUnassigned_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListBox lb = (ListBox)sender;
                foreach (SimItem si in lb.SelectedItems)
                {
                    si.ch = !si.ch;
                }

                int cc = 0;
                foreach (SimItem si in lb.Items)
                {
                    if (si.ch) cc++;
                }

                if (sender == lbUnassigned)
                {
                    markedFreeSim = cc;
                    setCountsLabel(lFreeSim, "Свободные карты", markedFreeSim, lb.SelectedItems.Count);
                }
                else
                {
                    markedAssignedSim = cc;
                    setCountsLabel(lAssignedSim, "Непроданные карты", markedAssignedSim, lb.SelectedItems.Count);
                }

                lb.Invalidate(true);
                lb.Update();
            }
        }

        private void lbUnassigned_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground(); //Draw an item's background
            e.DrawFocusRectangle(); //Draw an item's border focus

            SimItem si = (SimItem)(((ListBox)sender).Items[e.Index]);

            string s = si.ToString();
            e.Graphics.DrawString(s, e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
        }

        private void tsmiMsisdn_CheckedChanged(object sender, EventArgs e)
        {
            _select();
        }

        private void bAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;
                string d_own = DateTime.Now.ToString("yyyyMMdd"), owner = ((StringTagItem)cbUnit.SelectedItem).Tag;

                string sqwh = "";

                IDEXData d = (IDEXData)toolbox;

                string cpb = "";

                foreach (SimItem sim in lbUnassigned.Items)
                {
                    if (sim.ch)
                    {
                        cpb += sim.icc + "\n";
                        sqwh += (sqwh.Equals("") ? "" : " or ") + string.Format("id = {0}", sim.id);
                        if (cnt % 100 == 0 && !sqwh.Equals(""))
                        {
                            d.runQuery(string.Format(
                                "update `um_data` set status = 1, owner_id = {0}, date_own = '{1}' where {2}",
                                owner, d_own, sqwh)
                                );
                            sqwh = "";
                        }
                        cnt++;
                    }
                }

                if (!sqwh.Equals(""))
                {
                    d.runQuery(string.Format(
                        "update `um_data` set status = 1, owner_id = {0}, date_own = '{1}' where {2}",
                        owner, d_own, sqwh)
                        );
                }

                if (cnt > 0)
                {

                    cpb = "ID|2|" + cnt.ToString() + "|Отделение: " + ((StringTagItem)cbUnit.SelectedItem).Text + "\n" + cpb;

                    if (cbAssignToClipboard.Checked) Clipboard.SetText(cpb);

                    _select();
                }
                else
                {
                    MessageBox.Show("Не помечено ни одной карты для операции.");
                }
            }
            catch (Exception)
            {
            }
        }

        private void bUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = 0;

                string sqwh = "";

                IDEXData d = (IDEXData)toolbox;

                foreach (SimItem sim in lbUnsold.Items)
                {
                    if (sim.ch)
                    {
                        sqwh += (sqwh.Equals("") ? "" : " or ") + string.Format("id = {0}", sim.id);
                        if (cnt % 100 == 0 && !sqwh.Equals(""))
                        {
                            d.runQuery(string.Format("update `um_data` set status = 0, owner_id = -1, date_own = '' where {0}", sqwh));
                            sqwh = "";
                        }
                        cnt++;
                    }
                }

                if (!sqwh.Equals(""))
                {
                    d.runQuery(string.Format("update `um_data` set status = 0, owner_id = -1, date_own = '' where {0}", sqwh));
                }

                if (cnt > 0)
                {
                    _select();
                }
                else
                {
                    MessageBox.Show("Не помечено ни одной карты для операции.");
                }
            }
            catch (Exception)
            {
            }
        }

        private void bDistributeFromFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                try
                {
                    IDEXData d = (IDEXData)toolbox;
                    DataTable sd = d.getQuery("select id, icc, owner_id from `um_data` where status = 0 or status = 1");

                    if (sd != null && sd.Rows.Count > 0)
                    {

                        string[] ss = File.ReadAllLines(ofd.FileName);
                        int all = 0, distributed = 0, missed = 0;
                        foreach (string s in ss)
                        {
                            bool isDistributed = false;
                            string[] str = s.Split(new char[] { ';' }, StringSplitOptions.None);
                            if (str != null && str.Length >= 2)
                            {
                                DataRow[] sels = sd.Select("icc=" + str[1].Substring(6));
                                if (sels != null && sels.Length > 0)
                                {

                                    //if (str[0].Equals(sels[0]["msisdn"].ToString()))
                                    //{
                                        try
                                        {
                                            string dbd = str[3].Substring(6, 4) + str[3].Substring(3, 2) + str[3].Substring(0, 2);
                                            d.runQuery(string.Format(
                                                "update `um_data` set status = 1, owner_id = {0}, date_own = '{1}' where id = {2}",
                                                str[2], d.EscapeString(dbd), sels[0]["id"].ToString())
                                                );
                                            isDistributed = true;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    //}
                                }
                            }

                            if (isDistributed) distributed++;
                            else missed++;
                            all++;
                        }

                        MessageBox.Show("Импорт распределения завершен.\n" +
                            "Всего записей: " + all.ToString() + "\n" +
                            "Распределено: " + distributed.ToString() + "\n" +
                            "Пропущено: " + missed.ToString());

                        _select();
                    }
                    else
                    {
                        MessageBox.Show("Невозможно открыть справочник SIM-карт либо он не содержит записей.");
                    }
                }
                catch (Exception)
                {
                }

            }
        }

        DataTable ListBoxToDataTable(ListBox src, bool hdrs)
        {
            DataTable ret = new DataTable();
            ret.Columns.Add("cdate", typeof(string));
            //ret.Columns.Add("msisdn", typeof(string));
            ret.Columns.Add("icc", typeof(string));
            ret.Columns.Add("party", typeof(string));
            //ret.Columns.Add("plan", typeof(string));

            try
            {
                if (hdrs)
                {
                    DataRow nr = ret.NewRow();
                    nr["cdate"] = "Дата";
                    //nr["msisdn"] = "MSISDN";
                    nr["icc"] = "ICC";
                    nr["party"] = "Партия";
                    //nr["plan"] = "ТП";
                    ret.Rows.Add(nr);
                }

                foreach (SimItem sim in src.Items)
                {
                    DataRow nr = ret.NewRow();
                    nr["cdate"] = sim.date;
                    //nr["msisdn"] = sim.msisdn;
                    nr["icc"] = sim.icc;
                    nr["party"] = sim.party;
                    //nr["plan"] = sim.plan;
                    ret.Rows.Add(nr);
                }
            }
            catch (Exception)
            {
            }

            return ret;
        }

        private void bPrintUnassigned_Click(object sender, EventArgs e)
        {
            DataTable dtsrc = ListBoxToDataTable(lbUnassigned, false);
            if (dtsrc != null && dtsrc.Rows.Count > 0)
            {
                Form frm = new Form();
                frm.WindowState = FormWindowState.Maximized;


                ReportViewer rv = new ReportViewer();
                rv.Dock = DockStyle.Fill;
                frm.Controls.Add(rv);

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("ds_sim", dtsrc));

                rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Function.Yota.DistributeSIM.SimList.rdlc";
                rv.ProcessingMode = ProcessingMode.Local;
                rv.SetDisplayMode(DisplayMode.PrintLayout);

                rv.LocalReport.SetParameters(
                    new ReportParameter[] {
                        new ReportParameter("SimListType", "Неприсвоенные SIM-карты"),
                        new ReportParameter("SimOwner", "-"),
                        new ReportParameter("Datextp", "поступления"),
                        new ReportParameter("SimListDate", DateTime.Now.ToString("dd.MM.yyyy"))
                    }
                    );

                rv.RefreshReport();

                frm.ShowDialog();
                
            }
        }

        private void bPrintUnsold_Click(object sender, EventArgs e)
        {
            DataTable dtsrc = ListBoxToDataTable(lbUnsold, false);
            if (dtsrc != null && dtsrc.Rows.Count > 0)
            {
                Form frm = new Form();
                frm.WindowState = FormWindowState.Maximized;


                ReportViewer rv = new ReportViewer();
                rv.Dock = DockStyle.Fill;
                frm.Controls.Add(rv);

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("ds_sim", dtsrc));

                rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Function.Yota.DistributeSIM.SimList.rdlc";
                rv.ProcessingMode = ProcessingMode.Local;
                rv.SetDisplayMode(DisplayMode.PrintLayout);

                rv.LocalReport.SetParameters(
                    new ReportParameter[] {
                        new ReportParameter("SimListType", "SIM-карты отделения"),
                        new ReportParameter("SimOwner", cbUnit.Text),
                        new ReportParameter("Datextp", "распределения"),
                        new ReportParameter("SimListDate", DateTime.Now.ToString("dd.MM.yyyy"))
                    }
                    );

                rv.RefreshReport();

                frm.ShowDialog();

            }
        }

        private void bExportUnsold_Click(object sender, EventArgs e)
        {
            DataTable dtsrc = ListBoxToDataTable(lbUnsold, true);
            if (dtsrc != null && dtsrc.Rows.Count > 0)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CSVParser.tableToFile(dtsrc, ";", true, sfd.FileName, false);
                        /*
                        string ssrc = CSVParser.tableToString(dtsrc, ";", true);
                        File.WriteAllText(sfd.FileName, ssrc);
                         */
                        MessageBox.Show("Данные сохранены в файл:\n" + sfd.FileName);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void setCountsLabel(Label lbl, string txt, int marked, int selected)
        {
            string res = "";
            try
            {
                if (marked > 0) res += " Помечено: " + marked.ToString();
                if (selected > 0) res += " Выделено: " + selected.ToString();
                if (!res.Equals("")) res = "(" + res.Trim() + ")";
            }
            catch (Exception)
            {
            }

            lbl.Text = txt + " " + res;
        }

        private void lbUnassigned_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCountsLabel(lFreeSim, "Свободные карты", markedFreeSim, lbUnassigned.SelectedItems.Count);
        }

        private void lbUnsold_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCountsLabel(lAssignedSim, "Непроданные карты", markedAssignedSim, lbUnsold.SelectedItems.Count);
        }
    }

    public class SimItem
    {
        public string id, text, icc, date, party;
        public bool ch;

        public SimItem(string aid, string atext, string aicc, string adate, string aparty, bool ach)
        {
            id = aid;
            text = atext;
            ch = ach;
            icc = aicc;
            date = adate;
            party = aparty;
        }

        public override string ToString()
        {
            return (ch ? "[X]     " : "") + text;
        }
    }
}
