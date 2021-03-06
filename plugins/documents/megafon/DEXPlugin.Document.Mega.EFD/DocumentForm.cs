using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXSIM;
using DEXExtendLib;
using System.Collections;
using System.Net;
using System.Globalization;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;

namespace DEXPlugin.Document.Mega.EFD
{
    public partial class DocumentForm : Form
    {
        public object toolbox;
        public Document module;
        IDEXSim sim;
        bool isOnline;

        IDEXDocumentData fsource, fdocument;
        bool ReadOnly;

        DataTable tPlan; //-, dtSbmsPlan = null;

        bool ifChangeScan = false;
        bool ifScanIsset = false;

        string plan, intPlanPrn;

        //- string pctyp_id = null;
        string docnum;

        string _ProfileName = "", _ProfileCode = "";
        string _DealerCode = "", _DealerName = "";
        bool _isFS = false;
        bool isGF = false;

        string nodejsserver = "";
        string DutyId = null;
        
        bool isFS
        {
            get
            {
                return _isFS;
            }
            set 
            {
                _isFS = value;
                try
                {
                    lFS.Visible = value;
                }
                catch (Exception) { }
            }
        }

        
        public DocumentForm()
        {
            InitializeComponent();
        }

        public void InitDocument(IDEXDocumentData source, IDEXDocumentData document, bool clone, bool ReadOnly)
        {
            isOnline = ((IDEXUserData)toolbox).isOnline;
            this.ReadOnly = ReadOnly;
            bOk.Visible = !ReadOnly;

            IDEXConfig cfg = (IDEXConfig)toolbox;

            String currentDb = cfg.getStr("common", "currentDb", "");

            bool expressInput = cfg.getBool(module.ID, "express_input", false);

            if (expressInput)
            {
                cbSex.Tag = deBirth;
                tbAddrApartment.Tag = bOk;
            }

            IDEXServices srv = (IDEXServices)toolbox;
            sim = (IDEXSim)srv.GetService("sim");

            cbScan.SelectedIndex = 0;

            fsource = source;
            fdocument = document;

            lDocUnit.Visible = !isOnline;
            cbDocUnit.Visible = !isOnline;

            if (!isOnline)
            {
               

                DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");
                StringTagItem.UpdateCombo(cbDocUnit, t, null, "uid", "title", false);

                cbDocUnit.Text = "";
                cbControl.Enabled = true;

            }
            else
            {
                addOrg.Visible = false;
                chDulIsCorrect.Visible = false;
            }

            try
            {
                DataTable t1 = ((IDEXData)toolbox).getQuery("select rvalue from `registers` where rname = 'nodejsserver'");
                nodejsserver = t1.Rows[0]["rvalue"].ToString();
            }
            catch (Exception)
            {
                string ssss = "";
            }

            // тарифные планы для GF


            //DataTable tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf` where allow_m = '1' order by title DESC");
            DataTable tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf` where allow_m = '1' order by title ASC");
            StringTagItem.UpdateCombo(cbPlan, tgf, null, "plan_id", "title", false);
            cbPlan.Items.Add(" ");
            cbPlan.SelectedIndex = cbPlan.FindString(" ");

            //DataTable tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf` WHERE `allowed` = 1 order by title");
            //StringTagItem.UpdateCombo(cbPlan, tgf, null, "plan_id", "title", false);
            //cbPlan.Items.Add(" ");
            //cbPlan.SelectedIndex = cbPlan.FindString(" ");

            // профили отправки
            DataTable dtProf = ((IDEXData)toolbox).getQuery("select * from `efd_profiles` order by pname");
            StringTagItem.UpdateCombo(cbProf, dtProf, null, "pcode", "pname", false);
            cbProf.Items.Add(" ");
            cbProf.SelectedIndex = cbProf.FindString(" ");

            docnum = "";
            cbDocCategory.SelectedIndex = 0;

            //cbDocClientType
            tbCodeWord.Text = "";

            mtbMSISDN.Text = "";
            mtbMSISDN.Mask = ((IDEXConfig)toolbox).getRegisterStr("megafon_msisdn_mask", "0000000000");

            mtbICC.Text = "";
            mtbICC.Mask = ((IDEXConfig)toolbox).getRegisterStr("megafon_icc_mask", "00000000000000000");

            isFS = false;
            isGF = false;

            plan = "";
            intPlanPrn = "";

//-            pctyp_id = "";
//-            cbPlan.Items.Clear();

            lOwner.Text = "";
            tbAddrCity.Text = "";
            //tbDeliveryCity.Text = "";

            InitDictionaries();
            LoadDefaultsToControls();

            IDEXUserData dud = (IDEXUserData)toolbox;
            SimpleXML props = dud.UserProperties;
            try
            {
                cbDocStatus.SelectedIndex = int.Parse(props["DefaultDocumentState"].Text);
            }
            catch (Exception)
            {
                cbDocStatus.SelectedIndex = 0;
            }

            //cbDocStatus.Enabled = !isOnline;

            _DealerCode = props[currentDb + "DealerCode"].Text;
            _DealerName = props[currentDb + "DealerName"].Text;

            DutyId = null;

            fdocument.documentDate = DateTime.Now.ToString("yyyyMMddhhmmssfff");

            deDocDate.Value = DateTime.Now;

            if (fsource != null)
            {
                try
                {
                    SimpleXML xml = SimpleXML.LoadXml(fsource.documentText);

                    if (!isOnline)
                    {
                        StringTagItem.SelectByTag(cbDocUnit, fsource.documentUnitID.ToString(), true);
                    }

                    tbDocCity.Text = xml["DocCity"].Text;

                    docnum = xml.GetNodeByPath("DocNum", true).Text;
                    StringTagItem.SelectByTag(cbDocCategory, xml.GetNodeByPath("DocCategory", true).Text, false);
                    StringTagItem.SelectByTag(cbDocClientType, xml.GetNodeByPath("DocClientType", true).Text, false);
                    tbCodeWord.Text = xml.GetNodeByPath("CodeWord", true).Text;

                    mtbICC.Text = xml.GetNodeByPath("ICC", true).Text;
                    int icccrc = DEXTools.calcIccCtl(mtbICC.Text);
                    tbICCCTL.Text = icccrc > -1 ? icccrc.ToString() : "";
                    mtbMSISDN.Text = xml.GetNodeByPath("MSISDN", true).Text;

                    isFS = (true.ToString().Equals(xml["fs"].Text) || "1".Equals(xml["fs"].Text));
                    isGF = (true.ToString().Equals(xml["gf"].Text) || "1".Equals(xml["gf"].Text));

                    plan = xml["Plan"].Text;                     
                    intPlanPrn = xml["IntPlanPrn"].Text;

                    //lbCustomerId.Text = xml.GetNodeByPath("CustomerId", true).Text;
                    tbCID.Text = xml.GetNodeByPath("CustomerId", true).Text;
                    tbCAN.Text = xml.GetNodeByPath("sbms_paccount", true).Text;


                    try
                    {
                        cbScan.SelectedIndex = Convert.ToInt32(xml["FizDocScan"].Text);
                        lbScanMime.Text = xml["FizDocScanMime"].Text;

                        if (cbScan.SelectedIndex == 1)
                        {
                            ifScanIsset = true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    //
                    signatire.Text = fsource.signature;

                    // определим, gf ли симка
                    if (isFS)
                    {
                        cbPlan.Items.Clear();
                        tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf`  WHERE allowed = '1' AND allow_m = '1' order by title");
                        StringTagItem.UpdateCombo(cbPlan, tgf, null, "plan_id", "title", false);
                        StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, false);
                    }

                    if (isGF)
                    {
                        //cbPlan.Visible = true;
                        lGfPlan.Visible = true;
                        bSetProf.Visible = true;
                        cbProf.Visible = true;
                        

                        //cbFizDocType.Items.Clear();
                        //DataTable tidt = ((IDEXData)toolbox).getQuery("select * from `gf_identity_doc_types` order by name");
                        //StringTagItem.UpdateCombo(cbFizDocType, tidt, null, "identityDocTypeId", "name", false);

                        StringTagItem.SelectByTag(cbProf, xml.GetNodeByPath("ProfileCode", true).Text, false);
                    }
                    //StringTagItem.UpdateCombo(cbFizDocType, tidt, null, "identityDocTypeId", "name", false);
                    //tidt = ((IDEXData)toolbox).getQuery("select * from `gf_identity_doc_types` order by name");
                    //StringTagItem.UpdateCombo(cbFizDocType, tidt, null, "identityDocTypeId", "name", false);
                    //cbFizDocType.Items.Add(" ");
                    //cbFizDocType.SelectedIndex = cbFizDocType.FindString(" ");
                    StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, false);


                    if (isGF)
                    {
                        //cbPlan.Visible = true;
                        //lGfPlan.Visible = true;
                        //StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, false);

                        //bSetProf.Visible = true;
                        //cbProf.Visible = true;

                        //StringTagItem.SelectByTag(cbProf, xml.GetNodeByPath("ProfileCode", true).Text, false);
                    }

                    /*
                    SimpleXML xmlPlans = xml.GetNodeByPath("SBMSPlans", false);
                    if (xmlPlans != null)
                    {
                        dtSbmsPlan = new DataTable();
                        dtSbmsPlan.Columns.Add("id", typeof(string));
                        dtSbmsPlan.Columns.Add("title", typeof(string));
                        foreach (SimpleXML xmlItem in xmlPlans.Child)
                        {
                            if (xmlItem.Name.Equals("item") && xmlItem.Attributes.ContainsKey("id") && xmlItem.Attributes.ContainsKey("title"))
                            {
                                dtSbmsPlan.Rows.Add(xmlItem.Attributes["id"], xmlItem.Attributes["title"]);
                            }
                        }
                        if (dtSbmsPlan.Rows.Count > 0)
                        {
                            StringTagItem.UpdateCombo(cbPlan, dtSbmsPlan, null, "id", "title", false);
                            StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, true);
                        }
                        else
                        {
                            dtSbmsPlan = null;
                        }
                    }
                    */

                    //- if (xml.GetNodeByPath("pctyp_id", false) != null) pctyp_id = xml["pctyp_id"].Text;

                    tbFirstName.Text = xml.GetNodeByPath("FirstName", true).Text;
                    tbSecondName.Text = xml.GetNodeByPath("SecondName", true).Text;
                    tbLastName.Text = xml.GetNodeByPath("LastName", true).Text;
                    StringTagItem.SelectByTag(cbSex, xml.GetNodeByPath("Sex", true).Text, true);

                    tbFizBirthPlace.Text = xml.GetNodeByPath("FizBirthPlace", true).Text;
                    deBirth.Text = xml.GetNodeByPath("Birth", true).Text;

                    tbFizDocOrg.Text = xml.GetNodeByPath("FizDocOrg", true).Text;
                    // код подразделения и гражданство
                    tbFizDocOrgCode.Text = xml.GetNodeByPath("FizDocOrgCode", true).Text;

                    deFizDocDate.Text = xml.GetNodeByPath("FizDocDate", true).Text;
                    tbFizDocNumber.Text = xml.GetNodeByPath("FizDocNumber", true).Text;
                    tbFizDocSeries.Text = xml.GetNodeByPath("FizDocSeries", true).Text;

                    try
                    {

                        cbControl.Checked = Convert.ToBoolean( Convert.ToInt32(xml.GetNodeByPath("Control", true).Text));
                    }
                    catch (Exception) { }

                    StringTagItem.SelectByTag(cbFizDocType, xml.GetNodeByPath("FizDocType", true).Text, false);

                    StringTagItem.SelectByTag(cbAddrCountry, xml.GetNodeByPath("AddrCountry", true).Text, false);
                    tbAddrState.Text = xml.GetNodeByPath("AddrState", true).Text;
                    tbAddrCity.Text = xml.GetNodeByPath("AddrCity", true).Text;
                    tbAddrZip.Text = xml.GetNodeByPath("AddrZip", true).Text;
                    tbAddrRegion.Text = xml.GetNodeByPath("AddrRegion", true).Text;
                    tbAddrStreet.Text = xml.GetNodeByPath("AddrStreet", true).Text;
                    tbAddrHouse.Text = xml.GetNodeByPath("AddrHouse", true).Text;
                    tbAddrBuilding.Text = xml.GetNodeByPath("AddrBuilding", true).Text;
                    tbAddrApartment.Text = xml.GetNodeByPath("AddrApartment", true).Text;

                    tbAddrPhone.Text = xml.GetNodeByPath("AddrPhone", true).Text;
                    tbContactEmail.Text = xml.GetNodeByPath("ContactEmail", true).Text;
                    tbFizInn.Text = xml.GetNodeByPath("FizInn", true).Text;

                    //StringTagItem.SelectByTag(cbDeliveryType, xml.GetNodeByPath("DeliveryType", true).Text, false);
                    //tbDeliveryPhone.Text = xml.GetNodeByPath("DeliveryPhone", true).Text;
                    //tbDeliveryFax.Text = xml.GetNodeByPath("DeliveryFax", true).Text;

                    //StringTagItem.SelectByTag(cbDeliveryCountry, xml.GetNodeByPath("DeliveryCountry", true).Text, false);
                    //tbDeliveryState.Text = xml.GetNodeByPath("DeliveryState", true).Text;
                    //tbDeliveryCity.Text = xml.GetNodeByPath("DeliveryCity", true).Text;
                    //tbDeliveryZip.Text = xml.GetNodeByPath("DeliveryZip", true).Text;
                    //tbDeliveryRegion.Text = xml.GetNodeByPath("DeliveryRegion", true).Text;
                    //tbDeliveryStreet.Text = xml.GetNodeByPath("DeliveryStreet", true).Text;
                    //tbDeliveryHouse.Text = xml.GetNodeByPath("DeliveryHouse", true).Text;
                    //tbDeliveryBuilding.Text = xml.GetNodeByPath("DeliveryBuilding", true).Text;
                    //tbDeliveryApartment.Text = xml.GetNodeByPath("DeliveryApartment", true).Text;
                    deDocDate.Text = xml.GetNodeByPath("DocDate", true).Text;

                    try
                    {
                        cbDocStatus.SelectedIndex = fsource.documentStatus;
                    }
                    catch (Exception)
                    {
                    }

                    if (!clone)
                    {
                        fdocument.documentDate = fsource.documentDate;
                    }

                    _ProfileName = xml["ProfileName"].Text;
                    _ProfileCode = xml["ProfileCode"].Text;

                    _DealerCode = xml["DealerCode"].Text;
                    _DealerName = xml["DealerName"].Text;

                    if (xml.GetNodeByPath("DutyId", false) != null) DutyId = xml["DutyId"].Text;

                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }
            }

            StringTagItem.SelectByTag(cbDocCategory, "1", true);

            bGetDocNum_Click(bGetDocNum, null);


            try
            {
                string dts = fdocument.documentDate;
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

        public void InitDictionaries()
        {
            IDEXData d = (IDEXData)toolbox;
            tPlan = d.getTable("um_plans");

            //DataTable td = d.getTable("efd_identity_types");
            //StringTagItem.UpdateCombo(cbFizDocType, td, null, "id", "title", false);

            DataTable td = d.getTable("gf_identity_doc_types");
            StringTagItem.UpdateCombo(cbFizDocType, td, null, "identityDocTypeId", "name", false);

            td = d.getTable("efd_country");
            StringTagItem.UpdateCombo(cbAddrCountry, td, null, "id", "title", false);
            //StringTagItem.UpdateCombo(cbDeliveryCountry, td, null, "id", "title", false);
            //td = d.getTable("efd_delivery_types");
            //StringTagItem.UpdateCombo(cbDeliveryType, td, null, "id", "title", false);
            td = d.getTable("efd_client_cats");
            StringTagItem.UpdateCombo(cbDocCategory, td, null, "id", "title", false);
            td = d.getTable("efd_client_resident_types");
            StringTagItem.UpdateCombo(cbDocClientType, td, null, "id", "title", false);
            td = d.getTable("efd_genders");
            StringTagItem.UpdateCombo(cbSex, td, null, "id", "title", false);

            //- cbPlan.Items.Clear();
            //- cbPlan.Sorted = true;

            InitComboDictionary(tbFizDocOrg, "fiz_doc_org"); //
            //InitComboDictionary(tbFizDocType, "fiz_doc_type"); //
            InitComboDictionary(tbAddrStreet, "street"); //
            InitComboCity(tbAddrRegion, "region");
            InitComboDictionary(tbAddrState, "state");
            InitComboCity(tbAddrCity, "city");
            InitComboCity(tbDocCity, "city");
        }

        private void LoadDefaultsToControls()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            StringList defs = new StringList(cfg.getStr(module.ID, "DefaultValues", ""));

            tbDocCity.Text = defs["DocCity"];
            tbDocNum.Text = defs["DocNum"];
            try
            {
                cbDocCategory.SelectedIndex = int.Parse(defs["DocCategory"]);
            }
            catch (Exception)
            {
            }
            try
            {
                cbDocClientType.SelectedIndex = int.Parse(defs["DocClientType"]);
            }
            catch (Exception)
            {
            }
            tbFirstName.Text = defs["FirstName"];
            tbSecondName.Text = defs["SecondName"];
            tbLastName.Text = defs["LastName"];
            StringTagItem.SelectByTag(cbSex, defs["Sex"], false);
            tbFizBirthPlace.Text = defs["FizBirthPlace"];
            deBirth.Text = defs["Birth"];
            StringTagItem.SelectByTag(cbFizDocType, defs["FizDocType"], true);
            tbFizDocSeries.Text = defs["FizDocSeries"];
            tbFizDocNumber.Text = defs["FizDocNumber"];
            tbFizDocOrg.Text = defs["FizDocOrg"];
            deFizDocDate.Text = defs["FizDocDate"];
            StringTagItem.SelectByTag(cbAddrCountry, defs["AddrCountry"], false);
            tbAddrState.Text = defs["AddrState"];
            tbAddrRegion.Text = defs["AddrRegion"];
            tbAddrCity.Text = defs["AddrCity"];
            tbAddrZip.Text = defs["AddrZip"];
            tbAddrStreet.Text = defs["AddrStreet"];
            tbAddrHouse.Text = defs["AddrHouse"];
            tbAddrBuilding.Text = defs["AddrBuilding"];
            tbAddrApartment.Text = defs["AddrApartment"];
            tbAddrPhone.Text = defs["AddrPhone"];
            tbContactEmail.Text = defs["ContactEmail"];
            tbFizInn.Text = defs["FizInn"];
            //StringTagItem.SelectByTag(cbDeliveryType, defs["DeliveryType"], false);
            //tbDeliveryPhone.Text = defs["DeliveryPhone"];
            //tbDeliveryFax.Text = defs["DeliveryFax"];
            //StringTagItem.SelectByTag(cbDeliveryCountry, defs["DeliveryCountry"], false);
            //tbDeliveryState.Text = defs["DeliveryState"];
            //tbDeliveryRegion.Text = defs["DeliveryRegion"];
            //tbDeliveryCity.Text = defs["DeliveryCity"];
            //tbDeliveryZip.Text = defs["DeliveryZip"];
            //tbDeliveryStreet.Text = defs["DeliveryStreet"];
            //tbDeliveryHouse.Text = defs["DeliveryHouse"];
            //tbDeliveryBuilding.Text = defs["DeliveryBuilding"];
            //tbDeliveryApartment.Text = defs["DeliveryApartment"];

            StringTagItem.SelectByTag(cbDocUnit, cfg.getStr(this.Name, "cbDocUnit", ""), false);

        }

        private void bSaveDefaults_Click(object sender, EventArgs e)
        {
            StringList defs = new StringList();
            defs["DocCity"] = tbDocCity.Text;
            defs["DocNum"] = tbDocNum.Text;
            defs["DocCategory"] = cbDocCategory.SelectedIndex.ToString();
            defs["DocClientType"] = cbDocClientType.SelectedIndex.ToString();
            defs["FirstName"] = tbFirstName.Text;
            defs["SecondName"] = tbSecondName.Text;
            defs["LastName"] = tbLastName.Text;
            defs["Sex"] = StringTagItem.getSelectedTag(cbSex, "");
            defs["Birth"] = deBirth.Text;
            defs["FizBirthPlace"] = tbFizBirthPlace.Text;
            defs["FizDocType"] = StringTagItem.getSelectedTag(cbFizDocType, "");
            defs["FizDocSeries"] = tbFizDocSeries.Text;
            defs["FizDocNumber"] = tbFizDocNumber.Text;
            defs["FizDocOrg"] = tbFizDocOrg.Text;
            defs["FizDocDate"] = deFizDocDate.Text;
            defs["AddrCountry"] = StringTagItem.getSelectedTag(cbAddrCountry, "");
            defs["AddrState"] = tbAddrState.Text;
            defs["AddrRegion"] = tbAddrRegion.Text;
            defs["AddrCity"] = tbAddrCity.Text;
            defs["AddrZip"] = tbAddrZip.Text;
            defs["AddrStreet"] = tbAddrStreet.Text;
            defs["AddrHouse"] = tbAddrHouse.Text;
            defs["AddrBuilding"] = tbAddrBuilding.Text;
            defs["AddrApartment"] = tbAddrApartment.Text;
            defs["AddrPhone"] = tbAddrPhone.Text;
            defs["ContactEmail"] = tbContactEmail.Text;
            defs["FizInn"] = tbFizInn.Text;
            //defs["DeliveryType"] = StringTagItem.getSelectedTag(cbDeliveryType, "");
            //defs["DeliveryPhone"] = tbDeliveryPhone.Text;
            //defs["DeliveryFax"] = tbDeliveryFax.Text;
            //defs["DeliveryCountry"] = StringTagItem.getSelectedTag(cbDeliveryCountry, "");
            //defs["DeliveryState"] = tbDeliveryState.Text;
            //defs["DeliveryRegion"] = tbDeliveryRegion.Text;
            //defs["DeliveryCity"] = tbDeliveryCity.Text;
            //defs["DeliveryZip"] = tbDeliveryZip.Text;
            //defs["DeliveryStreet"] = tbDeliveryStreet.Text;
            //defs["DeliveryHouse"] = tbDeliveryHouse.Text;
            //defs["DeliveryBuilding"] = tbDeliveryBuilding.Text;
            //defs["DeliveryApartment"] = tbDeliveryApartment.Text;
           
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setStr(module.ID, "DefaultValues", defs.SaveToString());

            MessageBox.Show("Значения по умолчанию сохранены");

        }


        private void cbDocUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                Control ctl = (Control)sender;
                if (ctl == deBirth) button1_Click(button1, null);
                if (ctl.Tag != null && ctl.Tag is Control)
                {
                    e.SuppressKeyPress = true;
                    if (ctl.Tag == bOk)
                    {
                        bOk.PerformClick();
                    }
                    else
                        ((Control)ctl.Tag).Focus();
                }
                else
                {


                    /*
                    for (int i = 0; i < obj["data"]["list"].Count(); i++)
                    {
                        tbFizDocOrg.Text = obj["data"]["list"][i]["title"].ToString();
                        cbFizDocOrg.Items.Add(new StringTagItem(obj["data"]["list"][i]["title"].ToString(), obj["data"]["list"][i]["code"].ToString()));
                    }

                    if (obj["data"]["list"].Count() == 1) cbFizDocOrg.SelectedIndex = 0;
                    else
                    {

                    }
                    */
                    if (sender == cbSex)
                    {
                        deBirth.Focus();
                    }
                    else if (sender == tbAddrApartment)
                    {
                        bOk.PerformClick();
                    } 
                    else
                    {
                        SelectNextControl((Control)sender, true, true, true, false);
                    }

                }

            }
        }

        private void cbDocUnit_Enter(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybRU();
        }

        private void tbDocNum_Enter(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybEN();
        }

        private void mtbMSISDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                fillSim(sim.getSimByMSISDN(mtbMSISDN.Text.Trim()));
                e.SuppressKeyPress = true;
                //SelectNextControl((Control)sender, true, true, true, false);
                cbPlan.Focus();
                cbPlan.DroppedDown = true;
            }
        }

        /*
        private void bSearchSIM_Click(object sender, EventArgs e)
        {
            //- dtSbmsPlan = null;
            //- cbPlan.Items.Clear();
            fillSim(sim.getSimByICC(mtbICC.Text.Trim()), true);
        }
        */

        private void cbDocStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                bOk.PerformClick();
            }
        }

        string autocompleteFind(AutoCompleteStringCollection acsc, string search, string def)
        {
            try
            {
                foreach (string s in acsc)
                {
                    if (search.Equals(s, StringComparison.InvariantCultureIgnoreCase)) return s;
                }
            }
            catch (Exception) { }
            
            return def;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (ReadOnly) return;

            //0, 3
            if (fsource == null)
            {
                try
                {

                    //int dcode = int.Parse(((StringTagItem)cbDeliveryType.SelectedItem).Tag);
                    //if (dcode == 0 || dcode == 3) {
                    //    StringTagItem.SelectByTag(cbDeliveryType, "3", false);
                    //    bDeliveryCopy_Click(bDeliveryCopy, null);
                    //}
                }
                catch (Exception) { }
            }

            if (isOnline)
            {
                if (StringTagItem.getSelectedTag(cbProf, "") == "")
                {
                    cbDocStatus.SelectedIndex = 1;
                }
            }
        
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
            xml["DocCity"].Text = autocompleteFind(tbDocCity.AutoCompleteCustomSource, tbDocCity.Text, tbDocCity.Text);
            xml["DocNum"].Text = tbDocNum.Text;
            xml["DocDateJournal"].Text = deJournalDate.Value.ToString("dd.MM.yyyy");
            xml["DocDate"].Text = deDocDate.Text;
            xml["DocCategory"].Text = StringTagItem.getSelectedTag(cbDocCategory, "");
            if (cbDocCategory.SelectedItem != null) xml["DocCategoryPrn"].Text = cbDocCategory.SelectedItem.ToString();
            xml["DocClientType"].Text = StringTagItem.getSelectedTag(cbDocClientType, "");
            xml["CodeWord"].Text = tbCodeWord.Text;

            xml["MSISDN"].Text = mtbMSISDN.Text;
            xml["ICC"].Text = mtbICC.Text;
            xml["ICCCTL"].Text = tbICCCTL.Text;
            xml["fs"].Text = isFS.ToString();
            xml["gf"].Text = isGF.ToString();

            //xml["customerId"].Text = lbCustomerId.Text;
            xml["customerId"].Text = tbCID.Text;
            xml["sbms_paccount"].Text = tbCAN.Text;

            try
            {
                xml["Control"].Text = Convert.ToInt32(cbControl.Checked).ToString();
            }
            catch (Exception) { }

            //- xml["Plan"].Text = StringTagItem.getSelectedTag(cbPlan, "");
            /*-
            try
            {
                xml["PlanPrn"].Text = cbPlan.SelectedItem.ToString();
            }
            catch (Exception) { }
            */
            // получим регион симки
            DataTable dtRegion = ((IDEXData)toolbox).getQuery("select `region_id` from `um_data` WHERE `msisdn` = '" + mtbMSISDN.Text + "'");
            if (dtRegion != null && dtRegion.Rows.Count > 0)
            {
                xml["RegionId"].Text = dtRegion.Rows[0]["region_id"].ToString();
            }
           
            // если сим не GF, то как обычно, если GF, то получим план GF из выпадалки
            bool errGfPlan = false;
            //if (!isGF) 
            //{
            //    xml["IntPlanPrn"].Text = intPlanPrn;
            //    xml["Plan"].Text = plan;
            //
            //} 
            //else 
            //{
                //try
                //{
                    xml["IntPlanPrn"].Text = cbPlan.SelectedItem.ToString();
                    xml["Plan"].Text = StringTagItem.getSelectedTag(cbPlan, ""); // (((StringTagItem)cbPlan.SelectedItem).Tag);
                //} 
                //catch(Exception) 
                //{
                //    errGfPlan = true;
                //}
            //}
            

            /*-
            if (dtSbmsPlan != null && dtSbmsPlan.Rows.Count > 0)
            {
                SimpleXML xmlSbmsPlans = xml.CreateChild("SBMSPlans");
                foreach (DataRow row in dtSbmsPlan.Rows)
                {
                    SimpleXML xmlItem = xmlSbmsPlans.CreateChild("item");
                    xmlItem.Attributes["id"] = row["id"].ToString();
                    xmlItem.Attributes["title"] = row["title"].ToString();
                }
            }

            if (pctyp_id != null) xml["pctyp_id"].Text = pctyp_id;
            */

            xml["FirstName"].Text = tbFirstName.Text;
            xml["SecondName"].Text = tbSecondName.Text != "" ? tbSecondName.Text : "";
            xml["LastName"].Text = tbLastName.Text;
            xml["Sex"].Text = StringTagItem.getSelectedTag(cbSex, "");

            xml["FizBirthPlace"].Text = tbFizBirthPlace.Text;
            xml["Birth"].Text = deBirth.Text;

            xml["FizDocOrg"].Text = tbFizDocOrg.Text;

            xml["FizDocNumber"].Text = tbFizDocNumber.Text.Trim();
            xml["FizDocSeries"].Text = tbFizDocSeries.Text.Trim();


            xml["FizDocType"].Text = StringTagItem.getSelectedTag(cbFizDocType, "");
            xml["FizDocDate"].Text = deFizDocDate.Text;

            xml["FizDocOrgCode"].Text = tbFizDocOrgCode.Text;

            xml["FizDocScan"].Text = cbScan.SelectedIndex.ToString();
            xml["FizDocScanMime"].Text = lbScanMime.Text;

            xml["AddrCountry"].Text = StringTagItem.getSelectedTag(cbAddrCountry, "");
            xml["AddrState"].Text = tbAddrState.Text;

            xml["AddrCity"].Text = autocompleteFind(tbAddrCity.AutoCompleteCustomSource, tbAddrCity.Text, tbAddrCity.Text);

            xml["AddrZip"].Text = tbAddrZip.Text;
            xml["AddrRegion"].Text = tbAddrRegion.Text;
            xml["AddrStreet"].Text = tbAddrStreet.Text;
            xml["AddrHouse"].Text = tbAddrHouse.Text;
            xml["AddrBuilding"].Text = tbAddrBuilding.Text;
            xml["AddrApartment"].Text = tbAddrApartment.Text;

            xml["AddrPhone"].Text = tbAddrPhone.Text;
            xml["ContactEmail"].Text = tbContactEmail.Text;
            xml["FizInn"].Text = tbFizInn.Text;

            xml["DeliveryType"].Text = "3";//StringTagItem.getSelectedTag(cbDeliveryType, "");
            //xml["DeliveryPhone"].Text = tbDeliveryPhone.Text;
            //xml["DeliveryFax"].Text = tbDeliveryFax.Text;

            //xml["DeliveryCountry"].Text = StringTagItem.getSelectedTag(cbDeliveryCountry, "");
            //xml["DeliveryState"].Text = tbDeliveryState.Text;
            //xml["DeliveryCity"].Text = autocompleteFind(tbDeliveryCity.AutoCompleteCustomSource, tbDeliveryCity.Text, tbDeliveryCity.Text);
            //xml["DeliveryZip"].Text = tbDeliveryZip.Text;
            //xml["DeliveryRegion"].Text = tbDeliveryRegion.Text;
            //xml["DeliveryStreet"].Text = tbDeliveryStreet.Text;
            //xml["DeliveryHouse"].Text = tbDeliveryHouse.Text;
            //xml["DeliveryBuilding"].Text = tbDeliveryBuilding.Text;
            //xml["DeliveryApartment"].Text = tbDeliveryApartment.Text;



            xml["ProfileName"].Text = _ProfileName;
            xml["ProfileCode"].Text = _ProfileCode;

            xml["DealerName"].Text = _DealerName;
            xml["DealerCode"].Text = _DealerCode;
            if (DutyId != null) xml["DutyId"].Text = DutyId;

            if (StringTagItem.getSelectedTag(cbProf, "") != "")
            {
                //xml["sendNow"].Text = "true";
                //xml["ProfileName"].Text = tbProfileName.Text;
                //xml["ProfileCode"].Text = tbProfileCode.Text;



                xml["ProfileName"].Text = cbProf.SelectedItem.ToString();
                xml["ProfileCode"].Text = StringTagItem.getSelectedTag(cbProf, "");
            }
            //IDEXData d = (IDEXData)toolbox;
            /*
            bool statusPassport = true;
            if ( xml["FizDocType"].Text == "1" && comboPassport.SelectedIndex == 0 ) 
            {

                try
                {

                    //DataTable tu = d.getQuery("select count(id) from `wrong_passports` where value='{0}'", tbFizDocSeries.Text + tbFizDocNumber.Text);
                    DataTable tu = d.getQuery("select SQL_CALC_FOUND_ROWS value from `wrong_passports` where value='{0}'", tbFizDocSeries.Text + tbFizDocNumber.Text);
                    //if ( tu != null && tu.Rows.Count > 0 && Convert.ToInt32(tu.Rows[0]) > 0)
                    if (tu != null)
                    {
                        statusPassport = false;
                    }
                }
                catch ( Exception )
                {
                }
            }
            else if ( xml["FizDocType"].Text == "1" && comboPassport.SelectedIndex == 1 )
            {
                try
                {
                    string conn = "server=192.168.0.64;port=3306;user id=passport;Password=12473513;" +
                                  "persist security info=True;database=passports;charset=cp1251;" +
                                  "Default Command Timeout=60";
                    
                    MySqlConnection con_test = new MySqlConnection(conn);
                    
                    MySqlCommand cmd;
                    con_test.Open();
                    //cmd = new MySqlCommand("select count(id) from `wrong_passports_result` where value=@number", con_test);
                    cmd = new MySqlCommand("select SQL_CALC_FOUND_ROWS value from `wrong_passports_result` where value=@number", con_test);
                    cmd.Parameters.AddWithValue("@number", Convert.ToInt64(tbFizDocSeries.Text + tbFizDocNumber.Text));
                    cmd.ExecuteNonQuery();
                    MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                    DataTable t = new DataTable();
                    ada.Fill(t);
                    con_test.Close();
                    if ( Convert.ToInt64(t.Rows[0].ItemArray[0]) > 0 )
                    {
                        statusPassport = false;
                    }
                    



                    /*
                    string sql = string.Format("select count(id) from `wrong_passports_result` where value={0}", Convert.ToInt32(tbFizDocSeries.Text + tbFizDocNumber.Text));
                    string sql = string.Format("select count(id) from `wrong_passports_result` where value={0}", Convert.ToInt32(tbFizDocSeries.Text + tbFizDocNumber.Text));

                    MySqlConnection MSqlConnDummy = new MySqlConnection(conn);
                    MySqlCommand mSqlCmdSelectCustomers = MSqlConnDummy.CreateCommand();
                    mSqlCmdSelectCustomers.CommandText = @sql;
                    MySqlDataReader mSqlReader_Customers;
                    MSqlConnDummy.Open();
                    mSqlReader_Customers = mSqlCmdSelectCustomers.ExecuteReader();
                    DataTable dtCustomers = new DataTable();
                    dtCustomers.Load(mSqlReader_Customers);
                    
                    MSqlConnDummy.Close();
                    
                }
                catch ( Exception )
                {
                }
            }
            */

            fdocument.documentDate = deJournalDate.Value.ToString("yyyyMMddhhmmssfff");
            fdocument.documentText = SimpleXML.SaveXml(xml);
            ArrayList err = module.ValidateDocument(toolbox, fdocument);

            if (isOnline)
            {
                if (xml["ProfileName"].Text.Equals("") || xml["ProfileCode"].Text.Equals(""))
                {
                    if (xml["FizDocType"].Text != "1")
                    {
                        //err.Add("Регистрация возможна только на парпорт гражданина РФ");
                    }
                    if (xml["DocClientType"].Text != "1")
                    {
                        //err.Add("Регистрация возможна только на резидента");
                    }
                }
            }

            IDEXValidators dv = (IDEXValidators)toolbox;
            if (!dv.CheckPeriodDate(deJournalDate.Value))
            {
                if (err == null) err = new ArrayList();
                err.Add("Некорректная дата документа DEX");
            }
            if (errGfPlan) err.Add("Вы не выбрали тарифный план!\n");


            string vendor = "megafon";
            string vendorBase = "";
            string currentBase = ((IDEXUserData)toolbox).currentBase;
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
                if (ifChangeScan)
                {
                    try
                    {
                        // если есть скан, то сохраним
                        //string vendor = "megafon";
                        //string vendorBase = "";
                        if (isOnline)
                        {
                            //dexUid = ((IDEXUserData)toolbox).UID;
                            //querySelect += " and owner_id = '" + dexUid + "'";
                            currentBase = ((IDEXUserData)toolbox).currentBase;
                            JObject data = new JObject();
                            data["base"] = currentBase;
                            IDEXServices idis = (IDEXServices)toolbox;
                            JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexolToServName?data=" + JsonConvert.SerializeObject(data), 0));
                            //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                            if (int.Parse(obj["data"]["status"].ToString()) == 1)
                            {
                                vendorBase = obj["data"]["base"].ToString();
                            }
                        }
                        else
                        {
                            currentBase = ((IDEXUserData)toolbox).dataBase;
                            JObject data = new JObject();
                            data["base"] = currentBase;
                            IDEXServices idis = (IDEXServices)toolbox;
                            JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
                            //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                            if (int.Parse(obj["data"]["status"].ToString()) == 1)
                            {
                                vendorBase = obj["data"]["base"].ToString();
                            }
                        }

                        string mime = Path.GetExtension(lbScanPath.Text);

                        string uriString = "http://" + nodejsserver + ":3020/dexdealer/uploadScan";
                        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                        WebClient myWebClient = new WebClient();
                        myWebClient.Headers.Set("vendor", vendor);
                        myWebClient.Headers.Set("base", vendorBase);
                        myWebClient.Headers.Set("filename", fdocument.signature.ToString());
                        myWebClient.Headers.Set("mime", mime);
                        try
                        {
                            byte[] responseArray = myWebClient.UploadFile(uriString, "PUT", lbScanPath.Text);
                            xml["FizDocScan"].Text = "1";
                            xml["FizDocScanMime"].Text = mime;

                            fdocument.documentText = SimpleXML.SaveXml(xml);
                        }
                        catch (Exception) { }
                    }
                    catch (Exception) { }
                }
                else
                {
                    xml["FizDocScan"].Text = "1";
                    xml["FizDocScanMime"].Text = lbScanMime.Text;
                    fdocument.documentText = SimpleXML.SaveXml(xml);
                }

                // если данные подтверждены для добавления в автодок
                if (!isOnline)
                {
                    try
                    {
                        if (chDulIsCorrect.Checked)
                        {
                            if (xml["FizDocScan"].Text.Equals("1"))
                            {
                                IDEXData d = (IDEXData)toolbox;
                                DataTable um_d = d.getQuery("select region_id FROM `um_data` where icc='{0}' AND msisdn = '{1}'", xml["ICC"].Text, xml["MSISDN"].Text);
                                IDEXServices idis = (IDEXServices)toolbox;
                                string mime;

                                string cb = "";
                                JObject data = new JObject();

                                JObject obj;
                                JObject objInfoBase = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/adapters/getDexDexolBase?clientType=dexol", 1));
                                string pfBase = "";
                                if (isOnline)
                                {
                                    cb = ((IDEXUserData)toolbox).currentBase;
                                    foreach (JObject jo in objInfoBase["data"])
                                    {
                                        if (jo["dex_dexol_base_name"].ToString().Equals(cb))
                                        {
                                            pfBase = jo["tag"].ToString();
                                        }
                                    }
                                }
                                else
                                {

                                    cb = ((IDEXUserData)toolbox).dataBase;
                                    foreach (JObject jo in objInfoBase["data"])
                                    {
                                        if (jo["list"].ToString().Equals(cb))
                                        {
                                            pfBase = jo["tag"].ToString();
                                            currentBase = jo["dex_dexol_base_name"].ToString();
                                        }
                                    }
                                }
                                if (ifChangeScan)
                                {
                                    mime = Path.GetExtension(lbScanPath.Text);
                                }
                                else
                                {
                                    mime = lbScanMime.Text;
                                }
                                string fizDocTypeBase = "";
                                if (xml["FizDocType"].Text.Equals("104")) fizDocTypeBase = "inistr";
                                else if (xml["FizDocType"].Text.Equals("1")) fizDocTypeBase = "ru";
                                else fizDocTypeBase = "undef";
                                JObject packet = new JObject();
                                packet["com"] = "dexdealer.adapters.mega";
                                packet["subcom"] = "apiInsertPasspAutodoc";
                                packet["data"] = new JObject();
                                packet["data"]["base"] = currentBase;
                                packet["data"]["unitid"] = fdocument.documentUnitID;
                                packet["data"]["vendor"] = "mega";
                                packet["data"]["fullOperName"] = "megafon";
                                packet["data"]["signature"] = fdocument.signature;
                                packet["data"]["mime"] = mime;
                                packet["data"]["region"] = um_d.Rows.Count > 0 ? um_d.Rows[0]["region_id"].ToString() : "";
                                packet["data"]["fizDocTypeBase"] = fizDocTypeBase;
                                packet["data"]["xml"] = new JObject();

                                packet["data"]["xml"]["LastName"] = xml["LastName"].Text;
                                packet["data"]["xml"]["FirstName"] = xml["FirstName"].Text;
                                packet["data"]["xml"]["SecondName"] = xml["SecondName"].Text;
                                packet["data"]["xml"]["Birth"] = xml["Birth"].Text;
                                packet["data"]["xml"]["Sex"] = xml["Sex"].Text;
                                packet["data"]["xml"]["FizDocSeries"] = xml["FizDocSeries"].Text;
                                packet["data"]["xml"]["FizDocNumber"] = xml["FizDocNumber"].Text;
                                packet["data"]["xml"]["FizBirthPlace"] = xml["FizBirthPlace"].Text;
                                packet["data"]["xml"]["FizDocDate"] = xml["FizDocDate"].Text;
                                packet["data"]["xml"]["FizDocOrg"] = xml["FizDocOrg"].Text;
                                packet["data"]["xml"]["FizDocOrgCode"] = xml["FizDocOrgCode"].Text;
                                packet["data"]["xml"]["AddrCountry"] = xml["AddrCountry"].Text;
                                packet["data"]["xml"]["AddrState"] = xml["AddrState"].Text;
                                packet["data"]["xml"]["AddrCity"] = xml["AddrCity"].Text;
                                packet["data"]["xml"]["AddrRegion"] = xml["AddrRegion"].Text;
                                packet["data"]["xml"]["AddrStreet"] = xml["AddrStreet"].Text;
                                packet["data"]["xml"]["AddrHouse"] = xml["AddrHouse"].Text;
                                packet["data"]["xml"]["AddrBuilding"] = xml["AddrBuilding"].Text;
                                packet["data"]["xml"]["ContactEmail"] = xml["ContactEmail"].Text;
                                packet["data"]["xml"]["AddrZip"] = xml["AddrZip"].Text;
                                packet["data"]["xml"]["FizInn"] = xml["FizInn"].Text;
                                JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/megafon/cmd?packet=" + JsonConvert.SerializeObject(packet), 0));
                            }
                            else
                            {
                                MessageBox.Show("Скан не приложен, данные не добавлены в проверенный автодок");
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                IDEXConfig cfg = (IDEXConfig)toolbox;
                cfg.setStr(this.Name, "cbDocUnit", StringTagItem.getSelectedTag(cbDocUnit, ""));

                IDEXData tb = (IDEXData)toolbox;
                if (tbFirstName.Text.Trim() != "") tb.setDataHint("first_name", tbFirstName.Text);
                if (tbSecondName.Text.Trim() != "") tb.setDataHint("second_name", tbSecondName.Text);
                if (tbLastName.Text.Trim() != "") tb.setDataHint("last_name", tbLastName.Text);
                if (tbFizDocOrg.Text.Trim() != "") tb.setDataHint("fiz_doc_org", tbFizDocOrg.Text);
                if (tbAddrStreet.Text.Trim() != "") tb.setDataHint("street", tbAddrStreet.Text);
                if (tbAddrRegion.Text.Trim() != "") tb.setDataHint("region", tbAddrRegion.Text);
                if (tbAddrState.Text.Trim() != "") tb.setDataHint("state", tbAddrState.Text);
                if (tbAddrCity.Text.Trim() != "") tb.setDataHint("city", tbAddrCity.Text);

                fdocument.documentDigest = string.Format(
                    "{0}, {1} {2} {3}", mtbMSISDN.Text, tbLastName.Text, tbFirstName.Text, tbSecondName.Text
                    );
                DialogResult = DialogResult.OK;
            }
        }


        private void setPeopleDataToFields(StringList pdata)
        {
            tbFirstName.Text = pdata["FirstName"];
            tbSecondName.Text = pdata["SecondName"];
            tbLastName.Text = pdata["LastName"];

            StringTagItem.SelectByTag(cbDocClientType, pdata["DocClientType"], false);
            
            StringTagItem.SelectByTag(cbSex, pdata["Sex"], false);

            deBirth.Text = pdata["Birth"];
            tbFizBirthPlace.Text = pdata["FizBirthPlace"];

            StringTagItem.SelectByTag(cbFizDocType, pdata["FizDocType"], true);
            tbFizDocSeries.Text = pdata["FizDocSeries"];
            tbFizDocNumber.Text = pdata["FizDocNumber"];
            try { tbFizDocOrgCode.Text = pdata["FizDocOrgCode"]; }
            catch (Exception) { }
            
            tbFizDocOrg.Text = pdata["FizDocOrg"];
            deFizDocDate.Text = pdata["FizDocDate"];

            StringTagItem.SelectByTag(cbAddrCountry, pdata["AddrCountry"], false);
            tbAddrState.Text = pdata["AddrState"];
            tbAddrRegion.Text = pdata["AddrRegion"];
            tbAddrCity.Text = pdata["AddrCity"];
            tbAddrZip.Text = pdata["AddrZip"];
            tbAddrStreet.Text = pdata["AddrStreet"];
            tbAddrHouse.Text = pdata["AddrHouse"];
            tbAddrBuilding.Text = pdata["AddrBuilding"];
            tbAddrApartment.Text = pdata["AddrApartment"];
            tbAddrPhone.Text = pdata["AddrPhone"];
            tbContactEmail.Text = pdata["ContactEmail"];
            tbFizInn.Text = pdata["FizInn"];

            //StringTagItem.SelectByTag(cbDeliveryType, pdata["DeliveryType"], false);
            //tbDeliveryPhone.Text = pdata["DeliveryPhone"];
            //tbDeliveryFax.Text = pdata["DeliveryFax"];

            //StringTagItem.SelectByTag(cbDeliveryCountry, pdata["DeliveryCountry"], false);
            //tbDeliveryState.Text = pdata["DeliveryState"];
            //tbDeliveryRegion.Text = pdata["DeliveryRegion"];
            //tbDeliveryCity.Text = pdata["DeliveryCity"];
            //tbDeliveryZip.Text = pdata["DeliveryZip"];
            //tbDeliveryStreet.Text = pdata["DeliveryStreet"];
            //tbDeliveryHouse.Text = pdata["DeliveryHouse"];
            //tbDeliveryBuilding.Text = pdata["DeliveryBuilding"];
            //tbDeliveryApartment.Text = pdata["DeliveryApartment"];

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
                        setPeopleDataToFields(sl);

                        cbDocStatus.Focus();
                    }
                }
                else
                {
                    IDEXPeopleSearcher ps = (IDEXPeopleSearcher)toolbox;
                    setPeopleDataToFields(ps.getPeopleData((string)h[0]));
                }
            }
        }

        private void DocumentForm_Shown(object sender, EventArgs e)
        {
            deJournalDate.Focus();
        }

        private void mtbICC_TextChanged(object sender, EventArgs e)
        {
            int iccctl = DEXTools.calcIccCtl(mtbICC.Text);
            tbICCCTL.Text = iccctl > -1 ? iccctl.ToString() : "";
        }

        private void tbLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                ((Control)sender).Text = capitalize(((Control)sender).Text);
                SelectNextControl((Control)sender, true, true, true, false);
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

        private void zipToCity(TextBox tbCity, TextBox tbZip) 
        {
            try
            {
                tbCity.Text = capitalize(tbCity.Text);
                Dictionary<string, string> dic = ((IDEXCitySearcher)toolbox).getCityData("city", tbCity.Text);
                if (dic["zip"] != "")
                {
                    tbZip.Text = dic["zip"];
                }
            }
            catch (Exception)
            {
            }
        }

        private void tbAddrCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                zipToCity(tbAddrCity, tbAddrZip);
                //if (tbFizBirthPlace.Text.Trim().Equals("")) tbFizBirthPlace.Text = tbAddrCity.Text;
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void tbDeliveryCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                //zipToCity(tbDeliveryCity, tbDeliveryZip);
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void bDeliveryCopy_Click(object sender, EventArgs e)
        {
            //tbDeliveryPhone.Text = tbAddrPhone.Text;
            //StringTagItem.SelectByTag(cbDeliveryCountry, StringTagItem.getSelectedTag(cbAddrCountry, ""), false);
            //tbDeliveryState.Text = tbAddrState.Text;
            //tbDeliveryCity.Text = tbAddrCity.Text;
            //tbDeliveryZip.Text = tbAddrZip.Text;
            //tbDeliveryRegion.Text = tbAddrRegion.Text;
            //tbDeliveryStreet.Text = tbAddrStreet.Text;
            //tbDeliveryHouse.Text = tbAddrHouse.Text;
            //tbDeliveryBuilding.Text = tbAddrBuilding.Text;
            //tbDeliveryApartment.Text = tbAddrApartment.Text;
        }

        private void fillSim(Dictionary<string, string> dicsim)
        {
            try
            {
                lPlan.Text = "?";
                if (dicsim != null)
                {
                    //теперь определим GF или нет данная сим
                    bool bil = false;
                    try
                    {
                        bil = Convert.ToBoolean(Convert.ToString(dicsim["billing"]));
                    }
                    catch (Exception) {
                        bil = 1.ToString().Equals(dicsim["billing"]);
                    }

                    DataTable tidt;
                    //if (dicsim.ContainsKey("billing") && bil)
                   // {
                        lGfPlan.Visible = true;
                        cbPlan.Visible = true;
                        isGF = true;

                        cbProf.Visible = true;
                        bSetProf.Visible = true;
                        // тарифные планы для GF
                        try
                        {
                            DataTable tgf;
                            
                            cbPlan.Items.Clear();

                            bool fs = false;
                            try
                            {
                                fs = Convert.ToBoolean(dicsim["fs"]);
                            }
                            catch (Exception)
                            {
                                fs = 1.ToString().Equals(dicsim["fs"]);
                            }

                            if (fs)
                            {
                                tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf` WHERE allowed = '1' AND allow_m = '1' order by title DESC");       
                            }
                            else 
                            {
                                tgf = ((IDEXData)toolbox).getQuery("select * from `um_plans_gf` WHERE allow_m = '1' order by title DESC");
                            }
                            

                            StringTagItem.UpdateCombo(cbPlan, tgf, null, "plan_id", "title", false);
                            cbPlan.Items.Add(" ");
                            cbPlan.SelectedIndex = cbPlan.FindString(" ");

                            /*
                            tidt = ((IDEXData)toolbox).getQuery("select * from `gf_identity_doc_types` order by name");
                            StringTagItem.UpdateCombo(cbFizDocType, tidt, null, "identityDocTypeId", "name", false);
                            cbFizDocType.Items.Add(" ");
                            cbFizDocType.SelectedIndex = cbFizDocType.FindString(" ");
                            */
                        } catch(Exception) {}
                    /*
                    }
                    else
                    {
                        lGfPlan.Visible = false;
                        cbPlan.Visible = false;
                        cbProf.Visible = false;
                        bSetProf.Visible = false;

                        tidt = ((IDEXData)toolbox).getQuery("select * from `efd_identity_types` order by title");
                        StringTagItem.UpdateCombo(cbFizDocType, tidt, null, "id", "title", false);
                        cbFizDocType.Items.Add(" ");
                        cbFizDocType.SelectedIndex = cbFizDocType.FindString(" ");

                    }
                    */
                    if (dicsim.ContainsKey("msisdn")) mtbMSISDN.Text = dicsim["msisdn"];
                    if (dicsim.ContainsKey("icc")) mtbICC.Text = dicsim["icc"];
                    if (dicsim.ContainsKey("fs")) isFS = (true.ToString().Equals(dicsim["fs"]) || "1".Equals(dicsim["fs"]));
                    if (dicsim.ContainsKey("plan_id"))
                    {
                        try
                        {
                            plan = dicsim["plan_id"];
                            intPlanPrn = tPlan.Select("plan_id = '" + plan + "'")[0]["title"].ToString();
                            lPlan.Text = intPlanPrn;


                        }
                        catch (Exception) { }
                    }

                    if (!isOnline)
                    {
                        if (dicsim.ContainsKey("owner_title")) lOwner.Text = dicsim["owner_title"];
                        else lOwner.Text = "?";
                        if (dicsim.ContainsKey("region_title")) lOwner.Text += ", " + dicsim["region_title"];
                        if (dicsim.ContainsKey("owner_status")) lOwner.Text += ", " + ((bool.Parse(dicsim["owner_status"])) ? "[Активен]" : "[Заблокирован]");
                    }
                }

                /*
                //TODO Найти карту в сбмс и загрузить тариф
                // https://83.149.26.71:9443/CLIR_API/CLIR_API_LOGIN?LOGIN=DLREFD_NTELEKOM_09&PASSWORD=56gX7soa
                // https://83.149.26.71:9443/CLIR_API/CLIR_API_CHECK_MSISDN?SESSION_ID=AAAFswADgXTLRnTwA5jn2tL2YOKrUXba&pmsisdn_find=9280255850&picc_find=897010284319515166
                // Получаем psubs_id, pctyp_id. pccat_id берём из cbDocCategory. Если там не выбрано значение, берём из xml и в cbDocCategory выбираем его
                // https://83.149.26.71:9443/CLIR_API/CLIR_API_GET_RTPL_LIST?SESSION_ID=AAAFswADgXQ42zJAufk8GDyuDT.kuecs&psubs_id=46615336&pccat_id=1&pctyp_id=11
                // https://83.149.26.71:9443/CLIR_API/CLIR_GET_PCONTRACT_NEW_DATA_R?SESSION_ID=AAAFxAABNm1cOmJR5GFcNBu.8oQx3rk0&pmsisdn_find=9283223129&picc_find=897010284319237423            

                //msisdn, icc, 

                // psubs_id – идентификатор абонента (должен содержать только цифры); берется из операции CLIR_API_CHECK_MSISDN тэг <psubs_id>;
                // pccat_id (cbDocCategory) – идентификатор категории клиента (должен содержать только цифры); берется из операции CLIR_API_GET_NSI_DATA тэг <CLIENT_CATS>;
                // pctyp_id () – идентификатор типа клиента (должен содержать только цифры); берется из операции CLIR_API_GET_NSI_DATA тэг <CLIENT_TYPES>.

                string errmsg = null;
                try
                {
                    if (mtbMSISDN.Text.Length == 10 && DEXTools.calcIccCtl(mtbICC.Text) > -1)
                    {
                        string prtpl_id = StringTagItem.getSelectedTag(cbPlan, null);
                        if (dtSbmsPlan == null)
                        {
                            
                            SbmsSession sbms = new SbmsSession();
                            if (!sbms.connect(sbms_user, sbms_pass))
                                throw new Exception("Не удалось соединиться с сервером СБМС");

                            SimpleXML xml = sbms.httpRequest(
                                "CLIR_API_CHECK_MSISDN",
                                "pmsisdn_find", mtbMSISDN.Text
                                );

                            if (xml == null) throw new Exception("Не удалось выполнить запрос (1)");
                            if (sbms.lastErrorMessage != null) throw new Exception("Сообщение сервера СБМС (1): " + sbms.lastErrorMessage);
                            String xmldbg = SimpleXML.SaveXml(xml);

                            SimpleXML xml2 = xml.GetNodeByPath("CLIR_API_CHECK_MSISDN", false);
                            if (xml2 == null) throw new Exception("Сервер вернул неверный ответ на запрос (1)");
                            xmldbg = SimpleXML.SaveXml(xml2);

                            string psubs_id = xml2["psubs_id"].Text;
                            pctyp_id = xml2["pctyp_id"].Text;
                            string pccat_id = StringTagItem.getSelectedTag(cbDocCategory, null);
                            if (pccat_id == null)
                            {
                                pccat_id = xml2["pccat_id"].Text;
                                StringTagItem.SelectByTag(cbDocCategory, pccat_id, false);
                            }

                            if (prtpl_id == null) prtpl_id = xml2["prtpl_id"].Text;

                            xml = sbms.httpRequest("CLIR_API_GET_RTPL_LIST", "psubs_id", psubs_id, "pccat_id", pccat_id, "pctyp_id", pctyp_id);
                            if (xml == null) throw new Exception("Не удалось выполнить запрос (2)");
                            if (sbms.lastErrorMessage != null) throw new Exception("Сообщение сервера СБМС (2): " + sbms.lastErrorMessage);
                            xmldbg = SimpleXML.SaveXml(xml);

                            xml2 = xml.GetNodeByPath("CLIR_API_GET_RTPL_LIST", false);
                            if (xml2 == null) throw new Exception("Сервер вернул неверный ответ на запрос (2)");
                            xmldbg = SimpleXML.SaveXml(xml2);

                            dtSbmsPlan = sbms.xml2table(xml2, "RTPL_");
                             
                        }

                        StringTagItem.UpdateCombo(cbPlan, dtSbmsPlan, null, "id", "title", true);
                        StringTagItem.SelectByTag(cbPlan, prtpl_id, false);
                            
                    }
                }
                catch (Exception ex) 
                {
                    errmsg = ex.Message;
                }

                if (errmsg != null) MessageBox.Show(errmsg);
                 */
            }
            catch (Exception)
            {
            }
        }

        private void bGetDocNum_Click(object sender, EventArgs e)
        {
            if (docnum == "")
            {

                IDEXData d = (IDEXData)toolbox;
                IDEXCrypt cr = (IDEXCrypt)toolbox;
                string iid = d.EscapeString( cr.StringToMD5(DateTime.Now.ToString("yyyyMMddHHmmssfff") + ((IDEXUserData)toolbox).MAC));
                d.runQuery(
                    "update `efd_docnum` set owner = '{0}' where id = " +
                    "(SELECT id FROM (SELECT id FROM `efd_docnum` WHERE owner = '' LIMIT 0, 1) AS dummytable)",
                    iid
                    );

                DataTable dt = d.getQuery("select * from `efd_docnum` where owner = '{0}'", iid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    docnum = dt.Rows[0]["docnum"].ToString();
                }

                d.runQuery("delete from `efd_docnum` where owner = '{0}'", iid);
            }

            tbDocNum.Text = docnum;
        }

        private void bSetProf_Click(object sender, EventArgs e)
        {
            try
            {
                SetProf sp = new SetProf();
                string code;
                if (sp.ShowDialog(this) == DialogResult.OK)
                {
                    code = sp.tbCode.Text;
                    if (code != "")
                    {
                        // тарифные планы для GF
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        dict.Add("555879", "ofis_sk_new");
                        dict.Add("457258", "pyat");
                        dict.Add("457825", "pyat2");
                        dict.Add("457826", "pyat3");
                        dict.Add("652478", "kara3");
                        dict.Add("965471", "kara4");
                        if (dict.ContainsKey(code))
                        {
                            StringTagItem.SelectByTag(cbProf, dict[code], false);
                        }

                    }
                }
                sp.Dispose();
            }
            catch (Exception) { }
        }

        private void bAddScanDocument_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OPF = new OpenFileDialog();
                OPF.Filter = "Files(*.JPG;*.JPEG;*.PNG;*.PDF)|*.JPG;*.JPEG;*.PNG;*.PDF";
                //OPF.Filter = "Image Files(*.JPEG)|*.JPEG";
                OPF.Multiselect = false;
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    lbScanPath.Text = OPF.FileName;
                    ifChangeScan = true;
                    ifScanIsset = false;
                    lbIfScanChange.Visible = true;
                    lbIfScanChange.Text = "Приложен для отправки";
                    lbIfScanChange.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception)
            {
                lbIfScanChange.Visible = true;
                lbIfScanChange.Text = "Ошибка во время вложения";
                lbIfScanChange.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void bShowScan_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor = "megafon";
                string vendorBase = "";

                IDEXServices idis = (IDEXServices)toolbox;
                if (isOnline)
                {
                    //dexUid = ((IDEXUserData)toolbox).UID;
                    //querySelect += " and owner_id = '" + dexUid + "'";
                    string currentBase = ((IDEXUserData)toolbox).currentBase;
                    JObject data = new JObject();
                    data["base"] = currentBase;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexolToServName?data=" + JsonConvert.SerializeObject(data), 0));
                    //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                    if (int.Parse(obj["data"]["status"].ToString()) == 1)
                    {
                        vendorBase = obj["data"]["base"].ToString();
                    }
                }
                else
                {
                    string currentBase = ((IDEXUserData)toolbox).dataBase;
                    JObject data = new JObject();
                    data["base"] = currentBase;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
                    //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                    if (int.Parse(obj["data"]["status"].ToString()) == 1)
                    {
                        vendorBase = obj["data"]["base"].ToString();
                    }
                }

                JObject ifScan = new JObject();
                ifScan["vendor"] = vendor;
                ifScan["base"] = vendorBase;
                ifScan["signature"] = signatire.Text;
                ifScan["mime"] = lbScanMime.Text;
                JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/ifIssetScan?data=" + JsonConvert.SerializeObject(ifScan), 0));
                if (o["data"]["status"].ToString().Equals("1"))
                {

                    //Bitmap MyImage;
                    //image = new Bitmap(open_dialog.FileName);
                    //pictureBox = new pictureBox();
                    string dd = "http://"+nodejsserver+":3020/static/" + vendor + "/" + vendorBase + "/" + signatire.Text + "" + lbScanMime.Text;
                    try
                    {
                        if (lbScanMime.Text.Equals(".pdf") || lbScanMime.Text.Equals(".PDF"))
                        {
                            System.Diagnostics.Process.Start(dd);
                        }
                        else
                        {

                            ScanForm sf = new ScanForm(dd, lbScanMime.Text);
                            sf.Show();
                        }

                        //pictureBox1.Load("http://37.29.115.178:3020/static/" + vendor + "/" + vendorBase + "/" + signatire.Text+".jpeg");
                        //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        //MyImage = new Bitmap(fileToDisplay);
                        //pictureBox1.ClientSize = new Size(xSize, ySize);
                        //pictureBox1.Image = (Image) MyImage ;
                    }
                    catch (Exception)
                    {
                        // log that the download was not successful
                    }

                }
                else
                {
                    MessageBox.Show("К данному договору не прикреплен скан документа");
                }
                string sss = "dfv";
            }
            catch (Exception) { }
        }

