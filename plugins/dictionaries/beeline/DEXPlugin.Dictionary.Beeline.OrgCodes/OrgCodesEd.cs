using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Beeline.OrgCodes
{
    public partial class OrgCodesEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public OrgCodesEd()
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
                tbCode.Text = "";
                tbTitle.Text = "";
            }
            else
            {
                tbCode.Text = row["code"].ToString();
                tbTitle.Text = row["title"].ToString();
            }

            tbCode.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            string clCode = tbCode.Text.Trim();
            string clTitle = tbTitle.Text.Trim();

            if (clCode.Length < 1)
            {
                er += "* Не указан код подразделения\n";
            }

            if (clTitle.Length < 1)
            {
                er += "* Не указано наименование подразделения\n";
            }

            if (er == "")
            {
                DataRow[] r = data.Select("code = '" + clCode.Replace("'", "\'") + "'");
                /*
                if (r != null && r.Length > 0 && r[0] != row)
                {
                    er += "* Населенный пункт с таким индексом уже существует\n";
                }
                */
            }
            
            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                }

                nr.BeginEdit();

                nr["code"] = tbCode.Text;
                nr["title"] = tbTitle.Text;

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
