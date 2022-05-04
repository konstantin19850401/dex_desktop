using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using DEXExtendLib;
using System.Collections;
using System.Drawing.Printing;


namespace dexol
{
    public partial class Main : Form
    {
        LoginForm loginForm;
        DexolSession session;
        DateTime nextPingTime;
        PersistWindowState pws;
        DEXToolBox toolbox;
        BindingSource bsJournal = null;
        DataTable dtJournal;

        public Main()
        {
            InitializeComponent();
            toolbox = DEXToolBox.getToolBox();


            dgvJournal.AutoGenerateColumns = false;

            pws = new PersistWindowState(this);

            session = DexolSession.inst();

            nameTheForm();

            loginForm = new LoginForm();
            nextPingTime = DateTime.Now;
            tStatus.Enabled = true;
            deDate.Value = DateTime.Now;

            toolbox.Plugins.ScanPlugins(toolbox.AppDir + @"\plugins\");

            tsddbNewDoc.DropDownItems.Clear();
            tsddbSetup.DropDownItems.Clear();

            dgvJournal.ColumnWidthChanged -= new DataGridViewColumnEventHandler(dgvJournal_ColumnWidthChanged);
            IDEXConfig cfg = (IDEXConfig)toolbox;
            foreach (DataGridViewTextBoxColumn col in dgvJournal.Columns)
            {
                col.Width = cfg.getInt("dgvJournal", col.Name, 32);
            }
            dgvJournal.ColumnWidthChanged += new DataGridViewColumnEventHandler(dgvJournal_ColumnWidthChanged);

            cbAutoRefresh.Checked = cfg.getBool("common", "cbAutoRefresh", true);
        }

        void nameTheForm()
        {
            string formTitle = "DEX Онлайн";
            string uname = DexolSession.inst().user_name;
            if (uname != null && !"".Equals(uname)) formTitle += " - " + uname;

            try
            {
                formTitle += " - [ " + File.ReadAllText("update.key") + " ] ";
            }
            catch (Exception) { }

            Text = formTitle;
        }

        bool timer_busy = false;

        private void tStatus_Tick(object sender, EventArgs e)
        {
            if (timer_busy) return;
            timer_busy = true;
            try
            {
                if (nextPingTime < DateTime.Now)
                {
                    session.ping();
                    nextPingTime = DateTime.Now.AddMinutes(5);
                }

                if (!session.isLoggedIn())
                {
                    tStatus.Enabled = false;
                    DialogResult result = loginForm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        nameTheForm();

                        toolbox.ParseUserData(DexolSession.inst().user_props);
                        string cdb = ((IDEXConfig)toolbox).getStr("common", "currentDb", null);

                        Dictionary<string, StringDbItem> dbitems = session.dblist();
                        cbDb.Items.Clear();
                        int selIndex = -1;
                        foreach (KeyValuePair<string, StringDbItem> kvp in dbitems)
                        {
                            int tindex = cbDb.Items.Add(kvp.Value);
                            if (cdb != null && cdb.Equals(kvp.Value.dbname)) selIndex = tindex;
                        }

                        cbDb.SelectedIndex = selIndex;

                        tStatus.Enabled = true;
                    }
                }
            }
            catch (Exception) { }
            timer_busy = false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            session.logout();
            ((IDEXConfig)toolbox).setBool("common", "cbAutoRefresh", cbAutoRefresh.Checked);
        }

        private void bQuery_Click(object sender, EventArgs e)
        {
            string er = "";

            if (!deDate.IsValid || deDate.Value.Ticks == 0) er += "* Некорректная дата\n";
            if (cbDb.SelectedItem == null) er += "* Не выбрана БД\n";

            if (er == "")
            {
                // Получение выборки документов за определенную дату
                DexolSession ds = DexolSession.inst();
                ds.currentDb = ((StringDbItem)cbDb.SelectedItem).dbname;

                DataTable dt = ds.queryJournal(deDate.Value);
                if (dt != null)
                {
                    bsJournal = new BindingSource();
                    bsJournal.DataSource = dt;
                    dgvJournal.DataSource = bsJournal;
                }
                else
                {
                    bsJournal = null;
                    dgvJournal.DataSource = null;
                }
                dgvJournal.Visible = dt != null && dt.Rows.Count > 0;
            }
            else
            {
                if (sender != null) MessageBox.Show("Ошибки:\n\n" + er);
            }
        }
        private void dgvJournal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string coln = dgvJournal.Columns[e.ColumnIndex].Name;

