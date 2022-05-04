using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DEXExtendLib;


namespace DEXPlugin.Report.Mega.Activation
{
    public partial class ActivationMain : Form
    {
        object toolbox;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };
        BindingSource bs;
        DataTable dt;
        Dictionary<string, string> dDid = new Dictionary<string, string>();

        public ActivationMain(object toolbox)
        {

            InitializeComponent();
            this.toolbox = toolbox;

            cbDocType.Items.Clear();

            cbDocType.Items.Add(new StringTagItem("Любой тип документа", StringTagItem.VALUE_ANY));

            ArrayList dcs = ((IDEXPluginSystemData)toolbox).getPlugins().getDocuments();
            if (dcs != null && dcs.Count > 0)
            {
                foreach (IDEXPluginDocument dci in dcs)
                {
                    cbDocType.Items.Add(new StringTagItem(dci.Title, dci.ID));
                    dDid[dci.ID] = dci.Title;
                }
            }
            
            dt = null;
            bs = new BindingSource();
            bs.DataSource = dt;
            dgv.DataSource = bs;

            IDEXConfig cfg = (IDEXConfig)toolbox;
            tbSrc.Text = cfg.getStr(this.Name, "tbSrc", "");
            cbEnc.SelectedIndex = cfg.getInt(this.Name, "cbEnc", 0);
            cbSeparator.SelectedIndex = cfg.getInt(this.Name, "cbSeparator", 0);
            cbQuotes.Checked = cfg.getBool(this.Name, "cbQuotes", false);
            cbMsisdnSubstr.Checked = cfg.getBool(this.Name, "cbMsisdnSubstr", false);
            nudMsisdnPos.Value = cfg.getInt(this.Name, "nudMsisdnPos", 1);
            StringTagItem.SelectByTag(cbDocType, cfg.getStr(this.Name, "cbDocType", ""), true);
            cbJournalOption.SelectedIndex = cfg.getInt(this.Name, "cbJournalOption", 2);
            RepButtonsStatus();
        }

