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
    public partial class UsersDicForm : Form
    {
        Tools tools;
        BindingSource bs;
        DbDataAdapter ada;
        DataTable dt;
        UserEdForm userEd;
        FormState fs;

        public UsersDicForm()
        {
            InitializeComponent();
            
            tools = Tools.instance;
            fs = new FormState(tools.dataDir + @"\UsersDicForm.fs");

            dgv.AutoGenerateColumns = false;

            try
            {
                dt = new DataTable();
                bs = new BindingSource();
                bs.DataSource = dt;
                dgv.DataSource = bs;

                ada = Db.dataAdapter();
                ada.SelectCommand = Db.command("select * from users");

                ada.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

            userEd = new UserEdForm();
        }

        private void UsersDicForm_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
        }

        private void UsersDicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            fs.SaveToFile(tools.dataDir + @"\UsersDicForm.fs");
            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        public void RefreshView()
        {
            string pid = "-";
            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    DataRowView drv = bs[dgv.SelectedRows[0].Index] as DataRowView;
                    pid = drv["id"].ToString();
                }
            }
            catch (Exception) { }

            dt.Clear();
            ada.Fill(dt);

            try
            {
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    DataRowView drv = bs[r.Index] as DataRowView;
                    r.Selected = drv["id"].ToString().Equals(pid);
                }
            }
            catch (Exception) { }
        }

        private void bDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                DataRowView drv = bs[dgv.SelectedRows[0].Index] as DataRowView;

                if (!Db.isMysql)
                {
                    int journal_cnt = 0, rules_cnt = 0;

                    using (DbCommand cmd = Db.command("select count(id) from journal where user_cr = @user_id or user_ch = @user_id"))
                    {
                        Db.param(cmd, "user_id", drv["id"]);
                        journal_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (DbCommand cmd = Db.command("select count(id) from rules where user_id = @user_id"))
                    {
                        Db.param(cmd, "user_id", drv["id"]);
                        rules_cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string msg = "";
                    if (journal_cnt > 0) msg += "* Журнал операций\n";
                    if (rules_cnt > 0) msg += "* Операционные правила в справочнике пользователей\n";

                    if (journal_cnt + rules_cnt > 0)
                    {
                        MessageBox.Show("Невозможно удалить запись т.к. на неё ссылаются:\n\n" + msg);
                        return;
                    }
                }


                if (MessageBox.Show(string.Format("Удалить пользователя <{0}>?", drv["login"].ToString()), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int dpid;

                    if (int.TryParse(drv["id"].ToString(), out dpid))
                    {
                        try
                        {
                            using (DbCommand cmdel = Db.command("delete from users where id = @id"))
                            {
                                Db.param(cmdel, "id", dpid);
                                cmdel.ExecuteNonQuery();
                            }
                            tools.MarkRecordDeleted(dpid, "users");
                        }
                        catch (Exception ex) 
                        {
                            MessageBox.Show(tools.DbErrorMsg(ex, "Не удалось удалить пользователя."));
                        }
                    }
                    RefreshView();
                }
            }
            else MessageBox.Show("Не выбрана запись.");
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void bNewUser_Click(object sender, EventArgs e)
        {
            userEd.prepareNewForm();
            if (userEd.ShowDialog() == DialogResult.OK)
            {
                RefreshView();
            }
        }

        private void bEditUser_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                DataRowView drv = bs[dgv.SelectedRows[0].Index] as DataRowView;
                int upid = 0;
                int.TryParse(drv["id"].ToString(), out upid);
                userEd.prepareEditForm(
                    upid, Convert.ToBoolean(drv["active"]), drv["login"].ToString(), 
                    drv["pass"].ToString(), drv["prefs"].ToString(), Convert.ToBoolean(drv["mark_new"])
                    );
                if (userEd.ShowDialog() == DialogResult.OK)
                {
                    RefreshView();
                    if (upid == tools.currentUser.PID)
                    {
                        string msg = "Вы изменили настройки собственной учётной записи.\nДля того, чтобы они вступили в силу, необходимо перезапустить Кассу.";
                        if ((int)AccessMode.WRITE != userEd.cbDicUsers.SelectedIndex)
                        {
                            msg += "\n\nВнимание!\nУ вас выключена возможность редактирования справочника пользователей.\nПосле перезапуска существует возможность того, что справочник пользователей не будет доступен ни одному из пользователей.";
                        }
                        MessageBox.Show(msg);
                    }
                }
            }
            else MessageBox.Show("Не выбрана запись.");
        }

        private void dgv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                bDeleteUser_Click(bDeleteUser, null);
            }
        }

        private void dgv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bEditUser_Click(bEditUser, null);
        }
    }
}
