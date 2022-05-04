using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using DEXExtendLib;

namespace dexol
{
    public partial class DocsReportForm : Form
    {
        public DocsReportForm()
        {
            InitializeComponent();
        }

        string[] sortOption = { "docdate", "status", "docnum", "doctype", "digest" };

        private void bReport_Click(object sender, EventArgs e)
        {
            
            string er = "";
            if (cbSortBy.SelectedItem == null) er += "Не указано поле сортировки";
            if (!deDateStart.Value.ToString("dd.MM.yyyy").Equals(deDateStart.Text)) er += "Некорректная дата начала интервала\n";
            if (!deDateEnd.Value.ToString("dd.MM.yyyy").Equals(deDateEnd.Text)) er += "Некорректная дата конца интервала\n";
            if (er == "")
            {
                string dt1 = deDateStart.Value.ToString("yyyyMMdd");
                string dt2 = deDateEnd.Value.ToString("yyyyMMdd");
                if (dt1.CompareTo(dt2) > 0)
                {
                    string de = dt1;
                    dt1 = dt2;
                    dt2 = de;
                }

                dt1 += "000000000";
                dt2 += "999999999";



                DataTable dt = new DataTable();
                dt.Columns.Add("digest", typeof(string));
                dt.Columns.Add("doctype", typeof(string));
                dt.Columns.Add("docnum", typeof(string));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("docdate", typeof(string));

                string curdb = DexolSession.inst().currentDb;

                string ret = WaitMessage.Execute(new WaitMessageEvent(prepareReport), dt, dt1, dt2);

                DexolSession.inst().currentDb = curdb;

                if (ret == null)
                {

                    dt.DefaultView.Sort = sortOption[cbSortBy.SelectedIndex];

                    Form frm = new Form();
                    frm.WindowState = FormWindowState.Maximized;


                    ReportViewer rv = new ReportViewer();
                    rv.Dock = DockStyle.Fill;
                    frm.Controls.Add(rv);

                    rv.LocalReport.DataSources.Clear();
                    rv.LocalReport.DataSources.Add(new ReportDataSource("RepDataSet_dtDocsReport", dt.DefaultView));

                    rv.LocalReport.ReportEmbeddedResource = "dexol.DocsReport.rdlc";
                    rv.ProcessingMode = ProcessingMode.Local;

                    List<ReportParameter> paraml = new List<ReportParameter>();
                    paraml.Add(new ReportParameter("datebegin", deDateStart.Value.ToString("dd.MM.yyyy")));
                    paraml.Add(new ReportParameter("dateend", deDateEnd.Value.ToString("dd.MM.yyyy")));
                    paraml.Add(new ReportParameter("daterep", DateTime.Now.ToString("dd.MM.yyyy")));
                    paraml.Add(new ReportParameter("unitname", DexolSession.inst().user_name));
                    rv.LocalReport.SetParameters(paraml);

                    rv.SetDisplayMode(DisplayMode.Normal);

                    rv.RefreshReport();

                    frm.ShowDialog();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(ret);
                }
            }
        }

