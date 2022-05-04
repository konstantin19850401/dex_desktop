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
    public partial class CurrTypeEdForm : Form
    {
        public CurrTypeEdForm()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";
            if (tbCurrID.Text.Trim().Equals("")) er += "* Не указан идентификатор типа\n";
            if (tbName.Text.Trim().Equals("")) er += "* Не указано наименование\n";
            if (tbCode.Text.Trim().Equals("")) er += "* Не указан код валюты\n";
                
            /*
            Tools t = Tools.instance;
            MySqlCommand cmd = new MySqlCommand("select * from curr_list where curr_id = @curr_id or name = @name or code = @code", t.connection);
            t.SetDbParameter(cmd, "curr_id", tbCurrID.Text.Trim());
            t.SetDbParameter(cmd, "name", tbName.Text.Trim());
            t.SetDbParameter(cmd, "code", tbCode.Text.Trim());

            using (DataTable dt = t.MySqlFillTable(cmd))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    er += "* Уже имеется тип валюты с подобным идентификатором, наименованием или кодом валюты\n";
                }
            }
            */

            DbCommand cmd = Db.command("select * from curr_list where curr_id = @curr_id or name = @name or code = @code");
            Db.param(cmd, "curr_id", tbCurrID.Text.Trim());
            Db.param(cmd, "name", tbName.Text.Trim());
            Db.param(cmd, "code", tbCode.Text.Trim());
            DataTable dt = Db.fillTable(cmd);

            if (er.Equals("")) DialogResult = DialogResult.OK;
            else MessageBox.Show("Ошибки:\n\n" + er);
        }

        private void CurrTypeEdForm_Shown(object sender, EventArgs e)
        {
            tbCurrID.Text = "";
            tbName.Text = "";
            tbCurrID.Focus();
        }
    }
}
