using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using DEXExtendLib;
using System.Globalization;
using System.Collections.Specialized;
using System.IO;
using System.Data.Common;

namespace Kassa3
{
    public partial class RecForm : Form
    {
        Tools tools;

        public int opid;
        List<StringObjTagItem> lsotiSrcAcc, lsotiDstAcc, lsotiSrcCli, lsotiDstCli;
        StringObjTagItem sotiSrcAccSel, sotiDstAccSel, sotiSrcCliSel, sotiDstCliSel;

        Dictionary<int, int> dAccCurr;
        int rubCurrId;

        bool isNew = true;

        DateTime lastDateUsed = DateTime.Now;

        public KassaRecord oldRecord, newRecord;

        AutoCompleteStringCollection scRPrim;

        PrimForm primForm = new PrimForm();

        DataRow curRow = null;

        string o_deRDate, o_cbOp, o_cbSrcType, o_cbSrc, o_cbDstType, o_cbDst, o_tbRSum, o_tbRPrim;
        decimal o_nudSrcCurrValue, o_nudDstCurrValue;

        public RecForm()
        {
            InitializeComponent();
            tools = Tools.instance;
            // Восстановить значения полей "Дата", "Операция", "Тип источника", "Источник", "Тип приёмника", "Приёмник", "Курс валюты"
            lsotiSrcAcc = new List<StringObjTagItem>();
            lsotiDstAcc = new List<StringObjTagItem>();
            lsotiSrcCli = new List<StringObjTagItem>();
            lsotiDstCli = new List<StringObjTagItem>();
            sotiSrcAccSel = null;
            sotiDstAccSel = null;
            sotiSrcCliSel = null;
            sotiDstCliSel = null;
            cbSrcType.SelectedIndex = 0;
            cbDstType.SelectedIndex = 0;
            scRPrim = new AutoCompleteStringCollection();
            tbRPrim.AutoCompleteCustomSource = scRPrim;
            try {
                //string[] ss = File.ReadAllLines(tools.dataDir + @"\prim.cache");
                //scRPrim.AddRange(ss);
                using (DataTable dtfa = Db.fillTable(Db.command("select text from prim_cache")))
                {
                    foreach (DataRow r in dtfa.Rows)
                    {
                        scRPrim.Add(r["text"].ToString());
                    }
                }
            } catch (Exception) { }
        }



        /// <summary>
        /// Инициализация операции
        /// </summary>
        /// <param name="opid"></param>
        public bool initOperation(int opid, bool cloneRec)
        {
            oldRecord = null;

            lCurrCode.Text = "";
            lsotiSrcAcc.Clear();
            lsotiDstAcc.Clear();
            lsotiSrcCli.Clear();
            lsotiDstCli.Clear();
            sotiSrcAccSel = null;
            sotiDstAccSel = null;
            sotiSrcCliSel = null;
            sotiDstCliSel = null;


            // Получим ID валюты "российский рубль"

            try
            {
                /*
                MySqlCommand cmdGR = new MySqlCommand("select id from curr_list where curr_id = @curr_id", tools.connection);
                tools.SetDbParameter(cmdGR, "curr_id", Tools.DEF_CURRENCY);
                rubCurrId = Convert.ToInt32(cmdGR.ExecuteScalar());
                 */

                using (DbCommand cmd = Db.command("select id from curr_list where curr_id = @curr_id"))
                {
                    Db.param(cmd, "curr_id", Tools.DEF_CURRENCY);
                    rubCurrId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception) { 
                rubCurrId = -1;
            }


            // Заполним список операций

            string opsWhere = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.OPERATION, "id");
            /*
            DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from ops where " + 
                opsWhere + " order by title", tools.connection));
             */
            using (DataTable dt = Db.fillTable(Db.command("select * from ops where " + opsWhere + " order by title")))
            {
                StringTagItem.UpdateCombo(cbOp, dt, null, "id", "title", true);
            }


            // Создадим список фирм и счетов

            string accwhere = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.FIRM, "id");
            /*
            DataTable dtf = tools.MySqlFillTable(new MySqlCommand("select * from firms where " +
                accwhere + " order by title", tools.connection));
            */
            using (DataTable dtf = Db.fillTable(Db.command("select * from firms where " + accwhere + " order by title")))
            {

                accwhere = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.ACCOUNT, "id");
                /*
                DataTable dta = tools.MySqlFillTable(new MySqlCommand("select * from accounts where " +
                    accwhere + " order by title", tools.connection));
                */

