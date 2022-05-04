using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using MySql.Data.MySqlClient;
using DevExpress.Utils;
using DevExpress.Data;
using DevExpress.Skins;
using DevExpress.XtraGrid.Columns;
using DEXExtendLib;
using System.Data.Common;

namespace Kassa3
{
    public partial class MainForm : Form
    {
        Tools tools;
        FormState fs;

        public static MainForm mainForm = null;

        public NetLoginForm loginForm = null;

        public RecForm recForm;

        BalancePropForm balancePropForm;
        OptionsLayoutGrid olg;

        public MainForm()
        {
            MainForm.mainForm = this;

            InitializeComponent();
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"MainForm.fs");

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

            loginForm = new NetLoginForm();
            DialogResult dr = loginForm.ShowDialog();

            if (dr != DialogResult.OK)
            {
                Environment.Exit(-1);
                return;
            }

            using (DbCommand cmd = Db.command("select title from kassa limit 1"))
            {
                try
                {
                    Text = Convert.ToString(cmd.ExecuteScalar());
                }
                catch (Exception) { }
            }

            if (tools.isBoss) Text += " [++]";

            tsmiJournal.Visible = tools.isBoss;

            recForm = new RecForm();
            tools.fixCurrencyRouble();

            olg = new OptionsLayoutGrid();
            olg.StoreAllOptions = false;
            olg.StoreAppearance = false;
            olg.StoreDataSettings = true;
            olg.StoreVisualOptions = true;
            olg.Columns.StoreAllOptions = false;
            olg.Columns.AddNewColumns = true;
            olg.Columns.RemoveOldColumns = false;
            olg.Columns.StoreAppearance = true;
            olg.Columns.StoreLayout = true;


            List<string> sfields = new List<string>();
            try
            {
                string sfs = fs.getValue("shownFields", "").Trim();
                if (!string.IsNullOrEmpty(sfs)) sfields.AddRange(sfs.Split('|'));
            }
            catch (Exception) { }

            Dictionary<int, double> dcc = new Dictionary<int, double>();
            try
            {
                string[] dccs = fs.getValue("CurrencyValues", "").Trim().Split('|');
                foreach (string dccss in dccs)
                {
                    try
                    {
                        string[] dccsss = dccss.Split(';');
                        dcc[int.Parse(dccsss[0])] = double.Parse(dccsss[1]);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }

            balancePropForm = new BalancePropForm(sfields, dcc);
            balancePropForm.init();

            gc.Visible = false;

            updateStatusControls();
            RefreshMenuFirms();
        }


        public void updateStatusControls()
        {
            string uuser = "";
            if (tools.currentUser != null)
            {
                uuser = tools.currentUser.Login.Trim();
                tsmiDicCurrencies.Enabled = tools.currentUser.prefs.dicCurrency != AccessMode.NONE;
                tsmiDicUsers.Enabled = tools.currentUser.prefs.dicUsers != AccessMode.NONE;
                tsmiAppSettings.Enabled = tools.currentUser.prefs.appSettings == SimplePermission.PERMIT;
            }
            tsslUser.Text = uuser;

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
            int pbh;
            int.TryParse(fs.getValue("pBalance_height", pBalCaption.Height.ToString()), out pbh);
            fs.LoadValue("cbBalFrom", cbBalFrom);
            deBalStart.Enabled = cbBalFrom.Checked;
            fs.LoadValue("deBalStart", deBalStart);
            fs.LoadValue("deBalEnd", deBalEnd);
            int bt;
            if (!int.TryParse(fs.getValue("cbBalType", "0"), out bt)) bt = 0;
            cbBalType.SelectedIndex = bt;
            pBalance.Height = pbh;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            fs.SaveValue("pBalance_height", pBalance.Height);
            fs.SaveValue("cbBalFrom", cbBalFrom.Checked);
            fs.SaveValue("deBalStart", deBalStart);
            fs.SaveValue("deBalEnd", deBalEnd);
            fs.SaveValue("cbBalType", cbBalType.SelectedIndex);
            balancePropForm.saveValues(fs); //CurrencyValues, shownFields
            fs.SaveToFile(tools.dataDir + @"MainForm.fs");
            mainForm = null;
        }

        private void tsmiDicUsers_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.dicUsers == AccessMode.NONE)
                {
                    MessageBox.Show("У вас недостаточно прав для работы со справочником пользователей.");
                }
                else
                {
                    UsersDicForm udf = null;
                    foreach (Form form in this.MdiChildren)
                    {
                        if (form is UsersDicForm)
                        {
                            udf = (UsersDicForm)form;
                            if (udf.WindowState == FormWindowState.Minimized)
                            {
                                udf.WindowState = FormWindowState.Normal;
                            }
                            udf.BringToFront();
                            break;
                        }
                    }


                    if (udf == null)
                    {
                        udf = new UsersDicForm();
                        MenuRegisterWindow("Справочник: Пользователи", udf);
                        udf.MdiParent = this;
                        udf.Show();
                    }
                }
            /*
            }
            else
            {
                MessageBox.Show("Для работы со справочником пользователей необходимо подключиться к БД");
             
            }
             */
        }

