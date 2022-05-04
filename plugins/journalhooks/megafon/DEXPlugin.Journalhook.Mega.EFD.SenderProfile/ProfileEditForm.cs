using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Journalhook.Mega.EFD.SenderProfile
{
    public partial class ProfileEditForm : Form
    {
        object toolbox;
        List<string> pcodes = new List<string>();

        public ProfileEditForm(object toolbox)
        {
            this.toolbox = toolbox;
            InitializeComponent();
            lbProfiles.Items.Clear();
            refresh();
        }

        void refresh()
        {
            string selectedTag = null;
            if (lbProfiles.SelectedItem != null && lbProfiles.SelectedItem is StringTagItem)
            {
                selectedTag = ((StringTagItem)lbProfiles.SelectedItem).Tag;
            }

            lbProfiles.Items.Clear();
            pcodes.Clear();

            DataTable t = ((IDEXData)toolbox).getTable("efd_profiles");

            if (t != null && t.Rows.Count > 0)
            {
                int sel = -1;
                foreach (DataRow r in t.Rows)
                {
                    int cid = lbProfiles.Items.Add(new StringTagItem(r["pname"].ToString(), r["pcode"].ToString()));
                    if (r["pcode"].ToString().Equals(selectedTag)) sel = cid;
                    pcodes.Add(r["pcode"].ToString());
                }

                lbProfiles.SelectedIndex = sel;
            }

        }

        private void bNew_Click(object sender, EventArgs e)
        {
            ProfileEditEd pee = new ProfileEditEd("", "", pcodes);
            if (pee.ShowDialog() == DialogResult.OK)
            {
                IDEXData d = (IDEXData)toolbox;
                d.runQuery("insert into `efd_profiles` (pname, pcode) values ('{0}', '{1}')", d.EscapeString(pee.tbPname.Text), d.EscapeString(pee.tbPcode.Text));
                refresh();
            }
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedItem != null && lbProfiles.SelectedItem is StringTagItem)
            {
                StringTagItem ste = (StringTagItem)lbProfiles.SelectedItem;
                ProfileEditEd pee = new ProfileEditEd(ste.Text, ste.Tag, pcodes);
                if (pee.ShowDialog() == DialogResult.OK)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery("update `efd_profiles` set pname = '{0}', pcode = '{1}' where pcode = '{2}'", 
                        d.EscapeString(pee.tbPname.Text), d.EscapeString(pee.tbPcode.Text), d.EscapeString(ste.Tag));
                    refresh();
                }
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedItem != null && lbProfiles.SelectedItem is StringTagItem)
            {
                StringTagItem ste = (StringTagItem)lbProfiles.SelectedItem;
                if (MessageBox.Show(string.Format("Удалить запись '{0}'?", ste.Text), "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery("delete from `efd_profiles` where pcode = '{0}'", d.EscapeString(ste.Tag));
                    refresh();
                }
            }
        }
    }
}
