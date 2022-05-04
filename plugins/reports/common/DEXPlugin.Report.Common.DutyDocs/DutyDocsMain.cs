using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WinForms;

namespace DEXPlugin.Report.Common.DutyDocs
{
    public partial class DutyDocsMain : Form
    {
        Object toolbox;
        DataTable reportTable;

        public DutyDocsMain(Object toolbox)
        {
            this.toolbox = toolbox;
            InitializeComponent();

            IDEXConfig cfg = (IDEXConfig)toolbox;
            deStart.Value = cfg.getDate(this.Name, "deStart", DateTime.Now);
            deEnd.Value = cfg.getDate(this.Name, "deEnd", DateTime.Now);
            DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, t, "Все отделения", "uid", "title", false);

            if (cbUnit.Items.Count > 0)
                StringTagItem.SelectByTag(cbUnit, cfg.getStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.Items[0]).Tag), true);

        }

        public void SaveForm()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "deStart", deStart.Value);
            cfg.setDate(this.Name, "deEnd", deEnd.Value);
            cfg.setStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

            if (!rxdate.IsMatch(deStart.Text)) er += "* Некорректная дата начала интервала\n";
            if (!rxdate.IsMatch(deEnd.Text)) er += "* Некорректная дата конца интервала\n";

            if (er.Equals(""))
            {
                if (deStart.Value > deEnd.Value) er += "* Дата начала интервала позже конечной даты\n";
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

                    string repname = "DEXPlugin.Report.Common.DutyDocs.DutyDocs.rdlc";

                    rv.LocalReport.ReportEmbeddedResource = repname;
                    rv.ProcessingMode = ProcessingMode.Local;

                    
                    List<ReportParameter> paraml = new List<ReportParameter>();
                    paraml.Add(new ReportParameter("datestart", deStart.Text));
                    paraml.Add(new ReportParameter("dateend", deEnd.Text));
                    paraml.Add(new ReportParameter("rep_unit", cbUnit.SelectedItem.ToString()));
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

        public string MakeReport(IWaitMessageEventArgs e)
        {
            string er = "";

            reportTable = null;

            e.canAbort = true;
            e.textMessage = "Выборка из журнала документов";

            string sql = string.Format(
                "select * from `journal` where jdocdate >= '{0}' and jdocdate <= '{1}'",
                    deStart.Value.ToString("yyyyMMdd") + "000000000",
                    deEnd.Value.ToString("yyyyMMdd") + "235959999"
                );

            StringTagItem sti = (StringTagItem)cbUnit.SelectedItem;
            if (!sti.Tag.Equals(StringTagItem.VALUE_ANY))
            {
                sql += string.Format("and unitid = {0} ", sti.Tag);
            }

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
                t.Columns.Add(new DataColumn("msisdn", typeof(string)));
                t.Columns.Add(new DataColumn("icc", typeof(string)));
                t.Columns.Add(new DataColumn("fio", typeof(string)));
                t.Columns.Add(new DataColumn("unit_old", typeof(string)));
                t.Columns.Add(new DataColumn("unit_new", typeof(string)));


                int cnt = 0;

                foreach (DataRow r in jou.Rows)
                {
                    try
                    {
                        SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                        if (xml.GetNodeByPath("DutyId", false) != null)
                        {
                            DataRow nr = t.NewRow();
                            String dt = r["jDocDate"].ToString();
                            nr["date"] = string.Format("{0}.{1}.{2}",
                                dt.Substring(6, 2), dt.Substring(4, 2), dt.Substring(0, 4)
                                );
                            if (nr["date"].ToString().Equals("")) nr["date"] = @"-";

                            nr["fio"] = string.Format("{0} {1} {2}",
                                xml.GetNodeByPath("LastName", true).Text,
                                xml.GetNodeByPath("FirstName", true).Text,
                                xml.GetNodeByPath("SecondName", true).Text
                                );

                            nr["msisdn"] = xml.GetNodeByPath("msisdn", true).Text;
                            nr["icc"] = xml.GetNodeByPath("icc", true).Text;

                            nr["unit_new"] = un[r["unitid"].ToString()];

                            if (xml.GetNodeByPath("DutyId", false) != null)
                            {
                                nr["unit_old"] = un[xml["DutyId"].Text];
                            }

                            t.Rows.Add(nr);

                        }

                    }
                    catch (Exception)
                    {

                    }
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
    }
}
