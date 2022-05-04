using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Data.Common;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public partial class RuleEditForm : Form
    {
        Tools tools = Tools.instance;

        string protocol;
        ImportRule rule;

        bool isNew;

        Dictionary<string, string> dKeys;

        List<StringObjTagItem>  lsotiSrcAcc = new List<StringObjTagItem>(),
                                lsotiDstAcc = new List<StringObjTagItem>(), 
                                lsotiSrcCli = new List<StringObjTagItem>(), 
                                lsotiDstCli = new List<StringObjTagItem>();

        StringObjTagItem sotiSrcAccSel, sotiDstAccSel, sotiSrcCliSel, sotiDstCliSel;

        List<long> deletedMatches = new List<long>();

        public RuleEditForm(string protocol, ImportRule rule, Dictionary<string, string> dKeys)
        {
            InitializeComponent();
            this.protocol = protocol;
            this.rule = rule;
            isNew = rule.id == -1;
            this.dKeys = dKeys;
            deletedMatches.Clear();

            // Наименование правила
            tbTitle.Text = rule.title;
            cbStatus.Checked = (rule.status == 1);

            // Параметры операции
            string opsWhere = tools.currentUser.prefs.getSqlForRuleType(OpRuleType.OPERATION, "id");
            /*
            DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from ops where " +
                opsWhere + " order by title", tools.connection));
             */

            using (DataTable dt = Db.fillTable(Db.command("select * from ops where " + opsWhere + " order by title")))
            {
                StringTagItem.UpdateCombo(cbOp, dt, null, "id", "title", true);
                StringTagItem.SelectByTag(cbOp, rule.op_id.ToString(), false);
            }

            //
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

            //
            if (rule.src_acc_id > -1) sotiSrcAccSel = findItem(lsotiSrcAcc, rule.src_acc_id);
            if (rule.src_client_id > -1) sotiSrcCliSel = findItem(lsotiSrcCli, rule.src_client_id);
            if (rule.dst_acc_id > -1) sotiDstAccSel = findItem(lsotiDstAcc, rule.dst_acc_id);
            if (rule.dst_client_id > -1) sotiDstCliSel = findItem(lsotiDstCli, rule.dst_client_id);
            cbSrcType.SelectedIndex = rule.srctype;
            cbDstType.SelectedIndex = rule.dsttype;

            tbRPrim.Text = rule.r_prim;

            // Контекстное меню строки "Примечание".
            cmsPrim.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in dKeys)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(kvp.Value);
                tsmi.Tag = kvp.Key;
                tsmi.Click += new EventHandler(tsmiPrim_Click);
                cmsPrim.Items.Add(tsmi);
            }

            // Матчи
            lbMatches.Items.Clear();
            foreach (ImportMatch match in rule.matches)
            {
                lbMatches.Items.Add(match);
            }

            cbSrcType_SelectedIndexChanged(cbSrcType, null);
            cbDstType_SelectedIndexChanged(cbDstType, null);

            tbTitle.Focus();
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
                foreach (StringObjTagItem soti in lsoti)
                {
                    if (soti.Tag.Equals(tag)) return soti;
                }
            }
            catch (Exception) { }

            return null;
        }


        private void tsmiPrim_Click(object sender, EventArgs e)
        {
            try
            {
                int sel = tbRPrim.SelectionStart;
                string s = tbRPrim.Text;
                string k = "${" + (string)((ToolStripMenuItem)sender).Tag + "}";
                if (tbRPrim.SelectionLength > 0) s = s.Remove(sel, tbRPrim.SelectionLength);
                tbRPrim.Text = s.Insert(tbRPrim.SelectionStart, k);
                tbRPrim.SelectionStart = sel + k.Length;
                tbRPrim.SelectionLength = 0;
            }
            catch (Exception) { }                
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

            if (cbSrcType.SelectedIndex == 1 && cbDstType.SelectedIndex == 1)
            {
                cbDstType.SelectedIndex = 0;
            }

            checkSrcDstType();
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

            if (cbSrcType.SelectedIndex == 1 && cbDstType.SelectedIndex == 1)
            {
                cbSrcType.SelectedIndex = 0;
            }

            checkSrcDstType();
        }

        void checkSrcDstType()
        {
            lWarning.Visible = cbSrcType.SelectedIndex == 0 && cbDstType.SelectedIndex == 0;
            /*
            int dstt = 1 - cbSrcType.SelectedIndex;
            if (dstt > 1) dstt = -1;
            cbDstType.SelectedIndex = dstt;
             */
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            ImportMatch match = new ImportMatch(-1, rule.id, "", -1, "", "");
            MatchForm mf = new MatchForm(match, dKeys);
            if (mf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Занесём новое правило в таблицу и в листбокс            
                rule.matches.Add(match);
                lbMatches.Items.Add(match);
            }
        }

        private void bDel_Click(object sender, EventArgs e)
        {
            if (lbMatches.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить критерий?", "Подтверждение", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    ImportMatch im = (ImportMatch)lbMatches.SelectedItem;
                    if (im.id > -1) deletedMatches.Add(im.id);
                    rule.matches.Remove(im);
                    lbMatches.Items.Remove(im);
                }
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (lbMatches.SelectedItem != null)
            {
                ImportMatch match = (ImportMatch)lbMatches.SelectedItem;
                MatchForm mf = new MatchForm(match, dKeys);
                if (mf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    match._changed = true;
                    int pos = lbMatches.Items.IndexOf(match);
                    int sel = lbMatches.SelectedIndex;
                    lbMatches.Items.Remove(match);
                    lbMatches.Items.Insert(pos, match);
                    lbMatches.SelectedIndex = sel;
                }
            }
            else
            {
                MessageBox.Show("Выберите критерий для редактирования");
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            if (tbTitle.Text.Trim().Equals("")) er += "* Отсутствует наименование правила\n";

            if (cbStatus.Checked)
            {
                if (cbOp.SelectedItem == null) er += "* Не выбрана операция\n";
                if (cbSrcType.SelectedIndex < 0) er += "* Не выбран тип источника\n";
                if (cbSrc.SelectedItem == null) er += "* Не указан источник операции\n";
                if (cbDstType.SelectedIndex < 0) er += "* Не указан тип приёмника\n";
                if (cbDst.SelectedItem == null) er += "* Не указан приёмник операции\n";
                if (cbSrcType.SelectedIndex ==  1 && cbSrcType.SelectedIndex == cbDstType.SelectedIndex) er += "* Источник и приёмник не могут указывать на контрагента одновременно\n";
                if (rule.matches.Count < 1) er += "* Не описано ни одного критерия импорта\n";
            }

            if (er == "") {

                rule.title = tbTitle.Text;
                rule.srctype = cbSrcType.SelectedIndex;
                rule.dsttype = cbDstType.SelectedIndex;
                try 
                {
                    rule.op_id = int.Parse(((StringTagItem)cbOp.SelectedItem).Tag);
                } 
                catch(Exception) 
                {
                    rule.op_id = -1;
                }

                rule.src_acc_id = -1;
                if (cbSrcType.SelectedIndex == 0 && cbSrc.SelectedItem != null) rule.src_acc_id = (int)((StringObjTagItem)cbSrc.SelectedItem).Tag;
                rule.src_client_id = -1;
                if (cbSrcType.SelectedIndex == 1 && cbSrc.SelectedItem != null) rule.src_client_id = (int)((StringObjTagItem)cbSrc.SelectedItem).Tag;
                rule.dst_acc_id = -1;
                if (cbDstType.SelectedIndex == 0 && cbDst.SelectedItem != null) rule.dst_acc_id = (int)((StringObjTagItem)cbDst.SelectedItem).Tag;
                rule.dst_client_id = -1;
                if (cbDstType.SelectedIndex == 1 && cbDst.SelectedItem != null) rule.dst_client_id = (int)((StringObjTagItem)cbDst.SelectedItem).Tag;

                rule.r_prim = tbRPrim.Text;
                rule.status = cbStatus.Checked ? 1 : 0;

                if (isNew)
                {
                    /*
                    using(MySqlCommand cmd = new MySqlCommand(
                        "insert into `import_rules` (protocol, title, op_id, r_prim, srctype, src_acc_id, src_client_id, dsttype, dst_acc_id, dst_client_id, status) " +
                        "values ('" + MySqlHelper.EscapeString(rule.protocol) + "', '" + MySqlHelper.EscapeString(rule.title) +
                        "', " + rule.op_id.ToString() + ", '" + MySqlHelper.EscapeString(rule.r_prim) +
                        "', " + rule.srctype.ToString() + ", " + rule.src_acc_id.ToString() + ", " + rule.src_client_id.ToString() +
                        ", " + rule.dsttype.ToString() + ", " + rule.dst_acc_id.ToString() + ", " + rule.dst_client_id.ToString() +
                        ", " + rule.status.ToString() + ")", tools.connection)) 
                     */

                    using (DbCommand cmd = Db.command(
                        "insert into import_rules (protocol, title, op_id, r_prim, srctype, src_acc_id, src_client_id, dsttype, dst_acc_id, dst_client_id, status) " +
                        "values ('" + Db.escape(rule.protocol) + "', '" + Db.escape(rule.title) +
                        "', " + rule.op_id.ToString() + ", '" + Db.escape(rule.r_prim) +
                        "', " + rule.srctype.ToString() + ", " + rule.src_acc_id.ToString() + ", " + rule.src_client_id.ToString() +
                        ", " + rule.dsttype.ToString() + ", " + rule.dst_acc_id.ToString() + ", " + rule.dst_client_id.ToString() +
                        ", " + rule.status.ToString() + ")")) 
                    {

                        cmd.ExecuteNonQuery();

//                        rule.id = cmd.LastInsertedId;
                        rule.id = Db.LastInsertedId(cmd, "import_rules");
                    }

                    foreach (ImportMatch match in rule.matches)
                    {
                        match.rule_id = rule.id;
                    }
                }
                else
                {
                    /*
                    using (MySqlCommand cmd = new MySqlCommand(
                        "update `import_rules` set title = '" + MySqlHelper.EscapeString(rule.title) + "', op_id = " + rule.op_id.ToString() +
                        ", r_prim = '" + MySqlHelper.EscapeString(rule.r_prim) + "', srctype = " + rule.srctype.ToString() + ", src_acc_id = " +
                        rule.src_acc_id.ToString() + ", src_client_id = " + rule.src_client_id.ToString() + ", dsttype = " + rule.dsttype.ToString() +
                        ", dst_acc_id = " + rule.dst_acc_id.ToString() + ", dst_client_id = " + rule.dst_client_id.ToString() + ", status = " +
                        rule.status.ToString() + " where id = " + 
                        rule.id.ToString(), tools.connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                     */

                    using (DbCommand cmd = Db.command(
                        "update `import_rules` set title = '" + Db.escape(rule.title) + "', op_id = " + rule.op_id.ToString() +
                        ", r_prim = '" + Db.escape(rule.r_prim) + "', srctype = " + rule.srctype.ToString() + ", src_acc_id = " +
                        rule.src_acc_id.ToString() + ", src_client_id = " + rule.src_client_id.ToString() + ", dsttype = " + rule.dsttype.ToString() +
                        ", dst_acc_id = " + rule.dst_acc_id.ToString() + ", dst_client_id = " + rule.dst_client_id.ToString() + ", status = " +
                        rule.status.ToString() + " where id = " +
                        rule.id.ToString()))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }

                foreach (ImportMatch match in rule.matches)
                {
                    if (match.id < 0)
                    {
                        // Новое правило
                        /*
                        using(MySqlCommand cmd = new MySqlCommand(
                            "insert into `import_matches` (rule_id, `field`, `operation`, match_value) values (" +
                            match.rule_id + ", '" + MySqlHelper.EscapeString(match.field) + "', " +
                            match.operation.ToString() + ", '" + MySqlHelper.EscapeString(match.match) + "')"
                            , tools.connection)) 
                        {
                            cmd.ExecuteNonQuery();
                            match.id = cmd.LastInsertedId;
                        }
                         */

                        using (DbCommand cmd = Db.command(
                            "insert into import_matches (rule_id, `field`, `operation`, match_value) values (" +
                            match.rule_id + ", '" + Db.escape(match.field) + "', " +
                            match.operation.ToString() + ", '" + Db.escape(match.match) + "')"
                            ))
                        {
                            cmd.ExecuteNonQuery();
                            match.id = Db.LastInsertedId(cmd, "import_matches");
                        }
                    }
                    else
                        if (match._changed)
                        {
                            // Правило изменено
                            /*
                            using (MySqlCommand cmd = new MySqlCommand(
                                "update `import_matches` set field = '" + MySqlHelper.EscapeString(match.field) +
                                "', operation = " + match.operation.ToString() + ", match_value = '" +
                                MySqlHelper.EscapeString(match.match) + "' where id = " + match.id.ToString()
                                , tools.connection))
                             */
                            using (DbCommand cmd = Db.command(
                                "update import_matches set field = '" + Db.escape(match.field) +
                                "', operation = " + match.operation.ToString() + ", match_value = '" +
                                Db.escape(match.match) + "' where id = " + match.id.ToString()
                                ))
                            {
                                cmd.ExecuteNonQuery();
                                match._changed = false;
                            }
                        }
                }

                if (deletedMatches.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (long match_id in deletedMatches)
                    {
                        if (sb.Length > 0) sb.Append(",");
                        sb.Append(match_id);
                    }

//                    using (MySqlCommand cmd = new MySqlCommand("delete from `import_matches` where id in (" + sb.ToString() + ")", tools.connection))
                    using (DbCommand cmd = Db.command("delete from `import_matches` where id in (" + sb.ToString() + ")"))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    deletedMatches.Clear();
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
            } else {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
        }

        private void lbMatches_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                bDel.PerformClick();
            }
        }

        private void RuleEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = (MessageBox.Show("Закрыть редактор правила импорта?", "Подтверждение", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No);
            }
        }

    }
}
