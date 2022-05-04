using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using DEXExtendLib;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MySql.Data.MySqlClient;

namespace DEXPlugin.Function.MTS.Autodoc
{
    public partial class AutodocMain : Form
    {
        object toolbox;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };

        DataTable dtSrc;
        /*
        List<string> lNotUsed = null, lUsed = null;
        Dictionary<string, StringList> lData = null;
        */
        // 30.12.13
        const int ANY_UNIT = int.MaxValue;

        DataTable dtUnits;
        Dictionary<int, DealerInfo> dealerInfo = new Dictionary<int, DealerInfo>();

        DealerInfo getDealer(int unitid)
        {
            if (!dealerInfo.ContainsKey(unitid))
            {
                dealerInfo[unitid] = new DealerInfo(unitid);
                if (unitid == ANY_UNIT)
                {
                    dealerInfo[unitid].unitTitle = "Все отделения";
                }
                else
                {
                    DataRow[] rr = dtUnits.Select("uid = " + unitid);
                    if (rr != null && rr.Length > 0)
                    {
                        dealerInfo[unitid].unitTitle = rr[0]["title"].ToString();
                    }
                }
            }
            return dealerInfo[unitid];
        }
        // 30.12.13 /

        // 08.02.14
        DealerInfo getDealerWithMaxOverage()
        {
            DealerInfo ret = null;
            foreach (KeyValuePair<int, DealerInfo> kvp in dealerInfo)
            {
                if (ret == null || kvp.Value.dataOverage > ret.dataOverage) ret = kvp.Value;
            }

            if (ret != null && ret.dataOverage == 0) ret = null;

            return ret;
        }
        // 08.02.14 /

        public AutodocMain(object toolbox)
        {
            this.toolbox = toolbox;
            InitializeComponent();

            dgvSrc.AutoGenerateColumns = false;
            int i = dgvSrc.Columns.Add("msisdn", "MSISDN");
            dgvSrc.Columns[i].DataPropertyName = "msisdn";
            i = dgvSrc.Columns.Add("icc", "ICC");
            dgvSrc.Columns[i].DataPropertyName = "icc";
            i = dgvSrc.Columns.Add("plan", "ТП");
            dgvSrc.Columns[i].DataPropertyName = "plan";
            i = dgvSrc.Columns.Add("AssignedDPCode", "Код точки продаж");
            dgvSrc.Columns[i].DataPropertyName = "AssignedDPCode";
            i = dgvSrc.Columns.Add("unitid", "ID отделения");
            dgvSrc.Columns[i].DataPropertyName = "unitid";
            i = dgvSrc.Columns.Add("date", "Дата активации");
            dgvSrc.Columns[i].DataPropertyName = "date";
            i = dgvSrc.Columns.Add("name", "Ф.И.О абонента");
            dgvSrc.Columns[i].DataPropertyName = "name";
            i = dgvSrc.Columns.Add("lastdate", "Дата последнего документа");
            dgvSrc.Columns[i].DataPropertyName = "lastdate";
            i = dgvSrc.Columns.Add("usedcount", "Кол-во договоров");
            dgvSrc.Columns[i].DataPropertyName = "usedcount";
            // phash
            // data
            i = dgvSrc.Columns.Add("error", "Ошибки");
            dgvSrc.Columns[i].DataPropertyName = "error";

            IDEXConfig cfg = (IDEXConfig)toolbox;
            string sec = this.Name + "_tp1";
            tbSrcFile.Text = cfg.getStr(sec, "tbSrcFile", "");
            cbEnc.SelectedIndex = cfg.getInt(sec, "cbEnc", -1);
            cbSeparator.SelectedIndex = cfg.getInt(sec, "cbSeparator", -1);
            cbQuotes.Checked = cfg.getBool(sec, "cbQuotes", false);
            cbMsisdnSubstr.Checked = cfg.getBool(sec, "cbMsisdnSubstr", false);
            nudMsisdnPos.Value = (decimal)cfg.getInt(sec, "nudMsisdnPos", 1);
            deDocDate.Value = cfg.getDate(sec, "deDocDate", DateTime.Now);
            cbScan.Checked = cfg.getBool(sec, "cbScan", false);
            cbNoPlanError.Checked = cfg.getBool(sec, "cbNoPlanError", false);
            cbLockToUnitId.Checked = cfg.getBool(sec, "cbLockToUnitId", false);
            cbUnitAsSIM.Checked = cfg.getBool(sec, "cbUnitAsSIM", false);
            cbScanMode.SelectedIndex = cfg.getInt(sec, "cbScanMode", 0);
            cbFixedDocDate.Checked = cfg.getBool(sec, "cbFixedDocDate", false);

            //08.02.14
            tbAnyPassUnits.Text = cfg.getStr(sec, "tbAnyPassUnits", "");
            //08.02.14 /

            //30.12.13
            /*DataTable t*/
            dtUnits = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");
            StringTagItem.UpdateCombo(cbUnit, /*t*/ dtUnits, null, "uid", "title", false);

            //30.12.13 /
            StringTagItem.SelectByTag(cbUnit, cfg.getStr(sec, "cbUnit", ""), true);


            bFillTable.Enabled = false;
            bMakeDocs.Enabled = false;
        }

