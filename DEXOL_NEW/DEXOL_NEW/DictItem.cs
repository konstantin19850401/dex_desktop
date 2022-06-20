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
using DevExpress.XtraEditors;
using System.IO;
using System.Collections;

namespace DEXOL_NEW
{
    public partial class DictItem : Form
    {
        string hash = null;
        JArray schemas;
        ArrayList fieldslist = new ArrayList();
        bool edit = false;
        string dictname;
        public DictItem(string dictname, JObject data, JArray schemas)
        {
            InitializeComponent();
            Toolbox tb = Toolbox.getToolBox();
            hash = tb.GenerateHash();
            tb.handler.AddWind(hash, this);
            this.schemas = schemas;
            this.dictname = dictname;
            fillTable(data);
        }

        private void fillTable(JObject data)
        {
            int cnt = 1;
            foreach(JObject jo in schemas)
            {
                // label
                Label lb = new Label();
                lb.Text = jo["title"].ToString();
                lb.Visible = true;
                lb.AutoSize = true;
                lb.Location = new Point(12, 30 * cnt);
                lb.Name = "lb_"+jo["name"];
                lb.Refresh();
                this.Controls.Add(lb);
                

                // text
                if (jo["foreignKey"] == null)
                {
                    TextEdit te = new TextEdit();
                    te.Name = "te#"+ jo["name"];
                    te.Width = 520;
                    te.Left = this.Width - 550;
                    te.Top = 30 * cnt;
                    te.Anchor = AnchorStyles.Right;
                    this.Controls.Add(te);
                    fieldslist.Add("te#" + jo["name"]);
                } else
                {
                    string[] str = jo["foreignKey"].ToString().Split('.');
                    Dictionary<string, JArray> dicts = Toolbox.getToolBox().Dictionaries();
                    foreach(KeyValuePair<string, JArray> kvp in dicts)
                    {
                        if (kvp.Key == str[0])
                        {
                            System.Windows.Forms.ComboBox cb = new System.Windows.Forms.ComboBox();
                            cb.Name = "cb#"+jo["name"];
                            cb.Width = 520;
                            cb.Left = this.Width - 550;
                            cb.Top = 30 * cnt;
                            cb.Anchor = AnchorStyles.Right;
                            cb.DropDownStyle = ComboBoxStyle.DropDownList;
                            fieldslist.Add("cb#" + jo["name"]);
                            //наполнить!
                            Dictionary<int, string> comboBoxSource = new Dictionary<int, string>(kvp.Value.Count);
                            foreach (JObject jao in kvp.Value)
                            {
                                comboBoxSource.Add((int)jao["id"], jao["title"].ToString());
                                cb.DataSource = new BindingSource(comboBoxSource, null);
                                cb.DisplayMember = "Value";
                                cb.ValueMember = "Key";
                            }

                            this.Controls.Add(cb);
                            break;
                        }
                    }
                }
                cnt++;
            }

            if (data != null) edit = true;
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

        public void Cmd(JObject jo)
        {
            Console.WriteLine("пришел!!! ", jo);
        }

        private void DictItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            Toolbox tb = Toolbox.getToolBox();
            tb.handler.DeleteWindByHash(hash);
        }

        private void sb_ok_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            foreach(string field in fieldslist)
            {
                string[] arr = field.Split('#');
                string name = arr[1];
                if (arr[0].Equals("te")) {
                    TextEdit te = Controls[field] as TextEdit;
                    fields.Add(name, te.Text);
                } else if (arr[0].Equals("cb"))
                {
                    System.Windows.Forms.ComboBox cb = Controls[field] as System.Windows.Forms.ComboBox;
                    string ss = cb.ValueMember;

                }
            }

            Toolbox tb = Toolbox.getToolBox();
            JObject jo = new JObject();
            jo["com"] = "skyline.apps." + tb.AppId;
            jo["subcom"] = "appApi";
            jo["data"] = new JObject();
            jo["data"]["fields"] = new JObject();
            if (!edit)
            {
                jo["data"]["action"] = "createNewRecordInDictV1";
                foreach (KeyValuePair<string, string> kvp in fields)
                {
                    jo["data"]["fields"][kvp.Key] = kvp.Value;
                }
            }
            else
            {

            }
            jo["data"]["dict"] = dictname;
            jo["hash"] = hash;

            tb.comet.Get(jo);
        }
    }
}
