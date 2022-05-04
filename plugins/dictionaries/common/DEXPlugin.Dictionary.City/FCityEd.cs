using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.City
{
    public partial class FCityEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public FCityEd()
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
                tbZip.Text = "";
                tbCity.Text = "";
                tbRegion.Text = "";
            }
            else
            {
                tbZip.Text = row["zip"].ToString();
                tbCity.Text = row["city"].ToString();
                tbRegion.Text = row["region"].ToString();
            }

            tbZip.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            string clZip = tbZip.Text.Trim();
            string clCity = tbCity.Text.Trim();
            string clRegion = tbRegion.Text.Trim();

            if (clZip.Length < 1)
            {
                er += "* Не указан индекс населенного пункта\n";
            }

            if (clCity.Length < 1)
            {
                er += "* Не указано наименование населенного пункта\n";
            }

            if (clRegion.Length < 1)
            {
                er += "* Не указан район населенного пункта\n";
            }

            if (er == "")
            {
                DataRow[] r = data.Select("zip = '" + clZip.Replace("'", "\'") + "'");
                if (r != null && r.Length > 0 && r[0] != row)
                {
                    er += "* Населенный пункт с таким индексом уже существует\n";
                }
            }

            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                }

                nr.BeginEdit();

                nr["zip"] = tbZip.Text;
                nr["city"] = tbCity.Text;
                nr["region"] = tbRegion.Text;

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
