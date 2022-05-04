using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using DEXExtendLib;
using System.IO;
using System.Data.Common;

namespace Kassa3
{
    public partial class ReclogForm : Form
    {
        Tools tools = Tools.instance;
        FormState fs;
        bool needColWidthsUpdate = false;

        Dictionary<int, string> dUsers = new Dictionary<int, string>();
        Dictionary<string, int> dColWidths = new Dictionary<string, int>();

        public ReclogForm()
        {
            InitializeComponent();
            dgv.AutoGenerateColumns = false;

            // Выборка из журнала пользователей
            try
            {
                dUsers.Clear();
                cbUser.Items.Clear();
                cbUser.Items.Add(new StringObjTagItem("Любой пользователь", -1));

//                DataTable dtu = tools.MySqlFillTable(new MySqlCommand("select id, login from users", tools.connection));

                using (DataTable dtu = Db.fillTable(Db.command("select id, login from users")))
                {
                    foreach (DataRow dr in dtu.Rows)
                    {
                        cbUser.Items.Add(new StringObjTagItem(dr["login"].ToString(), Convert.ToInt32(dr["id"])));
                        dUsers[Convert.ToInt32(dr["id"])] = dr["login"].ToString();
                    }
                }
            }
            catch (Exception) { }

            fs = new FormState(tools.dataDir + @"\ReclogForm.fs");
            
            try
            {
                StringObjTagItem.SelectByTag(cbUser, Convert.ToInt32(fs.getValue("cbUser", "-1")), true);
            }
            catch (Exception) { }

            try
            {
                cbKind.SelectedIndex = Convert.ToInt32(fs.getValue("cbKind", "0"));
            }
            catch (Exception) { }

            fs.LoadValue("deStart", deStart);
            fs.LoadValue("deEnd", deEnd);

            try
            {
                dColWidths.Clear();
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(tools.dataDir + @"\ReclogForm.dgv"));
                foreach(SimpleXML c in xml.Child) {
                    try
                    {
                        dColWidths[c.Attributes["name"]] = Convert.ToInt32(c.Attributes["width"]);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            
        }

        private void ReclogForm_Load(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
        }

        private void ReclogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.SaveValue("deStart", deStart);
            fs.SaveValue("deEnd", deEnd);
            fs.SaveValue("cbUser", cbUser.SelectedItem is StringObjTagItem ? ((StringObjTagItem)cbUser.SelectedItem).Tag : "-1");
            fs.SaveValue("cbKind", cbKind.SelectedIndex);
            fs.UpdateFromForm(this);
            fs.SaveToFile(tools.dataDir + @"\ReclogForm.fs");

            SimpleXML xml = new SimpleXML("dgv");

            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                SimpleXML col = xml.CreateChild("Column");
                col.Attributes["name"] = dgvc.Name;
                col.Attributes["width"] = "" + dgvc.Width;
            }

            File.WriteAllText(tools.dataDir + @"\ReclogForm.dgv", SimpleXML.SaveXml(xml));

            MainForm.mainForm.MenuUnregisterWindow(this);
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            String dpn = dgv.Columns[e.ColumnIndex].DataPropertyName;

            if ("op_kind".Equals(dpn))
            {
                int v = Convert.ToInt32(e.Value);
                if (v == 0) e.Value = "Новая";
                else e.Value = "Изменение";
            }
            else if ("op_user".Equals(dpn))
            {
                int v = Convert.ToInt32(e.Value);
                if (dUsers.ContainsKey(v)) e.Value = dUsers[v];
            }
            else if ("op_info".Equals(dpn))
            {
                string s = (string)e.Value;
                int sindex = s.IndexOf("\n");
                if (sindex >= 0)
                {
                    e.Value = s.Substring(0, sindex);
                }
            }
            else if ("op_date".Equals(dpn))
            {
                string s = (string)e.Value;
                if (s != null && s.Length >= 8)
                {
                    e.Value = s.Substring(6, 2) + "." + s.Substring(4, 2) + "." + s.Substring(0, 4);
                }
            }
        }

        private void bShow_Click(object sender, EventArgs e)
        {
            if (needColWidthsUpdate)
            {
                foreach (DataGridViewColumn dgvc in dgv.Columns)
                {
                    dColWidths[dgvc.Name] = dgvc.Width;
                }
            }

            StringObjTagItem sotiUser = (StringObjTagItem)cbUser.SelectedItem;

            string sql = "select * from `reclog` where op_date >= @destart and op_date <= @deend";
            if (sotiUser != null && Convert.ToInt32(sotiUser.Tag) > -1) sql += " and op_user = @op_user";
            if (cbKind.SelectedIndex > 0) sql += " and op_kind = @op_kind";

            sql += " order by id";

            /*
            MySqlCommand cmd = new MySqlCommand(sql, tools.connection);
            tools.SetDbParameter(cmd, "destart", deStart.Value.ToString("yyyyMMdd"));
            tools.SetDbParameter(cmd, "deend", deEnd.Value.ToString("yyyyMMdd"));
            if (sotiUser != null && Convert.ToInt32(sotiUser.Tag) > -1) tools.SetDbParameter(cmd, "op_user", Convert.ToInt32(sotiUser.Tag));
            if (cbKind.SelectedIndex > 0) tools.SetDbParameter(cmd, "op_kind", cbKind.SelectedIndex - 1);

            DataTable dt = tools.MySqlFillTable(cmd);
             */

            DbCommand cmd = Db.command(sql);
            Db.param(cmd, "destart", deStart.Value.ToString("yyyyMMdd"));
            Db.param(cmd, "deend", deEnd.Value.ToString("yyyyMMdd"));
            if (sotiUser != null && Convert.ToInt32(sotiUser.Tag) > -1) Db.param(cmd, "op_user", Convert.ToInt32(sotiUser.Tag));
            if (cbKind.SelectedIndex > 0) Db.param(cmd, "op_kind", cbKind.SelectedIndex - 1);

            DataTable dt = Db.fillTable(cmd);

            dgv.DataSource = dt;

            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                if (dColWidths.ContainsKey(dgvc.Name)) dgvc.Width = dColWidths[dgvc.Name];
            }

            dgv_RowEnter(null, null);

            needColWidthsUpdate = dt != null && dt.Rows.Count > 0;
        }

        private void dgv_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    DataGridViewRow dgvr = dgv.Rows[dgv.SelectedRows[0].Index];
                    tbInfo.Lines = ((string)dgvr.Cells["op_info"].Value).Split(new char[] { '\n' });
                }
            }
            catch (Exception) { }
        }


    }
}
