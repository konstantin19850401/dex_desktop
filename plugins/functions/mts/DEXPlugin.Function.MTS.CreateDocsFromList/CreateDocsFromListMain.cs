using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Text.RegularExpressions;

namespace DEXPlugin.Function.MTS.CreateDocsFromList
{
    public partial class CreateDocsFromListMain : Form
    {
/*
        public const int DOCUMENT_NONE = -1;
        public const int DOCUMENT_DRAFT = 0;
        public const int DOCUMENT_UNAPPROVED = 1;
        public const int DOCUMENT_APPROVED = 2;
        public const int DOCUMENT_TOEXPORT = 3;
        public const int DOCUMENT_EXPORTED = 4;
        public const int DOCUMENT_RETURNED = 5;
        public const int DOCUMENT_EXPORTING = 6;
        public const int DOCUMENT_TODELETE = 100;

        public static string[] DOCUMENT_STATE_TEXT = { "Черновик", "На подтверждение", "Подтверждён", "На отправку", "Отправлен", "Возвращён", "Отправляется" };
*/
        object toolbox;
        DataTable dtUnits;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };
        DataTable srcdt = null;
        DataTable dtSrc;

        public CreateDocsFromListMain(object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;

            dgvSrc.AutoGenerateColumns = false;

            int i = dgvSrc.Columns.Add("msisdn", "MSISDN");
            dgvSrc.Columns[i].DataPropertyName = "msisdn";
            
            //i = dgvSrc.Columns.Add("icc", "ICC");
            //dgvSrc.Columns[i].DataPropertyName = "icc";

            //i = dgvSrc.Columns.Add("plan", "Код тарифный план");
            //dgvSrc.Columns[i].DataPropertyName = "plan";

            i = dgvSrc.Columns.Add("docDate", "Дата договора");
            dgvSrc.Columns[i].DataPropertyName = "docDate";

            i = dgvSrc.Columns.Add("dealerCode", "Код точки");
            dgvSrc.Columns[i].DataPropertyName = "AssignedDPCode";

            i = dgvSrc.Columns.Add("icc", "ICC");
            dgvSrc.Columns[i].DataPropertyName = "icc";

            i = dgvSrc.Columns.Add("description", "Описание состояния");
            dgvSrc.Columns[i].DataPropertyName = "description";

            dtUnits = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");
            StringTagItem.UpdateCombo(cbUnit, /*t*/ dtUnits, null, "uid", "title", false);

            cbEnc.SelectedIndex = 1;
            cbSeparator.SelectedIndex = 0;
            cbStatusDoc.SelectedIndex = 4;
            cbDeviceType.SelectedIndex = 0;

        }

        private void cbUnitAsSIM_CheckedChanged(object sender, EventArgs e)
        {
            cbUnit.Enabled = !cbUnitAsSIM.Checked;

        }

        private void cbFixedDocDate_CheckedChanged(object sender, EventArgs e)
        {
            //deDocDate.Enabled = !deDocDate.Enabled;
            //cbDateEquals.Checked = !cbDateEquals.Checked;
        }

