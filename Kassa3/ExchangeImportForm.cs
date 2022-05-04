using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.IO;
using System.Globalization;
using System.Data.Common;

namespace Kassa3
{
    public partial class ExchangeImportForm : Form
    {
        Tools tools;
        string protocol;
        Dictionary<string, string> dKeys;

        public ExchangeImportForm(string protocol)
        {
            InitializeComponent();

            this.protocol = protocol;

            lbLog.Items.Clear();

            tools = Tools.instance;

            if ("ibank2".Equals(protocol))
            {
                Text = "Импорт из файла iBank2";

                dKeys = new Dictionary<string, string>()
                {
                    { "ACCOUNT", "Счёт клиента" },
                    { "OPER_ID", "Идентификатор операции" },
                    { "OPER_DATE", "Дата операции" },
                    { "OPER_CODE", "Код операции" },
                    { "DOC_DATE", "Дата документа" },
                    { "DOC_NUM", "Номер документа" },
                    { "CORR_INN", "ИНН получателя" },
                    { "CORR_NAME", "Наименование получателя" },
                    { "CORR_ACCOUNT", "Счет получателя" },
                    { "CORR_BANK_NAME", "Наименование банка получателя" },
                    { "CORR_BANK_BIC", "БИК банка получателя" },
                    { "CORR_BANK_ACC", "Корсчет банка получателя" },
                    { "CORR_KPP", "КПП получателя" },
                    { "CLN_INN", "ИНН плательщика" },
                    { "CLN_NAME", "Наименование плательщика" },
                    { "CLN_ACCOUNT", "Счет плательщика" },
                    { "CLN_BANK_NAME", "Наименование банка плательщика" },
                    { "CLN_BANK_BIC", "БИК банка плательщика" },
                    { "CLN_BANK_ACC", "Корсчет банка плательщика" },
                    { "CLN_KPP", "КПП плательщика" },
                    { "AMOUNT", "Сумма платежа" },
                    { "RUR_AMOUNT", "Рублевое покрытие операции" },
                    { "OPER_DETAILS", "Назначение платежа" },
                    { "CHARGE_CREATOR", "Статус составителя документа" },
                    { "CHARGE_KBK", "Код бюджетной классификации" },
                    { "CHARGE_OKATO", "Код OKATO" },
                    { "CHARGE_BASIS", "Основание платежа" },
                    { "CHARGE_PERIOD", "Налоговый период" },
                    { "CHARGE_NUM_DOC", "Бюджет - Номер документа" },
                    { "CHARGE_DATE_DOC", "Бюджет - Дата документа" },
                    { "CHARGE_TYPE", "Тип платежа" },
                    { "QUEUE", "Очередность платежа" }
                };
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                MessageBox.Show("Указанный протокол импорта отсутствует");
                return;
            }

            log(new ColorString(Text, Color.DarkRed));

            int loadedRules = 0;

            lbRules.Items.Clear();
            try
            {
                /*
                MySqlCommand cmd = new MySqlCommand("select * from `import_rules` where protocol = '" + protocol + "'", tools.connection);
                using (MySqlDataAdapter ada = new MySqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        ada.Fill(dt);
                 */

                        DataTable dt = Db.fillTable(Db.command("select * from `import_rules` where protocol = '" + protocol + "'"));

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                List<ImportMatch> matches = new List<ImportMatch>();
                                /*
                                ada.SelectCommand.CommandText = "select * from `import_matches` where rule_id = " + dr["id"].ToString();
                                using (DataTable dt2 = new DataTable())
                                {
                                    ada.Fill(dt2);
                                 */

                                    DataTable dt2 = Db.fillTable(Db.command("select * from `import_matches` where rule_id = " + dr["id"].ToString()));

                                    if (dt2 != null && dt2.Rows.Count > 0)
                                    {
                                        foreach(DataRow dr2 in dt2.Rows) {
                                            matches.Add(
                                                new ImportMatch(
                                                    Convert.ToInt32(dr2["id"]), 
                                                    Convert.ToInt32(dr2["rule_id"]), 
                                                    dr2["field"].ToString(),
                                                    Convert.ToInt16(dr2["operation"]),
                                                    dr2["match_value"].ToString(),
                                                    dKeys[dr2["field"].ToString()]
                                                )
                                            );
                                        }
                                        /*
                                    }
                                         */
                                }

                                ImportRule ir = new ImportRule(
                                    Convert.ToInt64(dr["id"]), //id
                                    dr["protocol"].ToString(),
                                    dr["title"].ToString(),
                                    Convert.ToInt32(dr["op_id"]),
                                    dr["r_prim"].ToString(),
                                    Convert.ToInt32(dr["srctype"]),
                                    Convert.ToInt32(dr["src_acc_id"]),
                                    Convert.ToInt32(dr["src_client_id"]),
                                    Convert.ToInt32(dr["dsttype"]),
                                    Convert.ToInt32(dr["dst_acc_id"]),
                                    Convert.ToInt32(dr["dst_client_id"]),
                                    Convert.ToInt16(dr["status"]),
                                    matches);

                                lbRules.Items.Add(ir);
                                loadedRules++;
                            }
                        }
/*
                    }
                }
 */ 
            }
            catch (Exception) { }

