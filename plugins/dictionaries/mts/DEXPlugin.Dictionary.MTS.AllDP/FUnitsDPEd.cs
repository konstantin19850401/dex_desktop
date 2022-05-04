using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.MTS.AllDP
{
    public partial class FUnitsDPEd : Form
    {
        DataRow row;
        Object toolbox;

        public FUnitsDPEd(Object AToolBox, DataRow ARow)
        {
            InitializeComponent();

            toolbox = AToolBox;
            row = ARow;

            //DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            //StringTagItem.UpdateCombo(cbFilterUnit, t, "Любое", "uid", "title", false);

            if (row == null)
            {
                cbKind.Checked = false;
                tbDPCode.Text = "";
            }
            else
            {
                //StringTagItem.SelectByTag(cbFilterUnit, row["uid"].ToString(), true);
                cbKind.Checked = Convert.ToInt32(row["kind"]) == 1;
                tbDPCode.Text = row["dpcode"].ToString();
            }

        }

        private void bOK_Click(object sender, EventArgs e)
        {
            IDEXData d = (IDEXData)toolbox;
            string er = "";

            /*
            if (cbFilterUnit.SelectedItem == null || cbFilterUnit.SelectedItem == StringTagItem.VALUE_ANY)
            {
                er += "* Не выбрано отделение\n";
            }
            */

            if (tbDPCode.Text.Trim() == "") er += "* Не указан код точки продаж\n";

            if (er == "")
            {
                //string uid = ((StringTagItem)cbFilterUnit.SelectedItem).Tag;
                int kind = cbKind.Checked ? 1 : 0;


                string sql =
                    "select count(id) as cid from `mts_dp_all` where dpcode = '" +
                    d.EscapeString(tbDPCode.Text) + "'";

                if (row != null) sql += " and id <> " + row["id"].ToString();

                DataTable dt = d.getQuery(sql);

                if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cid"]) > 0)
                {
                    er += "* Такая запись уже есть в базе данных\n";
                }
            }

            if (er == "")
            {
                //string uid = ((StringTagItem)cbFilterUnit.SelectedItem).Tag;
                string dpcode = d.EscapeString(tbDPCode.Text.Trim());
                int kind = cbKind.Checked ? 1 : 0;

                if (row == null)
                {
                    d.runQuery("insert into `mts_dp_all` (dpcode, kind) values ('" + dpcode + "', " + kind + ")");
                }
                else
                {
                    d.runQuery("update `mts_dp_all` set dpcode = '" + dpcode + "', kind = " + kind + " where id = " + row["id"].ToString());
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