        public string prepareReport(IWaitMessageEventArgs wmea)
        {
            DataTable dt = (DataTable)wmea.args[0];
            string dt1 = (string)wmea.args[1];
            string dt2 = (string)wmea.args[2];
            DexolSession ses = DexolSession.inst();
            DEXToolBox toolbox = DEXToolBox.getToolBox();
            IDEXData d = (IDEXData)toolbox;

            try
            {
                wmea.canAbort = true;
                wmea.progressVisible = false;
                wmea.textMessage = "Получение сведений из БД";
                wmea.DoEvents();

                Dictionary<string, StringDbItem> dblist = ses.dblist();

                int overallCount = 0;
                foreach (KeyValuePair<string, StringDbItem> kvp in dblist)
                {
                    ses.currentDb = kvp.Value.dbname;
                    string docid_in = "";
                    foreach (string docid in kvp.Value.doctypes)
                    {
                        if (!"".Equals(docid_in)) docid_in += ",";
                        docid_in += "'" + d.EscapeString(docid) + "'";
                    }

                    if (!"".Equals(docid_in))
                    {
                        DataTable t = d.getQuery(
                            "select count(id) as cid from `journal` where jdocdate >= '{0}' and jdocdate <= '{1}' and unitid = {2} and docid in ({3})",
                            dt1, dt2, kvp.Value.unit_uid, docid_in);
                        if (t != null && t.Rows.Count > 0) overallCount += Convert.ToInt32(t.Rows[0]["cid"].ToString());
                    }
                    if (wmea.isAborted) return "Операция прервана";
                    wmea.DoEvents();
                }

                if (overallCount <= 0) return "В указанном интервале нет документов";

                wmea.textMessage = "Загрузка данных о документах";
                wmea.maxValue = overallCount;
                wmea.progressValue = 0;
                wmea.progressVisible = true;

                Dictionary<string, string> doctypes = new Dictionary<string,string>();
                foreach(IDEXPluginDocument idpd in toolbox.Plugins.getDocuments()) 
                {
                    doctypes[idpd.ID] = idpd.Title;
                }

                int pv = 0;

                foreach (KeyValuePair<string, StringDbItem> kvp in dblist)
                {
                    ses.currentDb = kvp.Value.dbname;

                    string docid_in = "";
                    foreach (string docid in kvp.Value.doctypes)
                    {
                        if (!"".Equals(docid_in)) docid_in += ",";
                        docid_in += "'" + d.EscapeString(docid) + "'";
                    }

                    if (!"".Equals(docid_in))
                    {
                        DataTable t = d.getQuery(
                            "select count(id) as cid from `journal` where jdocdate >= '{0}' and jdocdate <= '{1}' and unitid = {2} and docid in ({3})",
                            dt1, dt2, kvp.Value.unit_uid, docid_in);
                        if (t != null && t.Rows.Count > 0)
                        {
                            int docsCount = Convert.ToInt32(t.Rows[0]["cid"].ToString());
                            int docsProcessed = 0;
                            while (docsCount > docsProcessed)
                            {
                                t = d.getQuery(
                                    "select status, jdocdate, docid, digest, data from `journal` " +
                                    "where jdocdate >= '{0}' and jdocdate <= '{1}' and unitid = {2} and docid in ({3}) " +
                                    "limit {4}, 1000",
                                    dt1, dt2, kvp.Value.unit_uid, docid_in, docsProcessed);
                                if (t != null && t.Rows.Count > 0)
                                {
                                    docsProcessed += t.Rows.Count;
                                    pv += t.Rows.Count;
                                    foreach (DataRow row in t.Rows)
                                    {
                                        DataRow nr = dt.NewRow();
                                        nr["digest"] = row["digest"];
                                        nr["doctype"] = doctypes.ContainsKey(row["docid"].ToString()) ? doctypes[row["docid"].ToString()] : "?";
                                        try
                                        {
                                            nr["status"] = toolbox.DocumentStatesText[Convert.ToInt32(row["status"].ToString())];
                                        }
                                        catch (Exception)
                                        {
                                            nr["status"] = "?";
                                        }
                                        string dd = row["jdocdate"].ToString();
                                        try
                                        {
                                            nr["docdate"] = dd.Substring(6, 2) + "." + dd.Substring(4, 2) + "." + dd.Substring(0, 4);
                                        }
                                        catch (Exception)
                                        {
                                            nr["docdate"] = "?";
                                        }

                                        try
                                        {
                                            SimpleXML xml = SimpleXML.LoadXml(row["data"].ToString());
                                            nr["docnum"] = xml["DocNum"].Text;
                                        }
                                        catch (Exception)
                                        {
                                            nr["docnum"] = "?";
                                        }

                                        dt.Rows.Add(nr);
                                    }
                                }
                            }
                        }
                    }
                    if (wmea.isAborted) return "Операция прервана";
                    wmea.progressValue = pv;
                }
            }
            catch (Exception)
            {
                return "Ошибка получения выборки из БД";
            }
            return null;
        }

        private void DocsReportForm_Shown(object sender, EventArgs e)
        {
            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            deDateStart.Value = cfg.getDate(this.Name, "deDateStart.Value", DateTime.Now);
            deDateEnd.Value = cfg.getDate(this.Name, "deDateEnd.Value", DateTime.Now);
            cbSortBy.SelectedIndex = cfg.getInt(this.Name, "cbSortBy.SelectedIndex", 0);
        }

        private void DocsReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            cfg.setDate(this.Name, "deDateStart.Value", deDateStart.Value);
            cfg.setDate(this.Name, "deDateEnd.Value", deDateEnd.Value);
            cfg.setInt(this.Name, "cbSortBy.SelectedIndex", cbSortBy.SelectedIndex);
        }
    }
}
