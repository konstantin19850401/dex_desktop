using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using DEXExtendLib;

namespace DEXPlugin.Report.Common.Sverka
{
    public partial class SverkaMain : Form
    {
        object toolbox;

        public SverkaMain(object toolbox)
        {
            this.toolbox = toolbox;
            InitializeComponent();

            cbDocType.Items.Add(new StringTagItem("Любой документ", StringTagItem.VALUE_ANY));
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
            cbRepType.SelectedIndex = cfg.getInt(this.Name, "cbRepType", 0);
            StringTagItem.SelectByTag(cbDocType, cfg.getStr(this.Name, "cbDocType", ""), true);
            cbJournalOption.SelectedIndex = cfg.getInt(this.Name, "cbJournalOption", 2);
        }

        public void SaveForm()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "deStart", deStart.Value);
            cfg.setDate(this.Name, "deEnd", deEnd.Value);
            cfg.setInt(this.Name, "cbRepType", cbRepType.SelectedIndex);
            if (cbDocType.SelectedIndex > -1) cfg.setStr(this.Name, "cbDocType", ((StringTagItem)cbDocType.SelectedItem).Tag);
            cfg.setInt(this.Name, "cbJournalOption", cbJournalOption.SelectedIndex);
        }

        DataTable rep;
        DateTime d1, d2;
        int doccnt;

        string MakeSverka(IWaitMessageEventArgs wmea)
        {
            try
            {
                string[] journaln = new string[] { "journal", "archive" };

                string did = ((StringTagItem)cbDocType.SelectedItem).Tag;

                string[] month = new string[] { "","January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                string[] monthRus = new string[] {"", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

                string[] totalMonth = new string[] {};

                int start;// = d1.Month-1;
                int end;// = d2.Month;

                doccnt = 0;
                rep = new DataTable();
                if ( !cbDevidePeriod.Checked )
                {
                    rep.Columns.Add("unitid", typeof(int));
                    rep.Columns.Add("unitname", typeof(string));
                    rep.Columns.Add("region", typeof(string));
                    rep.Columns.Add("plan", typeof(string));
                    rep.Columns.Add("cnt", typeof(int));
                }
                else
                {

                    rep.Columns.Add("unitid", typeof(int));
                    rep.Columns.Add("unitname", typeof(string));
                    rep.Columns.Add("region", typeof(string));
                    rep.Columns.Add("plan", typeof(string));
                    rep.Columns.Add("cnt", typeof(int));
                    rep.Columns.Add("month", typeof(string));
                    
                    /*
                    for ( int i = start; i < d2.Month; i++ )
                    {
                        rep.Columns.Add(month[i], typeof(string));
                    }
                    */

                }
                IDEXData data = (IDEXData)toolbox;

                int acnt = 0;
                //for (int t = 0; t < 2; ++t)
                //{
                //    if (t != cbJournalOption.SelectedIndex)
                //    {
                        if ( !cbDevidePeriod.Checked )
                        {

                            for (int t = 0; t < 2; ++t)
                            {
                                if (t != cbJournalOption.SelectedIndex)
                                {
                                    #region если не выбрано разбитие по месяцам
                                    string sql = string.Format(
                                        "select j.*, u.title as unittitle, u.region as region from `{0}` as j, `units` as u " +
                                        "where j.unitid = u.uid and j.jdocdate >= '{1}' and j.jdocdate <= '{2}'",
                                        journaln[t], d1.ToString("yyyyMMdd000000000"), d2.ToString("yyyyMMdd235959999")
                                        );

                                    if (!StringTagItem.VALUE_ANY.Equals(did))
                                    {
                                        sql += string.Format(" and j.docid = '{0}'", data.EscapeString(did));
                                    }

                                    DataTable d = data.getQuery(sql);

                                    if (d != null && d.Rows.Count > 0)
                                    {
                                        int pw = d.Rows.Count * t;
                                        wmea.minValue = 0;
                                        wmea.maxValue = d.Rows.Count * 2;
                                        wmea.progressValue = pw;
                                        wmea.progressVisible = true;
                                        wmea.textMessage = "Подготовка сверки";
                                        wmea.canAbort = true;

                                        foreach (DataRow r in d.Rows)
                                        {
                                            acnt++;
                                            int uid = int.Parse(r["unitid"].ToString());
                                            string utitle = r["unittitle"].ToString();
                                            string region = r["region"].ToString();
                                            SimpleXML doc = SimpleXML.LoadXml(r["data"].ToString());
                                            string plan = "Неопределенный";
                                            if (cbRepType.SelectedIndex >= 1 && cbRepType.SelectedIndex <= 3)
                                            {
                                                if (cbRepType.SelectedIndex == 1)
                                                {
                                                    if (doc != null && doc.GetNodeByPath("Plan", false) != null)
                                                    {
                                                        plan = doc.GetNodeByPath("Plan", false).Text;
                                                        try
                                                        {
                                                            DataTable d3 = data.getQuery(
                                                                "SELECT up.title as title FROM `um_data` as ud, `um_plans` as up where ud.msisdn = '{0}' and ud.icc = '{1}' and up.plan_id = ud.plan_id",
                                                                data.EscapeString(doc["msisdn"].Text), data.EscapeString(doc["icc"].Text)
                                                                );
                                                            if (d3 != null && d3.Rows.Count > 0) plan = d3.Rows[0]["title"].ToString();
                                                            else
                                                            {
                                                                d3 = data.getQuery(
                                                                    "SELECT up.title as title FROM `um_data_out` as ud, `um_plans` as up where ud.msisdn = '{0}'  and ud.icc = '{1}' and up.plan_id = ud.plan_id",
                                                                    data.EscapeString(doc["msisdn"].Text), data.EscapeString(doc["icc"].Text)
                                                                );
                                                                if (d3 != null && d3.Rows.Count > 0) plan = d3.Rows[0]["title"].ToString();
                                                            }
                                                        }
                                                        catch (Exception) { }
                                                    }
                                                }
                                                else if (cbRepType.SelectedIndex == 2)
                                                {
                                                    if (doc != null && doc.GetNodeByPath("msisdn", false) != null)
                                                    {
                                                        DataTable d3 = data.getQuery(string.Format(
                                                            "select region_id from `um_data` where msisdn = '{0}'",
                                                            data.EscapeString(doc["msisdn"].Text)));

                                                        if (d3 == null || d3.Rows.Count <= 0)
                                                        {
                                                            d3 = data.getQuery(string.Format(
                                                                "select region_id from `um_data_out` where msisdn = '{0}'",
                                                                data.EscapeString(doc["msisdn"].Text)));
                                                        }

                                                        if (d3 != null && d3.Rows.Count > 0)
                                                        {
                                                            plan = d3.Rows[0]["region_id"].ToString();
                                                        }
                                                    }
                                                }
                                                else if (cbRepType.SelectedIndex == 3)
                                                {
                                                    if (doc != null && doc.GetNodeByPath("msisdn", false) != null)
                                                    {
                                                        DataTable d3 = data.getQuery(string.Format(
                                                            "select balance from `um_data` where msisdn = '{0}'",
                                                            data.EscapeString(doc["msisdn"].Text)));

                                                        if (d3 == null || d3.Rows.Count <= 0)
                                                        {
                                                            d3 = data.getQuery(string.Format(
                                                                "select balance from `um_data_out` where msisdn = '{0}'",
                                                                data.EscapeString(doc["msisdn"].Text)));
                                                        }

                                                        if (d3 != null && d3.Rows.Count > 0)
                                                        {
                                                            plan = d3.Rows[0]["balance"].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                plan = "-";
                                            }


                                            DataRow nr = null;

                                            try
                                            {
                                                string prePlan = plan.Replace(@"'", @"");
                                                //string prePlan = plan.Replace(@"'", @"\'");
                                                DataRow[] rs = rep.Select(string.Format(
                                                "unitid = {0} and plan = '{1}'",
                                                uid.ToString(), prePlan
                                                        ));
                                                if (rs != null && rs.Length > 0)
                                                {
                                                    nr = rs[0];
                                                }
                                                else
                                                {
                                                    nr = rep.NewRow();
                                                    nr["unitname"] = utitle;
                                                    nr["region"] = region;
                                                    nr["unitid"] = uid;
                                                    nr["plan"] = plan;
                                                    nr["cnt"] = 0;
                                                    rep.Rows.Add(nr);

                                                }

                                            }
                                            catch (Exception)
                                            {

                                            }

                                            if (nr != null)
                                            {
                                                int ocnt = int.Parse(nr["cnt"].ToString());
                                                ocnt++;
                                                nr["cnt"] = ocnt.ToString();
                                                doccnt++;
                                            }

                                            wmea.progressValue = pw++;
                                            if (wmea.isAborted) return "Формирование отчёта прервано";
                                            wmea.DoEvents();
                                        }


                                        
                                        }

                                    }
                                    #endregion
                                }
                            }

                        if (rep != null && rep.Rows.Count > 0)
                        {
                            Dictionary<string, int> totals = new Dictionary<string, int>();
                            foreach (DataRow r in rep.Rows)
                            {
                                string uid = r["unitname"].ToString();
                                int cnt = int.Parse(r["cnt"].ToString());
                                if (totals.ContainsKey(uid))
                                {
                                    totals[uid] += cnt;
                                }
                                else
                                {
                                    totals[uid] = cnt;
                                }
                            }


                            foreach (KeyValuePair<string, int> kvp in totals)
                            {
                                DataRow nr = rep.NewRow();
                                nr["unitname"] = kvp.Key;
                                nr["unitid"] = 0;
                                nr["plan"] = "Всего";
                                nr["cnt"] = kvp.Value;
                                rep.Rows.Add(nr);
                            }

                        }
                        else 
                        {
                            for (int t = 0; t < 2; ++t)
                            {
                                if (t != cbJournalOption.SelectedIndex)
                                {
                                    #region если выбрано разбитие по месяцам
                                    DataRow nr = null;
                                    /*
                                    if ( d1.Year != d2.Year )
                                    {
                                        return "Выбранный период не корректен. Год должен быть одинаковым для начала и конца периода!";
                                    }
                                    else
                                    {
                                       */
                                    string yearStart = d1.Year.ToString();
                                    string yearEnd = d2.Year.ToString();
                                    start = d1.Month;
                                    string dayStart = d1.Day < 10 ? ("0" + d1.Day.ToString()) : d1.Day.ToString();

                                    // сколько пунктов считать
                                    int maxValue = 0;
                                    int startProgress = start;
                                    for (int k = int.Parse(yearStart); k <= int.Parse(yearEnd); k++)
                                    {
                                        end = d2.Month;
                                        if (k < d2.Year)
                                        {
                                            end = 12;
                                        }
                                        for (int i = startProgress; i <= end; i++)
                                        {
                                            maxValue++;
                                        }
                                        startProgress = 1;
                                    }

                                    wmea.minValue = 0;
                                    wmea.maxValue = maxValue;

                                    wmea.progressVisible = true;
                                    wmea.textMessage = "Подготовка сверки";
                                    wmea.canAbort = true;
                                    int currentProgress = 0;

                                    for (int k = int.Parse(yearStart); k <= int.Parse(yearEnd); k++)
                                    {
                                        end = d2.Month;
                                        if (k < d2.Year)
                                        {
                                            end = 12;
                                        }

                                        for (int l = start; l <= end; l++)
                                        {
                                            Array.Resize(ref totalMonth, totalMonth.Length + 1);
                                            totalMonth[totalMonth.Length - 1] = monthRus + " " + k;
                                            try
                                            {
                                                rep.Columns.Add(monthRus[l] + " " + k, typeof(string));
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }




                                        for (int i = start; i <= end; i++)
                                        {
                                            currentProgress++;
                                            wmea.progressValue = currentProgress;
                                            string dayEnd = DateTime.DaysInMonth(k, i).ToString();

                                            string monthDate = i < 10 ? ("0" + (i).ToString()) : (i).ToString();
                                            DateTime dateStart = DateTime.ParseExact(k + monthDate + dayStart, "yyyyMMdd", null);
                                            DateTime dateEnd = DateTime.ParseExact(k + monthDate + dayEnd, "yyyyMMdd", null);

                                            string sql = string.Format(
                                                "select j.*, u.title as unittitle from `{0}` as j, `units` as u " +
                                                "where j.unitid = u.uid and j.jdocdate >= '{1}' and j.jdocdate <= '{2}'",
                                                journaln[t], dateStart.ToString("yyyyMMdd000000000"), dateEnd.ToString("yyyyMMdd235959999")
                                                );

                                            if (!StringTagItem.VALUE_ANY.Equals(did))
                                            {
                                                sql += string.Format(" and j.docid = '{0}'", data.EscapeString(did));
                                            }

                                            DataTable d = data.getQuery(sql);
                                            if (d != null && d.Rows.Count > 0)
                                            {
                                                /*
                                                int pw = d.Rows.Count * t;
                                                wmea.minValue = 0;
                                                wmea.maxValue = d.Rows.Count * 2;
                                                wmea.progressValue = pw;
                                                wmea.progressVisible = true;
                                                wmea.textMessage = "Подготовка сверки";
                                                wmea.canAbort = true;
                                                */
                                                foreach (DataRow r in d.Rows)
                                                {
                                                    acnt++;
                                                    int uid = int.Parse(r["unitid"].ToString());
                                                    string utitle = r["unittitle"].ToString();
                                                    SimpleXML doc = SimpleXML.LoadXml(r["data"].ToString());
                                                    string plan = "Всего";
                                                    nr = null;
                                                    DataRow[] rs = rep.Select(string.Format(
                                                        "unitid = {0} and plan = '{1}'",
                                                        uid.ToString(), plan.Replace(@"'", @"\'")
                                                            ));
                                                    if (rs != null && rs.Length > 0)
                                                    {
                                                        nr = rs[0];
                                                    }
                                                    else
                                                    {
                                                        nr = rep.NewRow();
                                                        nr["unitname"] = utitle;
                                                        nr["unitid"] = uid;
                                                        nr["plan"] = monthRus[i] + " " + k;
                                                        nr["cnt"] = 0;
                                                        nr["month"] = monthRus[i] + " " + k;
                                                        rep.Rows.Add(nr);
                                                    }

                                                    if (nr != null)
                                                    {
                                                        int ocnt = int.Parse(nr["cnt"].ToString());
                                                        ocnt++;
                                                        nr["cnt"] = ocnt.ToString();
                                                        nr[monthRus[i] + " " + k] = ocnt.ToString();
                                                        doccnt++;
                                                    }
                                                    //wmea.progressValue = pw++;
                                                    if (wmea.isAborted)
                                                        return "Формирование отчёта прервано";
                                                    wmea.DoEvents();
                                                }
                                            }


                                            if (rep != null && rep.Rows.Count > 0)
                                            {
                                                Dictionary<string, int> totals = new Dictionary<string, int>();

                                                foreach (DataRow r in rep.Rows)
                                                {
                                                    string uid = r["unitname"].ToString();
                                                    int cnt = int.Parse(r["cnt"].ToString());
                                                    try
                                                    {
                                                        int s = 0;
                                                        for (int j = start; j <= i; j++)
                                                        {
                                                            try
                                                            {
                                                                s += int.Parse(r[monthRus[j] + " " + i].ToString());
                                                            }
                                                            catch
                                                            {
                                                                s += 0;
                                                            }

                                                        }
                                                        r[monthRus[i] + " " + k] = int.Parse(monthRus[i] + " " + k) - s;
                                                        r["всего"] = int.Parse(r[monthRus[i] + " " + k].ToString()) - s;
                                                    }
                                                    catch
                                                    {
                                                    }

                                                    if (totals.ContainsKey(uid))
                                                    {
                                                        totals[uid] += cnt;
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            totals[uid] = cnt;
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }

                                                /*
                                                foreach ( KeyValuePair<string, int> kvp in totals )
                                                {
                                                    DataRow rs = rep.NewRow();
                                                    rs["unitname"] = kvp.Key;
                                                    rs["unitid"] = 0;
                                                    rs["plan"] = "Всего";
                                                    rs["cnt"] = kvp.Value;
                                                    rep.Rows.Add(rs);
                                                }
                                                */
                                            }
                                            dayStart = "01";
                                        }
                                        start = 1;
                                    }
                                    #endregion
                                }
                            }
                        }


                        if (cbShowZero.Checked) 
                        {
                            string sql = string.Format(
                                        "select uid, title from units");
                            DataTable d = data.getQuery(sql);
                            Dictionary<string, string> dd = new Dictionary<string, string>();
                            if (d != null && d.Rows.Count > 0)
                            {
                                foreach (DataRow dr in d.Rows)
                                {
                                    bool ifIsset = false;
                                    foreach (DataRow dr1 in rep.Rows)
                                    {
                                        if (dr1["unitid"].ToString().Equals(dr["uid"].ToString()))
                                        {
                                            dd.Add(dr1["unitid"].ToString(), dr1["unitname"].ToString());
                                            ifIsset = true;
                                            break;
                                        }
                                    }
                                    if (!ifIsset)
                                    {
                                        DataRow nr = rep.NewRow();
                                        nr["unitname"] = dr["title"];
                                        nr["unitid"] = dr["uid"];
                                        nr["plan"] = "Всего";
                                        nr["cnt"] = 0;
                                        rep.Rows.Add(nr);
                                    }
                                    //if (rep[""])
                                }
                            }
                        }
                    //}
                //}

                if (acnt == 0) return "Нет данных для формирования отчёта";

                return "";
            }
            catch (Exception)
            {
                return "Ошибка построения отчёта\n";
            }
        }

        private void bReport_Click(object sender, EventArgs e)
        {
            string er = "";
            d1 = deStart.Value.Date;
            d2 = deEnd.Value.Date;

            
            if (!deStart.Text.Equals(d1.ToString("dd.MM.yyyy"))) er += "* Не указана начальная дата\n";
            if (!deEnd.Text.Equals(d2.ToString("dd.MM.yyyy"))) er += "* Не указана конечная дата\n";
            if (d1 > d2) er += "* Дата начала позже даты конца\n";
            if (cbDocType.SelectedIndex < 0) er += "* Не указан тип документа для отчёта\n";                

            if (er.Equals(""))
            {

                er = WaitMessage.Execute(MakeSverka);
                if (er.Equals(""))
                {
                    Form frm = new Form();
                    frm.WindowState = FormWindowState.Maximized;

                    ReportViewer rv = new ReportViewer();
                    rv.Dock = DockStyle.Fill;
                    frm.Controls.Add(rv);

                    rv.LocalReport.DataSources.Clear();
                    rv.LocalReport.DataSources.Add(new ReportDataSource("ds_sverka", rep));

                    if ( !cbDevidePeriod.Checked )
                    {
                        rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Report.Common.Sverka.Sverka.rdlc";
                    }
                    else
                    {
                        //rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Report.Common.Sverka.SverkaDevide.rdlc";
                        rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Report.Common.Sverka.SverkaDevideTest.rdlc";
                    }
                   
                    rv.ProcessingMode = ProcessingMode.Local;

                    List<ReportParameter> paraml = new List<ReportParameter>();
                    paraml.Add(new ReportParameter("dstart", d1.ToString("dd.MM.yyyy")));
                    paraml.Add(new ReportParameter("dend", d2.ToString("dd.MM.yyyy")));
                    paraml.Add(new ReportParameter("doccnt", doccnt.ToString()));
                    rv.LocalReport.SetParameters(paraml);

                    

                    rv.SetDisplayMode(DisplayMode.Normal);
//                    rv.ZoomMode = ZoomMode.PageWidth;

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

        private void cbDevidePeriod_CheckedChanged(object sender, EventArgs e)
        {
            if ( cbDevidePeriod.Checked )
            {
                cbRepType.SelectedIndex = 0;
                cbRepType.Enabled = false;
            }
            else
            {
                cbRepType.Enabled = true;
            }
        }
            
    }
}
