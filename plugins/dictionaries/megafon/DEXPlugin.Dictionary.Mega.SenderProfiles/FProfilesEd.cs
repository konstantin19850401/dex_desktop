using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.Mega.SenderProfiles
{
    public partial class FProfilesEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public FProfilesEd()
        {
            InitializeComponent();
        }

        public void InitForm(Object AToolBox, DataTable AData, DataRow ARow)
        {
            toolbox = AToolBox;
            data = AData;
            row = ARow;
            if (row == null)
            {
                tbProfile.Text = "";
                tbLogin.Text = "";
                tbPassword.Text = "";
                tbLink.Text = "";
                cbPsubscribers.Checked = false;
            }
            else
            {
                tbProfile.Text = row["pname"].ToString();
                tbLogin.Text = row["plogin"].ToString();
                tbPassword.Text = row["ppassword"].ToString();
                tbLink.Text = row["plink"].ToString();
                cbPsubscribers.Checked = bool.Parse(row["psubscribers"].ToString());
            }

            tbProfile.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            string clProfile = tbProfile.Text.Trim();
            string clLogin = tbLogin.Text.Trim();
            string clLink = tbLink.Text.Trim();

            if (clProfile.Length < 1)
            {
                er += "* Не указано наименование профиля\n";
            }

            if (clLogin.Length < 1)
            {
                er += "* Не указано имя пользователя\n";
            }

            if (clLink.Length < 1)
            {
                er += "* Не указана страница УД\n";
            }

            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                }

                nr.BeginEdit();

                nr["pname"] = tbProfile.Text;
                nr["plogin"] = tbLogin.Text;
                nr["ppassword"] = tbPassword.Text;
                nr["plink"] = tbLink.Text;
                nr["psubscribers"] = cbPsubscribers.Checked;

                nr.EndEdit();

                if (row == null)
                {
                    data.Rows.Add(nr);
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
        }

    }
}
