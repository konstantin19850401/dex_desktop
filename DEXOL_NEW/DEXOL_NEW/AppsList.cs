using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using Newtonsoft.Json.Linq;

namespace DEXOL_NEW
{
    public partial class AppsList : Form
    {
        public AppsList()
        {
            InitializeComponent();
            Toolbox tb = Toolbox.getToolBox();
            gv_list.OptionsView.ShowGroupPanel = false;
            if (tb.AppsData.Count > 1)
            {
                // если юзеру доступно более одного приложения, предложить для выбора
                
                
                gv_list.Columns.AddVisible("id", "ID");
                gv_list.Columns.AddVisible("title", "Наименование");

                DataTable dtData = new DataTable();
                dtData.Columns.Add("id", typeof(string));
                dtData.Columns.Add("title", typeof(string));
                gv_list.Columns.Clear();
                gv_list.Columns.Add(createBGColumn("id", "id", "id", SummaryItemType.Sum, gv_list.Columns.Count));
                gv_list.Columns.Add(createBGColumn("title", "title", "Наименование", SummaryItemType.Sum, gv_list.Columns.Count));
                gv_list.Columns[0].Visible = false;

                foreach (KeyValuePair<string, JObject> jo in tb.AppsData)
                {
                    DataRow nr = dtData.NewRow();
                    nr["id"] = jo.Key.ToString();
                    nr["title"] = jo.Value["pseudo"];
                    dtData.Rows.Add(nr);
                }
                BindingSource bsData = new BindingSource();
                bsData.DataSource = dtData;
                gс_apps_list.DataSource = bsData;
            }
            else if (tb.AppsData.Count == 1)
            {
                string appId = null;
                foreach (KeyValuePair<string, JObject> jo in tb.AppsData)
                {
                    appId = jo.Key.ToString();
                }
                launch(appId);
            }
        }

        private void sb_cancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы уверены? Программа будет закрыта.", "Внимание!", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void gv_list_DoubleClick(object sender, EventArgs e)
        {
            int[] seld = gv_list.GetSelectedRows();
            DataRowView drv = (DataRowView) gv_list.GetRow(seld[0]);
            string appId = drv["id"].ToString();
            if (appId != null)
            {
                launch(appId);
            }
        }

        private async void launch(string appId)
        {
            JObject packet = new JObject();
            packet["com"] = "skyline.core.apps";
            packet["subcom"] = "select";
            packet["data"] = new JObject();
            packet["data"]["appid"] = appId;
            Toolbox tb = Toolbox.getToolBox();
            await tb.comet.Get(packet);

            DialogResult = DialogResult.OK;

            //Button button1 = new Button();
            //button1.DialogResult = DialogResult.OK;
            //Controls.Add(button1);
            //button1.PerformClick();
            //DialogResult = DialogResult.OK;
            //Close();

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

      
        
    }
}