        private void bSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor = "megafon";
                string vendorBase = "";

                IDEXServices idis = (IDEXServices)toolbox;
                if (isOnline)
                {
                    //dexUid = ((IDEXUserData)toolbox).UID;
                    //querySelect += " and owner_id = '" + dexUid + "'";
                    string currentBase = ((IDEXUserData)toolbox).currentBase;
                    JObject data = new JObject();
                    data["base"] = currentBase;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexolToServName?data=" + JsonConvert.SerializeObject(data), 0));
                    //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                    if (int.Parse(obj["data"]["status"].ToString()) == 1)
                    {
                        vendorBase = obj["data"]["base"].ToString();
                    }
                }
                else
                {
                    string currentBase = ((IDEXUserData)toolbox).dataBase;
                    JObject data = new JObject();
                    data["base"] = currentBase;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
                    //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                    if (int.Parse(obj["data"]["status"].ToString()) == 1)
                    {
                        vendorBase = obj["data"]["base"].ToString();
                    }
                }

                JObject ifScan = new JObject();
                ifScan["vendor"] = vendor;
                ifScan["base"] = vendorBase;
                ifScan["signature"] = signatire.Text;
                ifScan["mime"] = lbScanMime.Text;
                JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/ifIssetScan?data=" + JsonConvert.SerializeObject(ifScan), 0));
                if (o["data"]["status"].ToString().Equals("1"))
                {
                    string dd = "http://"+nodejsserver+":3020/static/" + vendor + "/" + vendorBase + "/" + signatire.Text + "" + lbScanMime.Text;
                    if (lbScanMime.Text.ToLower().Equals(".pdf"))
                    {
                        System.Diagnostics.Process.Start(dd);
                    }
                    else
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        //saveFileDialog1.Filter = ;  
                        saveFileDialog1.Filter = "File " + lbScanMime.Text + " |*" + lbScanMime.Text; 
                        saveFileDialog1.Title = "Сохранение скана документа";
                        saveFileDialog1.FileName = signatire.Text + "" + lbScanMime.Text;
                        saveFileDialog1.ShowDialog();

                        // If the file name is not an empty string open it for saving.  
                        //if (saveFileDialog1.FileName != "")
                        //{

                        //}
                        WebClient client = new WebClient();
                        Uri uri = new Uri(dd);
                        client.DownloadFileAsync(uri, saveFileDialog1.FileName);
                    }
                    //Console.WriteLine("Картинка скачана");
                    //Console.Read();

                }
                else
                {
                    MessageBox.Show("К договору не прикреплен скан");
                }

            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                JObject packet = new JObject();
                packet["data"] = new JObject();
                packet["data"]["code"] = tbFizDocOrgCode.Text;
                packet["data"]["title"] = tbFizDocOrg.Text;
                IDEXServices idis = (IDEXServices)toolbox;
                JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/setNewDocOrg?data=" + JsonConvert.SerializeObject(packet), 0));
            }
            catch (Exception) { }
        }

        private void tbFizDocOrgCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    IDEXServices idis = (IDEXServices)toolbox;
                    JObject fizDocOrg = new JObject();
                    fizDocOrg["data"] = new JObject();
                    fizDocOrg["data"]["code"] = tbFizDocOrgCode.Text;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/getFizDocOrgBase?data=" + JsonConvert.SerializeObject(fizDocOrg) + "&uid=&clientType=dexol&dexolUid=", 1));

                    //cbFizDocOrg.Items.Clear();
                    //cbFizDocOrg.Text = "";

                    if (obj["data"]["list"].Count() == 0)
                    {
                    }
                    else if (obj["data"]["list"].Count() == 1)
                    {
                        tbFizDocOrg.Text = obj["data"]["list"][0]["title"].ToString();
                    }
                    else
                    {
                        SelectFizDocOrg sfdo = new SelectFizDocOrg((JArray)obj["data"]["list"]);

                        if (sfdo.ShowDialog() == DialogResult.OK)
                        {
                            tbFizDocOrg.Text = sfdo.cbOrgs.Text;
                        }
                    }
                }
                catch (Exception) { }
                tbFizDocOrg.Focus();
            }
            
        }

      

        private void cbPlan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                //SelectNextControl((Control)sender, true, true, true, false);
                tbLastName.Focus();
            }
        }


    


 

      

    }

}