            if (coln.Equals("vdocdate"))
            {
                if (e.Value != null)
                {
                    string old = e.Value.ToString();
                    e.Value = string.Format("{0}.{1}.{2}",
                        old.Substring(6, 2), old.Substring(4, 2), old.Substring(0, 4)
                        );
                }
            }
            else if (coln.Equals("vdocnum")) 
            {
                try
                {
                    DataRow r = ((DataRowView)dgvJournal.Rows[e.RowIndex].DataBoundItem).Row;
                    SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                    e.Value = xml["DocNum"].Text;
                }
                catch (Exception) { }
            }
            else if (coln.Equals("status"))
            {
                string[] ss = DEXToolBox.DOCUMENT_STATE_TEXT;
                try
                {
                    e.Value = ss[int.Parse(e.Value.ToString())];
                }
                catch (Exception)
                {
                    e.Value = "-";
                }
            }
        }

        private void dgvJournal_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int nw = (e.Column.Width < 10) ? 10 : e.Column.Width;
            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            cfg.setInt("dgvJournal", e.Column.Name, nw);
        }


        string dicdir;

        bool doRunUpdate = false;

        private void bRefreshDics_Click(object sender, EventArgs e)
        {
            dicdir = toolbox.DataDir + DexolSession.inst().currentDb + @"\";
            if (!Directory.Exists(dicdir)) Directory.CreateDirectory(dicdir);
            string ret = WaitMessage.Execute(wmeRefreshDics);
            if (ret != "") MessageBox.Show("Ошибки:\n\n" + ret);
            else MessageBox.Show("Справочники успешно обновлены");
            
            if (doRunUpdate)
            {
                string rootdir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
                System.Diagnostics.Process.Start(rootdir + "update.exe", rootdir);
                Application.Exit();
            }
            
        }



        string wmeRefreshDics(IWaitMessageEventArgs wmea)
        {
            string ret = "";

            doRunUpdate = false;

            wmea.canAbort = true;
            wmea.minValue = 0;
            wmea.maxValue = 1;
            wmea.progressVisible = true;

            int pv = 0;

            DexolSession session = DexolSession.inst();

            // Обновление программы
            wmea.textMessage = "Обновление программы";
            wmea.DoEvents();
            
            string rkey = session.getUpdateKey();
            if (rkey != null)
            {
                if (!File.Exists("update.key") || !rkey.Equals(File.ReadAllText("update.key")))
                {
                    if (session.downloadUpdate(Path.GetDirectoryName(Application.ExecutablePath) + @"\update.exe"))
                    {
                        doRunUpdate = true;
                        foreach (string arg in Environment.GetCommandLineArgs())
                        {
                            if (arg != null && arg.IndexOf("noupdate") > -1) doRunUpdate = false;
                        }
                    }
                }
            }
            
            // Обновление справочников
            wmea.textMessage = "Загрузка справочников";
            wmea.DoEvents();

            Dictionary<string, string> dss = session.getDicList();

            if (dss != null && dss.Count > 0)
            {
                wmea.maxValue = dss.Count - 1;

                foreach (KeyValuePair<string, string> kvp in dss)
                {
                    bool needUpdate = true;

                    if (File.Exists(dicdir + kvp.Key + ".md5"))
                    {
                        try
                        {
                            if (kvp.Value.Equals(File.ReadAllText(dicdir + kvp.Key + ".md5"))) needUpdate = false;
                        }
                        catch (Exception) { }
                    }

                    if (needUpdate)
                    {
                        try
                        {
                            int fcount = session.tableRecordsCount(kvp.Key);
                            if (fcount > 0)
                            {
                                wmea.textMessage = string.Format("Загрузка справочника <{0}>", kvp.Key);
                                wmea.progressValue = 0;
                                wmea.maxValue = fcount;
                                wmea.DoEvents();

                                const int FPOS_INC = 10000;

                                JObject commonJobj = null;
                                int fpos = 0;
                                while (fpos < fcount)
                                {
                                    string sql = string.Format("select * from `{0}` limit {1}, {2}", kvp.Key, fpos, FPOS_INC);

                                    JObject nobj = session.jsonHttpRequest("sid", session.sid, "f", "getquery", "db", session.currentDb, "sql", sql);
                                    if (commonJobj == null) commonJobj = nobj;
                                    else
                                    {
                                        JArray jdata = (JArray)commonJobj["data"];
                                        JArray jsrc = (JArray)nobj["data"];
                                        int rowsCount = jsrc.Count;
                                        for (int i = 0; i < rowsCount; ++i)
                                        {
                                            jdata.Add(jsrc[i]);
                                        }
                                    }

                                    wmea.progressValue = fpos;
                                    wmea.DoEvents();
                                    if (wmea.isAborted)
                                    {
                                        return null; // "Операция прервана";
                                    }
                                    fpos += FPOS_INC;
                                }
                                if (commonJobj != null)
                                {
                                    File.WriteAllText(dicdir + kvp.Key + ".json", commonJobj.ToString(), Encoding.UTF8);
                                }
                            }

                            File.WriteAllText(dicdir + kvp.Key + ".md5", kvp.Value, Encoding.UTF8);
                        }
                        catch (Exception)
                        {
                            ret += "Не удалось обновить справочник <" + kvp.Key + ">\n";
                        }
                    }

                    wmea.maxValue = dss.Count - 1;
                    wmea.progressValue = pv;
                    pv++;
                }

                // Обновление справочника SIM-карт 
                try
                {
                    string sql = string.Format("select * from `um_data` where status = 1 and owner_id = {0}", ((StringDbItem)cbDb.SelectedItem).unit_uid);
                    File.WriteAllText(dicdir + "um_data.json",
                        session.jsonHttpRequest("sid", session.sid, "f", "getquery", "db", session.currentDb, "sql", sql).ToString(),
                        Encoding.UTF8);
                }
                catch (Exception) {
                    ret += "Не удалось обновить справочник SIM-карт\n";
                }
            }
            else
            {
                ret = "Не удалось получить таблицу справочников";
            }

            toolbox.dtCity = null;
            toolbox.simTable = null;


            return ret;
        }

        private void cbDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DexolSession.inst().currentDb = ((StringDbItem)cbDb.SelectedItem).dbname;
                toolbox.currentDocumentTypes = ((StringDbItem)cbDb.SelectedItem).doctypes;
                IDEXConfig cfg = (IDEXConfig)toolbox;
                cfg.setStr("common", "currentDb", ((StringDbItem)cbDb.SelectedItem).dbname);
                toolbox.dtCity = null;
                toolbox.simTable = null;
                toolbox.sUID = "" + ((StringDbItem)cbDb.SelectedItem).unit_uid;
                toolbox.sCurrentBase = ((StringDbItem)cbDb.SelectedItem).dbname;
                // Регистрация документов в меню
                tsddbNewDoc.DropDownItems.Clear();
                toolbox.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DOCUMENT, tsddbNewDoc, 
                    new EventHandler(NewDocument_Click), toolbox.currentDocumentTypes, false);
                toolbox.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DOCUMENT, tsddbSetup,
                    new EventHandler(SetupDictionary_Click), true);

            }
            catch (Exception) { }
        }
        private void SetupDictionary_Click(object sender, EventArgs e)
        {
            // Вызов пункта меню справочника (настройки)
            if (((ToolStripItem)sender).Tag is IDEXPluginSetup)
            {
                IDEXPluginSetup caller = (IDEXPluginSetup)((ToolStripItem)sender).Tag;
                caller.Setup(DEXToolBox.getToolBox());
            }
        }

        private void NewDocument_Click(object sender, EventArgs e)
        {
            //TODO Кнопка "Новый документ"
            if (((ToolStripItem)sender).Tag is IDEXPluginDocument)
            {
                IDEXPluginDocument doc = (IDEXPluginDocument)((ToolStripItem)sender).Tag;

                CDEXDocumentData newdoc = new CDEXDocumentData();

                Random r = new Random();
                int i = r.Next(99999999);
                String newsignature = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT_MS) + i.ToString("D8");
                newdoc.signature = newsignature;

                toolbox.setCurrentJournal(new SimpleXML("journal"));

                if (doc.NewDocument((object)toolbox, newdoc))
                {
                    newdoc.documentStatus = int.Parse(toolbox.UserProperties["DefaultDocumentState"].Text);
                    //newdoc.documentStatus = newdoc.documentStatus;


                    // Проверка criticals
                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_DRAFT)
                    {
                        ArrayList err = toolbox.checkDocumentCriticals(doc.GetDocumentCriticals(toolbox), newdoc);
                        if (err != null && err.Count > 0)
                        {
                            toolbox.AddRecord("Выявлены совпадения с другими документами:");
                            foreach (string eitem in err) toolbox.AddRecord(eitem);
                            toolbox.AddRecord("Статус документа понижен.");
                            newdoc.documentStatus = DEXToolBox.DOCUMENT_DRAFT;

                            //TODO Сделать диалог запроса, продолжить ли редактирование этого документа?
                            MessageBox.Show("Выявлены совпадения с другими документами.\nСтатус документа понижен.");
                        }
                    }

                    JObject json = new JObject();
                    json["action"] = "new";
                    json["status"] = newdoc.documentStatus;
                    json["signature"] = newsignature;
                    json["jdocdate"] = newdoc.documentDate;
                    json["docid"] = doc.ID;
                    json["digest"] = newdoc.documentDigest;
                    json["data"] = newdoc.documentText;
                    json["journal"] = SimpleXML.SaveXml(toolbox.getCurrentJournal());
                    json["criticals"] = DexolSession.inst().jsonDocumentCriticals(doc.GetDocumentCriticals(toolbox), newdoc);

                    // Установка People Data
                    try
                    {
                        StringList slp = doc.GetPeopleData(toolbox, newdoc);
                        if (slp != null)
                        {
                            JObject pdata = new JObject();
                            foreach (KeyValuePair<string, string> kvp in slp.getDictonary())
                            {
                                pdata[kvp.Key] = kvp.Value;
                            }
                            json["peopledata"] = pdata;
                            
                        }
                    }
                    catch (Exception) { }

                    string result = DexolSession.inst().commitDocument(json);
                    if (result != null) MessageBox.Show("Ошибка:\n\n" + result);
                    else
                    {
                        if (cbAutoRefresh.Checked) bQuery_Click(null, null);
                    }

                }
                toolbox.setCurrentJournal(null);
            }
        }

        private void tsbEditDoc_Click(object sender, EventArgs e)
        {
            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                DataRow row = ((DataRowView)bsJournal.Current).Row;
                int rid = int.Parse(row["id"].ToString());
                string signature = row["signature"].ToString();

                DataTable dt = toolbox.getQuery("select * from `journal` where id = {0}", rid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string er = "";

                    DataRow r = dt.Rows[0];
                    try
                    {
                        int status = Convert.ToInt32(r["status"]);
                        if (status >= DEXToolBox.DOCUMENT_TOEXPORT && status != DEXToolBox.DOCUMENT_RETURNED) er += "* Обратитесь в офис для понижения статуса документа\n";

                        string lck = r["locked"].ToString();
                        if (lck != "" && lck != toolbox.MAC) er += "* Документ редактируется другим пользователем\n";
                    }
                    catch (Exception) 
                    {
                        er += "* Некорректное значение в данных\n";
                    }

                    IDEXPluginDocument doc = toolbox.Plugins.getDocumentByID(r["docid"].ToString());
                    if (doc == null) er += "* Невозможно загрузить документ (отсутствует модуль)\n";

                    if (er == "")
                    {
                        string sql = string.Format(
                            "update `journal` set locked = '{0}', locktime = '{1}' where id = {2}",
                            toolbox.EscapeString(toolbox.MAC),
                            toolbox.EscapeString(DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT)),
                            rid
                            );

                        int qresult = toolbox.runQuery(sql);
                        if (qresult == 0)
                        {
                            try
                            {
                                //
                                // Редактирование документа
                                //
                                CDEXDocumentData olddoc = new CDEXDocumentData();
                                olddoc.documentDate = r["jdocdate"] as string;
                                olddoc.documentStatus = int.Parse(toolbox.UserProperties["DefaultDocumentState"].Text); //int.Parse(r["status"].ToString());
                                olddoc.documentText = r["data"].ToString();
                                olddoc.documentUnitID = int.Parse(r["unitid"].ToString());
                                olddoc.signature = signature;

                                CDEXDocumentData newdoc = new CDEXDocumentData();
                                newdoc.signature = signature;

                                toolbox.setCurrentJournal(SimpleXML.LoadXml(r["journal"].ToString()));

                                if (doc.EditDocument(toolbox, olddoc, newdoc, null, olddoc.documentStatus == DEXToolBox.DOCUMENT_EXPORTED))
                                {
                                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_TOEXPORT) newdoc.documentStatus = DEXToolBox.DOCUMENT_TOEXPORT;

                                    // Проверка criticals
                                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_DRAFT)
                                    {
                                        ArrayList err = toolbox.checkDocumentCriticals(doc.GetDocumentCriticals(toolbox), newdoc);
                                        if (err != null && err.Count > 0)
                                        {
                                            toolbox.AddRecord("Выявлены совпадения с другими документами:");
                                            foreach (string eitem in err) toolbox.AddRecord(eitem);
                                            toolbox.AddRecord("Статус документа понижен.");
                                            newdoc.documentStatus = DEXToolBox.DOCUMENT_DRAFT;

                                            //TODO Сделать диалог запроса, продолжить ли редактирование этого документа?
                                            MessageBox.Show("Выявлены совпадения с другими документами.\nСтатус документа понижен.");
                                        }
                                    }

                                    JObject json = new JObject();
                                    json["action"] = "edit";
                                    json["id"] = rid;
                                    json["status"] = newdoc.documentStatus;
                                    json["signature"] = signature;
                                    json["jdocdate"] = newdoc.documentDate;
                                    json["docid"] = doc.ID;
                                    json["digest"] = newdoc.documentDigest;
                                    json["data"] = newdoc.documentText;
                                    json["journal"] = SimpleXML.SaveXml(toolbox.getCurrentJournal());
                                    json["criticals"] = DexolSession.inst().jsonDocumentCriticals(doc.GetDocumentCriticals(toolbox), newdoc);

                                    // Установка People Data
                                    try
                                    {
                                        StringList slp = doc.GetPeopleData(toolbox, newdoc);
                                        if (slp != null)
                                        {
                                            JObject pdata = new JObject();
                                            foreach (KeyValuePair<string, string> kvp in slp.getDictonary())
                                            {
                                                pdata[kvp.Key] = kvp.Value;
                                            }
                                            json["peopledata"] = pdata;

                                        }
                                    }
                                    catch (Exception) { }

                                    string result = DexolSession.inst().commitDocument(json);
                                    if (result != null) MessageBox.Show("Ошибка:\n\n" + result);
                                    else
                                    {
                                        if (cbAutoRefresh.Checked) bQuery_Click(null, null);
                                    }
                                }
                                else
                                {
                                    toolbox.runQuery("update `journal` set locked = '', locktime = '' where id = {0}", rid);
                                }
                            }
                            catch (Exception) 
                            {
                                MessageBox.Show("Ошибка получения значений из записи.");
                            } 

                        }
                        else
                        {
                            MessageBox.Show(string.Format("Невозможно заблокировать запись ({0})", qresult));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невозможно редактировать документ по следующим причинам:\n\n" + er);
                    }

                }
                else
                {
                    MessageBox.Show("Ошибка получения записи с сервера.");
                }
            }
        }

        private void tsbDeleteDoc_Click(object sender, EventArgs e)
        {
            string msg = null;
            if (dgvJournal != null)
            {
                try
                {
                    DataRow row = ((DataRowView)bsJournal.Current).Row;
                    int rid = int.Parse(row["id"].ToString());

                    if (MessageBox.Show("Удалить документ?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes) 
                    {
                        DexolSession ds = DexolSession.inst();
                        msg = ds.deleteDocument(rid);
                        if (msg == null)
                        {
                            if (cbAutoRefresh.Checked) bQuery_Click(null, null);
                            msg = "Документ удалён";
                        }
                    }
                }
                catch (Exception) {
                    msg = "Не выделен документ для удаления";
                }

            }
            else
            {
                msg = "Не выделен документ для удаления";
            }

            if (msg != null) MessageBox.Show(msg);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete)
            {
                tsbDeleteDoc_Click(tsbDeleteDoc, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvJournal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsbEditDoc_Click(null, null);
        }

        private void tsmiTTCExport_Click(object sender, EventArgs e)
        {
            TTCExportForm form = new TTCExportForm(toolbox, deDate.Value);
            form.ShowDialog();
        }

        private void tsmiDocsReport_Click(object sender, EventArgs e)
        {
            new DocsReportForm().ShowDialog();
        }

        
        #region Печать документов

        private void tsmiPreviewDoc_Click(object sender, EventArgs e)
        {
            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                if (dgvJournal.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Возможен просмотр только одного документа.\nВыделите один документ и попробуйте вызвать просмотр снова.");
                    return;
                }

                DataRow r = ((DataRowView)bsJournal.Current).Row;

                SimpleXML dc = null;
                try
                {
                    dc = SimpleXML.LoadXml(r["data"].ToString());
                    //dc["ICC"].Text = dc["ICC"].Text.Substring(7, 10);
                    dc["MainDealerName"].Text = "OOO 'N-Telecom'";
                    dc["MainDealerFIO"].Text = "Айвазашвили О.Л.";
                    dc["MainDealerPowAt"].Text = "23АГ554154";
                    dc["MainDealerDatePowAt"].Text = "01.07.2009";

                    

                    
                }
                catch (Exception)
                {
                }

                DEXToolBox tb = DEXToolBox.getToolBox();

                SimpleXML[] schemes = CPrintDocument.GetSchemesForId(tb.DataDir + @"printing_schemes\" , r["docid"].ToString());

                if (schemes != null && schemes.Length > 0)
                {
                    SimpleXML scheme = schemes[0];
                    if (schemes.Length > 1)
                    {
                        // Диаложек
                        SelectSchemeForm ssf = new SelectSchemeForm(schemes);
                        if (ssf.ShowDialog() == DialogResult.OK && ssf.selected > -1)
                        {
                            scheme = schemes[ssf.selected];
                        }
                        else
                        {
                            scheme = null;
                        }
                    }

                    if (scheme != null)
                    {
                        PrinterSettings ps = tb.LoadPrinterSettings();
                        /*
                        DataTable t = tb.getQuery(string.Format(
                            "select * from `prnschemes` where printer = '{0}' and guid = '{1}'",
                            tb.EscapeString(ps.PrinterName), tb.EscapeString(scheme["GUID"].Text)));
                        if (t != null && t.Rows.Count > 0)
                        {
                            scheme = SimpleXML.LoadXml(t.Rows[0]["data"].ToString());
                        }
                        */

                        string schemeName = tb.DataDir + "scheme-" + tb.StringToMD5(ps.PrinterName) + "-" + scheme["GUID"].Text + ".xml";

                        if (File.Exists(schemeName))
                        {
                            SimpleXML s2 = SimpleXML.LoadXml(File.ReadAllText(schemeName, Encoding.UTF8));
                            if (s2 != null) scheme = s2;
                        }

                        CPrintDocument doc = new CPrintDocument(dc, scheme, ps, tb);
                        doc.Preview();
                    }
                }
            }

        }

        private void tsmiPrintDoc_Click(object sender, EventArgs e)
        {
            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                bool doPrint = dgvJournal.SelectedRows.Count == 1;
                if (dgvJournal.SelectedRows.Count > 1)
                {
                    doPrint = MessageBox.Show(string.Format(
                        "Распечатать выделенные документы ({0})?", dgvJournal.SelectedRows.Count
                        ), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes;
                }

                if (doPrint)
                {
                    DEXToolBox tb = DEXToolBox.getToolBox();
                    /*
                    tsslStatus.Text = "Отправка документов на печать";
                    tspbProgress.Minimum = 0;
                    tspbProgress.Maximum = dgvJournal.SelectedRows.Count;
                    tspbProgress.Value = 0;
                    */
                    // Подготовка справочника соответствия документов схемам
                    Dictionary<string, SimpleXML> schemes = new Dictionary<string, SimpleXML>();
                    bool printingInterrupted = false;
                    bool schemeWarn = false;

                    foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                    {
                        DataRowView drv = bsJournal[r.Index] as DataRowView;
                        string docid = drv["docid"].ToString();
                        if (!schemes.ContainsKey(docid))
                        {
                            SimpleXML scheme = null;

                            SimpleXML[] ss = CPrintDocument.GetSchemesForId(tb.DataDir + @"printing_schemes\", docid);
                            if (ss != null && ss.Length > 0)
                            {
                                scheme = ss[0];

                                if (ss.Length > 1)
                                {
                                    // Диаложек
                                    SelectSchemeForm ssf = new SelectSchemeForm(ss);
                                    DialogResult ssfdr = ssf.ShowDialog();
                                    if (ssfdr == DialogResult.OK && ssf.selected > -1)
                                    {
                                        scheme = ss[ssf.selected];
                                    }
                                    else
                                    {
                                        scheme = null;
                                        schemeWarn = true;
                                    }

                                    if (ssfdr == DialogResult.Cancel)
                                    {
                                        printingInterrupted = true;
                                        break;
                                    }
                                }
                            }

                            schemes[docid] = scheme;
                        }
                    }

                    if (!printingInterrupted)
                    {

                        if (schemeWarn)
                        {
                            MessageBox.Show("Для некоторых типов документов не указаны схемы печати.\n" +
                                "Печать таких документов производиться не будет.");
                        }

                        PrinterSettings ps = tb.LoadPrinterSettings();
                
                        foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                        {
                            DataRowView drv = bsJournal[r.Index] as DataRowView;

                            try
                            {
                                SimpleXML dc = null;
                                try
                                {
                                    dc = SimpleXML.LoadXml(drv["data"].ToString());
                                    //dc["ICC"].Text = dc["ICC"].Text.Substring(7, 10);
                                    dc["MainDealerName"].Text = "OOO 'N-Telecom'";
                                    dc["MainDealerFIO"].Text = "Айвазашвили О.Л.";
                                    dc["MainDealerPowAt"].Text = "23АГ554154";
                                    dc["MainDealerDatePowAt"].Text = "01.07.2009";
                                    dc["DocUnit"].Text = drv["unitid"].ToString();
                                }
                                catch (Exception)
                                {
                                }

                                SimpleXML scheme = schemes[drv["docid"].ToString()];
                                if (scheme != null)
                                {
                                    /*
                                    DataTable t = tb.getQuery(string.Format(
                                        "select * from `prnschemes` where printer = '{0}' and guid = '{1}'",
                                        tb.EscapeString(ps.PrinterName), tb.EscapeString(scheme["GUID"].Text)));
                                    if (t != null && t.Rows.Count > 0)
                                    {
                                        scheme = SimpleXML.LoadXml(t.Rows[0]["data"].ToString());
                                    }
                                    */


                                    string schemeName = tb.DataDir + "scheme-" + tb.StringToMD5(ps.PrinterName) + "-" + scheme["GUID"].Text + ".xml";

                                    if (File.Exists(schemeName))
                                    {
                                        SimpleXML s2 = SimpleXML.LoadXml(File.ReadAllText(schemeName, Encoding.UTF8));
                                        if (s2 != null) scheme = s2;
                                    }
                            
                                    CPrintDocument doc = new CPrintDocument(dc, scheme, ps, tb);
                                    doc.Print(false);
                                }
                            }
                            catch (Exception)
                            {

                            }
//                            tspbProgress.Value++;
                        }
                        /*
                        tspbProgress.Value = 0;
                        tsslStatus.Text = "";
                         */
                    }

                    if (printingInterrupted)
                    {
                        MessageBox.Show("Печать прервана");
                    }
                }
            }
        }

        private void tsmiPrinterSettings_Click(object sender, EventArgs e)
        {
            DEXToolBox tb = DEXToolBox.getToolBox();
            PrintDialog pdlg = new PrintDialog();

            pdlg.PrinterSettings = tb.LoadPrinterSettings();

            if (pdlg.ShowDialog() == DialogResult.OK)
            {
                tb.SavePrinterSettings(pdlg.PrinterSettings);
            }
        }

        private void tsmiSchemesSetup_Click(object sender, EventArgs e)
        {
            SchemesSetupForm ssform = new SchemesSetupForm();
            ssform.ShowDialog();
        }

        #endregion

        private void dgvJournal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                if (e.Control) tsmiPrintDoc_Click(null, null);
                else tsmiPreviewDoc_Click(null, null);
            }
        }


    
    }



}
