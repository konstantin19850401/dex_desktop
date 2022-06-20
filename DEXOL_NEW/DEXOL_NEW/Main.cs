using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DEXOL_NEW
{
    public partial class Main : Form
    {
        public static Main mainForm = null;
        Toolbox tb;
        public Main()
        {
            InitializeComponent();
            tb = Toolbox.getToolBox();
            this.Text = String.Format("{0} Версия приложения: {1}\t Пользователь {2} {3} {4}",
                tb.AppName, tb.AppVer, tb.LastName, tb.FirstName, tb.SecondName);
            Main.mainForm = this;

            if (tb.AppsData.Count > 1)
            {
                AppsList appl = new AppsList();
                if (appl.ShowDialog(this) == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }
            } else
            {
                string appId = null;
                foreach (KeyValuePair<string, JObject> jo in tb.AppsData)
                {
                    appId = jo.Key.ToString();
                }
                JObject packet = new JObject();
                packet["com"] = "skyline.core.apps";
                packet["subcom"] = "select";
                packet["data"] = new JObject();
                packet["data"]["appid"] = appId;
                tb.comet.Get(packet);
            }
        }

        #region Работа с меню
        public void InitMenu(Dictionary<string, JArray> menu)
        {
            foreach (KeyValuePair<string, JArray> d in menu)
            {
                if (d.Key == "dicts")
                {
                    ToolStripMenuItem dicts = new ToolStripMenuItem();
                    dicts.Text = "Справочники";
                    foreach (JObject jo in d.Value)
                    {
                        ToolStripMenuItem t = new ToolStripMenuItem(jo["description"].ToString());
                        t.Tag = jo["name"].ToString();
                        t.Click += new EventHandler(OpenDictWindow);
                        dicts.DropDownItems.Add(t);
                        
                    }
                    menuStrip1.Items.AddRange(new ToolStripItem[] { dicts });
                }
                if (d.Key == "bases")
                {
                    ToolStripMenuItem bases = new ToolStripMenuItem();
                    bases.Text = "Базы данных";
                    foreach (JObject jo in d.Value)
                    {
                        ToolStripMenuItem t = new ToolStripMenuItem(jo["description"].ToString());
                        t.Tag = jo["id"].ToString();
                        t.Click += new EventHandler(OpenBaseWindow);
                        bases.DropDownItems.Add(t);
                    }
                    menuStrip1.Items.AddRange(new ToolStripItem[] { bases });
                }
            }
        }
        public void OpenDictWindow(object sender, EventArgs e)
        {
            string dictId = (string)((ToolStripMenuItem)sender).Tag;
            tb.handler.OpenDict(dictId);
        }
        public void OpenBaseWindow(object sender, EventArgs e)
        {
            string baseId = (string)((ToolStripMenuItem)sender).Tag;
            tb.handler.LaunchBase(baseId);
        }
        #endregion

        public void AddForm(Form form, JObject jo)
        {
            form.Show();
            AddOwnedForm(form);
            //DictForm df = new DictForm(jo);
            //df.Show();
            //AddOwnedForm(df);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
