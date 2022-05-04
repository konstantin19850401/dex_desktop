using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Units
{
    public partial class FUnitsEd : Form
    {
        
        DataTable data;
        DataRow row;
        Object toolbox;

        public FUnitsEd()
        {
            InitializeComponent();
        }

        public void InitForm(Object AToolBox, DataTable AData, DataRow ARow)
        {
            toolbox = AToolBox;
            data = AData;
            row = ARow;

            string[] ss = ((IDEXSysData)toolbox).DocumentStatesText;
            cbDocumentState.Items.Clear();
            foreach (string ssi in ss)
            {
                cbDocumentState.Items.Add(ssi);
            }

            if (row == null)
            {
                nudUID.Value = 0;
                tbForeignId.Text = "";
                tbTitle.Text = "";
                tbDesc.Text = "";
                tbRegion.Text = "";
                cbStatus.Checked = true;
                cbDocumentState.SelectedIndex = 0;
                nudUID.Enabled = true;
            }
            else
            {
                nudUID.Value = int.Parse(row["uid"].ToString());
                tbForeignId.Text = row["foreign_id"].ToString();
                tbTitle.Text = row["title"].ToString();
                tbDesc.Text = row["desc"].ToString();
                tbRegion.Text = row["region"].ToString();
                cbStatus.Checked = bool.Parse(row["status"].ToString());
                try
                {
                    cbDocumentState.SelectedIndex = int.Parse(row["documentstate"].ToString());
                }
                catch (Exception)
                {
                    cbDocumentState.SelectedIndex = 0;
                }

                IDEXData d = (IDEXData)toolbox;
                nudUID.Enabled = (d.getDataReference("units", row["uid"].ToString()) < 1);
            }

            
            nudUID.Focus();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            string er = "";

            string clTitle = tbTitle.Text.Trim();

            if (clTitle.Length < 1)
            {
                er += "* Не указано наименование отделения\n";
            }

            
            string region = tbRegion.Text.Trim();
            /*
            if (region.Length < 1)
            {
                er += "* Не указан регион отделения\n";
            }
            */

            if (nudUID.Value < 1)
            {
                er += "* Указан некорректный ID отделения\n";
            }

            if (cbDocumentState.SelectedIndex < 0)
            {
                er += "* Не указан статус импортируемого документа\n";
            }
            
            if (er == "")
            {
                if (data != null)
                {
                    DataRow[] r = data.Select("title = '" + clTitle.Replace("'", "\'") + "'");
                    if (r != null && r.Length > 0 && r[0] != row)
                    {
                        er += "* Отделение с таким наименованием уже существует\n";
                    }
                }
            }

            if (er == "")
            {
                if (data != null)
                {
                    DataRow[] r = data.Select("uid = " + nudUID.Value.ToString());
                    if (r != null && r.Length > 0 && r[0] != row)
                    {
                        er += "* Отделение с таким ID уже существует\n";
                    }
                }
            }
            
            if (er == "")
            {
                IDEXData d = (IDEXData)toolbox;
                if (row == null)
                {
                    d.runQuery(
                        "insert into `units` (uid, foreign_id, title, `units`.desc, status, documentstate, data, region) values ({0}, '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}')",
                        nudUID.Value.ToString(), 
                        d.EscapeString(tbForeignId.Text), 
                        d.EscapeString(tbTitle.Text),
                        d.EscapeString(tbDesc.Text),
                        cbStatus.Checked ? 1 : 0, 
                        cbDocumentState.SelectedIndex,
                        "",
                        d.EscapeString(tbRegion.Text)
                        );
                }
                else
                {
                    d.runQuery(
                        "update `units` set uid = {0}, foreign_id = '{1}', title = '{2}', `units`.desc = '{3}', status = {4}, documentstate = {5}, region = '{6}' where id = {7}",
                        nudUID.Value.ToString(), d.EscapeString(tbForeignId.Text), d.EscapeString(tbTitle.Text), d.EscapeString(tbDesc.Text),
                        cbStatus.Checked ? 1 : 0, cbDocumentState.SelectedIndex, d.EscapeString(tbRegion.Text), row["id"].ToString() 
                        );
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
