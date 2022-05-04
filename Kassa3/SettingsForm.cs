using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data.Common;

namespace Kassa3
{
    public partial class SettingsForm : Form
    {
        bool isBoss = Tools.instance.isBoss;

        public SettingsForm()
        {
            InitializeComponent();

            gbLegalDate.Visible = isBoss;

//            DataTable dt = Tools.instance.MySqlFillTable(new MySqlCommand("select * from `kassa` limit 0, 1", Tools.instance.connection));

            using (DataTable dt = Db.fillTable(Db.command("select * from `kassa` limit 0, 1")))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];

                    tbDBTitle.Text = r["title"].ToString();

                    try
                    {
                        deLegalDate.Value = DateTime.ParseExact(r["legal_date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    }
                    catch (Exception)
                    {
                        deLegalDate.Value = new DateTime(1970, 2, 7);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            MySqlCommand cmd = new MySqlCommand("update `kassa` set title = @title", Tools.instance.connection);
            Tools.instance.SetDbParameter(cmd, "title", tbDBTitle.Text);
            cmd.ExecuteNonQuery();
            */

            using (DbCommand cmd = Db.command("update kassa set title = @title"))
            {
                Db.param(cmd, "title", tbDBTitle.Text);
                cmd.ExecuteNonQuery();
            }

            if (Tools.instance.isBoss)
            {
                if (deLegalDate.Value != null && deLegalDate.IsValid)
                {
                    string sd = deLegalDate.Value.ToString("yyyyMMdd");
                    /*
                    cmd = new MySqlCommand("update `kassa` set legal_date = @ldate", Tools.instance.connection);
                    Tools.instance.SetDbParameter(cmd, "ldate", sd);
                    cmd.ExecuteNonQuery();
                     */
                    using (DbCommand cmd = Db.command("update kassa set legal_date = @ldate"))
                    {
                        Db.param(cmd, "ldate", sd);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }
    }
}
