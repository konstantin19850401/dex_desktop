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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Utils;
using System.Data.Common;

namespace Kassa3
{
    public partial class BalanceForm : Form
    {
        Tools tools;
        CurrValueEdForm cvef;
        FormState fs;
        DateTime d1, d2;

        string lastControlSignature = "";

        DataTable dtData = null;
        BindingSource bsData = null;
        List<string> sfields;
        Dictionary<string, int> dAccCurr = new Dictionary<string, int>(); // Id валюты по полю счёта
        Dictionary<int, string> dCurTitle = new Dictionary<int, string>(); // Наименование валюты по Id валюты
        Dictionary<string, int> dAccId = new Dictionary<string, int>(); //Id счёта по его наименованию
        Dictionary<string, string> dAccTitle = new Dictionary<string, string>(); // Наименование счёта по Id счёта
        Dictionary<int, DataRow> dIdRow = new Dictionary<int, DataRow>(); // DataRow счёта по Id счёта

        OptionsLayoutGrid olg;

        int rubCurrId;

        public BalanceForm()
        {
            InitializeComponent();

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

            cvef = new CurrValueEdForm();

            sfields = new List<string>();
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\balance.fs");
            fs.LoadValue("deStart", deStart);
            fs.LoadValue("deEnd", deEnd);
            fs.LoadValue("cbFromDate", cbFromDate);
//            fs.LoadValue("nudItoCource", nudItoCource);
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

            gc.Visible = false;

            deStart.Enabled = cbFromDate.Checked;


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
            catch (Exception)
            {
                rubCurrId = -1;
            }


            // Подготовка информации о фирмах и счетах
            dAccCurr.Clear();
            dCurTitle.Clear();
            dAccId.Clear();
            dAccTitle.Clear();
            clbAccounts.Items.Clear();
            lbCurr.Items.Clear();
            string where = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.ACCOUNT, "ac.id");

            string SCONCAT = Db.isMysql ? "concat(fi.title, ' - ', ac.title)" : "fi.title || ' - ' || ac.title";

            string sql = "select ac.id, " + SCONCAT + " as title, ac.curr_id, cl.name as curr_title " +
                "from accounts as ac, firms as fi, curr_list as cl where ac.firm_id = fi.id and ac.curr_id = cl.id and (" +
                where + ") order by fi.title, ac.title";

            /*
            MySqlCommand cmd = new MySqlCommand(sql, tools.connection);

            DataTable dt = tools.MySqlFillTable(cmd);
            */
            DataTable dt = Db.fillTable(Db.command(sql));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    string accstr = "a" + r["id"].ToString();
                    dAccId[accstr] = Convert.ToInt32(r["id"]);
                    clbAccounts.Items.Add(new StringTagItem(r["title"].ToString(), accstr), sfields.Contains(accstr));
                    int curr_id = Convert.ToInt32(r["curr_id"]);
                    dAccCurr[accstr] = curr_id;
                    dAccTitle[accstr] = r["title"].ToString();
                    if (!dCurTitle.ContainsKey(curr_id))
                    {
                        dCurTitle[curr_id] = r["curr_title"].ToString();
                        double CursValue = 1;
                        if (dcc.ContainsKey(curr_id)) CursValue = dcc[curr_id];
                        lbCurr.Items.Add(new StringObjTagItem(r["curr_title"].ToString() + ": " + CursValue.ToString(), new CurrencyDescriptor(curr_id, CursValue))); // TODO Сделать запоминание указанных значений
                    }
                }

                double ItoCursValue = 1;
                if (dcc.ContainsKey(-2)) ItoCursValue = dcc[-2];
                dCurTitle[-2] = "Курс итогов";
                lbCurr.Items.Add(new StringObjTagItem(dCurTitle[-2] + ": " + ItoCursValue.ToString(), new CurrencyDescriptor(-2, ItoCursValue)));
            }

        }

        private void BalanceForm_Load(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
        }

        private void BalanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = "";
            foreach (string sf in sfields)
            {
                if (s.Length > 0) s += "|";
                s += sf;
            }

            fs.SaveValue("shownFields", s);

            s = "";
            foreach (StringObjTagItem soti in lbCurr.Items)
            {
                if (s != "") s += "|";
                CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                s += cd.id.ToString() + ";" + cd.value.ToString();
            }

            fs.SaveValue("CurrencyValues", s);

            fs.SaveValue("deStart", deStart);
            fs.SaveValue("deEnd", deEnd);
            fs.SaveValue("cbFromDate", cbFromDate);