        public void SaveParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            string sec = this.Name + "_tp1";
            cfg.setStr(sec, "tbSrcFile", tbSrcFile.Text);
            cfg.setInt(sec, "cbEnc", cbEnc.SelectedIndex);
            cfg.setInt(sec, "cbSeparator", cbSeparator.SelectedIndex);
            cfg.setBool(sec, "cbQuotes", cbQuotes.Checked);
            cfg.setBool(sec, "cbMsisdnSubstr", cbMsisdnSubstr.Checked);
            cfg.setInt(sec, "nudMsisdnPos", (int)nudMsisdnPos.Value);
            cfg.setDate(sec, "deDocDate", deDocDate.Value);
            cfg.setBool(sec, "cbScan", cbScan.Checked);
            cfg.setBool(sec, "cbNoPlanError", cbNoPlanError.Checked);
            cfg.setBool(sec, "cbLockToUnitId", cbLockToUnitId.Checked);
            cfg.setBool(sec, "cbUnitAsSIM", cbUnitAsSIM.Checked);
            cfg.setInt(sec, "cbScanMode", cbScanMode.SelectedIndex);
            cfg.setBool(sec, "cbFixedDocDate", cbFixedDocDate.Checked);

            //08.02.14
            cfg.setStr(sec, "tbAnyPassUnits", tbAnyPassUnits.Text);
            //08.02.14 /

            try
            {
                cfg.setStr(sec, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
            }
            catch (Exception) { }
        }

