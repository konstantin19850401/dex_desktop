using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Regions
{
    public partial class FRegionsEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public FRegionsEd()
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
                tbRegion_id.Text = "";
                tbTitle.Text = "";
                tbRegion_id.Enabled = true;
            }
            else
            {
                tbRegion_id.Text = row["region_id"].ToString();
                tbTitle.Text = row["title"].ToString();

                IDEXData d = (IDEXData)toolbox;
                tbRegion_id.Enabled = (d.getDataReference("regions", row["region_id"].ToString()) < 1);
            }

            tbRegion_id.Focus();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            string er = "";

            string clTitle = tbTitle.Text.Trim();
            string clRegion_id = tbRegion_id.Text.Trim();

            if (clTitle.Length < 1)
            {
                er += "* Не указано наименование региона\n";
            }

            if (clRegion_id.Length < 1)
            {
                er += "* Указан некорректный ID региона\n";
            }

            if (er == "")
            {
                DataRow[] r = data.Select("title = '" + clTitle.Replace("'", "\'") + "'");
                if (r != null && r.Length > 0 && r[0] != row)
                {
                    er += "* Регион с таким наименованием уже существует\n";
                }
            }

            if (er == "")
            {
                DataRow[] r = data.Select("region_id = '" + clRegion_id.Replace("'", "\'") + "'");
                if (r != null && r.Length > 0 && r[0] != row)
                {
                    er += "* Регион с таким идентификатором уже существует\n";
                }
            }

            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                    nr.BeginEdit();
                }
                else
                {
                    nr.BeginEdit();
                }

                nr["region_id"] = tbRegion_id.Text;
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
