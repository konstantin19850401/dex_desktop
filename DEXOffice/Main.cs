using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MySql.Data.MySqlClient;
using DEXExtendLib;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

namespace DEXOffice
{
    public partial class Main : Form
    {
        #region Инициализация и деинициализация

        BindingSource bsLog;
        DataTable dtLog;

        public Main(bool SMIgnoreDatesInterval, bool SMClearSearchSettings, bool SMClearPrinterSettings)
        {
            InitializeComponent();

            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            DEXToolBox tb = DEXToolBox.getToolBox();

            dgvLog.AutoGenerateColumns = false;

            bsLog = new BindingSource();
            dgvLog.DataSource = bsLog;

            dtLog = new DataTable();
            dtLog.Columns.Add("time", typeof(string));
            dtLog.Columns.Add("text", typeof(string));
            dtLog.Columns.Add("color", typeof(Color));

            bsLog.DataSource = dtLog;
            Program.log("Проверка системного пользователя");

            // Проверка системного пользователя
            // * login: SYSTEM pass:- uid: SYSTEM title: Система 
            // * datecreated/datechanged: 00000000000000
            // * settings:- status: 0
            DataTable tu = tb.getQuery("select * from `users` where login='SYSTEM'");
            if (tu == null || tu.Rows.Count < 1)
            {
                MySqlCommand cmd = new MySqlCommand(
                    "insert into `users` (login, pass, uid, title, datecreated, datechanged, " +
                    "settings, status) values (@login, @pass, @uid, @title, @datecreated, " +
                    "@datechanged, @settings, @status)", tb.getConnection());
                cmd.Parameters.AddWithValue("login", "SYSTEM");
                cmd.Parameters.AddWithValue("pass", " ");
                cmd.Parameters.AddWithValue("uid", "SYSTEM");
                cmd.Parameters.AddWithValue("title", "Система");
                cmd.Parameters.AddWithValue("datecreated", "00000000000000");
                cmd.Parameters.AddWithValue("datechanged", "00000000000000");
                cmd.Parameters.AddWithValue("settings", " ");
                cmd.Parameters.AddWithValue("status", 0);
                cmd.ExecuteNonQuery();
            }



            // Регистрация справочников в меню
            Program.log("Регистрация справочников в меню");

            int totalbtn = 0;

            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DICTIONARY, miDictionaries,
                new EventHandler(DictionaryCall_Click), false
                );
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DICTIONARY, miSetupDictionaries,
                new EventHandler(SetupDictionary_Click), true
                );

            totalbtn += tb.Plugins.FillButtons(PluginFramework.PLUGIN_TYPE_DICTIONARY, tsMainButtons,
                new EventHandler(DictionaryCall_Click), false);
            

            miSetupDictionaries.Visible = miSetupDictionaries.HasDropDownItems;
            miDictionaries.Visible = miDictionaries.HasDropDownItems;

            // Регистрация документов в меню
            Program.log("Регистрация документов в меню");
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DOCUMENT, tsddbDocuments,
                new EventHandler(NewDocument_Click), false
                );
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_DOCUMENT, miSetupDocuments,
                new EventHandler(SetupDictionary_Click), true
                );

            // Регистрация отчётов в меню
            Program.log("Регистрация отчётов в меню");
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_REPORT, miReports,
                new EventHandler(Report_Click), false
                );
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_REPORT, miSetupReports,
                new EventHandler(SetupDictionary_Click), true
                );
            miSetupReports.Visible = miSetupReports.HasDropDownItems;

            if (tb.Plugins.getReports().Count > 0 && totalbtn > 0)
            {
                tsMainButtons.Items.Add(new ToolStripSeparator());
            }
            totalbtn += tb.Plugins.FillButtons(PluginFramework.PLUGIN_TYPE_REPORT, tsMainButtons,
                new EventHandler(Report_Click), false
                );

            // Регистрация функций в меню
            Program.log("Регистрация функций в меню");
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_FUNCTION, miFunctions,
                new EventHandler(Function_Click), false
                );

            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_FUNCTION, miSetupFunctions,
                new EventHandler(SetupDictionary_Click), true
                );
            miSetupFunctions.Visible = miSetupFunctions.HasDropDownItems;
            miFunctions.Visible = miFunctions.HasDropDownItems;

            if (tb.Plugins.getFunctions().Count > 0 && totalbtn > 0)
            {
                tsMainButtons.Items.Add(new ToolStripSeparator());
            }
            totalbtn += tb.Plugins.FillButtons(PluginFramework.PLUGIN_TYPE_FUNCTION, tsMainButtons,
                new EventHandler(Function_Click), false
                );


            // Регистрация задач планировщика в меню
            Program.log("Регистрация задач планировщика в меню");
            tb.Plugins.FillMenu(PluginFramework.PLUGIN_TYPE_SCHEDULE, miSetupSchedules,
                new EventHandler(SetupDictionary_Click), true
                );
            miSetupSchedules.Visible = miSetupSchedules.HasDropDownItems;

            // Регистрация документов в кнопке
            Program.log("Регистрация документов в кнопке");
            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Name = "tsDocControlDocSeparator";
            tsDocControl.Items.Insert(0, sep);

            tb.Plugins.FillButtons(PluginFramework.PLUGIN_TYPE_DOCUMENT, tsDocControl,
                new EventHandler(NewDocument_Click), true
                );

