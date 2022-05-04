using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using DEXExtendLib;
using DEXSIM;

namespace DEXPlugin.Document.Mega.EAD
{
    public partial class DocumentForm : Form
    {
        public Object toolbox;
        public Document module;
        IDEXSim sim;

        IDEXDocumentData fsource, fdocument;
        bool ReadOnly;
        string region_id = "";

        string profileName, profileLogin, profilePassword, profileSubscribers; // Поля для DEXPlugin.Journalhook.Mega.SenderProfile 

        bool isOnline;

        public DocumentForm()
        {
            InitializeComponent();
        }

        public void InitDictionaries()
        {
            InitComboCity(tbDocCity, "city");

            DataTable tPlan = ((IDEXData)toolbox).getTable("um_plans");

            StringTagItem.UpdateCombo(cbPlan, tPlan, null, "plan_id", "title", false);
            cbPlan.Sorted = true;

//            InitComboDictionary(tbSecondName, "second_name"); //
//            InitComboDictionary(tbFirstName, "first_name"); //
//            InitComboDictionary(tbLastName, "last_name"); //
            InitComboDictionary(tbFizDocOrg, "fiz_doc_org"); //
            InitComboDictionary(tbFizDocType, "fiz_doc_type"); //
            InitComboDictionary(tbAddrStreet, "street"); //
            InitComboCity(tbAddrRegion, "region");
            InitComboDictionary(tbAddrState, "state");
            InitComboCity(tbAddrCity, "city");
        }