                using (DataTable dta = Db.fillTable(Db.command("select * from accounts where " + accwhere + " order by title")))
                {

                    dAccCurr = new Dictionary<int, int>();
                    foreach (DataRow r0 in dta.Rows) dAccCurr[Convert.ToInt32(r0["id"])] = Convert.ToInt32(r0["curr_id"]);

                    foreach (DataRow r1 in dtf.Rows)
                    {
                        int nid = Convert.ToInt32(r1["id"]);
                        string sCat = "[" + r1["title"].ToString() + "]";

                        DataRow[] ra = dta.Select("firm_id = " + nid.ToString());
                        foreach (DataRow r2 in ra)
                        {
                            int tagid1 = Convert.ToInt32(r2["id"]);
                            lsotiSrcAcc.Add(new StringObjTagItem(r2["title"].ToString() + " " + sCat, tagid1));
                            lsotiDstAcc.Add(new StringObjTagItem(r2["title"].ToString() + " " + sCat, tagid1));
                        }
                    }

                }
            }

            // Создадим список клиентов

            /*
            DataTable dtcat = tools.MySqlFillTable(new MySqlCommand("select * from client_cat ", 
                tools.connection));
            */

            using (DataTable dtcat = Db.fillTable(Db.command("select * from client_cat")))
            {

                string cliwhere = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.CLIENT, "id");
                /*
                DataTable dtcli = tools.MySqlFillTable(new MySqlCommand("select * from client_data where " +
                    cliwhere + " order by title", tools.connection));
                */

