using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using DEXExtendLib;
using DEXSIM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DEXPlugin.Document.MTS.Jeans
{
    public partial class DocumentForm : Form
    {
        public Object toolbox;
        public Document module;
        IDEXSim sim;
     
        //public string test;

        Dictionary<string, string> dUidForeign = new Dictionary<string, string>();

        IDEXDocumentData fsource, fdocument;
        bool ReadOnly;
        bool isOnline;
        string DutyId = null;

        bool ifChangeScan = false;
        bool ifScanIsset = false;
        string nodejsserver = "";

        // Алгоритм Луна
        public string getIccCtl(string icc)
        {
            int sum = 0;
            for (int i = 0; i < icc.Length; ++i)
            {
                int cur = int.Parse(icc.Substring(i, 1));
                if (i % 2 == 0)
                {
                    cur *= 2;
                    if (cur > 9) cur -= 9;
                }
                sum += cur;
            }

            sum = 10 - (sum % 10);
            if (sum == 10) sum = 0;
            return sum.ToString();
        }

        public DocumentForm()
        {
            InitializeComponent();
        }

        public void InitDictionaries()
        {
            DataTable tPlan = ((IDEXData)toolbox).getTable("um_plans");
            tPlan.DefaultView.Sort = "title";
            StringTagItem.UpdateCombo(cbPlan, tPlan, null, "plan_id", "title", false);
            InitComboDictionary(tbFizDocOrg, "fiz_doc_org"); //

            DataTable tDocType = ((IDEXData)toolbox).getTable("mts_doctype");
            tDocType.DefaultView.Sort = "title";
            StringTagItem.UpdateCombo(cbFizDocType, tDocType, null, "doctype_id", "title", false);

            //DataTable allDpCode = ((IDEXData)toolbox).getQuery("select * from `mts_dp_all` where kind = 1 order by dpcode");
            //StringTagItem.UpdateCombo(cbAllDpCode, allDpCode, "", "id", "dpcode", false);
            
            InitComboDictionary(tbAddrStreet, "street"); //
            InitComboDictionary(tbAddrState, "state");
            InitComboCity(tbAddrCity, "city");
            DataTable tDocCountry = ((IDEXData)toolbox).getTable("mts_doccountry");
            tDocCountry.DefaultView.Sort = "title";
            StringTagItem.UpdateCombo(cbFizDocCountry, tDocCountry, null, "doccountry_id", "title", false);
            StringTagItem.UpdateCombo(cbFizDocСitizen, tDocCountry, null, "doccountry_id", "title", false);
        }


        public void InitDocument(IDEXDocumentData source, IDEXDocumentData document, bool clone, bool ReadOnly)
        {
            isOnline = ((IDEXUserData)toolbox).isOnline;
            
            this.ReadOnly = ReadOnly;
            bOk.Visible = !ReadOnly;
            IDEXServices srv = (IDEXServices)toolbox;
            sim = (IDEXSim)srv.GetService("sim");

            if (!isOnline)
            {
                cbControl.Enabled = true;
            }
            else
            {
                cbControl.Enabled = false;
            }
            

            fsource = source;
            fdocument = document;

            // 1 = очистка полей

            label1.Visible = !isOnline;
            cbDocUnit.Visible = !isOnline;

            if (!isOnline)
            {
                DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");

                dUidForeign.Clear();
                
                if (t != null && t.Rows.Count > 0)
                {
                    foreach (DataRow row in t.Rows)
                    {
                        try
                        {
                            string fid = row["foreign_id"].ToString().Trim();
                            
                            if (fid.Length > 0) dUidForeign[row["uid"].ToString()] = fid;
                        }
                        catch (Exception) { }
                    }
                }

                StringTagItem.UpdateCombo(cbDocUnit, t, null, "uid", "title", false);
                //StringTagItem.UpdateCombo(cbUnittitlesim, t, null, "uid", "title", false);

                cbDocUnit.Text = "";
                //cbUnittitlesim.Text = "";
            }

            if (isOnline)
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

            mtbAssignedDPCode.Mask = ((IDEXConfig)toolbox).getRegisterStr("mts_dpcode_mask", "00000");
            mtbAssignedDPCode.Text = "";


            //cbAllDpCode.Mask = ((IDEXConfig)toolbox).getRegisterStr("mts_dpcode_mask", "00000");
            //cbAllDpCode.Text = "";

            deDocDate.Value = DateTime.Now;
            mtbMSISDN.Text = "";
            mtbMSISDN.Mask = ((IDEXConfig)toolbox).getRegisterStr("mts_msisdn_mask", "0000000000");
            mtbICC.Text = "";
            mtbICC.Mask = ((IDEXConfig)toolbox).getRegisterStr("mts_icc_mask", "0000000000000000000");
            tbICCCTL.Text = "0";
            tbFizInn.Text = "";
            cbDocCategory.SelectedIndex = 0;
            tbCodeWord.Text = "";

            tbLastName.Text = "";
            tbFirstName.Text = "";
            tbSecondName.Text = "";
            deBirth.Text = "";
            cbSex.SelectedIndex = -1;

            tbAddrZip.Text = "";
            tbAddrState.Text = "";
            tbAddrCity.Text = "";
            tbAddrStreet.Text = "";
            tbAddrHouse.Text = "";
            tbAddrBuilding.Text = "";
            tbAddrApartment.Text = "";

            tbFizBirthPlace.Text = "";
            tbFizDocSeries.Text = "";
            tbFizDocNumber.Text = "";
            deFizDocDate.Text = "";

            InitDictionaries();
            DutyId = null;

            lOwner.Text = "";

            LoadDefaultsToControls();

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

            fdocument.documentDate = DateTime.Now.ToString("yyyyMMddhhmmssfff");

            if (fsource != null)
            {
                try
                {
                    // 2 = Загрузка полей из документа

                    SimpleXML xml = SimpleXML.LoadXml(fsource.documentText);

                    StringTagItem.SelectByTag(cbDocUnit, fsource.documentUnitID.ToString(), true);
                    mtbAssignedDPCode.Text = xml.GetNodeByPath("AssignedDPCode", true).Text;

                    deDocDate.Text = xml.GetNodeByPath("DocDate", true).Text;
                    mtbMSISDN.Text = xml.GetNodeByPath("MSISDN", true).Text;
                    mtbICC.Text = xml.GetNodeByPath("ICC", true).Text;
                    tbICCCTL.Text = xml.GetNodeByPath("ICCCTL", true).Text;
                    StringTagItem.SelectByTag(cbPlan, xml.GetNodeByPath("Plan", true).Text, true);
                    tbFizInn.Text = xml.GetNodeByPath("FizInn", true).Text;

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
                    signatire.Text = fsource.signature;

                    try
                    {
                        cbDocCategory.SelectedIndex = int.Parse(xml.GetNodeByPath("DocCategory", true).Text);
                    }
                    catch (Exception) { }

                    tbCodeWord.Text = xml.GetNodeByPath("CodeWord", true).Text;
                    tbLastName.Text = xml.GetNodeByPath("LastName", true).Text;
                    tbFirstName.Text = xml.GetNodeByPath("FirstName", true).Text;
                    tbSecondName.Text = xml.GetNodeByPath("SecondName", true).Text;
                    deBirth.Text = xml.GetNodeByPath("Birth", true).Text;

                    try
                    {
                        cbSex.SelectedIndex = int.Parse(xml.GetNodeByPath("Sex", true).Text);
                    }
                    catch (Exception) { }

                    try
                    {
                        //StringTagItem.SelectByTag(cbUnittitlesim, xml.GetNodeByPath("Unittitlesim", true).Text, true);
                    } catch(Exception) 
                    {
                    }

                    tbAddrZip.Text = xml.GetNodeByPath("AddrZip", true).Text;
                    tbAddrState.Text = xml.GetNodeByPath("AddrState", true).Text;
                    tbAddrCity.Text = xml.GetNodeByPath("AddrCity", true).Text;
                    tbAddrStreet.Text = xml.GetNodeByPath("AddrStreet", true).Text;
                    tbAddrHouse.Text = xml.GetNodeByPath("AddrHouse", true).Text;
                    tbAddrBuilding.Text = xml.GetNodeByPath("AddrBuilding", true).Text;
                    tbAddrApartment.Text = xml.GetNodeByPath("AddrApartment", true).Text;

                    tbFizBirthPlace.Text = xml.GetNodeByPath("FizBirthPlace", true).Text;
                    StringTagItem.SelectByTag(cbFizDocCountry, xml.GetNodeByPath("FizDocCountry", true).Text, true);

                    if (xml.GetNodeByPath("FizDocCitizen", false) != null && xml.GetNodeByPath("FizDocCitizen", true).Text != "")
                    {
                        try
                        {
                            StringTagItem.SelectByTag(cbFizDocСitizen, xml.GetNodeByPath("FizDocCitizen", true).Attributes["tag"], true);
                        }
                        catch (Exception e) { }
                        
                    }

                    
                    StringTagItem.SelectByTag(cbFizDocType, xml.GetNodeByPath("FizDocType", true).Text, true);


                    try
                    {
                        //cbAllDpCode.Text = xml.GetNodeByPath("AssignedDPCode", true).Text;
                        //if (xml.GetNodeByPath("AssignedDPCode", true).Text != "")
                        //{
                            //cbAllDpCode.SelectedIndex = cbAllDpCode.FindString(xml.GetNodeByPath("AssignedDPCode", true).Text);
                        //}
                        //if (cbAllDpCode.SelectedIndex == -1) 
                        //{
                        //    cbAllDpCode.Items.Add(xml.GetNodeByPath("AssignedDPCode", true).Text);
                        //    cbAllDpCode.SelectedIndex = cbAllDpCode.FindString(xml.GetNodeByPath("AssignedDPCode", true).Text);
                        //}
                    } catch (Exception) 
                    {
                    }
                   


                    if ( int.Parse(xml.GetNodeByPath("DocCategory", true).Text) == 0 )
                    {
                        tbFizDocSeries.Text = xml.GetNodeByPath("FizDocSeries", true).Text;
                        tbFizDocNumber.Text = xml.GetNodeByPath("FizDocNumber", true).Text;
                       
                    }
                    else if ( int.Parse(xml.GetNodeByPath("DocCategory", true).Text) == 1 )
                    {
                        tbFizDocSeries.Text = xml.GetNodeByPath("FizDocSeriesNotRez", true).Text;
                        tbFizDocNumber.Text = xml.GetNodeByPath("FizDocNumberNotRez", true).Text;
                    }

                    



                    deFizDocDate.Text = xml.GetNodeByPath("FizDocDate", true).Text;
                    tbFizDocOrg.Text = xml.GetNodeByPath("FizDocOrg", true).Text;
                    tbFizDocOrgCode.Text = xml.GetNodeByPath("FizDocOrgCode", true).Text;
                    if (xml.GetNodeByPath("DutyId", false) != null) DutyId = xml["DutyId"].Text;

                    deFizDocExp.Text = xml.GetNodeByPath("FizDocExp", true).Text;

                    lOwner.Text = ""; //TODO
                    try
                    {
                        cbDocStatus.SelectedIndex = fsource.documentStatus;
                    }
                    catch (Exception)
                    {
                    }
                    if (!clone) fdocument.documentDate = fsource.documentDate;
                }
                catch (Exception) { }
            }

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

        private void DocumentForm_Shown(object sender, EventArgs e)
        {
            deJournalDate.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (ReadOnly) return;

            fdocument.documentStatus = cbDocStatus.SelectedIndex;

            int unid = -1;

            try
            {
                if (!isOnline)
                {
                    unid = int.Parse(((StringTagItem)cbDocUnit.SelectedItem).Tag);
                }
                else
                {
                    unid = int.Parse(((IDEXUserData)toolbox).UID);
                }
            }
            catch (Exception) { }

            fdocument.documentUnitID = unid;

            // 3 = Сохранение документа

            SimpleXML xml = new SimpleXML("Document");
            xml.Attributes["ID"] = module.ID;
            xml.GetNodeByPath("AssignedDPCode", true).Text = mtbAssignedDPCode.Text;

            xml.GetNodeByPath("DPCodeKind", true).Text = "НП";

            xml["isOnline"].Text = "0";
            if (isOnline) xml["isOnline"].Text = "1";

            IDEXData d = (IDEXData)toolbox;

            DataTable dt = d.getQuery("select kind from `mts_units_dp` where uid = " + fdocument.documentUnitID + " and dpcode = '" + d.EscapeString(mtbAssignedDPCode.Text) + "'");
            
            if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["kind"]) == 1)
            {
                xml.GetNodeByPath("DPCodeKind", true).Text = "П";
            }

            xml.GetNodeByPath("DocDate", true).Text = deDocDate.Text;
            xml.GetNodeByPath("MSISDN", true).Text = mtbMSISDN.Text;
            xml.GetNodeByPath("ICC", true).Text = mtbICC.Text;
            xml.GetNodeByPath("ICCCTL", true).Text = tbICCCTL.Text;

            //xml.GetNodeByPath("Unittitlesim", true).Text = ((StringTagItem)cbUnittitlesim.SelectedItem).Tag;

            try
            {
                xml["Control"].Text = Convert.ToInt32(cbControl.Checked).ToString();
            }
            catch (Exception) {
                xml["Control"].Text = "0";
            }

            try
            {
                xml.GetNodeByPath("Plan", true).Text = ((StringTagItem)cbPlan.SelectedItem).Tag;
            }
            catch (Exception)
            {
                xml.GetNodeByPath("Plan", true).Text = "-";
            }

            xml.GetNodeByPath("FizInn", true).Text = tbFizInn.Text;
            xml.GetNodeByPath("DocCategory", true).Text = cbDocCategory.SelectedIndex.ToString();
            xml.GetNodeByPath("CodeWord", true).Text = tbCodeWord.Text;
            xml.GetNodeByPath("LastName", true).Text = tbLastName.Text;
            xml.GetNodeByPath("FirstName", true).Text = tbFirstName.Text;
            xml.GetNodeByPath("SecondName", true).Text = tbSecondName.Text;
            xml.GetNodeByPath("Birth", true).Text = deBirth.Text;
            xml.GetNodeByPath("Sex", true).Text = cbSex.SelectedIndex.ToString();

            xml.GetNodeByPath("AddrZip", true).Text = tbAddrZip.Text;
            xml.GetNodeByPath("AddrState", true).Text = tbAddrState.Text;
            xml.GetNodeByPath("AddrCity", true).Text = tbAddrCity.Text;
            xml.GetNodeByPath("AddrStreet", true).Text = tbAddrStreet.Text;
            xml.GetNodeByPath("AddrHouse", true).Text = tbAddrHouse.Text;
            xml.GetNodeByPath("AddrBuilding", true).Text = tbAddrBuilding.Text;
            xml.GetNodeByPath("AddrApartment", true).Text = tbAddrApartment.Text;

            //xml.GetNodeByPath("AssignedDPCode", true).Text = cbAllDpCode.Text;

            xml.GetNodeByPath("FizBirthPlace", true).Text = tbFizBirthPlace.Text;
            try
            {
                xml.GetNodeByPath("FizDocCountry", true).Text = ((StringTagItem)cbFizDocCountry.SelectedItem).Tag;
                xml.GetNodeByPath("FizDocCountry", true).Attributes["printable"] = ( (StringTagItem)cbFizDocCountry.SelectedItem ).Text;


                xml.GetNodeByPath("FizDocCitizen", true).Attributes["tag"] = ( (StringTagItem)cbFizDocСitizen.SelectedItem ).Tag;
                xml.GetNodeByPath("FizDocCitizen", true).Text = ( (StringTagItem)cbFizDocСitizen.SelectedItem ).Text;


            }
            catch (Exception) { }
            try
            {
                xml.GetNodeByPath("FizDocType", true).Text = ((StringTagItem)cbFizDocType.SelectedItem).Tag;
                xml.GetNodeByPath("FizDocType", true).Attributes["printable"] = ((StringTagItem)cbFizDocType.SelectedItem).Text;
            }
            catch (Exception) { }

            if ( cbDocCategory.SelectedIndex.ToString() == "0" )
            {
                xml.GetNodeByPath("FizDocSeries", true).Text = tbFizDocSeries.Text;
                xml.GetNodeByPath("FizDocNumber", true).Text = tbFizDocNumber.Text;
                xml.GetNodeByPath("FizDocSeriesNotRez", true).Text = "";
                xml.GetNodeByPath("FizDocNumberNotRez", true).Text = "";
            }
            else if ( cbDocCategory.SelectedIndex.ToString() == "1") 
            {
                xml.GetNodeByPath("FizDocSeries", true).Text = tbFizDocSeries.Text;
                xml.GetNodeByPath("FizDocNumber", true).Text = tbFizDocNumber.Text;
                xml.GetNodeByPath("FizDocSeriesNotRez", true).Text = tbFizDocSeries.Text;
                xml.GetNodeByPath("FizDocNumberNotRez", true).Text = tbFizDocNumber.Text;
            }
            
            xml.GetNodeByPath("FizDocDate", true).Text = deFizDocDate.Text;
            xml.GetNodeByPath("FizDocOrg", true).Text = tbFizDocOrg.Text;
            xml.GetNodeByPath("FizDocOrgCode", true).Text = tbFizDocOrgCode.Text;
            //xml["FizDocScan"].Text = cbFizDocScan.Checked ? "X" : "-";
            if (DutyId != null) xml["DutyId"].Text = DutyId;

            if (!((StringTagItem)cbFizDocType.SelectedItem).Tag.Equals("21")) xml.GetNodeByPath("FizDocExp", true).Text = deFizDocExp.Text;

            fdocument.documentDate = deJournalDate.Value.ToString("yyyyMMddhhmmssfff");
            fdocument.documentText = SimpleXML.SaveXml(xml);
            ArrayList err = module.ValidateDocument(toolbox, fdocument);

            string vendor = "mts";
            string vendorBase = "";
            string currentBase = ((IDEXUserData)toolbox).currentBase;


            IDEXValidators dv = (IDEXValidators)toolbox;
            if (!dv.CheckPeriodDate(deJournalDate.Value))
            {
                if (err == null) err = new ArrayList();
                err.Add("Некорректная дата документа DEX");
            }

            if (!((StringTagItem)cbFizDocType.SelectedItem).Tag.Equals("21"))
            {
                if (!ifScanIsset && !ifChangeScan) 
                {
                    err.Add("Вы должны приложить скан документа");
                }
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
                if (ifChangeScan)
                {
                    try
                    {
                        // если есть скан, то сохраним

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
                        catch (Exception)
                        {
                            MessageBox.Show("Операция передачи скана серверу DEX произошла с ошибкой. Проверьте корректность приложенного документа!(Открыть сохраненный договор и нажать кнопку \"Показать\" в месте " +
                                "прикладывания скана)");
                        }
                    }
                    catch (Exception) {
                        string s = "";
                    }
                }
                else if (ifScanIsset == true)
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
                                if (xml["FizDocType"].Text.Equals("10")) fizDocTypeBase = "inistr";
                                else if (xml["FizDocType"].Text.Equals("21")) fizDocTypeBase = "ru";
                                else fizDocTypeBase = "undef";
                                JObject packet = new JObject();
                                packet["com"] = "dexdealer.adapters.mts";
                                packet["subcom"] = "apiInsertPasspAutodoc";
                                packet["data"] = new JObject();
                                packet["data"]["base"] = currentBase;
                                packet["data"]["unitid"] = fdocument.documentUnitID;
                                packet["data"]["vendor"] = "mts";
                                packet["data"]["fullOperName"] = "mts";
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
                                packet["data"]["xml"]["FizDocExp"] = xml["FizDocExp"].Text;
                                
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
                                if (!((StringTagItem)cbFizDocType.SelectedItem).Tag.Equals("21")) packet["data"]["xml"]["FizDocExp"] = deFizDocExp.Text;
                                JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/mts/cmd?packet=" + JsonConvert.SerializeObject(packet), 0));
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

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["zip"] = tbAddrZip.Text;
                dic["city"] = tbAddrCity.Text;
                dic["region"] = tbAddrState.Text;
                ((IDEXCitySearcher)toolbox).setCityData(dic);

                IDEXData tb = (IDEXData)toolbox;
                if (tbFirstName.Text.Trim() != "")
                    tb.setDataHint("first_name", tbFirstName.Text);
                if (tbSecondName.Text.Trim() != "")
                    tb.setDataHint("second_name", tbSecondName.Text);
                if (tbLastName.Text.Trim() != "")
                    tb.setDataHint("last_name", tbLastName.Text);
                if (tbFizDocOrg.Text.Trim() != "")
                    tb.setDataHint("fiz_doc_org", tbFizDocOrg.Text);
                if (tbAddrStreet.Text.Trim() != "")
                    tb.setDataHint("street", tbAddrStreet.Text);
                if (tbAddrState.Text.Trim() != "")
                    tb.setDataHint("state", tbAddrState.Text);
                if (tbAddrCity.Text.Trim() != "")
                    tb.setDataHint("city", tbAddrCity.Text);

                //TODO Сделать добавление абонента в people

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

        private void cbDocUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (sender == cbDocUnit)
                {
                    string tag = StringTagItem.getSelectedTag(cbDocUnit, StringTagItem.VALUE_ANY);

                    if (!StringTagItem.VALUE_ANY.Equals(tag) && dUidForeign.ContainsKey(tag))
                    {
                        mtbAssignedDPCode.Text = dUidForeign[tag];
                    }
                }
                if ( sender == cbDocCategory )
                {
                    
                   if ( ( (ComboBox)sender ).SelectedItem.ToString() == "Нерезидент" )
                   {
                       //cbFizDocType.Enabled = false;
                       StringTagItem.SelectByTag(cbFizDocType, "10", false);
                   }
                   else
                   {
                       //cbFizDocType.Enabled = true;
                   }
           
                }

                if (sender == deBirth) button1_Click(button1, null);
                //if ( ( sender == tbFizDocNumber || sender == tbFizDocSeries ) && cbFizDocType.SelectedIndex  )
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
                        tbAddrState.Text = dic["region"];
                    }
                }
                catch (Exception)
                {
                }
*/
                //if (tbFizBirthPlace.Text.Trim().Equals("")) tbFizBirthPlace.Text = tbAddrCity.Text;

                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void fillSim(Dictionary<string, string> dicsim)
        {
//            region_id = "";
            try
            {
                if (dicsim != null)
                {
                    if (dicsim.ContainsKey("status"))
                    {
                        if (!dicsim["status"].Equals("2"))
                        {
                            if (dicsim.ContainsKey("msisdn")) mtbMSISDN.Text = dicsim["msisdn"];
                            if (dicsim.ContainsKey("icc")) mtbICC.Text = dicsim["icc"];
                            if (dicsim.ContainsKey("plan_id")) StringTagItem.SelectByTag(cbPlan, dicsim["plan_id"], true);
                            if (dicsim.ContainsKey("owner_title")) lOwner.Text = dicsim["owner_title"];
                            else lOwner.Text = "?";
                            if (dicsim.ContainsKey("region_title")) lOwner.Text += ", " + dicsim["region_title"];
                            if (dicsim.ContainsKey("owner_status")) lOwner.Text += ", " + ((bool.Parse(dicsim["owner_status"])) ? "[Активен]" : "[Заблокирован]");
                            if (dicsim.ContainsKey("balance")) tb_balance.Text = dicsim["balance"];

                            // баланс сим
                            //IDEXData d = (IDEXData)toolbox;
                            //DataTable dt = d.getQuery("select kind from `mts_units_dp` where uid = " + fdocument.documentUnitID + " and dpcode = '" + d.EscapeString(mtbAssignedDPCode.Text) + "'");

                            //if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["kind"]) == 1)

                            try
                            {
                                //StringTagItem.SelectByTag(cbUnittitlesim, dicsim["owner_id"], true);
                            }
                            catch (Exception) { }
                        }
                        else
                        {
                            MessageBox.Show("Сим с данным номером в справочнике сим-карт имеет статус Продана");
                        }
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

        private void tbAddrCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    tbAddrCity.Text = capitalize(tbAddrCity.Text);
                    Dictionary<string, string> dic = ((IDEXCitySearcher)toolbox).getCityData("city", tbAddrCity.Text);
                    if (dic["zip"] != "")
                    {
                        tbAddrZip.Text = dic["zip"];
                    }
                    if (dic["region"] != "")
                    {
                        tbAddrState.Text = dic["region"];
                    }
                }
                catch (Exception)
                {
                }
                //if (tbFizBirthPlace.Text.Trim().Equals("")) tbFizBirthPlace.Text = tbAddrCity.Text;

                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void setPeopleDataToFields(StringList pdata, string phash)
        {
            //5 = Установка из peopledata в форму
            tbFizInn.Text = pdata["FizInn"];

            try
            {
                cbDocCategory.SelectedIndex = int.Parse(pdata["DocCategory"]);
            }
            catch (Exception) { }

            tbLastName.Text = pdata["LastName"];
            tbFirstName.Text = pdata["FirstName"];
            tbSecondName.Text = pdata["SecondName"];
            deBirth.Text = pdata["Birth"];

            try
            {
                cbSex.SelectedIndex = int.Parse(pdata["Sex"]);
            }
            catch (Exception) { }

            tbAddrZip.Text = pdata["AddrZip"];
            tbAddrState.Text = pdata["AddrState"];
            tbAddrCity.Text = pdata["AddrCity"];
            tbAddrStreet.Text = pdata["AddrStreet"];
            tbAddrHouse.Text = pdata["AddrHouse"];
            tbAddrBuilding.Text = pdata["AddrBuilding"];
            tbAddrApartment.Text = pdata["AddrApartment"];
            tbFizBirthPlace.Text = pdata["FizBirthPlace"];
            StringTagItem.SelectByTag(cbFizDocCountry, pdata["FizDocCountry"], true);
            StringTagItem.SelectByTag(cbFizDocСitizen, pdata["FizDocCountry"], true);
            StringTagItem.SelectByTag(cbFizDocType, pdata["FizDocType"], true);
            tbFizDocSeries.Text = pdata["FizDocSeries"];
            tbFizDocNumber.Text = pdata["FizDocNumber"];
            deFizDocDate.Text = pdata["FizDocDate"];
            tbFizDocOrg.Text = pdata["FizDocOrg"];
            try { tbFizDocOrgCode.Text = pdata["FizDocOrgCode"]; }
            catch (Exception) { }
            //cbFizDocScan.Checked = "X".Equals(pdata["FizDocScan"]);

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
                        setPeopleDataToFields(pf.selectedItem, "");
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


        private void LoadDefaultsToControls()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            StringList defs = new StringList(cfg.getStr(module.ID, "DefaultValues", ""));

            //6 = Загрузка дефолтов в поля
            StringTagItem.SelectByTag(cbDocUnit, cfg.getStr(this.Name, "cbDocUnit", ""), false);

            try
            {
                cbDocCategory.SelectedIndex = int.Parse(defs["DocCategory"]);
            }
            catch (Exception) { }

            tbCodeWord.Text = defs["CodeWord"];
            StringTagItem.SelectByTag(cbFizDocCountry, defs["FizDocCountry"], false);
            StringTagItem.SelectByTag(cbFizDocСitizen, defs["FizDocCitizen"], false);
            StringTagItem.SelectByTag(cbFizDocType, defs["FizDocType"], false);
            //cbFizDocScan.Checked = "X".Equals(defs["FizDocScan"]);

            
        }

        private void bSaveDefaults_Click(object sender, EventArgs e)
        {
            StringList defs = new StringList();
            //7 = Сохранение дефолтов из полей
            defs["DocCategory"] = cbDocCategory.SelectedIndex.ToString();
            defs["CodeWord"] = tbCodeWord.Text;
            
            try
            {
                defs["FizDocCountry"] = ( (StringTagItem)cbFizDocCountry.SelectedItem ).Tag;
                defs["FizDocCitizen"] = ( (StringTagItem)cbFizDocСitizen.SelectedItem ).Tag;
            }
            catch (Exception) { }

            try
            {
                defs["FizDocType"] = ((StringTagItem)cbFizDocType.SelectedItem).Tag;
            }
            catch (Exception) { }

            //defs["FizDocScan"] = cbFizDocScan.Checked ? "X" : "-";

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

        private void tbLastName_Leave(object sender, EventArgs e)
        {
            ((Control)sender).Text = capitalize(((Control)sender).Text);
        }

        private void tbAddrCity_Leave(object sender, EventArgs e)
        {
            ((Control)sender).Text = capitalize(((Control)sender).Text);
            //if (tbFizBirthPlace.Text.Trim().Length < 1) tbFizBirthPlace.Text = tbAddrCity.Text;
        }

        private void cbFizDocType_SelectedValueChanged(object sender, EventArgs e)
        {
            tbFizDocSeries.MaxLength = 32767;
            tbFizDocNumber.MaxLength = 32767;
            try
            {
                StringTagItem sti = (StringTagItem)cbFizDocType.SelectedItem;
                if (sti.Tag.Equals("21"))
                {
                    tbFizDocSeries.MaxLength = 4;
                    tbFizDocNumber.MaxLength = 6;

                    lbFizDocExp.Visible = false;
                    deFizDocExp.Visible = false;
                }
                if (!sti.Tag.Equals("21"))
                {
                    lbFizDocExp.Visible = true;
                    deFizDocExp.Visible = true;
                }
            }
            catch (Exception) { }
        }

        private void mtbICC_TextChanged(object sender, EventArgs e)
        {
            tbICCCTL.Text = getIccCtl(mtbICC.Text);
        }

        private void DocCategoryChange(object sender, EventArgs e)
        {

            if ( ( (ComboBox)sender ).SelectedItem.ToString() == "Нерезидент" )
            {
                //cbFizDocType.Enabled = false;
                StringTagItem.SelectByTag(cbFizDocType, "10", false);
                //tbFizDocOrg.Enabled = false;
                //tbFizDocOrg.Text = "";
                //tbFizDocOrgCode.Enabled = false;
                //tbFizDocOrgCode.Text = "";
                //deFizDocDate.Enabled = false;
                //deFizDocDate.Text = "__.__.____";
                //cbFizDocCountry.Enabled = false;
                //cbFizDocCountry.Text = "";
                //tbAddrState.Enabled = false;
                //tbAddrState.Text = "";
                //tbAddrCity.Enabled = false;
                //tbAddrCity.Text = "";
                //tbAddrZip.Enabled = false;
                //tbAddrZip.Text = "";
                //tbAddrStreet.Enabled = false;
                //tbAddrStreet.Text = "";
                //tbAddrHouse.Enabled = false;
                //tbAddrHouse.Text = "";
                //tbAddrBuilding.Enabled = false;
                //tbAddrBuilding.Text = "";
                //tbAddrApartment.Enabled = false;
                //tbAddrApartment.Text = "";
            }
            else
            {
                cbFizDocType.Enabled = true;
                //tbFizDocOrg.Enabled = true;
                //tbFizDocOrgCode.Enabled = true;
                //deFizDocDate.Enabled = true;
                //cbFizDocCountry.Enabled = true;
                //tbAddrState.Enabled = true;
                //tbAddrCity.Enabled = true;
                //tbAddrZip.Enabled = true;
                //tbAddrStreet.Enabled = true;
                //tbAddrHouse.Enabled = true;
                //tbAddrBuilding.Enabled = true;
                //tbAddrApartment.Enabled = true;
            }
        }

        private void addOrg_Click(object sender, EventArgs e)
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
                    if (((StringTagItem)cbFizDocType.SelectedItem).Tag.Equals("21"))
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
                }
                catch (Exception) { }
                tbFizDocOrg.Focus();
            }
        }

        private void bSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor = "mts";
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
                    string dd = "http://" + nodejsserver + ":3020/static/" + vendor + "/" + vendorBase + "/" + signatire.Text + "" + lbScanMime.Text;
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

        private void bShowScan_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor = "mts";
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
                    string dd = "http://" + nodejsserver + ":3020/static/" + vendor + "/" + vendorBase + "/" + signatire.Text + "" + lbScanMime.Text;
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

        private void bAddScanDocument_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OPF = new OpenFileDialog();
                OPF.Filter = "Files(*.JPG;*.JPEG;*.PNG;)|*.JPG;*.JPEG;*.PNG;";
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

        private void deFizDocExp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                tbFizDocOrgCode.Focus();
            }
        }

        private void tbFizDocOrg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                deFizDocDate.Focus();
            }
        }

    }
}
