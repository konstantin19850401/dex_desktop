using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using DEXExtendLib;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Globalization;
using DevExpress.Data;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using System.IO;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid;
using System.Data.Common;

namespace Kassa3
{
    public partial class FirmForm : Form, DataFlow
    {
        Tools tools;
        FormState fs;

        List<string> sfields;
        public int firmId;
        Dictionary<int, string> dOwnAcc = new Dictionary<int, string>();
        Dictionary<int, string> dOp = new Dictionary<int, string>();
        Dictionary<int, string> dCli = new Dictionary<int, string>();
        Dictionary<int, string> dAcc = new Dictionary<int, string>();
        Dictionary<int, string> dUsers = new Dictionary<int, string>();

        string opid_in, accid_in;
        DateTime d1, d2;

        string lastControlSignature = "";

        DataTable dtData = null;
        BindingSource bsData = null;

        bool firstShown = false;
        OptionsLayoutGrid olg;


        System.Timers.Timer timer;

        int focusedRowHandle = -1;

        public FirmForm(int firmId)
        {
            InitializeComponent();

            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new ElapsedEventHandler(OnSelectionIntervalExpires);
            timer.Enabled = false;
            timer.AutoReset = false;

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

            sfields = new List<string>();
            tools = Tools.instance;
            this.firmId = firmId;

            fs = new FormState(tools.dataDir + @"\FirmForm" + firmId.ToString() + ".fs");
            fs.LoadValue("deStart", deStart);
            fs.LoadValue("deEnd", deEnd);
            fs.LoadValue("cbEnd", cbEnd);
            cbEnd_CheckedChanged(cbEnd, null);
            fs.LoadValue("cbShowInternal", cbShowInternal);
            fs.LoadValue("cbShowBalance", cbShowBalance);
            fs.LoadValue("cbShowDeleted", cbShowDeleted);

            try
            {
                sfields.AddRange(fs.getValue("shownFields", "").Split('|'));
            }
            catch (Exception) { }

            prepareFields();
            gc.Visible = false;

            //Создание таблицы
            createDataTable();

            try
            {
//                Text = Convert.ToString(new MySqlCommand("select title from firms where id = " + firmId.ToString(), tools.connection).ExecuteScalar());

                using(DbCommand cmd = Db.command("select title from firms where id = " + firmId.ToString()))
                {
                    Text = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                Text = "?";
            }

            tssl.Text = "";
        }

        void prepareFields()
        {
            clbColumns.Items.Clear();
            try
            {
                clbColumns.Items.Add(new StringTagItem("№ операции", "id"), sfields.Contains("id"));
                clbColumns.Items.Add(new StringTagItem("Дата операции", "r_date"), sfields.Contains("r_date"));
                clbColumns.Items.Add(new StringTagItem("Клиент/Счёт", "cl_title"), sfields.Contains("cl_title"));
                clbColumns.Items.Add(new StringTagItem("Операция", "op_title"), sfields.Contains("op_title"));
                clbColumns.Items.Add(new StringTagItem("Примечания", "r_prim"), sfields.Contains("r_prim"));
                clbColumns.Items.Add(new StringTagItem("Автор записи", "user_cr"), sfields.Contains("user_cr"));
                clbColumns.Items.Add(new StringTagItem("Автор изменений", "user_ch"), sfields.Contains("user_ch"));

                string where = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.ACCOUNT, "id");
                /*
                MySqlCommand cmd = new MySqlCommand("select * from accounts where firm_id = @firm_id and (" + where + ")", tools.connection);
                tools.SetDbParameter(cmd, "firm_id", firmId);
                DataTable dt = tools.MySqlFillTable(cmd);
                 */

                DbCommand cmd = Db.command("select * from accounts where firm_id = @firm_id and (" + where + ")");
                Db.param(cmd, "firm_id", firmId);
                DataTable dt = Db.fillTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        string accstr = "a" + r["id"].ToString();
                        if (cbShowBalance.Checked)
                        {
                            clbColumns.Items.Add(new StringTagItem(r["title"].ToString(), accstr + "b"), sfields.Contains(accstr + "b"));
                        }
                        else
                        {
                            clbColumns.Items.Add(new StringTagItem(r["title"].ToString() + " - Приход", accstr + "p"), sfields.Contains(accstr + "p"));
                            clbColumns.Items.Add(new StringTagItem(r["title"].ToString() + " - Расход", accstr + "r"), sfields.Contains(accstr + "r"));
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        string getControlSignature()
        {
            string ret = "deStart=" + deStart.Value.ToString("yyyyMMdd") + "\n" +
                "deEnd=" + deEnd.Value.ToString("yyyyMMdd") + "\n" +
                "cbEnd=" + cbEnd.Checked.ToString() + "\n" +
                "cbShowInternal=" + cbShowInternal.Checked.ToString() + "\n" +
                "cbShowBalance=" + cbShowBalance.Checked.ToString() + "\n" +
                "cbShowDeleted=" + cbShowDeleted.Checked.ToString() + "\n" +
                "sfields=";
            
            string sall = "";
            foreach (string s in sfields)
            {
                if (sall != "") sall += "|";
                sall += s;
            }
            ret += sall;

            return ret;
        }

        private void FirmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string s = "";
                foreach ( string sf in sfields )
                {
                    if ( s.Length > 0 )
                        s += "|";
                    s += sf;
                }

                fs.SaveValue("shownFields", s);
                fs.SaveValue("deStart", deStart);
                fs.SaveValue("deEnd", deEnd);
                fs.SaveValue("cbEnd", cbEnd);
                fs.SaveValue("cbShowInternal", cbShowInternal);
                fs.SaveValue("cbShowBalance", cbShowBalance);
                fs.SaveValue("cbShowDeleted", cbShowDeleted);
                fs.UpdateFromForm(this);
                fs.SaveToFile(tools.dataDir + @"\FirmForm" + firmId.ToString() + ".fs");


                try
                {
                    if ( gc.Visible )
                        bgv.SaveLayoutToXml(tools.dataDir + @"\account" + firmId.ToString() + "_layout.xml", olg);
                }
                catch ( Exception )
                {
                }


                MainForm.mainForm.MenuUnregisterWindow(this);
            }
            catch ( Exception )
            {
            }
        }

        private void clbColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                string tfield = ((StringTagItem)clbColumns.Items[e.Index]).Tag;
                if (e.NewValue == CheckState.Checked)
                {
                    if (!sfields.Contains(tfield)) sfields.Add(tfield);
                }
                else
                {
                    if (sfields.Contains(tfield)) sfields.Remove(tfield);
                }
            }
            catch (Exception) { }
          
        }