                using (DataTable dtcli = Db.fillTable(Db.command("select * from client_data where " + cliwhere + " order by title")))
                {
                    processCategory(-1, "", dtcat, dtcli);
                }
            }

            this.opid = opid;

            // Попробуем загрузить запись об операции с указанным ID
            /*
            dt = tools.MySqlFillTable(new MySqlCommand(
                "select j.*, nr.id as nr_id from journal j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = " +
                tools.currentUser.PID +" where j.deleted = 0 and j.id = " + opid.ToString(), tools.connection
                ));
             */
            using (DataTable dt = Db.fillTable(Db.command(
                "select j.*, nr.id as nr_id from journal j left join `new_recs` nr on nr.rec_id = j.id and nr.user_id = " +
                tools.currentUser.PID + " where j.deleted = 0 and j.id = " + opid.ToString())))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    curRow = row;

                    // Есть запись в БД
                    if (row.IsNull("locker_id"))
                    {
                        // Запись неблокирована

                        if (!cloneRec)
                        {
                            // Блокируем запись, если она уже существовала и мы не клонируем её
                            /*
                            MySqlCommand cmd = new MySqlCommand("update journal set locker_id = @locker_id, lock_till = @lock_till where id = @id", tools.connection);
                            tools.SetDbParameter(cmd, "locker_id", tools.currentUser.PID);
                            tools.SetDbParameter(cmd, "lock_till", DateTime.Now.Add(new TimeSpan(0, 30, 0)).ToString("yyyyMMddHHmmss"));
                            tools.SetDbParameter(cmd, "id", opid);
                            cmd.ExecuteNonQuery();
                             */
                            using (DbCommand cmd = Db.command("update journal set locker_id = @locker_id, lock_till = @lock_till where id = @id"))
                            {
                                Db.param(cmd, "locker_id", tools.currentUser.PID);
                                Db.param(cmd, "lock_till", DateTime.Now.Add(new TimeSpan(0, 30, 0)).ToString("yyyyMMddHHmmss"));
                                Db.param(cmd, "id", opid);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Заполняем поля
                        StringTagItem.SelectByTag(cbOp, row["op_id"].ToString(), true);

                        DateTime dtv = DateTime.Now;
                        DateTime.TryParseExact(row["r_date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtv);
                        deRDate.Value = dtv;

                        if (!row.IsNull("src_acc_id")) sotiSrcAccSel = findItem(lsotiSrcAcc, Convert.ToInt32(row["src_acc_id"]));
                        if (!row.IsNull("src_client_id")) sotiSrcCliSel = findItem(lsotiSrcCli, Convert.ToInt32(row["src_client_id"]));
                        if (!row.IsNull("dst_acc_id")) sotiDstAccSel = findItem(lsotiDstAcc, Convert.ToInt32(row["dst_acc_id"]));
                        if (!row.IsNull("dst_client_id")) sotiDstCliSel = findItem(lsotiDstCli, Convert.ToInt32(row["dst_client_id"]));
                        cbSrcType.SelectedIndex = Convert.ToInt32(row["srctype"]);
                        cbDstType.SelectedIndex = Convert.ToInt32(row["dsttype"]);

                        if (cloneRec)
                            tbRSum.Text = "";
                        else
                            tbRSum.Text = row["r_sum"].ToString();

                        nudSrcCurrValue.Value = Convert.ToDecimal(row["src_curr_value"]);
                        nudDstCurrValue.Value = Convert.ToDecimal(row["dst_curr_value"]);
                        tbRPrim.Text = row["r_prim"].ToString();

                        try
                        {
                            if (!cloneRec) oldRecord = KassaRecord.fromDataRow(row);
                        }
                        catch (Exception) { }

                        isNew = cloneRec;
                        if (cloneRec) this.opid = -1;
                    }
                    else
                    {
                        // Запись блокирована
                        // Возвращаем false
                        /*
                        string uname = Convert.ToString(
                            new MySqlCommand("select login from users where id = " + row["locker_id"].ToString(), tools.connection
                                ).ExecuteScalar());
                         */

                        using (DbCommand cmd = Db.command("select login from users where id = " + row["locker_id"].ToString()))
                        {
                            string uname = Convert.ToString(cmd.ExecuteScalar());

                            if (uname == string.Empty) uname = "Неустановленный пользователь";

                            MessageBox.Show(string.Format("Пользователь <{0}> в данный момент уже редактирует эту запись.", uname));
                        }
                        return false;
                    }
                }
                else
                {
                    // Записи в БД нет - операция новая
                    curRow = null;

                    // Заполняем поля
                    tbRSum.Text = "";

                    if (!deRDate.Value.ToString("dd.MM.yyyy").Equals(deRDate.Text)) deRDate.Value = DateTime.Now;

                    isNew = true;
                }
            }

            cbSrcType_SelectedIndexChanged(cbSrcType, null);
            cbDstType_SelectedIndexChanged(cbDstType, null);

            deRDate.Focus();

            if (!isNew)
            {
                o_deRDate = deRDate.Text;
                o_cbOp = cbOp.Text;
                o_cbSrcType = cbSrcType.Text;
                o_cbSrc = cbSrc.Text;
                o_cbDstType = cbDstType.Text;
                o_cbDst = cbDst.Text;
                o_tbRSum = tbRSum.Text;
                o_tbRPrim = tbRPrim.Text;
                o_nudSrcCurrValue = nudSrcCurrValue.Value;
                o_nudDstCurrValue = nudDstCurrValue.Value;
            }

            return true;
        }


        void processCategory(long cat_id, string path, DataTable dtcat, DataTable dtdata)
        {
            DataRow[] catrows = dtcat.Select("parent_id = " + cat_id.ToString());
            if (catrows != null && catrows.Length > 0)
            {
                foreach (DataRow catrow in catrows)
                {
                    string npath = path + (string.IsNullOrEmpty(path) ? "" : " > ") + catrow["cat_title"].ToString();

                    DataRow[] clirows = dtdata.Select("cat_id = " + catrow["id"].ToString());
                    if (clirows != null && clirows.Length > 0)
                    {
                        foreach (DataRow clirow in clirows)
                        {
                            int iTag = Convert.ToInt32(clirow["id"]);
                            string sTitle = clirow["title"].ToString() + (string.IsNullOrEmpty(npath) ? "" : " [" + npath + "]");
                            lsotiSrcCli.Add(new StringObjTagItem(sTitle, iTag));
                            lsotiDstCli.Add(new StringObjTagItem(sTitle, iTag));
                        }
                    }

                    processCategory(Convert.ToInt32(catrow["id"]), npath, dtcat, dtdata);
                }
            }
        }

        StringObjTagItem findItem(List<StringObjTagItem> lsoti, int tag) 
        {
            try 
            {
                foreach(StringObjTagItem soti in lsoti) 
                {
                    if (soti.Tag.Equals(tag)) return soti;
                }
            } 
            catch(Exception) { }

            return null;
        }


        void checkSrcDstType()
        {
            if (cbSrcType.SelectedIndex == 0)
            {
                // Счёт
                if (!cbDstType.Enabled)
                {
                    cbDstType.Enabled = true;
                }
            }
            else
            {
                // Контрагент
                if (cbDstType.Enabled)
                {
                    cbDstType.SelectedIndex = 0;
                    cbDstType.Enabled = false;
                }
            }
        }

        void checkCurrencyVisible()
        {
            bool curvis = false, srcru = false, dstru = false;
            
            try
            {
                if (cbSrcType.SelectedIndex == 0 && cbDstType.SelectedIndex == 0 &&
                    cbSrc.SelectedItem != null && cbDst.SelectedItem != null)
                {
                    int srcCurrId = (int)((StringObjTagItem)cbSrc.SelectedItem).Tag;
                    int dstCurrId = (int)((StringObjTagItem)cbDst.SelectedItem).Tag;
                    
                    srcru = dAccCurr[srcCurrId] == rubCurrId;
                    dstru = dAccCurr[dstCurrId] == rubCurrId;
                    if (dAccCurr[srcCurrId] != dAccCurr[dstCurrId]) curvis = true;
                }
            }
            catch (Exception) { }

            label6.Visible = curvis && !srcru;
            nudSrcCurrValue.Visible = curvis && !srcru;
            bSrcCurrValueGet.Visible = curvis && !srcru;

            label7.Visible = curvis && !dstru;
            nudDstCurrValue.Visible = curvis && !dstru;
            bDstCurrValueGet.Visible = curvis && !dstru;
        }

        //true
        private void deRDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void cbSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsotiSrcCli.Contains((StringObjTagItem)cbSrc.SelectedItem)) sotiSrcCliSel = (StringObjTagItem)cbSrc.SelectedItem;
            else if (lsotiSrcAcc.Contains((StringObjTagItem)cbSrc.SelectedItem)) sotiSrcAccSel = (StringObjTagItem)cbSrc.SelectedItem;
            checkCurrencyVisible();
            checkCurrCode();
        }

        private void cbDst_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsotiDstCli.Contains((StringObjTagItem)cbDst.SelectedItem)) sotiDstCliSel = (StringObjTagItem)cbDst.SelectedItem;
            else if (lsotiDstAcc.Contains((StringObjTagItem)cbDst.SelectedItem)) sotiDstAccSel = (StringObjTagItem)cbDst.SelectedItem;
            checkCurrencyVisible();
            checkCurrCode();
        }


        private void bSrcCurrValueGet_Click(object sender, EventArgs e)
        {
            try
            {
                nudSrcCurrValue.Value = getCurrById(dAccCurr[(int)((StringObjTagItem)cbSrc.SelectedItem).Tag]);
            }
            catch (Exception) { }
        }

        private void bDstCurrValueGet_Click(object sender, EventArgs e)
        {
            try
            {
                nudDstCurrValue.Value = getCurrById(dAccCurr[(int)((StringObjTagItem)cbDst.SelectedItem).Tag]);
            }
            catch (Exception) { }
        }

        decimal getCurrById(int currid)
        {
            /*
            MySqlCommand cmd = new MySqlCommand(
                "select value from curr_values where currlist_id = @curr_id and date = " +
                "(select max(cv2.date) from curr_values as cv2 where cv2.date <= @date and cv2.currlist_id = @curr_id)", 
                tools.connection
                );

            tools.SetDbParameter(cmd, "date", deRDate.Value.ToString("yyyyMMdd"));
            tools.SetDbParameter(cmd, "curr_id", currid);
            */

            using(DbCommand cmd = Db.command(
                "select value from curr_values where currlist_id = @curr_id and date = " +
                "(select max(cv2.date) from curr_values as cv2 where cv2.date <= @date and cv2.currlist_id = @curr_id)"))
            {

                Db.param(cmd, "date", deRDate.Value.ToString("yyyyMMdd"));
                Db.param(cmd, "curr_id", currid);

                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        private void cbSrcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSrc.SuspendLayout();
            try
            {
                int v = cbSrcType.SelectedIndex;
                cbSrc.Items.Clear();
                cbSrc.Items.AddRange(v == 0 ? lsotiSrcAcc.ToArray() : lsotiSrcCli.ToArray());
                cbSrc.SelectedItem = v == 0 ? sotiSrcAccSel : sotiSrcCliSel;
            }
            catch (Exception) 
            {
                cbSrc.Items.Clear();
                cbSrc.SelectedItem = null;
            }
            cbSrc.ResumeLayout();

            cbSrc.Visible = cbSrc.Items.Count > 0;

            checkSrcDstType();
            checkCurrencyVisible();
            checkCurrCode();
        }

        private void cbDstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDst.SuspendLayout();
            try
            {
                int v = cbDstType.SelectedIndex;
                cbDst.Items.Clear();
                cbDst.Items.AddRange(v == 0 ? lsotiDstAcc.ToArray() : lsotiDstCli.ToArray());
                cbDst.SelectedItem = v == 0 ? sotiDstAccSel : sotiDstCliSel;
            }
            catch (Exception)
            {
                cbDst.Items.Clear();
                cbDst.SelectedItem = null;
            }
            cbDst.ResumeLayout();

            cbDst.Visible = cbDst.Items.Count > 0;

            checkSrcDstType();
            checkCurrencyVisible();
            checkCurrCode();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
                double dRSum = 0;
                string er = "";
                if ( !deRDate.Value.ToString("dd.MM.yyyy").Equals(deRDate.Text) )
                    er += "* Указана некорректная дата\n";
                if ( cbOp.SelectedItem == null )
                    er += "* Не указана операция\n";

                if ( cbSrcType.SelectedIndex < 0 )
                    er += "* Не указан тип источника\n";
                if ( cbSrc.SelectedItem == null )
                    er += "* Не указан источник\n";

                if ( cbDstType.SelectedIndex < 0 )
                    er += "* Не указан тип приёмника\n";
                if ( cbDst.SelectedItem == null )
                    er += "* Не указан приёмник\n";

                if ( cbSrcType.SelectedIndex == cbDstType.SelectedIndex &&
                    cbSrc.SelectedItem != null && cbDst.SelectedItem != null &&
                    ( (StringObjTagItem)cbSrc.SelectedItem ).Tag == ( (StringObjTagItem)cbDst.SelectedItem ).Tag )
                {
                    er += "* Источник и приёмник не должны быть одним счётом\n";
                }

                if ( nudSrcCurrValue.Visible && nudSrcCurrValue.Value <= 0 )
                    er += "* Валютный курс источника должен быть больше нуля\n";
                if ( nudDstCurrValue.Visible && nudDstCurrValue.Value <= 0 )
                    er += "* Валютный курс приёмника должен быть больше нуля\n";

                try
                {
                    String nsum = tbRSum.Text.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                    nsum = nsum.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                    dRSum = double.Parse(nsum, NumberStyles.Float);
                    if ( dRSum <= 0 )
                        er += "* Сумма операции должна быть больше нуля\n";
                }
                catch ( Exception )
                {
                    er += "* Указана некорректная сумма операции\n";
                }

                if ( er.Equals("") )
                {
                    // Проверка даты начала открытого периода
//                    string ld = Convert.ToString(new MySqlCommand("select legal_date from `kassa` limit 0,1", tools.connection).ExecuteScalar());

                    using (DbCommand cmd = Db.command("select legal_date from `kassa` limit 0,1"))
                    {
                        string ld = Convert.ToString(cmd.ExecuteScalar());
                        string rd = deRDate.Value.ToString("yyyyMMdd");
                        if (string.Compare(rd, ld) < 0)
                        {
                            er += "* Внутренний сбой 777. Заперещено изменять записи, в закрытом периоде";
                        }
                    }

                    //TODO Записать попытку сохранения новой записи 
                    //opLogTitle = deRDate.Text + ", №" + opid + ", " + cbOp.Text + ", [" + cbSrcType.Text + " => " + cbDstType.Text + "] " + tbRSum.Text;

                }

                if ( er.Equals("") )
                {
                    try
                    {
                        decimal srcCurrValue = 0, dstCurrValue = 0;
                        if ( cbSrcType.SelectedIndex == 0 && cbDstType.SelectedIndex == 0 )
                        {
                            srcCurrValue = dAccCurr[(int)sotiSrcAccSel.Tag] == rubCurrId ? 1 : nudSrcCurrValue.Value;
                            dstCurrValue = dAccCurr[(int)sotiDstAccSel.Tag] == rubCurrId ? 1 : nudDstCurrValue.Value;
                        }

                        string sql;

                        if ( isNew )
                        {
                            // Новая запись
                            sql = "insert into journal (op_id, r_date, r_sum, r_prim, srctype, src_acc_id, src_client_id, src_curr_value, " +
                                "dsttype, dst_acc_id, dst_client_id, dst_curr_value, user_cr) values (@op_id, @r_date, @r_sum, " +
                                "@r_prim, @srctype, @src_acc_id, @src_client_id, @src_curr_value, @dsttype, @dst_acc_id, @dst_client_id, " +
                                "@dst_curr_value, @user_cr)";
                        }
                        else
                        {
                            // Изменение записи (не забыть снять блокировку)
                            sql = "update journal set op_id = @op_id, r_date = @r_date, r_sum = @r_sum, r_prim = @r_prim, srctype = @srctype, " +
                                "src_acc_id = @src_acc_id, src_client_id = @src_client_id, src_curr_value = @src_curr_value, dsttype = @dsttype, " +
                                "dst_acc_id = @dst_acc_id, dst_client_id = @dst_client_id, dst_curr_value = @dst_curr_value, user_ch = @user_ch, " +
                                "locker_id = null, lock_till = null where id = @id";
                        }

                        /*
                        MySqlCommand cmd = new MySqlCommand(sql, tools.connection);
                        tools.SetDbParameter(cmd, "op_id", Convert.ToInt32(( (StringTagItem)cbOp.SelectedItem ).Tag));
                        tools.SetDbParameter(cmd, "r_date", deRDate.Value.ToString("yyyyMMdd"));
                        tools.SetDbParameter(cmd, "r_sum", dRSum);
                        tools.SetDbParameter(cmd, "r_prim", tbRPrim.Text);
                        tools.SetDbParameter(cmd, "srctype", cbSrcType.SelectedIndex);
                        tools.SetDbParameter(cmd, "src_acc_id", cbSrcType.SelectedIndex == 0 ? ( (StringObjTagItem)cbSrc.SelectedItem ).Tag : null);
                        tools.SetDbParameter(cmd, "src_client_id", cbSrcType.SelectedIndex == 1 ? ( (StringObjTagItem)cbSrc.SelectedItem ).Tag : null);
                        tools.SetDbParameter(cmd, "src_curr_value", srcCurrValue);
                        tools.SetDbParameter(cmd, "dsttype", cbDstType.SelectedIndex);
                        tools.SetDbParameter(cmd, "dst_acc_id", cbDstType.SelectedIndex == 0 ? ( (StringObjTagItem)cbDst.SelectedItem ).Tag : null);
                        tools.SetDbParameter(cmd, "dst_client_id", cbDstType.SelectedIndex == 1 ? ( (StringObjTagItem)cbDst.SelectedItem ).Tag : null);
                        tools.SetDbParameter(cmd, "dst_curr_value", dstCurrValue);

                        if
                            ( isNew )
                            tools.SetDbParameter(cmd, "user_cr", tools.currentUser.PID);
                        else
                            tools.SetDbParameter(cmd, "user_ch", tools.currentUser.PID);

                        if ( !isNew )
                            tools.SetDbParameter(cmd, "id", opid);
                        cmd.ExecuteNonQuery();
                        
                        if ( isNew )
                            opid = (int)cmd.LastInsertedId;
                        */

                        using (DbCommand cmd = Db.command(sql))
                        {
                            Db.param(cmd, "op_id", Convert.ToInt32(((StringTagItem)cbOp.SelectedItem).Tag));
                            Db.param(cmd, "r_date", deRDate.Value.ToString("yyyyMMdd"));
                            Db.param(cmd, "r_sum", dRSum);
                            Db.param(cmd, "r_prim", tbRPrim.Text);
                            Db.param(cmd, "srctype", cbSrcType.SelectedIndex);
                            Db.param(cmd, "src_acc_id", cbSrcType.SelectedIndex == 0 ? ((StringObjTagItem)cbSrc.SelectedItem).Tag : null);
                            Db.param(cmd, "src_client_id", cbSrcType.SelectedIndex == 1 ? ((StringObjTagItem)cbSrc.SelectedItem).Tag : null);
                            Db.param(cmd, "src_curr_value", srcCurrValue);
                            Db.param(cmd, "dsttype", cbDstType.SelectedIndex);
                            Db.param(cmd, "dst_acc_id", cbDstType.SelectedIndex == 0 ? ((StringObjTagItem)cbDst.SelectedItem).Tag : null);
                            Db.param(cmd, "dst_client_id", cbDstType.SelectedIndex == 1 ? ((StringObjTagItem)cbDst.SelectedItem).Tag : null);
                            Db.param(cmd, "dst_curr_value", dstCurrValue);

                            if (isNew)
                            {
                                Db.param(cmd, "user_cr", tools.currentUser.PID);
                            }
                            else
                            {
                                Db.param(cmd, "user_ch", tools.currentUser.PID);
                            }

                            if (!isNew)
                            {
                                Db.param(cmd, "id", opid);
                            }

                            cmd.ExecuteNonQuery();

                            if (isNew)
                            {
                                opid = (int)Db.LastInsertedId(cmd, "journal");
                            }
                        
                        }

                        newRecord = new KassaRecord();
                        newRecord.id = opid;
                        newRecord.op_id = Convert.ToInt32(( (StringTagItem)cbOp.SelectedItem ).Tag);
                        newRecord.r_date = deRDate.Value;
                        newRecord.r_sum = dRSum;
                        newRecord.r_prim = tbRPrim.Text;
                        newRecord.srctype = cbSrcType.SelectedIndex;
                        newRecord.src_acc_id = cbSrcType.SelectedIndex == 0 ? Convert.ToInt32(( (StringObjTagItem)cbSrc.SelectedItem ).Tag) : -1;
                        newRecord.src_client_id = cbSrcType.SelectedIndex == 1 ? Convert.ToInt32(( (StringObjTagItem)cbSrc.SelectedItem ).Tag) : -1;
                        newRecord.src_curr_value = Convert.ToDouble(srcCurrValue);
                        newRecord.dsttype = cbDstType.SelectedIndex;
                        newRecord.dst_acc_id = cbDstType.SelectedIndex == 0 ? Convert.ToInt32(( (StringObjTagItem)cbDst.SelectedItem ).Tag) : -1;
                        newRecord.dst_client_id = cbDstType.SelectedIndex == 1 ? Convert.ToInt32(( (StringObjTagItem)cbDst.SelectedItem ).Tag) : -1;
                        newRecord.dst_curr_value = Convert.ToDouble(dstCurrValue);
                        newRecord.user_cr = ( isNew || oldRecord == null ) ? tools.currentUser.PID : oldRecord.user_cr;
                        newRecord.user_ch = tools.currentUser.PID;

                        string logn = deRDate.Text + ", №" + opid + ", " + cbOp.Text + ", [" + cbSrc.Text + " => " + cbDst.Text + "], " + tbRSum.Text;
                        if ( !isNew )
                        {
                            logn += "\nИзменения:";
                            if ( !o_deRDate.Equals(deRDate.Text) )
                                logn += "\n* Дата операции: " + o_deRDate + " => " + deRDate.Text;
                            if ( !o_cbOp.Equals(cbOp.Text) )
                                logn += "\n* Операция: " + o_cbOp + " => " + cbOp.Text;
                            if ( !o_cbSrcType.Equals(cbSrcType.Text) )
                                logn += "\n* Тип источника: " + o_cbSrcType + " => " + cbSrcType.Text;
                            if ( !o_cbSrc.Equals(cbSrc.Text) )
                                logn += "\n* Источник: " + o_cbSrc + " => " + cbSrc.Text;
                            if ( o_nudSrcCurrValue != nudSrcCurrValue.Value )
                                logn += "\n* Курс источника к рублю: " + o_nudSrcCurrValue + " => " + nudSrcCurrValue.Value;
                            if ( !o_cbDstType.Equals(cbDstType.Text) )
                                logn += "\n* Тип приёмника: " + o_cbDstType + " => " + cbDstType.Text;
                            if ( !o_cbDst.Equals(cbDst.Text) )
                                logn += "\n* Приёмник: " + o_cbDst + " => " + cbDst.Text;
                            if ( o_nudDstCurrValue != nudDstCurrValue.Value )
                                logn += "\n* Курс приёмника к рублю: " + o_nudDstCurrValue + " => " + nudDstCurrValue.Value;
                            if ( !o_tbRSum.Equals(tbRSum.Text) )
                                logn += "\n* Сумма: " + o_tbRSum + " => " + tbRSum.Text;
                            if ( !o_tbRPrim.Equals(tbRPrim.Text) )
                                logn += "\n* Примечание: " + o_tbRPrim + " => " + tbRPrim.Text;
                        }

                        /*
                        cmd = new MySqlCommand("insert into `reclog` (op_date, op_user, op_kind, op_info) values (@op_date, @op_user, @op_kind, @op_info)", tools.connection);
                        tools.SetDbParameter(cmd, "op_date", deRDate.Value.ToString("yyyyMMdd"));
                        tools.SetDbParameter(cmd, "op_user", tools.currentUser.PID);
                        tools.SetDbParameter(cmd, "op_kind", isNew ? 0 : 1);
                        tools.SetDbParameter(cmd, "op_info", logn);
                        cmd.ExecuteNonQuery();
                         */

                        using (DbCommand cmd = Db.command("insert into reclog (op_date, op_user, op_kind, op_info) values (@op_date, @op_user, @op_kind, @op_info)"))
                        {
                            Db.param(cmd, "op_date", deRDate.Value.ToString("yyyyMMdd"));
                            Db.param(cmd, "op_user", tools.currentUser.PID);
                            Db.param(cmd, "op_kind", isNew ? 0 : 1);
                            Db.param(cmd, "op_info", logn);
                            cmd.ExecuteNonQuery();
                        }

                        /*
                        cmd = new MySqlCommand("delete from `new_recs` where rec_id = @rec_id", tools.connection);
                        tools.SetDbParameter(cmd, "rec_id", opid);
                        cmd.ExecuteNonQuery();
                        */

                        using (DbCommand cmd = Db.command("delete from new_recs where rec_id = @rec_id"))
                        {
                            Db.param(cmd, "rec_id", opid);
                            cmd.ExecuteNonQuery();
                        }


//                        DataTable dtnr = tools.MySqlFillTable(new MySqlCommand("select id from `users` where mark_new = 1 and id <> " + tools.currentUser.PID, tools.connection));

                        using (DataTable dtnr = Db.fillTable(Db.command("select id from `users` where mark_new = 1 and id <> " + tools.currentUser.PID)))
                        {
                            if (dtnr != null && dtnr.Rows.Count > 0)
                            {
                                foreach (DataRow r in dtnr.Rows)
                                {
                                    /*
                                    cmd = new MySqlCommand("insert into `new_recs` (user_id, rec_id) values (@user_id, @rec_id)", tools.connection);
                                    tools.SetDbParameter(cmd, "user_id", r["id"]);
                                    tools.SetDbParameter(cmd, "rec_id", opid);
                                    cmd.ExecuteNonQuery();
                                     */
                                    using (DbCommand cmd = Db.command("insert into `new_recs` (user_id, rec_id) values (@user_id, @rec_id)"))
                                    {
                                        Db.param(cmd, "user_id", r["id"]);
                                        Db.param(cmd, "rec_id", opid);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        DialogResult = DialogResult.OK;

                    }
                    catch ( Exception ex )
                    {
                        MessageBox.Show("Системная ошибка.\nКласс: " + ex.GetType().ToString() + "\nСообщение: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Ошибки:\n\n" + er);
                }
            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (opid > -1)
            {
                try
                {
//                    new MySqlCommand("update journal set locker_id = null, lock_till = null where id = " + opid.ToString(), tools.connection).ExecuteNonQuery();
                    using (DbCommand cmd = Db.command("update journal set locker_id = null, lock_till = null where id = " + opid.ToString()))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception) { }
            }
        }

        private void RecForm_Shown(object sender, EventArgs e)
        {
            deRDate.Focus();
        }

        private void tbRPrim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                bOk_Click(bOk, null);
            }
        }

        void checkCurrCode()
        {
            lCurrCode.SuspendLayout();
            lCurrCode.Text = "";
            try
            {
                int currId = -1;
                if (cbSrcType.SelectedIndex == 1)
                {
                    // Валюта приёмника
                    currId = dAccCurr[(int)sotiDstAccSel.Tag];
                }
                else
                {
                    // Валюта источника
                    currId = dAccCurr[(int)sotiSrcAccSel.Tag];
                }

                /*
                lCurrCode.Text = Convert.ToString(new MySqlCommand(
                    "select name from curr_list where id = " + currId.ToString(), 
                    tools.connection).ExecuteScalar());
                */

                using (DbCommand cmd = Db.command("select name from curr_list where id = " + currId.ToString()))
                {
                    lCurrCode.Text = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception) { }

            lCurrCode.ResumeLayout();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string value = ((TextBox)sender).Text;
                double v = double.Parse(value, NumberStyles.Float);
                ((TextBox)sender).Text = v.ToString("F2");
                e.Cancel = false;
            }
            catch (Exception) 
            {
                e.Cancel = true;
            }
        }

        static SolidBrush sbHighlight = new SolidBrush(SystemColors.Highlight);
        static SolidBrush sbHighlightText = new SolidBrush(SystemColors.HighlightText);

        private void bAddPrim_Click(object sender, EventArgs e)
        {
            if (tbRPrim.Text.Trim().Length > 0 && !scRPrim.Contains(tbRPrim.Text))
            {
                scRPrim.Add(tbRPrim.Text);

                string[] ss = new string[scRPrim.Count];
                int c = 0;
                foreach (string st in scRPrim) ss[c++] = st;

                //File.WriteAllLines(tools.dataDir + @"\prim.cache", ss);


                using (DbCommand cmd = Db.command("insert into `prim_cache` (text) values (@text)"))
                {
                    Db.param(cmd, "text", tbRPrim.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void bEditPrim_Click(object sender, EventArgs e)
        {
            string[] ss = new string[scRPrim.Count];
            int c = 0;
            foreach (string st in scRPrim) ss[c++] = st;

            primForm.tbPrim.Lines = ss;

            if (primForm.ShowDialog() == DialogResult.OK)
            {
                ss = primForm.tbPrim.Lines;
                //File.WriteAllLines(tools.dataDir + @"\prim.cache", ss);
                scRPrim.Clear();
                scRPrim.AddRange(ss);

                string[] s = new string[scRPrim.Count];
                c = 0;
                foreach (string st in ss)
                {
                    s[c++] = "('" + st + "')";
                }
                string p = String.Join(",", s);

                using (DbCommand cmd = Db.command("TRUNCATE TABLE `prim_cache`"))
                {
                    cmd.ExecuteNonQuery();
                }

                using (DbCommand cmd = Db.command("insert into `prim_cache` (text) values " + p))
                {
                    //Db.param(cmd, "text", p);
                    cmd.ExecuteNonQuery();
                }
            }

        }


/*
        private void tvSrc_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) != 0)
            {


                e.Graphics.FillRectangle(sbHighlight, e.Node.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                e.Graphics.DrawString(e.Node.Text, nodeFont, sbHighlightText, Rectangle.Inflate(e.Bounds, 2, 0));
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        */

        //TODO Получение курса из справочника

        /* В нашей системе валютный тип контрагента зависит от счёта, с которым он совершает операцию.
         * Если это операция прихода от контрагента на счёт, валютой операции считается валюта счёта-приёмника.
         * Если это операция расхода со счёта контрагенту, валютой операции считается валюта счёта-источника.
         * Если происходит перемещение между счетами с разной валютой, для операции рассчитывается кросс-курс валют счетов к рублю.
         *
         * По сути, curr_value нужен только при операции между счетами, у которых валюты разные.
         */

    }
}
