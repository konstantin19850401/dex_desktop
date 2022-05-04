﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.IO;
using System.Diagnostics;
using System.Collections;

using MySql.Data.MySqlClient;

namespace DEXPlugin.Function.Yota.RebuildAutodocPeople
{
    public partial class RebuildAutodocPeopleMain : Form
    {
        Object toolbox;

        public RebuildAutodocPeopleMain(Object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;

            IDEXConfig cfg = (IDEXConfig)toolbox;
            IDEXSysData sd = (IDEXSysData)toolbox;

            cbInterval.Checked = cfg.getBool(this.Name, "cbInterval", false);
            deFrom.Value = cfg.getDate(this.Name, "deFrom", DateTime.Now);
            deTo.Value = cfg.getDate(this.Name, "deTo", DateTime.Now);

            cbSource.SelectedIndex = cfg.getInt(this.Name, "cbSource", 2);

            ArrayList docs = sd.DocumentTypes();

            string seld = cfg.getStr(this.Name, "cbDocType", "");
            StringTagItem selo = null;

            cbDocType.Items.Clear();
            foreach (IDEXPluginDocument doc in docs)
            {
                StringTagItem sti = new StringTagItem(doc.Title, doc.ID);
                cbDocType.Items.Add(sti);
                if (seld.Equals(doc.ID)) selo = sti;
            }

            if (selo != null) cbDocType.SelectedItem = selo;
            else cbDocType.SelectedIndex = 0;

            tbFizDocType.Text = cfg.getStr(this.Name, "tbFizDocType", "");

            rbInclude.Checked = cfg.getBool(this.Name, "rbInclude", false);
            rbExclude.Checked = cfg.getBool(this.Name, "rbExclude", false);
            rbNotUse.Checked = cfg.getBool(this.Name, "rbNotUse", true);

            cbRestrictDocCount.Checked = cfg.getBool(this.Name, "cbRestrictDocCount", true);
            nudDocCount.Value = cfg.getInt(this.Name, "nudDocCount", 3);

            tbNotReassign.Text = cfg.getStr(this.Name, "tbNotReassign", "");

            cbInterval_CheckedChanged(cbInterval, null);
            rbNotUse_CheckedChanged(null, null);

            IDEXData d = (IDEXData)toolbox;
            DataTable dt = d.getQuery("select uid, title from `units` order by title");
            clbUnits.Items.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    clbUnits.Items.Add(new StringTagItem(row["title"].ToString(), row["uid"].ToString()), 
                        cfg.getBool(this.Name, "unit_" + row["uid"].ToString(), false));
                }
            }
        }

        public void SaveParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setBool(this.Name, "cbInterval", cbInterval.Checked);
            cfg.setDate(this.Name, "deFrom", deFrom.Value);
            cfg.setDate(this.Name, "deTo", deTo.Value);
            cfg.setInt(this.Name, "cbSource", cbSource.SelectedIndex);
            if (cbDocType.SelectedItem != null) cfg.setStr(this.Name, "cbDocType", ((StringTagItem)cbDocType.SelectedItem).Tag);
            cfg.setStr(this.Name, "tbFizDocType", tbFizDocType.Text);
            cfg.setBool(this.Name, "rbInclude", rbInclude.Checked);
            cfg.setBool(this.Name, "rbExclude", rbExclude.Checked);
            cfg.setBool(this.Name, "rbNotUse", rbNotUse.Checked);
            cfg.setBool(this.Name, "cbRestrictDocCount", cbRestrictDocCount.Checked);
            cfg.setInt(this.Name, "nudDocCount", (int)nudDocCount.Value);
            cfg.setStr(this.Name, "tbNotReassign", tbNotReassign.Text);

            foreach (StringTagItem sti in clbUnits.Items)
            {
                cfg.setBool(this.Name, "unit_" + sti.Tag, clbUnits.CheckedItems.Contains(sti));
            }
        }

        private void cbInterval_CheckedChanged(object sender, EventArgs e)
        {
            deFrom.Enabled = cbInterval.Checked;
            deTo.Enabled = cbInterval.Checked;
        }

        public string RebuildPeople(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = true;
            wmea.progressVisible = false;
            wmea.textMessage = "Подготовка параметров";
            bool statusPassport;

            string er = "";
            string[] tbname = new string[2] { "archive", "journal" };
            string[] tbtext = new string[2] { "архиве", "журнале" };

            if (cbDocType.SelectedItem == null) return "Не указан тип документа";

            string docid = ((StringTagItem)cbDocType.SelectedItem).Tag;

            string[] fizdoctypes = tbFizDocType.Text.Split(',');
            if (fizdoctypes != null)
            {
                if (fizdoctypes.Length < 1) fizdoctypes = null;
                else
                {
                    int nulls = 0;

                    for (int i = 0; i < fizdoctypes.Length; ++i)
                    {
                        if (fizdoctypes[i] != null) fizdoctypes[i] = fizdoctypes[i].Trim();
                        if ("".Equals(fizdoctypes[i])) fizdoctypes[i] = null;
                        if (fizdoctypes[i] == null) nulls++;
                    }

                    if (nulls == fizdoctypes.Length) fizdoctypes = null;
                }
            }

            string[] notreassign = tbNotReassign.Text.Split(',');
            if (notreassign != null)
            {
                if (notreassign.Length < 1) notreassign = null;
                else
                {
                    int nulls = 0;

                    for (int i = 0; i < notreassign.Length; ++i)
                    {
                        if (notreassign[i] != null) notreassign[i] = notreassign[i].Trim();
                        if ("".Equals(notreassign[i])) notreassign[i] = null;
                        if (notreassign[i] == null) nulls++;
                    }

                    if (nulls == notreassign.Length) notreassign = null;
                }
            }


            IDEXData d = (IDEXData)toolbox;
            string wwh = "where docid = '" + d.EscapeString(docid) + "' ";
            if (cbInterval.Checked) 
            {
                DateTime dt1 = deFrom.Value, dt2 = deTo.Value;
                if (dt1 > dt2)
                {
                    dt1 = deTo.Value;
                    dt2 = deFrom.Value;
                }

                wwh += "and jdocdate >= '" + dt1.ToString("yyyyMMdd") + "000000000' and jdocdate <= '" + dt2.ToString("yyyyMMdd") + "235959999' ";
            }

            if (rbExclude.Checked || rbInclude.Checked)
            {
                string ulist = "";
                foreach (StringTagItem sti in clbUnits.CheckedItems)
                {
                    if (!"".Equals(ulist)) ulist += ",";
                    ulist += "'" + sti.Tag + "'";
                }

                if (ulist != "")
                {
                    wwh += "and (unitid " + (rbExclude.Checked ? "not " : "") + "in (" + ulist + ")) ";
                }
                else
                {
                    er += "Не выделено ни одного отделения\n";
                }
            }

            List<string> lAdded = new List<string>();

            if (er == "")
            {

                long opcode = DateTime.Now.Ticks;
                IDEXCrypt crypt = (IDEXCrypt)toolbox;

                for (int i = 0; i < 2; ++i)
                {
                    if (cbSource.SelectedIndex == i || cbSource.SelectedIndex == 2)
                    {
                        try
                        {
                            DataTable dt = d.getQuery("select count(id) as cid from {0} {1}", tbname[i], wwh);
                            if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cid"]) > 0)
                            {
                                IDEXPeopleSearcher dps = (IDEXPeopleSearcher)toolbox;
                                PluginFramework plugins = ((IDEXPluginSystemData)toolbox).getPlugins();

                                int LIMIT = 1000;
                                int recCnt = Convert.ToInt32(dt.Rows[0]["cid"]), limCnt = 0, actCnt = 0;
                                wmea.textMessage = string.Format("Обработка записей в {0}", tbtext[i]);
                                wmea.minValue = 0;
                                wmea.maxValue = recCnt * 2;
                                wmea.progressVisible = true;
                                wmea.progressValue = recCnt * i;
                                wmea.DoEvents();

                                while (limCnt < recCnt)
                                {
                                    dt = d.getQuery("select signature, docid, jdocdate, status, unitid, data from {0} {1} limit {2}, {3}", tbname[i], wwh, limCnt, LIMIT);

                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow r in dt.Rows)
                                        {
                                            IDEXPluginDocument pdoc = plugins.getDocumentByID(r["docid"].ToString());
                                            if (pdoc != null)
                                            {
                                                SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());

                                                statusPassport = true;
                                                try
                                                {
                                                    if ( d.AccessRemoteServer )
                                                    {
                                                        //string conn = "server=192.168.0.64;port=3306;user id=passport;Password=12473513;" +
                                                        //                  "persist security info=True;database=passports;charset=cp1251;" +
                                                        //                  "Default Command Timeout=60";
                                                        string conn = "server={$db_server$};user id={$db_user$};Password={$db_pass$};" +
                                                            "persist security info=True;database={$db_name$}; charset=cp1251;" +
                                                             "Default Command Timeout=60";
                                                        StringBuilder sb = new StringBuilder(conn);
                                                        sb.Replace("{$db_server$}", d.PasspHostDb);
                                                        sb.Replace("{$db_name$}", d.PasspNameDb);
                                                        sb.Replace("{$db_user$}", d.PasspUserDb);
                                                        sb.Replace("{$db_pass$}", d.PasspPassDb);
                                                        conn = sb.ToString();
                                                        MySqlConnection con_test = new MySqlConnection(conn);
                                                        MySqlCommand cmd;
                                                        con_test.Open();
                                                        cmd = new MySqlCommand("select count(value) from `list_of_expired_passports` where value=@number", con_test);
                                                        cmd.Parameters.AddWithValue("@number", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                                                        cmd.ExecuteNonQuery();
                                                        MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                                                        DataTable t = new DataTable();
                                                        ada.Fill(t);
                                                        try
                                                        {
                                                            cmd = new MySqlCommand("select count(value) from `list_of_expired_passports2` where value=@number", con_test);
                                                            cmd.Parameters.AddWithValue("@number", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                                                            cmd.ExecuteNonQuery();
                                                            MySqlDataAdapter ada1 = new MySqlDataAdapter(cmd);
                                                            DataTable t1 = new DataTable();
                                                            ada1.Fill(t1);
                                                            if ( Convert.ToInt64(t1.Rows[0].ItemArray[0]) > 0 )
                                                            {
                                                                statusPassport = false;
                                                            }
                                                        }
                                                        catch ( Exception )
                                                        {
                                                        }
                                                        con_test.Close();
                                                        //if (t.Rows.Count != 0)
                                                        if ( Convert.ToInt64(t.Rows[0].ItemArray[0]) > 0 )
                                                        {
                                                            statusPassport = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DataTable tu = d.getQuery("select SQL_CALC_FOUND_ROWS value from `wrong_passports` where value='{0}'", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                                                        //if ( tu != null && tu.Rows.Count > 0 && Convert.ToInt32(tu.Rows[0]) > 0)
                                                        if ( tu != null )
                                                        {
                                                            statusPassport = false;
                                                        }
                                                    }

                                                }
                                                catch ( Exception )
                                                {
                                                }




                                                if (!true.ToString().Equals(xml["INVALID_DOCUMENT"]))
                                                {
                                                    int unitid = int.Parse(r["unitid"].ToString());

                                                    CDEXDocumentData docdata = new CDEXDocumentData();
                                                    docdata.documentText = r["data"].ToString();
                                                    docdata.documentDate = r["jdocdate"] as string;
                                                    docdata.documentStatus = int.Parse(r["status"].ToString());
                                                    docdata.documentUnitID = unitid;
                                                    docdata.signature = r["signature"].ToString();
                                                    

                                                    StringList sl = pdoc.GetPeopleData(toolbox, docdata);

                                                    if (!sl.ContainsKey("#isResident") || "1".Equals(sl["#isResident"]))
                                                    {

                                                        bool fdtok = (fizdoctypes == null);
                                                        if (!fdtok)
                                                        {
                                                            if (sl.ContainsKey("FizDocType")) {
                                                                string slfdt = sl["FizDocType"];
                                                                foreach (string sfdt in fizdoctypes)
                                                                {
                                                                    if (slfdt.Equals(sfdt))
                                                                    {
                                                                        fdtok = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (fdtok && statusPassport)
                                                        {
                                                            string nhash = crypt.StringToMD5(sl["firstname"] + sl["secondname"] + sl["lastname"] + sl["birth"]);
                                                            if (!lAdded.Contains(nhash))
                                                            {
                                                                string esNhash = d.EscapeString(nhash);

                                                                DataTable dt2 = d.getQuery("select id from `autodoc_people` where phash = '{0}'", esNhash);
                                                                if (dt2 == null || dt2.Rows.Count == 0 || cbRestrictDocCount.Checked)
                                                                {
                                                                    if (dt2 != null && dt2.Rows.Count > 0)
                                                                    {
                                                                        string query = "update `autodoc_people` set opcode = " + opcode.ToString() + ", doccount = 0";

                                                                        bool doReassign = true;
                                                                        if (notreassign != null && notreassign.Length > 0)
                                                                        {
                                                                            string sunitid = unitid.ToString();
                                                                            foreach (string nritem in notreassign)
                                                                            {
                                                                                if (sunitid.Equals(nritem))
                                                                                {
                                                                                    doReassign = false;
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }

                                                                        if (doReassign) query += ", unitid = " + unitid.ToString();
                                                                        query += "where phash = '" + d.EscapeString(esNhash) + "'";

                                                                        d.runQuery(query);

                                                                        //d.runQuery("update `autodoc_people` set opcode = {0}, doccount = 0, unitid = {1} where phash = '{2}'", opcode, unitid, esNhash);
                                                                    }
                                                                    else
                                                                    {
                                                                        d.runQuery("insert into `autodoc_people` (phash, opcode, unitid, firstname, secondname, lastname, birth, docid, data) " +
                                                                            "values ('{0}', {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                                                            d.EscapeString(nhash), opcode, unitid, d.EscapeString(sl["firstname"]), d.EscapeString(sl["secondname"]),
                                                                            d.EscapeString(sl["lastname"]), d.EscapeString(sl["birth"]), d.EscapeString(r["docid"].ToString()),
                                                                            d.EscapeString(sl.SaveToString()));
                                                                    }
                                                                    lAdded.Add(nhash);
                                                                    actCnt++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    wmea.progressValue = recCnt * i + limCnt;
                                    wmea.DoEvents();
                                    if (wmea.isAborted) return "Операция прервана";

                                    limCnt += LIMIT;
                                }

                                er += string.Format("В {0} взято и обработано {1} записей, подходящих: {2}\n", tbtext[i], recCnt, actCnt);
                            }
                            else
                                er += string.Format("В {0} документов нет подходящих записей\n", tbtext[i]);
                        }
                        catch (Exception ex)
                        {
                            er += "Ошибка " + ex.ToString() + "\n";
                        }
                    }
                }


                // Контроль повторяемости по всей БД
                //
                // Добавить во все БД                    
                // ALTER TABLE  `autodoc_people` ADD  `opcode` BIGINT( 20 ) NOT NULL DEFAULT  '0' COMMENT  'Код операции над записью' AFTER  `phash` , ADD INDEX (  `opcode` );
                // ALTER TABLE  `autodoc_people` ADD  `doccount` INT( 8 ) NOT NULL DEFAULT  '0' COMMENT  'Количество документов на данном абоненте' AFTER  `opcode` , ADD INDEX (  `doccount` )

                if (!wmea.isAborted) {
                    if (cbRestrictDocCount.Checked)
                    {
                        d.runQuery("update `autodoc_people` set doccount = 0 where opcode = {0}", opcode);

                        for (int i = 0; i < 2; ++i)
                        {
                            try
                            {
                                DataTable dt = d.getQuery("select count(id) as cid from {0}", tbname[i]);
                                if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cid"]) > 0)
                                {
                                    IDEXPeopleSearcher dps = (IDEXPeopleSearcher)toolbox;
                                    PluginFramework plugins = ((IDEXPluginSystemData)toolbox).getPlugins();

                                    int LIMIT = 1000;
                                    int recCnt = Convert.ToInt32(dt.Rows[0]["cid"]), limCnt = 0/*, actCnt = 0*/;
                                    wmea.textMessage = string.Format("Сверка записей в {0} с отчётом", tbtext[i]);
                                    wmea.minValue = 0;
                                    wmea.maxValue = recCnt * 2;
                                    wmea.progressVisible = true;
                                    wmea.progressValue = recCnt * i;
                                    wmea.DoEvents();

                                    while (limCnt < recCnt)
                                    {
                                        dt = d.getQuery("select signature, docid, jdocdate, status, unitid, data from {0} limit {1}, {2}", tbname[i], limCnt, LIMIT);

                                        if (dt != null && dt.Rows.Count > 0)
                                        {
                                            foreach (DataRow r in dt.Rows)
                                            {
                                                IDEXPluginDocument pdoc = plugins.getDocumentByID(r["docid"].ToString());
                                                if (pdoc != null)
                                                {
                                                    SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());

                                                    CDEXDocumentData docdata = new CDEXDocumentData();
                                                    docdata.documentText = r["data"].ToString();
                                                    docdata.documentDate = r["jdocdate"] as string;
                                                    docdata.documentStatus = int.Parse(r["status"].ToString());
                                                    docdata.documentUnitID = int.Parse(r["unitid"].ToString());
                                                    docdata.signature = r["signature"].ToString(); ;

                                                    StringList sl = pdoc.GetPeopleData(toolbox, docdata);

                                                    if (!sl.ContainsKey("#isResident") || "1".Equals(sl["#isResident"]))
                                                    {
                                                        string nhash = crypt.StringToMD5(sl["firstname"] + sl["secondname"] + sl["lastname"] + sl["birth"]);
                                                        if (lAdded.Contains(nhash))
                                                        {
                                                            DataTable dt2 = d.getQuery("select id, doccount from `autodoc_people` where phash = '{0}'", d.EscapeString(nhash));

                                                            if (dt2 != null && dt2.Rows.Count > 0)
                                                            {
                                                                if (Convert.ToInt32(dt2.Rows[0]["doccount"]) + 1 > nudDocCount.Value)
                                                                {
                                                                    d.runQuery("delete from `autodoc_people` where id = {0}", dt2.Rows[0]["id"].ToString());
                                                                    lAdded.Remove(nhash);
                                                                }
                                                                else
                                                                {
                                                                    d.runQuery("update `autodoc_people` set doccount = doccount + 1 where id = {0}", dt2.Rows[0]["id"].ToString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        wmea.progressValue = recCnt * i + limCnt;
                                        wmea.DoEvents();
                                        if (wmea.isAborted) return "Операция прервана";

                                        limCnt += LIMIT;
                                    }
// er += string.Format("В {0} взято и обработано {1} записей, подходящих: {2}\n", tbtext[i], recCnt, actCnt);

                                }
                            }
                            catch (Exception ex)
                            {
                                er += "Ошибка " + ex.ToString() + "\n";
                            }
                        }

                        d.runQuery("delete from `autodoc_people` where doccount > {0} and opcode = {1}", (int)nudDocCount.Value, opcode);
                        DataTable dt3 = d.getQuery("select count(id) as cid from `autodoc_people` where opcode = {0}", opcode);
                        if (dt3 != null && dt3.Rows.Count > 0)
                        {
                            er += "После контроля повторяемости, осталось " + dt3.Rows[0]["cid"].ToString() + " подходящих записей\n";
                        }
                        else
                        {
                            er += "Не удалось установить количество записей, оставшихся после контроля повторяемости\n";
                        }
                    }
                }
            }

            return er;
        }

        private void bBuild_Click(object sender, EventArgs e)
        {
            string s = WaitMessage.Execute(new WaitMessageEvent(RebuildPeople));
            MessageBox.Show("Результат выполнения:\n\n" + s);
        }

        private void bClear_Click(object sender, EventArgs e) //ad
        {
            int cnt = 0;
            IDEXData d = (IDEXData)toolbox;
            DataTable dt = d.getQuery("select count(id) as cid from `autodoc_people`");
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    cnt = Convert.ToInt32(dt.Rows[0]["cid"]);
                }
                catch (Exception) { }
            }

            if (cnt > 0)
            {
                if (MessageBox.Show(string.Format("В базе {0} записей.\n\nУдалить их?", cnt), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    d.runQuery("truncate table `autodoc_people`");
                    MessageBox.Show("База абонентов очищена");
                }
            }
            else
            {
                MessageBox.Show("База абонентов пуста.");
            }
                 
        }

        private void rbNotUse_CheckedChanged(object sender, EventArgs e)
        {
            clbUnits.Visible = !rbNotUse.Checked;
        }

        private void bCheckBadDocs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выполнить проверку корректности паспортных данных по всей базе?\r\n(Может быть очень долго)", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string s = WaitMessage.Execute(new WaitMessageEvent(CheckBadDocs));
                MessageBox.Show("Результат выполнения:\n\n" + s);
            }
        }

        public string CheckBadDocs(IWaitMessageEventArgs wmea)
        {

            /*
             * Всё очень просто:
             * 1. Очищаем check_duplicate, check_duplicate_res
             * 2. Выбираем документы и сличаем их с check_duplicate
             * 2.0. Если подобного документа нет - создаём в check_duplicate
             * 2.1. Если находится идентичный документ - приращиваем check_duplicate.refs_count
             * 2.2. Если находится документ с такими же паспортными данными, но с другим именем - создаём новую запись со ссылкой на самую раннюю запись с такими паспортными данными.
             * 3. Всем фиговым документам проставляем поле INVALID_DOCUMENT = true.ToString()
             * 4. Если заказан отчёт - формируем его из check_duplicate
             * 5. Очищаем people и перегоняем туда check_duplicate, игнорируя двойников. При этом, первое вхождение тоже перегоняем, но помечаем, как "подозрительное"
             */

            wmea.canAbort = true;
            wmea.progressVisible = false;

            string er = "";

            string[] tbname = new string[2] { "archive", "journal" };
            string[] tbtext = new string[2] { "архиве", "журнале" };
            string[] tbtext2 = new string[2] { "архиву", "журналу" };
            
            IDEXData d = (IDEXData)toolbox;
            IDEXConfig cfg = (IDEXConfig)toolbox;
            PluginFramework plugins = ((IDEXPluginSystemData)toolbox).getPlugins();
            IDEXCrypt crypt = (IDEXCrypt)toolbox;
            DataTable dt;
            int LIMIT = 50;

            wmea.textMessage = "Получение объёма задачи";
            wmea.DoEvents();

            int[] doc_counts = new int[2];

            for (int i = 0; i < 2; ++i)
            {
                try
                {
                    dt = d.getQuery("select count(id) as cid from `" + tbname[i] + "`");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        doc_counts[i] = Convert.ToInt32(dt.Rows[0]["cid"]);
                    }
                    if (wmea.isAborted) return "Операция прервана";
                }
                catch (Exception)
                {
                    return "Не удалось получить количество документов";
                }
            }

            wmea.textMessage = "Очистка проверочных таблиц";
            wmea.DoEvents();

            d.runQuery("truncate `check_duplicate`");
            d.runQuery("truncate `check_duplicate_refs`");

            int cur_doc = 0;

            wmea.minValue = 0;
            wmea.maxValue = doc_counts[0] + doc_counts[1];
            wmea.progressVisible = true;
            
            for (int i = 0; i < 2; ++i)
            {
                int limCnt = 0;
                wmea.textMessage = string.Format("Обработка записей в {0}", tbtext[i]);

                while (limCnt < doc_counts[i])
                {
                    dt = d.getQuery("select id, signature, docid, jdocdate, status, unitid, data from {0} limit {1}, {2}", tbname[i], limCnt, LIMIT);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            IDEXPluginDocument pdoc = plugins.getDocumentByID(r["docid"].ToString());
                            if (pdoc != null)
                            {
                                CDEXDocumentData docdata = new CDEXDocumentData();
                                docdata.documentText = r["data"].ToString();
                                docdata.documentDate = r["jdocdate"] as string;
                                docdata.documentStatus = int.Parse(r["status"].ToString());
                                docdata.documentUnitID = int.Parse(r["unitid"].ToString());
                                docdata.signature = r["signature"].ToString(); ;

                                StringList sl = pdoc.GetPeopleData(toolbox, docdata);
                                if (!sl.ContainsKey("#isResident") || "1".Equals(sl["#isResident"]))
                                {
                                    string fizdocnumber = sl["FizDocNumber"], fizdocseries = sl["FizDocSeries"],
                                        firstname = sl["FirstName"], secondname = sl["SecondName"], lastname = sl["LastName"],
                                        birth = sl["Birth"], DocReg = sl["DocReg"];

                                    string nhash = d.EscapeString(crypt.StringToMD5(firstname + secondname + lastname + birth));

                                    long early_id = long.MaxValue; // Самый ранний ID для случая 2.2
                                    DataTable dt2 = d.getQuery("select * from `check_duplicate` where fizdocnumber = '{0}' and fizdocseries = '{1}' order by id", 
                                        d.EscapeString(fizdocnumber), d.EscapeString(fizdocseries));
                                    if (dt2 != null && dt2.Rows.Count > 0)
                                    {
                                        foreach (DataRow r2 in dt2.Rows)
                                        {
                                            if (firstname.Equals(r2["firstname"].ToString()) && secondname.Equals(r2["secondname"].ToString()) &&
                                                lastname.Equals(r2["lastname"].ToString()) && birth.Equals(r2["birth"].ToString()))
                                            {
                                                // 2.1. Если находится идентичный документ - приращиваем check_duplicate.refs_count
                                                early_id = long.MaxValue;
                                                long cd_id = Convert.ToInt64(r2["id"]);
                                                d.runQuery("update `check_duplicate` set refs_count = refs_count + 1 where id = {0}", cd_id);
                                                d.runQuery("insert into `check_duplicate_refs` (rectype, cd_id, document_id) values ({0}, {1}, {2})", i, cd_id, Convert.ToInt32(r["id"]));
                                                break;
                                            }
                                            else
                                            {
                                                // 2.2. Если находится документ с такими же паспортными данными, но с другим именем - создаём новую запись со ссылкой на самую раннюю запись с такими паспортными данными.
                                                long new_early_id = Convert.ToInt64(r2["id"]);
                                                if (new_early_id < early_id) early_id = new_early_id;
                                            }
                                        }

                                        if (early_id != long.MaxValue)
                                        {
                                            long lid = d.runQueryReturnLastInsertedId(
                                                "insert into `check_duplicate` (parent_id, firstname, secondname, lastname, birth, fizdocnumber, fizdocseries, phash, data) " +
                                                "values ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                                early_id, d.EscapeString(firstname), d.EscapeString(secondname), d.EscapeString(lastname),
                                                d.EscapeString(birth), d.EscapeString(fizdocnumber), d.EscapeString(fizdocseries), nhash,
                                                d.EscapeString(sl.SaveToString()));

                                            d.runQuery("insert into `check_duplicate_refs` (rectype, cd_id, document_id) values ({0}, {1}, {2})", i, lid, Convert.ToInt32(r["id"]));

                                            d.runQuery("update `check_duplicate` set dubs_count = dubs_count + 1 where id = {0}", early_id);
                                        }
                                    }
                                    else
                                    {
                                        // 2.0. Если подобного документа нет - создаём в check_duplicate
                                        long lid = d.runQueryReturnLastInsertedId(
                                            "insert into `check_duplicate` (firstname, secondname, lastname, birth, fizdocnumber, fizdocseries, phash, data) " +
                                            "values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                                            d.EscapeString(firstname), d.EscapeString(secondname), d.EscapeString(lastname),
                                            d.EscapeString(birth), d.EscapeString(fizdocnumber), d.EscapeString(fizdocseries), nhash,
                                            d.EscapeString(sl.SaveToString()));

                                        d.runQuery("insert into `check_duplicate_refs` (rectype, cd_id, document_id) values ({0}, {1}, {2})", i, lid, Convert.ToInt32(r["id"]));
                                    }

                                }                                
                            }

                            cur_doc++;
                        }
                    }


                    wmea.progressValue = cur_doc;
                    wmea.DoEvents();

                    limCnt += LIMIT;
                    if (wmea.isAborted) return "Операция прервана";
                }
            }

            // Размаркировка документов, промаркированных ранее.
            wmea.textMessage = "Оценка количества маркированных документов";
            wmea.progressVisible = false;
            wmea.DoEvents();

            long[] marked_count = new long[2];
            for(int i = 0; i < 2; ++i) {
                dt = d.getQuery("select count(id) as cid from {0}  where data like '%<INVALID_DOCUMENT>{1}</INVALID_DOCUMENT>%'", tbname[i], true.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    marked_count[i] = Convert.ToInt64(dt.Rows[0]["cid"]);
                }
                else
                {
                    marked_count[i] = 0;
                }
            }


            if (marked_count[0] + marked_count[1] > 0)
            {
                cur_doc = 0;

                wmea.progressVisible = true;
                wmea.maxValue = (int)(marked_count[0] + marked_count[1]);

                for (int i = 0; i < 2; ++i)
                {
                    int limCnt = 0;
                    wmea.textMessage = string.Format("Размаркировка документов в {0}", tbtext[i]);

                    while (limCnt < marked_count[i])
                    {
                        dt = d.getQuery("select id, data from {0}  where data like '%<INVALID_DOCUMENT>{1}</INVALID_DOCUMENT>%' limit {2}, {3}", tbname[i], true.ToString(), limCnt, LIMIT);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow r in dt.Rows)
                            {
                                SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                                xml["INVALID_DOCUMENT"].Parent = null;
                                d.runQuery("update {0} set data = '{1}' where id = {2}", tbname[i], d.EscapeString(SimpleXML.SaveXml(xml)), r["id"]);
                                cur_doc++;
                                wmea.progressValue = cur_doc;
                            }
                        }

                        limCnt += LIMIT;
                        if (wmea.isAborted) return "Операция прервана";
                    }
                }
            }

            // 3. Всем фиговым документам проставляем поле <INVALID_DOCUMENT>true.ToString()</INVALID_DOCUMENT>
            wmea.textMessage = "Оценка количества документов для маркировки";
            wmea.progressVisible = false;
            wmea.DoEvents();

            for (int i = 0; i < 2; ++i)
            {
                dt = d.getQuery("select count(jrn.id) as cid from `{0}` as jrn, `check_duplicate_refs` as cdr , `check_duplicate` as cd " +
                    "where cdr.rectype = {1} and cdr.document_id = jrn.id and cd.id = cdr.cd_id and (cd.parent_id > -1 or cd.dubs_count > 0)", 
                    tbname[i], i);

                if (dt != null && dt.Rows.Count > 0)
                {
                    marked_count[i] = Convert.ToInt64(dt.Rows[0]["cid"]);
                }
                else
                {
                    marked_count[i] = 0;
                }
            }

            if (marked_count[0] + marked_count[1] > 0)
            {
                cur_doc = 0;

                wmea.progressVisible = true;
                wmea.maxValue = (int)(marked_count[0] + marked_count[1]);


                for (int i = 0; i < 2; ++i)
                {
                    int limCnt = 0;
                    wmea.textMessage = string.Format("Маркировка документов в {0}", tbtext[i]);

                    while (limCnt < marked_count[i])
                    {
                        dt = d.getQuery("select jrn.id, jrn.data from `{0}` as jrn, `check_duplicate_refs` as cdr , `check_duplicate` as cd " +
                            "where cdr.rectype = {1} and cdr.document_id = jrn.id and cd.id = cdr.cd_id and (cd.parent_id > -1 or cd.dubs_count > 0) limit {2}, {3}",
                            tbname[i], i, limCnt, LIMIT);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow r in dt.Rows)
                            {
                                SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                                xml["INVALID_DOCUMENT"].Text = true.ToString();

                                d.runQuery("update {0} set data = '{1}' where id = {2}", tbname[i], d.EscapeString(SimpleXML.SaveXml(xml)), r["id"]);
                                cur_doc++;
                                wmea.progressValue = cur_doc;
                            }
                        }

                        limCnt += LIMIT;
                        if (wmea.isAborted) return "Операция прервана";
                    }
                }
            }

            //Сборка таблицы people

            wmea.textMessage = "Перенос записей о паспортных данных";
            wmea.progressVisible = false;
            wmea.DoEvents();
            d.runQuery("truncate people");
            d.runQuery("insert into people (phash, firstname, secondname, lastname, birth, data) " +
                "(select phash, firstname, secondname, lastname, birth, data from `check_duplicate` where parent_id = -1 and dubs_count = 0)");

            d.runQuery("insert into people (phash, firstname, secondname, lastname, birth, suspect, data) " +
                "(select phash, firstname, secondname, lastname, birth, 1, data from `check_duplicate` where parent_id = -1 and dubs_count > 0)");

            if (cbBadDocsReport.Checked)
            {
                // 4. Если заказан отчёт - формируем его из check_duplicate

                string fn = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" +
                    cfg.getRegisterStr("config_name", "DEX") + " - Отчёт о некорректных паспортных данных, " + DateTime.Now.ToString("dd.MM.yyyy");

                int rep = 0;
                while (File.Exists(fn + (rep > 0 ? " - " + rep : "") + ".txt")) rep++;
                fn += (rep > 0 ? " - " + rep : "") + ".txt";

                FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                wmea.textMessage = "Оценка количества документов для отчёта";
                wmea.progressVisible = false;
                wmea.DoEvents();
                try
                {

                    for (int i = 0; i < 2; ++i)
                    {
                        dt = d.getQuery("select count(jrn.id) as cid from `{0}` as jrn, `check_duplicate_refs` as cdr , `check_duplicate` as cd " +
                            "where cdr.rectype = {1} and cdr.document_id = jrn.id and cd.id = cdr.cd_id and cd.dubs_count > 0",
                            tbname[i], i);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            marked_count[i] = Convert.ToInt64(dt.Rows[0]["cid"]);
                        }
                        else
                        {
                            marked_count[i] = 0;
                        }
                    }

                    if (marked_count[0] + marked_count[1] > 0)
                    {
                        cur_doc = 0;

                        for (int i = 0; i < 2; ++i)
                        {
                            if (marked_count[i] > 0)
                            {
                                sw.WriteLine("В " + tbtext[i] + " выявлены следующие документы:");
                                sw.WriteLine();


                                int limCnt = 0;
                                wmea.textMessage = string.Format("Формирование отчёта по {0}", tbtext2[i]);

                                while (limCnt < marked_count[i])
                                {
                                    dt = d.getQuery(
                                        "select jrn.digest, jrn.jdocdate, cd.id as cdid from `{0}` as jrn, `check_duplicate_refs` as cdr , `check_duplicate` as cd " +
                                        "where cdr.rectype = {1} and cdr.document_id = jrn.id and cd.id = cdr.cd_id and cd.dubs_count > 0 limit {2}, {3}",
                                        tbname[i], i, limCnt, LIMIT);

                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow r in dt.Rows)
                                        {
                                            sw.WriteLine("{0} {1}", docdate(r["jdocdate"].ToString()), r["digest"].ToString());

                                            DataTable dt2 = d.getQuery(
                                                "select jrn.digest, jrn.jdocdate from `{0}` as jrn, `check_duplicate_refs` as cdr , `check_duplicate` as cd " +
                                                "where cdr.rectype = {1} and cdr.document_id = jrn.id and cd.id = cdr.cd_id and cd.parent_id = {2}",
                                                tbname[i], i, Convert.ToInt64(r["cdid"]));

                                            if (dt2 != null && dt2.Rows.Count > 0)
                                            {
                                                foreach (DataRow r2 in dt2.Rows)
                                                {
                                                    sw.WriteLine("* {0} {1}", docdate(r2["jdocdate"].ToString()), r2["digest"].ToString());
                                                }
                                            }
                                            sw.WriteLine();

                                            cur_doc++;
                                            wmea.progressValue = cur_doc;
                                        }
                                    }

                                    limCnt += LIMIT;
                                    if (wmea.isAborted) break;
                                }

                            }
                            else
                            {
                                sw.WriteLine("В " + tbtext[i] + " не обнаруждено документов с некорректными паспортными данными.");
                                sw.WriteLine();
                            }
                            if (wmea.isAborted) break;
                        }
                    }
                    else
                    {
                        sw.WriteLine("Не обнаруждено документов с некорректными паспортными данными.");
                    }
                }
                catch (Exception) { }

                sw.Close();
                if (wmea.isAborted)
                {
                    File.Delete(fn);
                    return "Операция прервана";
                }

                Process.Start(fn);
            }

            er = "Операция выполнена успешно";


            return er;
        }

        string docdate(string src)
        {
            return src.Substring(6, 2) + "." + src.Substring(4, 2) + "." + src.Substring(0, 4);
        }

        private void bHelpNotReassign_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "При создании базы, паспортные данные не изменяют принадлежность, в случае, если обнаруженный договор\n" +
                "относится к указанным отделениям. Однако, количество документов на паспорте увеличивается.\n" +
                "Это необходимо для того, чтобы паспортные данные не \"съедались\"  \"долговым\" отделением,\n" +
                "которому принадлежат документы, сформированные автоматически.\n\n" +
                "Необходимо указать ID таких отделений через запятую."
                );
        }
    }
}