        public string UniLoad(IWaitMessageEventArgs wmea)
        {
            string er = "";
            try
            {
                IDEXData d = (IDEXData)toolbox;
                if (srcdt != null && srcdt.Rows.Count > 0 && srcdt.Columns.Count > 1)
                {
                    dtSrc = new DataTable();
                    dtSrc.Columns.Add("msisdn", typeof(string));
                    
                    dtSrc.Columns.Add("docDate", typeof(string));
                    //dtSrc.Columns.Add("plan", typeof(string));
                    dtSrc.Columns.Add("AssignedDPCode", typeof(string));
                    dtSrc.Columns.Add("icc", typeof(string));
                    dtSrc.Columns.Add("description", typeof(string));

                    foreach (DataRow r in srcdt.Rows)
                    {
                        DataRow nr = dtSrc.NewRow();
                        nr["msisdn"] = r["field0"].ToString();

                        DataTable iccUmData;
                        if (cbDeviceType.SelectedIndex == 1) // если спутниковое
                        {
                            iccUmData = d.getQuery("SELECT * FROM um_data WHERE icc = '" + nr["msisdn"] + "'"); ;
                        }
                        else
                        {
                            iccUmData = d.getQuery("SELECT * FROM um_data WHERE msisdn = '" + nr["msisdn"] + "'");
                        }
                        // получим icc из справочника сим
                        string icc = "";
                        
                        if (iccUmData != null && iccUmData.Rows.Count > 0) icc = iccUmData.Rows[0]["icc"].ToString();
                        
                        nr["docDate"] = r["field1"].ToString();
                        nr["AssignedDPCode"] = r["field2"].ToString();
                        nr["icc"] = icc;
                        nr["description"] = "";

                        if (cbDeviceType.SelectedIndex == 0)
                        {
                            DataTable msisdnDt = d.getQuery("SELECT * FROM criticals WHERE cvalue = '" + r["field0"].ToString() + "'");
                            DataTable iccDt = d.getQuery("SELECT * FROM criticals WHERE cvalue = '" + icc + "'");
                            if (msisdnDt != null && iccDt != null)
                            {
                                if (msisdnDt.Rows.Count > 0 && iccDt.Rows.Count > 0)
                                {
                                    nr["description"] = "Присутствует в базе. Договор создан не будет";
                                }
                            }
                        }
                        else
                        {
                            DataTable iccDt = d.getQuery("SELECT * FROM criticals WHERE cvalue = '" + icc + "'");
                            if (iccDt != null && iccDt.Rows.Count > 0)
                            {
                                nr["description"] = "Присутствует в базе. Договор создан не будет";
                            }
                        }

                        DataTable umData = d.getQuery("SELECT * FROM um_data WHERE icc = '" + icc + "'");
                        if (umData != null)
                        {
                            if (umData.Rows.Count > 1)
                            {
                                nr["description"] = "В справочнике сим-карт присутствует более одной записи с этим icc. Договор создан не будет";
                            }
                        }
                        else
                        {
                            nr["description"] = "В справочнике сим-карт отсутствует запись о этой сим-карте. Договор создан не будет";
                        }

                        dtSrc.Rows.Add(nr);
                    }
                }
                
            } catch(Exception e) 
            {
                er = e.ToString();
            }
            return er;
        }