            log("Правил импорта загружено: " + loadedRules);
        }

        void log(object msg)
        {
            bool selectLast = lbLog.Items.Count - 1 == lbLog.SelectedIndex;
            lbLog.Items.Add(msg);
            if (selectLast) lbLog.SelectedIndex = lbLog.Items.Count - 1;
        }

        private void lbLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                object sitem = ((ListBox)sender).Items[e.Index];

                Color foreColor = Color.Black;
                if (sitem is IStrColor) foreColor = ((IStrColor)sitem).getColor();
                e.DrawBackground();
                e.Graphics.DrawString(sitem.ToString(), e.Font, new SolidBrush(foreColor), e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
            catch (Exception) { }

        }

        private void bNew_Click(object sender, EventArgs e)
        {
            ImportRule rule = new ImportRule(-1, protocol, "", -1, "", -1, -1, -1, -1, -1, -1, 0, new List<ImportMatch>());
            RuleEditForm reff = new RuleEditForm(protocol, rule, dKeys);
            if (reff.ShowDialog() == DialogResult.OK)
            {
                //TODO Добавить правило в список lbRules
                lbRules.Items.Add(rule);
                log("Добавлено правило <" + rule.title + ">");
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (lbRules.SelectedItem != null)
            {
                ImportRule rule = (ImportRule)lbRules.SelectedItem;
                RuleEditForm reff = new RuleEditForm(protocol, rule, dKeys);
                if (reff.ShowDialog() == DialogResult.OK)
                {
                    int pos = lbRules.Items.IndexOf(rule);
                    int cur = lbRules.SelectedIndex;
                    lbRules.Items.Remove(rule);
                    lbRules.Items.Insert(pos, rule);
                    lbRules.SelectedIndex = cur;
                }
            }
            else
            {
                MessageBox.Show("Выберите правило для редактирования");
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (lbRules.SelectedItem != null)
            {
                ImportRule rule = (ImportRule)lbRules.SelectedItem;

                if (MessageBox.Show("Удалить правило <" + rule.title + ">?", "Подтверждение", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    /*
                    using (MySqlCommand cmd = new MySqlCommand("delete from `import_matches` where rule_id = " + rule.id.ToString(), tools.connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                     */

                    using (DbCommand cmd = Db.command("delete from `import_matches` where rule_id = " + rule.id.ToString()))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    /*
                    using (MySqlCommand cmd = new MySqlCommand("delete from `import_rules` where id = " + rule.id.ToString(), tools.connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    */

                    using (DbCommand cmd = Db.command("delete from `import_rules` where id = " + rule.id.ToString()))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    lbRules.Items.Remove(rule);
                }
            }
            else
            {
                MessageBox.Show("Выберите правило для удаления");
            }
        }


        static ColorString positive(string str)
        {
            return new ColorString(str, Color.DarkGreen);
        }

        static ColorString errstr(string str)
        {
            return new ColorString(str, Color.Red);
        }

        static ColorString warn(string str)
        {
            return new ColorString(str, Color.DarkMagenta);
        }

        ImportRule createRuleFromRec(Dictionary<string, string> src)
        {
            string ptitle = null, pr_prim = null;

            string[] iflds = null;
            if ("ibank2".Equals(protocol)) {
                iflds = new string[] {
                    "CORR_INN", "CORR_NAME", "CORR_ACCOUNT", "CORR_BANK_NAME", "CORR_BANK_BIC", "CORR_BANK_ACC", "CORR_KPP",
                    "CLN_INN", "CLN_NAME", "CLN_ACCOUNT", "CLN_BANK_NAME", "CLN_BANK_BIC", "CLN_BANK_ACC", "CLN_KPP", 
                    "CHARGE_CREATOR", "CHARGE_KBK", "CHARGE_OKATO", "CHARGE_BASIS", "CHARGE_PERIOD",  "CHARGE_TYPE", "QUEUE" 
                };
                ptitle = src["OPER_DETAILS"];
                pr_prim = "${OPER_DETAILS}";
            }


            if (iflds == null) return null;


            List<ImportMatch> matches = new List<ImportMatch>();
            foreach(string ifld in iflds) 
            {
                if (src.ContainsKey(ifld)) 
                {
                    string val = src[ifld];
                    if (val != null && !"".Equals(val.Trim())) matches.Add(new ImportMatch(-1, -1, ifld, 0, val, dKeys[ifld]));
                }
            }


            return new ImportRule(-1, protocol, ptitle, -1, pr_prim, -1, -1, -1, -1, -1, -1, 0, matches);
        }

        private void bImport_Click(object sender, EventArgs e)
        {
            log("Выбор файла для ипорта");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
//                    throw new Exception("test");
                    log(positive("Выбран файл: " + ofd.FileName));
                    log(positive("Загрузка"));
                    string[] src = File.ReadAllLines(ofd.FileName, Encoding.GetEncoding(1251));
                    log(positive("Строк считано: " + src.Length));
                    log(positive("Разбор файла"));

                    List<Dictionary<string, string>> ldss = DocumentParser.parse(src, protocol);
                    if (ldss == null) throw new Exception("Не удалось разобрать содержимое документа");

                    if (ldss.Count < 2) throw new Exception("Документ не содержит записей об операциях");

                    log(positive("Записей об операциях: " + (ldss.Count - 1).ToString()));
                    
                    log(positive("Обработка записей"));

                        foreach (Dictionary<string, string> rec in ldss)
                        {
                            if (rec.ContainsKey("$$BLOCK") && "body".Equals(rec["$$BLOCK"]))
                            {
                                string did = DocumentParser.getDocumentId(rec, protocol);
                                bool imported = false;
                                foreach (ImportRule ir in lbRules.Items)
                                {
                                    int cc = ir.check(rec);
                                    if (cc == 0)
                                    {
                                        log(positive("Документ " + did + " соответствует правилу " + ir.title));

                                        /*
                                        int dc = Convert.ToInt32(
                                            (new MySqlCommand(
                                                "select count(id) as cid from `journal` where foreign_id = '" + MySqlHelper.EscapeString(did) + "'",
                                                tools.connection)
                                            ).ExecuteScalar()
                                        );
                                        */

                                        int dc;

                                        using (DbCommand cmd = Db.command("select count(id) as cid from journal where foreign_id = '" + Db.escape(did) + "'"))
                                        {
                                            dc = Convert.ToInt32(cmd.ExecuteScalar());
                                        }

                                        if (dc > 0)
                                        {
                                            log(warn("Документ " + did + " импортирован ранее"));
                                        }
                                        else
                                        {
                                            if (ir.status == 1)
                                            {
                                                // Импорт документа
                                                /*
                                                MySqlCommand cmd = new MySqlCommand(
                                                    "insert into journal (op_id, r_date, r_sum, r_prim, srctype, src_acc_id, src_client_id, src_curr_value, " +
                                                    "dsttype, dst_acc_id, dst_client_id, dst_curr_value, user_cr, foreign_id) values (@op_id, @r_date, @r_sum, " +
                                                    "@r_prim, @srctype, @src_acc_id, @src_client_id, @src_curr_value, @dsttype, @dst_acc_id, @dst_client_id, " +
                                                    "@dst_curr_value, @user_cr, @foreign_id)"
                                                    , tools.connection);

                                                tools.SetDbParameter(cmd, "op_id", ir.op_id);
                                                tools.SetDbParameter(cmd, "r_date", DocumentParser.getR_Date(rec, protocol));
                                                tools.SetDbParameter(cmd, "r_sum", DocumentParser.getR_Sum(rec, protocol));
                                                tools.SetDbParameter(cmd, "r_prim", ir.getR_Prim(rec));
                                                tools.SetDbParameter(cmd, "srctype", ir.srctype);
                                                tools.SetDbParameter(cmd, "src_acc_id", ir.srctype == 0 ? (object)ir.src_acc_id : null);
                                                tools.SetDbParameter(cmd, "src_client_id", ir.srctype == 1 ? (object)ir.src_client_id : null);
                                                tools.SetDbParameter(cmd, "src_curr_value", ir.srctype == 0 ? 1 : 0);
                                                tools.SetDbParameter(cmd, "dsttype", ir.dsttype);
                                                tools.SetDbParameter(cmd, "dst_acc_id", ir.dsttype == 0 ? (object)ir.dst_acc_id : null);
                                                tools.SetDbParameter(cmd, "dst_client_id", ir.dsttype == 1 ? (object)ir.dst_client_id : null);
                                                tools.SetDbParameter(cmd, "dst_curr_value", ir.dsttype == 0 ? 1 : 0);
                                                tools.SetDbParameter(cmd, "user_cr", tools.currentUser.PID);
                                                tools.SetDbParameter(cmd, "foreign_id", did);

                                                cmd.ExecuteNonQuery();
                                                 */

                                                string sql =
                                                    "insert into journal (op_id, r_date, r_sum, r_prim, srctype, src_acc_id, src_client_id, src_curr_value, " +
                                                    "dsttype, dst_acc_id, dst_client_id, dst_curr_value, user_cr, foreign_id) values (@op_id, @r_date, @r_sum, " +
                                                    "@r_prim, @srctype, @src_acc_id, @src_client_id, @src_curr_value, @dsttype, @dst_acc_id, @dst_client_id, " +
                                                    "@dst_curr_value, @user_cr, @foreign_id)";

                                                using (DbCommand cmd = Db.command(sql))
                                                {
                                                    Db.param(cmd, "op_id", ir.op_id);
                                                    Db.param(cmd, "r_date", DocumentParser.getR_Date(rec, protocol));
                                                    Db.param(cmd, "r_sum", DocumentParser.getR_Sum(rec, protocol));
                                                    Db.param(cmd, "r_prim", ir.getR_Prim(rec));
                                                    Db.param(cmd, "srctype", ir.srctype);
                                                    Db.param(cmd, "src_acc_id", ir.srctype == 0 ? (object)ir.src_acc_id : null);
                                                    Db.param(cmd, "src_client_id", ir.srctype == 1 ? (object)ir.src_client_id : null);
                                                    Db.param(cmd, "src_curr_value", ir.srctype == 0 ? 1 : 0);
                                                    Db.param(cmd, "dsttype", ir.dsttype);
                                                    Db.param(cmd, "dst_acc_id", ir.dsttype == 0 ? (object)ir.dst_acc_id : null);
                                                    Db.param(cmd, "dst_client_id", ir.dsttype == 1 ? (object)ir.dst_client_id : null);
                                                    Db.param(cmd, "dst_curr_value", ir.dsttype == 0 ? 1 : 0);
                                                    Db.param(cmd, "user_cr", tools.currentUser.PID);
                                                    Db.param(cmd, "foreign_id", did);

                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            {
                                                log(errstr("Правило импорта отключено"));
                                            }
                                        }

                                        imported = true;
                                        break;
                                    }
                                }

                                if (!imported)
                                {
                                    log(warn("Не найдено правила для документа " + did));
                                    ImportRule irn = createRuleFromRec(rec);
                                    bool ruleExists = false;
                                    string irn_md5 = irn.getSignature();
                                    foreach (ImportRule ir in lbRules.Items)
                                    {
                                        if (irn_md5.Equals(ir.getSignature()))
                                        {
                                            ruleExists = true;
                                            break;
                                        }
                                    }

                                    if (!ruleExists)
                                    {
                                        /*
                                        using (MySqlCommand cmd = new MySqlCommand(
                                            "insert into `import_rules` (protocol, title, op_id, r_prim, srctype, src_acc_id, src_client_id, dsttype, dst_acc_id, dst_client_id, status) " +
                                            "values ('" + MySqlHelper.EscapeString(irn.protocol) + "', '" + MySqlHelper.EscapeString(irn.title) +
                                            "', " + irn.op_id.ToString() + ", '" + MySqlHelper.EscapeString(irn.r_prim) +
                                            "', " + irn.srctype.ToString() + ", " + irn.src_acc_id.ToString() + ", " + irn.src_client_id.ToString() +
                                            ", " + irn.dsttype.ToString() + ", " + irn.dst_acc_id.ToString() + ", " + irn.dst_client_id.ToString() +
                                            ", " + irn.status.ToString() + ")", tools.connection))
                                         */

                                        string sql =
                                            "insert into import_rules (protocol, title, op_id, r_prim, srctype, src_acc_id, src_client_id, dsttype, dst_acc_id, dst_client_id, status) " +
                                            "values ('" + Db.escape(irn.protocol) + "', '" + Db.escape(irn.title) +
                                            "', " + irn.op_id.ToString() + ", '" + Db.escape(irn.r_prim) +
                                            "', " + irn.srctype.ToString() + ", " + irn.src_acc_id.ToString() + ", " + irn.src_client_id.ToString() +
                                            ", " + irn.dsttype.ToString() + ", " + irn.dst_acc_id.ToString() + ", " + irn.dst_client_id.ToString() +
                                            ", " + irn.status.ToString() + ")";

                                        using(DbCommand cmd = Db.command(sql))
                                        {

                                            cmd.ExecuteNonQuery();

                                            //irn.id = cmd.LastInsertedId;
                                            irn.id = Db.LastInsertedId(cmd, "import_rules");
                                        }

                                        foreach (ImportMatch match in irn.matches)
                                        {
                                            match.rule_id = irn.id;
                                        }

                                        foreach (ImportMatch match in irn.matches)
                                        {
                                            if (match.id < 0)
                                            {
                                                // Новое правило
                                                /*
                                                using (MySqlCommand cmd = new MySqlCommand(
                                                    "insert into `import_matches` (rule_id, `field`, `operation`, match_value) values (" +
                                                    match.rule_id + ", '" + MySqlHelper.EscapeString(match.field) + "', " +
                                                    match.operation.ToString() + ", '" + MySqlHelper.EscapeString(match.match) + "')"
                                                    , tools.connection))
                                                 */

                                                using (DbCommand cmd = Db.command(
                                                    "insert into import_matches (rule_id, `field`, `operation`, match_value) values (" +
                                                    match.rule_id + ", '" + Db.escape(match.field) + "', " +
                                                    match.operation.ToString() + ", '" + Db.escape(match.match) + "')"))
                                                {
                                                    cmd.ExecuteNonQuery();
                                                    //match.id = cmd.LastInsertedId;
                                                    match.id = Db.LastInsertedId(cmd, "import_matches");
                                                }
                                            }
                                        }


                                        lbRules.Items.Add(irn);

                                        log(warn("Добавлено отключённое правило"));
                                    }
                                }
                            }
                        }
                }
                catch (Exception ex)
                {
                    log(errstr("Ошибка: " + ex.Message));
                }
            }
            else
            {
                log(errstr("Импорт прерван"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }


    }

    
}
