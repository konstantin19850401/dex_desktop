using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXOffice
{
    public partial class DataFieldsForm : Form
    {
        public Dictionary<string, string> allFields, selFields;

        public DataFieldsForm(Dictionary<string, string> aAll, Dictionary<string, string> aSel)
        {
            InitializeComponent();

            allFields = aAll;
            selFields = aSel;

            lbAll.Items.Clear();
            lbSel.Items.Clear();

            foreach (KeyValuePair<string, string> kvp in allFields)
            {
                if (!selFields.ContainsKey(kvp.Key))
                {
                    lbAll.Items.Add(new TagItem(kvp.Value, kvp.Key));
                }
            }
            foreach(KeyValuePair<string, string> kvp in selFields)
            {
                lbSel.Items.Add(new TagItem(kvp.Value, kvp.Key));
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (lbAll.SelectedItems.Count > 0)
            {
                ArrayList soc = new ArrayList(lbAll.SelectedItems);

                foreach (Object i in soc)
                {
                    lbSel.Items.Add(i);
                    lbAll.Items.Remove(i);
                }
            }
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (lbSel.SelectedItems.Count > 0)
            {
                ArrayList soc = new ArrayList(lbSel.SelectedItems);

                foreach (Object i in soc)
                {
                    lbAll.Items.Add(i);
                    lbSel.Items.Remove(i);
                }
            }
        }

        private void bUp_Click(object sender, EventArgs e)
        {
            if (lbSel.SelectedIndices.Count > 0)
            {
                ArrayList sic = new ArrayList(lbSel.SelectedIndices);
                foreach (int i in sic)
                {
                    if (i - 1 < 0) return;
                }

                foreach (int i in sic)
                {
                    Object o = lbSel.Items[i - 1];
                    lbSel.Items[i - 1] = lbSel.Items[i];
                    lbSel.Items[i] = o;
                }

                lbSel.SelectedIndices.Clear();
                foreach (int i in sic)
                {
                    int i2 = i - 1;
                    lbSel.SelectedIndices.Add(i2);
                }
            }
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            if (lbSel.SelectedIndices.Count > 0)
            {
                ArrayList sic = new ArrayList(lbSel.SelectedIndices);
                sic.Reverse();
                
                foreach (int i in sic)
                {
                    if (i + 1 >= lbSel.Items.Count) return;
                }

                foreach (int i in sic)
                {
                    Object o = lbSel.Items[i + 1];
                    lbSel.Items[i + 1] = lbSel.Items[i];
                    lbSel.Items[i] = o;
                }

                lbSel.SelectedIndices.Clear();
                foreach (int i in sic)
                {
                    int i2 = i + 1;
                    lbSel.SelectedIndices.Add(i2);
                }
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            selFields.Clear();
            foreach (TagItem i in lbSel.Items)
            {
                selFields.Add(i.Tag, i.Text);
            }

            DialogResult = DialogResult.OK;
        }
    }

    class TagItem
    {
        public string Tag;
        public string Text;

        public TagItem(string text, string tag)
        {
            Tag = tag;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }

}
