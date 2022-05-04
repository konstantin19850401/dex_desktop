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

using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Function.Mega.Autodoc
{
    public partial class AutodocMain : Form
    {
        object toolbox;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };

        DataTable dtSrc;

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

        public static string sbmsCert = "";
        public static string sbmsServer = "";
        public AutodocMain(object toolbox)
        {
            try
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
                i = dgvSrc.Columns.Add("intplan", "Внутренний ТП");
                dgvSrc.Columns[i].DataPropertyName = "intplan";
                i = dgvSrc.Columns.Add("unitid", "ID отделения");
                dgvSrc.Columns[i].DataPropertyName = "unitid";
                i = dgvSrc.Columns.Add("date", "Дата активации");
                dgvSrc.Columns[i].DataPropertyName = "date";
                i = dgvSrc.Columns.Add("docnum", "№ договора");
                dgvSrc.Columns[i].DataPropertyName = "docnum";
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
                cbLockToUnitId.Checked = cfg.getBool(sec, "cbLockToUnitId", false);
                cbUnitAsSIM.Checked = cfg.getBool(sec, "cbUnitAsSIM", false);
                cbFixedDocDate.Checked = cfg.getBool(sec, "cbFixedDocDate", false);

                //08.02.14
                tbAnyPassUnits.Text = cfg.getStr(sec, "tbAnyPassUnits", "");
                //08.02.14 /


                tbSBMSUser.Text = cfg.getStr(sec, "tbSBMSUser", "");

                string sbms_password = cfg.getStr(sec, "tbSBMSPass", "");
                if ( sbms_password != "" )
                    sbms_password = ( (IDEXCrypt)toolbox ).Decrypt(sbms_password, sec);
                tbSBMSPass.Text = sbms_password;

                //30.12.13
                /*DataTable t*/
                dtUnits = ( (IDEXData)toolbox ).getQuery("select * from `units` where status = 1 order by title");
                StringTagItem.UpdateCombo(cbUnit, /*t*/ dtUnits, null, "uid", "title", false);

                //30.12.13 /
                StringTagItem.SelectByTag(cbUnit, cfg.getStr(sec, "cbUnit", ""), true);

                DataTable dtRegions = ( (IDEXData)toolbox ).getQuery("select * from `um_regions`");
                StringTagItem.UpdateCombo(cbRegions, /*t*/ dtRegions, null, "region_id", "title", false);

                bFillTable.Enabled = false;
                bMakeDocs.Enabled = false;

                //загрузка сертификатов(30.06.2015)
                string str = "";
                loadCertificates(cbCert, str);

                if ( cfg.getStr(sec, "cbCert", "") != null && cfg.getStr(sec, "cbCert", "") != "" )
                {
                    cbCert.SelectedIndex = Convert.ToInt32(cfg.getStr(sec, "cbCert", ""));
                    sbmsCert = "CN=" + cbCert.SelectedItem.ToString();
                }
                if ( cfg.getStr(sec, "cbServer", "") != null && cfg.getStr(sec, "cbServer", "") != "" )
                {
                        cbServer.SelectedIndex = Convert.ToInt32(cfg.getStr(sec, "cbServer", ""));
                        sbmsServer = cbServer.SelectedItem.ToString();
                }
            } catch (Exception) {  }
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
            cfg.setBool(sec, "cbLockToUnitId", cbLockToUnitId.Checked);
            cfg.setBool(sec, "cbUnitAsSIM", cbUnitAsSIM.Checked);
            cfg.setBool(sec, "cbFixedDocDate", cbFixedDocDate.Checked);

            cfg.setStr(sec, "cbCert", cbCert.SelectedIndex.ToString());
            cfg.setStr(sec, "cbServer", cbServer.SelectedIndex.ToString());


            //08.02.14
            cfg.setStr(sec, "tbAnyPassUnits", tbAnyPassUnits.Text);
            //08.02.14 /
            
            try
            {
                cfg.setStr(sec, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
            }
            catch (Exception) { }

            cfg.setStr(sec, "tbSBMSUser", tbSBMSUser.Text);
            string sbms_password = "".Equals(tbSBMSPass.Text) ? "" : ((IDEXCrypt)toolbox).Encrypt(tbSBMSPass.Text, sec);
            cfg.setStr(sec, "tbSBMSPass", sbms_password);
            
        }
        public void loadCertificates(ComboBox cb, string seltp)
        {
            cb.Items.Clear();
            cb.Items.Add(new StringTagItem("Сертификат не выбран...", StringTagItem.VALUE_ANY));
            StringTagItem.SelectByTag(cb, StringTagItem.VALUE_ANY, false);

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                for (int i = 0; i < store.Certificates.Count; ++i)
                {
                    X509Certificate2 cert = store.Certificates[i];
                    //cert.Subject = "CN=Grishunina Marina Gennadevna, OID.0.9.2342.19200300.100.1.1=-303151, O=KChR OOO N-Telekom, S=CHR, C=RU"
                    string[] sfields = cert.Subject.Split(',');
                    string cn = cert.Subject;
                    foreach (string s in sfields)
                    {
                        if (s.StartsWith("CN="))
                        {
                            cn = s.Substring(3);
                            break;
                        }
                    }

                    //StringTagItem sti = new StringTagItem(
                    //    cn + " (С " + cert.NotBefore.ToString("dd.MM.yyyy") + " по " + cert.NotAfter.ToString("dd.MM.yyyy") + ")",
                    //    cert.Thumbprint
                    //    );
                    //cb.Items.Add(sti);
                    cb.Items.Add(cn);
                }
            }
            catch (Exception) { }
            StringTagItem.SelectByTag(cb, seltp, false);
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
                if (srcdt != null && srcdt.Rows.Count > 0 && srcdt.Columns.Count > 1)
                {
                    int pv = 0;
                    wmea.maxValue = srcdt.Rows.Count;
                    wmea.progressValue = pv;
                    wmea.progressVisible = true;

                    Regex msichk = new Regex(@"^\d{10}$");
                    Regex datechk = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

                    bool getDocNum = srcdt.Columns.Count > 2;
                    dtSrc = new DataTable();
                    dtSrc.Columns.Add("msisdn", typeof(string));
                    dtSrc.Columns.Add("icc", typeof(string));
                    dtSrc.Columns.Add("plan", typeof(string));
                    dtSrc.Columns.Add("intplan", typeof(string));
                    dtSrc.Columns.Add("docnum", typeof(string));
                    dtSrc.Columns.Add("unitid", typeof(int));
                    dtSrc.Columns.Add("date", typeof(string));
                    if (getDocNum) dtSrc.Columns.Add("docnum", typeof(string));
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
                        if (getDocNum) nr["docnum"] = r["field2"].ToString();

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
                                nr["plan"] = "";
                                nr["intplan"] = ticc.Rows[0]["plan_id"].ToString();

                                DataTable tintplan = d.getQuery("select title from `um_plans` where plan_id = '{0}'", ticc.Rows[0]["plan_id"].ToString());
                                if (tintplan != null && tintplan.Rows.Count > 0)
                                {
                                    nr["intplan"] = tintplan.Rows[0]["title"].ToString();
                                }

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

        class Json
        {
            public string series;
            public string number;
        }
        public string BuildDocsVsMsisdn(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = false;

            IDEXData d = (IDEXData)toolbox;

            /*
            SbmsSession sbms = new SbmsSession();
            try
            {
                if (!sbms.connect(tbSBMSUser.Text, tbSBMSPass.Text, sbmsCert, sbmsServer)) throw new Exception();
            }
            catch (Exception) 
            {
                return "Не удалось связаться с СБМС.\n\n(Проверить пользователя и пароль к СБМС в настройках документа Мегафон ЕФД)";
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

            try
            {
                wmea.textMessage = "Построение статистического списка";
                wmea.progressVisible = true;

                // тут определим, брать ли всех или только конкретное отделение
                string lockUnitId = "";
                /*
                if (lockToUnitId && !cbUnitAsSIM.Checked)
                {
                    try
                    {
                        lockUnitId = " unitid = '" + ((StringTagItem)cbUnit.SelectedItem).Tag + "' AND";
                    }
                    catch (Exception) { }
                }

                */
                string req = "";
                if (lockToUnitId)
                {
                    try
                    {
                        // Если абон даннные должны быть для того же отделения что и у сим, то пройдемся по списку сим и получим отделения для них
                        List<string> aa = new List<string> { };
                        foreach (DataRow r in dtSrc.Rows)
                        {
                            if (!aa.Contains(r["unitid"].ToString())) aa.Add(r["unitid"].ToString());
                        }

                        if (aa.Count > 0)
                        {
                            //lockUnitId = " where unitid = '" + ((StringTagItem)cbUnit.SelectedItem).Tag + "'";
                            
                            req += " unitid IN (" + String.Join(",", aa.ToArray()) + ")";
                        }
                    }
                    catch (Exception) { }
                }
               
                string reg = ( (StringTagItem)cbRegions.SelectedItem ).Tag;
                if (req.Equals("")) req += " data LIKE '%"+reg+"%'";
                else req += " AND data LIKE '%" + reg + "%'";

                string women = "";
                if (cbOnlyWomen.Checked) { 
                    if (req.Equals("")) req += " data LIKE '%Sex=2%'";
                    else req += " AND data LIKE '%Sex=2%'";
                }

                string onlyForeigner = "";
                if (cbOnlyForeigner.Checked)
                {
                    string dateOneYearAgo = "";
                    DateTime currentDate = DateTime.Now;
                    int oneYearAgo = currentDate.Year - 1;
                    dateOneYearAgo = new DateTime(oneYearAgo, currentDate.Month, 1).ToString("yyyyMMddhhmmssfff");
                    if (!req.Equals(""))
                    {
                        onlyForeigner = " AND ( data LIKE '%FizDocType=2%' OR data LIKE '%FizDocType=104%' ) AND data LIKE '%FizDocScanMime=.%'";
                    }
                    else
                    {
                        onlyForeigner = " where (data LIKE '%FizDocType=2%' OR data LIKE '%FizDocType=104%') AND data LIKE '%FizDocScanMime=.%'";
                    }
                }
                req += onlyForeigner;

                string fizDocOrg = "";
                if (cbOnlyFDOC.Checked)
                {
                    if (req.Equals("")) req += " data REGEXP 'FizDocOrgCode=[0-9]'";
                    else req += " AND data REGEXP 'FizDocOrgCode=[0-9]'";
                }

                if (!req.Equals("")) req += " AND";

                string ss = "SELECT phash, data, lastdate, usedcount, unitid " +
                    "FROM `autodoc_people` left join `autodoc_people_usage` using (phash) where" + req + " docid = 'DEXPlugin.Document.Mega.EFD.Fiz' order by usedcount, lastdate";

                DataTable tb = d.getQuery("SELECT phash, data, lastdate, usedcount, unitid " +
                    "FROM `autodoc_people` left join `autodoc_people_usage` using (phash) where" + req  + " docid = 'DEXPlugin.Document.Mega.EFD.Fiz' order by usedcount, lastdate");

                //DataTable tb = d.getQuery("SELECT phash, data, lastdate, usedcount, unitid " +
                //    "FROM `autodoc_people` left join `autodoc_people_usage` using (phash) where" + lockUnitId + " docid = 'DEXPlugin.Document.Mega.EFD.Fiz' and data like '%AutoDocRegId=" + reg + "%' order by usedcount, lastdate");

                IDEXServices idis = (IDEXServices)toolbox;
                // проверим полученные данные на терроризм

                List<string> valueIn = new List<string> { };
                bool newReq = false;

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

                                //foreach (JObject j in jo["data"]["coincidenceWithBirth"])
                                //{
                                //valueIn.Add(r["phash"].ToString());
                                //}
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
                            string sql = string.Format("delete from `autodoc_people` where phash = '{0}'",
                                d.EscapeString(s));
                            d.runQuery(sql);
                        }

                    }
                }
                if (newReq)
                {
                    //tb = d.getQuery("SELECT phash, data, lastdate, usedcount, unitid " +
                    //"FROM `autodoc_people` left join `autodoc_people_usage` using (phash) where" + lockUnitId + women + " docid = 'DEXPlugin.Document.Mega.EFD.Fiz' order by usedcount, lastdate");
                    tb = d.getQuery(ss);
                }

                if (tb != null && tb.Rows.Count > 0)
                {
                    wmea.maxValue = tb.Rows.Count;
                    int cnt = 0;

                    
                    // массив паспортных данных
                    //int ccc = 0;
                    //string ss = "{";

                    List<Json> jsarr = new List<Json>();
                    List<int> arr = new List<int>();

                    //int current = 0;
                    /*
                    foreach (DataRow r in tb.Rows)
                    {
                      
                        Regex rgx = new Regex("\\s+");
                        StringList dd = new StringList(r["data"].ToString());
                        string FizDocSeries = rgx.Replace(dd["FizDocSeries"].ToString(), "");
                        string FizDocNumber = dd["FizDocNumber"].ToString();
                        Json js = new Json();
                        js.series = FizDocSeries;
                        js.number = FizDocNumber;
                        jsarr.Add(js);

                        if (ccc == 49 || tb.Rows.Count - arr.Count < 50)
                        {
                            ss += "}";
                            IDEXServices idis = (IDEXServices)toolbox;
                            string json1 = JsonConvert.SerializeObject(jsarr);
                            string statusPassport = idis.checkPassportPacket(json1);

                            char[] charsToTrim = { '[', ']' };
                            string[] result = statusPassport.Trim(charsToTrim).Split(',');
                            for (int i = 0; i < result.Count(); i++)
                            {
                                arr.Add(Convert.ToInt32(result[i]));
                            }

                            ccc = 0;
                            ss = "";
                            jsarr.Clear();
                        }
                        else
                        {
                            ccc++;
                        }
                        
                    }
                    */





                    foreach (DataRow r in tb.Rows)
                    {
                        string ph = r["phash"].ToString();
                        StringList dd = new StringList(r["data"].ToString());


                        //bool statusPassport = false;
                        //if (arr[current] == 1) statusPassport = true;

                        //if (statusPassport)
                        //{
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


                        //}
                        //current++;
                    }
                }

            }
            catch (Exception ex)
            {
                return "Внутренняя ошибка <" + ex.Message + ">";
            }

            // 30.12.13 /

            try
            {
                // 30.12.13
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


                //sbms.connect(tbSBMSUser.Text, tbSBMSPass.Text, sbmsCert, sbmsServer);

//                Dictionary<string, StringList>.Enumerator enhk = lData.GetEnumerator();

                foreach (DataRow r in dtSrc.Rows)
                {
                    if (r["error"].ToString().Equals(""))
                    {
                        // 30.12.13
                        int _unitid = ANY_UNIT;
                        if (lockToUnitId) _unitid = Convert.ToInt32(r["unitid"]);
                        DealerInfo di = getDealer(_unitid);

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
                        r["lastdate"] = abdata["#lastdate"]; // dd.mm.yyyy <= dd.mm.yyyy
                        r["name"] = string.Format("{0} {1} {2}", abdata["LastName"], abdata["FirstName"], abdata["SecondName"]);
                        r["abdata"] = abdata.SaveToString();
                        r["phash"] = kvp.Key;

                        //тим сим-карты
                        DataTable dtTypeSim = d.getQuery("SELECT billing FROM `um_data` where msisdn = '"+r["msisdn"].ToString()+"'");
                        bool type = false;
                        if (dtTypeSim.Rows.Count > 0)
                        {
                            type = Convert.ToBoolean(dtTypeSim.Rows[0]["billing"].ToString());
                        }
                        if (!type)
                        {
                            // сим LG
                            try
                            {
                                //TODO получение данных из СБМС
                                //SimpleXML xml = sbms.httpRequest("CLIR_API_CHECK_MSISDN", "pmsisdn_find", r["msisdn"].ToString());
                                //if (xml == null) throw new Exception("1");
                                //if (sbms.lastErrorMessage != null) throw new Exception(sbms.lastErrorMessage);

                                //SimpleXML xml2 = xml.GetNodeByPath("CLIR_API_CHECK_MSISDN", false);
                                //if (xml2 == null) throw new Exception("2");

                                //string psubs_id = xml2["psubs_id"].Text;
                                //string pctyp_id = xml2["pctyp_id"].Text;
                                //string pccat_id = xml2["pccat_id"].Text;
                                //string prtpl_id = xml2["prtpl_id"].Text;
                                //string prtpl_name = "";

                                //xml = sbms.httpRequest("CLIR_API_GET_RTPL_LIST", "psubs_id", psubs_id, "pccat_id", pccat_id, "pctyp_id", pctyp_id);
                                //if (xml == null) throw new Exception("3");

                                //if (sbms.lastErrorMessage != null) throw new Exception(sbms.lastErrorMessage);

                                //xml2 = xml.GetNodeByPath("CLIR_API_GET_RTPL_LIST", false);
                                /*
                                if (xml2 == null) throw new Exception("4");

                                //dtSbmsPlan = sbms.xml2table(xml2, "RTPL_");

                                SimpleXML xmlSBMSPlans = new SimpleXML("SBMSPlans");

                                foreach (SimpleXML node in xml2.Child)
                                {
                                    try
                                    {
                                        if (node != null && "ROW".Equals(node.Name))
                                        {
                                            SimpleXML nid = node.GetNodeByPath("RTPL_ID", false), ndef = node.GetNodeByPath("RTPL_DEF", false);
                                            if (nid != null && ndef != null)
                                            {
                                                /* Это не нужно, т.к. мы не пользуемся информацией о ТП.
                                                SimpleXML xmlItem = xmlSBMSPlans.CreateChild("item");
                                                xmlItem.Attributes["id"] = nid.Text;
                                                xmlItem.Attributes["title"] = ndef.Text;
                                                 */
                              /*                  if (prtpl_id.Equals(nid.Text)) prtpl_name = ndef.Text;
                                            }
                                        }
                                    }
                                    catch (Exception) { }
                                }

                                SimpleXML xmlsel = xmlSBMSPlans.CreateChild("selected");
                                xmlsel.Attributes["id"] = prtpl_id;
                                xmlsel.Attributes["title"] = prtpl_name;
                                xmlsel.Attributes["pctyp_id"] = pctyp_id;

                                r["plan"] = SimpleXML.SaveXml(xmlSBMSPlans);

                                */
                                /*
                                xml = sbms.httpRequest(
                                    "CLIR_GET_PCONTRACT_NEW_DATA_R",
                                    "pmsisdn_find", r["msisdn"].ToString(), "picc_find", r["icc"].ToString() + DEXTools.calcIccCtl(r["icc"].ToString()).ToString(),
                                    "pis_fmcg", "0", "pccat_id", pccat_id, "pctyp_id", pctyp_id
                                    );

                                xml2 = xml.GetNodeByPath("CLIR_GET_PCONTRACT_NEW_DATA", false);
                                if (xml2 == null) throw new Exception("5");

                                r["docnum"] = xml2["new_pcntr_num"].Text;
                                */

                            }
                            catch (Exception ex)
                            {
                                r["error"] = "Не удалось получить данные из СБМС (" + ex.Message + ")";
                            }
                        }
                        else
                        {
                            // сим GF
                            try
                            {
                                

                            }
                            catch (Exception) { }

                        }

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
            string signaturedate = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            try
            {
                foreach (DataRow r in dtSrc.Rows)
                {
                    if (r["error"].ToString().Equals(""))
                    {
                        StringList abdata = new StringList(r["abdata"].ToString());
                        SimpleXML xml = new SimpleXML("Document");
                        xml.Attributes["ID"] = "DEXPlugin.Document.Mega.EFD.Fiz";
                        xml["DocCity"].Text = abdata["AddrCity"];
                        xml["DocNum"].Text = r["docnum"].ToString();

                        if (cbFixedDocDate.Checked)
                        {
                            xml["DocDate"].Text = deDocDate.Text;
                            xml["DocDateJournal"].Text = deDocDate.Text;
                        }
                        else
                        {
                            xml["DocDate"].Text = r["date"].ToString();
                            xml["DocDateJournal"].Text = deDocDate.Text;
                        }

                        xml["DocCategory"].Text = "1";
                        xml["DocCategoryPrn"].Text = "Предоплатный";
                        xml["DocClientType"].Text = "1";
                        xml["CodeWord"].Text = "";
                        xml["MSISDN"].Text = r["msisdn"].ToString();
                        xml["ICC"].Text = r["icc"].ToString();
                        xml["ICCCTL"].Text = DEXTools.calcIccCtl(r["icc"].ToString()).ToString();

                        IDEXData d = (IDEXData)toolbox;
                        DataTable dt = d.getQuery("SELECT billing FROM `um_data` where msisdn = '" + r["msisdn"].ToString() + "' and icc = '" + r["icc"].ToString() + "'");
                        bool typeSim = false;
                        SimpleXML xmlPlans = SimpleXML.LoadXml(r["plan"].ToString());
                        if (dt!= null && dt.Rows.Count > 0)
                        {
                            typeSim = Convert.ToBoolean(dt.Rows[0]["billing"].ToString());
                        }
                        typeSim = true;
                        if (typeSim)
                        {
                            xml["gf"].Text = "True";
                            DataTable dtplans = d.getQuery("SELECT plan_id, title FROM `um_plans_gf` where allowed = '1' and title = 'Всё просто'");
                            if (dtplans != null && dtplans.Rows.Count > 0)
                            {
                                xml["IntPlanPrn"].Text = dtplans.Rows[0]["title"].ToString();
                                xml["Plan"].Text = dtplans.Rows[0]["plan_id"].ToString();
                            }
                        }
                        else
                        {

                       
                            SimpleXML xmlsel = xmlPlans["selected"];
                            xmlsel.Parent = null;

                            xml["Plan"].Text = xmlsel.Attributes["id"];
                            xml["PlanPrn"].Text = xmlsel.Attributes["title"];
                            xml["IntPlanPrn"].Text = r["intplan"].ToString();
                            
                            xmlPlans.Parent = xml; // SBMSPlans                        

                            xml["pctyp_id"].Text = xmlsel.Attributes["pctyp_id"];
                        

                        }
                        xml["FirstName"].Text = abdata["FirstName"];
                        xml["SecondName"].Text = abdata["SecondName"];
                        xml["LastName"].Text = abdata["LastName"];
                        
                        xml["Sex"].Text = abdata["Sex"];
                        //xml["FizBirthPlace"].Text = abdata["FizBirthPlace"];
                        xml["Birth"].Text = abdata["Birth"];

                        // получим регион симки
                        DataTable dtRegion = ((IDEXData)toolbox).getQuery("select `region_id` from `um_data` WHERE `msisdn` = '" + xml["MSISDN"].Text + "'");
                        if (dtRegion != null && dtRegion.Rows.Count > 0)
                        {
                            xml["RegionId"].Text = dtRegion.Rows[0]["region_id"].ToString();
                        }


                        xml["FizDocOrg"].Text = abdata["FizDocOrg"];
                        xml["FizDocOrgCode"].Text = abdata["FizDocOrgCode"];
                        xml["FizDocNumber"].Text = abdata["FizDocNumber"];
                        xml["FizDocSeries"].Text = abdata["FizDocSeries"];
                        xml["FizDocType"].Text = abdata["FizDocType"];
                        xml["FizDocDate"].Text = abdata["FizDocDate"];
                        xml["FizBirthPlace"].Text = abdata["FizBirthPlace"];

                        xml["FizDocScan"].Text = abdata["FizDocScan"];//scn ? "X" : "-";
                        xml["FizDocScanMime"].Text = cbOnlyForeigner.Checked ? abdata["FizDocScanMime"] : "";

                        xml["AddrCountry"].Text = abdata["AddrCountry"];
                        xml["AddrState"].Text = abdata["AddrState"];
                        xml["AddrCity"].Text = abdata["AddrCity"];
                        xml["AddrZip"].Text = abdata["AddrZip"];
                        xml["AddrRegion"].Text = abdata["AddrRegion"];
                        xml["AddrStreet"].Text = abdata["AddrStreet"];
                        xml["AddrHouse"].Text = abdata["AddrHouse"];
                        xml["AddrBuilding"].Text = abdata["AddrBuilding"];
                        xml["AddrApartment"].Text = abdata["AddrApartment"];

                        xml["AddrPhone"].Text = abdata["AddrPhone"];
                        xml["ContactEmail"].Text = abdata["ContactEmail"];
                        xml["FizInn"].Text = abdata["FizInn"];

                        /*
                         * Если "тип доставки" не указан или "0" - присваиваем ему "не доставлять"
                         * Если "тип доставки" = "не доставлять" - копируем все поля Delivery из полей Addr
                         */
                        string dtype = abdata["DeliveryType"];
                        if ("".Equals(dtype) || "0".Equals(dtype)) dtype = "3";
                        string fpr = ("3".Equals(dtype)) ? "Addr" : "Delivery";

                        xml["DeliveryType"].Text = dtype;
                        xml["DeliveryPhone"].Text = abdata["DeliveryPhone"];
                        xml["DeliveryFax"].Text = abdata["DeliveryFax"];

                        xml["DeliveryCountry"].Text = abdata[fpr + "Country"];
                        xml["DeliveryState"].Text = abdata[fpr + "State"];
                        xml["DeliveryCity"].Text = abdata[fpr + "City"];
                        xml["DeliveryZip"].Text = abdata[fpr + "Zip"];
                        xml["DeliveryRegion"].Text = abdata[fpr + "Region"];
                        xml["DeliveryStreet"].Text = abdata[fpr + "Street"];
                        xml["DeliveryHouse"].Text = abdata[fpr + "House"];
                        xml["DeliveryBuilding"].Text = abdata[fpr + "Building"];
                        xml["DeliveryApartment"].Text = abdata[fpr + "Apartment"];

                        //13.01.2014
                        xml["DutyId"].Text = r["unitid"].ToString();
                        //13.01.2014 /

                        int unitid = -1;
                        try
                        {
                            if (cbUnitAsSIM.Checked) unitid = int.Parse(r["unitid"].ToString());
                            else unitid = int.Parse(((StringTagItem)cbUnit.SelectedItem).Tag.ToString());
                        }
                        catch (Exception)
                        {
                        }

                        string digest = string.Format(
                            "{0}, {1}, {2} {3} {4}",
                            xml["MSISDN"].Text, xml["DocDate"].Text, xml["LastName"].Text,
                            xml["FirstName"].Text, xml["SecondName"].Text
                            );

                        SimpleXML sjournal = new SimpleXML("journal");
                        IDEXDocumentJournal jrn = (IDEXDocumentJournal)toolbox;
                        jrn.AddRecord(sjournal, "Документ сформирован функцией формирования группы документов");

                        String ddd = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        
                        //IDEXData d = (IDEXData)toolbox;
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
                        string ldate = r["lastdate"].ToString(); // dd.mm.yyyy <= dd.mm.yyyy
                        if (ldate == null || ldate.Equals("-") || ldate.Length != 10)
                        {
                            ldate = DateTime.Now.ToString("yyyyMMdd"); // yyyymmdd
                        }
                        else
                        {
                            ldate = ldate.Substring(6, 4) + ldate.Substring(3, 2) + ldate.Substring(0, 2); // yyyymmdd <= dd.mm.yyyy
                        }

                        sql = string.Format(
                            "insert into `autodoc_people_usage` (phash, lastdate, usedcount) values " +
                            "('{0}', '{1}', {2})", d.EscapeString(r["phash"].ToString()),
                            d.EscapeString(ldate), ucnt);
                        d.runQuery(sql);



                        if (cbOnlyForeigner.Checked)
                        {
                            try
                            {
                                // если скан нужен для иностранца, то надо бы скан скопировать для договора, но для начала проверим, есть ли скан
                                DataTable t1 = ((IDEXData)toolbox).getQuery("select rvalue from `registers` where rname = 'nodejsserver'");
                                string nodejsserver = t1.Rows[0]["rvalue"].ToString();
                                string currentBase = ((IDEXUserData)toolbox).dataBase;
                                IDEXServices idis = (IDEXServices)toolbox;
                                JObject data = new JObject();
                                data["base"] = currentBase;
                                string vendorBase = "";
                                JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
                                //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                                if (int.Parse(obj["data"]["status"].ToString()) == 1)
                                {
                                    vendorBase = obj["data"]["base"].ToString();
                                }


                                string vendor = "megafon";
                                JObject ifScan = new JObject();
                                ifScan["vendor"] = vendor;
                                ifScan["base"] = vendorBase;
                                ifScan["signature"] = abdata["scanSignature"];
                                ifScan["mime"] = abdata["FizDocScanMime"];
                                JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/ifIssetScan?data=" + JsonConvert.SerializeObject(ifScan), 0));
                                if (o["data"]["status"].ToString().Equals("1"))
                                {
                                    // теперь скопируем файл и дадим ему новое имя
                                    JObject createNewScan = new JObject();
                                    createNewScan["vendor"] = vendor;
                                    createNewScan["base"] = vendorBase;
                                    createNewScan["signature"] = abdata["scanSignature"];
                                    createNewScan["mime"] = abdata["FizDocScanMime"];
                                    createNewScan["newname"] = signaturedate + cnt.ToString("D8");
                                    o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/copyScanForAutodoc?data=" + JsonConvert.SerializeObject(createNewScan), 0));
                                }
                                else
                                {
                                    // вот нет скана
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
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

        private void tbAnyPassUnits_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
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
