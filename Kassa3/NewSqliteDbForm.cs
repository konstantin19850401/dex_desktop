using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Kassa3
{
    public partial class NewSqliteDbForm : Form
    {
        int okPoints = 0;
        public string curDbName = null, curUser = null, curPass = null;

        private bool dbCreationFinished = false;

        public NewSqliteDbForm()
        {
            InitializeComponent();
            inCheck = true;
            lpb.Visible = false;
            pb.Visible = false;
            lDbMsg.Text = "";
            lUserMsg.Text = "";
            lPassMsg.Text = "";
            tbDb.Text = "";
            tbPass.Text = "";
            tbUser.Text = "";
            bCreate.Enabled = false;
            inCheck = false;
        }

        bool inCheck = false;

        void checkFields()
        {
            if (inCheck) return;
            
            inCheck = true;

            if (tbDb.Text != curDbName)
            {
                // Проверка базы
                curDbName = tbDb.Text.Trim();
                if (tbDb.Text != curDbName) tbDb.Text = curDbName;

                if (curDbName.Length < 1)
                {
                    lDbMsg.Text = "Не указано наименование базы";
                    lDbMsg.ForeColor = Color.Red;
                    okPoints = okPoints & 6;
                }
                else if (File.Exists(Tools.instance.localDbDir + curDbName + "." + Tools.SQLITE_DB_EXTENSION))
                {
                    lDbMsg.Text = "База с таким именем уже существует";
                    lDbMsg.ForeColor = Color.Red;
                    okPoints = okPoints & 6;
                }
                else
                {
                    lDbMsg.Text = "ОК";
                    lDbMsg.ForeColor = Color.Green;
                    okPoints = okPoints | 1;
                }
                
            }

            if (tbUser.Text != curUser)
            {
                // Проверка пользователя
                curUser = tbUser.Text;
                if (curUser.Trim().Length < 1)
                {
                    lUserMsg.Text = "Короткое имя";
                    lUserMsg.ForeColor = Color.Red;
                    okPoints = okPoints & 5;
                }
                else
                {
                    lUserMsg.Text = "ОК";
                    lUserMsg.ForeColor = Color.Green;
                    okPoints = okPoints | 2;
                }
            }

            if (tbPass.Text != curPass)
            {
                // Проверка пароля
                curPass = tbPass.Text;
                if (curPass.Length < 1)
                {
                    lPassMsg.Text = "Короткий пароль";
                    lPassMsg.ForeColor = Color.Red;
                    okPoints = okPoints & 3;
                }
                else
                {
                    lPassMsg.Text = "ОК";
                    lPassMsg.ForeColor = Color.Green;
                    okPoints = okPoints | 4;
                }
            }

            bCreate.Enabled = okPoints == 7;

            inCheck = false;
        }

        private void tbDb_TextChanged(object sender, EventArgs e)
        {
            checkFields();
        }

        private void bCreate_Click(object sender, EventArgs e)
        {
            if (okPoints == 7)
            {
                dbCreationFinished = false;
                timer1.Enabled = true;
                lpb.Visible = true;
                pb.Visible = true;
                bCreate.Visible = false;
                bCancel.Visible = false;

                new Thread(() =>
                {
                    Db.createSqliteDb(Tools.instance.localDbConnectionString(curDbName), curUser, curPass, curDbName);
                    dbCreationFinished = true;
                }).Start();

            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            int pbv = pb.Value + 1;
            if (pbv > 100) pbv = 0;
            pb.Value = pbv;
            pb.Refresh();
            pb.Invalidate();
            lpb.Refresh();
            lpb.Invalidate();
            if (dbCreationFinished)
            {
                timer1.Enabled = false;
                lpb.Visible = false;
                pb.Visible = false;
                bCreate.Visible = true;
                bCancel.Visible = true;
                DialogResult = DialogResult.OK;
            }
        }

        
    }
}