//            fs.SaveValue("nudItoCource", nudItoCource);
            fs.UpdateFromForm(this);
            fs.SaveToFile(tools.dataDir + @"\balance.fs");

            try
            {
                if (gc.Visible) gv.SaveLayoutToXml(tools.dataDir + @"\balance_layout.xml", olg);
            }
            catch (Exception) { }

            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        private void cbFromDate_CheckedChanged(object sender, EventArgs e)
        {
            deStart.Enabled = cbFromDate.Checked;
        }

        private void clbAccounts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                string tfield = ((StringTagItem)clbAccounts.Items[e.Index]).Tag;
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

        private void bRefresh_Click(object sender, EventArgs e)
        {
            /*if (tools.checkConnection(tools.connection, false))*/ RefreshView();
        }


        void RefreshView()
        {
            string er = "";

            try
            {
                if (gc.Visible) gv.SaveLayoutToXml(tools.dataDir + @"\balance_layout.xml", olg);
            }
            catch (Exception) { }

            lastControlSignature = getControlSignature();

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
            foreach (string sf in sfields)
            {
                if (dAccId.ContainsKey(sf))
                {
                    if (acc_in != "") acc_in += ", ";
                    acc_in += dAccId[sf];

                    string curfld = "c" + dAccCurr[sf].ToString();
                    if (!dtData.Columns.Contains(curfld))
                    {
                        dtData.Columns.Add(new DataColumn(curfld, typeof(double)));
                        gv.Columns.Add(createGColumn(curfld, curfld, dCurTitle[dAccCurr[sf]], SummaryItemType.Sum, gv.Columns.Count, true));
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

            dIdRow.Clear();

            // Добавляем все счета в таблицу и инициализируем поля валют в 0
            foreach (string sf in sfields)
            {
                if (dAccId.ContainsKey(sf))
                {
                    DataRow nr = dtData.NewRow();
                    nr["id"] = dAccId[sf];
                    nr["title"] = dAccTitle[sf];
                    foreach (string cols in colst) nr[cols] = 0.0f;
                    nr["ito"] = 0.0f;
                    dtData.Rows.Add(nr);
                    dIdRow[dAccId[sf]] = nr;
                }
            }

            string sql = "select id, op_id, r_sum, srctype, src_acc_id, src_curr_value, " +
                "dsttype, dst_acc_id, dst_curr_value from journal where ((srctype = 0 and src_acc_id in (" + acc_in +
                ")) or (dsttype = 0 and dst_acc_id in (" + acc_in + "))) and deleted = 0 and ";

            d2 = deEnd.Value;
            d1 = cbFromDate.Checked ? deStart.Value : d2;

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
                foreach (string sf in sfields)
                {
                    if (dAccId.ContainsKey(sf))
                    {
                        bc.AddAccount(/*dAccTitle[sf],*/ dAccId[sf], dAccCurr[sf], 0);
                    }
                }

                foreach (StringObjTagItem soti in lbCurr.Items)
                {
                    CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                    bc.currencies[cd.id] = cd.value;
                }


                try
                {
                    //DataTable dt = tools.MySqlFillTable(new MySqlCommand(sql, tools.connection));
                    DataTable dt = Db.fillTable(Db.command(sql));

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

                            bc.ProcessOperation(Convert.ToDouble(dr["r_sum"]), srctype, src_acc_id, src_curr_value, dsttype, dst_acc_id, dst_curr_Value);
                        }
                        catch (Exception) { }
                    }

                    double ItoCurrValue = 1;
                    foreach (StringObjTagItem soti in lbCurr.Items)
                    {
                        CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                        if (cd.id == -2)
                        {
                            ItoCurrValue = cd.value;
                            break;
                        }
                    }

                    foreach (KeyValuePair<int, DataRow> kvp in dIdRow)
                    {
                        DataRow dr = kvp.Value;
                        dr["c" + bc.accounts[kvp.Key].currId.ToString()] = bc.accounts[kvp.Key].balance;
                        dr["ito"] = bc.accounts[kvp.Key].balance * bc.currencies[bc.accounts[kvp.Key].currId] / ItoCurrValue;
                    }
                }
                catch (Exception) { }
            }
            else MessageBox.Show("Ошибки:\n\n" + er);

            try
            {
                gv.RestoreLayoutFromXml(tools.dataDir + @"\balance_layout.xml", olg);
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

        private void lbCurr_DoubleClick(object sender, EventArgs e)
        {
            if (lbCurr.Items.Count > 0 && lbCurr.SelectedItem != null)
            {
                StringObjTagItem soti = (StringObjTagItem)lbCurr.SelectedItem;
                CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                if (cd.id != rubCurrId)
                {
                    cvef.init(dCurTitle[cd.id], cd.value);
                    if (cvef.ShowDialog() == DialogResult.OK)
                    {
                        cd.value = cvef.returnValue;
                        soti.Text = dCurTitle[cd.id] + ": " + cd.value.ToString("F2");
                        typeof(ListBox).InvokeMember("RefreshItems", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, lbCurr, new object[] { });
                    }
                }
                else
                {
                    MessageBox.Show("Поскольку российский рубль является базовой валютой пересчёта, его курс всегда имеет значение 1.");
                }

            }
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


        string getControlSignature()
        {
            string ret = "deStart=" + deStart.Value.ToString("yyyyMMdd") + "\n" +
                "deEnd=" + deEnd.Value.ToString("yyyyMMdd") + "\n" +
                "cbFromDate=" + cbFromDate.Checked.ToString() + "\n" +
                "sfields=";

            string sall = "";
            foreach (string s in sfields)
            {
                if (sall != "") sall += "|";
                sall += s;
            }
            ret += sall + "\n";


            sall = "";
            foreach (StringObjTagItem soti in lbCurr.Items)
            {
                if (sall != "") sall += "|";
                CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                sall += cd.id.ToString() + ";" + cd.value.ToString();
            }

            ret += sall + "\n";

            return ret;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!getControlSignature().Equals(lastControlSignature))
                {
                    bRefresh.UseVisualStyleBackColor = !bRefresh.UseVisualStyleBackColor;
                    if (!bRefresh.UseVisualStyleBackColor) bRefresh.BackColor = Color.Red;
                }
                else
                {
                    bRefresh.UseVisualStyleBackColor = true;
                }
            }
            catch (Exception)
            {
                bRefresh.UseVisualStyleBackColor = true;
            }

            bRefresh.Invalidate();

        }

        private void clbAccounts_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool seld = clbAccounts.CheckedItems.Count == 0;
                for (int i = 0; i < clbAccounts.Items.Count; ++i) clbAccounts.SetItemChecked(i, seld);
            }
        }


    }
}
