using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Yota.Sim
{
    public partial class FBalanceMain : Form
    {
        object toolbox;
        FChangeBalance cbf;

        public FBalanceMain(object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;
            cbf = new FChangeBalance();
            refresh();
        }

        public void refresh()
        {
            string sid = "";
            try
            {
                sid = lbBal.Items[lbBal.SelectedIndex].ToString();
            }
            catch (Exception) { }

            DataTable dt = new DataTable();
            MySqlDataAdapter ada = ((IDEXMySqlData)toolbox).getDataAdapter("select * from `um_balances` order by title");
            ada.Fill(dt);

            lbBal.SuspendLayout();

            lbBal.Items.Clear();

            foreach (DataRow r in dt.Rows)
            {
                lbBal.Items.Add(r["title"].ToString());
            }

            try
            {
                lbBal.SelectedIndex = lbBal.Items.IndexOf(sid);
            }
            catch (Exception) { }

            lbBal.ResumeLayout();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            cbf.tbBalance.Text = "";
            bool q;
            do
            {
                q = true;
                if (cbf.ShowDialog() == DialogResult.OK)
                {
                    string er = "";
                    if (cbf.tbBalance.Text.Trim().Equals("")) er += "Указано пустое значение\n";
                    else if (lbBal.Items.Contains(cbf.tbBalance.Text.Trim())) er += "Такое значение уже имеется в списке\n";

                    if (er != "")
                    {
                        MessageBox.Show(er);
                        q = false;
                    }
                    else
                    {
                        ((IDEXData)toolbox).runQuery("insert into `um_balances` (title) values ('{0}')", 
                            ((IDEXData)toolbox).EscapeString(cbf.tbBalance.Text.Trim()));
                        lbBal.Items.Add(cbf.tbBalance.Text.Trim());
                    }
                    

                }
            } while (!q);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (lbBal.SelectedIndex > -1)
            {
                if (MessageBox.Show(string.Format("Удалить значение <{0}>?", lbBal.SelectedItem.ToString()),
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ((IDEXData)toolbox).runQuery("delete from `um_balances` where title = '{0}'",
                        ((IDEXData)toolbox).EscapeString(lbBal.SelectedItem.ToString()));
                    lbBal.Items.Remove(lbBal.SelectedItem);
                }
            }
        }
    }

    public class BalItem
    {
        public int id;
        public string title;

        public BalItem(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public override string ToString()
        {
            return title;
        }

        public int GetIndex(ListBox lb)
        {
            try
            {
                for (int i = 0; i < lb.Items.Count; ++i)
                {
                    if (id == ((BalItem)lb.Items[i]).id) return i;
                }
            }
            catch (Exception) { }

            return -1;
        }
    }
}
