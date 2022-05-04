using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using DEXExtendLib;

namespace DEXPlugin.Report.Yota.PeriodReestr
{
    public partial class PeriodReestrMain : Form
    {
        Object toolbox;
        DataTable reportTable;

        public PeriodReestrMain()
        {
            InitializeComponent();
        }

        /* Поля сортировки
         *  Дата договора
            Тип договора
            № договора
            MSISDN
            Ф.И.О. / Наименование
            Отделение
         */
//        string[] SortFields = { "date", "plan", "docnum", "msisdn", "fio", "unit" };


        public void InitForm(Object toolbox)
        {
            this.toolbox = toolbox;

            ArrayList dcs = ((IDEXPluginSystemData)toolbox).getPlugins().getDocuments();
            if (dcs != null && dcs.Count > 0)
            {
                foreach (IDEXPluginDocument dci in dcs)
                {
                    cbDocType.Items.Add(new StringTagItem(dci.Title, dci.ID));
                }
            }

            IDEXConfig cfg = (IDEXConfig)toolbox;
            deStart.Value = cfg.getDate(this.Name, "deStart", DateTime.Now);
            deEnd.Value = cfg.getDate(this.Name, "deEnd", DateTime.Now);
//            cbSort.SelectedIndex = cfg.getInt(this.Name, "cbSort", 0);
            cbFilter.SelectedIndex = cfg.getInt(this.Name, "cbFilter", 0);
            //cbIgnorePlan.Checked = cfg.getBool(this.Name, "cbIgnorePlan", false);
            StringTagItem.SelectByTag(cbDocType, cfg.getStr(this.Name, "cbDocType", ""), true);
            //cbExtReport.Checked = cfg.getBool(this.Name, "cbExtReport", false);

            DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, t, "Любое", "uid", "title", false);

            if (cbUnit.Items.Count > 0)
                StringTagItem.SelectByTag(cbUnit, cfg.getStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.Items[0]).Tag), true);

            t = ((IDEXData)toolbox).getTable("um_regions");
            StringTagItem.UpdateCombo(cbRegion, t, "Любой", "region_id", "title", false);
            StringTagItem.SelectByTag(cbRegion, cfg.getStr(this.Name, "cbRegion", StringTagItem.VALUE_ANY), true);

            t = ((IDEXData)toolbox).getTable("um_plans");
            //clbPlans.Items.Clear();

            if (t != null && t.Rows.Count > 0)
            {
                foreach (DataRow r in t.Rows)
                {
                    StringTagItem sti = new StringTagItem(r["title"].ToString(), r["plan_id"].ToString());
                    //clbPlans.Items.Add(sti, cfg.getBool(this.Name, "clbPlans_item_" + sti.Tag.GetHashCode().ToString(), false));
                }
            }
        }

        public void SaveForm()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "deStart", deStart.Value);
            cfg.setDate(this.Name, "deEnd", deEnd.Value);
