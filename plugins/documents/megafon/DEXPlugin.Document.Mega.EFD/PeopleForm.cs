using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Document.Mega.EFD
{
    public partial class PeopleForm : Form
    {
        ArrayList hashes;

        string ctemplate = "Документ:\nУдостоверение личности: FizDocType\n" +
            "Серия: FizDocSeries\nНомер: FizDocNumber\nКод подразделения: FizDocOrgCode\nКем выдан: FizDocOrg\nДата выдачи: FizDocDate\n" +
            "Адрес:\nОбласть: AddrState\nРайон: AddrRegion\nГород: AddrCity\nИндекс: AddrZip\n" +
            "Улица: AddrStreet\nДом: AddrHouse\nКорпус: AddrBuilding\nКвартира: AddrApartment";

        public StringList selectedItem;

        public PeopleForm(object toolbox, ArrayList ahashes)
        {
            InitializeComponent();
            hashes = ahashes;
            selectedItem = null;

            lbPeople.Items.Clear();
            
            IDEXPeopleSearcher ps = (IDEXPeopleSearcher)toolbox;
            
            foreach (string hash in hashes)
            {
                StringList pdata = ps.getPeopleData(hash);
                if (pdata != null)
                {
                    pdata["hash"] = hash;
                    string ptitle = string.Format("{0} {1} {2} ({3})",
                        pdata["firstname"], pdata["secondname"], pdata["lastname"], pdata["birth"]);
                    lbPeople.Items.Add(new StringObjTagItem(ptitle, pdata));
                }
            }

            lbPeople.SelectedIndex = 0;

            lbPeople.Focus();
        }

        private void lbPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtbData.Clear();
            try
            {
                Dictionary<string, string> dic = ((StringList)((StringObjTagItem)lbPeople.SelectedItem).Tag).getDictonary();
                string ctmp = ctemplate;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    //ctmp = ctmp.Replace(kvp.Key, kvp.Value);
                    if (kvp.Key == "FizDocOrg" && dic.ContainsKey("FizDocOrgCode"))
                    {
                        ctmp = ctmp.Replace("FizDocOrgCode", dic["FizDocOrgCode"].ToString());
                    }
                    ctmp = ctmp.Replace(kvp.Key, kvp.Value);
                }
                rtbData.Lines = ctmp.Split(new string[] { "\n" }, StringSplitOptions.None);
            }
            catch (Exception)
            {
            }
        }

        private void bSelect_Click(object sender, EventArgs e)
        {
            if (lbPeople.SelectedIndex > -1 && lbPeople.SelectedItem != null)
            {
                selectedItem = (StringList)((StringObjTagItem)lbPeople.SelectedItem).Tag;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