        public void SaveForm()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setStr(this.Name, "tbSrc", tbSrc.Text);
            cfg.setInt(this.Name, "cbEnc", cbEnc.SelectedIndex);
            cfg.setInt(this.Name, "cbSeparator", cbSeparator.SelectedIndex);
            cfg.setBool(this.Name, "cbQuotes", cbQuotes.Checked);
            cfg.setBool(this.Name, "cbMsisdnSubstr", cbMsisdnSubstr.Checked);
            cfg.setInt(this.Name, "nudMsisdnPos", int.Parse(nudMsisdnPos.Value.ToString()));
            cfg.setInt(this.Name, "cbJournalOption", cbJournalOption.SelectedIndex);
            if (cbDocType.SelectedIndex > -1) cfg.setStr(this.Name, "cbDocType", ((StringTagItem)cbDocType.SelectedItem).Tag);
        }

        void RepButtonsStatus()
        {
            bStartCheck.Enabled = dt != null;
            bSaveToFile.Enabled = dt != null;
            bCopyToClipboard.Enabled = dt != null;
        }

        DataTable srcdt;

        public string FillDtSource(IWaitMessageEventArgs wmea)
        {
            wmea.textMessage = "Загрузка данных";
            wmea.canAbort = true;
            wmea.minValue = 0;
            try
            {
                DataTable t = srcdt;
                if (t != null && t.Columns.Count > 0 && t.Rows.Count > 0)
                {
                    int pv = 0;
                    wmea.maxValue = t.Rows.Count;
                    wmea.progressValue = pv;
                    wmea.progressVisible = true;

                    dt = new DataTable();
                    dt.Columns.Add("owner", typeof(string)).Caption = "Владелец";
                    dt.Columns.Add("date", typeof(string)).Caption = "Дата документа";
                    dt.Columns.Add("d_in", typeof(string)).Caption = "Дата поступления";
                    dt.Columns.Add("d_sold", typeof(string)).Caption = "Дата продажи";
                    dt.Columns.Add("msisdnb", typeof(string)).Caption = "MSISDN";
                    dt.Columns.Add("iccb", typeof(string)).Caption = "ICC";
                    dt.Columns.Add("plan", typeof(string)).Caption = "ТП"; 
                    dt.Columns.Add("typetitle", typeof(string)).Caption = "Тип";
                    dt.Columns.Add("jtype_sim", typeof(string)).Caption = "Карта";
                    dt.Columns.Add("jtype_doc", typeof(string)).Caption = "Документ";
                    dt.Columns.Add("fs", typeof(string)).Caption = "FS";// добавлено
                    dt.Columns.Add("billing", typeof(string)).Caption = "GF";// добавлено
                    dt.Columns.Add("dpCode", typeof(string)).Caption = "Тип точки";
                    dt.Columns.Add("assignedDpCode", typeof(string)).Caption = "Присвоенный код точки";
                    dt.Columns.Add("region", typeof(string)).Caption = "Регион субдилера";
                    if (cbShowBalance.Checked)
                    {
                        dt.Columns.Add("balance", typeof(string)).Caption = "Баланс";
                    }
                   

                    for (int f = 0; f < t.Columns.Count - 1; ++f)
                    {
                        dt.Columns.Add("s" + f.ToString(), typeof(string)).Caption = "Доп.данные" + (f + 1).ToString();
                    }

                    int colcnt = t.Columns.Count;

                    // Загрузка msisdn и доп.полей из пропарсенного csv

                    foreach (DataRow r in t.Rows)
                    {
                        DataRow dtr = dt.NewRow();
                        dtr["owner"] = "";
                        string msisdnb = "";
                        string iccb = "";
                        bool checkByIcc = false;
                        //int sss = r[0].ToString().Length;
                        if (cbSearchByIcc.Checked)
                        {
                            iccb = r[0].ToString();
                            checkByIcc = true;
                        }
                        else
                        {
                            msisdnb = r[0].ToString();
                        }

                        
                        try
                        {
                            if (!checkByIcc)
                            {
                                if (cbMsisdnSubstr.Checked) msisdnb = msisdnb.Substring((int)nudMsisdnPos.Value - 1, 10);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (checkByIcc) dtr["iccb"] = iccb;
                        else dtr["msisdnb"] = msisdnb;
                        
                        for (int f = 0; f < colcnt - 1; ++f)
                        {
                            dtr["s" + f.ToString()] = r[f + 1];
                        }

                        dt.Rows.Add(dtr);
                        wmea.progressValue = pv++;
                        if (pv % 50 == 0) wmea.DoEvents();
                        if (wmea.isAborted) return "Загрузка данных прервана";
                    }

                    return "";
                }
            }
            catch (Exception ex)
            {
                return "Исключение <" + ex.Message + ">";
            }
            return "Не удалось загрузить данные, либо файл не содержит пригодные данные.";        
        }

        private void bSrcFromFile_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbSrc.Text))
            {
                dt = null;
                try
                {
                    Encoding enc = Encoding.UTF8;
                    if (cbEnc.SelectedIndex == 1) enc = Encoding.GetEncoding(1251);
                    if (cbEnc.SelectedIndex == 2) enc = Encoding.GetEncoding(866);
                    
                    byte[] b = File.ReadAllBytes(tbSrc.Text);
                    srcdt = CSVParser.stringToTable(enc.GetString(b), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);

                    string ret = WaitMessage.Execute(new WaitMessageEvent(FillDtSource));
                    srcdt = null;

                    if (!ret.Equals(""))
                    {
                        dt = null;
                        MessageBox.Show(ret);
                    }
                }
                catch (Exception)
                {

                }
                bs.DataSource = dt;

            }
            else
            {
                MessageBox.Show("Файл-источник отсутствует");
            }
            RepButtonsStatus();
        }

        private void bSrcFromBuffer_Click(object sender, EventArgs e)
        {
            dt = null;
            try
            {
                srcdt = CSVParser.stringToTable(Clipboard.GetText(), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);
                string ret = WaitMessage.Execute(new WaitMessageEvent(FillDtSource));
                srcdt = null;
                if (!ret.Equals(""))
                {
                    dt = null;
                    MessageBox.Show(ret);
                }
            }
            catch (Exception)
            {

            }
            bs.DataSource = dt;
            RepButtonsStatus();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                tbSrc.Text = ofd.FileName;
            }
        }

        private void cbMsisdnSubstr_CheckedChanged(object sender, EventArgs e)
        {
            nudMsisdnPos.Enabled = cbMsisdnSubstr.Checked;
        }

        private void bClearTable_Click(object sender, EventArgs e)
        {
            dt = null;
            bs.DataSource = null;
            RepButtonsStatus();
        }

        public string CheckDt(IWaitMessageEventArgs wmea)
        {
            if (dt == null || dt.Rows.Count < 1) return "Не загружены данные из источника";
            if (cbDocType.SelectedIndex < 0) return "Не выбран тип документа";

            int pv;
            wmea.textMessage = "Загрузка данных SIM";
            wmea.canAbort = true;
            try
            {
                IDEXData d = (IDEXData)toolbox;

                Dictionary<string, string> dplans = new Dictionary<string, string>();
                try
                {
                    DataTable dtPlans = d.getQuery("select * from `um_plans`");
                    if (dtPlans != null && dtPlans.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPlans.Rows)
                        {
                            dplans[dr["plan_id"].ToString()] = dr["title"].ToString();
                        }
                    }
                }
                catch (Exception)
                {
                }
                
                string sdocid = ((StringTagItem)cbDocType.SelectedItem).Tag;

                Dictionary<string, SimItem> dsim = new Dictionary<string, SimItem>();

                DataTable u = d.getQuery("select * from `units`");
                Dictionary<int, string> dunits = new Dictionary<int, string>();
                Dictionary<int, string> dUnitsRegion = new Dictionary<int, string>();
                if (u != null && u.Rows.Count > 0)
                {
                    foreach (DataRow row in u.Rows)
                    {
                        dunits[Convert.ToInt32(row["uid"])] = row["title"].ToString();
                        dUnitsRegion[Convert.ToInt32(row["uid"])] = row["region"].ToString();
                    }
                }

                string[] num_data = new string[2] { "um_data", "um_data_out" };
                string[] njtype = new string[2] { "Журнал", "Архив" };

                for (int t = 0; t < 2; ++t)
                {
                    if (t == cbJournalOption.SelectedIndex) continue;

                    string balance = "";
                    if (cbShowBalance.Checked) balance = ", balance ";

                    DataTable q = d.getQuery("select msisdn, status_j, icc, fs, billing, owner_id, plan_id, status, date_in, date_own, date_sold {0} from `{1}` order by date_sold", balance, num_data[t]);

                    if (q != null && q.Rows.Count > 0)
                    {
                        wmea.maxValue = q.Rows.Count * 2;
                        pv = q.Rows.Count * t;
                        wmea.progressValue = pv;
                        wmea.progressVisible = true;

                        foreach (DataRow rq in q.Rows)
                        {
                            //if (rq["msisdn"].ToString() == "9283089409") 
                            //{
                            //    string pp = "as";
                            //}
                            
                            SimItem sitem = null;
                            string msisdn = rq["msisdn"].ToString();

                            string searchString = msisdn;
                            if (cbSearchByIcc.Checked) searchString = rq["icc"].ToString();
                            

                            if (dsim.TryGetValue(searchString, out sitem))
                            //if (dsim.TryGetValue(msisdn, out sitem))
                            { // Если такая запись уже есть - смотрим, нужно ли её замещать новой.

                                //op1 - Из собранной таблицы
                                //op2 - Из "um_data", "um_data_out"
                                string op1 = "-".Equals(sitem.d_sold) ? DateTime.Now.AddDays(1).ToString("yyyyMMdd") : sitem.d_sold;
//                                string op2 = Convert.ToInt32(rq["status"]) < 2 ? DateTime.Now.AddDays(1).ToString("yyyyMMdd") : rq["date_sold"].ToString();
                                string op2 = rq["date_sold"].ToString();
                                if (t == 0)
                                { // Журнал
                                    if (Convert.ToInt32(rq["status"]) < 2) op2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                                }
                                else
                                { // Архив
                                    if (Convert.ToInt32(rq["status"]) < 2)
                                    {
                                        op2 = rq["date_own"].ToString().Trim();
                                        int dd;
                                        if (op2.Length != 8 || !Int32.TryParse(op2, out dd))
                                        {
                                            op2 = rq["date_in"].ToString().Trim();
                                            if (op2.Length != 8 || !Int32.TryParse(op2, out dd))
                                            {
                                                op2 = new DateTime(0L).ToString("yyyyMMdd");
                                            }
                                        }
                                    }
                                }

                                // Если op1 меньше op2
                                if (op1.CompareTo(op2) < 0)
                                {
                                    if (!dunits.TryGetValue(Convert.ToInt32(rq["owner_id"]), out sitem.owner)) sitem.owner = "-";
                                    
                                    if (Convert.ToInt32(rq["status"]) == 2) sitem.d_sold = rq["date_sold"].ToString();
                                    else sitem.d_sold = "-";

                                    sitem.plan = rq["plan_id"].ToString();
                                    sitem.jtype = njtype[t];
                                    sitem.icc = rq["icc"].ToString();
                                    sitem.fs = Convert.ToBoolean(rq["fs"]);
                                    sitem.billing = Convert.ToBoolean(rq["billing"]);
                                    sitem.d_in = rq["date_in"].ToString();
                                 
                                    if (cbShowBalance.Checked) sitem.balance = rq["balance"].ToString();
                         
                                }
                            } 
                            else 
                            { // Если записи нет в dSim - создаём её (msisdn, icc, owner, d_sold, plan_id, "Журнал/Архив")
                                string owner, d_sold;

                                //if (rq["msisdn"].ToString() == "9283089409")
                                //{
                                //    string pp = "as";
                                //}


                                if (!dunits.TryGetValue(Convert.ToInt32(rq["owner_id"]), out owner)) owner = "-";

                                if (Convert.ToInt32(rq["status"]) == 2) d_sold = rq["date_sold"].ToString();
                                else d_sold = "-";

                                string dsimBalance = "";
                                if (cbShowBalance.Checked) dsimBalance = rq["balance"].ToString();

                                string sss = rq["billing"].ToString();


                                if (cbSearchByIcc.Checked)
                                {
                                    dsim.Add(rq["icc"].ToString(), new SimItem(msisdn, rq["icc"].ToString(), Convert.ToBoolean(rq["fs"]), Convert.ToBoolean(rq["billing"]), owner, d_sold, rq["date_in"].ToString(), rq["plan_id"].ToString(), njtype[t], dsimBalance, Int32.Parse(rq["owner_id"].ToString()) ));
                                }
                                else
                                {
                                    dsim.Add(msisdn, new SimItem(msisdn, rq["icc"].ToString(), Convert.ToBoolean(rq["fs"]), Convert.ToBoolean(rq["billing"]), owner, d_sold, rq["date_in"].ToString(), rq["plan_id"].ToString(), njtype[t], dsimBalance, Int32.Parse(rq["owner_id"].ToString()) ));
                                }
                                //dsim.Add(msisdn, new SimItem(msisdn, rq["icc"].ToString(), Convert.ToBoolean(rq["fs"]), Convert.ToBoolean(rq["billing"]), owner, d_sold, rq["date_in"].ToString(), rq["plan_id"].ToString(), njtype[t], dsimBalance));
                            }

                            pv++;
                            if (pv % 50 == 0) wmea.progressValue = pv;
                            if (wmea.isAborted) return "Операция прервана";
                        }
                    }
                }

                wmea.textMessage = "Загрузка документов";
                wmea.progressValue = 0;

                Dictionary<string, DocItem> mda = new Dictionary<string, DocItem>();

                string whdocid = (StringTagItem.VALUE_ANY.Equals(sdocid)) ? "" : " where docid = '" + sdocid + "'";
                string[] njournal = new string[2] { "journal", "archive" };

                for (int t = 1; t >= 0; --t)
                {
                    if (t == cbJournalOption.SelectedIndex) continue;

                    DataTable tb = d.getQuery("select count(id) as cid from `{0}`" + whdocid, njournal[t]);
                    DataTable drd = d.getQuery("select substr(jdocdate, 1, 8) as sdocdate, data from `{0}`" + whdocid, njournal[t]);
                    
                    bool doCount = (tb != null && tb.Rows.Count > 0);
                    int cntval = 0;
                    if (doCount) cntval = int.Parse(tb.Rows[0]["cid"].ToString());
                    wmea.maxValue = cntval * 2;
                    pv = doCount ? cntval * t : 0;
                    wmea.progressValue = doCount ? pv : 0;
                    wmea.progressVisible = doCount;

                    if (drd != null && drd.Rows.Count > 0)
                    {
                        foreach (DataRow drdr in drd.Rows)
                        {

                            SimpleXML xml = SimpleXML.LoadXml(drdr["data"].ToString());
                           
                            if (xml != null && xml.GetNodeByPath("msisdn", false) != null)
                            {
                                string typetitle = "?";
                                // добавлено поле для вывода типа точки
                                string dpCode = "";
                                string assignedDpCode = "";

                                if (xml.Attributes.ContainsKey("ID") && dDid.ContainsKey(xml.Attributes["ID"]))
                                {
                                    typetitle = dDid[xml.Attributes["ID"]];
                                    
                                }                               
                                
                                
                                try
                                {
                                    if (xml["DPCodeKind"] != null) dpCode = xml["DPCodeKind"].Text;
                                    if (xml["AssignedDpCode"] != null) assignedDpCode = xml["AssignedDpCode"].Text;
                                    string msisdn = xml.GetNodeByPath("msisdn", false).Text;
                                    if (cbSearchByIcc.Checked)
                                    {
                                        mda[xml["icc"].Text] = new DocItem(msisdn, xml["icc"].Text, dpCode, assignedDpCode, drdr["sdocdate"].ToString(), typetitle, njtype[t]);
                                    }
                                    else
                                    {
                                        mda[msisdn] = new DocItem(msisdn, xml["icc"].Text, dpCode, assignedDpCode, drdr["sdocdate"].ToString(), typetitle, njtype[t]);
                                    }
                                    //mda[msisdn] = new DocItem(msisdn, xml["icc"].Text, dpCode, assignedDpCode, drdr["sdocdate"].ToString(), typetitle, njtype[t]);
                                }
                                catch (Exception) { }

                            }
                            if (doCount)
                            {
                                pv++;
                                if (pv % 50 == 0) wmea.progressValue = pv;
                            }
                            else
                            {
                                wmea.DoEvents();
                            }

                            if (wmea.isAborted) return "Операция прервана";
                        }
                    }
                }


                wmea.textMessage = "Обработка информации";

                wmea.maxValue = dt.Rows.Count;
                pv = 0;
                foreach (DataRow r in dt.Rows)
                {

                    SimItem simitem;


                    string search = r["msisdnb"].ToString() == "" ? r["iccb"].ToString() : r["msisdnb"].ToString();

                    //if (dsim.TryGetValue(r["msisdnb"].ToString(), out simitem))
                    if (dsim.TryGetValue(search, out simitem))
                    {
                        r["owner"] = simitem.owner;

                        int ssss = dsim.GetHashCode();
                        if (cbSearchByIcc.Checked) r["msisdnb"] = simitem.msisdn;
                       
                   

                        
                        try
                        {
                            string dd = simitem.d_sold;
                            r["d_sold"] = string.Format("{0}.{1}.{2}", dd.Substring(6, 2), dd.Substring(4, 2), dd.Substring(0, 4));
                        }
                        catch (Exception)
                        {
                            r["d_sold"] = "-";
                        }

                        try
                        {
                            string dd = simitem.d_in;
                            r["d_in"] = string.Format("{0}.{1}.{2}", dd.Substring(6, 2), dd.Substring(4, 2), dd.Substring(0, 4));
                        }
                        catch (Exception)
                        {
                            r["d_in"] = "-";
                        }

                        string pplan = simitem.plan.Replace(";", " ").Trim();
                        if (pplan.Equals("")) pplan = "-";
                        dplans.TryGetValue(pplan, out pplan);
                        r["plan"] = pplan;
                        r["jtype_doc"] = simitem.jtype;
                        r["fs"] = simitem.fs ? "ФС" : "МБ";
                        r["billing"] = simitem.billing ? "GF" : "LG";
                        if (cbShowBalance.Checked) r["balance"] = simitem.balance;

                        try
                        {
                            string regionOwner = "-";
                            var pattern = new Regex("[:!@#$%^&*()}{|\":?><\\;'/.,~]");
                            if (!dUnitsRegion.TryGetValue(simitem.owner_id, out regionOwner)) regionOwner = "-";
                            regionOwner = pattern.Replace(regionOwner, "");
                            regionOwner = regionOwner.Trim();
                            r["region"] = regionOwner.Trim('\n');
                        }
                        catch (Exception e) {
                            string s = "";
                        }
                    }
                    
                    DocItem docitem;

                    //if (mda.TryGetValue(r["msisdnb"].ToString(), out docitem))
                    if (mda.TryGetValue(search, out docitem))
                    {
                        if (docitem.icc.CompareTo(simitem.icc) == 0)
                        {
                            try
                            {
                                string dd = docitem.date;
                                r["date"] = string.Format("{0}.{1}.{2}", dd.Substring(6, 2), dd.Substring(4, 2), dd.Substring(0, 4));
                            }
                            catch (Exception)
                            {
                                r["date"] = "-";
                            }
                            r["typetitle"] = docitem.typetitle;
                            r["jtype_sim"] = docitem.jtype;
                            r["dpCode"] = docitem.dpCode;
                            r["assignedDpCode"] = docitem.assignedDpCode;

                            if (r["msisdnb"].ToString().Equals("")) r["msisdnb"] = docitem.msisdn;
                        }
                    }


                    pv++;
                    if (pv % 50 == 0) wmea.progressValue = pv;
                    if (wmea.isAborted) return "Операция прервана";
                }
            }
            catch (Exception ex)
            {
                return "Внутренняя ошибка <" + ex.Message + ">";
            }
            return "";
        }


        private void bStartCheck_Click(object sender, EventArgs e)
        {
            string ret = WaitMessage.Execute(new WaitMessageEvent(CheckDt));
            if (!ret.Equals("")) MessageBox.Show(ret);
        }

        string _genHeaders(DataTable t, string sep, bool q)
        {
            try
            {
                string ret = "";
                for (int f = 0; f < t.Columns.Count; ++f)
                {
                    ret += (ret.Equals("") ? "" : sep) + CSVParser._q(t.Columns[f].Caption, q);
                }
                if (!ret.Equals("")) ret += "\n";
                return ret;
            }
            catch (Exception)
            {
            }
            return "";
        }

        private void bSaveToFile_Click(object sender, EventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    CSVParser.tableToFile(dt, separators[cbSeparator.SelectedIndex], cbQuotes.Checked, sfd.FileName, true);
                    MessageBox.Show("Данные сохранены:\n" + sfd.FileName);
                }
            }
            else
            {
                MessageBox.Show("Нет данных для сохранения");
            }


            /*
            string ret = _genHeaders(dt, separators[cbSeparator.SelectedIndex], cbQuotes.Checked) +
                CSVParser.tableToString(dt, separators[cbSeparator.SelectedIndex], cbQuotes.Checked);
            if (ret != null)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, ret);
                    MessageBox.Show("Данные сохранены:\n" + sfd.FileName);
                }
            }
            else
            {
                MessageBox.Show("Нет данных для сохранения");
            }
             */
        }

        private void bCopyToClipboard_Click(object sender, EventArgs e)
        {
            string ret = _genHeaders(dt, separators[cbSeparator.SelectedIndex], cbQuotes.Checked) +
                CSVParser.tableToString(dt, separators[cbSeparator.SelectedIndex], cbQuotes.Checked);
            if (ret != null)
            {
                Clipboard.SetText(ret);
                MessageBox.Show("Данные скопированы в буфер обмена.");
            }
            else
            {
                MessageBox.Show("Нет данных для сохранения");
            }
        }

    }

    class SimItem
    {
        public string msisdn, icc, owner, d_sold, d_in, plan, jtype, balance;
        public bool fs, billing;
        public int owner_id;
        public SimItem(string msisdn, string icc, bool fs, bool billing, string owner, string d_sold, string d_in, string plan, string jtype, string balance, int owner_id)
        {
            this.msisdn = msisdn;
            this.icc = icc;
            this.fs = fs;
            this.billing = billing;
            this.owner = owner;
            this.d_sold = d_sold;
            this.d_in = d_in;
            this.plan = plan;
            this.jtype = jtype;
            this.balance = balance;
            this.owner_id = owner_id;
        }
    }

    class DocItem
    {
        public string msisdn, icc, dpCode, assignedDpCode, date, typetitle, jtype;
        public DocItem(string msisdn, string icc, string dpCode, string assignedDpCode, string date, string typetitle, string jtype) 
        {
            this.msisdn = msisdn;
            this.icc = icc;
            this.dpCode = dpCode;
            this.assignedDpCode = assignedDpCode;
            this.date = date;
            this.typetitle = typetitle;
            this.jtype = jtype;
        }
    }
}