        private void tsmiDicCurrencies_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.dicCurrency == AccessMode.NONE)
                {
                    MessageBox.Show("У вас недостаточно прав для работы со справочником валют.");
                }
                else
                {
                    CurrDicForm cdf = null;

                    foreach (Form form in this.MdiChildren)
                    {
                        if (form is CurrDicForm)
                        {
                            cdf = (CurrDicForm)form;
                            if (cdf.WindowState == FormWindowState.Minimized)
                            {
                                cdf.WindowState = FormWindowState.Normal;
                            }
                            cdf.BringToFront();
                            break;
                        }
                    }


                    if (cdf == null)
                    {
                        cdf = new CurrDicForm();
                        MenuRegisterWindow("Справочник: Валюты", cdf);
                        cdf.MdiParent = this;
                        cdf.Show();
                        balancePropForm.init();
                    }
                }
            /*
            }
            else
            {
                MessageBox.Show("Для работы со справочником валют необходимо подключиться к БД.");
            }
             */
        }

        private void tsmiAppSettings_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.appSettings == SimplePermission.PERMIT)
                {
                    //TODO Окно изменения настроек приложения
                    new SettingsForm().ShowDialog();

                    using (DbCommand cmd = Db.command("select title from kassa limit 1"))
                    {
                        try
                        {
                            Text = Convert.ToString(cmd.ExecuteScalar());
                        }
                        catch (Exception) { }
                    }

                    if (tools.isBoss) Text += " [++]";
                    
                }
                else
                {
                    MessageBox.Show("У вас недостаточно прав для изменения настроек приложения.");
                }
            /*
            }
            else
            {
                MessageBox.Show("Для изменения настроек приложения необходимо подключиться к БД.");
            }
             */
        }

        private void tsmiDicClients_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.dicClients == AccessMode.NONE)
                {
                    MessageBox.Show("У вас недостаточно прав для работы со справочником контрагентов.");
                }
                else
                {
                    ClientsDicForm cdf = null;

                    foreach (Form form in this.MdiChildren)
                    {
                        if (form is ClientsDicForm)
                        {
                            cdf = (ClientsDicForm)form;
                            if (cdf.WindowState == FormWindowState.Minimized)
                            {
                                cdf.WindowState = FormWindowState.Normal;
                            }
                            cdf.BringToFront();
                            break;
                        }
                    }


                    if (cdf == null)
                    {
                        cdf = new ClientsDicForm();
                        MenuRegisterWindow("Справочник: Контрагенты", cdf);
                        cdf.MdiParent = this;
                        cdf.Show();
                    }
                }
            /*
            }
            else
            {
                MessageBox.Show("Для работы со справочником контрагентов необходимо подключиться к БД.");
            }
             */
        }

        private void tsmiDicOps_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.dicOps == AccessMode.NONE)
                {
                    MessageBox.Show("У вас недостаточно прав для работы со справочником операций.");
                }
                else
                {
                    OpsDicForm odf = null;

                    foreach (Form form in this.MdiChildren)
                    {
                        if (form is OpsDicForm)
                        {
                            odf = (OpsDicForm)form;
                            if (odf.WindowState == FormWindowState.Minimized)
                            {
                                odf.WindowState = FormWindowState.Normal;
                            }
                            odf.BringToFront();
                            break;
                        }
                    }


                    if (odf == null)
                    {
                        odf = new OpsDicForm();
                        MenuRegisterWindow("Справочник: Операции", odf);
                        odf.MdiParent = this;
                        odf.Show();
                    }
                }
            /*
            }
            else
            {
                MessageBox.Show("Для работы со справочником операций необходимо подключиться к БД.");
            }
             */
        }

        private void tsmiDicFirmAcc_Click(object sender, EventArgs e)
        {
            /*
            if (tools.currentMode != Tools.DbMode.NONE)
            {
             */
                if (tools.currentUser.prefs.dicFirmAcc == AccessMode.NONE)
                {
                    MessageBox.Show("У вас недостаточно прав для работы со справочником фирм и счетов.");
                }
                else
                {
                    FirmAccDicForm fadf = null;

                    foreach (Form form in this.MdiChildren)
                    {
                        if (form is FirmAccDicForm)
                        {
                            fadf = (FirmAccDicForm)form;
                            if (fadf.WindowState == FormWindowState.Minimized)
                            {
                                fadf.WindowState = FormWindowState.Normal;
                            }
                            fadf.BringToFront();
                            break;
                        }
                    }


                    if (fadf == null)
                    {
                        fadf = new FirmAccDicForm();
                        MenuRegisterWindow("Справочник: Фирмы и счета", fadf);
                        fadf.MdiParent = this;
                        fadf.Show();
                        balancePropForm.init();
                    }
                }
            /*
            }
            else
            {
                MessageBox.Show("Для работы со справочником фирм и счетов необходимо подключиться к БД.");
            }
             */
        }
        
        private void OpenFirmWindow(object sender, EventArgs e) 
        {
            //TODO OpenFirmWindow
            int firmId = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            foreach (Form fc in MdiChildren)
            {
                if (fc is FirmForm && ((FirmForm)fc).firmId == firmId)
                {
                    fc.BringToFront();
                    return;
                }
            }

            FirmForm ff = new FirmForm(firmId);
            ff.MdiParent = this;
            ff.Show();
            MenuRegisterWindow("Фирма: " + ff.Text, ff);
        }

        #region Члены AppFunctions

        public void RefreshMenuFirms()
        {
            tsmiFirmsList.DropDownItems.Clear();
            try
            {
                string where = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.FIRM, "id");
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from firms where " + where + " order by title", tools.connection));

                using (DataTable dt = Db.fillTable(Db.command("select * from firms where " + where + " order by title")))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            ToolStripMenuItem tsmi = new ToolStripMenuItem(r["title"].ToString());
                            tsmi.Tag = Convert.ToInt32(r["id"]);
                            tsmi.Click += new EventHandler(OpenFirmWindow);
                            tsmiFirmsList.DropDownItems.Add(tsmi);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public void MenuRegisterWindow(string title, object item)
        {
            foreach (ToolStripMenuItem tsmi in tsmiWindows.DropDownItems)
            {
                if (tsmi.Tag == item)
                {
                    tsmi.Text = title;
                    return;
                }
            }

            ToolStripMenuItem tsmi2 = new ToolStripMenuItem(title);
            tsmi2.Tag = item;
            tsmi2.Click += new EventHandler(MenuWindowClick);
            tsmiWindows.DropDownItems.Add(tsmi2);
        }

        void MenuWindowClick(object sender, EventArgs e)
        {
            try
            {
                Form f = (Form)((ToolStripMenuItem)sender).Tag;
                if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
                f.BringToFront();
            }
            catch (Exception) { }
        }

        public void MenuUnregisterWindow(object item)
        {
            List<ToolStripMenuItem> ltsmi = new List<ToolStripMenuItem>();

            foreach (ToolStripMenuItem tsmi in tsmiWindows.DropDownItems)
            {
                if (tsmi.Tag == item) ltsmi.Add(tsmi);
            }

            foreach(ToolStripMenuItem tsmi in ltsmi) tsmiWindows.DropDownItems.Remove(tsmi);

        }

        public void EditRecord(int recid, bool cloneRec)
        {
            /*
            if (tools.checkConnection(tools.connection, false))
            {
             */
                if (recForm.initOperation(recid, cloneRec))
                {
                    if (recForm.ShowDialog() == DialogResult.OK)
                    {
                        NotifyDataChanged(recForm.oldRecord, recForm.newRecord, recForm.oldRecord == null ? DataAction.NEW : DataAction.EDIT);
                    }
                }
            /*
            }
             */
        }

        public void NotifyDataChanged(KassaRecord oldRecord, KassaRecord newRecord, DataAction action)
        {
            try
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form is DataFlow) ((DataFlow)form).OnJournalChanged(oldRecord, newRecord, action);
                }
            }
            catch (Exception) { }
        }

        #endregion

        private void tsmiNewRec_Click(object sender, EventArgs e)
        {
            EditRecord(-1, false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Insert || keyData == Keys.F11 || keyData == Keys.F12)
            {
                EditRecord(-1, false);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tsmiBalance_Click(object sender, EventArgs e)
        {
            BalanceForm bf = new BalanceForm();
            bf.MdiParent = this;
            bf.Show();
            MenuRegisterWindow(bf.Text, bf);
        }

        private void cbBalFrom_CheckedChanged(object sender, EventArgs e)
        {
            deBalStart.Enabled = cbBalFrom.Checked;
        }

        #region Форма баланса




        private void bBalanceParameters_Click(object sender, EventArgs e)
        {
            balancePropForm.init();
            balancePropForm.ShowDialog();
        }

        DataTable dtData = null;
        BindingSource bsData = null;
        int lastBType = -1;

        private void bBalanceFold_Click(object sender, EventArgs e)
        {

            if (tools.tmDataChanges.checkTableChanged()) tools.currentUser.prefs.needRebuildRuleMapping = true;
            
            if (cbBalType.SelectedIndex == 0) calcBalanceTypeCurrencies();
            else if (cbBalType.SelectedIndex == 1) calcBalanceTypeBanktags();
        }
        
        void calcBalanceTypeCurrencies() 
        {
            string er = "";

            try
            {
                if (gc.Visible && lastBType > -1) gv.SaveLayoutToXml(tools.dataDir + @"\balance_layout" + lastBType.ToString() + ".xml", olg);
            }
            catch (Exception) { }

            gc.SuspendLayout();

            int currCnt = 0;

            gv.Columns.Clear();

            // Собрать информацию о валютах
            dtData = new DataTable();
            dtData.Columns.Add(new DataColumn("id", typeof(int)));
            dtData.Columns.Add(new DataColumn("title", typeof(string)));

            gv.Columns.Add(createGColumn("title", "title", "Фирма / Счёт", SummaryItemType.Sum, gv.Columns.Count, false));

            string acc_in = "";

            // Перечень полей разных валют
            List<string> colst = new List<string>();

            // Добавляем в таблицу и сетку поля
            foreach (string sf in balancePropForm.sfields)
            {
                if (balancePropForm.dAccId.ContainsKey(sf))
                {
                    if (acc_in != "") acc_in += ", ";
                    acc_in += balancePropForm.dAccId[sf];

                    string curfld = "c" + balancePropForm.dAccCurr[sf].ToString();
                    if (!dtData.Columns.Contains(curfld))
                    {
                        dtData.Columns.Add(new DataColumn(curfld, typeof(double)));
                        gv.Columns.Add(createGColumn(curfld, curfld, balancePropForm.dCurTitle[balancePropForm.dAccCurr[sf]], SummaryItemType.Sum, gv.Columns.Count, true));
                        colst.Add(curfld);
                        currCnt++;
                    }
                }
            }

            if (currCnt == 0) er += "* Не указан ни один счёт\n";

            dtData.Columns.Add(new DataColumn("ito", typeof(double)));
            gv.Columns.Add(createGColumn("ito", "ito", "Итог", SummaryItemType.Sum, gv.Columns.Count, true));

            bsData = new BindingSource();
            bsData.DataSource = dtData;
            gc.DataSource = bsData;
            gc.MainView = gv;

            balancePropForm.dIdRow.Clear();

            // Добавляем все счета в таблицу и инициализируем поля валют в 0
            foreach (string sf in balancePropForm.sfields)
            {
                if (balancePropForm.dAccId.ContainsKey(sf))
                {
                    DataRow nr = dtData.NewRow();
                    nr["id"] = balancePropForm.dAccId[sf];
                    nr["title"] = balancePropForm.dAccTitle[sf];
                    foreach (string cols in colst) nr[cols] = 0.0f;
                    nr["ito"] = 0.0f;
                    dtData.Rows.Add(nr);
                    balancePropForm.dIdRow[balancePropForm.dAccId[sf]] = nr;
                }
            }

            string sql = "select id, op_id, r_sum, srctype, src_acc_id, src_curr_value, " +
                "dsttype, dst_acc_id, dst_curr_value from journal where ((srctype = 0 and src_acc_id in (" + acc_in +
                ")) or (dsttype = 0 and dst_acc_id in (" + acc_in + "))) and deleted = 0 and ";

            DateTime d2 = deBalEnd.Value;
            DateTime d1 = cbBalFrom.Checked ? deBalStart.Value : d2;

            if (d1 > d2)
            {
                DateTime dx = d1;
                d1 = d2;
                d2 = dx;
            }

            if (d1 == d2) sql += "r_date <= '" + d1.ToString("yyyyMMdd") + "' ";
            else sql += "(r_date >= '" + d1.ToString("yyyyMMdd") + "' and r_date <= '" + d2.ToString("yyyyMMdd") + "') ";

            if (er == "")
            {
                BalanceCalcCur bc = new BalanceCalcCur();
                foreach (string sf in balancePropForm.sfields)
                {
                    if (balancePropForm.dAccId.ContainsKey(sf))
                    {
                        bc.AddAccount(balancePropForm.dAccId[sf], balancePropForm.dAccCurr[sf], 0);

                    }
                }

                foreach (StringObjTagItem soti in balancePropForm.lbCurr.Items)
                {
                    CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                    bc.currencies[cd.id] = cd.value;
                }


                try
                {
//                    DataTable dt = tools.MySqlFillTable(new MySqlCommand(sql, tools.connection));

                    using (DataTable dt = Db.fillTable(Db.command(sql)))
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                int srctype = 0, dsttype = 0, src_acc_id = 0, dst_acc_id = 0;
                                double src_curr_value = 1.0f, dst_curr_Value = 1.0f;

                                int.TryParse(dr["srctype"].ToString(), out srctype);
                                int.TryParse(dr["dsttype"].ToString(), out dsttype);
                                int.TryParse(dr["src_acc_id"].ToString(), out src_acc_id);
                                int.TryParse(dr["dst_acc_id"].ToString(), out dst_acc_id);
                                double.TryParse(dr["src_curr_value"].ToString(), out src_curr_value);
                                double.TryParse(dr["dst_curr_Value"].ToString(), out dst_curr_Value);

                                //01.02.2016
                                if (src_curr_value == 0) src_curr_value = 1;
                                if (dst_curr_Value == 0) dst_curr_Value = 1;
                                //01.02.2016 /

                                bc.ProcessOperation(Convert.ToDouble(dr["r_sum"]), srctype, src_acc_id, src_curr_value, dsttype, dst_acc_id, dst_curr_Value);
                            }
                            catch (Exception) { }
                        }
                    }

                    double ItoCurrValue = 1;
                    foreach (StringObjTagItem soti in balancePropForm.lbCurr.Items)
                    {
                        CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                        if (cd.id == -2)
                        {
                            ItoCurrValue = cd.value;
                            break;
                        }
                    }

                    foreach (KeyValuePair<int, DataRow> kvp in balancePropForm.dIdRow)
                    {
                        DataRow dr = kvp.Value;
                        dr["c" + bc.accounts[kvp.Key].currId.ToString()] = bc.accounts[kvp.Key].balance;
                        dr["ito"] = bc.accounts[kvp.Key].balance * bc.currencies[bc.accounts[kvp.Key].currId] / ItoCurrValue;
                    }
                }
                catch (Exception) { }
            }
            else MessageBox.Show("Ошибки:\n\n" + er);

            lastBType = 0;

            try
            {
                gv.RestoreLayoutFromXml(tools.dataDir + @"\balance_layout" + lastBType.ToString() + ".xml", olg);
            }
            catch (Exception) { }


            gc.ResumeLayout();
            gc.Visible = dtData != null && dtData.Rows.Count > 0;

        }

        void calcBalanceTypeBanktags()
        {
            string er = "";

            try
            {
                if (gc.Visible && lastBType > -1) gv.SaveLayoutToXml(tools.dataDir + @"\balance_layout" + lastBType.ToString() + ".xml", olg);
            }
            catch (Exception) { }

            gc.SuspendLayout();
            gv.Columns.Clear();
            dtData = new DataTable();
            bsData = new BindingSource();
            bsData.DataSource = dtData;
            gc.DataSource = bsData;
            gc.MainView = gv;

            string acc_in = "";
            int currCnt = 0;

            // Собираем с окна настроек список выбранных счетов
            foreach (string sf in balancePropForm.sfields)
            {
                if (balancePropForm.dAccId.ContainsKey(sf))
                {
                    if (acc_in != "") acc_in += ", ";
                    acc_in += balancePropForm.dAccId[sf];
                    currCnt++;
                }
            }

            if (currCnt == 0) er += "* Не указан ни один счёт\n";

            string sql = "select id, op_id, r_sum, srctype, src_acc_id, src_curr_value, " +
                "dsttype, dst_acc_id, dst_curr_value from journal where ((srctype = 0 and src_acc_id in (" + acc_in +
                ")) or (dsttype = 0 and dst_acc_id in (" + acc_in + "))) and deleted = 0 and ";

            DateTime d2 = deBalEnd.Value;
            DateTime d1 = cbBalFrom.Checked ? deBalStart.Value : d2;

            if (d1 > d2)
            {
                DateTime dx = d1;
                d1 = d2;
                d2 = dx;
            }

            if (d1 == d2) sql += "r_date <= '" + d1.ToString("yyyyMMdd") + "' ";
            else sql += "(r_date >= '" + d1.ToString("yyyyMMdd") + "' and r_date <= '" + d2.ToString("yyyyMMdd") + "') ";

            if (er == "")
            {
                BalanceCalcBT bbt = new BalanceCalcBT();

                bbt.currnames = balancePropForm.dCurTitle;

                foreach (string sf in balancePropForm.sfields)
                {
                    if (balancePropForm.dAccId.ContainsKey(sf))
                    {
                        bbt.AddAccount(balancePropForm.dAccId[sf], balancePropForm.dAccCurr[sf], balancePropForm.dAccBt[sf]);
                    }
                }

                try
                {
                    //DataTable dtfa = tools.MySqlFillTable(new MySqlCommand("select id, firm_id from accounts", tools.connection));

                    using (DataTable dtfa = Db.fillTable(Db.command("select id, firm_id from accounts")))
                    {
                        foreach (DataRow r in dtfa.Rows)
                        {
                            int acc_id, firm_id;
                            if (int.TryParse(r["id"].ToString(), out acc_id) && int.TryParse(r["firm_id"].ToString(), out firm_id))
                            {
                                bbt.firmsaccs[acc_id] = firm_id;
                            }
                        }
                    }

                    //dtfa = tools.MySqlFillTable(new MySqlCommand("select id, title from firms", tools.connection));

                    Dictionary<int, string> dIdFirms = new Dictionary<int, string>();
                    using (DataTable dtfa = Db.fillTable(Db.command("select id, title from firms")))
                    {
                        foreach (DataRow r in dtfa.Rows)
                        {
                            int firm_id;
                            if (int.TryParse(r["id"].ToString(), out firm_id))
                            {
                                dIdFirms[firm_id] = r["title"].ToString();
                            }
                        }
                    }

//                    DataTable dt = tools.MySqlFillTable(new MySqlCommand(sql, tools.connection));

                    using (DataTable dt = Db.fillTable(Db.command(sql)))
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                int srctype = 0, dsttype = 0, src_acc_id = 0, dst_acc_id = 0;
                                double src_curr_value = 1.0f, dst_curr_Value = 1.0f;

                                int.TryParse(dr["srctype"].ToString(), out srctype);
                                int.TryParse(dr["dsttype"].ToString(), out dsttype);
                                int.TryParse(dr["src_acc_id"].ToString(), out src_acc_id);
                                int.TryParse(dr["dst_acc_id"].ToString(), out dst_acc_id);
                                double.TryParse(dr["src_curr_value"].ToString(), out src_curr_value);
                                double.TryParse(dr["dst_curr_Value"].ToString(), out dst_curr_Value);

                                bbt.ProcessOperation(Convert.ToDouble(dr["r_sum"]), srctype, src_acc_id, src_curr_value, dsttype, dst_acc_id, dst_curr_Value);
                            }
                            catch (Exception) { }
                        }
                    }

                    dtData.Columns.Add(new DataColumn("title", typeof(string)));

                    gv.Columns.Add(createGColumn("title", "title", "Фирма", SummaryItemType.Count, gv.Columns.Count, false));

                    Dictionary<string, string> btf = bbt.getBankTagFields();
                    foreach (KeyValuePair<string, string> kvp in btf)
                    {
                        dtData.Columns.Add(new DataColumn(kvp.Key, typeof(double)));
                        gv.Columns.Add(createGColumn(kvp.Key, kvp.Key, kvp.Value, SummaryItemType.Sum, gv.Columns.Count, true));
                    }
                    
                    dtData.Columns.Add(new DataColumn("ito", typeof(double)));
                    gv.Columns.Add(createGColumn("ito", "ito", "Итог", SummaryItemType.Sum, gv.Columns.Count + 1, true));

                    foreach (KeyValuePair<int, Dictionary<string, Dictionary<int, double>>> kvpfirm in bbt.fgroups)
                    {
                        DataRow dr = dtData.NewRow();
                        dr["title"] = dIdFirms.ContainsKey(kvpfirm.Key) ? dIdFirms[kvpfirm.Key] : "?";

                        foreach (KeyValuePair<string, string> kvp in btf) dr[kvp.Key] = 0; // Инициализируем поля
                        double ito = 0; // Итог фирмы в рублях

                        foreach (KeyValuePair<string, Dictionary<int, double>> kvpgroup in kvpfirm.Value)
                        { // банковская группа и набор пар <код валюты, сумма>
                            foreach(KeyValuePair <int, double> kvpcurrsum in kvpgroup.Value)
                            {
                                string fname = kvpgroup.Key.GetHashCode().ToString() + "_" + kvpcurrsum.Key.ToString();
                                
                                double tcv = 0;
                                try
                                {
                                    tcv = Convert.ToDouble(dr[fname]);
                                }
                                catch (Exception) { }

                                double cource = 1;
                                if (balancePropForm.dcc.ContainsKey(kvpcurrsum.Key))
                                {
                                    cource = balancePropForm.dcc[kvpcurrsum.Key];
                                }
                                if (cource <= 0) cource = 1;

                                tcv += kvpcurrsum.Value * cource;
                                
                                dr[fname] = tcv;
                                ito += tcv;
                            }
                        }

                        if (balancePropForm.dcc.ContainsKey(balancePropForm.rubCurrId))
                        {
                            ito *= balancePropForm.dcc[balancePropForm.rubCurrId];
                        }
                        dr["ito"] = ito;

                        dtData.Rows.Add(dr);
                    }

                }
                catch (Exception) { }


            }
            else MessageBox.Show("Ошибки:\n\n" + er);

            lastBType = 1;

            try
            {
                gv.RestoreLayoutFromXml(tools.dataDir + @"\balance_layout" + lastBType.ToString() + ".xml", olg);
            }
            catch (Exception) { }


            gc.ResumeLayout();
            gc.Visible = dtData != null && dtData.Rows.Count > 0;

        }

        GridColumn createGColumn(string colName, string fieldName, string colCaption, SummaryItemType sit, int vi, bool currency)
        {
            GridColumn ret = new GridColumn();
            ret.Name = colName;
            ret.FieldName = fieldName;
            ret.Caption = colCaption;
            ret.SummaryItem.SummaryType = sit;
            ret.Visible = true;
            ret.VisibleIndex = vi;
            if (currency)
            {
                ret.Tag = "N2";
            }
            return ret;
        }

        #endregion

        private void gv_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (!String.IsNullOrEmpty((string)e.Column.Tag))
            {
                try
                {

                    e.DisplayText = Convert.ToDouble(e.Value).ToString((string)e.Column.Tag);
                }
                catch (Exception) { }
            }
        }

        private void gv_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (!String.IsNullOrEmpty((string)e.Info.Column.Tag))
            {
                try
                {
                    e.Info.DisplayText = Convert.ToDouble(e.Info.Value).ToString((string)e.Info.Column.Tag);
                }
                catch (Exception) { }
            }
        }

        Color clrRedMinus = Color.FromArgb(0xff, 0xc0, 0xc0);

        private void gv_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.ColumnType == typeof(double) && (double)e.CellValue < 0)
            {
                e.Appearance.BackColor = clrRedMinus;
            }
        }

        private void tsmiIbank2Import_Click(object sender, EventArgs e)
        {
            ExchangeImportForm ib2if = new ExchangeImportForm("ibank2");
            ib2if.ShowDialog();
        }

        private void tsmiJournal_Click(object sender, EventArgs e)
        {
            foreach (Form fc in MdiChildren)
            {
                if (fc is ReclogForm)
                {
                    fc.BringToFront();
                    return;
                }
            }


            ReclogForm rf = new ReclogForm();
            rf.MdiParent = this;
            rf.Show();
            MenuRegisterWindow(rf.Text, rf);
        }

    }
}
