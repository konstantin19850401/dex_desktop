using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using DEXExtendLib;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public partial class CurrDicForm : Form
    {
        Tools tools;
        DataTable dtCT, dtCV;
        BindingSource bsCT, bsCV;
//        MySqlDataAdapter adaCT, adaCV;
//        MySqlCommand dcCV;

        DbDataAdapter adaCT, adaCV;
        DbCommand dcCV;

        FormState fs;

        CurrTypeEdForm ctef;

        public CurrDicForm()
        {
            InitializeComponent();

            ctef = new CurrTypeEdForm();

            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\CurrDicForm.fs");

            dgvCT.AutoGenerateColumns = false;

            //adaCT = new MySqlDataAdapter("select * from curr_list", tools.connection);
            adaCT = Db.dataAdapter();
            adaCT.SelectCommand = Db.command("select * from curr_list");

            dtCT = new DataTable();
            bsCT = new BindingSource();
            bsCT.DataSource = dtCT;
            dgvCT.DataSource = bsCT;

            /*
            dcCV = new MySqlCommand(
                "select cv.value as cvalue, cv.date as cdate, cl.name as cname from curr_values as cv, curr_list as cl " +
                "where cv.currlist_id = cl.id and cv.date = (" +
                "select max(cv2.date) from curr_values as cv2 where cv2.date <= @date and cv2.currlist_id = cv.currlist_id" +
                ") group by cv.currlist_id", tools.connection
                );

            tools.SetDbParameter(dcCV, "date", DateTime.Now.ToString("yyyyMMdd"));
            adaCV = new MySqlDataAdapter(dcCV);
             */
            dcCV = Db.command(
                "select cv.value as cvalue, cv.date as cdate, cl.name as cname from curr_values as cv, curr_list as cl " +
                "where cv.currlist_id = cl.id and cv.date = (" +
                "select max(cv2.date) from curr_values as cv2 where cv2.date <= @date and cv2.currlist_id = cv.currlist_id" +
                ") group by cv.currlist_id"
                );
            Db.param(dcCV, "date", DateTime.Now.ToString("yyyyMMdd"));
            adaCV = Db.dataAdapter();
            adaCV.SelectCommand = dcCV;

            dtCV = new DataTable();
            bsCV = new BindingSource();
            bsCV.DataSource = dtCV;
            dgvCV.DataSource = bsCV;

            refreshCTView();
        }

        ~CurrDicForm()
        {
            tools = null;
            dtCT = null;
            bsCT = null;
            adaCT = null;
            fs = null;
            ctef = null;
        }

        private void CurrDicForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
            fs.LoadValue("tc_SelectedIndex", tc);
            fs.LoadValue("deCurrDate_Text", deCurrDate);
            if (fs.values.ContainsKey("deCurrDate_Text")) refreshCVView();
            bool readWrite = tools.currentUser.prefs.dicCurrency == AccessMode.WRITE;
            bDellCurrDate.Enabled = readWrite;
            bAddCurrency.Enabled = readWrite;
            bDeleteCurrency.Enabled = readWrite;
            bCurrStatus.Enabled = readWrite;
        }

        private void CurrDicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            fs.SaveValue("tc_SelectedIndex", tc);
            fs.SaveValue("deCurrDate_Text", deCurrDate);
            fs.SaveToFile(tools.dataDir + @"\CurrDicForm.fs");
            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        public void refreshCTView()
        {
            string pid = "-";
            try
            {
                if (dgvCT.SelectedRows.Count > 0)
                {
                    DataRowView drv = bsCT[dgvCT.SelectedRows[0].Index] as DataRowView;
                    pid = drv["id"].ToString();
                }
            }
            catch (Exception) { }

            dtCT.Clear();
            adaCT.Fill(dtCT);

            try
            {
                foreach (DataGridViewRow r in dgvCT.Rows)
                {
                    DataRowView drv = bsCT[r.Index] as DataRowView;
                    r.Selected = drv["id"].ToString().Equals(pid);
                }
            }
            catch (Exception) { }

            dgvCT.Visible = dtCT.Rows.Count > 0;
        }

        public void refreshCVView()
        {
            dtCV.Clear();
            try
            {
//                tools.SetDbParameter(dcCV, "date", deCurrDate.Value.ToString("yyyyMMdd"));
                Db.param(dcCV, "date", deCurrDate.Value.ToString("yyyyMMdd"));
                adaCV.Fill(dtCV);
            }
            catch (Exception) { }
            dgvCV.Visible = dtCV.Rows.Count > 0;
        }

        private void bAddCurrency_Click(object sender, EventArgs e)
        {
            if (ctef.ShowDialog() == DialogResult.OK)
            {
                /*
                MySqlCommand cmd = new MySqlCommand(
                    "insert into curr_list (curr_id, name, active) values (@curr_id, @name, @active)", tools.connection
                    );
                tools.SetDbParameter(cmd, "curr_id", ctef.tbCurrID.Text.Trim());
                tools.SetDbParameter(cmd, "name", ctef.tbName.Text.Trim());
                tools.SetDbParameter(cmd, "active", true);

                cmd.ExecuteNonQuery();
                */

                using (DbCommand cmd = Db.command("insert into curr_list (curr_id, name, active) values (@curr_id, @name, @active)"))
                {
                    Db.param(cmd, "curr_id", ctef.tbCurrID.Text.Trim());
                    Db.param(cmd, "name", ctef.tbName.Text.Trim());
                    Db.param(cmd, "active", true);

                    cmd.ExecuteNonQuery();
                }

                refreshCTView();
            }
        }

        private void bCurrStatus_Click(object sender, EventArgs e)
        {
            if (dgvCT.SelectedRows != null && dgvCT.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgvCT.SelectedRows)
                {
                    DataRowView drv = bsCT[dgvr.Index] as DataRowView;
                    /*
                    MySqlCommand cmd = new MySqlCommand(
                        "update curr_list set active = 1 - active where curr_id = @curr_id", tools.connection
                        );
                    tools.SetDbParameter(cmd, "curr_id", drv["curr_id"].ToString());
                    cmd.ExecuteNonQuery();
                    */

                    using (DbCommand cmd = Db.command("update curr_list set active = 1 - active where curr_id = @curr_id"))
                    {
                        Db.param(cmd, "curr_id", drv["curr_id"].ToString());
                        cmd.ExecuteNonQuery();
                    }

                    bool d = (bool)drv["active"];
                    drv["active"] = !d;
                }
                dgvCT.Refresh();
            }
        }

        private void bDeleteCurrency_Click(object sender, EventArgs e)
        {
            if (dgvCT.SelectedRows != null && dgvCT.SelectedRows.Count > 0)
            {

                if (MessageBox.Show("Удалить выбранный тип валюты?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        DataRowView drv = bsCT[dgvCT.SelectedRows[0].Index] as DataRowView;
                        
                        if (drv["curr_id"].ToString().Equals(Tools.DEF_CURRENCY)) throw new Exception();

                        using (DbCommand cmd = Db.command("delete from curr_list where curr_id = @curr_id"))
                        {
                            Db.param(cmd, "curr_id", drv["curr_id"].ToString());
                            cmd.ExecuteNonQuery();
                        }

                        if (!Db.isMysql) {
                            using (DbCommand cmd = Db.command("delete from curr_values where currlist_id = @currlist_id"))
                            {
                                Db.param(cmd, "currlist_id", drv["id"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить выбранный тип валюты."));
                    }

                    refreshCTView();
                }
            }
        }

        public string updateCurrTypes(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = true;
            wmea.minValue = 0;
            wmea.maxValue = 2;
            wmea.progressValue = 0;
            wmea.progressVisible = true;

            wmea.textMessage = "Загрузка типов валют";

//            DataTable dt = tools.MySqlFillTable(new MySqlCommand("select curr_id from curr_list", tools.connection));
            
            DataTable dt = Db.fillTable(Db.command("select curr_id from curr_list"));

            /*
            MySqlCommand cmdAddCurr = new MySqlCommand(
                "insert into curr_list (curr_id, name, active) values (@curr_id, @name, @active)", tools.connection
                );
            tools.SetDbParameter(cmdAddCurr, "active", true);
            */

            using (DbCommand cmdAddCurr = Db.command("insert into curr_list (curr_id, name, active) values (@curr_id, @name, @active)"))
            {
                Db.param(cmdAddCurr, "active", true);

                int pv = 0;

                for (int i = 0; i < 2; ++i)
                {
                    try
                    {
                        string dst = tools.makeWebRequest(
                            "http://www.cbr.ru/scripts/XML_val.asp?d=" + i.ToString(),
                            Encoding.GetEncoding(1251)
                            );
                        SimpleXML xml = SimpleXML.LoadXml(dst);
                        // Распарсить валюты и вставить их в таблицу

                        if (xml.Name.ToLowerInvariant().Equals("valuta"))
                        {
                            ArrayList alItems = xml.GetChildren("Item");
                            if (alItems != null && alItems.Count > 0)
                            {
                                foreach (SimpleXML item in alItems)
                                {
                                    if (item.Attributes.ContainsKey("ID") && item.GetNodeByPath("Name", false) != null)
                                    {
//                                        DataRow[] dr = dt.Select("curr_id='" + MySqlHelper.EscapeString(item.Attributes["ID"]) + "'");
                                        DataRow[] dr = dt.Select("curr_id='" + Db.escape(item.Attributes["ID"]) + "'");
                                        if (dr == null || dr.Length < 1)
                                        {
                                            /*
                                            tools.SetDbParameter(cmdAddCurr, "curr_id", item.Attributes["ID"]);
                                            tools.SetDbParameter(cmdAddCurr, "name", item["Name"].Text);
                                             */

                                            Db.param(cmdAddCurr, "curr_id", item.Attributes["ID"]);
                                            Db.param(cmdAddCurr, "name", item["Name"].Text);

                                            cmdAddCurr.ExecuteNonQuery();

                                            DataRow nr = dt.NewRow();
                                            nr.BeginEdit();
                                            nr["curr_id"] = item.Attributes["ID"];
                                            nr.EndEdit();
                                            dt.Rows.Add(nr);
                                        }
                                    }
                                }
                            }
                        }

                        /*
                        <Valuta name="Foreign Currency Market Lib">
                            <Item ID="R01498C">
                                <Name>Мозамбикский метикал</Name>
                                <EngName>Mozambique Metical</EngName>
                                <Nominal>100</Nominal>
                                <ParentCode>R01498    </ParentCode>
                            </Item>

                         */
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                    pv++;
                    wmea.progressValue = pv;
                    wmea.DoEvents();
                    if (wmea.isAborted) return "Операция прервана";
                }
            }

            return null;
        }

        private void bRefreshCurrTitles_Click(object sender, EventArgs e)
        {
            string result = WaitMessage.Execute(updateCurrTypes);
            if (result != null && !result.Trim().Equals("")) MessageBox.Show(result);
            refreshCTView();
        }


        DateTime updateCurrValuesDate;

        public string updateCurrValues(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = false;
            wmea.progressVisible = false;
            wmea.textMessage = string.Format("Загрузка курсов валют на {0}", updateCurrValuesDate.ToString("dd.MM.yyyy"));

            return tools.updateCurrencyValues(updateCurrValuesDate, false);
        }

        private void bUpdateWeb_Click(object sender, EventArgs e)
        {
            updateCurrValuesDate = deCurrDate.Value;
            string result = WaitMessage.Execute(updateCurrValues);
            if (result == "") result = "Курсы валют успешно получены";
            if (result != null && !result.Trim().Equals("")) MessageBox.Show(result);
            refreshCVView();
        }

        private void dgvCT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bCurrStatus_Click(bCurrStatus, null);
        }

        private void bShowDay_Click(object sender, EventArgs e)
        {
            refreshCVView();
        }

        private void dgvCV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCV.Columns[e.ColumnIndex].Name.Equals("cdate"))
            {
                try
                {
                    string ov = e.Value.ToString();
                    e.Value = ov.Substring(6, 2) + "." + ov.Substring(4, 2) + "." + ov.Substring(0, 4);
                    e.FormattingApplied = true;
                }
                catch (Exception)
                {
                }
            }
        }

        private string DeleteCurrValues(string cvdate)
        {
            try
            {
                /*
                object sc = (new MySqlCommand(
                    "select count(id) cid from curr_values where date = '" + cvdate + "'", tools.connection
                    )).ExecuteScalar();
                */

                using (DbCommand cmd = Db.command("select count(id) cid from curr_values where date = '" + cvdate + "'"))
                {
                    object sc = cmd.ExecuteScalar();
                
                    if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0)
                    {
                        /*
                        (new MySqlCommand(
                            "delete from curr_values where date = '" + cvdate + "'", tools.connection
                            )).ExecuteNonQuery();
                        */

                        using (DbCommand cmd2 = Db.command("delete from curr_values where date = '" + cvdate + "'"))
                        {
                            cmd2.ExecuteNonQuery();
                        }

                        return "Курсы валют на указанную дату удалены";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Ошибка: " + tools.DbErrorMsg(ex, ex.Message);
            }

            return "В заданной дате не имелось ни одного значения валютного курса.";
        }

        private void bDellCurrDate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить курсы валют на указанную дату?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(DeleteCurrValues(deCurrDate.Value.ToString("yyyyMMdd")));
                refreshCVView();
            }
        }
    }
}