//            miSettings.Visible = false;
            miSetupDocuments.Visible = miSetupDocuments.HasDropDownItems;
            miReports.Visible = miReports.HasDropDownItems;

            bool mvis = false;

            foreach (ToolStripMenuItem tsi in miSettings.DropDownItems)
            {
                if (tsi.HasDropDownItems) mvis = true;
            }

            miSettings.Visible = mvis;


            Program.log("RefreshNavControls();");
            RefreshNavControls();

            IDEXConfig cfg = (IDEXConfig)tb;

            WindowState = (FormWindowState)cfg.getInt("MainForm", "WindowState", (int)FormWindowState.Normal);
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;

            Program.log("WindowState = " + WindowState.ToString());

            int i = (cfg.getBool("MainForm", "NavOpened", false)) ? 1 : 0;
            bNavOpenClose.Tag = i;
            bNavOpeClose_Click(bNavOpenClose, null);

            if (Program.SAFE_MODE || SMClearSearchSettings)
            {
                tbDocSearch.Text = "";
                cbSearchMethod.SelectedIndex = 0;
                cbRecLimit.SelectedIndex = 0;
            }
            else
            {
                tbDocSearch.Text = cfg.getStr("MainForm", "tbDocSearch_Text", "");
                cbSearchMethod.SelectedIndex = cfg.getInt("MainForm", "cbSearchMethod_SelectedIndex", 0);
                cbRecLimit.SelectedIndex = cfg.getInt("MainForm", "cbRecLimit_SelectedIndex", 0);
            }

            if (Program.SAFE_MODE || SMIgnoreDatesInterval)
            {
                cbDateTo.Checked = false;
            }
            else
            {
                cbDateTo.Checked = cfg.getBool("MainForm", "cbDateTo_Checked", false);
            }

            cbDateTo_CheckStateChanged(null, null);

            if (!cbDateTo.Checked)
            {
                dtpDateFrom.Value = DateTime.Now;
                dtpDateTo.Value = DateTime.Now;
            }
            else
            {
                dtpDateFrom.Value = cfg.getDate("MainForm", "dtpDateFrom_Value", DateTime.Now);
                dtpDateTo.Value = cfg.getDate("MainForm", "dtpDateTo_Value", DateTime.Now);
            }

            string did = cfg.getStr("MainForm", "cbDocTypeSelectedIndex", "ANY");
            cbDocType.SelectedIndex = DocumentTypes.IndexOf(did);

            int uid = cfg.getInt("MainForm", "cbUnitSelectedIndex", -1);
            cbUnit.SelectedIndex = Units.IndexOf(uid);

            MultiUnitsForm.selectedIds.Clear();
            try
            {
                string[] sMultiIds = cfg.getStr("MainForm", "sMultiIds", "").Split('|');
                if (sMultiIds != null && sMultiIds.Length > 0)
                {
                    foreach (string ss in sMultiIds)
                    {
                        try
                        {
                            MultiUnitsForm.selectedIds.Add(Convert.ToInt32(ss));
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }

            if (SMClearPrinterSettings) DEXToolBox.getToolBox().ClearPrinterSettings();

            Program.log("journalColumns = new Dictionary<string, string>();");
            journalColumns = new Dictionary<string, string>();
            Dictionary<string, string> aAll = DEXToolBox.getToolBox().Plugins.getDocumentFields(null);
            journalFieldsAddSystemFields(aAll);

            try
            {
                string[] flds = cfg.getStr("MainForm", "Columns", "").Split(new char[] { '|' });
                foreach (string k in flds)
                {
                    if (aAll.ContainsKey(k))
                    {
                        journalColumns[k] = aAll[k];
                    }
                }
            }
            catch (Exception)
            {
            }

            cbFilterImmediate.Checked = cfg.getBool("MainForm", "cbFilterImmediate", false);

            tb.StartSchedule();

            if (journalColumns.Count < 1) journalFieldsAddSystemFields(journalColumns);

            string cfgname = tb.getRegisterStr("config_name", "");
            journalType = (DEXJournalType)cfg.getInt("MainForm", "JournalType", (int)DEXJournalType.JOURNAL);
            switchJournalMode();

            pLog.Height = cfg.getInt("MainForm", "pLog_Height", pJournal.Height);

            Text = "Журнал документов";
            if (cfgname != "") Text = "[" + (Program.SAFE_MODE ? "!" : "") + cfgname + "] " + Text;

           
            //object plg = null;
            //if ( IDEXDocumentPlans plg is IDEXDocumentPlans )
            //{
            //    ( (IDEXDocumentPlans)plg ).setPlans(tb);
            //}

            //IDEXDocumentPlans plg;
            //Object sss = new Object();
            //IDEXPlans test = (IDEXPlans)Activator.CreateInstance();
            //IDEXDocumentPlans ssss = (IDEXDocumentPlans)Activator.CreateInstance(Type curi);

            Program.log("InitJournal();");
            InitJournal();
            Program.log("_journal(0);");
            _journal(0);
            Program.log("_journal(0); /");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Быстрое подтверждение
            if (keyData == Keys.F2)
            {
                tsbApprove_Click(tsbApprove, new EventArgs());
                return true;
            }

            // Клавиши быстрого вызова документов
            Keys[] fk = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };

            ArrayList dp = DEXToolBox.getToolBox().Plugins.getDocuments();

            for (int i = 0; i < fk.Length; ++i)
            {
                if (tsDocControl.Items.Count > i && (keyData == (fk[i] | Keys.Control)))
                {
                    try
                    {
                        tsDocControl.Items[i].PerformClick();
//                        NewDocument_Click(tsddbDocuments.DropDownItems[i], new EventArgs());
                        return true;
                    }
                    catch (Exception) { }
                }
            }

            if (keyData == Keys.Delete)
            {
                tsbDeleteDocument_Click(tsbDeleteDocument, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Console.WriteLine("OnApplicationExit");
            try
            {
                IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();

                cfg.setInt("MainForm", "WindowState", (int)WindowState);
                int i = (int)bNavOpenClose.Tag;
                cfg.setBool("MainForm", "NavOpened", i == 1);
                cfg.setStr("MainForm", "tbDocSearch_Text", tbDocSearch.Text);
                cfg.setInt("MainForm", "cbSearchMethod_SelectedIndex", cbSearchMethod.SelectedIndex);
                cfg.setInt("MainForm", "cbRecLimit_SelectedIndex", cbRecLimit.SelectedIndex);
                cfg.setBool("MainForm", "cbDateTo_Checked", cbDateTo.Checked);
                cfg.setDate("MainForm", "dtpDateFrom_Value", dtpDateFrom.Value);
                cfg.setDate("MainForm", "dtpDateTo_Value", dtpDateTo.Value);

                if (Units != null && Units.Count > 0 && cbUnit.SelectedIndex > -1)
                {
                    try
                    {
                        int uid = (int)Units[cbUnit.SelectedIndex];
                        cfg.setInt("MainForm", "cbUnitSelectedIndex", uid);

                        string sids = "";
                        foreach (int ii in MultiUnitsForm.selectedIds)
                        {
                            if (sids.Length > 0) sids += "|";
                            sids += ii.ToString();
                        }

                        cfg.setStr("MainForm", "sMultiIds", sids);
                    }
                    catch (Exception) { }
                }

                if (DocumentTypes != null && DocumentTypes.Count > 0 && cbDocType.SelectedIndex > -1)
                {
                    try
                    {
                        string did = (string)DocumentTypes[cbDocType.SelectedIndex];
                        cfg.setStr("MainForm", "cbDocTypeSelectedIndex", did);
                    }
                    catch (Exception) { }
                }

                cfg.setBool("MainForm", "cbFilterImmediate", cbFilterImmediate.Checked);
                cfg.setInt("MainForm", "JournalType", (int)journalType);

                cfg.setInt("MainForm", "pLog_Height", pLog.Height);

                IDEXData d = (IDEXData)DEXToolBox.getToolBox();
                d.runQuery(string.Format("update `journal` set locked = '', locktime = '' where locked = '{0}'",
                    d.EscapeString(DEXToolBox.getToolBox().sUID)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                DEXToolBox.getToolBox().getConnection().Close();
            }
            catch (Exception) { }
            Console.WriteLine("OnApplicationExit /");
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
//OnApplicationExit
        }

        #endregion

        #region Работа со справочниками

        private void DictionaryCall_Click(object sender, EventArgs e)
        {
            // Вызов пункта меню справочника
            if (((ToolStripItem)sender).Tag is IDEXPluginDictionary)
            {
                IDEXPluginDictionary caller = (IDEXPluginDictionary)((ToolStripItem)sender).Tag;
                caller.ShowDictionary(DEXToolBox.getToolBox());
            }
        }

        private void SetupDictionary_Click(object sender, EventArgs e)
        {
            // Вызов пункта меню справочника (настройки)
            if (((ToolStripItem)sender).Tag is IDEXPluginSetup)
            {
                DEXToolBox.getToolBox().SuspendSchedule();
                IDEXPluginSetup caller = (IDEXPluginSetup)((ToolStripItem)sender).Tag;
                caller.Setup(DEXToolBox.getToolBox());
                DEXToolBox.getToolBox().ContinueSchedule();
            }
        }

        #endregion

        #region Работа с навигационной панелью

        ArrayList DocumentTypes;
        ArrayList Units;

        private void RefreshNavControls()
        {
            DEXToolBox tb = DEXToolBox.getToolBox();

            DocumentTypes = new ArrayList();
            cbDocType.Items.Clear();

            DocumentTypes.Add("ANY");
            cbDocType.Items.Add("[Любой]");

            ArrayList docs = tb.Plugins.getDocuments();
            if (docs != null && docs.Count > 0)
            {
                foreach (IDEXPluginDocument doc in docs)
                {
                    cbDocType.Items.Add(doc.Title);
                    DocumentTypes.Add(doc.ID);
                }
            }

            Units = new ArrayList();
            cbUnit.Items.Clear();

            Units.Add(-2);
            cbUnit.Items.Add("[Множественный выбор]");

            Units.Add(-1);
            cbUnit.Items.Add("[Любое]");

            DataTable tbu = tb.getQuery("select * from `units` order by title");

            // получим тарифные планы
            DataTable dtPlans = tb.getQuery("select `title` from `um_plans`");
            ArrayList tplans = new ArrayList();
            if ( dtPlans != null && dtPlans.Rows.Count > 0 )
            {
                foreach(DataRow dr in dtPlans.Rows) 
                {
                        tplans.Add(dr["title"]);
                }
            }
            
            try
            {
                // добавление статусов в cbStatus
                cbStatus.Items.Clear();
                cbStatus.Items.Add("[Любой]");
                cbStatus.SelectedIndex = 0;
                string[] comboStatus = DEXToolBox.DOCUMENT_STATE_TEXT;
                cbStatus.Items.AddRange(comboStatus);

                // добавление тарифных планов в cbPlans
                cbPlans.Items.Clear();
                cbPlans.Items.Add("[Любой]");
                cbPlans.SelectedIndex = 0;
                foreach ( string arrayItem in tplans )
                {
                    cbPlans.Items.Add(arrayItem);
                }
            }
            catch (Exception) { }

            // добавление в фильтр метода создания документа
            cbTypeCreateDoc.Items.Add("[Любой]");
            cbTypeCreateDoc.Items.Add("[Автоматически]");
            cbTypeCreateDoc.Items.Add("[Вручную]");
            cbTypeCreateDoc.SelectedIndex = 0;

            if (tbu != null && tbu.Rows.Count > 0)
            {
                foreach (DataRow r in tbu.Rows)
                {
                    try
                    {
                        int uid = int.Parse(r["uid"].ToString());
                        Units.Add(uid);
                        string uname = r["title"].ToString();
                        cbUnit.Items.Add(uname);
                    }
                    catch (Exception) { }
                }
            }


            // добавление в фильтр статуса наличия скана
            cbScan.Items.Add("[Любой]");
            cbScan.Items.Add("[Приложен]");
            cbScan.Items.Add("[Не приложен]");
            cbScan.SelectedIndex = 0;

        }

        private void bNavOpeClose_Click(object sender, EventArgs e)
        {
            int i = (int)bNavOpenClose.Tag;
            if (e != null)
            {
                i = 1 - i;
                bNavOpenClose.Tag = i;
            }
            if (i == 1)
            {
                pNav.Height = 86;
                bNavOpenClose.Text = "<";
            }
            else if (i == 0)
            {
                pNav.Height = 30;
                bNavOpenClose.Text = ">";
            }
        }

        private void cbDateTo_CheckStateChanged(object sender, EventArgs e)
        {
            dtpDateTo.Enabled = cbDateTo.Checked;
        }

        private void bNavSetFilter_Click(object sender, EventArgs e)
        {
            _journal(0);
        }

        private void cbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterImmediate.Checked) _journal(lastPageNum);
        }

        private void tbDocSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return && cbFilterImmediate.Checked) _journal(lastPageNum);
        }

        #endregion

        #region Работа с таблицей журнала

        Dictionary<string, string> IdsUnits;
        Dictionary<string, string> IdsUsers;

        MySqlDataAdapter adaJournal;
        BindingSource bsJournal;
        DataTable dtJournal;

        string journalTable;

        Dictionary<string, string> journalColumns;
        int lastPageNum = -1;
        bool dgvJournalInitMode = true;

        DEXJournalType journalType = DEXJournalType.JOURNAL;

        /// <summary>
        /// Обновление виртуальных столбцов в таблице dtJournal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        void dtJournalUpdateAddRow(DataRow r)
        {
            DEXToolBox tb = DEXToolBox.getToolBox();

            DataRowState oldState = r.RowState;

            r.BeginEdit();
            try
            {
                SimpleXML sdoc = SimpleXML.LoadXml(r["data"].ToString());
                IDEXPluginDocument pdoc = tb.Plugins.getDocumentByID(r["docid"].ToString());
                r["vdoctype"] = pdoc.Title;
             
                try
                {
                    r["vdocdate"] = r["jdocdate"].ToString().Substring(0, 8);
                }
                catch (Exception)
                {
                }

                Dictionary<string, string> dct = new Dictionary<string, string>();
                try
                {
                    DataTable dt = tb.getQuery("select * from `sex`");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dct[dr["iid"].ToString()] = dr["title"].ToString();
                        }
                    }
                }
                catch (Exception)
                {
                }

                string unitid = r["unitid"].ToString();
                if (IdsUnits.ContainsKey(unitid)) r["unittitle"] = IdsUnits[unitid];
                else r["unittitle"] = "[" + unitid + "]";

                string userid = r["userid"].ToString();
                if (IdsUsers.ContainsKey(userid)) r["usertitle"] = IdsUsers[userid];
                else r["usertitle"] = "[" + userid + "]";

                foreach (KeyValuePair<string, string> kvp in journalColumns)
                {
                    if (kvp.Key.Length > 0 && kvp.Key[0] != '#')
                    {
                        //string ssss;
                        string vv = sdoc[kvp.Key].Text;
                        if (sdoc[kvp.Key].Attributes.ContainsKey("printable")) vv = sdoc[kvp.Key].Attributes["printable"];
                        if ( kvp.Key == "Unittitlesim" )
                        {

                            try
                            {
                                if (sdoc.GetNodeByPath("MSISDN", true).Text == "")
                                {
                                    //ssss = sdoc.GetNodeByPath("ICC", true).Text;
                                    DataTable dtUnitTitle = tb.getQuery("select `title` from `units` where uid = (select `owner_id` from `um_data` where icc = {0})", sdoc.GetNodeByPath("ICC", true).Text);
                                    foreach (DataRow dr in dtUnitTitle.Rows)
                                    {
                                        vv = dr["title"].ToString();
                                    }
                                }
                                else
                                {
                                    //ssss = sdoc.GetNodeByPath("MSISDN", true).Text;
                                    DataTable dtUnitTitle = tb.getQuery("select `title` from `units` where uid = (select `owner_id` from `um_data` where msisdn = {0})", sdoc.GetNodeByPath("MSISDN", true).Text);
                                    foreach (DataRow dr in dtUnitTitle.Rows)
                                    {
                                        vv = dr["title"].ToString();
                                    }
                                }
                            }
                            catch
                            {
                                vv = "";
                            }
                            
                        }
                        if (kvp.Key == "Sex" )
                        {
                            if (dct.Count > 0)
                            {
                                vv = dct[sdoc.GetNodeByPath("Sex", true).Text];
                            }
                        }
                        if (pdoc != null)
                        {
                      
                            vv = pdoc.GetFieldValueText(tb, kvp.Key, vv);
                        }

                        // Добавлено 01.11.2016 выставление тайтла плана вместо id(блок можно удалить и тогда будет plan_id вместо title)
                        if (kvp.Key == "Plan")
                        {
                            DataTable dt = tb.getQuery("select `title` from `um_plans` where plan_id = '"+vv+"'");
                            if (dt == null)
                            {
                                dt = tb.getQuery("select `title` from `um_plans_gf` where plan_id = '" + vv + "'");
                            }
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                vv = dt.Rows[0]["title"].ToString();
                            }
                        
                        }
                        

                        // это было
                        r[kvp.Key] = vv;
                    }
                }
            }
            catch (Exception)
            {
            }
            r.EndEdit();
            if (oldState == DataRowState.Unchanged) r.AcceptChanges();

        }

        /// <summary>
        /// Инициализация таблицы, вида и адаптера журнала
        /// </summary>
        void InitJournal()
        {
            journalTable = journalType == DEXJournalType.JOURNAL ? "journal" : "archive";

            dgvJournalInitMode = true;
            bsJournal = new BindingSource();
            dgvJournal.DataSource = bsJournal;
            dtJournal = new DataTable();
            bsJournal.DataSource = dtJournal;

            MySqlConnection cn = DEXToolBox.getToolBox().getConnection();

            adaJournal = new MySqlDataAdapter();

            // select command (fake)
            adaJournal.SelectCommand = new MySqlCommand(string.Format("select * from `{0}`", journalTable) , cn);

            // insert command
            adaJournal.InsertCommand = new MySqlCommand(string.Format(
                "insert into `{0}` (locked, locktime, userid, status, signature, jdocdate, " +
                "unitid, docid, digest, data, journal) values (@locked, @locktime, @userid, " +
                "@status, @signature, @jdocdate, @unitid, @docid, @digest, @data, @journal)", journalTable),
                cn
                );
            MySqlParameterCollection pc = adaJournal.InsertCommand.Parameters;
            pc.Add("locked", MySqlDbType.VarChar, 32, "locked");
            pc.Add("locktime", MySqlDbType.VarChar, 14, "locktime");
            pc.Add("userid", MySqlDbType.VarChar, 32, "userid");
            pc.Add("status", MySqlDbType.Int64, 11, "status");
            pc.Add("signature", MySqlDbType.VarChar, 25, "signature");
            pc.Add("jdocdate", MySqlDbType.VarChar, 17, "jdocdate");
            pc.Add("unitid", MySqlDbType.Int32, 8, "unitid");
            pc.Add("docid", MySqlDbType.VarChar, 64, "docid");
            pc.Add("digest", MySqlDbType.VarChar, 255, "digest");
            pc.Add("data", MySqlDbType.MediumText, 0, "data");
            pc.Add("journal", MySqlDbType.MediumText, 0, "journal");


            // update command
            adaJournal.UpdateCommand = new MySqlCommand(string.Format(
                "update `{0}` set locked=@locked, locktime=@locktime, userid=@userid, status=@status, " +
                "signature=@signature, jdocdate=@jdocdate, unitid=@unitid, docid=@docid, digest=@digest, " +
                "data=@data, journal=@journal where id=@id", journalTable),
                cn
                );
            pc = adaJournal.UpdateCommand.Parameters;
            pc.Add("locked", MySqlDbType.VarChar, 32, "locked");
            pc.Add("locktime", MySqlDbType.VarChar, 14, "locktime");
            pc.Add("userid", MySqlDbType.VarChar, 32, "userid");
            pc.Add("status", MySqlDbType.Int64, 11, "status");
            pc.Add("signature", MySqlDbType.VarChar, 25, "signature");
            pc.Add("jdocdate", MySqlDbType.VarChar, 17, "jdocdate");
            pc.Add("unitid", MySqlDbType.Int32, 8, "unitid");
            pc.Add("docid", MySqlDbType.VarChar, 64, "docid");
            pc.Add("digest", MySqlDbType.VarChar, 255, "digest");
            pc.Add("data", MySqlDbType.MediumText, 0, "data");
            pc.Add("journal", MySqlDbType.MediumText, 0, "journal");
            pc.Add("id", MySqlDbType.Int64, 14, "id").SourceVersion = DataRowVersion.Original;

            // delete command
            adaJournal.DeleteCommand = new MySqlCommand(string.Format("delete from `{0}` where id=@id", journalTable), cn);
            pc = adaJournal.DeleteCommand.Parameters;
            pc.Add("id", MySqlDbType.Int64, 14, "id").SourceVersion = DataRowVersion.Original;

            ////

            dgvJournal.AutoGenerateColumns = false;
            dgvJournal.DataSource = bsJournal;
            
            dgvJournal.VirtualMode = false;
            status.DataPropertyName = "status";
            vdocdate.DataPropertyName = "vdocdate";
            unittitle.DataPropertyName = "unittitle";
            usertitle.DataPropertyName = "usertitle";
            vdoctype.DataPropertyName = "vdoctype";
            digest.DataPropertyName = "digest";

            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            dgvJournal.Columns.Clear();
            if (journalColumns != null && journalColumns.Count > 0)
            {
                ArrayList cols = new ArrayList();
                foreach (KeyValuePair<string, string> kvp in journalColumns)
                {
                    string k = kvp.Key;
                    if (k.Length > 0)
                    {
                        if (k.Substring(0, 1).Equals("#"))
                        {
                            // Системное поле
                            k = k.Substring(1).Trim();
                            if (k.Equals("status")) cols.Add(status);
                            if (k.Equals("vdocdate")) cols.Add(vdocdate);
                            if (k.Equals("unittitle")) cols.Add(unittitle);
                            if (k.Equals("usertitle")) cols.Add(usertitle);
                            if (k.Equals("vdoctype")) cols.Add(vdoctype);
                            if (k.Equals("digest")) cols.Add(digest);
                        }
                        else
                        {
                            //Виртуальное поле
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                            col.HeaderText = kvp.Value;
                            col.Name = k;
                            col.ReadOnly = true;
                            col.SortMode = DataGridViewColumnSortMode.Programmatic;
                            col.DataPropertyName = k;
                            cols.Add(col);
                            dtJournal.Columns.Add(k, typeof(string));
                        }
                    }
                }

                dtJournal.Columns.Add("vdoctype", typeof(string));
                dtJournal.Columns.Add("unittitle", typeof(string));
                dtJournal.Columns.Add("usertitle", typeof(string));

                if (cols.Count > 0)
                {
                    int di = 0;
                    foreach (DataGridViewTextBoxColumn col in cols)
                    {
                        col.Width = cfg.getInt("dgvJournal", col.Name, 32);
                        col.DisplayIndex = di++;
                        col.SortMode = DataGridViewColumnSortMode.Automatic;
                        dgvJournal.Columns.Add(col);
                    }
                }
            }

            dgvJournalInitMode = false;

        }

        /// <summary>
        /// Подготовка выпадалки со страницами журнала документов
        /// </summary>
        /// <param name="rcnt"></param>
        /// <param name="lim"></param>
        /// <param name="pageNum"></param>
        void PreparePagination(int rcnt, int lim, int pageNum)
        {
            try
            {

                tsddbPagination.DropDownItems.Clear();
                tsddbPagination.Visible = false;

                if (lim > -1)
                {
                    int pgcnt = 0;
                    if (rcnt > 0)
                    {
                        pgcnt = rcnt / lim;
                    }

                    int pgbase = 0, pgnum = 1;
                    if (pgcnt > 0)
                    {
                        for (int f = 0; f < pgcnt; ++f)
                        {
                            ToolStripMenuItem tsmi = new ToolStripMenuItem();
                            tsmi.Text = string.Format("Страница {0} (с {1} по {2})",
                                pgnum, pgbase + 1, pgbase + lim);

                            if (pgnum - 1 == pageNum) tsddbPagination.Text = tsmi.Text;

                            tsmi.Name = string.Format("tsmiPagination{0}", pgnum);
                            tsmi.Tag = pgnum - 1;
                            tsmi.Click += paginationMenuItem_Click;
                            tsddbPagination.DropDownItems.Add(tsmi);
                            pgbase += lim;
                            pgnum++;
                        }
                        tsddbPagination.Visible = true;
                    }

                    if (rcnt % lim > 0)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem();
                        tsmi.Text = string.Format("Страница {0} (с {1} по {2})",
                            pgnum, pgbase + 1, rcnt);

                        if (pgnum - 1 == pageNum) tsddbPagination.Text = tsmi.Text;

                        tsmi.Name = string.Format("tsmiPagination{0}", pgnum);
                        tsmi.Tag = pgnum - 1;
                        tsmi.Click += paginationMenuItem_Click;
                        tsddbPagination.DropDownItems.Add(tsmi);

                        tsddbPagination.Visible = true;
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Выборка из журнала
        /// </summary>
        /// <param name="pageNum"></param>
        void _journal(int pageNum)
        {
            Program.log("_journal(" + pageNum.ToString() + ")");
            DEXToolBox tb = DEXToolBox.getToolBox();

            IdsUnits = new Dictionary<string, string>();

            DataTable dtl = tb.getQuery("select uid, title from `units`");
            if (dtl != null && dtl.Rows.Count > 0)
            {
                foreach (DataRow rw in dtl.Rows)
                {
                    IdsUnits[rw["uid"].ToString()] = rw["title"].ToString();
                }
            }
            Program.log("IdsUsers");
            IdsUsers = new Dictionary<string, string>();
            dtl = tb.getQuery("select uid, title from `users`");
            if (dtl != null && dtl.Rows.Count > 0)
            {
                foreach (DataRow rw in dtl.Rows)
                {
                    IdsUsers[rw["uid"].ToString()] = rw["title"].ToString();
                }
            }

            string sql = string.Format("from `{0}` as j where 1=1 ", journalTable);

            string sqldata =
                "select j.id, j.locked, j.locktime, j.userid, j.status, j.signature, " +
                "j.jdocdate, j.unitid, j.docid, j.digest, " +
                "substring(j.jdocdate, 1, 8) as vdocdate, " +
                "j.data, j.journal, j.importhash ";

            string sqlcnt = "select count(j.id) as cid ";

            //bool ifSelectReg = false;

            if (tb.GetRightsItem(DEXToolBox.RESTRICTED_KEY))
            {
                sql += string.Format("and j.userid = '{0}' ", tb.sUID);
            }
            try
            {
                // добавление в фильтр статуса
                int selectedStatus = (int)cbStatus.SelectedIndex - 1;
                sql += selectedStatus < 0 ? "" : string.Format("and j.status = '{0}' ", selectedStatus);

                // добавление в фильтр плана
                int selectedPlan = (int)cbPlans.SelectedIndex - 1;
                //string planText = cbPlans.SelectedItem.ToString().Replace("'", "&amp;#39;");
                string planText = cbPlans.SelectedItem.ToString().Replace("'", "").Trim();
                planText = planText.Replace("&", "&amp;").Trim();
                //sql += selectedPlan < 0 ? "" : string.Format("and j.data like '%<PlanPrn>{0}<%'", planText);
                sql += selectedPlan < 0 ? "" : string.Format("and j.data like '%{0}%'", planText);


                string reg = "(SELECT region_id from `um_data` AS um WHERE um.msisdn = j.digest+0) in (";
                if (MultiIgnorRegForm.selectedIdReg.Count > 0)
                {
                    sql += "and (";
                    string[] indexReg;
                    string[] nameReg;
                    foreach (string soti in MultiIgnorRegForm.selectedIdReg)
                    {
                        nameReg = soti.Split(',');
                        indexReg = nameReg[0].Split('-');
                        int start = Convert.ToInt32(indexReg[0]);
                        int end = indexReg.Length > 1 ? Convert.ToInt32(indexReg[1]) : Convert.ToInt32(indexReg[0]);
                        reg += string.Format("'{0}',", nameReg[1]);
                    }
                    sql += string.Format("{0}))", reg.Substring(0, reg.Length - 1));
                }


                // добавление в фильтр метода создания документа
                int selectedTypeCreateDoc = (int)cbTypeCreateDoc.SelectedIndex;
                string[] arr = { "", "regexp '<text>Документ сформирован функцией формирования группы документов<'", "not regexp '<text>Документ сформирован функцией формирования группы документов<'" };
                string selected = arr[selectedTypeCreateDoc];
                sql += selectedTypeCreateDoc < 1 ? "" : string.Format("and j.journal {0}", selected);

                
               
                
                
                
                // добавление в фильтр игнорируемых субъектов
                
                /*
                string reg = ""; 
                if (MultiIgnorRegForm.selectedIdReg.Count > 0) {
                    sql += "and (";
                    string[] indexReg;
                    string[] nameReg;
                    foreach ( string soti in MultiIgnorRegForm.selectedIdReg )
                    {
                        nameReg = soti.Split(',');
                        indexReg = nameReg[0].Split('-');
                        int start = Convert.ToInt32(indexReg[0]);
                        int end = indexReg.Length > 1 ? Convert.ToInt32(indexReg[1]) : Convert.ToInt32(indexReg[0]);
                        reg += string.Format("((SELECT region_id from `um_data` AS um WHERE um.msisdn = j.digest+0) = '{0}') |", nameReg[1]);
                    }
                    sql += string.Format("{0})", reg.Substring(0, reg.Length -1));
                }
                */
                /*
                string reg = "(SELECT region_id from `um_data` AS um WHERE um.msisdn = j.digest+0) in (";
                if (MultiIgnorRegForm.selectedIdReg.Count > 0)
                {
                    sql += "and (";
                    string[] indexReg;
                    string[] nameReg;
                    foreach (string soti in MultiIgnorRegForm.selectedIdReg)
                    {
                        nameReg = soti.Split(',');
                        indexReg = nameReg[0].Split('-');
                        int start = Convert.ToInt32(indexReg[0]);
                        int end = indexReg.Length > 1 ? Convert.ToInt32(indexReg[1]) : Convert.ToInt32(indexReg[0]);
                        reg += string.Format("'{0}',", nameReg[1]);
                    }
                    sql += string.Format("{0}))", reg.Substring(0, reg.Length - 1));
                }
                */
                /*
                // если выбран регион для фильтра
                string ifSelectRegQuery = "where";
                if (MultiIgnorRegForm.selectedIdReg.Count > 0)
                {
                    ifSelectReg = true;
                    string[] indexReg;
                    string[] nameReg;
                    foreach (string soti in MultiIgnorRegForm.selectedIdReg)
                    {
                        nameReg = soti.Split(',');
                        indexReg = nameReg[0].Split('-');
                        //int start = Convert.ToInt32(indexReg[0]);
                        //int end = indexReg.Length > 1 ? Convert.ToInt32(indexReg[1]) : Convert.ToInt32(indexReg[0]);
                        ifSelectRegQuery += " region_id = '" + nameReg[1] + "' OR";
                        //reg += string.Format("((SELECT region_id from `um_data` AS um WHERE um.msisdn = j.digest+0) = '{0}') |", nameReg[1]);
                    }
                    ifSelectRegQuery = string.Format("{0}", ifSelectRegQuery.Substring(0, ifSelectRegQuery.Length - 2));
                }
                */


            }
            catch (Exception) { }


            #region Условия навигационной панели
            Program.log("Условия навигационной панели");

            if (cbDateTo.Checked)
            {
                DateTime d1 = (dtpDateFrom.Value < dtpDateTo.Value) ? dtpDateFrom.Value : dtpDateTo.Value;
                DateTime d2 = (dtpDateFrom.Value < dtpDateTo.Value) ? dtpDateTo.Value : dtpDateFrom.Value;
                sql += "and j.jdocdate >= '" + d1.ToString("yyyyMMdd") +
                      "000000000' and j.jdocdate <= '" + d2.ToString("yyyyMMdd") +
                       "235959999' ";
                //sql += "and j.jdocdate >= '" + d1.ToString("yyyyMMdd") +
                //       "%' and j.jdocdate <= '" + d2.ToString("yyyyMMdd") +
                //       "%' ";
            }
            else
            {
                sql += "and j.jdocdate like '" + dtpDateFrom.Value.ToString("yyyyMMdd") + "%' ";
            }

            try
            {
                string did = (string)DocumentTypes[cbDocType.SelectedIndex];
                if (did != "ANY")
                {
                    sql += "and j.docid = '" + did.Replace("'", "\\'") + "' ";
                }
            }
            catch (Exception)
            {
            }

            try
            {
                int uid = (int)Units[cbUnit.SelectedIndex];
                if (uid == -2 && MultiUnitsForm.selectedIds.Count > 0)
                {
                    string sids = "";
                    foreach (int i in MultiUnitsForm.selectedIds)
                    {
                        if (sids.Length > 0) sids += ",";
                        sids += i.ToString();
                    }
                    if (sids.Length > 0) sql += "and j.unitid in (" + sids + ") ";
                } 
                else if (uid > -1)
                {
                    sql += string.Format(" and j.unitid = {0} ", uid);
                }
            }
            catch (Exception)
            {
            }

            string search = tbDocSearch.Text.Trim();
            int sm = cbSearchMethod.SelectedIndex;
            if (sm < 0) sm = 0;
            else if (sm > 2) sm = 2;

            if (sm <= 0)
            {
                if (search.Length > 0)
                {
                    sql += string.Format(" and j.data like '%{0}%' ", search.Replace("'", "\\'"));

                }
            }
            else
            {
                string[] searches = search.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (searches != null && searches.Length > 0)
                {
                    string searchsp = (sm == 1) ? " and " : " or ";
                    string sql2 = "";
                    foreach (string csearch in searches)
                    {
                        if (!sql2.Equals("")) sql2 += searchsp;
                        sql2 += string.Format("j.data like '%{0}%'", csearch.Replace("'", "\\'"));
                    }
                    if (!sql2.Equals("")) sql += " and (" + sql2 + ") ";
                }
            }
            #endregion

            // добавление в фильтр проверки наличия скана
            /*
            int selectedScan = (int)cbScan.SelectedIndex;
            if (selectedScan != 0)
            {
                int statusScan = 0;
                if (selectedScan == 1) statusScan = 1;
                sql += string.Format(" and data like '%{0}%'", statusScan);
            }
            */

            // Quering
            sqlcnt += sql;

            #region Разбивка на страницы
            Program.log("Разбивка на страницы");

            // LIMIT POS, COUNT
            int[] lims = { -1, 10000, 5000, 1000, 500, 100, 50, 10, 5 };
            int lim = -1;
            try
            {
                lim = lims[cbRecLimit.SelectedIndex];
            }
            catch (Exception)
            {
                lim = -1;
            }

            if (lim > -1)
            {
                if (pageNum < 0) pageNum = 0;
                sql += string.Format("limit {0}, {1} ", pageNum * lim, lim);
            }

            int rcnt = 0;

            DataTable dtCnt = DEXToolBox.getToolBox().getQuery(sqlcnt);
            if (dtCnt != null)
            {
                try
                {
                    rcnt = int.Parse(dtCnt.Rows[0]["cid"].ToString());
                }
                catch (Exception)
                {
                    rcnt = 0;
                }
            }

            Program.log("PreparePagination");
            PreparePagination(rcnt, lim, pageNum);
            Program.log("PreparePagination /");

            #endregion

            sqldata += sql;

            Program.log("sqldata = " + sqldata);

            adaJournal.SelectCommand.CommandText = sqldata;

            dtJournal.Clear();
            Program.log("adaJournal.Fill(dtJournal);");
            adaJournal.Fill(dtJournal);
            Program.log("adaJournal.Fill(dtJournal); /");

            tsslDocCount.Text = "";

            if (dtJournal.Rows.Count > 0)
            {
                foreach (DataRow r in dtJournal.Rows)
                {
                    dtJournalUpdateAddRow(r);
                }

                tsslDocCount.Text = string.Format("Документов: {0}", dtJournal.Rows.Count);

                tsslStatus.Text = "";
                if (dgvJournal.CurrentRow != null)
                {
                    dgvJournal_RowEnter(dgvJournal, new DataGridViewCellEventArgs(0, dgvJournal.CurrentRow.Index));
                }
            }
            lastPageNum = pageNum;

            dgvJournal.Visible = dtJournal.Rows.Count > 0;
            Program.log("_journal(" + pageNum.ToString() + ") /");
            dgvJournal_SelectionChanged(dgvJournal, null);
        }

        ArrayList JournalSaveSelection()
        {
            ArrayList sel = new ArrayList();
            try
            {
                if (dgvJournal.Visible)
                {
                    foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                    {
                        DataRowView drv = bsJournal[r.Index] as DataRowView;
                        int did = int.Parse(drv["id"].ToString());
                        sel.Add(did);
                    }
                }
            }
            catch (Exception)
            {
            }
            return sel;
        }

        void JournalLoadSelection(ArrayList sel)
        {
            try
            {
                if (dgvJournal.Visible)
                {
                    foreach (DataGridViewRow r in dgvJournal.Rows)
                    {
                        DataRowView drv = bsJournal[r.Index] as DataRowView;
                        int did = int.Parse(drv["id"].ToString());
                        r.Selected = (sel.Contains(did));
                    }

                    if (dgvJournal.SelectedRows.Count > 0)
                    {
                        int cri = dgvJournal.SelectedRows[0].Index;

                        if (cri < dgvJournal.FirstDisplayedScrollingRowIndex ||
                            cri >= dgvJournal.FirstDisplayedScrollingRowIndex + dgvJournal.DisplayedRowCount(false))
                        {
                            dgvJournal.FirstDisplayedScrollingRowIndex = cri;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void paginationMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int pg = (int)((ToolStripMenuItem)sender).Tag;
                _journal(pg);
            }
            catch (Exception)
            {
            
            }
        }

        private void dgvJournal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvJournal.Columns[e.ColumnIndex].Name.Equals("vdocdate"))
            {
                if (e.Value != null)
                {
                    string old = e.Value.ToString();
                    e.Value = string.Format("{0}.{1}.{2}",
                        old.Substring(6, 2), old.Substring(4, 2), old.Substring(0, 4)
                        );
/*
                    e.Value = string.Format("{0}.{1}.{2} {3}:{4}:{5}",
                        old.Substring(6, 2), old.Substring(4, 2), old.Substring(0, 4),
                        old.Substring(8, 2), old.Substring(10, 2), old.Substring(12, 2)
                        );
*/
                }
            }
            else if (dgvJournal.Columns[e.ColumnIndex].Name == "status")
            {
                string[] ss = DEXToolBox.DOCUMENT_STATE_TEXT;
                try
                {
                    e.Value = ss[int.Parse(e.Value.ToString())];
                }
                catch(Exception)
                {
                    e.Value = "-";
                }
            }
        }

        private void dgvJournal_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!dgvJournalInitMode)
            {
                int nw = (e.Column.Width < 10) ? 10 : e.Column.Width;
                IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
                cfg.setInt("dgvJournal", e.Column.Name, nw);
            }
        }

        private void dgvJournal_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
/*            
            ArrayList cols = new ArrayList(dgvJournal.Columns);
            cols.Sort(
            for (int x1 = 0; x1 < cols.Count - 1; x1++)
            {
                for(int
            }

            //TODO Доделать сортировку полей по DisplayIndex

            Dictionary<string, string> order = new Dictionary<string, string>();
            foreach (DataGridViewTextBoxColumn c in dgvJournal.Columns)
            {
                try
                {
                    order[c.Name] = journalColumns[c.Name];
                }
                catch (Exception)
                {
                    try
                    {
                        order["#" + c.Name] = journalColumns["#" + c.Name];
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            journalColumns = order;
            journalFieldsSave();
 */
        }

        private void tsbDataFields_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> aAll = DEXToolBox.getToolBox().Plugins.getDocumentFields(null);
            journalFieldsAddSystemFields(aAll);

            DataFieldsForm dff = new DataFieldsForm(aAll, journalColumns);
            if (dff.ShowDialog() == DialogResult.OK)
            {
                journalColumns = dff.selFields;
                if (journalColumns.Count < 1) journalFieldsAddSystemFields(journalColumns);

                journalFieldsSave();

                InitJournal();
                _journal(lastPageNum);
            }
        }

        private void journalFieldsAddSystemFields(Dictionary<string, string> dic)
        {
            dic["#status"] = "Статус";
            dic["#vdocdate"] = "Дата документа";
            dic["#unittitle"] = "Наименование отделения";
            dic["#usertitle"] = "Пользователь";
            dic["#vdoctype"] = "Тип документа";
            dic["#digest"] = "Дайджест";
        }

        private void journalFieldsSave()
        {
            string jk = "";
            foreach (KeyValuePair<string, string> kvp in journalColumns)
            {
                if (jk.Length > 0) jk += "|";
                jk += kvp.Key;
            }

            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            cfg.setStr("MainForm", "Columns", jk);
        }

        private void dgvJournal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsbEditDocument_Click(null, null);
        }

        private void dgvJournal_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    tsbEditDocument_Click(null, null);
                    break;

                case Keys.Insert:
                    tsddbDocuments.ShowDropDown();
                    break;

                case Keys.Delete:
                    tsbDeleteDocument_Click(null, null);
                    break;
                case Keys.F12:
                    if (e.Control)
                    {
                        tsmiPrintDoc_Click(null, null);
                    }
                    else
                    {
                        tsmiPreviewDoc_Click(null, null);
                    }
                    break;
            }

        }

        private void dgvJournal_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string nstatus = "";
            try
            {
                dtLog.Clear();

                DataRowView row = bsJournal[dgvJournal.Rows[e.RowIndex].Index] as DataRowView;
                SimpleXML xjou = SimpleXML.LoadXml(row["journal"].ToString());
                
                Color[] clrs = { Color.White, Color.LightGray };
                int clr = 0;

                ArrayList recs = xjou.GetChildren("record");
                if (recs != null && recs.Count > 0)
                {
                    SimpleXML lrec = (SimpleXML)recs[recs.Count - 1];
                    
                    nstatus = string.Format("{0} :: {1}", lrec.Attributes["time"], lrec["text"].Text);

                    foreach (SimpleXML rec in recs)
                    {
                        DataRow r = dtLog.NewRow();
                        try
                        {
                            r["time"] = rec.Attributes["time"];
                        }
                        catch (Exception)
                        {
                        }
                        if (rec.GetNodeByPath("text", false) != null)
                            r["text"] = rec["text"].Text;
                        else
                            r["text"] = "(Нет текста)";

                        r["color"] = clrs[clr];
                        dtLog.Rows.Add(r);

                        ArrayList sds = rec.GetChildren("subdata");
                        if (sds != null && sds.Count > 0)
                        {
                            foreach (SimpleXML sd in sds)
                            {
                                r = dtLog.NewRow();
                                r["time"] = "";
                                r["text"] = sd.Text;
                                r["color"] = clrs[clr];
                                dtLog.Rows.Add(r);
                            }
                        }

                        clr = 1 - clr;
                    }
                }
            }
            catch (Exception)
            {
            }
            if (nstatus.Length < 100)
            {
                tsslStatus.Text = nstatus;
            }
            else
            {
                string pp = nstatus.Substring(0, 100);
                pp = pp + "....";
                tsslStatus.Text = pp;
            }
            
        }

        private void tsslStatus_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataRowView)bsJournal.Current).Row;
                SimpleXML jrn = SimpleXML.LoadXml(row["journal"].ToString());
                DocJournalForm djf = new DocJournalForm(jrn);
                djf.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        private void bAllJournal_Click(object sender, EventArgs e)
        {
            DEXToolBox tb = DEXToolBox.getToolBox();
            DataTable t = tb.getQuery("SELECT * FROM `{0}` order by jdocdate limit 0,1", journalTable);
            DateTime stdate = DateTime.MinValue;
            if (t != null && t.Rows.Count > 0)
            {
                try
                {
                    string sd = t.Rows[0]["jdocdate"].ToString();
                    stdate = new DateTime(int.Parse(sd.Substring(0, 4)), int.Parse(sd.Substring(4, 2)), int.Parse(sd.Substring(6, 2)));
                }
                catch (Exception) { }
            }

            DateTime eddate = DateTime.MaxValue;
            t = tb.getQuery("SELECT * FROM `{0}` order by jdocdate desc limit 0,1", journalTable);
            if (t != null && t.Rows.Count > 0)
            {
                try
                {
                    string sd = t.Rows[0]["jdocdate"].ToString();
                    eddate = new DateTime(int.Parse(sd.Substring(0, 4)), int.Parse(sd.Substring(4, 2)), int.Parse(sd.Substring(6, 2)));
                }
                catch (Exception) { }
            }

            if (stdate > DateTime.MinValue && eddate < DateTime.MaxValue)
            {
                dtpDateFrom.Value = stdate;
                dtpDateTo.Value = eddate;
                cbDateTo.Checked = stdate < eddate;
            }
            else
            {
                MessageBox.Show("Невозможно сделать выборку по периоду из за ошибки в БД\nОтображаемая дата начала журнала: " + stdate.ToString() + "\nОтображаемая дата конца журнала: " + eddate.ToString());
            }
        }

        public void switchJournalMode()
        {
            DEXToolBox tb = DEXToolBox.getToolBox();

            foreach (IDEXPluginInfo plg in tb.Plugins.getDictionaries()) plg.setJournalMode(journalType);
            foreach (IDEXPluginInfo plg in tb.Plugins.getDocuments()) plg.setJournalMode(journalType);
            foreach (IDEXPluginInfo plg in tb.Plugins.getFunctions()) plg.setJournalMode(journalType);
            foreach (IDEXPluginInfo plg in tb.Plugins.getJournalhooks()) plg.setJournalMode(journalType);
            foreach (IDEXPluginInfo plg in tb.Plugins.getReports()) plg.setJournalMode(journalType);
            foreach (IDEXPluginInfo plg in tb.Plugins.getSchedules()) plg.setJournalMode(journalType);

            tsddbDocuments.Enabled = journalType == DEXJournalType.JOURNAL;
            tsbDeleteDocument.Enabled = journalType == DEXJournalType.JOURNAL;
            tsbApprove.Enabled = journalType == DEXJournalType.JOURNAL;
            tsbChangeStatus.Enabled = journalType == DEXJournalType.JOURNAL;
            tsbExport.Enabled = journalType == DEXJournalType.JOURNAL;
            tsbChangeStatus.Enabled = journalType == DEXJournalType.JOURNAL;
            tsddbJournalType.Text = journalType == DEXJournalType.JOURNAL ? "Журнал" : "Архив";

            pNav.BackColor = journalType == DEXJournalType.JOURNAL ? SystemColors.Control : Color.FromArgb(0xff, 0xd0, 0xd0);
        }

        private void tsmiJournalModeJournal_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.JOURNAL)
            {
                journalType = DEXJournalType.JOURNAL;
                switchJournalMode();
                InitJournal();
                _journal(0);
            }
        }

        private void tsmiJournalModeArchive_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.ARCHIVE)
            {
                journalType = DEXJournalType.ARCHIVE;
                switchJournalMode();
                InitJournal();
                _journal(0);
            }
        }
        #endregion

        #region Работа с документами

        private void NewDocument_Click(object sender, EventArgs e)
        {
            // Создание нового документа
            if (journalType != DEXJournalType.JOURNAL)
            {
                MessageBox.Show("В режиме архива невозможно создание новых документов.\nПереключитесь в режим журнала.");
                return;
            }

            if (((ToolStripItem)sender).Tag is IDEXPluginDocument)
            {
                DEXToolBox tb = DEXToolBox.getToolBox();
                IDEXPluginDocument doc = (IDEXPluginDocument)((ToolStripItem)sender).Tag;

                CDEXDocumentData newdoc = new CDEXDocumentData();

                //IDEXDocumentPlans sss = (IDEXDocumentPlans)( (ToolStripItem)sender ).Tag;
                //sss.setPlans(tb);

                Random r = new Random();
                int i = r.Next(99999999);
                String newsignature = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT_MS) + i.ToString("D8");
                newdoc.signature = newsignature;

                tb.setCurrentJournal(new SimpleXML("journal"));

                if (doc.NewDocument((object)tb, newdoc))
                {
                     
                    // Проверка criticals
                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_DRAFT)
                    {
                        ArrayList err = tb.checkDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc);
                        if (err != null && err.Count > 0)
                        {
                            tb.AddRecord("Выявлены совпадения с другими документами:");
                            foreach (string eitem in err) tb.AddRecord(eitem);
                            tb.AddRecord("Статус документа понижен.");
                            newdoc.documentStatus = DEXToolBox.DOCUMENT_DRAFT;
                        }
                    }

                    DataRow nr = dtJournal.NewRow();
                    nr.BeginEdit();
                    nr["locked"] = "";
                    nr["locktime"] = "";
                    nr["userid"] = tb.sUID;
                    nr["status"] = newdoc.documentStatus;
                    nr["signature"] = newsignature;
