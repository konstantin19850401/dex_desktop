using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.People
{
    public partial class FPeopleMain : Form
    {
        object toolbox;
        BindingSource bs = new BindingSource();

        public FPeopleMain(object toolbox)
        {
            this.toolbox = toolbox;
            InitializeComponent();

            dgv.AutoGenerateColumns = false;
            dgv.Visible = false;
            dgv.DataSource = bs;
        }

        private void bFilter_Click(object sender, EventArgs e)
        {

            string where = "";

            IDEXData d = (IDEXData)toolbox;
            if (tbFirstname.Text.Trim().Length > 0) where += "firstname like '" + d.EscapeString(tbFirstname.Text) + "'";
            if (tbSecondname.Text.Trim().Length > 0) where += (where != "" ? " and " : "") + "secondname like '" + d.EscapeString(tbSecondname.Text) + "'";
            if (tbLastname.Text.Trim().Length > 0) where += (where != "" ? " and " : "") + "lastname like '" + d.EscapeString(tbLastname.Text) + "'";

            DateTime de = deBirth.Value;
            if (de.Ticks > 0) where += (where != "" ? " and " : "") + "birth = '" + de.ToString("dd.MM.yyyy") + "'";

            if (where != "") {
                string sql = "select * from `people` where " + where;
                string ret = WaitMessage.Execute(new WaitMessageEvent(doSelect), sql);
                if (ret != null) MessageBox.Show(ret);
            } else {
                MessageBox.Show("Необходимо указать хотя бы один критерий поиска");
            }
        }

        public string doSelect(IWaitMessageEventArgs wmea)
        {
            wmea.textMessage = "Выборка из БД";
            IDEXData d = (IDEXData)toolbox;
            string sql = (string)wmea.args[0];                
            DataTable dt = d.getQuery(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                bs.DataSource = dt;
                dgv.Visible = true;
            }
            else
            {
                dgv.Visible = false;
                return "Нет ни одной подходящей записи в БД";
            }
            return null;
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow r = ((DataRowView)bs.Current).Row;
                if (MessageBox.Show(
                    string.Format("Удалить запись о <{0} {1} {2}, {3}>?",
                        r["lastname"].ToString(), r["firstname"].ToString(), r["secondname"].ToString(), r["birth"].ToString()),
                        "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery("delete from `people` where phash = '{0}'", d.EscapeString(r["phash"].ToString()));
                    ((DataTable)bs.DataSource).Rows.Remove(r);
                }
            }
            catch (Exception) { }

        }

    }
}