        public void InitDocument(IDEXDocumentData source, IDEXDocumentData document, bool clone, bool ReadOnly)
        {
            isOnline = ((IDEXUserData)toolbox).isOnline;
            this.ReadOnly = ReadOnly;
            bOk.Visible = !ReadOnly;
            IDEXServices srv = (IDEXServices)toolbox;
            sim = (IDEXSim)srv.GetService("sim");

            fsource = source;
            fdocument = document;

            lDocUnit.Visible = !isOnline;
            cbDocUnit.Visible = !isOnline;

            if (!isOnline)
            {
                DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");
                StringTagItem.UpdateCombo(cbDocUnit, t, null, "uid", "title", false);

                cbDocUnit.Text = "";
            }

            cbDocCategory.SelectedIndex = 0;

            deDocDate.Value = DateTime.Now;
            tbDocNum.Text = "";

            mtbMSISDN.Text = "";
            mtbMSISDN.Mask = ((IDEXConfig)toolbox).getRegisterStr("msisdn_mask", "0000000000");

            mtbICC.Text = "";
            mtbICC.Mask = ((IDEXConfig)toolbox).getRegisterStr("icc_mask", "00000000000000000");
            tbICCCTL.Text = "0";
            deBirth.Text = "";

            InitDictionaries();

            region_id = "";

            deFizDocDate.Text = "";
            cbFizDocScan.Checked = false;
            tbFizDocNumber.Text = "";
            tbFizDocSeries.Text = "";
            tbAddrZip.Text = "";
            tbAddrApartment.Text = "";
            tbAddrBuilding.Text = "";
            tbAddrHouse.Text = "";
            lOwner.Text = "";

            profileName = "";
            profileLogin = "";
            profilePassword = "";
            profileSubscribers = "";

            LoadDefaultsToControls();

//            cbDocStatus.SelectedIndex = ((IDEXConfig)toolbox).getInt(module.ID, "cbDocStatus", 0);
            IDEXUserData dud = (IDEXUserData)toolbox;
            SimpleXML props = dud.UserProperties;
            try
            {
                cbDocStatus.SelectedIndex = int.Parse(props["DefaultDocumentState"].Text);
            }
            catch(Exception)
            {
                cbDocStatus.SelectedIndex = 0;
            }

            cbDocStatus.Enabled = !isOnline;

            fdocument.documentDate = DateTime.Now.ToString("yyyyMMddhhmmssfff");

            if (fsource != null)
            {
                try
                {
                    SimpleXML xml = SimpleXML.LoadXml(fsource.documentText);

                    if (!isOnline)
                    {
                        StringTagItem.SelectByTag(cbDocUnit, fsource.documentUnitID.ToString(), true);
                    }

                    try
                    {
                        cbDocCategory.SelectedIndex = int.Parse(xml.GetNodeByPath("DocCategory", true).Text);
                    }
                    catch (Exception) { }

                    tbDocCity.Text = xml.GetNodeByPath("DocCity", true).Text;
                    deDocDate.Text = xml.GetNodeByPath("DocDate", true).Text;
                    tbDocNum.Text = xml.GetNodeByPath("DocNum", true).Text;

                    StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, true);
                    region_id = xml.GetNodeByPath("PlanRegionId", true).Text;

                    tbICCCTL.Text = xml.GetNodeByPath("ICCCTL", true).Text;
                    mtbICC.Text = xml.GetNodeByPath("ICC", true).Text;
                    mtbMSISDN.Text = xml.GetNodeByPath("MSISDN", true).Text;
                    deBirth.Text = xml.GetNodeByPath("Birth", true).Text;
                    tbFirstName.Text = xml.GetNodeByPath("FirstName", true).Text;
                    tbSecondName.Text = xml.GetNodeByPath("SecondName", true).Text;
                    tbLastName.Text = xml.GetNodeByPath("LastName", true).Text;
                    deFizDocDate.Text = xml.GetNodeByPath("FizDocDate", true).Text;

                    cbFizDocScan.Checked = xml.GetNodeByPath("FizDocScan", true).Text.Equals("X");
                    tbFizDocOrg.Text = xml.GetNodeByPath("FizDocOrg", true).Text;
                    tbFizDocNumber.Text = xml.GetNodeByPath("FizDocNumber", true).Text;
                    tbFizDocSeries.Text = xml.GetNodeByPath("FizDocSeries", true).Text;
                    tbFizDocType.Text = xml.GetNodeByPath("FizDocType", true).Text;
                    tbAddrStreet.Text = xml.GetNodeByPath("AddrStreet", true).Text;
                    tbAddrZip.Text = xml.GetNodeByPath("AddrZip", true).Text;
                    tbAddrRegion.Text = xml.GetNodeByPath("AddrRegion", true).Text;
                    tbAddrState.Text = xml.GetNodeByPath("AddrState", true).Text;
                    tbAddrCity.Text = xml.GetNodeByPath("AddrCity", true).Text;
                    tbAddrApartment.Text = xml.GetNodeByPath("AddrApartment", true).Text;
                    tbAddrBuilding.Text = xml.GetNodeByPath("AddrBuilding", true).Text;
                    tbAddrHouse.Text = xml.GetNodeByPath("AddrHouse", true).Text;

                    cbFizDocType_Leave(tbFizDocType, null);

                    lOwner.Text = ""; //TODO
                    try
                    {
                        cbDocStatus.SelectedIndex = fsource.documentStatus;
                    }
                    catch (Exception)
                    {
                    }

                    profileName = xml.GetNodeByPath("ProfileName", true).Text;
                    profileLogin = xml.GetNodeByPath("ProfileLogin", true).Text;
                    profilePassword = xml.GetNodeByPath("ProfilePassword", true).Text;
                    profileSubscribers = xml.GetNodeByPath("ProfileSubscribers", true).Text;

                    if (!clone)
                    {
                        fdocument.documentDate = fsource.documentDate;
                        log("DEXPlugin.Document.Mega.EAD.DocumentForm.cs 177 docdate = " + fdocument.documentDate);
                    }
                } 
                catch (Exception) { }
            }

            try
            {
                string dts = fdocument.documentDate;
                log("DEXPlugin.Document.Mega.EAD.DocumentForm.cs 186 docdate = " + fdocument.documentDate);
                DateTime dt = new DateTime(int.Parse(dts.Substring(0, 4)), int.Parse(dts.Substring(4, 2)), int.Parse(dts.Substring(6, 2)));
                deJournalDate.Value = dt;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }

        void InitComboDictionary(TextBox src, string hintname)
        {
            src.AutoCompleteCustomSource.Clear();
            src.AutoCompleteCustomSource.AddRange(((IDEXData)toolbox).getDataHint(hintname));
            src.Text = "";
        }

        void InitComboCity(TextBox src, string fieldname)
        {
            string[] hints = ((IDEXCitySearcher)toolbox).getCityValuesList(fieldname);
            src.AutoCompleteCustomSource.Clear();
            src.AutoCompleteCustomSource.AddRange(hints);
            src.Text = "";
        }

        private void DocumentForm_Shown(object sender, EventArgs e)
        {
//            cbDocUnit.Focus();
            deJournalDate.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (ReadOnly) return;

            fdocument.documentStatus = cbDocStatus.SelectedIndex;
            if (!isOnline)
            {
                int unid = -1;
                try
                {
                    unid = int.Parse(((StringTagItem)cbDocUnit.SelectedItem).Tag);
                }
                catch (Exception) { }

                fdocument.documentUnitID = unid;
            }

            SimpleXML xml = new SimpleXML("Document");
            xml.Attributes["ID"] = module.ID;
            xml.GetNodeByPath("DocCategory", true).Text = cbDocCategory.SelectedIndex.ToString();
            xml.GetNodeByPath("DocCity", true).Text = tbDocCity.Text;
            xml.GetNodeByPath("DocDate", true).Text = deDocDate.Text;
            xml.GetNodeByPath("DocNum", true).Text = tbDocNum.Text;
            
            try
            {
                xml.GetNodeByPath("Plan", true).Text = ((StringTagItem)cbPlan.SelectedItem).Tag;
            }
            catch(Exception) 
            {
                xml.GetNodeByPath("Plan", true).Text = "-";                
            }
            xml.GetNodeByPath("PlanRegionId", true).Text = region_id;

            xml.GetNodeByPath("ICCCTL", true).Text = tbICCCTL.Text;
            xml.GetNodeByPath("ICC", true).Text = mtbICC.Text;
            xml.GetNodeByPath("MSISDN", true).Text = mtbMSISDN.Text;
            xml.GetNodeByPath("Birth", true).Text = deBirth.Text;
            xml.GetNodeByPath("FirstName", true).Text = tbFirstName.Text;
            xml.GetNodeByPath("SecondName", true).Text = tbSecondName.Text;
            xml.GetNodeByPath("LastName", true).Text = tbLastName.Text;
            xml.GetNodeByPath("FizDocDate", true).Text = deFizDocDate.Text;
            xml.GetNodeByPath("FizDocOrg", true).Text = tbFizDocOrg.Text;
            xml.GetNodeByPath("FizDocNumber", true).Text = tbFizDocNumber.Text;
            xml.GetNodeByPath("FizDocSeries", true).Text = tbFizDocSeries.Text;
            xml.GetNodeByPath("FizDocType", true).Text = tbFizDocType.Text;
            xml.GetNodeByPath("FizDocScan", true).Text = (cbFizDocScan.Checked) ? "X" : "-";
            xml.GetNodeByPath("AddrStreet", true).Text = tbAddrStreet.Text;
            xml.GetNodeByPath("AddrZip", true).Text = tbAddrZip.Text;
            xml.GetNodeByPath("AddrRegion", true).Text = tbAddrRegion.Text;
            xml.GetNodeByPath("AddrState", true).Text = tbAddrState.Text;
            xml.GetNodeByPath("AddrCity", true).Text = tbAddrCity.Text;
            xml.GetNodeByPath("AddrApartment", true).Text = tbAddrApartment.Text;
            xml.GetNodeByPath("AddrBuilding", true).Text = tbAddrBuilding.Text;
            xml.GetNodeByPath("AddrHouse", true).Text = tbAddrHouse.Text;

            xml.GetNodeByPath("ProfileName", true).Text = profileName;
            xml.GetNodeByPath("ProfileLogin", true).Text = profileLogin;
            xml.GetNodeByPath("ProfilePassword", true).Text = profilePassword;
            xml.GetNodeByPath("ProfileSubscribers", true).Text = profileSubscribers;


            fdocument.documentDate = deJournalDate.Value.ToString("yyyyMMddhhmmssfff");
            log("DEXPlugin.Document.Mega.EAD.DocumentForm.cs 278 docdate = " + fdocument.documentDate);
            fdocument.documentText = SimpleXML.SaveXml(xml);
            ArrayList err = module.ValidateDocument(toolbox, fdocument);

            IDEXValidators dv = (IDEXValidators)toolbox;
            if (!dv.CheckPeriodDate(deJournalDate.Value))
            {
                if (err == null) err = new ArrayList();
                err.Add("Некорректная дата документа DEX");
            }

            if (err != null && err.Count > 0)
            {
                string s = "В документе обнаружены ошибки:\n\n";
                foreach (string s2 in err)
                {
                    s += "* " + s2 + "\n";
                }
                MessageBox.Show(s);
            }
            else
            {
                /*
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["zip"] = tbAddrZip.Text;
                dic["city"] = tbAddrCity.Text;
                dic["region"] = tbAddrRegion.Text;
                ((IDEXCitySearcher)toolbox).setCityData(dic);
                */

                IDEXData tb = (IDEXData)toolbox;
                if (tbFirstName.Text.Trim() != "")
                    tb.setDataHint("first_name", tbFirstName.Text);
                if (tbSecondName.Text.Trim() != "")
                    tb.setDataHint("second_name", tbSecondName.Text);
                if (tbLastName.Text.Trim() != "")
                    tb.setDataHint("last_name", tbLastName.Text);
                if (tbFizDocOrg.Text.Trim() != "")
                    tb.setDataHint("fiz_doc_org", tbFizDocOrg.Text);
                if (tbFizDocType.Text.Trim() != "")
                    tb.setDataHint("fiz_doc_type", tbFizDocType.Text);
                if (tbAddrStreet.Text.Trim() != "")
                    tb.setDataHint("street", tbAddrStreet.Text);
                if (tbAddrRegion.Text.Trim() != "")
                    tb.setDataHint("region", tbAddrRegion.Text);
                if (tbAddrState.Text.Trim() != "")
                    tb.setDataHint("state", tbAddrState.Text);
                if (tbAddrCity.Text.Trim() != "")
                    tb.setDataHint("city", tbAddrCity.Text);

/*
                StringList sl = new StringList();
                sl["FirstName"] = tbFirstName.Text; //FirstName
                sl["SecondName"] = tbSecondName.Text; //SecondName
                sl["LastName"] = tbLastName.Text; // LastName
                sl["Birth"] = deBirth.Text; //Birth
                sl["FizDocType"] = tbFizDocType.Text; //FizDocType
                sl["FizDocSeries"] = tbFizDocSeries.Text; //FizDocSeries
                sl["FizDocNumber"] = tbFizDocNumber.Text; //FizDocNumber
                sl["FizDocOrg"] = tbFizDocOrg.Text;
                sl["FizDocDate"] = deFizDocDate.Text;
                sl["FizDocScan"] = (cbFizDocScan.Checked) ? "X" : "-";
                sl["AddrState"] = tbAddrState.Text;
                sl["AddrRegion"] = tbAddrRegion.Text;
                sl["AddrCity"] = tbAddrCity.Text;
                sl["AddrZip"] = tbAddrZip.Text;
                sl["AddrStreet"] = tbAddrStreet.Text;
                sl["AddrHouse"] = tbAddrHouse.Text;
                sl["AddrBuilding"] = tbAddrBuilding.Text;
                sl["AddrApartment"] = tbAddrApartment.Text;
                
                IDEXPeopleSearcher ps = (IDEXPeopleSearcher)toolbox;
                ps.setPeopleData(sl);
*/
                fdocument.documentDigest = string.Format(
                    "{0}, {1}, {2} {3} {4}", mtbMSISDN.Text, deDocDate.Text,
                    tbLastName.Text, tbFirstName.Text, tbSecondName.Text
                    );
                DialogResult = DialogResult.OK;
            }
        }