//            cfg.setInt(this.Name, "cbSort", cbSort.SelectedIndex);
            cfg.setInt(this.Name, "cbFilter", cbFilter.SelectedIndex);
            cfg.setStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
            cfg.setStr(this.Name, "cbRegion", ((StringTagItem)cbRegion.SelectedItem).Tag);
            //cfg.setBool(this.Name, "cbIgnorePlan", cbIgnorePlan.Checked);
            //cfg.setBool(this.Name, "cbExtReport", cbExtReport.Checked);

            if (cbDocType.SelectedIndex > -1) cfg.setStr(this.Name, "cbDocType", ((StringTagItem)cbDocType.SelectedItem).Tag);

            //for (int f = 0; f < clbPlans.Items.Count; ++f)
            //{
            //    StringTagItem sti = (StringTagItem)clbPlans.Items[f];
            //    cfg.setBool(this.Name, "clbPlans_item_" + sti.Tag.GetHashCode().ToString(), clbPlans.GetItemChecked(f));
            //}
        }

        public string MakeReport(IWaitMessageEventArgs e)
        {
            //bool extReport = cbExtReport.Checked;
            bool onlyDuty = cbDuty.Checked;

            string er = "";
            string did = ((StringTagItem)cbDocType.SelectedItem).Tag;

            reportTable = null;

            e.canAbort = true;
            e.textMessage = "Выборка из журнала документов";

            string sql = string.Format(
                "select * from `journal` where jdocdate >= '{0}' and jdocdate <= '{1}' and docid = '{2}'",
                    deStart.Value.ToString("yyyyMMdd") + "000000000",
                    deEnd.Value.ToString("yyyyMMdd") + "235959999",
                    ((IDEXData)toolbox).EscapeString(did)
                );

            StringTagItem sti = (StringTagItem)cbUnit.SelectedItem;
            if (!sti.Tag.Equals(StringTagItem.VALUE_ANY))
            {
                sql += string.Format("and unitid = {0} ", sti.Tag);
            }

            if (cbFilter.SelectedIndex > 0)
            {
                sql += string.Format("and status = {0} ", cbFilter.SelectedIndex - 1);
            }

            Dictionary<string, string> pl = new Dictionary<string, string>();
            DataTable tpl = ((IDEXData)toolbox).getTable("um_plans");
            if (tpl != null && tpl.Rows.Count > 0)
            {
                foreach (DataRow r in tpl.Rows)
                {
                    pl[r["plan_id"].ToString()] = r["title"].ToString();
                }
            }

            //Dictionary<string, string> selplans = new Dictionary<string, string>();
            //foreach (StringTagItem sti2 in clbPlans.CheckedItems)
            //{
            //    selplans[sti2.Tag] = "yes";
            //}

            Dictionary<string, string> un = new Dictionary<string, string>();
            DataTable tun = ((IDEXData)toolbox).getTable("units");
            if (tun != null && tun.Rows.Count > 0)
            {
                foreach (DataRow r in tun.Rows)
                {
                     un[r["uid"].ToString()] = r["title"].ToString();
                }
            }


            IDEXData d = (IDEXData)toolbox;

            DataTable jou = d.getQuery(sql);
            if (jou != null && jou.Rows.Count > 0)
            {
                e.textMessage = "Обработка данных";
                e.minValue = 0;
                e.maxValue = jou.Rows.Count;
                e.progressValue = 0;
                e.progressVisible = true;

                DataTable t = new DataTable();
                t.Columns.Add(new DataColumn("date", typeof(string)));
                //t.Columns.Add(new DataColumn("plan", typeof(string)));
                t.Columns.Add(new DataColumn("docnum", typeof(string)));
                //t.Columns.Add(new DataColumn("msisdn", typeof(string)));
                t.Columns.Add(new DataColumn("fio", typeof(string)));
                t.Columns.Add(new DataColumn("unit", typeof(string)));
                //if (extReport)
                //{
                    t.Columns.Add(new DataColumn("icc", typeof(string)));
                //    t.Columns.Add(new DataColumn("balance", typeof(string)));
                //}
                    t.Columns.Add(new DataColumn("region", typeof(string)));

                List<string> regionMsisdn = null;
                sti = (StringTagItem)cbRegion.SelectedItem;
                string region = "";
                if (!sti.Tag.Equals(StringTagItem.VALUE_ANY))
                {
                    regionMsisdn = new List<string>();
                    string sql2 = "select icc from um_data where region_id = '" + d.EscapeString(sti.Tag) + "' and ({0})";
                    string wh = "";
                    int s;
                    region = sti.Text;
                    foreach (DataRow r in jou.Rows)
                    {
                        if (wh.Length > 1000)
                        {
                            //DataTable mst = d.getTable(string.Format(sql2, wh)); //сменил на строку ниже 06.10.2015(Костя)
                            DataTable mst = d.getQuery(string.Format(sql2, wh));
                            if (mst != null)
                            {
                                foreach (DataRow msr in mst.Rows)
                                {
                                    //regionMsisdn.Add(msr["msisdn"].ToString());
                                    regionMsisdn.Add(msr["ICC"].ToString());
                                }
                            }
                            wh = "";
                        }

                        

                        SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                        if ( xml["ICC"].Text == "0204835654" )
                        {
                            string dd = "dfdf";
                        }
                        if (!wh.Equals("")) wh += " or ";
                        //wh += string.Format("msisdn = '{0}'", d.EscapeString(xml["MSISDN"].Text));
                        wh += string.Format("icc = '{0}'", d.EscapeString(xml["ICC"].Text));
                        s = wh.Length;
                    }


                    if ( wh.Length > 0 )
                    {
                        //DataTable mst = d.getTable(string.Format(sql2, wh)); //сменил на строку ниже 06.10.2015(Костя)
                        try
                        {
                            DataTable mst = d.getQuery(string.Format(sql2, wh));
                            if ( mst != null )
                            {
                                foreach ( DataRow msr in mst.Rows )
                                {
                                    //regionMsisdn.Add(msr["msisdn"].ToString());
                                    regionMsisdn.Add(msr["ICC"].ToString());
                                }
                            }
                            wh = "";
                        }
                        catch
                        {
                        }
                    }
                }

                



                int cnt = 0;

                foreach (DataRow r in jou.Rows)
                {
                    try
                    {
                        SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());

                        if ( sti.Tag.Equals(StringTagItem.VALUE_ANY) )
                        {
                            string regionQuery = string.Format("select title from um_regions where region_id = (select region_id from um_data where icc = {0})", xml.GetNodeByPath("icc", true).Text);
                            DataTable regTable = d.getQuery(regionQuery);
                            if ( regTable != null )
                            {
                                region = regTable.Rows[0]["title"].ToString();
                            }
                        }

                        string ttt = xml["icc"].Text;

                        if (
                            //(cbIgnorePlan.Checked || selplans.ContainsKey(xml.GetNodeByPath("Plan", true).Text))
                            //&&
                            //( regionMsisdn == null || regionMsisdn.Contains(xml["MSISDN"].Text) )
                            (regionMsisdn == null || regionMsisdn.Contains(xml["icc"].Text))
                            //13.01.2014
                            &&
                            (!onlyDuty || xml.GetNodeByPath("DutyId", false) != null)
                            //13.01.2014 /
                           )
                        {
                            DataRow nr = t.NewRow();
                            String dt = r["jDocDate"].ToString();
                            nr["date"] = string.Format("{0}.{1}.{2}",
                                dt.Substring(6, 2), dt.Substring(4, 2), dt.Substring(0, 4)
                                );
                            //                                    xml.GetNodeByPath("DocDate", true).Text;
                            if (nr["date"].ToString().Equals("")) nr["date"] = @"-";

                            string plan = xml.GetNodeByPath("Plan", true).Text;
                            if (pl.ContainsKey(plan)) plan = pl[plan];
                            
                            //nr["plan"] = plan;

                            nr["docnum"] = xml.GetNodeByPath("DocNum", true).Text;
                            if (nr["docnum"].ToString().Equals("")) nr["docnum"] = @"-";
                            nr["fio"] = string.Format("{0} {1} {2}",
                                xml.GetNodeByPath("LastName", true).Text,
                                xml.GetNodeByPath("FirstName", true).Text,
                                xml.GetNodeByPath("SecondName", true).Text
                                );

                            //nr["msisdn"] = xml.GetNodeByPath("msisdn", true).Text;
                            nr["icc"] = xml.GetNodeByPath("icc", true).Text;

                            //13.01.2014
//                            nr["unit"] = un[r["unitid"].ToString()];
                            string uname = un[r["unitid"].ToString()];

                            if (xml.GetNodeByPath("DutyId", false) != null)
                            {
                                if (xml["DutyId"].Text != "")
                                {
                                    uname += " (" + un[xml["DutyId"].Text] + ")";
                                }
                            }
                            nr["unit"] = uname;
                            nr["region"] = region;//r["region_id"];
                            //13.01.2014 /

                            /*
                            if (extReport)
                            {
                                string extsql = string.Format("select * from `um_data` where msisdn ='{0}'", 
                                    d.EscapeString(nr["msisdn"].ToString()));
                                using (DataTable dtExt = d.getQuery(extsql))
                                {
                                    if (dtExt != null && dtExt.Rows.Count > 0)
                                    {
                                        nr["icc"] = dtExt.Rows[0]["icc"];
                                        nr["balance"] = dtExt.Rows[0]["balance"];
                                    }
                                    else
                                    {
                                        extsql = string.Format("select * from `um_data_out` where msisdn ='{0}'",
                                            d.EscapeString(nr["msisdn"].ToString()));
                                        using (DataTable dtExt2 = d.getQuery(extsql))
                                        {
                                            if (dtExt2 != null && dtExt2.Rows.Count > 0)
                                            {
                                                nr["icc"] = dtExt2.Rows[0]["icc"];
                                                nr["balance"] = dtExt2.Rows[0]["balance"];
                                            }
                                            else
                                            {
                                                nr["icc"] = "?";
                                                nr["balance"] = "?";
                                            }
                                        }
                                    }
                                }
                            }
                            */

                            t.Rows.Add(nr);
                        }
                       
                    }
                    catch (Exception)
                    {
                        string ss = "";
                    }
                    if ( sti.Tag.Equals(StringTagItem.VALUE_ANY) ) region = "";
                    e.progressValue = cnt++;
                    e.DoEvents();
                }

                if (t != null && t.Rows.Count > 0)
                {
                    reportTable = t;
                }
                else
                {
                    er += "* В БД нет записей по указанным критериям\n";
                }
            }
            else
            {
                er += "* В БД нет записей по указанным критериям\n";
            }

            return er;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

            if (cbDocType.SelectedIndex < 0) er += "* Не указан тип документа для отчёта\n";
            if (!rxdate.IsMatch(deStart.Text)) er += "* Некорректная дата начала интервала\n";
            if (!rxdate.IsMatch(deEnd.Text)) er += "* Некорректная дата конца интервала\n";

            if (er.Equals(""))
            {
                if (deStart.Value > deEnd.Value) er += "* Дата начала интервала позже конечной даты\n";
                //if (!cbIgnorePlan.Checked && clbPlans.CheckedItems.Count < 1) er += "* Не указаны тарифные планы для отчёта\n";
            }

            if (er.Equals(""))
            {
                er = WaitMessage.Execute(new WaitMessageEvent(MakeReport));                

                if (er.Equals("") && reportTable != null && reportTable.Rows.Count > 0)
                {
                    Form frm = new Form();
                    frm.WindowState = FormWindowState.Maximized;


                    ReportViewer rv = new ReportViewer();
                    rv.Dock = DockStyle.Fill;
                    frm.Controls.Add(rv);

                    rv.LocalReport.DataSources.Clear();
                    rv.LocalReport.DataSources.Add(new ReportDataSource("ds_table", reportTable));

                    //string repname = "DEXPlugin.Report.Yota.PeriodReestr.PeriodReestr.rdlc";
                    //if (cbExtReport.Checked) repname = "DEXPlugin.Report.Yota.PeriodReestr.PeriodReestrExt.rdlc";
                    string repname = "DEXPlugin.Report.Yota.PeriodReestr.PeriodReestrExt.rdlc";

                    rv.LocalReport.ReportEmbeddedResource = repname;
                    rv.ProcessingMode = ProcessingMode.Local;

                    List<ReportParameter> paraml = new List<ReportParameter>();
                    paraml.Add(new ReportParameter("doctype", cbDocType.SelectedItem.ToString()));
                    rv.LocalReport.SetParameters(paraml);

                    rv.SetDisplayMode(DisplayMode.Normal);

                    rv.RefreshReport();

                    frm.ShowDialog();

                    DialogResult = DialogResult.OK;
                }
            }

            if (!er.Equals(""))
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
        }

        private void cbIgnorePlan_CheckedChanged(object sender, EventArgs e)
        {
            //clbPlans.Enabled = !cbIgnorePlan.Checked;
        }

    }
}