        private void bLoadFromClipboard_Click(object sender, EventArgs e)
        {
            bMakeDocs.Enabled = false;
            dgvSrc.DataSource = null;
            try
            {
                srcdt = CSVParser.stringToTable(Clipboard.GetText(), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);
                
                string er = WaitMessage.Execute(new WaitMessageEvent(UniLoad));

                dgvSrc.DataSource = dtSrc;

                if (!er.Equals(""))
                {
                    MessageBox.Show(er);
                }
                else
                {
                    if (dgvSrc.Rows.Count > 0)
                    {
                        bMakeDocs.Enabled = true;
                    }
                    else
                    {
                        bMakeDocs.Enabled = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bMakeDocs_Click(object sender, EventArgs e)
        {
            Dictionary<string, DataRow> dplans = new Dictionary<string, DataRow>();
            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
            /*
            try
            {
                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery("select * from `beeline_billplans2`");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            dplans[dr["soc"].ToString()] = dr;
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }
            */
            int cnt = 0;
            int cntErrors = 2;
            if (cbDeviceType.SelectedIndex == 0) cntErrors = 1;
            foreach (DataRow dr in dtSrc.Rows)
            {
                if (dr["description"].ToString().Equals(""))
                {
                    cnt++;
                    string unitid = "";
                    IDEXData d = (IDEXData)toolbox;
                    DataTable dt;
                    if (cbUnitAsSIM.Checked)
                    {
                        dt = d.getQuery("select owner_id from `um_data` where icc = '" + dr["icc"].ToString() + "'");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                unitid = dr1["owner_id"].ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        unitid = ((StringTagItem)cbUnit.SelectedItem).Tag;
                    }

                    string signaturedate = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    SimpleXML xml = new SimpleXML("Document");
                    xml.Attributes["ID"] = "DEXPlugin.Document.MTS.Jeans";

                    String ddd = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    if (cbDateEquals.Checked) 
                    {
                        string[] date = dr["docDate"].ToString().Split('.');
                        ddd = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        ddd = date[2] + date[1] + date[0] + ddd.Substring(8, 9);
                    } 
                    


                    xml["DocDate"].Text = dr["docDate"].ToString();
                    //xml["DocNum"].Text = "";
                    if (cbDeviceType.SelectedIndex == 0)
                    {
                        xml["MSISDN"].Text = dr["msisdn"].ToString();
                    }
                    xml["ICC"].Text = dr["icc"].ToString();
                    xml["AssignedDPCode"].Text = dr["AssignedDPCode"].ToString();
                    xml["DocUnit"].Text = unitid;
                    /*
                    string soc = dr["plan"].ToString();
                    if (dplans.ContainsKey(soc))
                    {
                        DataRow bp = dplans[soc];
                        SimpleXML xmlPlan = xml["Plan"];
                        xmlPlan.Text = soc;
                        xmlPlan.Attributes["Accept"] = bp["accept"].ToString();
                        xmlPlan.Attributes["CellnetsId"] = bp["cellnetsid"].ToString();
                        xmlPlan.Attributes["ChannellensId"] = bp["channellensid"].ToString();
                        xmlPlan.Attributes["Code"] = bp["code"].ToString();
                        xmlPlan.Attributes["Enable"] = bp["enable"].ToString();
                        xmlPlan.Attributes["id"] = bp["id"].ToString();
                        xmlPlan.Attributes["Name"] = bp["name"].ToString();
                        xmlPlan.Attributes["PaySystemsId"] = bp["paysystemsid"].ToString();
                        xmlPlan.Attributes["SOC"] = soc;

                        xml["PlanPrn"].Text = bp["name"].ToString();
                    }
                    */


                    int status = cbStatusDoc.SelectedIndex;
                    string digest = "";
                    if (cbDeviceType.SelectedIndex == 0)
                    {
                        digest = string.Format(
                                "{0}, {1}, {2} {3} {4}",
                                xml["MSISDN"].Text, xml["DocDate"].Text, "", "", ""
                                );
                    }
                    else
                    {
                        digest = string.Format(
                                "{0}, {1}, {2} {3} {4}",
                                xml["ICC"].Text, xml["DocDate"].Text, "", "", ""
                                );
                    }

                    SimpleXML sjournal = new SimpleXML("journal");
                    IDEXDocumentJournal jrn = (IDEXDocumentJournal)toolbox;
                    jrn.AddRecord(sjournal, "Документ создан функцией автоматического создания документа из файла");
                    
                    string signature = signaturedate + cnt.ToString("D8");



                    if (status > 0)
                    {
                        int countErr = 0;
                        if (cbDeviceType.SelectedIndex == 0) 
                        {
                            DataTable dtmsisdn = d.getQuery("select * from `criticals` where cname = 'MSISDN' AND svalue = '" + xml["MSISDN"].Text + "' AND signature <> '" + signature + "'");
                            ArrayList err = new ArrayList();
                            
                            if (dtmsisdn != null && dtmsisdn.Rows != null && dtmsisdn.Rows.Count > 0)
                            {
                                countErr++;
                            }
                        }

                        DataTable dticc = d.getQuery("select * from `criticals` where cname = 'ICC' AND svalue = '" + xml["ICC"].Text + "' AND signature <> '" + signature + "'");
                        if (dticc != null && dticc.Rows != null && dticc.Rows.Count > 0)
                        {
                            countErr++;
                        }

                        if (countErr != cntErrors)
                        {
                            string sql = string.Format(
                            "insert into `journal` (locked, locktime, userid, status, signature, jdocdate, " +
                            "unitid, docid, digest, data, journal) values ('', '', '{0}', '" + status + "', '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '{6}', '{7}')",
                            d.EscapeString(((IDEXUserData)toolbox).UID), signature,
                            d.EscapeString(ddd), unitid, d.EscapeString(xml.Attributes["ID"]),
                            d.EscapeString(digest), d.EscapeString(SimpleXML.SaveXml(xml)),
                            d.EscapeString(SimpleXML.SaveXml(sjournal)));

                            d.runQuery(sql);
                            //если статус отправлен, то спишем в справочнике сим
                            if (status == 4)
                            {
                                //string s1 = xml["DocDate"].Text.Substring(6, 4);
                                string date = xml["DocDate"].Text.Substring(6, 4) + "" + xml["DocDate"].Text.Substring(3, 2) + "" + xml["DocDate"].Text.Substring(0, 2);
                                DataTable udata = d.getQuery("select * from `um_data` where icc = '" + xml["ICC"].Text + "'");
                                if (udata != null && udata.Rows.Count == 1)
                                {
                                    if (cbDeviceType.SelectedIndex == 0)
                                    {
                                        string msisdnUdata = udata.Rows[0]["msisdn"].ToString();
                                        if (msisdnUdata.Substring(0, 1).Equals("0"))
                                        {
                                            d.getQuery("UPDATE um_data SET date_sold = '" + date + "', status='2', msisdn='" + xml["MSISDN"].Text + "' WHERE icc='" + xml["ICC"].Text + "'");
                                        }
                                        else
                                        {
                                            d.getQuery("UPDATE um_data SET date_sold = '" + date + "', status='2' WHERE icc='" + xml["ICC"].Text + "'");
                                        }
                                    }
                                    else
                                    {
                                        d.getQuery("UPDATE um_data SET date_sold = '" + date + "', status='2' WHERE icc='" + xml["ICC"].Text + "'");
                                    }
                                }
                            }

                            // добавим в criticals
                            // insert into `criticals` (signature, cname, cvalue) values (@signature, @cname, @cvalue)
                            if (cbDeviceType.SelectedIndex == 0)
                            {
                                sql = string.Format(
                                "insert into `criticals` (signature, cname, cvalue) " +
                                "values ('{0}', '{1}', '{2}')",
                                signature, "MSISDN", xml["MSISDN"].Text);
                                d.runQuery(sql);
                            }
                            sql = string.Format(
                            "insert into `criticals` (signature, cname, cvalue) " +
                            "values ('{0}', '{1}', '{2}')",
                            signature, "ICC", xml["ICC"].Text);
                            d.runQuery(sql);
                        }
                        else
                        {
                            string sql = string.Format(
                            "insert into `journal` (locked, locktime, userid, status, signature, jdocdate, " +
                            "unitid, docid, digest, data, journal) values ('', '', '{0}', '0', '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '{6}', '{7}')",
                            d.EscapeString(((IDEXUserData)toolbox).UID), signature,
                            d.EscapeString(ddd), unitid, d.EscapeString(xml.Attributes["ID"]),
                            d.EscapeString(digest), d.EscapeString(SimpleXML.SaveXml(xml)),
                            d.EscapeString(SimpleXML.SaveXml(sjournal)));

                            d.runQuery(sql);
                        }
                    }
                    else
                    {
                        string sql = string.Format(
                            "insert into `journal` (locked, locktime, userid, status, signature, jdocdate, " +
                            "unitid, docid, digest, data, journal) values ('', '', '{0}', '0', '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '{6}', '{7}')",
                            d.EscapeString(((IDEXUserData)toolbox).UID), signature,
                            d.EscapeString(ddd), unitid, d.EscapeString(xml.Attributes["ID"]),
                            d.EscapeString(digest), d.EscapeString(SimpleXML.SaveXml(xml)),
                            d.EscapeString(SimpleXML.SaveXml(sjournal)));

                        d.runQuery(sql);
                    }
                }
            }
        }

        private void dgvSrc_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            string cv = dgvSrc.Rows[e.RowIndex].Cells["description"].Value.ToString();
            if (cv.Equals(""))
            {
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            }
            else
            {
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(0xff, 0xb0, 0xb0);
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(0xff, 0x40, 0x40);
            }
        }
    }
}