        private string capitalize(string src)
        {
            try
            {
                string ret = src.TrimStart();
                return ret.Substring(0, 1).ToUpper() + ret.Substring(1);
            }
            catch (Exception)
            {
            }
            return src;
        }

        private void cbDocUnit_Enter(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybRU();            
        }

        private void tbDocNum_Enter(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybEN();
        }

        private void cbDocUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void cbDocStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                bOk.PerformClick();
            }
        }

        private void mtbICC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void tbAddrZip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                /*
                try
                {
                    Dictionary<string, string> dic = ((IDEXCitySearcher)toolbox).getCityData("zip", tbAddrZip.Text);
                    if (dic["city"] != "")
                    {
                        tbAddrCity.Text = dic["city"];
                    }
                    if (dic["region"] != "")
                    {
                        tbAddrRegion.Text = dic["region"];
                    }
                }
                catch (Exception)
                {
                }
                */
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void fillSim(Dictionary<string, string> dicsim)
        {
            region_id = "";
            try
            {
                if (dicsim != null)
                {
                    if (dicsim.ContainsKey("region_id")) region_id = dicsim["region_id"];

                    if (dicsim.ContainsKey("msisdn")) mtbMSISDN.Text = dicsim["msisdn"];
                    if (dicsim.ContainsKey("icc")) mtbICC.Text = dicsim["icc"];
                    if (dicsim.ContainsKey("plan_id")) StringTagItem.SelectByTag(cbPlan, dicsim["plan_id"], true);
                    if (!isOnline)
                    {
                        if (dicsim.ContainsKey("owner_title")) lOwner.Text = dicsim["owner_title"];
                        else lOwner.Text = "?";
                        if (dicsim.ContainsKey("region_title")) lOwner.Text += ", " + dicsim["region_title"];
                        if (dicsim.ContainsKey("owner_status")) lOwner.Text += ", " + ((bool.Parse(dicsim["owner_status"])) ? "[Активен]" : "[Заблокирован]");
                    }
                }
            }
            catch (Exception)
            {
            }
        
        }

        private void mtbMSISDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                fillSim(sim.getSimByMSISDN(mtbMSISDN.Text.Trim()));
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }

        }

        private void cbFizDocType_Leave(object sender, EventArgs e)
        {
            string s = ((Control)sender).Text;
            if (s.Trim().ToLower().Equals("паспорт"))
            {
                tbFizDocSeries.MaxLength = 4;
                tbFizDocNumber.MaxLength = 6;
            }
            else
            {
                tbFizDocSeries.MaxLength = 0;
                tbFizDocNumber.MaxLength = 0;
            }
        }

        private void cbAddrCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
