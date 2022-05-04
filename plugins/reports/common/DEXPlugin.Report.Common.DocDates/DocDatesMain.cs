using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using Microsoft.Reporting.WinForms;

namespace DEXPlugin.Report.Common.DocDates
{
    public partial class DocDatesMain : Form
    {
        object toolbox;

        DataTable dt = null;
        Dictionary<string, string> dunits;

        public DocDatesMain(object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;

            IDEXData d = (IDEXData)toolbox;

            DataTable t = d.getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, t, "Любое", "uid", "title", false);

            dunits = new Dictionary<string, string>();
            if (t != null && t.Rows.Count > 0)
            {
                foreach (DataRow r in t.Rows)
                {
                    try
                    {
                        dunits[r["uid"].ToString()] = r["title"].ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            IDEXConfig cfg = (IDEXConfig)toolbox;
            StringTagItem.SelectByTag(cbUnit, cfg.getStr(this.Name, "cbUnit", StringTagItem.VALUE_ANY), true);

            bReport.Enabled = false;
            bExportClipboard.Enabled = false;
        }

        public void SaveForm()
        {
            try
            {
                IDEXConfig cfg = (IDEXConfig)toolbox;

                if (cbUnit.SelectedItem != null)
                {
                    cfg.setStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
                }
            }
            catch (Exception)
            {
            }
        }

        public string DoLoadCheckSim(IWaitMessageEventArgs wmea)
        {
            if (Clipboard.GetText().Trim().Equals("")) return "Буфер обмена не содержит данных";
            string[] simdata = Clipboard.GetText().Split(new string[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            if (simdata == null || simdata.Length < 1) return "Буфер обмена не содержит данных";

            dgv.DataSource = null;

            wmea.textMessage = "Получение данных из БД";
            wmea.canAbort = false;
            wmea.progressVisible = false;
            wmea.DoEvents();

            IDEXData d = (IDEXData)toolbox;

//            DataTable criticals = new DataTable();

            DataTable criticals = new DataTable();
            criticals.Columns.Add("signature", typeof(string));
            criticals.Columns.Add("cvalue", typeof(string));
 
            try
            {
//                criticals = d.getQuery("select signature, cvalue from `criticals` where cname = 'MSISDN'");

                DataTable jou = d.getQuery("select signature, data from `journal` where status = 4");
                if (jou != null && jou.Rows.Count > 0)
                {
                    wmea.textMessage = "Подготовка criticals";
                    wmea.canAbort = true;
                    wmea.progressVisible = true;
                    wmea.minValue = 0;
                    wmea.maxValue = jou.Rows.Count;
                    wmea.progressValue = 0;
                    wmea.DoEvents();

                    int cnt2 = 0;

                    foreach (DataRow r in jou.Rows)
                    {
                        SimpleXML dc = SimpleXML.LoadXml(r["data"].ToString());
                        if (dc.GetNodeByPath("msisdn", false) != null)
                        {
                            DataRow nr = criticals.NewRow();
                            nr["signature"] = r["signature"].ToString();
                            nr["cvalue"] = dc["msisdn"].Text;
                            criticals.Rows.Add(nr);
                        }
                        cnt2++;
                        if (cnt2 % 50 == 0)
                        {
                            wmea.progressValue = cnt2;
                            wmea.DoEvents();
                        }
                        if (wmea.isAborted)
                        {
                            return "Операция прервана пользователем";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Ошибка получения данных из БД (criticals)";
            }

            DataTable umdata = null;
            try
            {
                string sql = "select msisdn, date_own, owner_id from `um_data` where status > 0";
                if (cbUnit.SelectedItem != null &&
                    !((StringTagItem)cbUnit.SelectedItem).Tag.Equals(StringTagItem.VALUE_ANY))
                {
                    sql += string.Format(" and owner_id = {0}", ((StringTagItem)cbUnit.SelectedItem).Tag);
                }

                umdata = d.getQuery(sql);
            }
            catch (Exception)
            {
                return "Ошибка получения данных из БД (um_data)";
            }

            wmea.textMessage = "Загрузка данных из буфера обмена";
            wmea.progressVisible = true;
            wmea.minValue = 0;
            wmea.maxValue = simdata.Length;
            wmea.progressValue = 0;
            wmea.progressVisible = true;
            wmea.canAbort = true;
            wmea.DoEvents();

            int cnt = 0;

            dt = new DataTable();
            dt.Columns.Add("msisdn", typeof(string));
            dt.Columns.Add("docnum", typeof(string));
            dt.Columns.Add("unit", typeof(string));
            dt.Columns.Add("date_own", typeof(string));
            dt.Columns.Add("docdate", typeof(string));

            try
            {
                foreach (string s in simdata)
                {
                    DataRow r = dt.NewRow();
                    r["msisdn"] = s;
                    r["unit"] = "-";
                    r["date_own"] = "-";
                    r["docnum"] = "-";
                    r["docdate"] = "-";

                    DataRow[] rr = umdata.Select(string.Format("msisdn = '{0}'", s));
                    if (rr != null && rr.Length > 0)
                    {
                        if (dunits.ContainsKey(rr[0]["owner_id"].ToString()))
                        {
                            r["unit"] = dunits[rr[0]["owner_id"].ToString()];
                        }
                        string dd = rr[0]["date_own"].ToString();
                        try
                        {
                            dd = dd.Substring(6, 2) + "." + dd.Substring(4, 2) + "." + dd.Substring(0, 4);
                        }
                        catch (Exception)
                        {
                            dd = "?";
                        }
                        r["date_own"] = dd;
                    }

                    rr = criticals.Select(string.Format("cvalue = '{0}'", s));
                    if (rr != null && rr.Length > 0)
                    {
                        DataTable dtj = d.getQuery(
                            string.Format("select * from `journal` where signature = '{0}'",
                            rr[0]["signature"].ToString()));
                        if (dtj != null && dtj.Rows.Count > 0)
                        {
                            SimpleXML doc = SimpleXML.LoadXml(dtj.Rows[0]["data"].ToString());
                            if (doc.GetNodeByPath("docnum", false) != null)
                            {
                                r["docnum"] = doc["docnum"].Text;
                            }
                            if (doc.GetNodeByPath("docdate", false) != null)
                            {
//                                r["docdate"] = doc["docdate"].Text;
                                String jd = dtj.Rows[0]["jdocdate"].ToString();
                                r["docdate"] = jd.Substring(6, 2) + "." + jd.Substring(4, 2) + "." + jd.Substring(0, 4);
                            }
                        }
                    }

                    dt.Rows.Add(r);

                    cnt++;
                    if (cnt % 10 == 0)
                    {
                        wmea.progressValue = cnt;
                        wmea.DoEvents();
                    }

                    if (wmea.isAborted)
                    {
                        return "Операция прервана пользователем";
                    }
                }
                dgv.DataSource = dt;
            }
            catch (Exception)
            {
                dt = null;
                return "Ошибка построения отчёта";
            }


            return "";

        }

        private void bLoadCheckSim_Click(object sender, EventArgs e)
        {
            bReport.Enabled = false;
            bExportClipboard.Enabled = false;

            string ret = WaitMessage.Execute(new WaitMessageEvent(DoLoadCheckSim));
            if (ret != "")
            {
                MessageBox.Show(ret);
                return;
            }

            bReport.Enabled = (dt != null && dgv.DataSource == dt);
            bExportClipboard.Enabled = (dt != null && dgv.DataSource == dt);
        }

        private void bExportClipboard_Click(object sender, EventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string csv = "\"MSISDN\";\"№ документа\";\"Отделение\";\"Дата отгрузки\";\"Дата договора\"\n" +
                    CSVParser.tableToString(dt, ";", true);
                Clipboard.SetText(csv);
                MessageBox.Show("Данные выгружены в буфер обмена");
            }
        }

        private void bReport_Click(object sender, EventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                Form frm = new Form();
                frm.WindowState = FormWindowState.Maximized;


                ReportViewer rv = new ReportViewer();
                rv.Dock = DockStyle.Fill;
                frm.Controls.Add(rv);

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("ds_data", dt));

                rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Report.Common.DocDates.DocDates.rdlc";
                rv.ProcessingMode = ProcessingMode.Local;

                List<ReportParameter> paraml = new List<ReportParameter>();
                paraml.Add(new ReportParameter("rstdate", DateTime.Now.ToString("dd.MM.yyyy")));
                rv.LocalReport.SetParameters(paraml);

                rv.SetDisplayMode(DisplayMode.Normal);

                rv.RefreshReport();

                frm.ShowDialog();
            }
        }
    }
}
