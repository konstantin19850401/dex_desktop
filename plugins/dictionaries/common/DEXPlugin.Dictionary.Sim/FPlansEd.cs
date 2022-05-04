using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Sim
{
    public partial class FPlansEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public FPlansEd()
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
                tbPlan_id.Text = "";
                tbTitle.Text = "";
                tbPlan_id.Enabled = true;
            }
            else
            {
                tbPlan_id.Text = row["plan_id"].ToString();
                tbTitle.Text = row["title"].ToString();
                
                IDEXData d = (IDEXData)toolbox;

                tbPlan_id.Enabled = (d.getDataReference("plans", tbPlan_id.Text) < 1);
            }

            if (tbPlan_id.Enabled) tbPlan_id.Focus();
            else tbTitle.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            if (tbPlan_id.Text.Trim().Length < 1)
            {
                er += "* Не указан идентификатор тарифного плана\n";
            }

            if (tbTitle.Text.Trim().Length < 1)
            {
                er += "* Не указано наименование тарифного плана\n";
            }

            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                    nr.BeginEdit();
                    nr["plan_id"] = tbPlan_id.Text;
                    nr["title"] = tbTitle.Text;
                }
                else
                {
                    nr.BeginEdit();
                    if (tbPlan_id.Enabled) nr["plan_id"] = tbPlan_id.Text;
                    nr["title"] = tbTitle.Text;
                }

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