        void createDataTable()
        {
            dtData = new DataTable();
            dtData.Columns.Add("id", typeof(int));
            dtData.Columns.Add("op_title", typeof(string));
            dtData.Columns.Add("r_date", typeof(DateTime));
            dtData.Columns.Add("r_prim", typeof(string));
            dtData.Columns.Add("cl_title", typeof(string));
            dtData.Columns.Add("user_cr", typeof(string));
            dtData.Columns.Add("user_ch", typeof(string));
            dtData.Columns.Add("access", typeof(bool));
            dtData.Columns.Add("unread_id", typeof(int));
            dtData.Columns.Add("deleted", typeof(bool));
        }

        bool showInternal, showBalance;
        bool prepareRow(DataRow nr, DataRow row)
        {
            try
            {
                return prepareRow(nr, KassaRecord.fromDataRow(row));
            }
            catch (Exception) { }

            return false;
        }

        bool prepareRow(DataRow nr, KassaRecord kr)
        {
            bool recAccess = true;
            bool doShowRec = true;

            nr["id"] = kr.id;
            try
            {
                nr["op_title"] = dOp[kr.op_id];
            }
            catch (Exception)
            {
                nr["op_title"] = "[Нет доступа к операции]";
                recAccess = false;
            }

            try
            {
                nr["r_date"] = kr.r_date;
            }
            catch (Exception) { }

            nr["r_prim"] = kr.r_prim;

            try
            {
                nr["user_cr"] = dUsers[kr.user_cr];
            }
            catch (Exception)
            {
                nr["user_cr"] = "[?]";
            }

            try
            {
                nr["user_ch"] = dUsers[kr.user_ch];
            }
            catch (Exception)
            {
                nr["user_ch"] = "[?]";
            }

            double r_sum = kr.r_sum;

            try
            {
                foreach (KeyValuePair<int, string> kvp in dOwnAcc)
                {
                    string fbase = "a" + kvp.Key.ToString();
                    nr[fbase + "p"] = 0;
                    nr[fbase + "r"] = 0;
                    nr[fbase + "b"] = 0;
                }


                int srctype = kr.srctype, dsttype = kr.dsttype;
                if (srctype == 0 && dsttype == 0)
                { // Со счёта на счёт
                    int src_acc_id = kr.src_acc_id, dst_acc_id = kr.dst_acc_id;

                    // Не показывать запись, если она внутренняя и запрещено отображение внутренних записей
                    if (dOwnAcc.ContainsKey(src_acc_id) && dOwnAcc.ContainsKey(dst_acc_id) && !showInternal) doShowRec = false;

                    if (doShowRec)
                    {
                        bool showByRashod = false;

                        if (dOwnAcc.ContainsKey(src_acc_id))
                        {
                            string fbase = "a" + src_acc_id.ToString();
                            nr[fbase + "p"] = 0;
                            nr[fbase + "r"] = r_sum;
                            nr[fbase + "b"] = -r_sum;

                            if ((sfields.Contains(fbase + "r") && !showBalance) || (sfields.Contains(fbase + "b") && showBalance)) showByRashod = true;
                        }

                        double src_curr_value = kr.src_curr_value, dst_curr_value = kr.dst_curr_value;

                        //01.02.2016
                        if (src_curr_value == 0) src_curr_value = 1;
                        if (dst_curr_value == 0) dst_curr_value = 1;
                        //01.02.2016 /

                        bool showByPrihod = false;

                        if (dOwnAcc.ContainsKey(dst_acc_id))
                        {
                            double dest_sum = r_sum * src_curr_value / dst_curr_value; // Переводим в рубли по курсу, а затем из рублей в конечную валюту
                            string fbase = "a" + dst_acc_id.ToString();
                            nr[fbase + "p"] = dest_sum;
                            nr[fbase + "r"] = 0;
                            nr[fbase + "b"] = dest_sum;

                            if ((sfields.Contains(fbase + "p") && !showBalance) || (sfields.Contains(fbase + "b") && showBalance)) showByPrihod = true;
                        }

                        string cl_title = dAcc.ContainsKey(src_acc_id) ? dAcc[src_acc_id] : "[Нет доступа]";
                        cl_title += " >> ";
                        cl_title += dAcc.ContainsKey(dst_acc_id) ? dAcc[dst_acc_id] : "[Нет доступа]";
                        cl_title += string.Format(" (Курс: {0} к {1})",
                            src_curr_value.ToString("G", CultureInfo.InvariantCulture),
                            dst_curr_value.ToString("G", CultureInfo.InvariantCulture)
                            );

                        nr["cl_title"] = cl_title;

                        if (!dAcc.ContainsKey(src_acc_id) || !dAcc.ContainsKey(dst_acc_id)) recAccess = false;
                        if (!showByPrihod && !showByRashod) doShowRec = false;
                    }
                }
                else if (srctype == 1)
                { // От контрагента на счёт (Приход)

                    int src_cli_id = kr.src_client_id, dst_acc_id = kr.dst_acc_id;

                    if (dOwnAcc.ContainsKey(dst_acc_id))
                    {
                        string fbase = "a" + dst_acc_id.ToString();
                        nr[fbase + "p"] = r_sum;
                        nr[fbase + "r"] = 0;
                        nr[fbase + "b"] = r_sum;
                        if ((!sfields.Contains(fbase + "p") && !showBalance) || (!sfields.Contains(fbase + "b") && showBalance)) doShowRec = false;
                    }

                    nr["cl_title"] = dCli.ContainsKey(src_cli_id) ? dCli[src_cli_id] : "[Нет доступа]";
                }
                else if (dsttype == 1)
                { // Со счёта контрагенту (Расход)
                    int src_acc_id = kr.src_acc_id, dst_cli_id = kr.dst_client_id;
                    if (dOwnAcc.ContainsKey(src_acc_id))
                    {
                        string fbase = "a" + src_acc_id.ToString();
                        nr[fbase + "p"] = 0;
                        nr[fbase + "r"] = r_sum;
                        nr[fbase + "b"] = -r_sum;
                        if ((!sfields.Contains(fbase + "r") && !showBalance) || (!sfields.Contains(fbase + "b") && showBalance)) doShowRec = false;
                    }

                    nr["cl_title"] = dCli.ContainsKey(dst_cli_id) ? dCli[dst_cli_id] : "[Нет доступа]";
                }

            }
            catch (Exception) { }


            nr["access"] = recAccess;
            nr["unread_id"] = kr.unreadId;
            nr["deleted"] = kr.deleted;

            //if (kr.deleted && !cbShowDeleted.Checked) doShowRec = false;

            return doShowRec;
        }

