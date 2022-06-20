using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.BandedGrid;


namespace DEXOL_NEW
{
    public partial class DictForm : Form
    {
        string dictname = null;
        string hash = null;
        JArray schemas = null;
        public DictForm(JObject data)
        {
            InitializeComponent();
            Toolbox tb = Toolbox.getToolBox();
            hash = tb.GenerateHash();
            this.Text = data["list"][0]["dictTitle"].ToString();
            this.dictname = data["list"][0]["dictName"].ToString();
            tb.handler.AddWind(hash, this);
            fillTable(data);
        }

        private void fillTable(JObject data)
        {
            gv_table.Columns.AddVisible("id", "ID");
            gv_table.Columns.AddVisible("title", "Наименование");
            gv_table.Columns.AddVisible("status", "Статус");

            DataTable dtData = new DataTable();
            dtData.Columns.Add("id", typeof(string));
            dtData.Columns.Add("title", typeof(string));
            dtData.Columns.Add("status", typeof(string));
            gv_table.Columns.Clear();
            gv_table.Columns.Add(createBGColumn("id", "id", "ID", SummaryItemType.Sum, gv_table.Columns.Count));
            gv_table.Columns.Add(createBGColumn("title", "title", "Наименование", SummaryItemType.Sum, gv_table.Columns.Count));
            gv_table.Columns.Add(createBGColumn("status", "status", "Статус", SummaryItemType.Sum, gv_table.Columns.Count));
            gv_table.Columns[0].Width = 5;
            foreach (JObject joDict in (JArray)data["list"])
            {
                if (joDict["dictName"].ToString().Equals(this.dictname))
                {
                    schemas = (JArray)joDict["schema"];
                    foreach (JObject jo in joDict["list"])
                    {
                        DataRow nr = dtData.NewRow();
                        nr["id"] = jo["uid"].ToString();
                        nr["title"] = jo["title"].ToString();
                        if (jo["status"] != null) nr["status"] = jo["status"].ToString();
                        else nr["status"] = "";
                        dtData.Rows.Add(nr);
                    }
                }
            }
            BindingSource bsData = new BindingSource();
            bsData.DataSource = dtData;
            gs_table.DataSource = bsData;
        }

        private void sb_addItem_Click(object sender, EventArgs e)
        {
            DictItem di = new DictItem(dictname, null, schemas);
            di.ShowDialog();
            AddOwnedForm(di);
        }

        BandedGridColumn createBGColumn(string colName, string fieldName, string colCaption, SummaryItemType sit, int cnt)
        {
            BandedGridColumn ret = new BandedGridColumn();
            ret.Name = colName;
            ret.FieldName = fieldName;
            ret.Caption = colCaption;
            ret.SummaryItem.SummaryType = sit;
            ret.Visible = true;
            ret.VisibleIndex = cnt;
            return ret;
        }

        private void gs_table_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Toolbox tb = Toolbox.getToolBox();
                int[] seld = gv_table.GetSelectedRows();
                DataRowView drv = (DataRowView)gv_table.GetRow(seld[0]);
                string recordId = drv["id"].ToString();
                GetDictSingleId(recordId);
            } catch (Exception ex)
            {
                MessageBox.Show("Ошибка! " + ex);
            }
        }

        private async Task GetDictSingleId(string id)
        {
            Toolbox tb = Toolbox.getToolBox();
            JObject jo = new JObject();
            jo["com"] = "skyline.apps." + tb.AppId;
            jo["subcom"] = "appApi";
            jo["data"] = new JObject();
            jo["data"]["action"] = "getDictSingleIdV1";
            jo["data"]["id"] = id;
            jo["data"]["dict"] = dictname;
            jo["hash"] = hash;

            tb.comet.Get(jo);
        }

        private void DictForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Toolbox tb = Toolbox.getToolBox();
            tb.handler.DeleteWindByHash(hash);
        }

        public void Cmd(JObject jo)
        {
            Console.WriteLine("пришел!!! ", jo.ToString());
            switch (jo["action"].ToString())
            {
                case "getDictSingleId":
                    //JObject j = (JArray)jo["list"];
                    DictItem di = new DictItem(dictname, jo, schemas);
                    di.ShowDialog();
                    AddOwnedForm(di);
                    break;
            }
        }
    }
}
