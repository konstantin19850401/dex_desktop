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
    public partial class ClientsDicForm : Form
    {
        public Tools tools;
        FormState fs;
        ClientEdForm clientEd;

        public ClientsDicForm()
        {
            InitializeComponent();
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\ClientsDicForm.fs");
            clientEd = new ClientEdForm();
        }

        private void ClientsDicForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
            fs.LoadValue("pCat", pCat);
            RefreshCat();
            RefreshClients();
        }

        private void ClientsDicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            fs.SaveValue("pCat", pCat);
            fs.SaveToFile(tools.dataDir + @"\ClientsDicForm.fs");
            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        public void RefreshCat()
        {
            tvCat.Nodes.Clear();

            //            DataTable dt = tools.MySqlFillTable(new MySqlCommand("select * from client_cat order by cat_title", tools.connection));
            DataTable dt = Db.fillTable(Db.command("select * from client_cat order by cat_title"));

            if (dt != null && dt.Rows.Count > 0)
            {
                List<TreeNode> todo = new List<TreeNode>();
                DataRow[] search = dt.Select("parent_id = -1");
                TreeNode tn = new TreeNode(search[0]["cat_title"].ToString());
                tn.Name = search[0]["id"].ToString();
                todo.Add(tn);
                tvCat.Nodes.Add(tn);

                List<TreeNode> kill = new List<TreeNode>();
                List<TreeNode> add = new List<TreeNode>();

                while (todo.Count > 0)
                {
                    kill.Clear();
                    add.Clear();
                    foreach (TreeNode tnc in todo)
                    {
                        search = dt.Select("parent_id = " + tnc.Name);
                        foreach (DataRow drs in search)
                        {
                            tn = new TreeNode(drs["cat_title"].ToString());
                            tn.Name = drs["id"].ToString();
                            tnc.Nodes.Add(tn);
                            add.Add(tn);
                        }
                        kill.Add(tnc);
                    }

                    todo.AddRange(add);
                    foreach (TreeNode tnc in kill) todo.Remove(tnc);
                }
            } 
            else 
            {
                // Добавить корневую запись
                /*
                (new MySqlCommand(
                    "insert into client_cat (parent_id, cat_title) values (-1, 'Контрагенты')", tools.connection
                 )).ExecuteNonQuery();
                */

                long rootId;
                using (DbCommand cmd = Db.command("insert into client_cat (parent_id, cat_title) values (-1, 'Контрагенты')"))
                {
                    cmd.ExecuteNonQuery();
                    rootId = Db.LastInsertedId(cmd, "client_cat");
                }

                TreeNode tn = new TreeNode("Контрагенты");
                tn.Name = "" + rootId;
                tvCat.Nodes.Add(tn);

                tools.currentUser.prefs.needRebuildRuleMapping = true;
                tools.tmDataChanges.markTableChanged();
            }
        }

        public void RefreshClients()
        {
            lbClients.SuspendLayout();
            lbClients.Visible = false;
            lbClients.Items.Clear();

            if (tvCat.SelectedNode != null)
            {
                try
                {
                    /*
                    MySqlCommand cmd = new MySqlCommand("select * from client_data where cat_id = @cat_id order by title", tools.connection);
                    tools.SetDbParameter(cmd, "cat_id", int.Parse(tvCat.SelectedNode.Name));
                    DataTable dt = tools.MySqlFillTable(cmd);
                     */

                    DbCommand cmd = Db.command("select * from client_data where cat_id = @cat_id order by title");
                    Db.param(cmd, "cat_id", int.Parse(tvCat.SelectedNode.Name));
                    DataTable dt = Db.fillTable(cmd);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                lbClients.Items.Add(new ClientItem(
                                    int.Parse(row["id"].ToString()), int.Parse(row["cat_id"].ToString()),
                                    row["title"].ToString(), row["shortcut"].ToString()
                                    ));
                            }
                            catch (Exception) { }
                        }
                        lbClients.Visible = true;
                    }
                }
                catch (Exception) { }
            }

            lbClients.ResumeLayout();
        }

        private void tvCat_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }

        private void tvCat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                bNewCat_Click(bNewCat, null);
            }
            else if (e.KeyCode == Keys.F2 && tvCat.SelectedNode != null)
            {
                tvCat.SelectedNode.BeginEdit();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                bDelCat_Click(bDelCat, null);
            }
        }

        private void bNewCat_Click(object sender, EventArgs e)
        {
            if (tvCat.SelectedNode != null)
            {
                int parent_id = 0;
                if (int.TryParse(tvCat.SelectedNode.Name, out parent_id))
                {
                    string NewNodeName = "Новая категория";

                    try
                    {
                        /*
                        MySqlCommand cmd = new MySqlCommand(
                            "insert into client_cat (parent_id, cat_title) values (@parent_id, @cat_title)", tools.connection
                            );
                        tools.SetDbParameter(cmd, "parent_id", parent_id);
                        tools.SetDbParameter(cmd, "cat_title", NewNodeName);
                        cmd.ExecuteNonQuery();
                        */

                        using (DbCommand cmd = Db.command("insert into client_cat (parent_id, cat_title) values (@parent_id, @cat_title)"))
                        {
                            Db.param(cmd, "parent_id", parent_id);
                            Db.param(cmd, "cat_title", NewNodeName);
                            cmd.ExecuteNonQuery();

                            TreeNode tn = new TreeNode(NewNodeName);
//                            tn.Name = cmd.LastInsertedId.ToString();
                            tn.Name = Convert.ToString(Db.LastInsertedId(cmd, "client_cat"));
                            tvCat.SelectedNode.Nodes.Add(tn);
                            tvCat.SelectedNode.Expand();
                        }

                        tools.currentUser.prefs.needRebuildRuleMapping = true;
                        tools.tmDataChanges.markTableChanged();
                    }
                    catch (Exception) { }

                }
            }
        }

        private void tvCat_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = e.Node.Parent == null || e.Label == null || e.Label.Trim().Length < 1;
            if (!e.CancelEdit)
            {
                try
                {
                    /*
                    MySqlCommand cmd = new MySqlCommand("update client_cat set cat_title = @cat_title where id = @id", tools.connection);
                    tools.SetDbParameter(cmd, "cat_title", e.Label);
                    tools.SetDbParameter(cmd, "id", int.Parse(e.Node.Name));
                    cmd.ExecuteNonQuery();
                     */
                    using (DbCommand cmd = Db.command("update client_cat set cat_title = @cat_title where id = @id"))
                    {
                        Db.param(cmd, "cat_title", e.Label);
                        Db.param(cmd, "id", int.Parse(e.Node.Name));
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception) 
                {
                    e.CancelEdit = true;
                }

                tools.currentUser.prefs.needRebuildRuleMapping = true;
                tools.tmDataChanges.markTableChanged();
            }
        }

        private void tvCat_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvCat.SelectedNode.BeginEdit();
        }

        private void tvCat_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void tvCat_DragOver(object sender, DragEventArgs e)
        {
/*
            Point targetPoint = tvCat.PointToClient(new Point(e.X, e.Y));
            TreeNode newSel = tvCat.GetNodeAt(targetPoint);
            if (tvCat.SelectedNode != newSel)
            {
                tvCat.SelectedNode = newSel;
                tvCat.Invalidate();
            }
 */
        }

        private void tvCat_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = tvCat.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = tvCat.GetNodeAt(targetPoint);

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
                {
                    if (MessageBox.Show(string.Format("Перенести категорию <{0}> в категорию <{1}>?", draggedNode.Text, targetNode.Text), 
                        "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            /*
                            MySqlCommand cmd = new MySqlCommand("update client_cat set parent_id = @parent_id where id = @id", tools.connection);
                            tools.SetDbParameter(cmd, "parent_id", int.Parse(targetNode.Name));
                            tools.SetDbParameter(cmd, "id", int.Parse(draggedNode.Name));
                            cmd.ExecuteNonQuery();
                            */

                            using (DbCommand cmd = Db.command("update client_cat set parent_id = @parent_id where id = @id"))
                            {
                                Db.param(cmd, "parent_id", int.Parse(targetNode.Name));
                                Db.param(cmd, "id", int.Parse(draggedNode.Name));
                                cmd.ExecuteNonQuery();
                            }

                            draggedNode.Remove();
                            targetNode.Nodes.Add(draggedNode);
                        }
                        catch (Exception) { }
                        targetNode.Expand();

                        tools.currentUser.prefs.needRebuildRuleMapping = true;
                        tools.tmDataChanges.markTableChanged();
                    }
                }
            }
            else if (e.Data.GetDataPresent(typeof(ListBox.SelectedObjectCollection)))
            {
                ListBox.SelectedObjectCollection sitems = (ListBox.SelectedObjectCollection)e.Data.GetData(typeof(ListBox.SelectedObjectCollection));
                if (sitems.Count > 0)
                {
                    ClientItem ci = (ClientItem)sitems[0];
                    int targetPid = int.Parse(targetNode.Name);
                    if (targetPid != ci.cat_id)
                    {
                        if (MessageBox.Show(string.Format("Перенести выделенных контрагентов ({0}) в категорию <{1}>?", sitems.Count, targetNode.Text),
                            "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                using (DbCommand cmd = Db.command("update client_data set cat_id = @cat_id where id = @id"))
                                {
                                    Db.param(cmd, "cat_id", targetPid);

                                    foreach (ClientItem cli in sitems)
                                    {
//                                        tools.SetDbParameter(cmd, "id", cli.id);
                                        Db.param(cmd, "id", cli.id);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception) { }
                            tvCat.SelectedNode = targetNode;

                            tools.currentUser.prefs.needRebuildRuleMapping = true;
                            tools.tmDataChanges.markTableChanged();
                        }
                    }
                }
            }
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;
            return ContainsNode(node1, node2.Parent);
        }

        bool NodesHaveRecords(TreeNode node)
        {
            try
            {
                using (DbCommand cmd = Db.command("select count(id) cpid from client_data where cat_id = @cat_id"))
                {
                    //MySqlCommand cmd = new MySqlCommand("select count(id) cpid from client_data where cat_id = @cat_id", tools.connection);
//                    tools.SetDbParameter(cmd, "cat_id", int.Parse(node.Name));
                    Db.param(cmd, "cat_id", int.Parse(node.Name));
                    object sc = cmd.ExecuteScalar();
                    if (sc != System.DBNull.Value && Convert.ToInt64(sc) > 0) return true;
                }
            }
            catch (Exception) { }

            foreach (TreeNode tn in node.Nodes)
            {
                if (NodesHaveRecords(tn)) return true;
            }

            return false;
        }

        string GetCatList(TreeNode node, string pn = "")
        {
            string ret = pn;
            foreach (TreeNode tn in node.Nodes) ret = GetCatList(tn, ret);

            try
            {
                int nodeId = int.Parse(node.Name);
                if (ret != "") ret += ",";
                ret += "" + nodeId;
            }
            catch (Exception) { }

            return ret;
        }

        void DeleteCategory(TreeNode node)
        {
            foreach (TreeNode tn in node.Nodes) DeleteCategory(tn);

            try
            {
                int nodeId = int.Parse(node.Name);

                /*
                MySqlCommand cmd = new MySqlCommand("delete from client_cat where id = @id", tools.connection);
                tools.SetDbParameter(cmd, "id", nodeId);
                cmd.ExecuteNonQuery();
                 */

                using (DbCommand cmd = Db.command("delete from client_cat where id = @id"))
                {
                    Db.param(cmd, "id", nodeId);
                    cmd.ExecuteNonQuery();
                }

                tools.MarkRecordDeleted(nodeId, "client_cat");
            }
            catch (Exception) { }

            node.Remove();
            tools.currentUser.prefs.needRebuildRuleMapping = true;
            tools.tmDataChanges.markTableChanged();
        }

        private void bDelCat_Click(object sender, EventArgs e)
        {
            if (tvCat.SelectedNode == null || tvCat.SelectedNode.Parent == null) return;

            if (NodesHaveRecords(tvCat.SelectedNode))
            {
                MessageBox.Show("Нельзя удалить категорию, т.к. в ней есть контрагенты.");
                return;
            }

            if (!Db.isMysql)
            {
                string catlist = GetCatList(tvCat.SelectedNode);
                using (DbCommand cmd = Db.command("select count(id) from rules where cat_id in (" + catlist + ")"))
                {
                    int cnt = Convert.ToInt32(cmd.ExecuteScalar());

                    if (cnt > 0)
                    {
                        MessageBox.Show("Невозможно удалить категорию, т.к. на неё ссылаются операционные правила (" + cnt + ") в справочнике пользователей");
                        return;
                    }
                }

            }


            if ((tvCat.SelectedNode.Nodes.Count < 1 && MessageBox.Show(
                "Удалить категорию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                || MessageBox.Show(
                "Категория имеет подкатегории, которые также будут удалены.\nПродолжить?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DeleteCategory(tvCat.SelectedNode);
            }

        }

        private void bEditCat_Click(object sender, EventArgs e)
        {
            if (tvCat.SelectedNode != null) tvCat.SelectedNode.BeginEdit();
            
        }

        private void lbClients_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (lbClients.SelectedItems.Count > 0)
                {
                    DoDragDrop(lbClients.SelectedItems, DragDropEffects.Move);
                }
            }
        }

        private void tvCat_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshClients();                
        }

        private void bNewCli_Click(object sender, EventArgs e)
        {
            if (tvCat.SelectedNode != null)
            {
                try
                {
                    clientEd.initForm(true, -1, int.Parse(tvCat.SelectedNode.Name), "", "");
                    if (clientEd.ShowDialog() == DialogResult.OK)
                    {
                        ClientItem ci = new ClientItem(clientEd.id, clientEd.cat_id, clientEd.tbTitle.Text.Trim(), clientEd.tbShortcut.Text.Trim());
                        lbClients.Items.Add(ci);
                        lbClients.SelectedItem = ci;
                        lbClients.Visible = true;
                    }
                }
                catch (Exception) { }
            }
        }

        private void bEditCli_Click(object sender, EventArgs e)
        {
            if (lbClients.SelectedItem != null)
            {
                ClientItem ci = (ClientItem)lbClients.SelectedItem;
                try
                {
                    clientEd.initForm(false, ci.id, ci.cat_id, ci.Shortcut, ci.Text);
                    if (clientEd.ShowDialog() == DialogResult.OK)
                    {
                        ci.Text = clientEd.tbTitle.Text.Trim();
                        ci.Shortcut = clientEd.tbShortcut.Text.Trim();

                        lbClients.SuspendLayout();
                        lbClients.Items.Remove(ci);
                        lbClients.Items.Add(ci);
                        lbClients.SelectedItem = ci;
                        lbClients.ResumeLayout();
                    }
                }
                catch (Exception) { }
            }
        }

        private void bDelCli_Click(object sender, EventArgs e)
        {
            if (lbClients.SelectedItem != null)
            {
                ClientItem ci = (ClientItem)lbClients.SelectedItem;

                if (!Db.isMysql)
                {
                    int journal_cnt = 0, import_rules_cnt = 0, rules_cnt = 0;

                    using (DbCommand cmd = Db.command("select count(id) from journal where src_client_id = @cli_id or dst_client_id = @cli_id"))
                    {
                        Db.param(cmd, "cli_id", ci.id);
                        journal_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (DbCommand cmd = Db.command("select count(id) from import_rules where src_client_id = @cli_id or dst_client_id = @cli_id"))
                    {
                        Db.param(cmd, "cli_id", ci.id);
                        import_rules_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (DbCommand cmd = Db.command("select count(id) from rules where client_id = @cli_id"))
                    {
                        Db.param(cmd, "cli_id", ci.id);
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

                if (MessageBox.Show("Удалить контрагента?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        /*
                        MySqlCommand cmd = new MySqlCommand("delete from client_data where id = @id", tools.connection);
                        tools.SetDbParameter(cmd, "id", ci.id);
                        cmd.ExecuteNonQuery();
                        */

                        using (DbCommand cmd = Db.command("delete from client_data where id = @id"))
                        {
                            Db.param(cmd, "id", ci.id);
                            cmd.ExecuteNonQuery();
                        }

                        lbClients.Items.Remove(ci);
                        lbClients.Visible = lbClients.Items.Count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить запись."));
                    }

                    tools.currentUser.prefs.needRebuildRuleMapping = true;
                    tools.tmDataChanges.markTableChanged();
                }
            }
        }

        private void lbClients_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                bNewCli_Click(bNewCli, null);
            }
            else if (e.KeyCode == Keys.Space)
            {
                bEditCli_Click(bEditCli, null);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                bDelCli_Click(bDelCli, null);
            }
        }


    }
}