        void refreshProgress(string msg, int value)
        {
            tspb.ProgressBar.Value = value;
            tssl.Text = msg;
            ss.Invalidate();
            Application.DoEvents();
        }


        void refreshView()
        {
            focusedRowHandle = -1;
            timer.Stop();

            tspb.ProgressBar.Maximum = 7;
            refreshProgress("Подготовка", 0);

            //1
            firstShown = true;
            lastControlSignature = getControlSignature();

            showInternal = cbShowInternal.Checked;
            showBalance = cbShowBalance.Checked;
            try
            {
                if (gc.Visible) bgv.SaveLayoutToXml(tools.dataDir + @"\account" + firmId.ToString() + "_layout.xml", olg);
            }
            catch (Exception) { }

            try
            {
//                Text = Convert.ToString(new MySqlCommand("select title from firms where id = " + firmId.ToString(), tools.connection).ExecuteScalar());
                using (DbCommand cmd = Db.command("select title from firms where id = " + firmId.ToString()))
                {
                    Text = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                Text = "?";
            }

            MainForm.mainForm.MenuRegisterWindow("Фирма: " + Text, this);

            if (tools.tmDataChanges.checkTableChanged()) tools.currentUser.prefs.needRebuildRuleMapping = true;

            // Выборка из журнала операций
            dOp.Clear();
            opid_in = tools.currentUser.prefs.getIdListForRuleType(OpRuleType.OPERATION);
            try
            {
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from ops where id in (" + opid_in + ")", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select * from ops where id in (" + opid_in + ")")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        dOp[Convert.ToInt32(r["id"])] = r["title"].ToString();
                    }
                }
            }
            catch (Exception) { }

            refreshProgress("Выборка из журнала клиентов", 1);
            //2
            // Выборка из журнала клиентов
            dCli.Clear();
            try
            {
                string where = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.CLIENT, "id");
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from client_data where " + where, tools.connection));

                using (DataTable dt = Db.fillTable(Db.command("select * from client_data where " + where)))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        dCli[Convert.ToInt32(r["id"])] = r["title"].ToString();
                    }
                }
            }
            catch (Exception) { }

            refreshProgress("Выборка из журнала счетов", 2);
            //3
            // Выборка из журнала счетов
            accid_in = tools.currentUser.prefs.getIdListForRuleType(OpRuleType.ACCOUNT, -1); //firmId
            dAcc.Clear();
            dOwnAcc.Clear();
            try
            {
                string sql = "SELECT ac.id AS id, ac.title AS actitle, ac.firm_id, fi.title AS fititle " +
                                "FROM  `accounts` AS ac,  `firms` AS fi " +
                                "WHERE ac.firm_id = fi.id and ac.id in (" + accid_in + ")";
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand(sql, tools.connection));
                
                using (DataTable dt = Db.fillTable(Db.command(sql)))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        int fid = Convert.ToInt32(r["firm_id"]);
                        string dname = r["actitle"].ToString();

                        if (fid != firmId) dname = r["fititle"].ToString() + " - " + dname;
                        else dOwnAcc[Convert.ToInt32(r["id"])] = r["actitle"].ToString();

                        dAcc[Convert.ToInt32(r["id"])] = dname;
                    }
                }
            }
            catch (Exception) { }
            accid_in = tools.currentUser.prefs.getIdListForRuleType(OpRuleType.ACCOUNT, firmId); //firmId

            refreshProgress("Выборка из журнала пользователей", 3);
            //4
            // Выборка из журнала пользователей
            dUsers.Clear();
            try
            {
//                DataTable dtu = tools.MySqlFillTable(new MySqlCommand("select id, login from users", tools.connection));
                
                using (DataTable dtu = Db.fillTable(Db.command("select id, login from users")))
                {
                    foreach (DataRow dr in dtu.Rows)
                    {
                        dUsers[Convert.ToInt32(dr["id"])] = dr["login"].ToString();
                    }
                }
            }
            catch (Exception) { }

            refreshProgress("Подготовка таблицы", 4);
            //5
            // Подготовка таблицы
            gc.SuspendLayout();

            createDataTable();

            foreach (KeyValuePair<int, string> kvp in dOwnAcc)
            {
                string fbase = "a" + kvp.Key.ToString();
                dtData.Columns.Add(fbase + "p", typeof(double));
                dtData.Columns.Add(fbase + "r", typeof(double));
                dtData.Columns.Add(fbase + "b", typeof(double));
            }

            d1 = deStart.Value;
            d2 = cbEnd.Checked ? deEnd.Value : deStart.Value;
            
            if (d1 > d2)
            {
                DateTime dx = d1;
                d1 = d2;
                d2 = dx;
            }

            if (d1 == d2) Text += " [" + d1.ToString("dd.MM.yyyy") + "]";
            else Text += " [" + d1.ToString("dd.MM.yyyy") + " - " + d2.ToString("dd.MM.yyyy") + "]";
            MainForm.mainForm.MenuRegisterWindow(Text, this);

            string showdel = "";
            if (!cbShowDeleted.Checked) showdel = " and j.deleted = 0 ";

            /*
            MySqlCommand cmda = new MySqlCommand(
                "select j.*, nr.id as nr_id from `journal` j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = @nr_user_id " +
                "where j.op_id in (" + opid_in + ") and j.r_date >= @startdate and j.r_date <= @enddate " + showdel +
                "and (((j.srctype = 0) and (j.src_acc_id in (" + accid_in + "))) or ((j.dsttype = 0) and (j.dst_acc_id in (" + accid_in + "))))",
                tools.connection);

            tools.SetDbParameter(cmda, "startdate", d1.ToString("yyyyMMdd"));
            tools.SetDbParameter(cmda, "enddate", d2.ToString("yyyyMMdd"));
            tools.SetDbParameter(cmda, "nr_user_id", tools.currentUser.PID);

            DataTable dtJou = tools.MySqlFillTable(cmda);
             */

            string sql2 =
                "select j.*, nr.id as nr_id from `journal` j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = @nr_user_id " +
                "where j.op_id in (" + opid_in + ") and j.r_date >= @startdate and j.r_date <= @enddate " + showdel +
                "and (((j.srctype = 0) and (j.src_acc_id in (" + accid_in + "))) or ((j.dsttype = 0) and (j.dst_acc_id in (" + accid_in + "))))";

            DbCommand cmda = Db.command(sql2);
            cmda.CommandTimeout = 28800; //на всякий случай
            Db.param(cmda, "startdate", d1.ToString("yyyyMMdd"));
            Db.param(cmda, "enddate", d2.ToString("yyyyMMdd"));
            Db.param(cmda, "nr_user_id", tools.currentUser.PID);

            DataTable dtJou = null;
            try
            {
                dtJou = Db.fillTable(cmda);
            }
            catch (Exception)
            {
            }
            //dtJou = Db.fillTable(cmda);

            refreshProgress("Заполнение таблицы", 5);
            //6
            if (dtJou != null && dtJou.Rows.Count > 0)
            {
                foreach (DataRow row in dtJou.Rows)
                {
                    DataRow nr = dtData.NewRow();
                    if (prepareRow(nr, row)) dtData.Rows.Add(nr);
                }
            }

            refreshProgress("Форматирование таблицы", 6);
            //7
            bsData = new BindingSource();
            bsData.DataSource = dtData;

            bgv.Columns.Clear();
            bgv.Bands.Clear();


            GridBand gbCommon = bgv.Bands.AddBand("Сведения");
            gbCommon.Name = "gbCommon";

            gbCommon.Columns.Add(createBGColumn("id", "id", "№ операции", SummaryItemType.None, false, FilterPopupMode.Default));
            gbCommon.Columns.Add(createBGColumn("op_title", "op_title", "Операция", SummaryItemType.None, false, FilterPopupMode.CheckedList));
            gbCommon.Columns.Add(createBGColumn("r_date", "r_date", "Дата", SummaryItemType.None, false, FilterPopupMode.DateSmart));
            gbCommon.Columns.Add(createBGColumn("r_prim", "r_prim", "Примечание", SummaryItemType.None, false, FilterPopupMode.Default));
            gbCommon.Columns.Add(createBGColumn("cl_title", "cl_title", "Клиент/Счёт", SummaryItemType.None, false, FilterPopupMode.CheckedList));

            foreach (KeyValuePair<int, string> kvp in dOwnAcc)
            {
                GridBand gbAcc = bgv.Bands.AddBand(kvp.Value);
                gbAcc.Name = "gbAcc" + kvp.Key.ToString();

                string fbase = "a" + kvp.Key.ToString();
                gbAcc.Columns.Add(createBGColumn(fbase + "p", fbase + "p", "Приход", SummaryItemType.Sum, true, FilterPopupMode.Default));
                gbAcc.Columns.Add(createBGColumn(fbase + "r", fbase + "r", "Расход", SummaryItemType.Sum, true, FilterPopupMode.Default));
                gbAcc.Columns.Add(createBGColumn(fbase + "b", fbase + "b", "Баланс", SummaryItemType.Sum, true, FilterPopupMode.Default));
            }


            GridBand gbUser = bgv.Bands.AddBand("Пользователь");
            gbUser.Name = "gbUser";
            gbUser.Columns.Add(createBGColumn("user_cr", "user_cr", "Создал", SummaryItemType.None, false, FilterPopupMode.CheckedList));
            gbUser.Columns.Add(createBGColumn("user_ch", "user_ch", "Изменил", SummaryItemType.None, false, FilterPopupMode.CheckedList));


            try
            {
                bgv.RestoreLayoutFromXml(tools.dataDir + @"\account" + firmId.ToString() + "_layout.xml", olg);
/*
                if (gc.Visible)
                    tools.loadBandedGridState(bgv, SimpleXML.LoadXml(File.ReadAllText(
                        tools.dataDir + @"\account" + firmId.ToString() + "_grid.xml", Encoding.UTF8)));
 */
            }
            catch (Exception) 
            {
                string s = "";
            }


            int maxIndex = -1;
            foreach (BandedGridColumn bgc in bgv.Columns)
            {
                if (bgc.VisibleIndex > maxIndex) maxIndex = bgc.VisibleIndex;
            }
            
            foreach (BandedGridColumn bgc in bgv.Columns)
            {
                bgc.Visible = sfields.Contains(bgc.FieldName);
                if (bgc.Visible && bgc.VisibleIndex < 0) bgc.VisibleIndex = ++maxIndex;
            }

            foreach (KeyValuePair<int, string> kvp in dOwnAcc)
            {
                string fbase = "a" + kvp.Key.ToString();
                try
                {
                    if (showBalance)
                    {
                        bgv.Columns.ColumnByFieldName(fbase + "p").Visible = false;
                        bgv.Columns.ColumnByFieldName(fbase + "r").Visible = false;
                    }
                    else
                    {
                        bgv.Columns.ColumnByFieldName(fbase + "b").Visible = false;
                    }
                }
                catch (Exception) 
                {
                    string s = "";
                }
            }

            gc.DataSource = bsData;

            refreshProgress("", 0);

            cleanupBands();

            gc.ResumeLayout();
            gc.Visible = dtData != null && dtData.Rows.Count > 0;
            bExport.Enabled = gc.Visible;
            bPrint.Enabled = gc.Visible;
        }

        void cleanupBands()
        {
            List<GridBand> gblist = new List<GridBand>();

            foreach (GridBand gband in bgv.Bands)
            {
                bool doRemove = true;
                foreach (BandedGridColumn bgcol in gband.Columns)
                {
                    if (bgcol.Visible)
                    {
                        doRemove = false;
                        break;
                    }
                }

                if (doRemove) gblist.Add(gband);
 
            }

            foreach (GridBand gband in gblist) bgv.Bands.Remove(gband);
        }

        BandedGridColumn createBGColumn(string colName, string fieldName, string colCaption, SummaryItemType sit, bool currency, FilterPopupMode filterPopupMode)
        {
            BandedGridColumn ret = new BandedGridColumn();
            ret.Name = colName;
            ret.FieldName = fieldName;
            ret.Caption = colCaption;
            ret.SummaryItem.SummaryType = sit;
            ret.OptionsFilter.FilterPopupMode = filterPopupMode;
            ret.Visible = true;
            if (currency)
            {
                ret.Tag = "N2";
            }

            return ret;
        }

        /*
        private void bgv_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            String fname = (e.Item as GridSummaryItem).FieldName;
            GridView view = sender as GridView;

            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                e.TotalValue = 0;
            }

            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                bool isDeleted = (bool)view.GetRowCellValue(e.RowHandle, "deleted");
                if (!isDeleted)
                {
                    e.TotalValue = Convert.ToDecimal(e.TotalValue) + Convert.ToDecimal(e.FieldValue);
                }
            }
            
            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                e.TotalValue = "is okay?";
            }
        }
        */

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                refreshView();
            }
            catch (Exception)
            {
                string s = "";
            }
            /*if (tools.checkConnection(tools.connection, false))*/ 
        }

        void doEditRecord()
        {
            if (bgv.SelectedRowsCount > 0)
            {
                int[] seld = bgv.GetSelectedRows();
                DataRowView drv = (DataRowView)bgv.GetRow(seld[0]);
                if (!Convert.ToBoolean(drv.Row["deleted"]))
                {
                    if (Convert.ToBoolean(drv.Row["access"]))
                    {

                        if ((SimplePermission)tools.currentUser.prefs.fieldsEdit == SimplePermission.PROHIBIT && drv.Row["user_cr"].ToString() != tools.currentUser.Login )
                        {

                          
                                MessageBox.Show("Вам запрещено редактировать чужие записи");
                            
                        }
                        else
                        {
                            MainForm.mainForm.EditRecord(Convert.ToInt32(drv.Row["id"]), false);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не можете редактировать эту запись, так как Вам запрещен доступ к одному или нескольким её параметрам.");
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно редактировать удалённую запись.");
                }
            }
        }

        private void gc_DoubleClick(object sender, EventArgs e)
        {
            doEditRecord();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!getControlSignature().Equals(lastControlSignature))
                {
                    bRefreshList.UseVisualStyleBackColor = !bRefreshList.UseVisualStyleBackColor;
                    if (!bRefreshList.UseVisualStyleBackColor) bRefreshList.BackColor = Color.Red;
                }
                else
                {
                    bRefreshList.UseVisualStyleBackColor = true;
                }
            }
            catch (Exception) 
            {
                bRefreshList.UseVisualStyleBackColor = true;
            }

            bRefreshList.Invalidate();
        }

        void doCloneRecord()
        {
            if (bgv.SelectedRowsCount > 0)
            {
                int[] seld = bgv.GetSelectedRows();
                DataRowView drv = (DataRowView)bgv.GetRow(seld[0]);
                if (Convert.ToBoolean(drv.Row["access"]))
                {
                    MainForm.mainForm.EditRecord(Convert.ToInt32(drv.Row["id"]), true);
                }
                else
                {
                    MessageBox.Show("Вы не можете дублировать эту запись, так как Вам запрещен доступ к одному или нескольким её параметрам.");
                }
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete && bgv.SelectedRowsCount > 0)
            {
                try
                {
                    int[] seld = bgv.GetSelectedRows();
                    DataRowView drv = (DataRowView)bgv.GetRow(seld[0]);

                    if (Convert.ToBoolean(drv.Row["deleted"]))
                    {
                        MessageBox.Show("Нельзя удалить запись, поскольку она уже удалена", "Предупреждение", MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int delId = Convert.ToInt32(drv.Row["id"]);
                            /*
                            MySqlCommand cmd = new MySqlCommand(
                                "select j.*, nr.id as nr_id from journal j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = @nr_user_id where j.id = @id",
                                tools.connection
                                );
                            tools.SetDbParameter(cmd, "id", delId);
                            tools.SetDbParameter(cmd, "nr_user_id", tools.currentUser.PID);
                            DataTable dt = tools.MySqlFillTable(cmd);
                             */

                            DbCommand cmd = Db.command("select j.*, nr.id as nr_id from journal j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = @nr_user_id where j.id = @id");
                            Db.param(cmd, "id", delId);
                            Db.param(cmd, "nr_user_id", tools.currentUser.PID);
                            DataTable dt = Db.fillTable(cmd);

                            if (dt != null && dt.Rows.Count > 0)
                            {

                                KassaRecord kr = KassaRecord.fromDataRow(dt.Rows[0]);
                                /*
                                cmd = new MySqlCommand("update journal set deleted = 1 where id = @id", tools.connection);
                                tools.SetDbParameter(cmd, "id", delId);
                                 */
                                cmd = Db.command("update journal set deleted = 1 where id = @id");
                                Db.param(cmd, "id", delId);
                                cmd.ExecuteNonQuery();

                                /*
                                cmd = new MySqlCommand("delete from `new_recs` where rec_id = @rec_id", tools.connection);
                                tools.SetDbParameter(cmd, "rec_id", delId);
                                 */
                                cmd = Db.command("delete from `new_recs` where rec_id = @rec_id");
                                Db.param(cmd, "rec_id", delId);
                                cmd.ExecuteNonQuery();

                                MainForm.mainForm.NotifyDataChanged(kr, kr, DataAction.DELETE);
                            }
                        }
                    }
                }
                catch (Exception) { }
            } else
                if (keyData == Keys.F5 && bgv.SelectedRowsCount > 0)
                {
                    try
                    {
                        doCloneRecord();
                        return true;
                    }
                    catch (Exception) { }
                }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Члены DataFlow
        public void OnJournalChanged(KassaRecord oldRecord, KassaRecord newRecord, DataAction action)
        {
            if (action == DataAction.DELETE)
            {
                try
                {
                    DataRow[] rr = dtData.Select("id = " + oldRecord.id.ToString());
                    if (cbShowDeleted.Checked)
                    {
                        foreach (DataRow r in rr) r["deleted"] = true;
                    }
                    else
                    {
                        foreach (DataRow r in rr) dtData.Rows.Remove(r);
                    }
                }
                catch (Exception) { }
            }
            else if (action == DataAction.EDIT || action == DataAction.NEW)
            {
                if (!firstShown) refreshView();

                bool doShowRec = true;

                // Сначала убедимся, что запись подходит 
                DataRow nr = dtData.NewRow();
                bool wasNew = true;

                try
                {
                    DataRow[] drsel = dtData.Select("id = " + newRecord.id.ToString());
                    if (drsel != null && drsel.Length > 0)
                    {
                        nr = drsel[0];
                        wasNew = false;
                    }
                }
                catch (Exception) { }

                try
                {
                    doShowRec = prepareRow(nr, newRecord);
                }
                catch (Exception)
                {
                    doShowRec = false;
                }

                if (doShowRec)
                {
                    if (wasNew) dtData.Rows.Add(nr);
                    try
                    {
                        bgv.UnselectRow(bgv.FocusedRowHandle);
                        int rowhandle = bgv.LocateByValue("id", newRecord.id);
                        bgv.SelectRow(rowhandle);
                        bgv.FocusedRowHandle = rowhandle;
                    }
                    catch (Exception) { }
                }
                else
                {
                    if (!wasNew)
                    {
                        try
                        {
                            dtData.Rows.Remove(nr);
                        }
                        catch (Exception) { }
                    }
                }

            }
            gc.Visible = dtData != null && dtData.Rows.Count > 0;
            bExport.Enabled = gc.Visible;
        
        }

        #endregion

        private void FirmForm_Load(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
        }

        private void bExport_Click(object sender, EventArgs e)
        {
//            Point p = bExport.PointToScreen(new Point(0, bExport.Size.Height));
//            cmsExport.Show(p);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 1) bgv.ExportToXls(sfd.FileName);
                else if (sfd.FilterIndex == 2) bgv.ExportToXlsx(sfd.FileName);
            }
        }

        private void cbShowBalance_CheckedChanged(object sender, EventArgs e)
        {
            prepareFields();
        }

        private void formatCurrencyField(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
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

        void GridControlViewController_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
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

        
        Font fBold = null; //new Font("Courier New", 10, FontStyle.Bold);

        private void bgv_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            DataRowView drv = (DataRowView)(sender as ColumnView).GetRow(e.RowHandle);

            if (Convert.ToInt32(drv["unread_id"]) > -1)
            {
                if (fBold == null)
                {
                    fBold = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                e.Appearance.Font = fBold;
            }

            if (Convert.ToBoolean(drv["deleted"]))
            {
                e.Appearance.ForeColor = Color.Red;
            }

            if (e.Column.ColumnType == typeof(double) && (double)e.CellValue < 0) 
            {
//                e.Handled = true;
                e.Appearance.BackColor = clrRedMinus;

                if (Convert.ToBoolean(drv["deleted"]))
                {
                    e.Appearance.ForeColor = Color.Gray;
                }
            }
        }


        private void bgv_DoubleClick(object sender, EventArgs e)
        {
        }

        private void bgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space /*|| e.KeyCode == Keys.Return*/)
            {
                doEditRecord();
            }
            else if (e.KeyCode == Keys.Home)
            {
                bgv.MoveFirst();
            }
            else if (e.KeyCode == Keys.End)
            {
                bgv.MoveLast();
            }

        }

        private void clbColumns_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool seld = clbColumns.CheckedItems.Count == 0;
                for (int i = 0; i < clbColumns.Items.Count; ++i) clbColumns.SetItemChecked(i, seld);
            }
        }

        void refreshSelectDate() {
            Dictionary<int, Dictionary<int, Dictionary<int, string>>> dd = new Dictionary<int, Dictionary<int, Dictionary<int, string>>>();
            try
            {
                /*
                string accid_in = tools.currentUser.prefs.getIdListForRuleType(OpRuleType.ACCOUNT, firmId);
                MySqlCommand cmd = new MySqlCommand("select r_date from `journal` where " + 
                    "(((srctype = 0) and (src_acc_id in (" + accid_in + "))) or ((dsttype = 0) and (dst_acc_id in (" + accid_in + ")))) group by r_date order by r_date",
                    tools.connection);
*/
/*
            accid_in = tools.currentUser.prefs.getIdListForRuleType(OpRuleType.ACCOUNT, firmId);
            dAcc.Clear();
            dOwnAcc.Clear();
            try
            {
                string sql = "SELECT ac.id AS id, ac.title AS actitle, ac.firm_id, fi.title AS fititle " +
                                "FROM  `accounts` AS ac,  `firms` AS fi " +
                                "WHERE ac.firm_id = fi.id and ac.id in (" + accid_in + ")";
                */
            }
            catch (Exception) { }
        }

        private void cmsExport_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tsmiXls2000_Click(object sender, EventArgs e)
        {
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (gc.Visible) gc.ShowPrintPreview();
            }
            catch (Exception) { }
        }

        private void cbEnd_CheckedChanged(object sender, EventArgs e)
        {
            deEnd.Enabled = cbEnd.Checked;
        }


        private void bgv_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                timer.Stop();
                ColumnView cv = (sender as ColumnView);
                DataRowView drv = (DataRowView)cv.GetRow(e.FocusedRowHandle);
                if (Convert.ToInt32(drv["unread_id"]) > -1)
                {
                    focusedRowHandle = e.FocusedRowHandle;
                    timer.Start();
                }
                else
                {
                    focusedRowHandle = -1;
                }
            }
            catch (Exception) { }

        }

        void OnSelectionIntervalExpires(object source, ElapsedEventArgs e)
        {
            if (focusedRowHandle > -1)
            {
                try
                {
                    DataRowView drv = (DataRowView)((ColumnView)bgv).GetRow(focusedRowHandle);
                    if (drv != null)
                    {
                        int unread_id = Convert.ToInt32(drv["unread_id"]);
                        if (unread_id > -1)
                        {
                            /*
                            MySqlCommand cmd = new MySqlCommand("delete from new_recs where id = @unread_id", tools.connection);
                            tools.SetDbParameter(cmd, "unread_id", unread_id);
                            cmd.ExecuteNonQuery();
                             */

                            using (DbCommand cmd = Db.command("delete from new_recs where id = @unread_id"))
                            {
                                Db.param(cmd, "unread_id", unread_id);
                                cmd.ExecuteNonQuery();
                            }

                            drv["unread_id"] = -1;
                            bgv.RefreshRow(focusedRowHandle);
                        }
                    }
                }
                catch (Exception) { }

                focusedRowHandle = -1;

            }
        }


    }
}
