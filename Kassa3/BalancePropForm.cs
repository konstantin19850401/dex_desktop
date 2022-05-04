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
using System.Reflection;
using System.Data.Common;

namespace Kassa3
{
    public partial class BalancePropForm : Form
    {
        Tools tools;
        CurrValueEdForm cvef;
        public int rubCurrId;

        public List<string> sfields;
        public Dictionary<int, double> dcc;
        public Dictionary<string, int> dAccCurr = new Dictionary<string, int>(); // Id валюты по полю счёта
        public Dictionary<int, string> dCurTitle = new Dictionary<int, string>(); // Наименование валюты по Id валюты
        public Dictionary<string, int> dAccId = new Dictionary<string, int>(); //Id счёта по его наименованию
        public Dictionary<string, string> dAccTitle = new Dictionary<string, string>(); // Наименование счёта по Id счёта
        public Dictionary<string, string> dAccBt = new Dictionary<string, string>(); // Финансовая группа по Id счёта
        public Dictionary<int, DataRow> dIdRow = new Dictionary<int, DataRow>(); // DataRow счёта по Id счёта

        public BalancePropForm(List<string> sfields, Dictionary<int, double> dcc)
        {
            InitializeComponent();
            tools = Tools.instance;

            cvef = new CurrValueEdForm();
            this.sfields = sfields;
            this.dcc = dcc;

        }

        public void init() 
        {
            dAccCurr.Clear();
            dCurTitle.Clear();
            dAccId.Clear();
            dAccTitle.Clear();
            clbAccounts.Items.Clear();
            lbCurr.Items.Clear();

            
            // Получим ID валюты "российский рубль"
            try
            {
                /*
                MySqlCommand cmdGR = new MySqlCommand("select id from curr_list where curr_id = @curr_id", tools.connection);
                tools.SetDbParameter(cmdGR, "curr_id", Tools.DEF_CURRENCY);
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
            string where = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.ACCOUNT, "ac.id");

            string SCONCAT = Db.isMysql ? "concat(fi.title, ' - ', ac.title)" : "fi.title || ' - ' || ac.title";

            string sql = "select ac.id, " + SCONCAT + " as title, ac.curr_id, ac.banktag, cl.name as curr_title " +
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
                    dAccBt[accstr] = r["banktag"].ToString();
                    clbAccounts.Items.Add(new StringTagItem(r["title"].ToString(), accstr), sfields.Contains(accstr));
                    int curr_id = Convert.ToInt32(r["curr_id"]);
                    dAccCurr[accstr] = curr_id;
                    dAccTitle[accstr] = r["title"].ToString();
                    if (!dCurTitle.ContainsKey(curr_id))
                    {
                        dCurTitle[curr_id] = r["curr_title"].ToString();
                        double CursValue = 1;
                        if (dcc.ContainsKey(curr_id)) CursValue = dcc[curr_id];
                        lbCurr.Items.Add(new StringObjTagItem(r["curr_title"].ToString() + ": " + CursValue.ToString("F2"), new CurrencyDescriptor(curr_id, CursValue))); // TODO Сделать запоминание указанных значений
                    }
                }

                double ItoCursValue = 1;
                if (dcc.ContainsKey(-2)) ItoCursValue = dcc[-2];
                dCurTitle[-2] = "Курс итогов";
                lbCurr.Items.Add(new StringObjTagItem(dCurTitle[-2] + ": " + ItoCursValue.ToString("F2"), new CurrencyDescriptor(-2, ItoCursValue)));
            }


        }

        public void saveValues(FormState fs)
        {
            string s = "";
            foreach (StringObjTagItem soti in lbCurr.Items)
            {
                if (s != "") s += "|";
                CurrencyDescriptor cd = (CurrencyDescriptor)soti.Tag;
                s += cd.id.ToString() + ";" + cd.value.ToString();
            }

            fs.SaveValue("CurrencyValues", s);

            s = "";
            foreach (string sf in sfields)
            {
                if (s != "") s += "|";
                s += sf;
            }

            fs.SaveValue("shownFields", s);
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

        private void clbAccounts_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool seld = clbAccounts.CheckedItems.Count == 0;
                for (int i = 0; i < clbAccounts.Items.Count; ++i) clbAccounts.SetItemChecked(i, seld);
            }
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
                        dcc[cd.id] = cd.value;
                    }
                }
                else
                {
                    MessageBox.Show("Поскольку российский рубль является базовой валютой пересчёта, его курс всегда имеет значение 1.");
                }

            }
        }
    }
}