//                    tbAddrCity.Text = capitalize(tbAddrCity.Text);
                    Dictionary<string, string> dic = ((IDEXCitySearcher)toolbox).getCityData("city", tbAddrCity.Text);
                    if (dic["zip"] != "")
                    {
                        tbAddrZip.Text = dic["zip"];
                    }
                    if (dic["region"] != "")
                    {
                        tbAddrRegion.Text = dic["region"];
                    }
                }
                catch (Exception)
                {
                }

                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }

        }

        private void deBirth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                tbFizDocType.Focus();
            }

        }

        private void setPeopleDataToFields(StringList pdata, string phash)
        {
            tbFirstName.Text = pdata["FirstName"];
            tbSecondName.Text = pdata["SecondName"];
            tbLastName.Text = pdata["LastName"];
            deBirth.Text = pdata["Birth"];
            tbFizDocType.Text = pdata["FizDocType"];
            tbFizDocSeries.Text = pdata["FizDocSeries"];
            tbFizDocNumber.Text = pdata["FizDocNumber"];
            tbFizDocOrg.Text = pdata["FizDocOrg"];
            deFizDocDate.Text = pdata["FizDocDate"];
            string fds = pdata["FizDocScan"];
            cbFizDocScan.Checked = fds.Equals("x", StringComparison.InvariantCultureIgnoreCase);
            tbAddrState.Text = pdata["AddrState"];
            tbAddrRegion.Text = pdata["AddrRegion"];
            tbAddrCity.Text = pdata["AddrCity"];
            tbAddrZip.Text = pdata["AddrZip"];
            tbAddrStreet.Text = pdata["AddrStreet"];
            tbAddrHouse.Text = pdata["AddrHouse"];
            tbAddrBuilding.Text = pdata["AddrBuilding"];
            tbAddrApartment.Text = pdata["AddrApartment"];


            if ("1".Equals(pdata["suspect"]))
            {
                MessageBox.Show("Возможно, у этого человека указаны неверные паспортные данные!\nПожалуйста, внимательно проверьте сведения.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        ArrayList phash;

        public string DoPeopleSearch(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = false;
            wmea.textMessage = "Поиск подходящих данных";
            wmea.DoEvents();
            phash = null;
            try
            {
                IDEXPeopleSearcher ps = (IDEXPeopleSearcher)toolbox;
                string firstn = (tbFirstName.Text.Trim().Equals("")) ? null : tbFirstName.Text.Trim();
                string secondn = (tbSecondName.Text.Trim().Equals("")) ? null : tbSecondName.Text.Trim();
                string lastn = (tbLastName.Text.Trim().Equals("")) ? null : tbLastName.Text.Trim();
                string dbirth = (deBirth.Text.Equals(deBirth.Value.ToString("dd.MM.yyyy"))) ? deBirth.Text : null;

                phash = ps.getPeopleHash(firstn, secondn, lastn, dbirth);
            }
            catch (Exception)
            {
            }
            return (phash == null || phash.Count < 1) ? "Не найдено ни одного абонента" : "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rt = WaitMessage.Execute(new WaitMessageEvent(DoPeopleSearch));

            if (rt != null && rt.Length > 0)
            {
                MessageBox.Show(rt);
                return;
            }

            ArrayList h = phash;
            if (h != null && h.Count > 0)
            {
                if (h.Count > 1)
                {
                    // Диалог выбора абонента из списка
                    PeopleForm pf = new PeopleForm(toolbox, h);
                    if (pf.ShowDialog() == DialogResult.OK)
                    {
                        StringList sl = pf.selectedItem;
                        tbFirstName.Text = sl["FirstName"];
                        tbSecondName.Text = sl["SecondName"];
                        tbLastName.Text = sl["LastName"];
                        deBirth.Text = sl["Birth"];
                        tbFizDocType.Text = sl["FizDocType"];
                        tbFizDocSeries.Text = sl["FizDocSeries"];
                        tbFizDocNumber.Text = sl["FizDocNumber"];
                        tbFizDocOrg.Text = sl["FizDocOrg"];
                        deFizDocDate.Text = sl["FizDocDate"];
                        string fds = sl["FizDocScan"];
                        cbFizDocScan.Checked = fds.Equals("x", StringComparison.InvariantCultureIgnoreCase);
                        tbAddrState.Text = sl["AddrState"];
                        tbAddrRegion.Text = sl["AddrRegion"];
                        tbAddrCity.Text = sl["AddrCity"];
                        tbAddrZip.Text = sl["AddrZip"];
                        tbAddrStreet.Text = sl["AddrStreet"];
                        tbAddrHouse.Text = sl["AddrHouse"];
                        tbAddrBuilding.Text = sl["AddrBuilding"];
                        tbAddrApartment.Text = sl["AddrApartment"];
                        cbDocStatus.Focus();
                    }
                }
                else
                {
                    IDEXPeopleSearcher ps = (IDEXPeopleSearcher)toolbox;
                    setPeopleDataToFields(ps.getPeopleData((string)h[0]), (string)h[0]);
                }
            }
        }

        private void tbDocCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                ((Control)sender).Text = capitalize(((Control)sender).Text);
                SelectNextControl((Control)sender, true, true, true, false);
            }

        }

        private void LoadDefaultsToControls()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            StringList defs = new StringList(cfg.getStr(module.ID, "DefaultValues", ""));

            tbDocNum.Text = defs["DocNum"];
            tbDocCity.Text = defs["DocCity"];
            try
            {
                cbDocCategory.SelectedIndex = int.Parse(defs["DocCategory"]);
            }
            catch (Exception)
            {
            }
            tbICCCTL.Text = defs["ICCCTL"];
            tbFirstName.Text = defs["FirstName"];
            tbSecondName.Text = defs["SecondName"];
            tbLastName.Text = defs["LastName"];
            deBirth.Text = defs["Birth"];
            tbFizDocType.Text = defs["FizDocType"];
            tbFizDocSeries.Text = defs["FizDocSeries"];
            tbFizDocNumber.Text = defs["FizDocNumber"];
            tbFizDocOrg.Text = defs["FizDocOrg"];
            deFizDocDate.Text = defs["FizDocDate"];
            cbFizDocScan.Checked = true.ToString().Equals(defs["FizDocScan"]);
            tbAddrState.Text = defs["AddrState"];
            tbAddrRegion.Text = defs["AddrRegion"];
            tbAddrCity.Text = defs["AddrCity"];
            tbAddrZip.Text = defs["AddrZip"];
            tbAddrStreet.Text = defs["AddrStreet"];
            tbAddrHouse.Text = defs["AddrHouse"];
            tbAddrBuilding.Text = defs["AddrBuilding"];
            tbAddrApartment.Text = defs["AddrApartment"];

            StringTagItem.SelectByTag(cbDocUnit, cfg.getStr(this.Name, "cbDocUnit", ""), false);

        }

        private void bSaveDefaults_Click(object sender, EventArgs e)
        {
            StringList defs = new StringList();
            defs["DocNum"] = tbDocNum.Text;
            defs["DocCity"] = tbDocCity.Text;
            defs["DocCategory"] = cbDocCategory.SelectedIndex.ToString();
            defs["ICCCTL"] = tbICCCTL.Text;
            defs["FirstName"] = tbFirstName.Text;
            defs["SecondName"] = tbSecondName.Text;
            defs["LastName"] = tbLastName.Text;
            defs["Birth"] = deBirth.Text;
            defs["FizDocType"] = tbFizDocType.Text;
            defs["FizDocSeries"] = tbFizDocSeries.Text;
            defs["FizDocNumber"] = tbFizDocNumber.Text;
            defs["FizDocOrg"] = tbFizDocOrg.Text;
            defs["FizDocDate"] = deFizDocDate.Text;
            defs["FizDocScan"] = cbFizDocScan.Checked.ToString();
            defs["AddrState"] = tbAddrState.Text;
            defs["AddrRegion"] = tbAddrRegion.Text;
            defs["AddrCity"] = tbAddrCity.Text;
            defs["AddrZip"] = tbAddrZip.Text;
            defs["AddrStreet"] = tbAddrStreet.Text;
            defs["AddrHouse"] = tbAddrHouse.Text;
            defs["AddrBuilding"] = tbAddrBuilding.Text;
            defs["AddrApartment"] = tbAddrApartment.Text;

            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setStr(module.ID, "DefaultValues", defs.SaveToString());

            MessageBox.Show("Значения по умолчанию сохранены");

        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                IDEXConfig cfg = (IDEXConfig)toolbox;
                cfg.setStr(this.Name, "cbDocUnit", ((StringTagItem)cbDocUnit.SelectedItem).Tag);
            }
            catch (Exception)
            {
            }
        }

        private void bSearchSIM_Click(object sender, EventArgs e)
        {
            fillSim(sim.getSimByICC(mtbICC.Text.Trim()));
        }

        public static void log(string s)
        {
            /*
                string fn = Application.ExecutablePath + ".log";
                try
                {
                    FileStream fs = new FileStream(fn, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(s);
                    sw.Close();

                }
                catch (Exception)
                {
                }
             */
        }

    }
}
