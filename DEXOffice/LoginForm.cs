using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;
using DEXExtendLib;

namespace DEXOffice
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            tbLogin.Text = "";
            tbPass.Text = "";
            cbSaveLogin.Checked = false;
            tbDbServer.Text = "";
            tbDbName.Text = "";
            tbDbUser.Text = "";
            tbDbPass.Text = "";
        }

        private string connectionStr
        {
            get
            {
                string conn = "server={$db_server$};user id={$db_user$};Password={$db_pass$};" +
                              "persist security info=True;database={$db_name$}; charset=cp1251;" +
                              "Default Command Timeout={$db_timeout$}";
                StringBuilder sb = new StringBuilder(conn);
                sb.Replace("{$db_server$}", tbDbServer.Text);
                sb.Replace("{$db_name$}", tbDbName.Text);                
                sb.Replace("{$db_user$}", tbDbUser.Text);
                sb.Replace("{$db_pass$}", tbDbPass.Text);
                sb.Replace("{$db_timeout$}", nudTimeout.Value.ToString());
                return sb.ToString();
            }
        }

        private void bCheckConnection_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DEXToolBox tb = DEXToolBox.getToolBox();
                MySqlConnection conn = tb.initConnection(connectionStr);
                if (conn == null || conn.State != ConnectionState.Open) throw new Exception("Неверные параметры или сервер отсутствует");
                conn.Close();
                Cursor = Cursors.Default;
                MessageBox.Show("Соединение с БД прошло успешно.");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Ошибка соединения с БД (" + ex.Message + ")");
            }
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            Console.WriteLine("BEGIN");
            bool isLogged = false;
            try
            {
                Cursor = Cursors.WaitCursor;

                DEXToolBox toolbox = DEXToolBox.getToolBox();

                string mpass = toolbox.StringToMD5(tbPass.Text);

                MySqlConnection c = toolbox.initConnection(connectionStr);

                MySqlCommand cmd = new MySqlCommand(
                    "select * from `users` where login=@login and pass=@pass", c
                    );
                cmd.Parameters.AddWithValue("login", tbLogin.Text);
                cmd.Parameters.AddWithValue("pass", mpass);

                DataTable dtUsers = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(dtUsers);

                foreach (DataRow row in dtUsers.Rows)
                {
                    if (tbLogin.Text.Equals(row["login"]) && mpass.Equals(row["pass"]))
                    {
                        isLogged = true;
                        toolbox.sLogin = row["login"].ToString();
                        toolbox.sPassword = row["pass"].ToString();
                        toolbox.sTitle = row["title"].ToString();
                        toolbox.sUID = row["uid"].ToString();

                        toolbox.sDateCreated = DateTime.ParseExact(row["datecreated"].ToString(), "yyyyMMddhhmmss", CultureInfo.InvariantCulture);
                        toolbox.sDateChanged = DateTime.ParseExact(row["datechanged"].ToString(), "yyyyMMddhhmmss", CultureInfo.InvariantCulture);

                        toolbox.sUserData = row["settings"].ToString();
                    }
                }

                Cursor = Cursors.Default;

                if (isLogged)
                {
                    bool sl = cbSaveLogin.Checked;
                    toolbox.setStr("login", "username", (sl) ? tbLogin.Text : null);
                    try
                    {
                        toolbox.setStr("login", "password", (sl) ? toolbox.Encrypt(tbPass.Text) : null);
                    }
                    catch (Exception)
                    {
                        toolbox.setStr("login", "password", "");
                    }
                    toolbox.setBool("login", "savelogin", sl);



                    toolbox.accessRemoteServer = true;
                    toolbox.passpHostDb = tbPasspHost.Text;
                    toolbox.passpNameDb = tbPasspName.Text;
                    toolbox.passpUserDb = tbPasspUser.Text;
                    toolbox.passpPassDb = tbPasspPass.Text;
                    toolbox.sDataBase = tbDbName.Text;
                    if ( rbPasspRem.Checked == true )
                    {
                        toolbox.setStr("passpBase", "remote", "1");
                    }
                    else
                    {
                        toolbox.accessRemoteServer = false;
                        toolbox.setStr("passpBase", "remote", "0");
                    }
                    toolbox.setStr("passpBase", "host", tbPasspHost.Text);
                    toolbox.setStr("passpBase", "basename", tbPasspName.Text);
                    toolbox.setStr("passpBase", "username", tbPasspUser.Text);
                    toolbox.setStr("passpBase", "userpass", tbPasspPass.Text);
                }
                else
                {
                    MessageBox.Show("Невозможно соединиться с БД\r\nУказан несуществующий пользователь или пароль");
                }
            }
            catch (MySqlException ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно соединиться с БД: " + ex.Message);
                isLogged = false;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Невозможно соединиться с БД: " + ex.Message);
                isLogged = false;
            }


            if (isLogged)
            {
                DialogResult = DialogResult.OK;
            }

        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DEXToolBox toolbox = DEXToolBox.getToolBox();
            toolbox.setStr("db", "address", tbDbServer.Text);
            toolbox.setStr("db", "name", tbDbName.Text);
            toolbox.setStr("db", "user", tbDbUser.Text);
            try 
            {
                toolbox.setStr("db", "password", toolbox.Encrypt(tbDbPass.Text));
            } 
            catch(Exception) 
            {
            }
            toolbox.setInt("db", "timeout", (int)nudTimeout.Value);
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            DEXToolBox toolbox = DEXToolBox.getToolBox();
            cbSaveLogin.Checked = toolbox.getBool("login", "savelogin", false);
            if (cbSaveLogin.Checked) 
            {
                tbLogin.Text = toolbox.getStr("login", "username", "");
                try
                {
                    tbPass.Text = toolbox.Decrypt(toolbox.getStr("login", "password", ""));
                }
                catch (Exception)
                {
                    tbPass.Text = "";
                }
            } 
            else 
            {
                tbLogin.Text = "";
                tbPass.Text = "";
            }

            tbDbServer.Text = toolbox.getStr("db", "address", "");
            tbDbName.Text = toolbox.getStr("db", "name", "");
            tbDbUser.Text = toolbox.getStr("db", "user", "");
            try
            {
                tbDbPass.Text = toolbox.Decrypt(toolbox.getStr("db", "password", ""));
            }
            catch (Exception)
            {
                tbDbPass.Text = "";
            }
            nudTimeout.Value = (decimal)toolbox.getInt("db", "timeout", 60);


            // подгрузка данных сервера

            if ( toolbox.getStr("passpBase", "remote", "") == "0" )
            {
                rbPasspLoc.Checked = true;
            }

            if ( toolbox.getStr("passpBase", "host", "") != "" )
            {
                tbPasspHost.Text = toolbox.getStr("passpBase", "host", "");
                tbPasspName.Text = toolbox.getStr("passpBase", "basename", "");
                tbPasspUser.Text = toolbox.getStr("passpBase", "username", "");
                tbPasspPass.Text = toolbox.getStr("passpBase", "userpass", "");
            }
        }

        private void ChangedPasspServer(object sender, EventArgs e)
        {
            if ( ( (RadioButton)sender ).Text == "Локальный" )
                tbPasspData.Enabled = false;
            else
                tbPasspData.Enabled = true;
        }


    }
}