//                    nr["jdocdate"] = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT_MS);
                    Program.log("Main.cs 1153 docdate = " + newdoc.documentDate);
                    nr["jdocdate"] = newdoc.documentDate;
                    nr["unitid"] = newdoc.documentUnitID;
                    nr["docid"] = doc.ID;
                    nr["digest"] = newdoc.documentDigest;
                    nr["data"] = newdoc.documentText;
                    nr["journal"] = SimpleXML.SaveXml(tb.getCurrentJournal());
                    nr.EndEdit();
                    dtJournal.Rows.Add(nr);

                    try
                    {
                        adaJournal.Update(dtJournal);
                    }
                    catch (Exception)
                    {
                    }

                    // Установка DataReferences
                    tb.setDataReference("units", newdoc.documentUnitID.ToString(), true);
                    tb.setDataReference("users", tb.sUID, true);

                    // Установка Criticals
                    tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc, true);

                    // Установка People Data
                    try
                    {
                        StringList slp = doc.GetPeopleData(tb, newdoc);
                        if (slp != null) tb.setPeopleData(slp);
                    }
                    catch (Exception) { }

                    _journal(lastPageNum);

                    try
                    {
                        foreach (DataGridViewRow dgvr in dgvJournal.Rows)
                        {
                            DataRowView drv = bsJournal[dgvr.Index] as DataRowView;
                            if (drv.Row["signature"].ToString().Equals(newsignature))
                            {
                                dgvr.Selected = true;
                                break;
                            }
                        }
                    }
                    catch(Exception)
                    {
                    }


                }

                tb.setCurrentJournal(null);
            }
        }

        private void tsbEditDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvJournal.Visible)
                {
                    ArrayList dsave = JournalSaveSelection();

                    DEXToolBox tb = DEXToolBox.getToolBox();
                    MySqlConnection c = tb.getConnection();

                    DataRow row = ((DataRowView)bsJournal.Current).Row;


                    foreach (DataColumn col in row.Table.Columns)
                    {
                        Program.log(string.Format("Name: {0}, Type: {1}, DTMode: {2}, Value: {3}", col.ColumnName, col.DataType.ToString(), col.DateTimeMode.ToString(), row[col].ToString()));
                    }

                    int rid = int.Parse(row["id"].ToString());
                    string signature = row["signature"].ToString();

                    MySqlCommand cmd = new MySqlCommand(
                        string.Format(
                            "select j.status, j.locked, j.locktime, u.title " +
                            "from `{0}` as j, `users` as u " +
                            "where j.id = {1} and j.locked = u.uid", 
                            journalTable, rid), 
                        c);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string locker_id = "<NONE>", locker_title = "", lock_time = "";
                    try
                    {
                        while (reader.Read())
                        {
                            locker_id = reader.GetString("locked");
                            locker_title = reader.GetString("title");
                            lock_time = reader.GetString("locktime");
                            break;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    cmd = new MySqlCommand(string.Format("select status from `{0}` where id = {1}", journalTable, rid), c);
                    reader = cmd.ExecuteReader();

                    bool isExporting = false, isExported = false;
                    try
                    {
                        while (reader.Read())
                        {
                            int st = reader.GetInt32("status");
                            isExporting = (st == DEXToolBox.DOCUMENT_EXPORTING || st == DEXToolBox.DOCUMENT_TOEXPORT);
                            isExported = (st == DEXToolBox.DOCUMENT_EXPORTED);
                            break;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    if (!isExporting)
                    {
                        if (locker_id.Equals("<NONE>"))
                        {
                            IDEXPluginDocument doc = tb.Plugins.getDocumentByID(row["docid"].ToString());
                            if (doc != null)
                            {

                                row.BeginEdit();
                                row["locked"] = tb.sUID;
                                row["locktime"] = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT);
                                row.EndEdit();
                                adaJournal.Update(dtJournal);

                                //
                                // Редактирование документа
                                //
                                CDEXDocumentData olddoc = new CDEXDocumentData();
                                olddoc.documentDate = row["jdocdate"] as string;
                                Program.log("Main.cs 1281 docdate = " + olddoc.documentDate);
                                olddoc.documentStatus = int.Parse(row["status"].ToString());
                                olddoc.documentText = row["data"].ToString();
                                olddoc.documentUnitID = int.Parse(row["unitid"].ToString());
                                olddoc.signature = signature;

                                CDEXDocumentData newdoc = new CDEXDocumentData();
                                newdoc.signature = signature;

                                tb.setCurrentJournal(SimpleXML.LoadXml(row["journal"].ToString()));

                                string swarn = "";

                                if (doc.EditDocument(tb, olddoc, newdoc, null, (isExported || journalType != DEXJournalType.JOURNAL)) && journalType == DEXJournalType.JOURNAL)
                                {

                                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_TOEXPORT)
                                    {
                                        newdoc.documentStatus = DEXToolBox.DOCUMENT_TOEXPORT;
                                    }

                                    // Проверка criticals
                                    if (newdoc.documentStatus > DEXToolBox.DOCUMENT_DRAFT)
                                    {
                                        ArrayList err = tb.checkDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc);
                                        if (err != null && err.Count > 0)
                                        {
                                            tb.AddRecord("Выявлены совпадения с другими документами:", err);
                                            tb.AddRecord("Статус документа понижен.");

                                            newdoc.documentStatus = DEXToolBox.DOCUMENT_DRAFT;

                                            swarn = string.Format(
                                                "Внимание!\n\n" +
                                                "Статус документа понижен, поскольку\n" +
                                                "выявлены совпадения данных с другими документами.\n" +
                                                "Количество совпадений: {0}\n\n" +
                                                "Подробности смотрите в журнале документа",
                                                err.Count
                                            );
                                        }
                                    }

                                    row["jdocdate"] = newdoc.documentDate;
                                    Program.log("Main.cs 1325 docdate = " + newdoc.documentDate);
                                    row["status"] = newdoc.documentStatus;
                                    row["unitid"] = newdoc.documentUnitID;
                                    row["digest"] = newdoc.documentDigest;
                                    row["data"] = newdoc.documentText;

                                    // Установка DataReferences
                                    tb.setDataReference("units", olddoc.documentUnitID.ToString(), false);
                                    tb.setDataReference("units", newdoc.documentUnitID.ToString(), true);

                                    // Установка Criticals
                                    tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc, true);

                                    // Установка People Data
                                    try
                                    {
                                        StringList slp = doc.GetPeopleData(tb, newdoc);
                                        if (slp != null) tb.setPeopleData(slp);
                                    }
                                    catch (Exception) { }

                                }

                                row["locked"] = "";
                                row["locktime"] = "";
                                row["journal"] = SimpleXML.SaveXml(tb.getCurrentJournal());

                                try
                                {
                                    adaJournal.Update(dtJournal);
                                }
                                catch (Exception)
                                {
                                    if (dtJournal.HasErrors)
                                    {
                                        DataRow[] errs = dtJournal.GetErrors();
                                        foreach (DataRow err in errs)
                                        {

                                            string errn = "";
                                            foreach (DataColumn co in err.GetColumnsInError())
                                            {
                                                errn += co.ColumnName + "\n";
                                            }
                                            err.ClearErrors();
                                        }
                                        adaJournal.Update(errs);
                                    }
                                }

                                dtJournalUpdateAddRow(row);
                                row.AcceptChanges();
                                dgvJournal_RowEnter(dgvJournal, new DataGridViewCellEventArgs(0, dgvJournal.CurrentRow.Index));

                                tb.setCurrentJournal(null);

                                if (swarn != "") MessageBox.Show(swarn);
                            }
                            else
                            {
                                string msg = string.Format(
                                    "Редактирование документа невозможно, так как\n" +
                                    "модуль документа <{0}> отсутствует.\n" +
                                    "Обратитесь к техническому специалисту.",
                                    row["docid"].ToString()
                                    );

                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            string msg = string.Format(
                                "Изменение документа невозможно, так как в данный момент\n" +
                                "он редактируется пользователем <{0}>.\n" +
                                "Время начала редактирования: {1}:{2}:{3} {4}.{5}.{6}",
                                locker_title,
                                lock_time.Substring(8, 2), lock_time.Substring(10, 2), lock_time.Substring(12, 2),
                                lock_time.Substring(6, 2), lock_time.Substring(4, 2), lock_time.Substring(0, 4)
                                );
                            MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Изменение документа невозможно, так как в данный момент\n" +
                            "он экспортируется, либо отмечен для экспорта.\n" +
                            "Для изменения документа дождитесь окончания экспорта или измените состояние документа.");
                    }
                    JournalLoadSelection(dsave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace);
            }
            
        }

        private void tsbCloneDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvJournal.Visible)
                {
                    DEXToolBox tb = DEXToolBox.getToolBox();
                    Random r = new Random();
                    int i = r.Next(99999999);
                    String newsignature = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT_MS) + i.ToString("D8");
                    CDEXDocumentData newdoc = new CDEXDocumentData();
                    newdoc.signature = newsignature;

                   
                    DataRow row = ((DataRowView)bsJournal.Current).Row;
                    IDEXPluginDocument doc = tb.Plugins.getDocumentByID(row["docid"].ToString());
                    string signature = row["signature"].ToString();


                    CDEXDocumentData olddoc = new CDEXDocumentData();
                    olddoc.documentDate = row["jdocdate"] as string;
                    Program.log("Main.cs 1281 docdate = " + olddoc.documentDate);
                    olddoc.documentStatus = int.Parse(row["status"].ToString());
                    olddoc.documentText = row["data"].ToString();
                    olddoc.documentUnitID = int.Parse(row["unitid"].ToString());
                    olddoc.documentDigest = row["digest"].ToString();
                    olddoc.signature = signature;
                    tb.setCurrentJournal(new SimpleXML("journal"));

                    if (doc.CloneDocument(tb, olddoc, newdoc))
                    {
                        
                        // Проверка criticals
                        if (newdoc.documentStatus > DEXToolBox.DOCUMENT_DRAFT)
                        {
                            ArrayList err = tb.checkDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc);
                            //tb.AddRecord("Документ создан клонированием. ID оригинала " + row["id"].ToString());
                            //tb.AddRecord("");
                            if (err != null && err.Count > 0)
                            {
                                tb.AddRecord("Выявлены совпадения с другими документами:");
                                foreach (string eitem in err) tb.AddRecord(eitem);
                                tb.AddRecord("Статус документа понижен.");
                                newdoc.documentStatus = DEXToolBox.DOCUMENT_DRAFT;
                            }
                        }
               

                        DataRow nr = dtJournal.NewRow();
                        nr.BeginEdit();
                        nr["locked"] = "";
                        nr["locktime"] = "";
                        nr["userid"] = tb.sUID;
                        nr["status"] = newdoc.documentStatus;
                        nr["signature"] = newsignature;
                        //                    nr["jdocdate"] = DateTime.Now.ToString(DEXToolBox.DB_DATE_FORMAT_MS);
                        Program.log("Main.cs 1153 docdate = " + newdoc.documentDate);
                        nr["jdocdate"] = newdoc.documentDate;
                        nr["unitid"] = newdoc.documentUnitID;
                        nr["docid"] = doc.ID;
                        nr["digest"] = newdoc.documentDigest;
                        nr["data"] = newdoc.documentText;
                        nr["journal"] = SimpleXML.SaveXml(tb.getCurrentJournal());
                        nr.EndEdit();
                        dtJournal.Rows.Add(nr);

                        try
                        {
                            adaJournal.Update(dtJournal);
                        }
                        catch (Exception)
                        {
                        }

                        // Установка DataReferences
                        tb.setDataReference("units", newdoc.documentUnitID.ToString(), true);
                        tb.setDataReference("users", tb.sUID, true);

                        // Установка Criticals
                        tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), newdoc, true);

                        // Установка People Data
                        try
                        {
                            StringList slp = doc.GetPeopleData(tb, newdoc);
                            if (slp != null) tb.setPeopleData(slp);
                        }
                        catch (Exception) { }

                        _journal(lastPageNum);

                        try
                        {
                            foreach (DataGridViewRow dgvr in dgvJournal.Rows)
                            {
                                DataRowView drv = bsJournal[dgvr.Index] as DataRowView;
                                if (drv.Row["signature"].ToString().Equals(newsignature))
                                {
                                    dgvr.Selected = true;
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                }
            } 
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace);
            }
        }

        private void tsbDeleteDocument_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.JOURNAL)
            {
                MessageBox.Show("В режиме архива невозможно удаление документов.\nПереключитесь в режим журнала.");
                return;
            }

            try
            {
                if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
                {
                    DEXToolBox tb = DEXToolBox.getToolBox();
                    MySqlConnection c = tb.getConnection();

                    if (MessageBox.Show(string.Format("Удалить документы ( {0} )?", dgvJournal.SelectedRows.Count),
                        "Удаление документов", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        List<DataRow> drows = new List<DataRow>();
                        foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                        {
                            DataRowView drv = bsJournal[r.Index] as DataRowView;
                            drows.Add(drv.Row);
                        }

                        int deletedCount = 0, errorCount = 0;

                        foreach (DataRow row in drows)
                        {
                            try
                            {
                                int rid = int.Parse(row["id"].ToString());
                                string signature = row["signature"].ToString();


                                MySqlCommand cmd = new MySqlCommand(
                                    string.Format(
                                        "select j.locked, j.locktime, u.title " +
                                        "from `journal` as j, `users` as u " +
                                        "where j.id = {0} and j.locked = u.uid",
                                        rid),
                                    c);
                                MySqlDataReader reader = cmd.ExecuteReader();

                                string locker_id = "<NONE>", locker_title = "", lock_time = "";
                                try
                                {
                                    while (reader.Read())
                                    {
                                        locker_id = reader.GetString("locked");
                                        locker_title = reader.GetString("title");
                                        lock_time = reader.GetString("locktime");
                                        break;
                                    }
                                }
                                finally
                                {
                                    reader.Close();
                                }

                                if (!locker_id.Equals("<NONE>")) throw new Exception();

                                IDEXPluginDocument doc = tb.Plugins.getDocumentByID(row["docid"].ToString());
                                if (doc == null) throw new Exception();

                                CDEXDocumentData olddoc = new CDEXDocumentData();
                                olddoc.documentDate = row["jdocdate"].ToString();

                                olddoc.documentStatus = DEXToolBox.DOCUMENT_TODELETE;
                                olddoc.documentText = row["data"].ToString();
                                olddoc.documentUnitID = int.Parse(row["unitid"].ToString());
                                olddoc.signature = signature;

                                // Документ имеет модуль
                                if (!doc.DeleteDocument(tb, olddoc, false)) throw new Exception();

                                // Чистим ссылки
                                tb.setDataReference("units", olddoc.documentUnitID.ToString(), false);
                                tb.setDataReference("users", row["userid"].ToString(), false);

                                // Чистим Criticals
                                tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), olddoc, true);

                                row.Delete();

                                deletedCount++;
                            }
                            catch (Exception)
                            {
                                errorCount++;
                            }
                        }

                        
                        MessageBox.Show(string.Format("Документов удалено: {0}\nНе удалось удалить: {1}", deletedCount, errorCount));

                        adaJournal.Update(dtJournal);

                        if (dgvJournal.Rows.Count < 1)
                        {
                            _journal(lastPageNum);
                        }

                    }


/* 
// Удаление одиночного документа

                    DataRow row = ((DataRowView)bsJournal.Current).Row;
                    int rid = int.Parse(row["id"].ToString());
                    string signature = row["signature"].ToString();

                    MySqlCommand cmd = new MySqlCommand(
                        string.Format(
                            "select j.locked, j.locktime, u.title " +
                            "from `journal` as j, `users` as u " +
                            "where j.id = {0} and j.locked = u.uid", 
                            rid), 
                        c);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string locker_id = "<NONE>", locker_title = "", lock_time = "";
                    try
                    {
                        while (reader.Read())
                        {
                            locker_id = reader.GetString("locked");
                            locker_title = reader.GetString("title");
                            lock_time = reader.GetString("locktime");
                            break;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    if (locker_id.Equals("<NONE>"))
                    {
                        // Удаляем док
                        bool doDeleteDoc = false;

                        if (MessageBox.Show("Удалить документ?", "Удаление документа", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            IDEXPluginDocument doc = tb.Plugins.getDocumentByID(row["docid"].ToString());
                            if (doc != null)
                            {
                                CDEXDocumentData olddoc = new CDEXDocumentData();
                                olddoc.documentDate = row["jdocdate"].ToString();
                                Program.log("Main.cs 1476 docdate = " + olddoc.documentDate);
                                olddoc.documentStatus = DEXToolBox.DOCUMENT_TODELETE;
                                olddoc.documentText = row["data"].ToString();
                                olddoc.documentUnitID = int.Parse(row["unitid"].ToString());
                                olddoc.signature = signature;

                                // Документ имеет модуль
                                doDeleteDoc = doc.DeleteDocument(tb, olddoc, false);

                                if (doDeleteDoc)
                                {
                                    // Чистим ссылки
                                    tb.setDataReference("units", olddoc.documentUnitID.ToString(), false);
                                    tb.setDataReference("users", row["userid"].ToString(), false);

                                    // Чистим Criticals
                                    tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), olddoc, true);
                                }
                            }
                        }

                        if (doDeleteDoc)
                        {
                            row.Delete();
                            adaJournal.Update(dtJournal);
                        }

                        if (dgvJournal.Rows.Count < 1)
                        {
                            _journal(lastPageNum);
                        }
                    }
                    else
                    {
                        string msg = string.Format(
                            "Удаление документа невозможно, так как в данный момент\n" +
                            "он редактируется пользователем <{0}>.\n" +
                            "Время начала редактирования: {1}:{2}:{3} {4}.{5}.{6}",
                            locker_title,
                            lock_time.Substring(8, 2), lock_time.Substring(10, 2), lock_time.Substring(12, 2),
                            lock_time.Substring(6, 2), lock_time.Substring(4, 2), lock_time.Substring(0, 4)
                            );
                        MessageBox.Show(msg);
                    }
 */
                }
            }
            catch (Exception)
            {
            }

        }

        #endregion

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
                DEXToolBox tb = DEXToolBox.getToolBox();
                
                
                
                try
                {
                    dc = SimpleXML.LoadXml(r["data"].ToString());
                    dc["DocUnit"].Text = r["unitid"].ToString();
                    //dc["ICC"].Text = dc["ICC"].Text.Substring(7, 10);
                    
                    dc["MainDealerName"].Text = "";
                    dc["MainDealerFIO"].Text = "";
                    dc["MainDealerPowAt"].Text = "";
                    dc["MainDealerDatePowAt"].Text = "";

                    
                }
                catch (Exception)
                {
                }

                DataTable dpCode = tb.getQuery("select rvalue from `registers` where rname='mts_dpcode_prefix'");
                if ( dpCode != null )
                {
                    dc["dpCodeMts"].Text = dpCode.Rows[0]["rvalue"].ToString();
                }
                


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

                        DataTable t = tb.getQuery(string.Format(
                            "select * from `prnschemes` where printer = '{0}' and guid = '{1}'",
                            tb.EscapeString(ps.PrinterName), tb.EscapeString(scheme["GUID"].Text)));
                        if (t != null && t.Rows.Count > 0)
                        {
                            scheme = SimpleXML.LoadXml(t.Rows[0]["data"].ToString());
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

                    tsslStatus.Text = "Отправка документов на печать";
                    tspbProgress.Minimum = 0;
                    tspbProgress.Maximum = dgvJournal.SelectedRows.Count;
                    tspbProgress.Value = 0;

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
                                    dc["DocUnit"].Text = drv["unitid"].ToString();
                                    
                                 
                                    //dc["ICC"].Text = dc["ICC"].Text.Substring(7, 10);
                                    dc["MainDealerName"].Text = "";
                                    dc["MainDealerFIO"].Text = "";
                                    dc["MainDealerPowAt"].Text = "";
                                    dc["MainDealerDatePowAt"].Text = "";
                                   
                                }
                                catch (Exception)
                                {
                                }

                                DataTable dpCode = tb.getQuery("select rvalue from `registers` where rname='mts_dpcode_prefix'");
                                if ( dpCode != null )
                                {
                                    dc["dpCodeMts"].Text = dpCode.Rows[0]["rvalue"].ToString();
                                }

                                SimpleXML scheme = schemes[drv["docid"].ToString()];
                                if (scheme != null)
                                {
                                    DataTable t = tb.getQuery(string.Format(
                                        "select * from `prnschemes` where printer = '{0}' and guid = '{1}'",
                                        tb.EscapeString(ps.PrinterName), tb.EscapeString(scheme["GUID"].Text)));
                                    if (t != null && t.Rows.Count > 0)
                                    {
                                        scheme = SimpleXML.LoadXml(t.Rows[0]["data"].ToString());
                                    }

                                    
                                    CPrintDocument doc = new CPrintDocument(dc, scheme, ps, tb);
                                    doc.Print(false);
                                }
                            }
                            catch (Exception)
                            {

                            }
                            tspbProgress.Value++;
                        }

                        tspbProgress.Value = 0;
                        tsslStatus.Text = "";
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
            try
            {
                Program.log("Попытка изменить настройки принтера!");
                DEXToolBox tb = DEXToolBox.getToolBox();
                PrintDialog pdlg = new PrintDialog();
                pdlg.UseEXDialog = true; // если не показаывается диалог, то этот для dot net 2.0

                pdlg.PrinterSettings = tb.LoadPrinterSettings();

                Program.log("Описание настроек принтера " + pdlg.PrinterSettings);

                if (pdlg.ShowDialog() == DialogResult.OK)
                {
                    Program.log("Попытка изменить настройки принтера принята пользователем!");
                    tb.SavePrinterSettings(pdlg.PrinterSettings);
                }
                else
                {
                    Program.log("Попытка изменить настройки принтера не принята пользователем!");
                }
            }
            catch (Exception)
            {
                Program.log("Ошибка в процессе изменить настроки принтера");
            }
        }

        private void tsmiSchemesSetup_Click(object sender, EventArgs e)
        {
            SchemesSetupForm ssform = new SchemesSetupForm();
            ssform.ShowDialog();
        }

        #endregion

        #region Работа с отчётами

        private void Report_Click(object sender, EventArgs e)
        {
            // Вызов пункта меню отчёта
            if (((ToolStripItem)sender).Tag is IDEXPluginReport)
            {
                IDEXPluginReport caller = (IDEXPluginReport)((ToolStripItem)sender).Tag;
                caller.Report(DEXToolBox.getToolBox());
            }
        }

        #endregion

        #region Работа с функциями

        private void Function_Click(object sender, EventArgs e)
        {
            // Вызов пункта меню функции
            if (((ToolStripItem)sender).Tag is IDEXPluginFunction)
            {
                IDEXPluginFunction caller = (IDEXPluginFunction)((ToolStripItem)sender).Tag;
                caller.Execute(DEXToolBox.getToolBox());
            }
        }

        #endregion

        #region Экспорт документов

        List<DataRow> idsToExport;

        public string CheckDocToExport(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = true;
            wmea.minValue = 0;
            wmea.maxValue = dgvJournal.SelectedRows.Count;
            wmea.textMessage = "Проверка выделенных документов";
            wmea.progressValue = 0;
            wmea.progressVisible = true;

            int pw = 0;

            MySqlCommand cmd = new MySqlCommand("select * from `journal` where id=@id", DEXToolBox.getToolBox().getConnection());
            cmd.Parameters.Add("id", MySqlDbType.Int24);
            idsToExport = new List<DataRow>();
            foreach (DataGridViewRow r in dgvJournal.SelectedRows)
            {
                DataRowView drv = bsJournal[r.Index] as DataRowView;

                cmd.Parameters["id"].Value = int.Parse(drv["id"].ToString());
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        int st = dr.GetInt32("status");
                        string locked = dr.GetString("locked");
                        if (locked.Equals(""))
                        {
                            if (st == DEXToolBox.DOCUMENT_APPROVED)
                            {
                                idsToExport.Add(drv.Row);
                            }
                        }
                    }
                    dr.Close();
                }

                wmea.progressValue = pw++;
                if (wmea.isAborted) break;
            }                
            
            
            return "";
        }

        public string MarkDocToExport(IWaitMessageEventArgs wmea)
        {
            if (idsToExport != null && idsToExport.Count > 0)
            {
                wmea.canAbort = true;
                wmea.minValue = 0;
                wmea.maxValue = idsToExport.Count;
                wmea.textMessage = "Изменение статуса документов";
                wmea.progressValue = 0;
                wmea.progressVisible = true;

                int pw = 0;

                try
                {
                    foreach (DataRow r in idsToExport)
                    {
                        r.BeginEdit();
                        r["status"] = DEXToolBox.DOCUMENT_TOEXPORT;
                        r.EndEdit();

                        wmea.progressValue = pw++;
                        if (wmea.isAborted) break;
                    }
                    adaJournal.Update(dtJournal);
                }
                catch (Exception)
                {
                }
            }

            return "";
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.JOURNAL)
            {
                MessageBox.Show("В режиме архива невозможен экспорт документов.\nПереключитесь в режим журнала.");
                return;
            }
                
            idsToExport = null;

            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                string ret = WaitMessage.Execute(new WaitMessageEvent(CheckDocToExport));
                if (!ret.Equals(""))
                {
                    MessageBox.Show("Ошибки:\n\n" + ret);
                    return;
                }
            }


            if (idsToExport != null && idsToExport.Count > 0)
            {
                if (MessageBox.Show(string.Format("Отметить для экспорта выделенные записи ({0})?",
                    idsToExport.Count), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string ret = WaitMessage.Execute(new WaitMessageEvent(MarkDocToExport));
                    if (!ret.Equals(""))
                    {
                        MessageBox.Show("Ошибки:\n\n" + ret);
                    }
                    ArrayList sel = JournalSaveSelection();
                    _journal(lastPageNum);
                    JournalLoadSelection(sel);
                }
            }
            else
            {
                MessageBox.Show("Не выделено ни одного документа для экспорта");
            }
        }

        #endregion

        #region Изменение статуса документов

        private void tsbChangeStatus_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.JOURNAL)
            {
                MessageBox.Show("В режиме архива невозможно изменение статуса документов.\nПереключитесь в режим журнала.");
                return;
            }

            
            bool noRecords = true;
            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                int singlestate = -1;
                List<DataRow> ids = new List<DataRow>();
                foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                {
                    DataRowView drv = bsJournal[r.Index] as DataRowView;
                    try
                    {
                        ids.Add(drv.Row);
                        if (singlestate > -2)
                        {
                            int cstate = int.Parse(drv["status"].ToString());
                            if (singlestate == -1) singlestate = cstate;
                            else if (singlestate != cstate)
                            {
                                singlestate = -2;
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                }

                if (ids.Count > 0)
                {
                    noRecords = false;
                    FChangeStatus fcs = new FChangeStatus();
                    fcs.lDocCount.Text = ids.Count.ToString();
                    fcs.cbStatus.Items.Clear();
                    foreach (string cust in DEXToolBox.DOCUMENT_STATE_TEXT)
                    {
                        if (!cust.Equals(DEXToolBox.DOCUMENT_STATE_TEXT[DEXToolBox.DOCUMENT_EXPORTING]))
                        {
                            fcs.cbStatus.Items.Add(cust);
                        }
                    }
                    fcs.cbStatus.SelectedIndex = (singlestate > -1 && singlestate < DEXToolBox.DOCUMENT_EXPORTING) ? singlestate : -1;
                    if (fcs.ShowDialog() == DialogResult.OK && fcs.cbStatus.SelectedIndex != singlestate)
                    {

                        foreach (DataRow r in ids)
                        {
                            r.BeginEdit();
                            r["status"] = fcs.cbStatus.SelectedIndex;
                            r.EndEdit();

                            CDEXDocumentData dc = new CDEXDocumentData();
                            dc.documentStatus = int.Parse(r["status"].ToString());
                            dc.signature = r["signature"].ToString();
                            dc.documentText = r["data"].ToString();
                            IDEXPluginDocument idpd = DEXToolBox.getToolBox().Plugins.getDocumentByID(r["docid"].ToString());
                            if (idpd != null)
                            {
                                DEXToolBox.getToolBox().setDocumentCriticals(
                                    idpd.GetDocumentCriticals(DEXToolBox.getToolBox()), dc, true
                                    );
                            }
                        }

                        adaJournal.Update(dtJournal);
                        
                        ArrayList jsel = JournalSaveSelection();
                        _journal(lastPageNum);
                        JournalLoadSelection(jsel);
                    }
                }
            }

            if (noRecords) MessageBox.Show("Не выделено ни одного документа для изменения статуса.");
        }
        #endregion

        #region Подтверждение выделенных документов

        List<DataRow> adDocs = new List<DataRow>();

        int adStatus;
        bool adIgnoreStReturned, adIgnoreStSent, adStReturnedStSent;
        int adDocsProcessed;

        void updateUmData(string msisdn, string status)
        {
            try
            {
                string sql = "update `um_data` set status_j = " + status + " where msisdn = " + msisdn;
                IDEXData d = (IDEXData)DEXToolBox.getToolBox();
                d.runQuery(sql);
            }
            catch (Exception)
            {

            }
        }

        public string ApproveDocuments(IWaitMessageEventArgs wmea)
        {
            string ret = "";
            adDocsProcessed = 0;

            try
            {
                if (adDocs.Count > 0)
                {
                    wmea.canAbort = true;
                    wmea.minValue = 0;
                    wmea.maxValue = adDocs.Count;
                    wmea.textMessage = "Подтверждение документов";
                    int pv = 0;
                    wmea.progressValue = pv;
                    wmea.progressVisible = true;

                    DEXToolBox tb = DEXToolBox.getToolBox();
                    
                    foreach (DataRow r in adDocs)
                    {
                        try
                        {
                            SimpleXML ddata = null, djournal = null;
                            int dstatus = DEXToolBox.DOCUMENT_NONE, dunitid = -1, did = int.Parse(r["id"].ToString());
                            string ddocdate = "", dsignature = "";

                            MySqlCommand cmd = new MySqlCommand(
                                string.Format(
                                    "select j.status, j.jdocdate, j.signature, j.unitid, j.locked, j.locktime, j.data, j.journal, u.title " +
                                    "from `journal` as j, `users` as u " +
                                    "where j.id = {0}", did),
                                tb.getConnection());
                            MySqlDataReader reader = cmd.ExecuteReader();

                            bool isExporting = false;
                            string locker_id = "<NONE>", locker_title = "", lock_time = "";
                            try
                            {
                                while (reader.Read())
                                {
                                    ddocdate = reader.GetString("jdocdate");
                                    dsignature = reader.GetString("signature");
                                    dunitid = reader.GetInt32("unitid");
                                    locker_id = reader.GetString("locked");
                                    locker_title = reader.GetString("title");
                                    lock_time = reader.GetString("locktime");
                                    dstatus = reader.GetInt32("status");
                                    isExporting = (dstatus == DEXToolBox.DOCUMENT_EXPORTING /*|| dstatus == DEXToolBox.DOCUMENT_TOEXPORT*/);
                                    ddata = SimpleXML.LoadXml(reader.GetString("data"));
                                    djournal = SimpleXML.LoadXml(reader.GetString("journal"));

                                    break;
                                }
                            }
                            finally
                            {
                                reader.Close();
                            }

                            if (!isExporting)
                            {
                                if (locker_id.Equals("<NONE>") || locker_id.Equals(""))
                                {
                                    IDEXPluginDocument doc = tb.Plugins.getDocumentByID(r["docid"].ToString());
                                    if (doc != null && ddata != null)
                                    {
                                        if (djournal == null) djournal = new SimpleXML("journal");
                                        if (dstatus == DEXToolBox.DOCUMENT_DRAFT || dstatus == DEXToolBox.DOCUMENT_UNAPPROVED)
                                        {
                                            CDEXDocumentData olddoc = new CDEXDocumentData();
                                            olddoc.documentDate = ddocdate;
                                            Program.log("Main.cs 2043 docdate = " + olddoc.documentDate);
                                            olddoc.documentStatus = dstatus;
                                            olddoc.documentText = SimpleXML.SaveXml(ddata);
                                            olddoc.documentUnitID = dunitid;
                                            olddoc.signature = dsignature;
                                            tb.setCurrentJournal(djournal);

                                            ArrayList errs = doc.ValidateDocument(tb, olddoc);
                                            ArrayList errs2 = null;
                                            if (errs != null && errs.Count == 0)
                                            {
                                                errs2 = tb.checkDocumentCriticals(doc.GetDocumentCriticals(tb), olddoc);
                                                if (errs2 != null && errs2.Count == 0)
                                                {
                                                    ArrayList jparams = new ArrayList();
                                                    jparams.Add(string.Format("Предыдущий статус: {0}", olddoc.documentStatus));

                                                    tb.AddRecord(djournal, "Успешное изменение статуса", jparams.ToArray());

                                                    olddoc.documentStatus = adStatus;
                                                    dstatus = adStatus;

                                                    r.BeginEdit();
                                                    r["status"] = adStatus;
                                                    r["data"] = SimpleXML.SaveXml(ddata);
                                                    r["journal"] = SimpleXML.SaveXml(djournal);
                                                    updateUmData(ddata["msisdn"].Text, r["status"].ToString());
                                                    r.EndEdit();


                                                    tb.setDocumentCriticals(doc.GetDocumentCriticals(tb), olddoc, true);
                                                    adDocsProcessed++;
                                                    // Сохранить изменения в базе
                                                    // Добавить criticals, если нету
                                                }
                                            }

                                            if ((errs != null && errs.Count > 0) || (errs2 != null && errs2.Count > 0))
                                            {
                                                tb.AddRecord(djournal, "Подтверждение документа не удалось");
                                                if (errs != null && errs.Count > 0)
                                                {
                                                    tb.AddRecord(djournal, "Ошибки валидации", errs.ToArray());
                                                }

                                                if (errs2 != null && errs2.Count > 0)
                                                {
                                                    tb.AddRecord(djournal, "Сведения о документах с похожими данными", errs2.ToArray());
                                                }

                                                r.BeginEdit();
                                                r["journal"] = SimpleXML.SaveXml(djournal);
                                                updateUmData(ddata["msisdn"].Text, r["status"].ToString());
                                                r.EndEdit();

                                            }
                                        }
                                        else
                                            if (dstatus != adStatus &&
                                                (dstatus == DEXToolBox.DOCUMENT_APPROVED || dstatus == DEXToolBox.DOCUMENT_TOEXPORT ||
                                                (dstatus == DEXToolBox.DOCUMENT_EXPORTED && !adIgnoreStSent) ||
                                                (dstatus == DEXToolBox.DOCUMENT_RETURNED && (!adIgnoreStReturned || adStReturnedStSent))
                                                ))
                                            {
                                                // Тупо поменять статус на adStatus
                                                r.BeginEdit();
                                                r["status"] = (dstatus == DEXToolBox.DOCUMENT_RETURNED && adStReturnedStSent) ? DEXToolBox.DOCUMENT_EXPORTED : adStatus;


                                                ArrayList jparams = new ArrayList();
                                                jparams.Add(string.Format("Пользователь {0} установил документу статус {1}", tb.sTitle, r["status"].ToString()));
                                                tb.AddRecord(djournal, "Успешное изменение статуса", jparams.ToArray());
                                                //tb.setCurrentJournal(djournal);
                                                r["journal"] = SimpleXML.SaveXml(djournal);


                                                updateUmData(ddata["msisdn"].Text, r["status"].ToString());

                                                r.EndEdit();

                                                adDocsProcessed++;
                                            }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        pv++;
                        if (pv % 10 == 0)
                        {
                            wmea.progressValue = pv;
                        }
                    }

                    adaJournal.Update(dtJournal);
                }
            }
            catch (Exception)
            {
            }
            
            return ret;
        }

        private void tsbApprove_Click(object sender, EventArgs e)
        {
            if (journalType != DEXJournalType.JOURNAL)
            {
                MessageBox.Show("В режиме архива невозможно подтверждение документов.\nПереключитесь в режим журнала.");
                return;
            }

            adDocs.Clear();
            int adDraftCnt = 0, adUnapprovedCnt = 0, adApprovedCnt = 0, adToExportCnt = 0, adExportedCnt = 0, adReturnedCnt = 0;

            // Подсчитать кол-во черновиков, документов на подтверждение и подтверждённых документов
            if (dgvJournal.Visible && dgvJournal.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                {
                    DataRowView drv = bsJournal[r.Index] as DataRowView;
                    try
                    {
                        int cid = int.Parse(drv["id"].ToString());
                        int cstate = int.Parse(drv["status"].ToString());
                        switch (cstate)
                        {
                            case DEXToolBox.DOCUMENT_DRAFT:
                                adDraftCnt++;
                                adDocs.Add(drv.Row);
                                break;
                            case DEXToolBox.DOCUMENT_UNAPPROVED:
                                adUnapprovedCnt++;
                                adDocs.Add(drv.Row);
                                break;
                            case DEXToolBox.DOCUMENT_APPROVED:
                                adApprovedCnt++;
                                adDocs.Add(drv.Row);
                                break;
                            case DEXToolBox.DOCUMENT_TOEXPORT:
                                adToExportCnt++;
                                adDocs.Add(drv.Row);
                                break;
                            case DEXToolBox.DOCUMENT_EXPORTED:
                                adExportedCnt++;
                                adDocs.Add(drv.Row);
                                break;
                            case DEXToolBox.DOCUMENT_RETURNED:
                                adReturnedCnt++;
                                adDocs.Add(drv.Row);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (adDraftCnt + adUnapprovedCnt + adApprovedCnt + adToExportCnt + adExportedCnt + adReturnedCnt > 0)
                {
                    ApproveForm afrm = new ApproveForm();
                    afrm.rtbInfo.Text = string.Format(
                        "Черновиков: {0}\nНеподтверждённых: {1}\nПодтверждённых: {2}\nПомеченных на экспорт: {3}\nЭкспортированных: {4}\nВозвращённых: {5}",
                        adDraftCnt, adUnapprovedCnt, adApprovedCnt, adToExportCnt, adExportedCnt, adReturnedCnt);
                    afrm.cbIgnoreStSent.Visible = adExportedCnt > 0;
                    afrm.cbIgnoreStReturned.Visible = adReturnedCnt > 0;
                    afrm.cbStReturnedStSent.Visible = adReturnedCnt > 0;

                    if (afrm.ShowDialog() == DialogResult.OK)
                    {
                        adIgnoreStReturned = afrm.cbIgnoreStReturned.Checked;
                        adIgnoreStSent = afrm.cbIgnoreStSent.Checked;
                        adStReturnedStSent = afrm.cbStReturnedStSent.Checked;
                        adStatus = (afrm.rbStApproved.Checked) ? DEXToolBox.DOCUMENT_APPROVED : DEXToolBox.DOCUMENT_TOEXPORT;

                        string ret = WaitMessage.Execute(new WaitMessageEvent(ApproveDocuments));
                        if (!ret.Equals("")) MessageBox.Show(ret);
                        else MessageBox.Show(string.Format("Обработано документов: {0}", adDocsProcessed));
                        ArrayList sel = JournalSaveSelection();
                        _journal(lastPageNum);
                        JournalLoadSelection(sel);
                    }
                    afrm.saveForm();
                }
                else
                {
                    MessageBox.Show("Не выделено ни одного подходящего документа.");
                }
            }
        }

        #endregion

        Color[] rowColors = new Color[] {
            Color.FromArgb(0xd0, 0xd0, 0xd0), // Draft
            Color.FromArgb(0xA0, 0xFF, 0xFF), // Unapproved
            Color.FromArgb(0xA0, 0xFF, 0xA0), // Approved
            Color.FromArgb(0xFF, 0xFF, 0xA0), // To export
            Color.FromArgb(0xFF, 0xFF, 0xFF), // Exported
            Color.FromArgb(0xFF, 0xD0, 0xD0), // Returned
            Color.FromArgb(0xFD, 0xC0, 0xFF) // Exporting
        };

        private void dgvJournal_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (e.State != DataGridViewElementStates.Selected)
                {
                    DataRow r = ((DataRowView)bsJournal[e.RowIndex]).Row;
                    dgvJournal.Rows[e.RowIndex].DefaultCellStyle.BackColor = rowColors[int.Parse(r["status"].ToString())];
                }
            }
            catch (Exception)
            {
            }
        }

        private void dgvJournal_SelectionChanged(object sender, EventArgs e)
        {
            Program.log("dgvJournal_SelectionChanged");

            DEXToolBox tb = DEXToolBox.getToolBox();
            ArrayList jhs = tb.Plugins.getJournalhooks();

            if (jhs != null && jhs.Count > 0)
            {
                foreach (IDEXPluginJournalhook jh in jhs)
                {
                    jh.InitReflist(tb);
                }
            }
            
            int rc = dgvJournal.Rows.GetRowCount(DataGridViewElementStates.Selected);
            tsslSelCount.Text = "Выделено: " + rc.ToString();

            if (rc > 0)
            {
                cmsJournalhook.Items.Clear();
                if (jhs != null && jhs.Count > 0)
                {
                    foreach (IDEXPluginJournalhook jh in jhs)
                    {
                        foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                        {
                            DataRowView drv = bsJournal[r.Index] as DataRowView;
                            try
                            {
                                jh.AddReferenceVisibility(tb, drv["docid"].ToString(), int.Parse(drv["status"].ToString()));
                            }
                            catch (Exception) { }
                        }

                        Dictionary<string, string> jhflist = jh.getVisibleFunctionsList(tb);
                        if (jhflist != null && jhflist.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> kvp in jhflist)
                            {
                                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                                tsmi.Text = kvp.Value;
                                tsmi.Name = kvp.Key;
                                tsmi.Tag = null;
                                tsmi.Image = jh.getBitmap();
                                cmsJournalhook.Items.Add(tsmi);

                                Dictionary<string, string> sublist = jh.getVisibleSubFunctionsList(kvp.Key);
                                if (sublist != null && sublist.Count > 0)
                                {
                                    foreach (KeyValuePair<string, string> kvp2 in sublist)
                                    {
                                        if (kvp2.Value.Equals("-"))
                                        {
                                            tsmi.DropDownItems.Add(new ToolStripSeparator());
                                        }
                                        else
                                        {
                                            ToolStripMenuItem tsmi2 = new ToolStripMenuItem();
                                            tsmi2.Text = kvp2.Value;
                                            tsmi2.Name = kvp.Key + "_" + kvp2.Key;
                                            tsmi2.Tag = new JournalHookItemInfo(jh, kvp.Key, kvp2.Key);
                                            tsmi2.Click += JournalHookMenuItem_Click;
                                            tsmi.DropDownItems.Add(tsmi2);
                                        }
                                    }
                                }
                                else
                                {
                                    tsmi.Tag = new JournalHookItemInfo(jh, kvp.Key, null);
                                    tsmi.Click += JournalHookMenuItem_Click;
                                }
                            }
                        }
                    }
                }
            }
            Program.log("dgvJournal_SelectionChanged /");

        }

        private void JournalHookMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DEXToolBox tb = DEXToolBox.getToolBox();
                int changesList = 0;
                JournalHookItemInfo jhii = (JournalHookItemInfo)(((ToolStripMenuItem)sender).Tag);

                foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                {
                    DataRow row = ( (DataRowView)bsJournal[r.Index] ).Row;

                    CDEXDocumentData doc = new CDEXDocumentData();
                    doc.documentDate = row["jdocdate"].ToString();
                    Program.log("Main.cs 2330 docdate = " + doc.documentDate);
                    doc.documentStatus = int.Parse(row["status"].ToString());
                    doc.documentText = row["data"].ToString();
                    doc.documentUnitID = int.Parse(row["unitid"].ToString());
                    doc.signature = row["signature"].ToString();
                    doc.documentIdJournal = row["id"].ToString();

                    tb.setCurrentJournal(SimpleXML.LoadXml(row["journal"].ToString()));
                    

                    //IDEXDocumentPlans sss = (IDEXDocumentPlans)( (ToolStripItem)sender ).Tag;
                    //sss.setPlans(tb);

                    bool docChanged = false;
                    bool doStop = jhii.Module.RunFunctionForDocument(jhii.FunctionName, jhii.SubFunctionName, row["docid"].ToString(), doc, out docChanged);

                    if ( docChanged && !doStop )
                    {
                        row["data"] = doc.documentText;
                        row["jdocdate"] = doc.documentDate;
                        row["unitid"] = doc.documentUnitID;
                        changesList++;
                        row["journal"] = SimpleXML.SaveXml(tb.getCurrentJournal());

                        try
                        {
                            adaJournal.Update(dtJournal);
                        }
                        catch ( Exception )
                        {
                            if ( dtJournal.HasErrors )
                            {
                                DataRow[] errs = dtJournal.GetErrors();
                                foreach ( DataRow err in errs )
                                {

                                    string errn = "";
                                    foreach ( DataColumn co in err.GetColumnsInError() )
                                    {
                                        errn += co.ColumnName + "\n";
                                    }
                                    err.ClearErrors();
                                }
                                adaJournal.Update(errs);
                            }
                        }

                        dtJournalUpdateAddRow(row);
                        row.AcceptChanges();
                        dgvJournal_RowEnter(dgvJournal, new DataGridViewCellEventArgs(0, r.Index));
                    }
                    tb.setCurrentJournal(null);

                    if ( doStop )
                        break;
                }
                
            }
            catch (Exception)
            {
            }
        }

        private void dgvLog_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DataRowView row = bsLog[dgvLog.Rows[e.RowIndex].Index] as DataRowView;
                Color cc = (Color)row["color"];
                dgvLog.Rows[e.RowIndex].DefaultCellStyle.BackColor = cc;
            }
            catch (Exception)
            {
            }
        }

        private void bMultiUnits_Click(object sender, EventArgs e)
        {
            MultiUnitsForm muf = new MultiUnitsForm();
            if (muf.ShowDialog() == DialogResult.OK)
            {
              
                //TODO Фильтровать по множественному выбору
                cbUnit.SelectedIndex = Units.IndexOf(-2);
                if (cbFilterImmediate.Checked) _journal(lastPageNum);
            }
        }

        private void bMultiUnitsIgnorReg_Click_(object sender, EventArgs e)
        {
            MultiIgnorRegForm muf = new MultiIgnorRegForm();
            if ( muf.ShowDialog() == DialogResult.OK )
            {

                //TODO Фильтровать по множественному выбору
                //cbUnit.SelectedIndex = Units.IndexOf(-2);
                if ( cbFilterImmediate.Checked )  _journal(lastPageNum);
            }
        }

        private void cbPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( cbFilterImmediate.Checked )
                _journal(lastPageNum);
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( cbFilterImmediate.Checked )
                _journal(lastPageNum);
        }

        private void cbTypeCreateDocChanged(object sender, EventArgs e)
        {
            if ( cbFilterImmediate.Checked )
                _journal(lastPageNum);
        }

        private void toolStripPrintTest_Click(object sender, EventArgs e)
        {
            try
            {
                DEXToolBox tb = DEXToolBox.getToolBox();
                string currentBase = tb.dataBase;
                IDEXServices idis = (IDEXServices)tb;
                JObject authData = new JObject();
                string vendor = "";

                DataTable dt = tb.getQuery("select rvalue from `registers` where rname = 'nodejsserver'");
                string nodejsserver = dt.Rows[0]["rvalue"].ToString();

                authData["login"] = ((IDEXUserData)tb).adaptersLogin == null ? "admin" : ((IDEXUserData)tb).adaptersLogin;
                authData["password"] = ((IDEXUserData)tb).adaptersPass == null ? "12473513" : ((IDEXUserData)tb).adaptersPass;
                //JObject authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                JObject authObj = new JObject();
                string adaptersUid = "";
                try
                {
                    authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                    adaptersUid = authObj["uid"].ToString();
                }
                catch (Exception) { }

                JObject objInfoBase = new JObject();
                objInfoBase = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/adapters/getDexDexolBase?uid=" + adaptersUid + "&clientType=dexol", 1));

                string pfBase = "";
                foreach (JObject jo in objInfoBase["data"])
                {
                    if (jo["list"].ToString().Equals(currentBase))
                    {
                        pfBase = jo["tag"].ToString();
                        currentBase = jo["dex_dexol_base_name"].ToString();
                        if (jo["vendor"].ToString() == "BEELINE") vendor = "beeline";
                        else if (jo["vendor"].ToString() == "MTS") vendor = "mts";
                        else if (jo["vendor"].ToString() == "YOTA") vendor = "yota";
                        else vendor = "megafon";
                        break;
                    }
                }

                //string idsText = "";
                List<string> idsList = new List<string>();
                //int counter = 0;
                foreach (DataGridViewRow r in dgvJournal.SelectedRows)
                {
                    //counter++;
                    DataRow row = ((DataRowView)bsJournal[r.Index]).Row;
                    idsList.Add(row["id"].ToString());
                    //if (counter == dgvJournal.SelectedRows.Count) idsText += row["id"].ToString();
                    //else idsText += row["id"].ToString() + ",";
                }
                string listToStr = String.Join(", ", idsList.ToArray());

                JObject packet = new JObject();
                try
                {
                    packet["com"] = "dexdealer.adapters."+vendor;
                    packet["subcom"] = "apiGetPrintForm";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["vendor"] = vendor;
                    packet["data"]["base"] = currentBase;
                    packet["data"]["list"] = "["+listToStr+"]";
                    packet["data"]["ignoreUid"] = 1;

                    JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/" + vendor + "/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol", 1));
                    if (o["data"]["status"].ToString() == "1")
                    {
                        System.Diagnostics.Process.Start("http://" + nodejsserver + ":3020/printing/" + o["data"]["link"]);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace);
            }
            
        }

        private void cbScan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterImmediate.Checked)
                _journal(lastPageNum);
        }
        
        

        
        

    }
}