        private void dgvSrc_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
            string cv = dgvSrc.Rows[e.RowIndex].Cells["error"].Value.ToString();
            if (cv.Equals(""))
            {
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            }
            else
            {
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(0xff, 0xb0, 0xb0);
                dgvSrc.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(0xff, 0x40, 0x40);
            }
        }

        private void bSelectSrcFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                tbSrcFile.Text = ofd.FileName;
                bLoadFromFile_Click(bLoadFromFile, null);
            }
        }


        DataTable srcdt = null;

        bool CheckMsisdn(string src)
        {
            if (src == null || src.Length < 1 || src.Length > 10) return false;
            foreach (char c in src.ToCharArray())
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        public string UniLoad(IWaitMessageEventArgs wmea)
        {
            wmea.textMessage = "Загрузка данных";
            wmea.canAbort = true;
            wmea.minValue = 0;

            string er = "";
            try
            {
                if (srcdt != null && srcdt.Rows.Count > 0 && srcdt.Columns.Count > 2)
                {
                    int pv = 0;
                    wmea.maxValue = srcdt.Rows.Count;
                    wmea.progressValue = pv;
                    wmea.progressVisible = true;

                    Regex msichk = new Regex(@"^\d{10}$");
                    Regex datechk = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

                    dtSrc = new DataTable();
                    dtSrc.Columns.Add("msisdn", typeof(string));
                    dtSrc.Columns.Add("icc", typeof(string));
                    dtSrc.Columns.Add("plan", typeof(string));
                    dtSrc.Columns.Add("AssignedDPCode", typeof(string));
                    dtSrc.Columns.Add("unitid", typeof(int));
                    dtSrc.Columns.Add("date", typeof(string));
                    dtSrc.Columns.Add("name", typeof(string));
                    dtSrc.Columns.Add("lastdate", typeof(string));
                    dtSrc.Columns.Add("usedcount", typeof(int));
                    dtSrc.Columns.Add("abdata", typeof(string));
                    dtSrc.Columns.Add("phash", typeof(string));

                    dtSrc.Columns.Add("error", typeof(string));

                    int goodCnt = 0;

                    IDEXData d = (IDEXData)toolbox;

                    foreach (DataRow r in srcdt.Rows)
                    {
                        string fer = "";
                        DataRow nr = dtSrc.NewRow();
                        string dpcode = r["field2"].ToString();
                        nr["AssignedDPCode"] = dpcode;
                        if (dpcode.Length < 5) fer += "Некорректный код точки продаж; ";

                        string msi = r["field0"].ToString();
                        if (msi != null && cbMsisdnSubstr.Checked && nudMsisdnPos.Value - 1 < msi.Length)
                        {
                            try
                            {
                                msi = msi.Substring((int)(nudMsisdnPos.Value - 1));
                            }
                            catch (Exception)
                            {
                            }
                        }
                        nr["msisdn"] = msi;
                        nr["date"] = r["field1"].ToString();

                        if (!msichk.IsMatch(msi))
                        {
                            fer += "Некорректный MSISDN; ";
                        }
                        else
                        {
                            DataTable ticc = d.getQuery(
                                string.Format("select icc, status, plan_id, owner_id from `um_data` where msisdn = '{0}'",
                                d.EscapeString(msi))
                                );
                            if (ticc != null && ticc.Rows.Count > 0)
                            {
                                nr["icc"] = ticc.Rows[0]["icc"].ToString();
                                string pl = ticc.Rows[0]["plan_id"].ToString();
                                if (pl.Length < 1)
                                {
                                    if (cbNoPlanError.Checked) fer += "Не присвоен ТП; ";
                                    pl = "-";
                                }
                                nr["plan"] = pl;
                                if (int.Parse(ticc.Rows[0]["status"].ToString()) == 2) fer += "SIM-карта продана; ";
                                else
                                {
                                    if (ticc.Rows[0].IsNull("owner_id") || int.Parse(ticc.Rows[0]["owner_id"].ToString()) < 0)
                                    {
                                        if (cbUnitAsSIM.Checked || cbLockToUnitId.Checked) fer += "SIM-карта не присвоена; ";
                                    }
                                    else
                                    {
                                        nr["unitid"] = int.Parse(ticc.Rows[0]["owner_id"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                fer += "ICC не найден в справочнике; ";
                            }
                        }

                        if (!datechk.IsMatch(r["field1"].ToString())) fer += "Некорректная дата; ";

                        nr["error"] = fer;
                        if (fer.Equals("")) goodCnt++;

                        dtSrc.Rows.Add(nr);

                        wmea.progressValue = pv++;
                        if (pv % 50 == 0) wmea.DoEvents();
                        if (wmea.isAborted)
                        {
                            dtSrc = null;
                            return "Загрузка данных прервана";
                        }
                    }

                    if (goodCnt == 0) er += "В загруженных данных нет ни одной корректной записи\n";
                }
                else
                {
                    er += "Не удалось загрузить данные\n";
                }
            }
            catch (Exception)
            {
                er += "Не удалось загрузить данные\n";
            }

            if (!er.Equals(""))
            {
                dtSrc = null;
            }
            return er;        
        }

        private void bLoadFromFile_Click(object sender, EventArgs e)
        {
            bFillTable.Enabled = false;
            bMakeDocs.Enabled = false;
            dtSrc = null;
            try
            {
                Encoding enc = Encoding.UTF8;
                if (cbEnc.SelectedIndex == 1) enc = Encoding.GetEncoding(1251);
                if (cbEnc.SelectedIndex == 2) enc = Encoding.GetEncoding(866);

                byte[] b = File.ReadAllBytes(tbSrcFile.Text);
                srcdt = CSVParser.stringToTable(enc.GetString(b), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);

                string er = WaitMessage.Execute(new WaitMessageEvent(UniLoad));

                if (!er.Equals(""))
                {
                    MessageBox.Show(er);
                }
            }
            catch (Exception)
            {
            }

            dgvSrc.DataSource = dtSrc;
            bFillTable.Enabled = (dtSrc != null && dtSrc.Rows.Count > 0);
        }

        private void bLoadFromClipboard_Click(object sender, EventArgs e)
        {
            bFillTable.Enabled = false;
            bMakeDocs.Enabled = false;
            try
            {
                srcdt = CSVParser.stringToTable(Clipboard.GetText(), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);
                string er = WaitMessage.Execute(new WaitMessageEvent(UniLoad));

                dgvSrc.DataSource = dtSrc;

                if (!er.Equals(""))
                {
                    MessageBox.Show(er);
                }
            }
            catch (Exception)
            {
            }
            bFillTable.Enabled = (dtSrc != null && dtSrc.Rows.Count > 0);
        }

        public string BuildDocsVsMsisdn(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = false;

            IDEXData d = (IDEXData)toolbox;
            /*
            if (lNotUsed == null || lUsed == null || lData == null)
            {
                try
                {
                    wmea.textMessage = "Построение статистического списка";
                    wmea.progressVisible = true;

                    lNotUsed = new List<string>();
                    lUsed = new List<string>();
                    lData = new Dictionary<string, StringList>();

                    DataTable tb = d.getQuery("SELECT phash, data, lastdate, usedcount " +
                        "FROM `autodoc_people` left join `autodoc_people_usage` using (phash) order by usedcount, lastdate");
                    //lastdate = yyyymmdd

                    if (tb != null && tb.Rows.Count > 0)
                    {
                        wmea.maxValue = tb.Rows.Count;
                        int cnt = 0;
                        foreach (DataRow r in tb.Rows)
                        {
                            string ph = r["phash"].ToString();
                            StringList dd = new StringList(r["data"].ToString());

                            if ((cbScanMode.SelectedIndex == 0 && dd["FizDocScan"].Equals("x", StringComparison.InvariantCultureIgnoreCase)) ||
                                (cbScanMode.SelectedIndex == 1 && !dd["FizDocScan"].Equals("x", StringComparison.InvariantCultureIgnoreCase)))
                            {

                                if (r.IsNull("usedcount"))
                                {
                                    lNotUsed.Add(ph);
                                    dd["#usedcount"] = "0";
                                    dd["#lastdate"] = "-";
                                }
                                else
                                {
                                    lUsed.Add(ph);
                                    try
                                    {
                                        dd["#usedcount"] = r["usedcount"].ToString();
                                        string ld = r["lastdate"].ToString(); //lastdate = yyyymmdd
                                        if (ld.Length >= 8)
                                        {
                                            dd["#lastdate"] = ld.Substring(6, 2) + "." + ld.Substring(4, 2) + "." + ld.Substring(0, 4);
                                            //#lastdate = dd.mm.yyyy
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                lData[ph] = dd;
                            }
                            cnt++;
                            if (cnt % 50 == 0)
                            {
                                wmea.progressValue = cnt;
                                wmea.DoEvents();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    return "Внутренняя ошибка <" + ex.Message + ">";
                }

            }
            */

            // 30.12.13

            // Если "Как в справочнике MSISDN" (unitAsSim) включено:
            // * Если "Абонентские данные должны принадлежать тому же отделению..." (lockToUnitId) включено
            // * * Абонент к карте подбирается согласно принадлежности карты дилеру
            // * ... выключено
            // * * Берётся любой доступный абонент
            // ... выключено
            // * Берётся любой доступный абонент

            dealerInfo.Clear();
            bool lockToUnitId = cbLockToUnitId.Checked;

            if (cbUnit.SelectedIndex == 1)
            {

            }
            else 
            {
                try
                {
                    wmea.textMessage = "Построение статистического списка";
                    wmea.progressVisible = true;

                    string reg = "";
                    string req = "";
                    if (AutodocIgnorRegForm.selectedIdReg.Count > 0) 
                    {
                        string[] nameReg;
                        reg += " where data not regexp '(";
                        foreach ( string soti in AutodocIgnorRegForm.selectedIdReg )
                        {
                            nameReg = soti.Split(',');
                            reg += string.Format("(AddrState=\\w*{0})|", nameReg[1]);
                            //where data not regexp 'AddrState=\\w*коми'
                        }
                        reg = reg.Substring(0, reg.Length - 1);
                        reg += ")'";

                    }
                    req += reg;

                    string women = "";
                    if (cbOnlyWomen.Checked)
                    {
                        if (!req.Equals(""))
                        {
                            women = " AND data LIKE '%Sex=1%'";
                        }
                        else
                        {
                            women = " where data LIKE '%Sex=1%'";
                        }
                    }
                    req += women;
                    string fizDocOrg = "";
                    if (cbOnlyFDOC.Checked)
                    {
                        if (!req.Equals(""))
                        {
                            fizDocOrg = " AND data REGEXP 'FizDocOrgCode=[0-9]'"; // '.*FizDocOrgCode=([0-9]+)-([0-9]+).*'
                        }
                        else
                        {
                            fizDocOrg = " where data REGEXP 'FizDocOrgCode=[0-9]'"; // '.*FizDocOrgCode=([0-9]+)-([0-9]+).*'
                        }  
                    }

                    // '.*FizDocOrgCode=([0-9]+)-([0-9]+).*'
                    req += fizDocOrg;
                    IDEXServices idis = (IDEXServices)toolbox;
                    // проверим полученные данные на терроризм

                    List<string> valueIn = new List<string> { };
                    bool newReq = false;

                    string sql = string.Format("SELECT phash, data, lastdate, usedcount, unitid FROM `autodoc_people` left join `autodoc_people_usage` using (phash) {0} order by usedcount, lastdate", req);

                    DataTable tb = d.getQuery(sql);
                    //DataTable tb = d.getQuery("SELECT phash, data, lastdate, usedcount, unitid " +
                    //    "FROM `autodoc_people` left join `autodoc_people_usage` using (phash) order by usedcount, lastdate");

                    if (cbChTerrorists.Checked == true)
                    {
                        if (tb != null && tb.Rows.Count > 0)
                        {
                            foreach (DataRow r in tb.Rows)
                            {
                                StringList dd = new StringList(r["data"].ToString());
                                JObject packet = new JObject();
                                packet["com"] = "dexdealer.adapters.checkForTerrorists";
                                packet["subcom"] = "checkForTerrorists";
                                packet["client"] = "dexol";
                                packet["data"] = new JObject();
                                packet["data"]["lastname"] = dd["lastname"].ToString();
                                packet["data"]["firstname"] = dd["firstname"].ToString();
                                packet["data"]["secondname"] = dd["secondname"].ToString();
                                packet["data"]["birth"] = dd["birth"].ToString();
                                string packetFromServer = idis.checkForTerrorists(JsonConvert.SerializeObject(packet));
                                JObject jo = JObject.Parse(packetFromServer);
                                try
                                {

                                    foreach (JObject j in jo["data"]["coincidenceWithBirth"])
                                    {
                                        valueIn.Add(r["phash"].ToString());
                                    }
                                    foreach (JObject j in jo["data"]["coincidenceOtherNames"])
                                    {
                                        valueIn.Add(r["phash"].ToString());
                                        //valueInOther.Add(r["phash"].ToString());
                                    }
                                    foreach (JObject j in jo["data"]["coincidenceAll"])
                                    {
                                        valueIn.Add(r["phash"].ToString());
                                        //valueInAll.Add(r["phash"].ToString());
                                    }
                                }
                                catch (Exception)
                                {
                                    //string sss = "";
                                }

                            }
                            if (valueIn.Count > 0) newReq = true;
                            // если есть совпадения, то просто удалим их из базы автодока
                            foreach (string s in valueIn)
                            {
                                sql = string.Format("delete from `autodoc_people` where phash = '{0}'",
                                    d.EscapeString(s));
                                d.runQuery(sql);
                            }

                        }
                        if (newReq)
                        {
                            tb = d.getQuery("SELECT phash, data, lastdate, usedcount, unitid FROM `autodoc_people` left join `autodoc_people_usage` using (phash) {0}{1} order by usedcount, lastdate", reg, women);
                        }
                    }
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        wmea.maxValue = tb.Rows.Count;
                        int cnt = 0;
                        foreach (DataRow r in tb.Rows)
                        {
                            string ph = r["phash"].ToString();
                            StringList dd = new StringList(r["data"].ToString());


                            // здесь делаем проверку на корректность паспорта


                            bool statusPassport = true;
                            //IDEXData d = (IDEXData)toolbox;

                            Regex rgx = new Regex("\\s+");
                            string FizDocSeries = rgx.Replace(dd["FizDocSeries"].ToString(), "");
                            string FizDocNumber = dd["FizDocNumber"].ToString();


                            //IDEXServices idis = (IDEXServices)toolbox;
                            statusPassport = idis.checkPassport(FizDocSeries, FizDocNumber);


                            if (statusPassport)
                            {


                                if ((cbScanMode.SelectedIndex == 0 && dd["FizDocScan"].Equals("x", StringComparison.InvariantCultureIgnoreCase)) ||
                                    (cbScanMode.SelectedIndex == 1 && !dd["FizDocScan"].Equals("x", StringComparison.InvariantCultureIgnoreCase)))
                                {



                                    int unitid = lockToUnitId ? Convert.ToInt32(r["unitid"]) : ANY_UNIT;

                                    DealerInfo di = getDealer(unitid);

                                    if (r.IsNull("usedcount"))
                                    {
                                        di.lNotUsed.Add(ph);
                                        dd["#usedcount"] = "0";
                                        dd["#lastdate"] = "-"; // dd.mm.yyyy
                                    }
                                    else
                                    {
                                        di.lUsed.Add(ph);
                                        try
                                        {
                                            dd["#usedcount"] = r["usedcount"].ToString();
                                            string ld = r["lastdate"].ToString();
                                            if (ld.Length >= 8)
                                            {
                                                // dd.mm.yyyy <- yyyymmdd
                                                dd["#lastdate"] = ld.Substring(6, 2) + "." + ld.Substring(4, 2) + "." + ld.Substring(0, 4);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                                    di.lData[ph] = dd;

                                    cnt++;
                                    if (cnt % 50 == 0)
                                    {
                                        wmea.progressValue = cnt;
                                        wmea.DoEvents();
                                    }

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return "Внутренняя ошибка <" + ex.Message + ">";
                }
            }

            // 30.12.13 /

            try
            {
                // 30.12.13

                /*
                //12.11.12
                //                if (lData.Count < 1) return "Недостаточно данных для сборки документов";
                if (lData.Count < dtSrc.Rows.Count)
                {
                    return string.Format("Недостаточно данных для сборки документов (Нужно ещё {0})", dtSrc.Rows.Count - lData.Count);
                }
                //12.11.12 /
                */

                if (!lockToUnitId)
                {
                    DealerInfo di = getDealer(ANY_UNIT);
                    if (di != null) di.docsCount = dtSrc.Rows.Count;
                }
                else
                {
                    //08.02.14
                    string[] apu = tbAnyPassUnits.Text.Split(',');
                    if (apu != null && apu.Length > 0)
                    {
                        for (int i = 0; i < apu.Length; ++i)
                        {
                            try
                            {
                                getDealer(Convert.ToInt32(apu[i])).anyUnitPass = true;
                            }
                            catch (Exception) { }
                        }
                    }
                    //08.02.14 /

                    foreach (DataRow r in dtSrc.Rows)
                    {
                        try
                        {
                            if ("".Equals(r["error"].ToString()))
                            {
                                int unitid = Convert.ToInt32(r["unitid"]);
                                DealerInfo di = getDealer(unitid);
                                if (di != null) di.docsCount++;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                string sLow = "";
                // 08.02.14
                int commonDocsCount = 0, commonLdataCount = 0;
                // 08.02.14 /
                foreach (KeyValuePair<int, DealerInfo> kvp in dealerInfo)
                {
                    DealerInfo di = kvp.Value;
                    //if (di.lData.Count < di.docsCount) sLow += di.unitTitle + " (" + (di.docsCount - di.lData.Count) + ")\n";
                    // 08.02.14
                    if (di.lData.Count < di.docsCount)
                    {
                        if (di.anyUnitPass) commonDocsCount += (di.docsCount - di.lData.Count);
                        else sLow += di.unitTitle + " (" + (di.docsCount - di.lData.Count) + ")\n";
                    }
                    else
                    {
                        di.dataOverage = di.lData.Count - di.docsCount;
                        commonLdataCount += di.dataOverage;
                    }
                    // 08.02.14 /
                    di.enhk = di.lData.GetEnumerator();
                }

                // 08.02.14
                if (commonLdataCount < commonDocsCount) sLow += "Любых отделений (" + (commonDocsCount - commonLdataCount) + ")\n";
                // 08.02.14 /

                if (sLow != "") return "Недостаточно данных для сборки документов:\n\n" + sLow;
                // 30.12.13 /

                wmea.textMessage = "Сборка документов";
                wmea.maxValue = dtSrc.Rows.Count;
                int dcnt = 0;
                wmea.progressValue = dcnt;
                wmea.progressVisible = true;
                wmea.DoEvents();

//                Dictionary<string, StringList>.Enumerator enhk = lData.GetEnumerator();

                foreach (DataRow r in dtSrc.Rows)
                {
                    if (r["error"].ToString().Equals(""))
                    {
                        // 30.12.13
                        int _unitid = ANY_UNIT;
                        if (lockToUnitId) _unitid = Convert.ToInt32(r["unitid"]);
                        DealerInfo di = getDealer(_unitid);
                        /*
                        if (!di.enhk.MoveNext())
                        {
                            di.enhk = di.lData.GetEnumerator();
                            di.enhk.MoveNext();
                        }
                         */

                        //08.02.14
                        if (!di.noMoreData && !di.enhk.MoveNext()) di.noMoreData = true;

                        if (di.noMoreData)
                        {
                            di = getDealerWithMaxOverage();
                            di.dataOverage--;
                            di.enhk.MoveNext();
                        }
                        //08.02.14

                        KeyValuePair<string, StringList> kvp = di.enhk.Current;
                        // 30.12.13 /

                        StringList abdata = kvp.Value;

                        r["usedcount"] = int.Parse(abdata["#usedcount"]);
                        r["lastdate"] = abdata["#lastdate"]; //lastdate = dd.mm.yyyy
                        r["name"] = string.Format("{0} {1} {2}", abdata["LastName"], abdata["FirstName"], abdata["SecondName"]);
                        r["abdata"] = abdata.SaveToString();
                        r["phash"] = kvp.Key;
                    }

                    dcnt++;
                    if (dcnt % 50 == 0)
                    {
                        wmea.progressValue = dcnt;
                        wmea.DoEvents();
                    }
                }

            }
            catch (Exception)
            {
            }
            return "";
        }



        private void button1_Click(object sender, EventArgs e)
        {
            bMakeDocs.Enabled = false;
            if (dtSrc == null || dtSrc.Rows.Count < 1)
            {
                MessageBox.Show("Нет загруженных данных");
                return;
            }

            string ret = WaitMessage.Execute(new WaitMessageEvent(BuildDocsVsMsisdn));
            bMakeDocs.Enabled = ret.Equals("");
            if (!ret.Equals("")) MessageBox.Show(ret);
        }

        public string MakeDocsDb(IWaitMessageEventArgs wmea)
        {
            wmea.textMessage = "Формирование документов";
            wmea.maxValue = dtSrc.Rows.Count;
            wmea.progressVisible = true;
            int cnt = 0;
            wmea.progressValue = cnt;
            IDEXData d = (IDEXData)toolbox;
            string signaturedate = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            DataTable tDocType = ((IDEXData)toolbox).getTable("mts_doctype");
            DataTable tDocCountry = ((IDEXData)toolbox).getTable("mts_doccountry");
            try
            {
                foreach (DataRow r in dtSrc.Rows)
                {
                    if (r["error"].ToString().Equals(""))
                    {
                        int unitid = -1;
                        try
                        {
                            if (cbUnitAsSIM.Checked) unitid = int.Parse(r["unitid"].ToString());
                            else unitid = int.Parse(((StringTagItem)cbUnit.SelectedItem).Tag.ToString());
                        }
                        catch (Exception) { }


                        StringList abdata = new StringList(r["abdata"].ToString());
                        SimpleXML xml = new SimpleXML("Document");
                        xml.Attributes["ID"] = "DEXPlugin.Document.MTS.Jeans";
                        xml["DocCategory"].Text = abdata["DocCategory"];

                        if (cbFixedDocDate.Checked)
                            xml["DocDate"].Text = deDocDate.Text;
                        else
                            xml["DocDate"].Text = r["date"].ToString();

                        xml["AssignedDPCode"].Text = r["AssignedDPCode"].ToString();

                        string DPCodeKind = "НП";


                        DataTable dt = d.getQuery("select kind from `mts_units_dp` where dpcode = '" + d.EscapeString(r["AssignedDPCode"].ToString()) + "' and uid = " + unitid);
                        if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["kind"]) == 1)
                        {
                            DPCodeKind = "П";
                        }

                        xml["DPCodeKind"].Text = DPCodeKind;

                        xml["Plan"].Text = r["plan"].ToString();
                        xml["ICCCTL"].Text = "0";
                        xml["ICC"].Text = r["icc"].ToString();
                        xml["MSISDN"].Text = r["msisdn"].ToString();
                        xml["CodeWord"].Text = "Нет";
                        xml["Birth"].Text = abdata["Birth"];
                        xml["Sex"].Text = abdata["Sex"];
                        xml["FirstName"].Text = abdata["FirstName"];
                        xml["SecondName"].Text = abdata["SecondName"];
                        xml["LastName"].Text = abdata["LastName"];
                        xml["FizDocDate"].Text = abdata["FizDocDate"];
                        xml["FizDocOrg"].Text = abdata["FizDocOrg"];
                        xml["FizDocOrgCode"].Text = abdata["FizDocOrgCode"];
                        xml["FizDocNumber"].Text = abdata["FizDocNumber"];
                        xml["FizDocSeries"].Text = abdata["FizDocSeries"];
                        xml["FizDocType"].Text = abdata["FizDocType"];

                        foreach (DataRow row in tDocType.Rows)
                        {
                            if (row["doctype_id"].ToString().Equals(abdata["FizDocType"]))
                            {
                                xml["FizDocType"].Attributes["printable"] = row["title"].ToString();
                            }
                        }

                        bool scn = cbScan.Checked || abdata["FizDocScan"].Equals("x", StringComparison.InvariantCultureIgnoreCase);
                        xml["FizDocScan"].Text = scn ? "X" : "-";
                        xml["FizDocCitizen"].Text = abdata["FizDocCountry"];
                        xml["FizDocCountry"].Attributes["tag"] = abdata["FizDocCountry"];
                        foreach (DataRow row in tDocCountry.Rows)
                        {
                            if (row["doccountry_id"].ToString().Equals(abdata["FizDocCountry"]))
                            {
                                xml["FizDocCountry"].Attributes["printable"] = row["title"].ToString();
                                xml["FizDocCitizen"].Text = row["title"].ToString();
                            }
                        }

                       
                        

                        xml["FizBirthPlace"].Text = abdata["FizBirthPlace"];
                        if (xml["FizBirthPlace"].Text.Trim().Equals("")) xml["FizBirthPlace"].Text = abdata["AddrCity"];
                        xml["FizInn"].Text = abdata["FizInn"];
                        xml["AddrStreet"].Text = abdata["AddrStreet"];
                        xml["AddrZip"].Text = abdata["AddrZip"];
                        xml["AddrState"].Text = abdata["AddrState"];
                        xml["AddrCity"].Text = abdata["AddrCity"];
                        xml["AddrApartment"].Text = abdata["AddrApartment"];
                        xml["AddrBuilding"].Text = abdata["AddrBuilding"];
                        xml["AddrHouse"].Text = abdata["AddrHouse"];

                        //13.01.2014
                        xml["DutyId"].Text = r["unitid"].ToString();
                        //13.01.2014 /


                        string digest = string.Format(
                            "{0}, {1}, {2} {3} {4}",
                            xml["MSISDN"].Text, xml["DocDate"].Text, xml["LastName"].Text,
                            xml["FirstName"].Text, xml["SecondName"].Text
                            );

                        SimpleXML sjournal = new SimpleXML("journal");
                        IDEXDocumentJournal jrn = (IDEXDocumentJournal)toolbox;
                        jrn.AddRecord(sjournal, "Документ сформирован функцией формирования группы документов");

                        String ddd = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        
                        string sql = string.Format(
                            "insert into `journal` (locked, locktime, userid, status, signature, jdocdate, " +
                            "unitid, docid, digest, data, journal) values ('', '', '{0}', 0, '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '{6}', '{7}')",
                            d.EscapeString(((IDEXUserData)toolbox).UID), signaturedate + cnt.ToString("D8"),
                            d.EscapeString(ddd), unitid, d.EscapeString(xml.Attributes["ID"]),
                            d.EscapeString(digest), d.EscapeString(SimpleXML.SaveXml(xml)), 
                            d.EscapeString(SimpleXML.SaveXml(sjournal)));

                        d.runQuery(sql);

                        //12.11.12
                        sql = string.Format("delete from `autodoc_people` where phash = '{0}'",
                            d.EscapeString(r["phash"].ToString()));
                        d.runQuery(sql);
                        //12.11.12 /


                        sql = string.Format("delete from `autodoc_people_usage` where phash = '{0}'",
                            d.EscapeString(r["phash"].ToString()));
                        d.runQuery(sql);
                        
                        int ucnt = int.Parse(r["usedcount"].ToString()) + 1;
                        string ldate = r["lastdate"].ToString();
                        if (ldate == null || ldate.Equals("-") || ldate.Length != 10)
                        {
                            ldate = DateTime.Now.ToString("yyyyMMdd");
                        }
                        else
                        {
                            ldate = ldate.Substring(6, 4) + ldate.Substring(3, 2) + ldate.Substring(0, 2);
                        }

                        sql = string.Format(
                            "insert into `autodoc_people_usage` (phash, lastdate, usedcount) values " +
                            "('{0}', '{1}', {2})", d.EscapeString(r["phash"].ToString()),
                            d.EscapeString(ldate), ucnt);
                        d.runQuery(sql);

                    }

                    cnt++;
                    if (cnt % 10 == 0)
                    {
                        wmea.progressValue = cnt;
                        wmea.DoEvents();
                    }
                }
            }
            catch (Exception)
            {
            }

            return "";
        }


        private void bMakeDocs_Click(object sender, EventArgs e)
        {
            if (dtSrc == null || dtSrc.Rows.Count < 1)
            {
                MessageBox.Show("Нет загруженных данных");
                return;
            }

            int good = 0, bad = 0;
            foreach (DataRow r in dtSrc.Rows)
            {
                if (r["error"].ToString().Trim().Equals("")) good++;
                else bad++;
            }

            if (good < 1)
            {
                MessageBox.Show("Нет ни одной записи для формирования документов");
                return;
            }

            if (!deDocDate.Text.Equals(deDocDate.Value.ToString("dd.MM.yyyy")))
            {
                MessageBox.Show("Некорректная дата формирования документов");
                return;
            }

            if (!cbUnitAsSIM.Checked && (cbUnit.SelectedItem == null || cbUnit.SelectedIndex < 0))
            {
                MessageBox.Show("Не указано отделение-владелец SIM-карт");
                return;
            }

            if (bad < 1 || MessageBox.Show(string.Format(
                "Будут сформированы только корректные документы ({0} из {1}).\nПродолжить?",
                good, bad + good), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string ret = WaitMessage.Execute(new WaitMessageEvent(MakeDocsDb));

                if (!ret.Equals("")) MessageBox.Show(ret);
                else DialogResult = DialogResult.OK;
            }
        }

        private void cbUnitAsSIM_CheckedChanged(object sender, EventArgs e)
        {
            cbUnit.Enabled = !cbUnitAsSIM.Checked;
        }

        private void cbFixedDocDate_CheckedChanged(object sender, EventArgs e)
        {
            deDocDate.Enabled = cbFixedDocDate.Checked;
        }

        private void bAnyPassText_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Список ID отделений, для которых формируются договора с паспортами любых других отделений,\n" +
                "если не хватает собственных.\n\n" +
                "Указываются ID отделений списком, чере запятую.");
        }
        private void bMultiUnitsIgnorReg_Click_(object sender, EventArgs e)
        {
            AutodocIgnorRegForm muir = new AutodocIgnorRegForm();
            if ( muir.ShowDialog() == DialogResult.OK )
            {

                //TODO Фильтровать по множественному выбору
                //cbUnit.SelectedIndex = Units.IndexOf(-2);
                //if ( cbFilterImmediate.Checked )
                //    _journal(lastPageNum);
            }
        }

        private void AutodocMain_Load(object sender, EventArgs e)
        {

        }

    }

    class DealerInfo
    {
        //08.02.14
        public bool anyUnitPass = false;
        public bool noMoreData = false;
        public int dataOverage = 0;
        //08.02.14 /

        public int unitid;
        public string unitTitle = "?";
        public int docsCount = 0;
        public List<string> lNotUsed = new List<string>(), lUsed = new List<string>();
        public Dictionary<string, StringList> lData = new Dictionary<string, StringList>();
        public Dictionary<string, StringList>.Enumerator enhk;

        public DealerInfo(int unitid)
        {
            this.unitid = unitid;
        }
    }

}
