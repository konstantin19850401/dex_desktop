using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DEXExtendLib;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class SelectFizDocOrg : Form
    {
        public SelectFizDocOrg(JArray ja)
        {
            InitializeComponent();

            for (int i = 0; i < ja.Count(); i++)
            {
                //cbOrgs.Items.Add();
                //tbFizDocOrg.Text = obj["data"]["list"][i]["title"].ToString();
                cbOrgs.Items.Add(new StringTagItem(ja[i]["title"].ToString(), ja[i]["code"].ToString()));
            }
            cbOrgs.DropDownWidth = comboBoxDropDownWidth(cbOrgs) + 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private int comboBoxDropDownWidth(ComboBox cb)
        {
            int newWidth = cb.DropDownWidth;
            int width = cb.DropDownWidth;
            System.Drawing.Font font = cb.Font;
            System.Drawing.Graphics g = cb.CreateGraphics();
            int vertScrollBarWidth = (cb.Items.Count > cb.MaxDropDownItems) ?
              System.Windows.Forms.SystemInformation.VerticalScrollBarWidth : 0;
            foreach (object item in (cb).Items)
            {
                string s = cb.GetItemText(item);
                newWidth = (int)g.MeasureString(s, font).Width + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }


            return width;
        }
    }
}
