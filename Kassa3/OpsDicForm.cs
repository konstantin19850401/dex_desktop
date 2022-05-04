using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public partial class OpsDicForm : Form
    {
        public Tools tools;
        FormState fs;
        OpEdForm opEd;

        public OpsDicForm()
        {
            InitializeComponent();
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\OpsDicForm.fs");
            opEd = new OpEdForm();
        }

        private void OpsDicForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
//            fs.LoadValue("pCat", pCat);
            RefreshOps();
        }

        private void OpsDicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
//            fs.SaveValue("pCat", pCat);
            fs.SaveToFile(tools.dataDir + @"\OpsDicForm.fs");
            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        void RefreshOps()
        {
            lbOps.SuspendLayout();
            lbOps.Visible = false;
            lbOps.Items.Clear();

            try
            {
                /*
                MySqlCommand cmd = new MySqlCommand("select * from ops order by title", tools.connection);
                DataTable dt = tools.MySqlFillTable(cmd);
                 */
                using (DataTable dt = Db.fillTable(Db.command("select * from ops order by title")))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                lbOps.Items.Add(new ClientItem(
                                    int.Parse(row["id"].ToString()), 0,
                                    row["title"].ToString(), row["shortcut"].ToString()
                                    ));
                            }
                            catch (Exception) { }
                        }
                        lbOps.Visible = true;
                    }
                }
            }
            catch (Exception) { }

            lbOps.ResumeLayout();
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            try
            {
                opEd.initForm(true, -1, "", "");
                if (opEd.ShowDialog() == DialogResult.OK)
                {
                    ClientItem ci = new ClientItem(opEd.id, 0, opEd.tbTitle.Text.Trim(), opEd.tbShortcut.Text.Trim());
                    lbOps.Items.Add(ci);
                    lbOps.SelectedItem = ci;
                    lbOps.Visible = true;
                }
            }
            catch (Exception) { }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (lbOps.SelectedItem != null)
            {
                ClientItem ci = (ClientItem)lbOps.SelectedItem;

                if (!Db.isMysql)
                {
                    int journal_cnt = 0, import_rules_cnt = 0, rules_cnt = 0;

                    using (DbCommand cmd = Db.command("select count(id) from journal where op_id = @op_id"))
                    {
                        Db.param(cmd, "op_id", ci.id);
                        journal_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (DbCommand cmd = Db.command("select count(id) from import_rules where op_id = @op_id"))
                    {
                        Db.param(cmd, "op_id", ci.id);
                        import_rules_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (DbCommand cmd = Db.command("select count(id) from rules where op_id = @op_id"))
                    {
                        Db.param(cmd, "op_id", ci.id);
                        rules_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string msg = "";
                    if (journal_cnt > 0) msg += "* Журнал операций\n";
                    if (import_rules_cnt > 0) msg += "* Правила импорта\n";
                    if (rules_cnt > 0) msg += "* Операционные правила в справочнике пользователей\n";

                    if (journal_cnt + import_rules_cnt + rules_cnt > 0)
                    {
                        MessageBox.Show("Невозможно удалить запись т.к. на неё ссылаются:\n\n" + msg);
                        return;
                    }

                }

                if (MessageBox.Show("Удалить операцию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (DbCommand cmd = Db.command("delete from ops where id = @id"))
                        {
                            Db.param(cmd, "id", ci.id);
                            cmd.ExecuteNonQuery();
                        }

                        lbOps.Items.Remove(ci);
                        lbOps.Visible = lbOps.Items.Count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить операцию."));
                    }
                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
            }

        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (lbOps.SelectedItem != null)
            {
                ClientItem ci = (ClientItem)lbOps.SelectedItem;
                try
                {
                    opEd.initForm(false, ci.id, ci.Shortcut, ci.Text);
                    if (opEd.ShowDialog() == DialogResult.OK)
                    {
                        ci.Text = opEd.tbTitle.Text.Trim();
                        ci.Shortcut = opEd.tbShortcut.Text.Trim();

                        lbOps.SuspendLayout();
                        lbOps.Items.Remove(ci);
                        lbOps.Items.Add(ci);
                        lbOps.SelectedItem = ci;
                        lbOps.ResumeLayout();
                    }
                }
                catch (Exception) { }
            }
        }

        private void lbOps_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                bNew_Click(bNew, null);
            }
            else if (e.KeyCode == Keys.Space)
            {
                bEdit_Click(bEdit, null);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                bDelete_Click(bDelete, null);
            }
        }
    }
}
