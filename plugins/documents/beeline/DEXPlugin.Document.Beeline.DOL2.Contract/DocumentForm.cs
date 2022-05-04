using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DEXExtendLib;
using DEXSIM;

using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//вебдрайвер
using System.Web;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;
//using OpenQA.Selenium.Remote;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Interactions;
//using System.Globalization;
using BarcodeLib;

using System.Net;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{

    public partial class DocumentForm : Form
    {
        public Object toolbox;
        public Document module;
        IDEXSim sim;
        public DOL2Data dol2;
        bool isOnline;
        string rus = "169";
        DataTable dtCity;

        string dex_dexol_base = "";
        string nodejsserver = "";
        bool ifChangeScan = false;
        bool ifScanIsset = false;

        IDEXDocumentData fsource, fdocument;
        bool ReadOnly;

        bool deliveryTypeChanged;

        string DutyId = null;

        public string adaptersUid = "";

        public DocumentForm()
        {
            InitializeComponent();
        }

        string normName(string source)
        {
            try
            {
                source = source.Substring(0, 1).ToUpperInvariant() + source.Substring(1).ToLowerInvariant();
                string[] str = source.Split('-');
                if (str.Count() > 1)
                {
                    int c = 0;
                    foreach (String s in str) 
                    {
                        //ss += s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
                        str[c] = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
                        c++;
                    }
                    source = string.Join("-", str);
                }
                str = source.Split(' ');
                if (str.Count() > 1)
                {
                    int c = 0;
                    foreach (String s in str)
                    {
                        //ss += s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
                        str[c] = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
                        c++;
                    }
                    source = string.Join(" ", str);
                }

                return source;
                //return source.Substring(0, 1).ToUpperInvariant() + source.Substring(1);
                //return source.Substring(0, 1).ToUpperInvariant() + source.Substring(1).ToLowerInvariant();
            }
            catch (Exception)
            {
            }
            return source;
        }

        public void InitDocument(IDEXDocumentData source, IDEXDocumentData document, bool clone, bool ReadOnly)
        {
            

            Text = "Билайн форма договора (Бета-тестирование)";


            isOnline = ((IDEXUserData)toolbox).isOnline;

            if (!isOnline)
            {
                cbControl.Enabled = true;
                checkAmountRegistartion.Enabled = true;
            }
            else
            {
                cbControl.Enabled = false;
                checkAmountRegistartion.Enabled = false;
            }

            //если дексол, сначала форму сделаем маленькой, чотбы были видны только icc и проверочный код
            this.Size = new Size(840, 644);
            //this.Size = new Size(1546, 590);
            
            // комбо со статусом скана
            //cbScan.SelectedIndex = 0;
            
            


            this.ReadOnly = ReadOnly;
            IDEXServices srv = (IDEXServices)toolbox;
            sim = (IDEXSim)srv.GetService("sim");

            fsource = source;
            fdocument = document;

            label1.Visible = !isOnline;
            cbDocUnit.Visible = !isOnline;

            IDEXConfig cfg = (IDEXConfig)toolbox;

            

            if (!isOnline)
            {
                DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");

                try
                {
                    IDEXData dd = (IDEXData)toolbox;
                    DataTable dt = dd.getQuery("SELECT * FROM units");
                    foreach (DataRow dr in t.Rows)
                    {
                        foreach (DataRow dtdr in dt.Rows)
                        {
                            if (dtdr["uid"].ToString().Equals(dr["uid"].ToString()))
                            {
                                if (!dtdr["region"].ToString().Equals(""))
                                {
                                    dr["title"] = dtdr["title"].ToString() + " [" + dtdr["region"].ToString() + "]";
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception) { }


                StringTagItem.UpdateCombo(cbDocUnit, t, null, "uid", "title", false);
                StringTagItem.SelectByTag(cbDocUnit, cfg.getStr("Beeline.Form", "SUID", ""), true);
                //cbDataCheck.Enabled = true;

                // пока не использовать
                /*
                if (fsource == null)
                {
                    SelectUnit su = new SelectUnit(t);
                    if (su.ShowDialog() == DialogResult.OK)
                    {
                        cbDocUnit.SelectedIndex = su.cbUnits.SelectedIndex;
                    }
                }
                */
            }

            if (isOnline)
            {
                this.Size = new Size(575, 70);
                tcMain.Enabled = false;
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

            if (dol2 == null) dol2 = new DOL2Data(toolbox);

            dtCity = ((IDEXData)toolbox).getTable("city");

            cbDocUnit.Text = "";

            cbDocSphere.Items.Clear();
            cbDocSphere.Items.AddRange(dol2.spheres.ToArray());

            cbFizDocType.Items.Clear();
            // поменять местами данные о документе 07.10.2015 для синхронизации с порядком в базе (Костя)
            Array tt = dol2.doctypes.ToArray();
            List<object> arr = new List<object>();
            foreach ( var item in dol2.doctypes.ToArray() )
                arr.Add(item);
            arr.Reverse();
            cbFizDocType.Items.AddRange(arr.ToArray());
            //cbFizDocType.Items.AddRange(dol2.doctypes.ToArray());

            cbAddrCityType.Items.Clear();
            cbAddrCityType.Items.AddRange(dol2.placetypes.ToArray());

            cbResidenceAddrCityType.Items.Clear();
            cbResidenceAddrCityType.Items.AddRange(dol2.placetypes.ToArray());

            // по умолчанию ставим тип гражданства - резидент
            cbCitizenType.Items.Clear();
            cbCitizenType.Items.Add("Нерезидент");
            cbCitizenType.Items.Add("Резидент");
            cbCitizenType.SelectedIndex = 1;

            cbAddrStreetType.Items.Clear();
            cbAddrStreetType.Items.AddRange(dol2.streettypes.ToArray());

            cbResidenceAddrStreetType.Items.Clear();
            cbResidenceAddrStreetType.Items.AddRange(dol2.streettypes.ToArray());

            cbAddrBuildingType.Items.Clear();
            cbAddrBuildingType.Items.Add(new StringTagItem("", "-5"));
            cbAddrBuildingType.Items.AddRange(dol2.buildingtypes.ToArray());
            StringTagItem.SelectByTag(cbAddrBuildingType, "-5", true);

            cbResidenceAddrBuildingType.Items.Clear();
            cbResidenceAddrBuildingType.Items.Add(new StringTagItem("", "-5"));
            cbResidenceAddrBuildingType.Items.AddRange(dol2.buildingtypes.ToArray());
            StringTagItem.SelectByTag(cbResidenceAddrBuildingType, "-5", true);

            cbAddrApartmentType.Items.Clear();
            cbAddrApartmentType.Items.Add(new StringTagItem("", "-5"));
            cbAddrApartmentType.Items.AddRange(dol2.roomtypes.ToArray());
            StringTagItem.SelectByTag(cbAddrApartmentType, "-5", true);

            cbResidenceAddrApartmentType.Items.Clear();
            cbResidenceAddrApartmentType.Items.Add(new StringTagItem("", "-5"));
            cbResidenceAddrApartmentType.Items.AddRange(dol2.roomtypes.ToArray());
            StringTagItem.SelectByTag(cbResidenceAddrApartmentType, "-5", true);

            // ===
            cbDeliveryType.Items.Clear();
            cbDeliveryType.Items.Add(new StringTagItem("", "-5"));
            cbDeliveryType.Items.AddRange(dol2.deliverytypes.ToArray());
            cbDeliveryType.SelectedIndex = 0;
            StringTagItem.SelectByTag(cbDeliveryType, "1", false);

            // гражданство
            cbCitizenship.Items.Clear();
            cbCitizenship.Items.Add(new StringTagItem("", "-5"));
            cbCitizenship.Items.AddRange(dol2.libCountries.ToArray());
            //cbCitizenship.SelectedIndex = 0;
            StringTagItem.SelectByTag(cbCitizenship, "165", false);

            //теперь страна регистрации это комбо
            cbAddrCountry.Items.Clear();
            cbAddrCountry.Items.Add(new StringTagItem("", "-5"));
            cbAddrCountry.Items.AddRange(dol2.libCountries.ToArray());
            StringTagItem.SelectByTag(cbAddrCountry, "165", false);

            //теперь страна доставки это комбо
            cbDeliveryCountry.Items.Clear();
            cbDeliveryCountry.Items.Add(new StringTagItem("", "-5"));
            cbDeliveryCountry.Items.AddRange(dol2.libCountries.ToArray());
            StringTagItem.SelectByTag(cbDeliveryCountry, "165", false);

            //страна пребывания
            cbResidenceAddrCountry.Items.Clear();
            cbResidenceAddrCountry.Items.Add(new StringTagItem("", "-5"));
            cbResidenceAddrCountry.Items.AddRange(dol2.libCountries.ToArray());
            StringTagItem.SelectByTag(cbResidenceAddrCountry, "169", false);


            cbDeliveryCityType.Items.Clear();
            cbDeliveryCityType.Items.AddRange(dol2.placetypes.ToArray());
            cbDeliveryStreetType.Items.Clear();
            cbDeliveryStreetType.Items.AddRange(dol2.streettypes.ToArray());

            cbDeliveryBuildingType.Items.Clear();
            cbDeliveryBuildingType.Items.Add(new StringTagItem("", "-5"));
            cbDeliveryBuildingType.Items.AddRange(dol2.buildingtypes.ToArray());
            StringTagItem.SelectByTag(cbDeliveryBuildingType, "-5", true);

            cbDeliveryApartmentType.Items.Clear();
            cbDeliveryApartmentType.Items.Add(new StringTagItem("", "-5"));
            cbDeliveryApartmentType.Items.AddRange(dol2.roomtypes.ToArray());
            StringTagItem.SelectByTag(cbDeliveryApartmentType, "-5", true);

            // ===
            clbServices.Items.Clear();

            cbDocPaySystem.Items.Clear();
            cbDocPaySystem.Items.AddRange(dol2.paysystems.ToArray());
            cbDocPaySystem.Enabled = true;

            cbDocBillDay.Items.Clear();
            cbDocBillDay.Items.AddRange(dol2.billcycles.ToArray());
            try
            {
                cbDocBillDay.SelectedIndex = 0;
            }
            catch (Exception) { }

            cbPlan.Items.Clear();

            DataTable dtPlans = ((IDEXData)toolbox).getQuery("select * from `um_plans`");
            foreach (DataRow dr in dtPlans.Rows)
            {
                cbPlan.Items.Add(new StringTagItem(dr["title"].ToString(), dr["plan_id"].ToString()));
            }
            //foreach (KeyValuePair<string, DOL2BillPlan> kvp in dol2.libBillplans)
            //{
            //    cbPlan.Items.Add(new StringObjTagItem(kvp.Value.Name, (object)kvp.Value));
            //}
            cbPlan.Enabled = true;



            /*
            if (cfg.getBool(module.ID, "suggest_fio", true))
            {
                InitComboDictionary(tbSecondName, "second_name");
                InitComboDictionary(tbFirstName, "first_name");
                InitComboDictionary(tbLastName, "last_name");
            }
            */

            // замена на InitComboOgrCode(tbFizDocOrg) в связи с вводом кода подразделения
            //InitComboDictionary(tbFizDocOrg, "fiz_doc_org");

            InitComboOgrCode(tbFizDocOrg);

            InitComboCity(tbFizBirthPlace, "city");

            //InitComboDictionary(tbAddrCountry, "country");
            InitComboDictionary(tbAddrStreet, "street");
            InitComboCity(tbAddrRegion, "region");
            InitComboDictionary(tbAddrState, "state");
            InitComboCity(tbAddrCity, "city");

            //InitComboDictionary(tbDeliveryCountry, "country");
            //адрес пребывания
            InitComboDictionary(tbResidenceAddrState, "state");
            InitComboCity(tbResidenceAddrRegion, "region");
            InitComboDictionary(tbResidenceAddrStreet, "street");
            InitComboCity(tbResidenceAddrCity, "city");

            InitComboDictionary(tbDeliveryStreet, "street");
            InitComboCity(tbDeliveryRegion, "region");
            InitComboDictionary(tbDeliveryState, "state");
            InitComboCity(tbDeliveryCity, "city");

            InitComboCity(tbDocCity, "city");

            deDocDate.Value = DateTime.Now;
            // Очистка полей

            tbDocNum.Text = "";
            deDocDate.Value = DateTime.Now;
            cbDocOrgType.SelectedIndex = 0;
            cbDocOrgType_SelectedIndexChanged(cbDocOrgType, null);

            cbDocSphere.SelectedIndex = 0;
            cbDocClientType.SelectedIndex = 0;

            //выберем по умолчанию тип клиента - Частное лицо
            StringTagItem.SelectByTag(cbDocClientType, "0", true);
            //по умолчанию сфера деятельности - Другие
            StringTagItem.SelectByTag(cbDocSphere, "12", true);

            cbSex.SelectedItem = 2;
            tbLastName.Text = "";
            tbFirstName.Text = "";
            tbSecondName.Text = "";

            tbCompanyTitle.Text = "";
            tbCompanyInn.Text = "";
            tbCompanyOkonh.Text = "";
            tbCompanyOkpo.Text = "";
            tbCompanyKpp.Text = "";

            cbFizDocType.SelectedIndex = 0;
            mtbFizDocSeries.Text = "";
            mtbFizDocNumber.Text = "";
            tbFizDocOrg.Text = "";
            cbFizDocScan.Checked = false;
            deFizDocDate.Text = "";
            tbFizInn.Text = "";
            tbFizBirthPlace.Text = "";
            deBirth.Text = "";

            tbOrgBank.Text = "";
            tbOrgRs.Text = "";
            tbOrgKs.Text = "";
            tbOrgBik.Text = "";

            //tbAddrCountry.Text = "";
            tbAddrState.Text = "";
            tbAddrRegion.Text = "";
            cbAddrCityType.SelectedIndex = 0;
            tbAddrCity.Text = "";
            tbAddrZip.Text = "";
            cbAddrStreetType.SelectedIndex = 0;
            tbAddrStreet.Text = "";
            tbAddrHouse.Text = "";
            cbAddrBuildingType.SelectedIndex = 0;
            tbAddrBuilding.Text = "";
            cbAddrApartmentType.SelectedIndex = 0;
            tbAddrApartment.Text = "";

            tbDeliveryComment.Text = "";
            //tbDeliveryCountry.Text = "";
            tbDeliveryState.Text = "";
            tbDeliveryRegion.Text = "";
            cbDeliveryCityType.SelectedIndex = 0;
            tbDeliveryCity.Text = "";
            tbDeliveryZip.Text = "";
            cbDeliveryStreetType.SelectedIndex = 0;
            tbDeliveryStreet.Text = "";
            tbDeliveryHouse.Text = "";
            cbDeliveryBuildingType.SelectedIndex = 0;
            tbDeliveryBuilding.Text = "";
            cbDeliveryApartmentType.SelectedIndex = 0;
            tbDeliveryApartment.Text = "";

            cbContactSex.SelectedItem = null;
            tbContactFio.Text = "";
            tbContactPhonePrefix.Text = "";
            tbContactPhone.Text = "";
            tbContactFaxPrefix.Text = "";
            tbContactFax.Text = "";
            tbContactEmail.Text = "";
            tbContactComment.Text = "";

            mtbMSISDN.Mask = ((IDEXConfig)toolbox).getRegisterStr("beeline_msisdn_mask", "0000000000");
            mtbMSISDN.Text = "";

            mtbICC.Mask = ((IDEXConfig)toolbox).getRegisterStr("beeline_icc_mask", "000000000000000000");
            mtbICC.Text = "";

            cbPlan.SelectedIndex = 0;

            cbDocPaySystem.SelectedIndex = 0;

            cbDocBillDay.SelectedIndex = 0;

            tbComments.Text = "";


            lvLogParams.Items.Clear();

            DutyId = null;

            IDEXData d = (IDEXData)toolbox;

            //DataTable dta = d.getQuery(string.Format("select * from `{0}` order by id", d.EscapeString(dol2.libname("logparams"))));
            DataTable dta = d.getTable(d.EscapeString(dol2.libname("logparams")));
            dta.DefaultView.Sort = "id";

            if (dta != null && dta.Rows.Count > 0)
            {
                foreach (DataRow row in dta.Rows)
                {
                    ListViewItem lvi = lvLogParams.Items.Add(row["name"].ToString());
                    lvi.Tag = row["code"].ToString();
                    lvi.ToolTipText = row["comments"].ToString();
                    lvi.Checked = Convert.ToBoolean(row["value"]);

                }

            }

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

            deliveryTypeChanged = false;

            if (fsource != null)
            {
                try
                {
                    // так как открывается договор с данными, то покажем сразу реальный размер формы
                    this.Size = new Size(840, 644);

                    SimpleXML xml = SimpleXML.LoadXml(fsource.documentText);

                    StringTagItem.SelectByTag(cbDocUnit, fsource.documentUnitID.ToString(), true);

                    label2.Visible = true;
                    tbDocNum.Visible = true;
                    // если договор заполнен, то проверочный код не нужен
                    tbDynamicIcc.Enabled = false;
                    label60.Visible = false;
                    tbVerificationCode.Visible = false;

                    try
                    {
                        cbScan.SelectedIndex = Convert.ToInt32(xml["FizDocScan"].Text);
                        if (cbScan.SelectedIndex == 1)
                        {
                            ifScanIsset = true;
                        }
                    }
                    catch (Exception)
                    {
                        cbScan.SelectedIndex = 0;
                    }
                    try
                    {
                        lbScanMime.Text = xml["FizDocScanMime"].Text;
                    }
                    catch (Exception) 
                    { 
                        
                    }
                    signatire.Text = fsource.signature;

                    gbNewDol.Visible = true;

                    //
                    // Загрузка полей формы из xml

                    deDocDate.Text = xml["DocDate"].Text;
                    tbDocNum.Text = xml["DocNum"].Text;

                    tbDocCity.Text = xml["DocCity"].Text;

                    SelectCB(cbDocOrgType, xml["DocOrgType"].Text, true);

                    StringTagItem.SelectByTag(cbDocSphere, xml["DocSphere"].Text, true);
                    StringTagItem.SelectByTag(cbDocClientType, xml["DocClientType"].Text, true);

                    SelectCB(cbSex, xml["Sex"].Text, false);

                    tbLastName.Text = xml["LastName"].Text;
                    tbFirstName.Text = xml["FirstName"].Text;
                    tbSecondName.Text = xml["SecondName"].Text;

                    tbCompanyTitle.Text = xml["CompanyTitle"].Text;
                    tbCompanyInn.Text = xml["CompanyInn"].Text;
                    tbCompanyOkonh.Text = xml["CompanyOkonh"].Text;
                    tbCompanyOkpo.Text = xml["CompanyOkpo"].Text;
                    tbCompanyKpp.Text = xml["CompanyKpp"].Text;

                    cbFizDocType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbFizDocType, xml["FizDocType"].Text, false);
                    mtbFizDocSeries.Text = xml["FizDocSeries"].Text;
                    mtbFizDocNumber.Text = xml["FizDocNumber"].Text;
                    deFizDocDate.Text = xml["FizDocDate"].Text;
                    tbFizDocOrg.Text = xml["FizDocOrg"].Text;
                    cbFizDocScan.Checked = xml.GetNodeByPath("FizDocScan", true).Text.Equals("X");
                    tbFizBirthPlace.Text = xml["FizBirthPlace"].Text;
                    deBirth.Text = xml["Birth"].Text;
                    tbFizInn.Text = xml["FizInn"].Text;

                    // код подразделения и гражданство
                    tbFizDocOrgCode.Text = xml["FizDocOrgCode"].Text;
                    StringTagItem.SelectByTag(cbCitizenship, xml["Citizenship"].Text, false);
                    // если не резидент РФ, то дадим возможность выбора и поставим текущее гражданство

                    try
                    {
                        tcMain.Visible = true;
                        tcMain.Enabled = true;
                        tbDynamicIcc.Text = xml["ICC"].Text.Substring(7);
                        tbVerificationCode.Enabled = false;

                        // покажем проверочный код
                        if (xml.GetNodeByPath("checkCode", false) != null)
                        {
                            tbVerificationCode.Text = xml["checkCode"].Text;
                        }
                        else
                        {
                            if (xml.GetNodeByPath("OLDNEWDOLMSISDN", false) != null)
                            {
                                tbVerificationCode.Text = xml["OLDNEWDOLMSISDN"].Text.Substring(6);
                            }
                            else
                            {
                                tbVerificationCode.Text = xml["MSISDN"].Text.Substring(6);
                            }
                        }

                        cbSimType.SelectedIndex = int.Parse(xml["Dynamic"].Text);
                        tbBoxType.Text = xml["boxType"].Text;
                    }
                    catch (Exception) { }

                    if (!isOnline)
                    {
                        tbDynamicIcc.Enabled = true;
                        tbVerificationCode.Enabled = true;
                        tbDynamicOldNumber.Enabled = true; 
                    }

                    //if ("1".Equals(xml["Dynamic"].Text))
                    //{
                    tbDynamicOldNumber.Text = xml["OLDNEWDOLMSISDN"].Text;

                    //}
                    //else
                    //{
                    //    label68.Visible = false;
                    //    tbDynamicOldNumber.Visible = false;
                    //}

                    try
                    {
                        cbCitizenType.SelectedIndex = Convert.ToInt32(xml["CitizenType"].Text);
                        if (Convert.ToInt32(xml["CitizenType"].Text) == 0)
                        {
                            tbMigrationCardSeries.Text = xml["MigrationCardSeries"].Text;
                            tbMigrationCardNumber.Text = xml["MigrationCardNumber"].Text;

                            deMigrationCardDateStart.Text = xml["MigrationCardDateStart"].Text;
                            deMigrationCardDateFinish.Text = xml["MigrationCardDateFinish"].Text;
                        }
                    }
                    catch (Exception) { }

                    tbOrgBank.Text = xml["OrgBank"].Text;
                    tbOrgRs.Text = xml["OrgRs"].Text;
                    tbOrgKs.Text = xml["OrgKs"].Text;
                    tbOrgBik.Text = xml["OrgBik"].Text;

                    //адрес регистрации
                    //tbAddrCountry.Text = xml["AddrCountry"].Text;
                    cbAddrCountry.Text = xml["AddrCountry"].Text;
                    tbAddrState.Text = xml["AddrState"].Text;
                    tbAddrRegion.Text = xml["AddrRegion"].Text;
                    cbAddrCityType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbAddrCityType, xml["AddrCityType"].Text, false);
                    tbAddrCity.Text = xml["AddrCity"].Text;
                    tbAddrZip.Text = xml["AddrZip"].Text;
                    cbAddrStreetType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbAddrStreetType, xml["AddrStreetType"].Text, false);
                    tbAddrStreet.Text = xml["AddrStreet"].Text;
                    tbAddrHouse.Text = xml["AddrHouse"].Text;
                    cbAddrBuildingType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbAddrBuildingType, xml["AddrBuildingType"].Text, false);
                    tbAddrBuilding.Text = xml["AddrBuilding"].Text;
                    cbAddrApartmentType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbAddrApartmentType, xml["AddrApartmentType"].Text, false);
                    tbAddrApartment.Text = xml["AddrApartment"].Text;


                    //адрес места пребывания
                    cbResidenceAddrCountry.Text = xml["ResidenceAddrCountry"].Text;
                    tbResidenceAddrState.Text = xml["ResidenceAddrState"].Text;
                    tbResidenceAddrRegion.Text = xml["ResidenceAddrRegion"].Text;
                    cbResidenceAddrCityType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbResidenceAddrCityType, xml["ResidenceAddrCityType"].Text, false);
                    tbResidenceAddrCity.Text = xml["ResidenceAddrCity"].Text;
                    tbResidenceAddrZip.Text = xml["ResidenceAddrZip"].Text;
                    cbResidenceAddrStreetType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbResidenceAddrStreetType, xml["ResidenceAddrStreetType"].Text, false);
                    tbResidenceAddrStreet.Text = xml["ResidenceAddrStreet"].Text;
                    tbResidenceAddrHouse.Text = xml["ResidenceAddrHouse"].Text;
                    cbResidenceAddrBuildingType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbResidenceAddrBuildingType, xml["ResidenceAddrBuildingType"].Text, false);
                    tbResidenceAddrBuilding.Text = xml["ResidenceAddrBuilding"].Text;
                    cbResidenceAddrApartmentType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbResidenceAddrApartmentType, xml["ResidenceAddrApartmentType"].Text, false);
                    tbResidenceAddrApartment.Text = xml["ResidenceAddrApartment"].Text;


                    // адрес доставки корреспонденции
                    cbDeliveryType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbDeliveryType, xml["DeliveryType"].Text, false);
                    tbDeliveryComment.Text = xml["DeliveryComment"].Text;
                    //tbDeliveryCountry.Text = xml["DeliveryCountry"].Text;
                    cbDeliveryCountry.Text = xml["DeliveryCountry"].Text;
                    tbDeliveryState.Text = xml["DeliveryState"].Text;
                    tbDeliveryRegion.Text = xml["DeliveryRegion"].Text;
                    cbDeliveryCityType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbDeliveryCityType, xml["DeliveryCityType"].Text, false);
                    tbDeliveryCity.Text = xml["DeliveryCity"].Text;
                    tbDeliveryZip.Text = xml["DeliveryZip"].Text;
                    cbDeliveryStreetType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbDeliveryStreetType, xml["DeliveryStreetType"].Text, false);
                    tbDeliveryStreet.Text = xml["DeliveryStreet"].Text;
                    tbDeliveryHouse.Text = xml["DeliveryHouse"].Text;
                    cbDeliveryBuildingType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbDeliveryBuildingType, xml["DeliveryBuildingType"].Text, false);
                    tbDeliveryBuilding.Text = xml["DeliveryBuilding"].Text;
                    cbDeliveryApartmentType.SelectedItem = null;
                    StringTagItem.SelectByTag(cbDeliveryApartmentType, xml["DeliveryApartmentType"].Text, false);
                    tbDeliveryApartment.Text = xml["DeliveryApartment"].Text;

                    cbContactSex.SelectedItem = null;
                    SelectCB(cbContactSex, xml["ContactSex"].Text, false);
                    tbContactFio.Text = xml["ContactFio"].Text;
                    tbContactPhonePrefix.Text = xml["ContactPhonePrefix"].Text;
                    tbContactPhone.Text = xml["ContactPhone"].Text;
                    tbContactFaxPrefix.Text = xml["ContactFaxPrefix"].Text;
                    tbContactFax.Text = xml["ContactFax"].Text;
                    tbContactEmail.Text = xml["ContactEmail"].Text;
                    tbContactComment.Text = xml["ContactComment"].Text;

                    mtbMSISDN.Text = xml["MSISDN"].Text;
                    mtbICC.Text = xml["ICC"].Text;

                    mtbAssignedDPCode.Text = xml.GetNodeByPath("AssignedDPCode", true).Text;

                    lOwner.Text = "?";

                    // скроем кнопку проверки симки
                    btnCheckInputData.Visible = false;
                    //покажем поле для проверочного кода
                    label60.Visible = true;
                    tbVerificationCode.Visible = true;

                    try
                    {
                        Dictionary<string, string> dicsim = sim.getSimByMSISDN(mtbMSISDN.Text.Trim());

                        if (dicsim.ContainsKey("owner_title")) lOwner.Text = dicsim["owner_title"];
                        if (dicsim.ContainsKey("region_title")) lOwner.Text += ", " + dicsim["region_title"];
                        if (dicsim.ContainsKey("owner_status")) lOwner.Text += ", " + ((Convert.ToBoolean(dicsim["owner_status"])) ? "[Активен]" : "[Заблокирован]");
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        //DOL2BillPlan bplan = dol2.libBillplans[xml["Plan"].Attributes["Code"]];
                        cbPlan.SelectedItem = null;
                        StringTagItem.SelectByTag(cbPlan, xml["Plan"].Attributes["Code"], true);
                        //StringObjTagItem.SelectByTag(cbPlan, bplan, false);

                        // форма оплаты
                        StringTagItem.SelectByTag(cbDocPaySystem, xml["Plan"].Attributes["PaySystemsId"], true);

                    }
                    catch (Exception)
                    {
                    }

                    StringTagItem.SelectByTag(cbDocBillDay, xml["DocBillDay"].Text, true);

                    SimpleXML xmlSvc = xml["Services"];
                    List<string> svcs = new List<string>();
                    foreach (SimpleXML xmlSvci in xmlSvc.GetChildren("Service"))
                    {
                        string code = xmlSvci.SafeAttribute("code", "");
                        if (code != "" && !svcs.Contains(code)) svcs.Add(code);
                    }

                    for (int f = 0; f < clbServices.Items.Count; ++f)
                    {
                        DOL2Service dol2service = (DOL2Service)((StringObjTagItem)clbServices.Items[f]).Tag;
                        string code = dol2service.Code;
                        clbServices.SetItemChecked(f, dol2service.Mandatory || svcs.Contains(code));
                    }

                    tbComments.Text = xml["Comments"].Text;

                    if (xml.GetNodeByPath("LogParams", false) != null)
                    {
                        SimpleXML xmlLogparams = xml["LogParams"];
                        foreach (SimpleXML lpitem in xmlLogparams.GetChildren("Param"))
                        {
                            if (lpitem.Attributes.ContainsKey("code"))
                            {
                                string lpcode = lpitem.Attributes["code"];
                                bool flag = (lpitem.Attributes.ContainsKey("status") && lpitem.Attributes["status"].Equals("1"));
                                foreach (ListViewItem lvi in lvLogParams.Items)
                                {
                                    if (((string)lvi.Tag).Equals(lpcode)) lvi.Checked = flag;
                                }
                            }
                        }
                    }

                    if (xml.GetNodeByPath("DutyId", false) != null) DutyId = xml["DutyId"].Text;

                    //
                    //

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
                deJournalDate.Focus();
            }
            else
            {
                //this.ActiveControl = tbDynamicIcc;
                tbDynamicIcc.Focus();
                //tbDynamicIcc.Select();
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

        void SelectCB(ComboBox cb, string s, bool def0)
        {
            try
            {
                cb.SelectedIndex = int.Parse(s);
            }
            catch (Exception)
            {
                if (def0) cb.SelectedIndex = 0;
            }
        }

        private void LoadDefaultsToControls()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            StringList defs = new StringList(cfg.getStr(module.ID, "DefaultValues", ""));

            // TODO Загрузка полей документа по умолчанию из defs
            SelectCB(cbDocOrgType, defs["DocOrgType"], true);
            StringTagItem.SelectByTag(cbDocSphere, defs["DocSphere"], true);
            StringTagItem.SelectByTag(cbDocClientType, defs["DocClientType"], true);
            cbSex.SelectedIndex = 2;
            SelectCB(cbSex, defs["Sex"], false);
            StringTagItem.SelectByTag(cbFizDocType, defs["FizDocType"], false);
            cbFizDocScan.Checked = "X".Equals(defs["FizDocScan"]);
            //tbAddrCountry.Text = defs["AddrCountry"];
            StringTagItem.SelectByTag(cbAddrCityType, defs["AddrCityType"], false);
            StringTagItem.SelectByTag(cbAddrStreetType, defs["AddrStreetType"], false);
            StringTagItem.SelectByTag(cbAddrBuildingType, defs["AddrBuildingType"], false);
            StringTagItem.SelectByTag(cbAddrApartmentType, defs["AddrApartmentType"], false);

            StringTagItem.SelectByTag(cbDeliveryType, defs["DeliveryType"], false);
            tbDeliveryComment.Text = defs["DeliveryComment"];
            try
            {
                tbDocCity.Text = defs["DocCity"];
            }
            catch (Exception) { }
            //tbDeliveryCountry.Text = defs["DeliveryCountry"];
            cbDeliveryCountry.Text = defs["DeliveryCountry"];
            StringTagItem.SelectByTag(cbDeliveryCityType, defs["DeliveryCityType"], false);
            StringTagItem.SelectByTag(cbDeliveryStreetType, defs["DeliveryStreetType"], false);
            StringTagItem.SelectByTag(cbDeliveryBuildingType, defs["DeliveryBuildingType"], false);
            StringTagItem.SelectByTag(cbDeliveryApartmentType, defs["DeliveryApartmentType"], false);

            try
            {
                tbNewDolLogin.Text = defs["NewDolLogin"];
                tbNewDolPassword.Text = defs["NewDolPassword"];
            }
            catch (Exception) { }
        }

        public string[] getDataHintOrgCode()
        {
            List<string> ret = new List<string>();
            //if (hintType == null || hintType.Trim().Length < 1) return ret.ToArray();

            DataTable t = ((IDEXData)toolbox).getQuery("select title from `org_codes`");
            try
            {
                foreach (DataRow rw in t.Rows)
                {
                    ret.Add(rw["title"].ToString());
                }
            }
            catch (Exception){}

            return ret.ToArray();
        }

        void InitComboOgrCode(TextBox src)
        {
            src.AutoCompleteCustomSource.Clear();
            try
            {
                src.AutoCompleteCustomSource.AddRange(getDataHintOrgCode());
            }
            catch (Exception) { }
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
            //deJournalDate.Focus();
            tbDynamicIcc.Focus();
        }

        void SaveSTI(SimpleXML node, ComboBox cb, string def)
        {
            try
            {
                node.Text = ((StringTagItem)cb.SelectedItem).Tag;
                if (cb.SelectedIndex > -1 && cb.SelectedIndex < cb.Items.Count)
                {
                    node.Attributes["printable"] = cb.Items[cb.SelectedIndex].ToString();
                }
            }
            catch (Exception)
            {
                if (def != null) node.Text = def;
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
         
            // вставим в соответствующие поля формы данные для места пребывания
            cbResidenceAddrCountry.SelectedIndex = cbAddrCountry.SelectedIndex;
            tbResidenceAddrState.Text = tbAddrState.Text;
            tbResidenceAddrRegion.Text = tbAddrRegion.Text;
            cbResidenceAddrCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
            tbResidenceAddrCity.Text = tbAddrCity.Text;
            tbResidenceAddrZip.Text = tbAddrZip.Text;
            cbResidenceAddrStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
            tbResidenceAddrStreet.Text = tbAddrStreet.Text;
            tbResidenceAddrHouse.Text = tbAddrHouse.Text;
            cbResidenceAddrBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
            tbResidenceAddrBuilding.Text = tbAddrBuilding.Text;
            cbResidenceAddrApartmentType.SelectedIndex = cbAddrApartmentType.SelectedIndex;
            tbResidenceAddrApartment.Text = tbAddrApartment.Text;

            // вставим в соответствующие поля формы данные для доставки
            cbDeliveryCountry.SelectedIndex = cbAddrCountry.SelectedIndex;
            tbDeliveryState.Text = tbAddrState.Text;
            tbDeliveryRegion.Text = tbAddrRegion.Text;
            cbDeliveryCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
            tbDeliveryCity.Text = tbAddrCity.Text;
            tbDeliveryZip.Text = tbAddrZip.Text;
            cbDeliveryStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
            tbDeliveryStreet.Text = tbAddrStreet.Text;
            tbDeliveryHouse.Text = tbAddrHouse.Text;
            cbDeliveryBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
            tbDeliveryBuilding.Text = tbAddrBuilding.Text;
            cbDeliveryApartmentType.SelectedIndex = cbAddrApartmentType.SelectedIndex;
            tbDeliveryApartment.Text = tbAddrApartment.Text;
            
            fdocument.documentStatus = cbDocStatus.SelectedIndex;

            int unid = -1;

            if (!isOnline)
            {
                try
                {
                    unid = int.Parse(((StringTagItem)cbDocUnit.SelectedItem).Tag);
                }
                catch (Exception) { }

                fdocument.documentUnitID = unid;
            }



            if (tbContactPhonePrefix.Text.Equals("") && tbContactPhone.Text.Equals(""))
            {
                try
                {
                    tbContactPhonePrefix.Text = mtbMSISDN.Text.Substring(0, 3);
                    tbContactPhone.Text = mtbMSISDN.Text.Substring(3, 7);
                }
                catch (Exception)
                {
                }
            }

            cbContactSex.SelectedIndex = cbSex.SelectedIndex;
            

            SimpleXML xml = new SimpleXML("Document");
            xml.Attributes["ID"] = module.ID;
            xml["DocDate"].Text = deDocDate.Text;
            xml["DocNum"].Text = tbDocNum.Text;
            xml["DocUnit"].Text = unid.ToString();

            xml["DocCity"].Text = autocompleteFind(tbDocCity.AutoCompleteCustomSource, tbDocCity.Text, tbDocCity.Text);

            xml["isOnline"].Text = "0";
            if (isOnline) xml["isOnline"].Text = "1";

            //
            // Сохранение полей формы в документе
            //

            //xml["DocDate"].Text = deDocDate.Text;
            //xml["DocNum"].Text = tbDocNum.Text;

            try
            {
                xml["Control"].Text = Convert.ToInt32(cbControl.Checked).ToString();
            }
            catch (Exception) { }


            xml["CheckAmountRegistartion"].Text = Convert.ToInt32(checkAmountRegistartion.Checked).ToString();

            xml["DocOrgType"].Text = cbDocOrgType.SelectedIndex.ToString();

            xml["AssignedDPCode"].Text = mtbAssignedDPCode.Text;

            SaveSTI(xml["DocSphere"], cbDocSphere, null);
            SaveSTI(xml["DocClientType"], cbDocClientType, null);

            xml["Sex"].Text = cbSex.SelectedIndex.ToString();

            xml["LastName"].Text = tbLastName.Text;
            xml["FirstName"].Text = tbFirstName.Text;
            xml["SecondName"].Text = tbSecondName.Text;

            xml["CompanyTitle"].Text = tbCompanyTitle.Text;
            xml["CompanyInn"].Text = tbCompanyInn.Text;
            xml["CompanyOkonh"].Text = tbCompanyOkonh.Text;
            xml["CompanyOkpo"].Text = tbCompanyOkpo.Text;
            xml["CompanyKpp"].Text = tbCompanyKpp.Text;

            SaveSTI(xml["FizDocType"], cbFizDocType, null);
            xml["FizDocSeries"].Text = mtbFizDocSeries.Text;
            xml["FizDocNumber"].Text = mtbFizDocNumber.Text;
            xml["FizDocDate"].Text = deFizDocDate.Text;
            xml["FizDocOrg"].Text = tbFizDocOrg.Text;
            xml["FizDocOrgCode"].Text = cbCitizenType.SelectedIndex == 1 ? tbFizDocOrgCode.Text: "";//cbCitizenType1.Checked ? tbFizDocOrgCode.Text : "";
            xml["FizDocScan"].Text = cbFizDocScan.Checked ? "X" : "-";
            xml["FizBirthPlace"].Text = tbFizBirthPlace.Text;
            xml["Birth"].Text = deBirth.Text;
            xml["FizInn"].Text = tbFizInn.Text;

            // гражданство
            xml["CitizenType"].Text = cbCitizenType.SelectedIndex.ToString();    
            xml["Citizenship"].Text = ((StringTagItem)cbCitizenship.SelectedItem).Tag;
            xml["CitizenshipText"].Text = ((StringTagItem)cbCitizenship.SelectedItem).Text;

            xml["MigrationCardSeries"].Text = tbMigrationCardSeries.Text;
            xml["MigrationCardNumber"].Text = tbMigrationCardNumber.Text;

            xml["MigrationCardDateStart"].Text = deMigrationCardDateStart.Text;
            xml["MigrationCardDateFinish"].Text = deMigrationCardDateFinish.Text;

            xml["OrgBank"].Text = tbOrgBank.Text;
            xml["OrgRs"].Text = tbOrgRs.Text;
            xml["OrgKs"].Text = tbOrgKs.Text;
            xml["OrgBik"].Text = tbOrgBik.Text;

            //xml["AddrCountry"].Text = tbAddrCountry.Text;

            //адрес регистрации
            xml["AddrCountry"].Text = cbAddrCountry.Text;
            xml["AddrState"].Text = tbAddrState.Text;
            xml["AddrRegion"].Text = tbAddrRegion.Text;
            SaveSTI(xml["AddrCityType"], cbAddrCityType, null);
            xml["AddrCity"].Text = tbAddrCity.Text;
            xml["AddrZip"].Text = tbAddrZip.Text;
            SaveSTI(xml["AddrStreetType"], cbAddrStreetType, null);
            xml["AddrStreet"].Text = tbAddrStreet.Text;
            xml["AddrHouse"].Text = tbAddrHouse.Text;
            SaveSTI(xml["AddrBuildingType"], cbAddrBuildingType, null);
            xml["AddrBuilding"].Text = tbAddrBuilding.Text;
            SaveSTI(xml["AddrApartmentType"], cbAddrApartmentType, null);
            xml["AddrApartment"].Text = tbAddrApartment.Text;

            // адрес места пребывания
            xml["ResidenceAddrCountry"].Text = cbResidenceAddrCountry.Text;
            xml["ResidenceAddrState"].Text = tbResidenceAddrState.Text;
            xml["ResidenceAddrRegion"].Text = tbResidenceAddrRegion.Text;
            SaveSTI(xml["ResidenceAddrCityType"], cbResidenceAddrCityType, null);
            xml["ResidenceAddrCity"].Text = tbResidenceAddrCity.Text;
            xml["ResidenceAddrZip"].Text = tbResidenceAddrZip.Text;
            SaveSTI(xml["ResidenceAddrStreetType"], cbResidenceAddrStreetType, null);
            xml["ResidenceAddrStreet"].Text = tbResidenceAddrStreet.Text;
            xml["ResidenceAddrHouse"].Text = tbResidenceAddrHouse.Text;
            SaveSTI(xml["ResidenceAddrBuildingType"], cbResidenceAddrBuildingType, null);
            xml["ResidenceAddrBuilding"].Text = tbResidenceAddrBuilding.Text;
            SaveSTI(xml["ResidenceAddrApartmentType"], cbResidenceAddrApartmentType, null);
            xml["ResidenceAddrApartment"].Text = tbResidenceAddrApartment.Text;


            // адрес доставки корреспонденции
            //xml["DeliveryCountry"].Text = tbDeliveryCountry.Text;
            SaveSTI(xml["DeliveryType"], cbDeliveryType, null);
            xml["DeliveryComment"].Text = tbDeliveryComment.Text;
            xml["DeliveryCountry"].Text = cbDeliveryCountry.Text;
            xml["DeliveryState"].Text = tbDeliveryState.Text;
            xml["DeliveryRegion"].Text = tbDeliveryRegion.Text;
            SaveSTI(xml["DeliveryCityType"], cbDeliveryCityType, null);
            xml["DeliveryCity"].Text = tbDeliveryCity.Text;
            xml["DeliveryZip"].Text = tbDeliveryZip.Text;
            SaveSTI(xml["DeliveryStreetType"], cbDeliveryStreetType, null);
            xml["DeliveryStreet"].Text = tbDeliveryStreet.Text;
            xml["DeliveryHouse"].Text = tbDeliveryHouse.Text;
            SaveSTI(xml["DeliveryBuildingType"], cbDeliveryBuildingType, null);
            xml["DeliveryBuilding"].Text = tbDeliveryBuilding.Text;
            SaveSTI(xml["DeliveryApartmentType"], cbDeliveryApartmentType, null);
            xml["DeliveryApartment"].Text = tbDeliveryApartment.Text;


            xml["ContactSex"].Text = cbContactSex.SelectedIndex.ToString();
            xml["ContactFio"].Text = tbContactFio.Text;
            
            xml["ContactFaxPrefix"].Text = tbContactFaxPrefix.Text;
            xml["ContactFax"].Text = tbContactFax.Text;
            xml["ContactEmail"].Text = tbContactEmail.Text;
            xml["ContactComment"].Text = tbContactComment.Text;

            xml["MSISDN"].Text = mtbMSISDN.Text;
            xml["OLDNEWDOLMSISDN"].Text = tbDynamicOldNumber.Text;
            xml["ICC"].Text = mtbICC.Text;
            try
            {
                xml["Plan"].Text = ((StringTagItem)cbPlan.SelectedItem).Tag;
            }
            catch (Exception) 
            {
                xml["Plan"].Text = "";
            }

            xml["checkCode"].Text = tbVerificationCode.Text;
            xml["boxType"].Text = tbBoxType.Text;

            //bool phoneEnter = false;
            //string newMsisdn = "";
            /*
            if (tbDynamicOldNumber.Text == "" && cbSimType.SelectedIndex == 1)
            {
                SetPhoneNumber dialog = new SetPhoneNumber();
                dialog.ShowDialog();
                if (dialog.tbNewNumber.Text.Length == 10)
                {
                    phoneEnter = true;
                    newMsisdn = dialog.tbNewNumber.Text;
                    xml["DocNum"].Text = dialog.tbDocNum.Text;

                    tbContactPhonePrefix.Text = dialog.tbNewNumber.Text.Substring(0,3);
                    tbContactPhone.Text = dialog.tbNewNumber.Text.Substring(3, 7);
                }
            }
            if (tbDynamicOldNumber.Text != "" && cbSimType.SelectedIndex == 1)
            {
                xml["OLDNEWDOLMSISDN"].Text = tbDynamicOldNumber.Text;
            }
            */ 

            xml["ContactPhonePrefix"].Text = tbContactPhonePrefix.Text;
            xml["ContactPhone"].Text = tbContactPhone.Text;

            try
            {
                //DataTable t = ((IDEXData)toolbox).getQuery("select dynamic from `um_data` where msisdn='" + xml["MSISDN"].Text + "' and icc = '" + xml["ICC"].Text + "'");
                //if (t != null && t.Rows.Count > 0)
                //{
                //xml["Dynamic"].Text = t.Rows[0]["dynamic"].ToString();
                xml["Dynamic"].Text = cbSimType.SelectedIndex.ToString();
                //}             
            }
            catch (Exception) 
            {
                
            }

            try
            {
                //DOL2BillPlan bp = (DOL2BillPlan)((StringObjTagItem)cbPlan.SelectedItem).Tag;

                DataTable dtPlans = ((IDEXData)toolbox).getQuery("select * from `beeline_billplans2` where soc='" + xml["Plan"].Text + "'");

                
                SimpleXML xmlPlan = xml["Plan"];
                xmlPlan.Text = dtPlans.Rows[0]["soc"].ToString();//.SOC;
                xmlPlan.Attributes["Accept"] = dtPlans.Rows[0]["accept"].ToString();//bp.Accept.ToString();
                xmlPlan.Attributes["CellnetsId"] = dtPlans.Rows[0]["cellnetsid"].ToString();//bp.CellnetsId.ToString();
                xmlPlan.Attributes["ChannellensId"] = dtPlans.Rows[0]["channellensid"].ToString();//bp.ChannellensId.ToString();
                xmlPlan.Attributes["Code"] = dtPlans.Rows[0]["code"].ToString(); //bp.Code;
                xmlPlan.Attributes["Enable"] = dtPlans.Rows[0]["enable"].ToString(); //bp.Enable.ToString();
                xmlPlan.Attributes["id"] = dtPlans.Rows[0]["id"].ToString(); //bp.id.ToString();
                xmlPlan.Attributes["Name"] = dtPlans.Rows[0]["name"].ToString(); //bp.Name;
                xmlPlan.Attributes["PaySystemsId"] = dtPlans.Rows[0]["paysystemsid"].ToString(); //bp.PaySystemsId.ToString();
                xmlPlan.Attributes["SOC"] = dtPlans.Rows[0]["soc"].ToString(); //bp.SOC;

                xml["PlanPrn"].Text = dtPlans.Rows[0]["name"].ToString();// bp.Name;
                
            }
            catch (Exception)
            {
            }

            SaveSTI(xml["DocBillDay"], cbDocBillDay, null);

            SimpleXML xmlSvc = xml["Services"];


            string prnServices = "";

            foreach (StringObjTagItem soti in clbServices.CheckedItems)
            {
                xmlSvc.CreateChild("Service").Attributes["code"] = ((DOL2Service)soti.Tag).Code;
                prnServices += ((DOL2Service)soti.Tag).Code + ":";
            }

            xml["PrnServices"].Text = prnServices;

            xml["Comments"].Text = tbComments.Text;

            SimpleXML xmlLogparams = xml["LogParams"];
            foreach (ListViewItem lvi in lvLogParams.Items)
            {
                SimpleXML x = xmlLogparams.CreateChild("Param");
                x.Attributes["code"] = (string)lvi.Tag;
                x.Attributes["status"] = lvi.Checked ? "1" : "0";
            }


            if (DutyId != null) xml["DutyId"].Text = DutyId;
            //
            //
            /*
            try
            {
                if ("1".Equals(xml["Dynamic"].Text) && driver != null)
                {
                    driver.Close();
                    driver.Quit();
                }
            }
            catch (Exception) { }
            */
            /*
            if ("1".Equals(xml["Dynamic"].Text))
            {
                if (DOL2Data.CheckOrgAttribute(newMsisdn, true, 10) && DOL2Data.CheckString(newMsisdn, false, true))
                {
                    if (tbDynamicOldNumber.Text == "")
                    {
                        xml["OLDNEWDOLMSISDN"].Text = xml["MSISDN"].Text;
                        xml["MSISDN"].Text = newMsisdn;
                    }
                }
               
            }
            */

            fdocument.documentDate = deJournalDate.Value.ToString("yyyyMMddhhmmssfff");
            fdocument.documentText = SimpleXML.SaveXml(xml);
            ArrayList err = module.ValidateDocument(toolbox, fdocument);

            /*
            // проверим корректность заполнения адреса регистрации и доставки корреспонденции
            if ((cbAddrBuildingType.SelectedIndex == 0 && tbAddrBuilding.Text.Trim().Length != 0) || (cbAddrBuildingType.SelectedIndex != 0 && tbAddrBuilding.Text.Trim().Length == 0)) err.Add("Адрес регистрации(прописки)/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");
            if ((cbAddrApartmentType.SelectedIndex == 0 && tbAddrApartment.Text.Trim().Length != 0) || (cbAddrApartmentType.SelectedIndex != 0 && tbAddrApartment.Text.Trim().Length == 0)) err.Add("Адрес регистрации(прописки)/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");

            if ((cbDeliveryBuildingType.SelectedIndex == 0 && tbDeliveryBuilding.Text.Trim().Length != 0) || (cbDeliveryBuildingType.SelectedIndex != 0 && tbDeliveryBuilding.Text.Trim().Length == 0)) err.Add("Адрес доставки корреспонденции/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");
            if ((cbDeliveryApartmentType.SelectedIndex == 0 && tbDeliveryApartment.Text.Trim().Length != 0) || (cbDeliveryApartmentType.SelectedIndex != 0 && tbDeliveryApartment.Text.Trim().Length == 0)) err.Add("Адрес доставки корреспонденции/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");

            if ((cbAddrBuildingType.SelectedIndex == 0 && tbAddrBuilding.Text.Trim().Length != 0) || (cbAddrBuildingType.SelectedIndex != 0 && tbAddrBuilding.Text.Trim().Length == 0)) err.Add("Адрес регистрации(прописки)/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");
            if ((cbAddrApartmentType.SelectedIndex == 0 && tbAddrApartment.Text.Trim().Length != 0) || (cbAddrApartmentType.SelectedIndex != 0 && tbAddrApartment.Text.Trim().Length == 0)) err.Add("Адрес регистрации(прописки)/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");

            if ((cbResidenceAddrBuildingType.SelectedIndex == 0 && tbResidenceAddrBuilding.Text.Trim().Length != 0) || (cbResidenceAddrBuildingType.SelectedIndex != 0 && tbResidenceAddrBuilding.Text.Trim().Length == 0)) err.Add("Адрес места пребывания/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");
            if ((cbResidenceAddrApartmentType.SelectedIndex == 0 && tbResidenceAddrApartment.Text.Trim().Length != 0) || (cbResidenceAddrApartmentType.SelectedIndex != 0 && tbResidenceAddrApartment.Text.Trim().Length == 0)) err.Add("Адрес места пребывания/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");
            */

            //if ((tbAddrBuilding.Text.Trim().Length == 0 && cbAddrBuildingType.SelectedIndex != 0) || (tbAddrBuilding.Text.Trim().Length != 0 && cbAddrBuildingType.SelectedIndex == 0)) err.Add("Адрес регистрации(прописки)/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");
            //if ((tbAddrBuilding.Text.Trim().Length == 0 && cbDeliveryBuildingType.SelectedIndex != 0) || (tbAddrBuilding.Text.Trim().Length != 0 && cbDeliveryBuildingType.SelectedIndex == 0)) err.Add("Адрес доставки корреспонденции/'Тип строения' и 'Строение' дожны быть либо оба заполены, либо оба пусты!");


            //if ((cbAddrApartmentType.SelectedIndex != 0 && tbAddrApartment.Text.Trim().Length == 0) || (cbAddrApartmentType.SelectedIndex == 0 && tbAddrApartment.Text.Trim().Length != 0)) err.Add("Адрес регистрации(прописки)/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");    
            //if ((cbDeliveryApartmentType.SelectedIndex != 0 && tbDeliveryBuilding.Text.Trim().Length == 0) || (cbDeliveryApartmentType.SelectedIndex == 0 && tbDeliveryApartment.Text.Trim().Length != 0)) err.Add("Адрес доставки корреспонденции/'Тип апартаментов' и 'Апартаменты' дожны быть либо оба заполены, либо оба пусты!");

            //if (cbDeliveryType.SelectedIndex == 0) err.Add("Адрес доставки корреспонденции/'Способ доставки' не может быть пустым!");
            //if (cbDocSphere.Text.Equals("")) err.Add("Учетные данные абонента/Сфера деятельности. Поле должно быть заполнено! Выберите значение из выпадающего списка с наименованием 'Другие'");
            //if (cbDocClientType.Text.Equals("")) err.Add("Учетные данные абонента/Тип клиента. Поле должно быть заполнено! Выберете значение из выпадающего списка с наименованием 'Частно лицо'");

            string vendor = "beeline";
            string vendorBase = "";
            string currentBase = ((IDEXUserData)toolbox).currentBase;

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
                //
                // Обновление справочников
                //

                if (ifChangeScan)
                {
                    try
                    {
                        // если есть скан, то сохраним

                        //string vendor = "beeline";
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
                        catch (Exception) 
                        {
                            MessageBox.Show("Операция передачи скана серверу DEX произошла с ошибкой. Проверьте корректность приложенного документа!(Открыть сохраненный договор и нажать кнопку \"Показать\" в месте "+
                                "прикладывания скана)");
                        }
                    }
                    catch (Exception) {
                        MessageBox.Show("Операция передачи скана серверу DEX произошла с ошибкой. Проверьте корректность приложенного документа!");
                    }
                }
                else if (ifScanIsset == true)
                {

                    xml["FizDocScan"].Text = "1";
                    xml["FizDocScanMime"].Text = lbScanMime.Text;
                    fdocument.documentText = SimpleXML.SaveXml(xml);
                }


                bool ifSub = false;
                // если данные подтверждены для добавления в автодок
                if (!isOnline)
                {
                    try
                    {
                        IDEXServices idis = (IDEXServices)toolbox;
                        JObject packet = new JObject();

                        JObject obj;
                        string cb = "";
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

                        if (chDulIsCorrect.Checked)
                        {
                            if (xml["FizDocScan"].Text.Equals("1"))
                            {
                                IDEXData d = (IDEXData)toolbox;
                                DataTable um_d = d.getQuery("select region_id FROM `um_data` where icc='{0}' AND msisdn = '{1}'", xml["ICC"].Text, xml["MSISDN"].Text);
                                
                                string mime;

                               
                                JObject data = new JObject();

                                
                                
                                if (ifChangeScan)
                                {
                                    mime = Path.GetExtension(lbScanPath.Text);
                                }
                                else
                                {
                                    mime = lbScanMime.Text;
                                }
                                string fizDocTypeBase = "";
                                if (xml["FizDocType"].Text.Equals("2")) fizDocTypeBase = "inistr";
                                else if (xml["FizDocType"].Text.Equals("1")) fizDocTypeBase = "ru";
                                else fizDocTypeBase = "undef";
                                packet = new JObject();
                                packet["com"] = "dexdealer.adapters.beeline";
                                packet["subcom"] = "apiInsertPasspAutodoc";
                                packet["data"] = new JObject();
                                packet["data"]["base"] = currentBase;
                                packet["data"]["unitid"] = fdocument.documentUnitID;
                                packet["data"]["vendor"] = "beeline";
                                packet["data"]["fullOperName"] = "beeline";
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
                                JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet), 0));


                               
                            }
                            else
                            {
                                MessageBox.Show("Скан не приложен, данные не добавлены в проверенный автодок");
                            }
                        }


                        // проверим не суб ли 
                        packet = new JObject();
                        packet["com"] = "dexdealer.adapters.beeline";
                        packet["subcom"] = "apiCheckFIOsubs";
                        packet["client"] = "dexol";
                        packet["data"] = new JObject();
                        packet["data"]["lastName"] = xml["LastName"].Text;
                        packet["data"]["firstName"] = xml["FirstName"].Text;
                        packet["data"]["secondName"] = xml["SecondName"].Text;
                        packet["data"]["birth"] = xml["Birth"].Text;
                        packet["data"]["vendor"] = "beeline";
                        packet["data"]["base"] = currentBase;

                        obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&clientType=dexol", 1));
                        //obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet), 0));
                        if (obj["data"]["ifIsset"].ToString().Equals("1"))
                        {
                            if (MessageBox.Show("Выявлено совпадение с субдилером",
                             "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ifSub = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        string ss = "sxv";
                    }
                }


                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["zip"] = tbAddrZip.Text;
                dic["city"] = tbAddrCity.Text;
                dic["region"] = tbAddrRegion.Text;
                ((IDEXCitySearcher)toolbox).setCityData(dic);

                dic.Clear();
                dic["zip"] = tbDeliveryZip.Text;
                dic["city"] = tbDeliveryCity.Text;
                dic["region"] = tbDeliveryRegion.Text;
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

                if (tbFizBirthPlace.Text.Trim() != "")
                    tb.setDataHint("city", tbFizBirthPlace.Text);

                //if (tbAddrCountry.Text.Trim() != "")
                //    tb.setDataHint("country", tbAddrCountry.Text);
                if (tbAddrStreet.Text.Trim() != "")
                    tb.setDataHint("street", tbAddrStreet.Text);
                if (tbAddrRegion.Text.Trim() != "")
                    tb.setDataHint("region", tbAddrRegion.Text);
                if (tbAddrState.Text.Trim() != "")
                    tb.setDataHint("state", tbAddrState.Text);
                if (tbAddrCity.Text.Trim() != "")
                    tb.setDataHint("city", tbAddrCity.Text);

                //if (tbDeliveryCountry.Text.Trim() != "")
                //    tb.setDataHint("country", tbDeliveryCountry.Text);
                if (tbDeliveryStreet.Text.Trim() != "")
                    tb.setDataHint("street", tbDeliveryStreet.Text);
                if (tbDeliveryRegion.Text.Trim() != "")
                    tb.setDataHint("region", tbDeliveryRegion.Text);
                if (tbDeliveryState.Text.Trim() != "")
                    tb.setDataHint("state", tbDeliveryState.Text);
                if (tbDeliveryCity.Text.Trim() != "")
                    tb.setDataHint("city", tbDeliveryCity.Text);

                //закроем web driver если открыт
                /*
                try
                {
                    if (driver != null)
                    {

                        if (MessageBox.Show("Закрыть адаптер и браузер?",
                            "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            driver.Close();
                            driver.Quit();
                        }
                       
                    }
                }
                catch (Exception) { }
                */

                if (!ifSub)
                {
                    try
                    {
                        IDEXConfig cfg = (IDEXConfig)toolbox;
                        cfg.setStr("Beeline.Form", "SUID", ((StringTagItem)cbDocUnit.SelectedItem).Tag);
                    }
                    catch (Exception) { }

                    fdocument.documentDigest = string.Format(
                        "{0}, {1}, {2} {3} {4}", xml["MSISDN"].Text, deDocDate.Text,
                        tbLastName.Text, tbFirstName.Text, tbSecondName.Text
                        );


                    DialogResult = DialogResult.OK;


                    // удалим сессию на сервере
                    
                }
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

        private void keybRu(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybRU();
        }

        private void keybEn(object sender, EventArgs e)
        {
            ((IDEXSysData)toolbox).keybEN();
        }

        public DataRow findCity(string title, bool isZip)
        {
            try
            {
                IDEXData d = (IDEXData)toolbox;

                DataRow[] rr = dtCity.Select((isZip ? "zip" : "city") + " = '" + d.EscapeString(title) + "'");
                if (rr != null && rr.Length > 0) return rr[0];

                /*
                string sql = string.Format("select * from `city` where {0} = '{1}'", isZip ? "zip" : "city", d.EscapeString(title));
                DataTable t = d.getQuery(sql);
                if (t != null && t.Rows.Count > 0) return t.Rows[0];
                 */
            }
            catch (Exception)
            {
            }
            return null;
        }

        private void cbDocUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;

                SelectNextControl((Control)sender, true, true, true, false);

            }
        }

       

        String[] addrTypeText = { "Адрес регистрации (прописки)", "Юридический адрес" };

        private void cbDocOrgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tp = cbDocOrgType.SelectedIndex;
            tcRequisites.TabPages.Remove(tpPassport);
            tcRequisites.TabPages.Remove(tpResidenceAddress);
            tcRequisites.TabPages.Remove(tpBank);
            tcRequisites.TabPages.Remove(tpRegAddress);
            tcRequisites.TabPages.Remove(tpCorrAdress);
            tcRequisites.TabPages.Remove(tpContactPerson);
            if (tp == 0) tcRequisites.TabPages.Add(tpPassport);
            tcRequisites.TabPages.Add(tpRegAddress);
            tcRequisites.TabPages.Add(tpResidenceAddress);
            tcRequisites.TabPages.Add(tpCorrAdress);
            tcRequisites.TabPages.Add(tpContactPerson);
            tcRequisites.TabPages.Add(tpBank);

            
            try
            {
                tpRegAddress1.Text = addrTypeText[tp];
            }
            catch (Exception)
            {
            }

            tcClientSp.TabPages.Remove(tpFiz);
            tcClientSp.TabPages.Remove(tpJur);
            if (tp == 0) tcClientSp.TabPages.Add(tpFiz);
            if (tp == 1) tcClientSp.TabPages.Add(tpJur);

//            bFromAbData.Visible = tp == 0;

            List<StringTagItem> clst = (tp == 0) ? dol2.persontypes : dol2.companytypes;
            cbDocClientType.Items.Clear();
            cbDocClientType.Items.AddRange(clst.ToArray());
            cbDocClientType.SelectedIndex = 0;
        }

        private void cbDocPaySystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int psid = int.Parse(((StringTagItem)cbDocPaySystem.SelectedItem).Tag);
                cbDocBillDay.Visible = psid == 1 || psid == 2;
                label51.Visible = psid == 1 || psid == 2;
            }
            catch (Exception)
            {
            }
        }

        private void cbDocNumberLength_SelectedIndexChanged(object sender, EventArgs e)
        {
/*
            try
            {
                BDBillPlan lastBp = null;
                if (cbPlan.SelectedItem != null) lastBp = (BDBillPlan)(((StringObjTagItem)cbPlan.SelectedItem).Tag);
                cbPlan.Items.Clear();
                if (cbDocNumberLength.SelectedItem != null)
                {
                    BDChannelLen ccl = (BDChannelLen)(((StringObjTagItem)cbDocNumberLength.SelectedItem).Tag);
                    if (ccl != null)
                    {
                        cbPlan.Items.AddRange(ccl.getBillPlansSOTI());
                        StringObjTagItem.SelectByTag(cbPlan, lastBp, true);
                    }
                }
            }
            catch (Exception)
            {
            }
 */
        }

        private void cbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            /** Это не нужно, оно ставит лишние галочки при переходе по ТП
            List<string> svcodes = new List<string>();

            foreach (StringObjTagItem soti in clbServices.CheckedItems)
            {
                try
                {
                    svcodes.Add(((DOL2Service)soti.Tag).Code);
                }
                catch (Exception)
                {
                }
            }
            */

            clbServices.Items.Clear();
            try
            {
                DOL2BillPlan bp = (DOL2BillPlan)((StringObjTagItem)cbPlan.SelectedItem).Tag;
                foreach (DOL2Service svc in bp.services)
                {
//                    bool svchecked = svcodes.Contains(svc.Code);
                    clbServices.Items.Add(
                        new StringObjTagItem(svc.Name + " [" + svc.Code + "]", svc), 
                        /*svchecked ||*/ svc.Mandatory
                        );
                }

                cbDocPaySystem.SelectedItem = null;
                StringTagItem.SelectByTag(cbDocPaySystem, bp.PaySystemsId.ToString(), false);
                
            }
            catch (Exception)
            {
            }
        }

        private void clbServices_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DOL2Service svc = (DOL2Service)((StringObjTagItem)clbServices.Items[e.Index]).Tag;
            if (svc.Mandatory) e.NewValue = CheckState.Checked;
        }

        private void cbDocOrgType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                tcMain.SelectedTab = tpHardware;
                //mtbMSISDN.Focus();

                mtbAssignedDPCode.Focus();
/*
                tcMain.SelectedTab = tpAbdata;
                if (cbDocOrgType.SelectedIndex == 0)
                {
                    tcClientSp.SelectedTab = tpFiz;
                    cbSex.Focus();
                }
                else if (cbDocOrgType.SelectedIndex == 1)
                {
                    tcClientSp.SelectedTab = tpJur;
                    tbCompanyTitle.Focus();
                }
 */
            }
        }

        private void mtbAssignedDPCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                
                tcMain.SelectedTab = tpHardware;
                //mtbMSISDN.Focus();
                //tbDynamicIcc.Focus();

                //if (isOnline)
                //{
                    //tcMain.Enabled = true;
                    //tcMain.SelectedTab = tpAbdata;
                    //tcMain.Focus();
                //}
                //else 
                clbServices.Focus();
            }
        }


        private void cbDocClientType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                if (cbDocOrgType.SelectedIndex == 0)
                {
                    tcClientSp.SelectedTab = tpFiz;
                    cbSex.Focus();
                }
                else if (cbDocOrgType.SelectedIndex == 1)
                {
                    tcClientSp.SelectedTab = tpJur;
                    tbCompanyTitle.Focus();
                }
            }
        }

        private void tbLastName_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = normName(((TextBox)sender).Text);
            tbContactFio.Text = string.Format("{0} {1} {2}", tbLastName.Text.Trim(), 
                tbFirstName.Text.Trim(), tbSecondName.Text.Trim());
            //tbContactPhonePrefix.Text = 
        }

        private void tbOrgBik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                tcAddress.SelectedTab = tpRegAddress1;
                //tbAddrCountry.Focus();
                cbAddrCountry.Focus();
            }
        }

        public bool doDeliveryFill()
        {
            try
            {
                return !deliveryTypeChanged && ((StringTagItem)cbDeliveryType.SelectedItem).Tag.Equals("1");
            }
            catch (Exception)
            {
            }
            return false;
        }

        private void TextBoxNormName_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = normName(((TextBox)sender).Text);
            //if (sender == tbAddrCountry && doDeliveryFill()) tbDeliveryCountry.Text = tbAddrCountry.Text;
            if (sender == cbAddrCountry && doDeliveryFill()) cbDeliveryCountry.Text = cbAddrCountry.Text;
        }

        private void tbAddrApartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                tbDeliveryApartment.Text = tbAddrApartment.Text;
                e.SuppressKeyPress = true;
                bOk_Click(bOk, new EventArgs());
            }
        }

        private void bGetFromAddr_Click(object sender, EventArgs e)
        {
            //tbDeliveryCountry.Text = cbAddrCountry.Text;
            cbDeliveryCountry.Text = cbAddrCountry.Text;
            tbDeliveryState.Text = tbAddrState.Text;
            tbDeliveryRegion.Text = tbAddrRegion.Text;
            cbDeliveryCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
            tbDeliveryCity.Text = tbAddrCity.Text;
            tbDeliveryZip.Text = tbAddrZip.Text;
            cbDeliveryStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
            tbDeliveryStreet.Text = tbAddrStreet.Text;
            tbDeliveryHouse.Text = tbAddrHouse.Text;
            cbDeliveryBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
            tbDeliveryBuilding.Text = tbAddrBuilding.Text;
            cbDeliveryApartmentType.SelectedIndex = cbAddrApartmentType.SelectedIndex;
            tbDeliveryApartment.Text = tbAddrApartment.Text;
            cbDeliveryType.Focus();
        }

        private void restate(Control c, bool enx, bool vix)
        {
            c.Enabled = enx;
            c.Visible = vix;
        }

        const string PDC_TEXT = "Платная доставка счетов(ID)";

        private void cbDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bDeliveryOption.Visible = false;
                cmsDeliveryOption.Items.Clear();

                int tag = int.Parse(((StringTagItem)cbDeliveryType.SelectedItem).Tag.Trim());

                bool deliveryInfo = (tag == 6 || tag == 7);

                if (tag == 1 || tag == 2 || tag == 4 || tag == 5 || tag == 10)
                { // По почте / Курьером / Офис дилера / Заказное письмо / Курьер платный (аванс)
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(tpRegAddress1.Text, null, new EventHandler(tsmiGetRegAddr_Click));
                    cmsDeliveryOption.Items.Add(tsmi);

                    if (tag == 10)
                    {
                        tbDeliveryComment.Text = PDC_TEXT;
                    }
                    else if (tbDeliveryComment.Text.CompareTo(PDC_TEXT) == 0)
                    {
                        tbDeliveryComment.Text = "";
                    }

                    bDeliveryOption.Visible = true;
                }

                if (tag == 3 || tag == 4)
                { // Офис ВЫМПЕЛКОМа
                    IDEXData d = (IDEXData)toolbox;
                    string dname = (tag == 3) ? dol2.libname("offices") : dol2.libname("dealeroffices");
//                    DataTable dt = d.getQuery(string.Format("select * from `{0}` order by name", d.EscapeString(dname)));
                    DataTable dt = d.getTable(d.EscapeString(dname));
                    dt.DefaultView.Sort = "name";
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach(DataRow row in dt.Rows)
                        {
                            ToolStripMenuItem tsmi = new ToolStripMenuItem(row["name"].ToString(), null, new EventHandler(tsmiGetDataRowAddr_Click));
                            tsmi.Tag = row;
                            cmsDeliveryOption.Items.Add(tsmi);
                        }
                    }
                    bDeliveryOption.Visible = true;
                }

                lDeliveryInfo.Visible = deliveryInfo;
                label25.Visible = !deliveryInfo;
                label28.Visible = !deliveryInfo;
                label29.Visible = !deliveryInfo;
                label30.Visible = !deliveryInfo;
                label31.Visible = !deliveryInfo;
                label32.Visible = !deliveryInfo;
                label33.Visible = !deliveryInfo;
                label34.Visible = !deliveryInfo;
                //tbDeliveryCountry.Visible = !deliveryInfo;
                cbDeliveryCountry.Visible = !deliveryInfo;
                tbDeliveryState.Visible = !deliveryInfo;
                tbDeliveryRegion.Visible = !deliveryInfo;
                cbDeliveryCityType.Visible = !deliveryInfo;
                tbDeliveryCity.Visible = !deliveryInfo;
                tbDeliveryZip.Visible = !deliveryInfo;
                cbDeliveryStreetType.Visible = !deliveryInfo;
                tbDeliveryStreet.Visible = !deliveryInfo;
                tbDeliveryHouse.Visible = !deliveryInfo;
                cbDeliveryBuildingType.Visible = !deliveryInfo;
                tbDeliveryBuilding.Visible = !deliveryInfo;
                cbDeliveryApartmentType.Visible = !deliveryInfo;
                tbDeliveryApartment.Visible = !deliveryInfo;

                deliveryTypeChanged = true;
            }
            catch (Exception)
            {
            }

        }

        private void tbContactComment_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void fillSim(Dictionary<string, string> dicsim)
        {
            try
            {
                cbPlan.Enabled = true;
                cbDocPaySystem.Enabled = true;
                lOwner.Text = "";

                if (dicsim != null)
                {
                    if (dicsim.ContainsKey("msisdn")) mtbMSISDN.Text = dicsim["msisdn"];
                    if (dicsim.ContainsKey("icc")) mtbICC.Text = dicsim["icc"];

                    if (dicsim.ContainsKey("plan_id"))
                    {
                        string pid = dicsim["plan_id"];

                        DataTable dtPlans = ((IDEXData)toolbox).getQuery("select * from `beeline_billplans2`");


                        //if (pid == "04NSALL") pid = "04NSALL";
                        //if (pid == "04V2_17") pid = "04V2_17  ";
                        //DOL2BillPlan bp = null;


                        //StringTagItem.SelectByTag(cbPlan, tpCode, false);

                        string paysystem = "";
                        foreach (DataRow drPlan in dtPlans.Rows)
                        {
                            if (drPlan["code"].ToString().Equals(pid))
                            {
                                paysystem = drPlan["paysystemsid"].ToString();
                                break;
                            }
                        }


                        //if (dol2.libBillplans.ContainsKey(pid)) bp = dol2.libBillplans[pid];
                        //if (bp != null)
                       // {
                            //StringObjTagItem.SelectByTag(cbPlan, bp, true);
                        StringTagItem.SelectByTag(cbPlan, pid, false);
                        cbPlan.Enabled = false;

                        StringTagItem.SelectByTag(cbDocPaySystem, paysystem, true);
                        cbDocPaySystem.Enabled = (cbDocPaySystem.SelectedItem == null);
                        //}
                    }

                    if (dicsim.ContainsKey("owner_title")) lOwner.Text = dicsim["owner_title"];
                    else lOwner.Text = "?";
                    if (dicsim.ContainsKey("region_title")) lOwner.Text += ", " + dicsim["region_title"];
                    if (dicsim.ContainsKey("owner_status")) lOwner.Text += ", " + ((Convert.ToBoolean(dicsim["owner_status"])) ? "[Активен]" : "[Заблокирован]");
                }
            }
            catch (Exception)
            {
            }
        }

        private void mtbMSISDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                fillSim(sim.getSimByMSISDN(mtbMSISDN.Text.Trim()));
                e.SuppressKeyPress = true;
                clbServices.Focus();
//                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private JObject authorization(int cnt)
        {
            IDEXServices idis = (IDEXServices)toolbox;
            JObject authData = new JObject();
            authData["login"] = ((IDEXUserData)toolbox).adaptersLogin == null ? "admin" : ((IDEXUserData)toolbox).adaptersLogin;
            authData["password"] = ((IDEXUserData)toolbox).adaptersPass == null ? "12473513" : ((IDEXUserData)toolbox).adaptersPass;
            //JObject authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
            JObject authObj = new JObject();
            bool auth = true;
            try
            {
                authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
            }
            catch (Exception)
            {
                auth = false;
            }

            if (!auth)
            {
                //MessageBox.Show("Ошибка авторизации. Попробуйте еще раз");
                //tbVerificationCode.Enabled = true;
                //btnCheckInputData.Enabled = true;
                //this.Enabled = true;
                if (cnt == 0) 
                {
                    return authObj;
                } 
                else 
                {
                    cnt--;
                    Thread.Sleep(1000);
                    return authorization(cnt);
                }
                
            }
            else
            {
                return authObj;
            }

        }

        private void checkSim()
        {
            string icc = "8970199" + tbDynamicIcc.Text;
            string checkCode = tbVerificationCode.Text;
            string msisdn = "", scadPointCode = "";
            string currentBase = ((IDEXUserData)toolbox).currentBase;
            string pfBase = "";

            //nodejsserver = "192.168.0.159";

            this.Enabled = false;
            bool s = false;

            // произведем авторизацию субдилера на сервере адаптеров
            string dexUid = "";
            //string querySelect = "select dynamic, msisdn, icc from `um_data` where icc = '" + icc + "' and msisdn like '%" + checkCode + "'";
            string querySelect = "select dynamic, msisdn, icc from `um_data` where icc = '" + icc + "' and msisdn REGEXP '" + checkCode + "$'";
            string dexUri = "";
            if (isOnline)
            {
               
                dexUid = ((IDEXUserData)toolbox).UID;

                querySelect += " and owner_id = '" + dexUid + "'";
                //dexUri = ((IDEXUserData)toolbox).
            }
            else
            {
                try
                {
                    dexUid = ((StringTagItem)cbDocUnit.SelectedItem).Tag;
                }
                catch (Exception)
                {
                    MessageBox.Show("Вы не выбрали отделение");
                }

                //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
                currentBase = ((IDEXUserData)toolbox).dataBase;
                
            }

            if (dexUid == "")
            {
                this.Enabled = true;
            }
            else
            {
                DataTable t = ((IDEXData)toolbox).getQuery(querySelect);



                JObject authObj = authorization(15);
                IDEXServices idis = (IDEXServices)toolbox;
                bool auth = true;
                if (authObj["message"].ToString() == null)
                {
                    auth = false;
                }


                //string dexServer = ((IDEXUserData)toolbox).dexServer;

                /*
                IDEXServices idis = (IDEXServices)toolbox;
                JObject authData = new JObject();
                authData["login"] = ((IDEXUserData)toolbox).adaptersLogin == null ? "admin" : ((IDEXUserData)toolbox).adaptersLogin;
                authData["password"] = ((IDEXUserData)toolbox).adaptersPass == null ? "12473513" : ((IDEXUserData)toolbox).adaptersPass;
                //JObject authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                JObject authObj = new JObject();
                bool auth = true;
                try
                {
                    int cnt = 10;
                    authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                }
                catch (Exception) 
                {
                    auth = false;
                }
                */

                if (!auth) 
                {
                    MessageBox.Show("Ошибка авторизации. Попробуйте еще раз");
                    tbVerificationCode.Enabled = true;
                    btnCheckInputData.Enabled = true;
                    this.Enabled = true;
                    return;
                }
                

                if (authObj["message"].ToString().Equals("error"))
                {
                    if (authObj["result"].ToString().Equals("not authorized"))
                    {
                        //MessageBox.Show("Ошибка авторизации. Возможно вам не разрешено пользоваться сервисом. Обратитесь в офис.");
                        string message = "Ошибка авторизации. \nВозможно, Вам не разрешено пользоваться сервисом. Обратитесь в офис.";
                        MessageShow newMessageShow = new MessageShow(message, "red", "Ошибка авторизации!");
                        if (newMessageShow.ShowDialog() == DialogResult.OK)
                        {
                        }
                        tbVerificationCode.Enabled = true;
                        btnCheckInputData.Enabled = true;
                    }

                }
                else
                {
                    adaptersUid = authObj["uid"].ToString();

                    //теперь подгрузим возможные базы
                    JObject objInfoBase = new JObject();
                    bool ifErrReq = false;
                    try
                    {
                        objInfoBase = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/adapters/getDexDexolBase?uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                    }
                    catch (Exception)
                    {
                        ifErrReq = true;

                    }
                    if (ifErrReq)
                    {
                        MessageBox.Show("Ошибка запроса. Повторите позднее.");
                    }
                    else
                    {
                        if (isOnline)
                        {
                            try
                            {

                                foreach (JObject jo in objInfoBase["data"])
                                {
                                    if (jo["dex_dexol_base_name"].ToString().Equals(currentBase))
                                    {
                                        pfBase = jo["tag"].ToString();
                                    }
                                }

                            }
                            catch (Exception) { }
                        }
                        else
                        {
                            try
                            {
                                foreach (JObject jo in objInfoBase["data"])
                                {
                                    if (jo["list"].ToString().Equals(currentBase))
                                    {
                                        pfBase = jo["tag"].ToString();
                                        currentBase = jo["dex_dexol_base_name"].ToString();
                                    }
                                }
                            }
                            catch (Exception) { }
                        }

                        if (t != null && t.Rows.Count > 0)
                        {
                            foreach (DataRow dr in t.Rows)
                            {
                                if (dr["msisdn"].ToString().Substring(6) == checkCode && dr["icc"].ToString().Substring(7) == tbDynamicIcc.Text)
                                {
                                    s = true;
                                    if ("1".Equals(t.Rows[0]["dynamic"].ToString()))
                                    {
                                        gbNewDol.Visible = true;
                                        bRegInNewDol.Visible = true;
                                        //tbDocNum.Visible = false;
                                        //label2.Visible = false;
                                        cbSimType.SelectedIndex = 1;
                                        mtbMSISDN.Text = t.Rows[0]["msisdn"].ToString();
                                        mtbICC.Text = icc;
                                    }
                                    else
                                    {
                                        gbNewDol.Visible = false;
                                        bRegInNewDol.Visible = false;
                                        //tbDocNum.Visible = true;
                                        //label2.Visible = true;
                                        cbSimType.SelectedIndex = 0;
                                        mtbMSISDN.Text = t.Rows[0]["msisdn"].ToString();
                                        mtbICC.Text = icc;
                                    }
                                    tcMain.Visible = true;
                                    fillSim(sim.getSimByMSISDN(mtbMSISDN.Text.Trim()));
                                    //e.SuppressKeyPress = true;
                                    //clbServices.Focus();
                                    //mtbAssignedDPCode.Focus();
                                    deJournalDate.Focus();
                                    break;
                                }
                            }
                            //clbServices.Focus();
                        }
                        else
                        {

                        }

                        if (!s)
                        {
                            btnCheckInputData.Enabled = true;
                            tbVerificationCode.Enabled = true;

                            //MessageBox.Show("Данная сим-карта отсутствует в справочнике сим-карт");

                            string message = "Данная SIM-карта отсутствует в справочнике SIM-карт. \nПроверьте корректность вносимых данных.";
                            MessageShow newMessageShow = new MessageShow(message, "red", "Получение информации о SIM-карте");
                            if (newMessageShow.ShowDialog() == DialogResult.OK)
                            {
                            }
                            this.Enabled = true;
                        }
                        else
                        {

                            //this.Size = new Size(840, 810);
                            // симка отписана на челокека и теепрь можно проверить какой тип имеет сим-карта для формирования интерфейса
                            // если динамическая, то необходимо для пользователя запросить список ТТ, чтобы он выбрал свою.
                            // если сим статическая, то показать данные формы для заполнения.

                            JObject packet = new JObject();
                            string tpCode = "";
                            string paysystemsid = "";
                            //data["pfBase"] = pfBase;
                            //data["icc"] = icc;
                            //data["checkCode"] = checkCode;

                            packet["com"] = "dexdealer.adapters.beeline";
                            packet["subcom"] = "apiGetSimInfo";
                            packet["client"] = "dexol";
                            packet["data"] = new JObject();
                            packet["data"]["vendor"] = "beeline";
                            packet["data"]["dexuid"] = dexUid;
                            packet["data"]["base"] = currentBase;
                            packet["data"]["icc"] = icc;
                            //packet["data"]["isOfis"] = isOnline == false ? 1 : 0;
                            if (!isOnline) packet["data"]["ignoreUid"] = 1;
                            else packet["data"]["ignoreUid"] = 0;
                            packet["data"]["checkCode"] = checkCode;


                            //string ssss = "37.29.115.178:3000/adapters/getSimInfo?data=" + JsonConvert.SerializeObject(data) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid;

                            //JObject obj = JObject.Parse(idis.sendRequest("GET", dexServer, "3000", "/adapters/getSimInfo?data=" + JsonConvert.SerializeObject(data) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                            JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                            int simStatus = Convert.ToInt32(obj["data"]["status"].ToString());

                            if (simStatus != 1)
                            {
                                mtbMSISDN.Text = "";
                                mtbICC.Text = "";
                                btnCheckInputData.Enabled = true;
                                tbVerificationCode.Enabled = true;
                                cbPlan.Enabled = true;
                                cbDocPaySystem.Enabled = true;
                                MessageBox.Show(obj["data"]["message"].ToString());
                                this.Enabled = true;
                            }



                            /*
                            if (obj["data"]["status"].ToString().Equals("-5"))
                            {
                                string message = "Cим-карта обнаружена в справочнике сим-карт,"+'\n'+" но в справочнике links2 отсутствует. Обратитесь в офис!";
                                MessageShow newMessageShow = new MessageShow(message, "red", "Получение информации о SIM-карте");
                                mtbMSISDN.Text = "";
                                mtbICC.Text = "";
                                if (newMessageShow.ShowDialog() == DialogResult.OK)
                                {
                                }
                                btnCheckInputData.Enabled = true;
                                tbVerificationCode.Enabled = true;
                            }
                            if (obj["data"]["status"].ToString().Equals("-1") || obj["data"]["status"].ToString().Equals("-3") || obj["data"]["status"].ToString().Equals("-5"))
                            {
                       
                                //MessageBox.Show("Код ошибки -10. Обратитесь в офис");
                                string message = obj["data"]["message"].ToString() + " Обратитесь  в офис!";
                                MessageShow newMessageShow = new MessageShow(message, "red", "Получение информации о SIM-карте");
                                if (newMessageShow.ShowDialog() == DialogResult.OK)
                                {
                                }
                                btnCheckInputData.Enabled = false;
                                tbVerificationCode.Enabled = false;

                            }
                            else if (obj["data"]["status"].ToString().Equals("-2"))
                            {
                                btnCheckInputData.Enabled = false;
                                tbVerificationCode.Enabled = false;
                                //MessageBox.Show("Код ошибки -2. Обратитесь в офис");

                                string message = "SIM-карта не распределена или не отсписана на данного субдилера. Обратитесь в офис";
                                mtbMSISDN.Text = "";
                                mtbICC.Text = "";
                                cbDocPaySystem.Enabled = true;
                                cbDocBillDay.Enabled = true;
                                cbPlan.Enabled = true;
                                tbVerificationCode.Enabled = true;
                                btnCheckInputData.Enabled = true;
                                this.Enabled = true;

                                MessageShow newMessageShow = new MessageShow(message, "red", "Получение информации о SIM-карте");
                                if (newMessageShow.ShowDialog() == DialogResult.OK)
                                {
                                }
                            }
                            */
                            else
                            {
                                string boxtype = obj["data"]["boxtype"].ToString();
                                string sernum = obj["data"]["sernum"].ToString();
                                string snb = obj["data"]["snb"].ToString();
                                string soc = obj["data"]["soc"].ToString();
                                tpCode = soc;

                                // узнаем тип оплаты
                                packet = new JObject();
                                packet["com"] = "dexdealer.adapters.beeline";
                                packet["subcom"] = "apiGetPaySystemsId";
                                packet["client"] = "dexol";
                                packet["data"] = new JObject();
                                packet["data"]["vendor"] = "beeline";
                                packet["data"]["dexuid"] = dexUid;
                                packet["data"]["base"] = currentBase;
                                packet["data"]["soc"] = obj["data"]["soc"];
                                packet["data"]["icc"] = icc;
                                packet["data"]["checkCode"] = checkCode;

                                obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                                bool next = true;
                                try
                                {
                                    paysystemsid = obj["data"]["paysystemsid"].ToString();
                                }
                                catch (Exception) 
                                {
                                    MessageBox.Show("Ошибка в процессе получения типа оплаты для sim-карты");
                                    btnCheckInputData.Enabled = true;
                                  
                                    this.Enabled = true;



                                    next = false;
                                }

                                bool openDog = false;
                                if (next)
                                {
                                    if (Convert.ToInt32(obj["data"]["status"].ToString()) == 1)
                                    {
                                        if ("D".Equals(boxtype) || "U".Equals(boxtype)) // динамический комплект. D - без номера
                                        {
                                            DataTable dtPlans = ((IDEXData)toolbox).getQuery("select * from `um_plans`");
                                            if (!isOnline)
                                            {
                                                DataTable dtAllUnuts = ((IDEXData)toolbox).getQuery("select * from `units` where status = 1 order by title");
                                                string qstr = "select owner_id from `um_data` where icc = '" + icc + "' and msisdn REGEXP '" + checkCode + "$'";
                                                DataTable dtPid = ((IDEXData)toolbox).getQuery(qstr);

                                                dexUid = dtPid.Rows[0]["owner_id"].ToString();
                                                //SelectUnit su = new SelectUnit(dtAllUnuts, dtPid.Rows[0]["owner_id"].ToString());
                                                //if (su.ShowDialog() == DialogResult.OK)
                                                //{
                                                //    dexUid = StringTagItem.getSelectedTag(su.cbUnits, "");
                                                //    StringTagItem.SelectByTag(cbDocUnit, dexUid, false);
                                                //}
                                            }

                                            ActivateSim acs = new ActivateSim(toolbox, boxtype, sernum, snb, soc, dtPlans, checkCode, adaptersUid, pfBase, dexUid, nodejsserver, currentBase);
                                            try
                                            {
                                                if (acs.ShowDialog() == DialogResult.OK)
                                                {
                                                    try
                                                    {
                                                        scadPointCode = StringTagItem.getSelectedTag(acs.cbScadPoints, "");
                                                        //string msisdnTest = acs.cbNumbers.Text;
                                                        tpCode = StringTagItem.getSelectedTag(acs.cbPlans, "");

                                                        msisdn = acs.cbNumbers.Text;
                                                        tbDynamicIcc.Enabled = false;
                                                        tbVerificationCode.Enabled = false;
                                                        StringTagItem.SelectByTag(cbPlan, tpCode, false);
                                                        cbPlan.Enabled = false;
                                                        openDog = true;
                                                        btnCheckInputData.Visible = false;
                                                        mtbAssignedDPCode.Enabled = false;

                                                        /*
                                                        // теперь можно активировать сим с номером
                                                        packet = new JObject();
                                                        packet["com"] = "dexdealer.adapters.beeline";
                                                        packet["subcom"] = "apiActivateDynamicSIM2";
                                                        packet["client"] = "dexol";
                                                        packet["data"] = new JObject();
                                                        packet["data"]["vendor"] = "beeline";
                                                        packet["data"]["dexuid"] = dexUid;
                                                        packet["data"]["base"] = currentBase;
                                                        packet["data"]["soc"] = soc;
                                                        packet["data"]["beautifulCTN"] = msisdnTest;
                                                        packet["data"]["icc"] = sernum;
                                                        packet["data"]["checkCode"] = checkCode;
                                                        packet["data"]["skadPointCode"] = scadPointCode;
                                            
                                                        obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));


                                                        int status = Convert.ToInt32(obj["data"]["status"].ToString());
                                                        if (status != -1 && status != -2 && status != -5 && status != -4 && status != -7)
                                                        {
                                                            if (status == 1)
                                                            {
                                                            }
                                                            else
                                                            {
                                                            }
                                                            // SIM была активирована ранее
                                                            msisdn = obj["data"]["CTN"].ToString();
                                                            openDog = true;
                                                            btnCheckInputData.Visible = false;
                                                            mtbAssignedDPCode.Enabled = false;

                                                            tbDynamicIcc.Enabled = false;
                                                            tbVerificationCode.Enabled = false;
                                                            StringTagItem.SelectByTag(cbPlan, tpCode, false);
                                                            cbPlan.Enabled = false;
                                                            //MessageShow newMessageShow = new MessageShow(message, "red", "Активация SIM-карты");
                                                        }
                                                        else
                                                        {
                                                
                                                            string message = obj["data"]["RUCheckDynamicSIMResult"].ToString();
                                                            if (status == -7) message = obj["data"]["activationError"].ToString();
                                                            MessageShow newMessageShow = new MessageShow(message, "red", "Активация SIM-карты");
                                                        }

                                                        */

                                                        /*
                                                        JObject data = new JObject();
                                                        data["pfBase"] = pfBase;
                                                        data["bindingNumber"] = msisdnTest;
                                                        data["scadPoinCode"] = scadPointCode;
                                                        data["icc"] = icc;
                                                        data["checkCode"] = checkCode;
                                                        data["soc"] = tpCode;
                                                        obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/adapters/activatedSimRequest?data=" + JsonConvert.SerializeObject(data) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 0));

                                        
                                                        int status = Convert.ToInt32(obj["data"]["status"].ToString());
                                                        if (status == 1 || status == 42 || status == 44)
                                                        {
                                                            //MessageBox.Show(obj["data"]["message"].ToString());
                                                            string message = obj["data"]["message"].ToString();
                                                            MessageShow newMessageShow = new MessageShow(message, "red", "Активация SIM-карты");
                                                            if (newMessageShow.ShowDialog() == DialogResult.OK)
                                                            {
                                                            }
                                                            try
                                                            {
                                                                msisdn = obj["data"]["bindingNumber"].ToString();
                                                            }
                                                            catch (Exception) 
                                                            {
                                                                if (status == 44)
                                                                {
                                                                    msisdn = msisdnTest;
                                                                }
                                                            }
                                                            openDog = true;
                                                            btnCheckInputData.Visible = false;
                                                            mtbAssignedDPCode.Enabled = false;

                                                            tbDynamicIcc.Enabled = false;
                                                            tbVerificationCode.Enabled = false;
                                                            StringTagItem.SelectByTag(cbPlan, tpCode, false);
                                                            cbPlan.Enabled = false;
                                                        }
                                                        else if (status == 41)
                                                        {
                                                            //ошибка активации сим-карты
                                                            //MessageBox.Show(obj["data"]["message"].ToString());
                                                            string message = obj["data"]["message"].ToString();
                                                            MessageShow newMessageShow = new MessageShow(message, "red", "Активация SIM-карты");
                                                            if (newMessageShow.ShowDialog() == DialogResult.OK)
                                                            {
                                                            }
                                                        }
                                                        */

                                                    }
                                                    catch (Exception) { }
                                                }
                                                else
                                                {
                                                    this.Enabled = true;
                                                    mtbMSISDN.Text = "";
                                                    btnCheckInputData.Enabled = true;
                                                    tbVerificationCode.Enabled = true;
                                                    tbDynamicOldNumber.Text = snb;
                                                    tbBoxType.Text = boxtype;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                this.Enabled = true;
                                                mtbMSISDN.Text = "";
                                                btnCheckInputData.Enabled = true;
                                                tbVerificationCode.Enabled = true;
                                                tbDynamicOldNumber.Text = snb;
                                                tbBoxType.Text = boxtype;
                                            }

                                        }
                                        else if ("P".Equals(boxtype) || "C".Equals(boxtype) || "O".Equals(boxtype) || "".Equals(boxtype) || "S".Equals(boxtype)) // статическая сим
                                        {
                                            //получим точки скад для суба
                                            packet = new JObject();
                                            packet["com"] = "dexdealer.adapters.beeline";
                                            packet["subcom"] = "apiGetScadPoints";
                                            packet["client"] = "dexol";
                                            packet["data"] = new JObject();
                                            packet["data"]["vendor"] = "beeline";
                                            packet["data"]["dexuid"] = dexUid;
                                            packet["data"]["base"] = currentBase;
                                            packet["data"]["icc"] = icc;
                                            packet["data"]["checkCode"] = checkCode;
                                            obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid, 1));
                                            foreach (JObject jo in obj["data"]["pointList"])
                                            {
                                                scadPointCode = jo["scadPointCode"].ToString();
                                                break;
                                            }

                                            btnCheckInputData.Visible = false;
                                            openDog = true;
                                            msisdn = snb;
                                            StringTagItem.SelectByTag(cbPlan, tpCode, false);


                                        }
                                        else // все остальные комплекты. (P,C,O, пустое значение). По идее будут отгружаться только P(скорее всего это статическая сим)
                                        {
                                        }

                                        if (openDog)
                                        {
                                            this.Enabled = true;
                                            tcMain.Enabled = true;
                                            tbBoxType.Text = boxtype;
                                            tbDynamicOldNumber.Text = snb;
                                            mtbAssignedDPCode.Text = scadPointCode;

                                            deJournalDate.Focus();

                                            StringTagItem.SelectByTag(cbDocPaySystem, paysystemsid, false);
                                            mtbMSISDN.Text = msisdn;
                                            mtbMSISDN.Enabled = false;
                                            this.Size = new Size(840, 644);
                                        }
                                    }
                                    else
                                    {
                                        //MessageBox.Show("Введены неверные данные!");
                                        string message = "Введены неверные данные!";
                                        MessageShow newMessageShow = new MessageShow(message, "red", "Получение информации о SIM-карте");
                                        if (newMessageShow.ShowDialog() == DialogResult.OK)
                                        {
                                        }
                                        this.Enabled = true;
                                    }
                                }
                                //JObject obj = JObject.Parse(str);

                            }
                            /*
                            if (t != null && t.Rows.Count == 1)
                            {

                                if ("1".Equals(t.Rows[0]["dynamic"].ToString()))
                                {
                                    gbNewDol.Visible = true;
                                    cbSimType.SelectedIndex = 1;
                                    //mtbMSISDN.vi = 
                                    mtbMSISDN.Text = t.Rows[0]["msisdn"].ToString();
                                    mtbICC.Text = icc;
                                }
                                else
                                {
                                    gbNewDol.Visible = false;
                                    cbSimType.SelectedIndex = 0;
                                    mtbMSISDN.Text = t.Rows[0]["msisdn"].ToString();
                                    mtbICC.Text = icc;
                                }
                                tcMain.Visible = true;
                                fillSim(sim.getSimByMSISDN(mtbMSISDN.Text.Trim()));
                                e.SuppressKeyPress = true;
                                clbServices.Focus();
                            }
                            else
                            {
                                MessageBox.Show("Данная сим-карта отсутствует в справочнике сим-карт");
                            }
                            */
                            //                SelectNextControl((Control)sender, true, true, true, false);

                        }
                    }

                }
            }
            
        }

        private void iccType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                btnCheckInputData.Enabled = false;
                //tbVerificationCode.Enabled = false;
                checkSim();
                e.SuppressKeyPress = true;
            }
        }

        private void tbFizInn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                if (cbDocOrgType.SelectedIndex == 0)
                {
                    tcAddress.SelectedTab = tpRegAddress1;
                    //tbAddrCountry.Focus();
                    cbAddrCountry.Focus();
                }
                else if (cbDocOrgType.SelectedIndex == 1)
                {
                    tcRequisites.SelectedTab = tpBank;
                    tbOrgBank.Focus();
                }
            }
        }

        private void bDeliveryOption_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            cmsDeliveryOption.Show(btn, new Point(btn.Width, 0));
        }

        private void tsmiGetRegAddr_Click(object sender, EventArgs e)
        {
            //TODO Вставить адрес регистрации/прописки в доставку
            //tbDeliveryCountry.Text = cbAddrCountry.Text;
            cbDeliveryCountry.Text = cbAddrCountry.Text;
            tbDeliveryState.Text = tbAddrState.Text;
            tbDeliveryRegion.Text = tbAddrRegion.Text;
            cbDeliveryCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
            tbDeliveryCity.Text = tbAddrCity.Text;
            tbDeliveryZip.Text = tbAddrZip.Text;
            cbDeliveryStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
            tbDeliveryStreet.Text = tbAddrStreet.Text;
            tbDeliveryHouse.Text = tbAddrHouse.Text;
            cbDeliveryBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
            tbDeliveryBuilding.Text = tbAddrBuilding.Text;
            cbDeliveryApartmentType.SelectedIndex = cbAddrApartmentType.SelectedIndex;
            tbDeliveryApartment.Text = tbAddrApartment.Text;
        }

        private void tsmiGetDataRowAddr_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow r = (DataRow)((ToolStripMenuItem)sender).Tag;

                //tbDeliveryCountry.Text = r["country"].ToString();
                cbDeliveryCountry.Text = r["country"].ToString();
                
                tbDeliveryState.Text = r["area"].ToString();
                tbDeliveryRegion.Text = r["region"].ToString();
                StringTagItem.SelectByTag(cbDeliveryCityType, r["place_type"].ToString(), true);
                tbDeliveryCity.Text = r["place_value"].ToString();
                tbDeliveryZip.Text = r["zip"].ToString();
                StringTagItem.SelectByTag(cbDeliveryStreetType, r["street_type"].ToString(), true);
                tbDeliveryStreet.Text = r["street_value"].ToString();
                tbDeliveryHouse.Text = r["house"].ToString();
                StringTagItem.SelectByTag(cbDeliveryBuildingType, r["building_type"].ToString(), true);
                tbDeliveryBuilding.Text = r["building_value"].ToString();
                StringTagItem.SelectByTag(cbDeliveryApartmentType, r["room_type"].ToString(), true);
                tbDeliveryApartment.Text = r["room_value"].ToString();
            }
            catch (Exception)
            {
            }
        }

        private void cbFizDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool nomask = true;
            try
            {
                int i = int.Parse(((StringTagItem)cbFizDocType.SelectedItem).Tag);
                if (i == 1) nomask = false;
            }
            catch (Exception)
            {
            }

            if (nomask)
            {
                mtbFizDocNumber.Mask = "";
                mtbFizDocSeries.Mask = "";
            }
            else
            {
                mtbFizDocNumber.Mask = "000000";
                mtbFizDocSeries.Mask = "00 00";
            }
        }

        private void setPeopleDataToFields(StringList pdata, string phash2)
        { //TODO Here
            try { cbSex.SelectedIndex = int.Parse(pdata["Sex"]); }
            catch (Exception) { }
            tbFirstName.Text = pdata["FirstName"];
            tbSecondName.Text = pdata["SecondName"];
            tbLastName.Text = pdata["LastName"];
            deBirth.Text = pdata["Birth"];

            //try { cbFizDocType.SelectedIndex = int.Parse(pdata["FizDocType"])-1; }
            //catch (Exception) { cbFizDocType.SelectedIndex = -1; }

            StringTagItem.SelectByTag(cbFizDocType, pdata["FizDocType"], true);

            mtbFizDocSeries.Text = pdata["FizDocSeries"];
            mtbFizDocNumber.Text = pdata["FizDocNumber"];
            tbFizDocOrg.Text = pdata["FizDocOrg"];
            try { tbFizDocOrgCode.Text = pdata["FizDocOrgCode"]; }
            catch (Exception) { }
            cbFizDocScan.Checked = "X".Equals(pdata["FizDocScan"]);
            deFizDocDate.Text = pdata["FizDocDate"];
            tbFizBirthPlace.Text = pdata["FizBirthPlace"];
            tbFizInn.Text = pdata["FizInn"];
            tbOrgBank.Text = pdata["OrgBank"];
            tbOrgRs.Text = pdata["OrgRs"];
            tbOrgKs.Text = pdata["OrgKs"];
            tbOrgBik.Text = pdata["OrgBik"];
            //tbAddrCountry.Text = pdata["AddrCountry"];
            cbAddrCountry.Text = pdata["AddrCountry"];
            tbAddrState.Text = pdata["AddrState"];
            
            tbAddrRegion.Text = pdata["AddrRegion"];

            StringTagItem.SelectByTag(cbAddrCityType, pdata["AddrCityType"], true);
            tbAddrCity.Text = pdata["AddrCity"];
            tbAddrZip.Text = pdata["AddrZip"];
            StringTagItem.SelectByTag(cbAddrStreetType, pdata["AddrStreetType"], true);
            tbAddrStreet.Text = pdata["AddrStreet"];
            tbAddrHouse.Text = pdata["AddrHouse"];
            StringTagItem.SelectByTag(cbAddrBuildingType, pdata["AddrBuildingType"], true);
            tbAddrBuilding.Text = pdata["AddrBuilding"];
            StringTagItem.SelectByTag(cbAddrApartmentType, pdata["AddrApartmentType"], true);
            tbAddrApartment.Text = pdata["AddrApartment"];

            if (doDeliveryFill())
            {
                //tbDeliveryCountry.Text = pdata["AddrCountry"];
                cbDeliveryCountry.Text = pdata["AddrCountry"];
                tbDeliveryState.Text = pdata["AddrState"];
                tbDeliveryRegion.Text = pdata["AddrRegion"];
                StringTagItem.SelectByTag(cbDeliveryCityType, pdata["AddrCityType"], true);
                tbDeliveryCity.Text = pdata["AddrCity"];
                tbDeliveryZip.Text = pdata["AddrZip"];
                StringTagItem.SelectByTag(cbDeliveryStreetType, pdata["AddrStreetType"], true);
                tbDeliveryStreet.Text = pdata["AddrStreet"];
                tbDeliveryHouse.Text = pdata["AddrHouse"];
                StringTagItem.SelectByTag(cbDeliveryBuildingType, pdata["AddrBuildingType"], true);
                tbDeliveryBuilding.Text = pdata["AddrBuilding"];
                StringTagItem.SelectByTag(cbDeliveryApartmentType, pdata["AddrApartmentType"], true);
                tbDeliveryApartment.Text = pdata["AddrApartment"];
            }

            if ("1".Equals(pdata["suspect"]))
            {
                MessageBox.Show("Возможно, у этого человека указаны неверные паспортные данные!\nПожалуйста, внимательно проверьте сведения.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        ArrayList phash;

        public string DoPeopleSearch(IWaitMessageEventArgs wmea)
        {//TODO Here
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

        private void bSearchName_Click(object sender, EventArgs e)
        {//TODO Here
            
            string rt = WaitMessage.Execute(new WaitMessageEvent(DoPeopleSearch));

            if (rt != null && rt.Length > 0)
            {
                MessageBox.Show(rt);
                mtbFizDocSeries.Focus();
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

        private void cbSex_Leave(object sender, EventArgs e)
        {
            cbContactSex.SelectedIndex = cbSex.SelectedIndex;
        }

        String GetDefValue(ComboBox cb, string def)
        {
            try
            {
                return ((StringTagItem)cb.SelectedItem).Tag;
            }
            catch (Exception)
            {
            }
            return def;
        }

        private void bSaveDefaults_Click(object sender, EventArgs e)
        {
            StringList defs = new StringList();
            defs["DocOrgType"] = cbDocOrgType.SelectedIndex.ToString();
            defs["DocSphere"] = GetDefValue(cbDocSphere, "");
            defs["DocClientType"] = GetDefValue(cbDocClientType, "");
            defs["Sex"] = cbSex.SelectedIndex.ToString();
            defs["DocCity"] = tbDocCity.Text;
            defs["FizDocType"] = GetDefValue(cbFizDocType, "");
            defs["FizDocScan"] = cbFizDocScan.Checked ? "X" : "-";
            //defs["AddrCountry"] = tbAddrCountry.Text;
            defs["AddrCountry"] = cbAddrCountry.Text;
            defs["AddrCityType"] = GetDefValue(cbAddrCityType, "");
            defs["AddrStreetType"] = GetDefValue(cbAddrStreetType, "");
            defs["AddrBuildingType"] = GetDefValue(cbAddrBuildingType, "");
            defs["AddrApartmentType"] = GetDefValue(cbAddrApartmentType, "");
            //defs["DeliveryCountry"] = tbDeliveryCountry.Text;
            defs["DeliveryCountry"] = cbDeliveryCountry.Text;
            defs["DeliveryType"] = GetDefValue(cbDeliveryType, "");
            defs["DeliveryComment"] = tbDeliveryComment.Text;
            defs["DeliveryCityType"] = GetDefValue(cbDeliveryCityType, "");
            defs["DeliveryStreetType"] = GetDefValue(cbDeliveryStreetType, "");
            defs["DeliveryBuildingType"] = GetDefValue(cbDeliveryBuildingType, "");
            defs["DeliveryApartmentType"] = GetDefValue(cbDeliveryApartmentType, "");
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setStr(module.ID, "DefaultValues", defs.SaveToString());

            MessageBox.Show("Значения по умолчанию сохранены");
        }

        private void tbAddrCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                DataRow drCity = findCity(tbAddrCity.Text.Trim(), false);
                if (drCity != null)
                {
                    try
                    {
                        tbAddrZip.Text = drCity["zip"].ToString();
                        if (!drCity["region"].ToString().Trim().Equals(""))
                        {
                            tbAddrRegion.Text = drCity["region"].ToString();
                            
                        }
                        StringList sl = new StringList(drCity["data"].ToString());
                        StringTagItem.SelectByTag(cbAddrCityType, sl["type"], false);
                    }
                    catch (Exception)
                    {
                    }
                }
                e.SuppressKeyPress = true;
                tbAddrCity.Text = normName(tbAddrCity.Text);
                tbDeliveryCity.Text = tbAddrCity.Text;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void clbServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;
                tcMain.SelectedTab = tpAbdata;
                cbDocSphere.Focus();
            }

        }

        private void tbFizBirthPlace_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbFizBirthPlace.Text = tbFizBirthPlace.Text.Substring(0, 1).ToUpper() + tbFizBirthPlace.Text.Remove(0, 1);
                    //deBirth.Focus();
                    cbCitizenType.Focus();
                    //tcRequisites.SelectedTab = tpRegAddress;
                    //cbAddrCountry.Focus();
                }
            }
            catch (Exception) 
            {
            }
        }

        private void tbAddrState_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                ((TextBox)sender).Text = normName(((TextBox)sender).Text);
            }
            
            if (doDeliveryFill())
            {
                if (sender == tbAddrState) tbDeliveryState.Text = tbAddrState.Text;
                if (sender == tbAddrRegion) tbDeliveryRegion.Text = tbAddrRegion.Text;
                if (sender == cbAddrCityType)
                {
                    StringTagItem.SelectByTag(cbDeliveryCityType, ((StringTagItem)cbAddrCityType.SelectedItem).Tag, false);
                }
                if (sender == tbAddrCity)
                {
                    tbDeliveryCity.Text = tbAddrCity.Text;
                    //if (tbFizBirthPlace.Text.Trim().Equals("")) tbFizBirthPlace.Text = tbAddrCity.Text;
                }

                if (sender == tbAddrZip) tbDeliveryZip.Text = tbAddrZip.Text;
                if (sender == cbAddrStreetType)
                {
                    StringTagItem.SelectByTag(cbDeliveryStreetType, ((StringTagItem)cbAddrStreetType.SelectedItem).Tag, false);
                }
                if (sender == cbAddrBuildingType)
                {
                    StringTagItem.SelectByTag(cbDeliveryBuildingType, ((StringTagItem)cbAddrBuildingType.SelectedItem).Tag, false);
                }
                if (sender == cbAddrApartmentType)
                {
                    StringTagItem.SelectByTag(cbDeliveryApartmentType, ((StringTagItem)cbAddrApartmentType.SelectedItem).Tag, false);
                }
                if (sender == tbAddrStreet) tbDeliveryStreet.Text = tbAddrStreet.Text;
                if (sender == tbAddrHouse) tbDeliveryHouse.Text = tbAddrHouse.Text;
                if (sender == tbAddrBuilding) tbDeliveryBuilding.Text = tbAddrBuilding.Text;
            }
        }

        private void cbCitizenType_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                lbCitizenshipText.Enabled = false;
                cbCitizenship.Enabled = false;
                lbMigarionCardSeries.Enabled = false;
                lbMigarionCardNumber.Enabled = false;
                lbMigrationCardDateStart.Enabled = false;
                lbMigrationCardDateFinish.Enabled = false;

                tbMigrationCardSeries.Enabled = false;
                tbMigrationCardNumber.Enabled = false;
                deMigrationCardDateStart.Enabled = false;
                deMigrationCardDateFinish.Enabled = false;

                tbMigrationCardSeries.Text = "";
                tbMigrationCardNumber.Text = "";
                deMigrationCardDateStart.Text = "";
                deMigrationCardDateFinish.Text = "";

                StringTagItem.SelectByTag(cbCitizenship, rus, false);
            }
            else
            {
                lbCitizenshipText.Enabled = true;
                cbCitizenship.Enabled = true;
                lbMigarionCardSeries.Enabled = true;
                lbMigarionCardNumber.Enabled = true;
                lbMigrationCardDateStart.Enabled = true;
                lbMigrationCardDateFinish.Enabled = true;

                tbMigrationCardSeries.Enabled = true;
                tbMigrationCardNumber.Enabled = true;
                deMigrationCardDateStart.Enabled = true;
                deMigrationCardDateFinish.Enabled = true;

                /*
                tbMigrationCardSeries.Text = "";
                tbMigrationCardNumber.Text = "";
                deMigrationCardDateStart.Text = "";
                deMigrationCardDateFinish.Text = "";
                */
            }
        }

        private void cbDocUnitOrg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                // проверим, есть ли такое значение в справочнике кодов подразделений
                try
                {
                    DataTable t = ((IDEXData)toolbox).getQuery("select * from `org_codes` where title LIKE '%" + ((TextBox)sender).Text + "%'");
                    if (t.Rows.Count == 1) 
                    {
                        tbFizDocOrgCode.Text = t.Rows[0]["code"].ToString();
                    }
                } catch (Exception) {}
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void bSaveAccNewDol_Click(object sender, EventArgs e)
        {
            StringList defs = new StringList();
            defs["DocOrgType"] = cbDocOrgType.SelectedIndex.ToString();
            defs["DocSphere"] = GetDefValue(cbDocSphere, "");
            defs["DocClientType"] = GetDefValue(cbDocClientType, "");
            defs["Sex"] = cbSex.SelectedIndex.ToString();
            defs["FizDocType"] = GetDefValue(cbFizDocType, "");
            defs["FizDocScan"] = cbFizDocScan.Checked ? "X" : "-";
            //defs["AddrCountry"] = tbAddrCountry.Text;
            defs["AddrCountry"] = cbAddrCountry.Text;
            defs["AddrCityType"] = GetDefValue(cbAddrCityType, "");
            defs["AddrStreetType"] = GetDefValue(cbAddrStreetType, "");
            defs["AddrBuildingType"] = GetDefValue(cbAddrBuildingType, "");
            defs["AddrApartmentType"] = GetDefValue(cbAddrApartmentType, "");
            //defs["DeliveryCountry"] = tbDeliveryCountry.Text;
            defs["DeliveryCountry"] = cbDeliveryCountry.Text;
            defs["DeliveryType"] = GetDefValue(cbDeliveryType, "");
            defs["DeliveryComment"] = tbDeliveryComment.Text;
            defs["DeliveryCityType"] = GetDefValue(cbDeliveryCityType, "");
            defs["DeliveryStreetType"] = GetDefValue(cbDeliveryStreetType, "");
            defs["DeliveryBuildingType"] = GetDefValue(cbDeliveryBuildingType, "");
            defs["DeliveryApartmentType"] = GetDefValue(cbDeliveryApartmentType, "");

            defs["NewDolLogin"] = tbNewDolLogin.Text;
            defs["NewDolPassword"] = tbNewDolPassword.Text;
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setStr(module.ID, "DefaultValues", defs.SaveToString());
        }

        private void tbDynamicIcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                tbVerificationCode.Focus();
            }
        }

        /*
        ChromeDriver driver;
        ChromeOptions opt = new ChromeOptions();
        private void bRegInNewDol_Click(object sender, EventArgs e)
        {
            string urlRegistration = @"https://dol.beeline.ru/#!/auth/login";

            string d = ((IDEXSysData)toolbox).AppDir;
            
            //opt.AddArgument("--ignore-certificate-errors");
            opt.AddArgument("--disable-background-networking --disable-client-side-phishing-detection --disable-component-update --disable-default-apps --disable-extensions --disable-hang-monitor --disable-plugins --disable-popup-blocking --disable-prompt-on-repost --disable-sync --disable-web-resources --enable-logging --ignore-certificate-errors");
            //driver = new ChromeDriver(opt);
            driver = new ChromeDriver(d);
            driver.Navigate().GoToUrl(urlRegistration);

            // ввод логин-пароля
            try
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[name='phone']")));
                driver.FindElementByCssSelector(@"input[name='phone']").SendKeys(tbNewDolLogin.Text);
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[name='password']")));
                driver.FindElementByCssSelector(@"input[name='password']").SendKeys(tbNewDolPassword.Text);
                driver.FindElementByCssSelector(@"input[type='submit']").Click();
            }
            catch (Exception) { }
            Thread.Sleep(5000);
            //выбираем кнопку "Договоры"
            try
            {
                List<IWebElement> list = driver.FindElements(By.CssSelector(@"table[class='navigation'] div[class='navigation__item'] > a[ui-sref='contracts.transfer']")).ToList();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"table[class='navigation'] div[class='navigation__item'] > a[ui-sref='contracts.transfer']")));
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(@"table[class='navigation'] div[class='navigation__item'] > a[ui-sref='contracts.transfer']")));
                Actions action = new Actions(driver);
                action.MoveToElement(driver.FindElementByCssSelector(@"table[class='navigation'] div[class='navigation__item'] > a[ui-sref='contracts.transfer']")).Click().Build().Perform();
                //driver.FindElementByCssSelector(@"li[data-ng-class='{hide: user.loggedSeller.isSellerMin}'] a[ui-sref='contracts.transfer']").Click();
            }
            catch (Exception) { }
            // заполним icc
            try
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-change='transfer.checkIccid()']")));
                driver.FindElementByCssSelector(@"input[data-ng-change='transfer.checkIccid()']").SendKeys(mtbICC.Text);
            }
            catch (Exception) { }
            try{
            // заполним msisdn
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='transfer.checkCode']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='transfer.checkCode']").SendKeys(mtbMSISDN.Text.Substring(6));
            }
            catch (Exception) { }

            try
            {
                //кнопка "перейти к договору"
                driver.FindElementByCssSelector(@"input[type='submit']").Click();
            }
            catch (Exception) { }
            try
            {
                //заполним данные абонента
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"button[data-ng-click='newcontract.showAbonentInfo()']")));
                driver.FindElementByCssSelector(@"button[data-ng-click='newcontract.showAbonentInfo()']").Click();
            }
            catch (Exception) { }
            try
            {
                //Подождем пока загрузиться форма данных для абонента
                (new WebDriverWait(driver, TimeSpan.FromSeconds(10))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.citizenship']")));
                (new WebDriverWait(driver, TimeSpan.FromSeconds(10))).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(@"input[data-ng-model='customer.person.citizenship']")));
            }
            catch (Exception) { }
            
            //чекбокс резидент
            if (cbCitizenType1.Checked == false)
            {
                try
                {
                    (new WebDriverWait(driver, TimeSpan.FromSeconds(10))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"label[for='isResidentCheck']")));
                    driver.FindElementByCssSelector(@"label[for='isResidentCheck']").Click();

                    //гражданство
                    (new WebDriverWait(driver, TimeSpan.FromSeconds(10))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.citizenship']")));
                    driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.citizenship']").SendKeys(cbCitizenship.Text);
                 }
                 catch (Exception) { }
            }
            
            try
            {
                // фамилия
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.surname']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.surname']").SendKeys(tbLastName.Text);
            }
            catch (Exception) { }
            try
            {
                //Имя
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.name']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.name']").SendKeys(tbFirstName.Text);
            }
            catch (Exception) { }
            try
            {
                // Отчество
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.patronymic']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.patronymic']").SendKeys(tbSecondName.Text);
            }
            catch (Exception) { }
            try
            {
                //дата рождения
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='dtBd']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='dtBd']").SendKeys(deBirth.Text);
            }
            catch (Exception) { }
            try
            {
                //место рождения
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.birthPlace']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.birthPlace']").SendKeys(tbFizBirthPlace.Text);
            }
            catch (Exception) { }
            try
            {
                //пол 
                List<IWebElement> comboList = driver.FindElements(By.CssSelector(@"select[data-ng-model='customer.person.sex'] option")).ToList();
                int i = 0;
                foreach (IWebElement cblist in comboList)
                {

                    if (i == cbSex.SelectedIndex)
                    {
                        cblist.Click();
                        break;
                    }
                
                        i++;
                }
            }
            catch (Exception) { }
            try
            {
                //тип документа удостоверяющего личность
                int FizDocType = int.Parse(StringTagItem.getSelectedTag(cbFizDocType, ""));
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"select[id='personDocType'] option[value='number:" + FizDocType + "']")));
                driver.FindElementByCssSelector(@"select[id='personDocType'] option[value='number:" + FizDocType + "']").Click();
            }
            catch (Exception) { }
                
            
            try
            {
                // серия и номер документа
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.passportDocument.series']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.passportDocument.series']").SendKeys(mtbFizDocSeries.Text);
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.person.passportDocument.number']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.person.passportDocument.number']").SendKeys(mtbFizDocNumber.Text);
            }
            catch (Exception) { }
                //проверить паспорт на корректность
            //(new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"button[data-ng-click='checkPassport()']")));
            //driver.FindElementByCssSelector(@"button[data-ng-click='checkPassport()']").Click();
            try
            {
                // когда выдан паспорт
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='dtGb']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='dtGb']").SendKeys(deFizDocDate.Text);
            }
            catch (Exception) { }
                //теперь подождем, пока произойдет проверка паспорта и уберем диалог
            //(new WebDriverWait(driver, TimeSpan.FromSeconds(10))).Until(ExpectedConditions.ElementIsVisible(By.CssSelector(@"p[id='ngdialog1']")));
            //driver.FindElementByCssSelector(@"div[id='ngdialog2'] button[data-ng-click='closeThisDialog()']").Click();
            //код подразделения
            if (cbCitizenType1.Checked)
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='givenCodePart1']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='givenCodePart1']").SendKeys(tbFizDocOrgCode.Text.Substring(0,3));
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='givenCodePart2']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='givenCodePart2']").SendKeys(tbFizDocOrgCode.Text.Substring(4));
            }
            try
            {
                // кем выдан
                driver.FindElementByCssSelector(@"textarea[data-ng-model='customer.person.passportDocument.givenBy']").Clear();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"textarea[data-ng-model='customer.person.passportDocument.givenBy']")));
                driver.FindElementByCssSelector(@"textarea[data-ng-model='customer.person.passportDocument.givenBy']").SendKeys(tbFizDocOrg.Text);
            }
            catch (Exception) { }
            try
            {
                // адрес регистрации(Область)
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.area']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.area']").SendKeys(tbAddrState.Text);
            }
            catch (Exception) { }
            try
            {
                // адрес регистрации(район)
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.region']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.region']").SendKeys(tbAddrRegion.Text);
            }
            catch (Exception) { }
            
    //адрес регистрации(населенный пункт)
            int addrCityType = int.Parse(StringTagItem.getSelectedTag(cbAddrCityType, ""));
            if (addrCityType > 0)
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"select[id='personPlaceType'] option[value='number:" + addrCityType + "']")));
                driver.FindElementByCssSelector(@"select[id='personPlaceType'] option[value='number:" + addrCityType + "']").Click();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.place.name']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.place.name']").SendKeys(tbAddrCity.Text);
            }
            // адрес регистрации(Улица)
            int addrStreetType = int.Parse(StringTagItem.getSelectedTag(cbAddrStreetType, ""));
            if (addrStreetType > 0)
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"select[id='personStreetType'] option[value='number:" + addrStreetType + "']")));
                driver.FindElementByCssSelector(@"select[id='personStreetType'] option[value='number:" + addrStreetType + "']").Click();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.street.name']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.street.name']").SendKeys(tbAddrStreet.Text);
            }
            // адрес регистрации(дом)
            (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.house']")));
            driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.house']").SendKeys(tbAddrHouse.Text);
            //адрес регистрации(корпус)
            int addrBuildingType = int.Parse(StringTagItem.getSelectedTag(cbAddrBuildingType, ""));
            if (addrBuildingType > 0)
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"select[id='personBuildingType'] option[value='number:" + addrBuildingType + "']")));
                driver.FindElementByCssSelector(@"select[id='personBuildingType'] option[value='number:" + addrBuildingType + "']").Click();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.building.name']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.building.name']").SendKeys(tbAddrBuilding.Text);
            }
            //адрес регистрации (квартира)
            int addrApartmentType = int.Parse(StringTagItem.getSelectedTag(cbAddrApartmentType, ""));
            if (addrApartmentType > 0)
            {
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"select[id='personRoomType'] option[value='number:" + addrApartmentType + "']")));
                driver.FindElementByCssSelector(@"select[id='personRoomType'] option[value='number:" + addrApartmentType + "']").Click();
                (new WebDriverWait(driver, TimeSpan.FromSeconds(50))).Until(ExpectedConditions.ElementExists(By.CssSelector(@"input[data-ng-model='customer.address.room.name']")));
                driver.FindElementByCssSelector(@"input[data-ng-model='customer.address.room.name']").SendKeys(tbAddrApartment.Text);
            }
            //string sssss = "scf";
            //string ssss111s = "scf";
            //driver.Close();
            //driver.Quit();

            //string sss = "dd";
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            //tbVerificationCode.Focus();
            //SendKeys.Send("{ENTER}");
            try
            {
                btnCheckInputData.Enabled = false;
                tbVerificationCode.Enabled = false;
                checkSim();
            }
            catch (Exception) 
            {
                MessageBox.Show("Ошибка!!! Повторите операцию чуть позже или обратитесь в офис если это постоянная ошибка");
                this.Enabled = true;
                tbDynamicIcc.Enabled = true;
                tbVerificationCode.Enabled = true;
                btnCheckInputData.Enabled = true;
            }
        }

        private void DocumentForm_Resize(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
        }

        private void deBirth_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    bSearchName.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbCitizenType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    if (!cbCitizenType1.Checked)
                        cbCitizenship.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbCitizenship_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbMigrationCardSeries.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbCitizenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 1)
            {
                lbCitizenshipText.Enabled = false;
                cbCitizenship.Enabled = false;
                lbMigarionCardSeries.Enabled = false;
                lbMigarionCardNumber.Enabled = false;
                lbMigrationCardDateStart.Enabled = false;
                lbMigrationCardDateFinish.Enabled = false;

                tbMigrationCardSeries.Enabled = false;
                tbMigrationCardNumber.Enabled = false;
                deMigrationCardDateStart.Enabled = false;
                deMigrationCardDateFinish.Enabled = false;

                tbMigrationCardSeries.Text = "";
                tbMigrationCardNumber.Text = "";
                deMigrationCardDateStart.Text = "";
                deMigrationCardDateFinish.Text = "";

                StringTagItem.SelectByTag(cbCitizenship, rus, false);
            }
            else
            {
                lbCitizenshipText.Enabled = true;
                cbCitizenship.Enabled = true;
                lbMigarionCardSeries.Enabled = true;
                lbMigarionCardNumber.Enabled = true;
                lbMigrationCardDateStart.Enabled = true;
                lbMigrationCardDateFinish.Enabled = true;

                tbMigrationCardSeries.Enabled = true;
                tbMigrationCardNumber.Enabled = true;
                deMigrationCardDateStart.Enabled = true;
                deMigrationCardDateFinish.Enabled = true;
            }
        }

        private void cbCitizenType_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    if (cbCitizenType.SelectedIndex == 0) cbCitizenship.Focus();
                    else
                    {
                        tcRequisites.SelectedTab = tcRequisites.TabPages["tpRegAddress"];
                        cbAddrCountry.Focus();
                    }
                    
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbMigrationCardSeries_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbMigrationCardNumber.Focus();

                }
            }
            catch (Exception)
            {

            }
        }

        private void tbMigrationCardNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    deMigrationCardDateStart.Focus();

                }
            }
            catch (Exception)
            {

            }
        }

        private void deMigrationCardDateStart_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    deMigrationCardDateFinish.Focus();

                }
            }
            catch (Exception)
            {

            }
        }

        private void deMigrationCardDateFinish_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tcRequisites.SelectedTab = tcRequisites.TabPages["tpRegAddress"];
                    cbAddrCountry.Focus();

                }
            }
            catch (Exception)
            {

            }
        }

        private void cbAddrCountry_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryCountry.SelectedIndex = cbAddrCountry.SelectedIndex;
                    tbAddrState.Focus();
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void tbAddrState_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbAddrState.Text = normName(tbAddrState.Text);
                    tbDeliveryState.Text = tbAddrState.Text;
                    tbAddrRegion.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbAddrRegion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbAddrRegion.Text = normName(tbAddrRegion.Text);
                    tbDeliveryRegion.Text = tbAddrRegion.Text;
                    cbAddrCityType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbAddrCityType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
                    tbAddrCity.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbAddrZip_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbDeliveryZip.Text = tbAddrZip.Text;
                    cbAddrStreetType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbAddrStreetType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
                    tbAddrStreet.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbAddrStreet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbAddrStreet.Text = normName(tbAddrStreet.Text);
                    tbDeliveryStreet.Text = tbAddrStreet.Text;
                    tbAddrHouse.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbAddrHouse_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbDeliveryHouse.Text = tbAddrHouse.Text;
                    cbAddrBuildingType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbAddrBuildingType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
                    tbAddrBuilding.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbAddrBuilding_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbDeliveryBuilding.Text = tbAddrBuilding.Text;
                    cbAddrApartmentType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbAddrApartmentType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryApartmentType.Text = cbAddrApartmentType.Text;
                    tbAddrApartment.Focus();
                }
            
            }
            catch (Exception)
            {

            }
        }

        private void tbDocCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;
                deDocDate.Focus();

                //SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void deJournalDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;
                if (isOnline) 
                {
                    tbDocCity.Focus();
                } 
                else 
                {
                     cbDocUnit.Focus();
                }
               
                //SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void cbDocUnit_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;
                tbDocCity.Focus();
                //SelectNextControl((Control)sender, true, true, true, false);
            }
        }

        private void btbCopyRegData_Click(object sender, EventArgs e)
        {
            cbResidenceAddrCountry.SelectedIndex = cbAddrCountry.SelectedIndex;
            tbResidenceAddrState.Text = tbAddrState.Text;
            tbResidenceAddrRegion.Text = tbAddrRegion.Text;
            cbResidenceAddrCityType.SelectedIndex = cbAddrCityType.SelectedIndex;
            tbResidenceAddrCity.Text = tbAddrCity.Text;
            tbResidenceAddrZip.Text = tbAddrZip.Text;
            cbResidenceAddrStreetType.SelectedIndex = cbAddrStreetType.SelectedIndex;
            tbResidenceAddrStreet.Text = tbAddrStreet.Text;
            tbResidenceAddrHouse.Text = tbAddrHouse.Text;
            cbResidenceAddrBuildingType.SelectedIndex = cbAddrBuildingType.SelectedIndex;
            tbResidenceAddrBuilding.Text = tbAddrBuilding.Text;
            cbResidenceAddrApartmentType.SelectedIndex = cbAddrApartmentType.SelectedIndex;
            tbResidenceAddrApartment.Text = tbAddrApartment.Text;
        }

        private void bAddScanDocument_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Barcode br = new Barcode();
                br.IncludeLabel = true;
                br.Encode(TYPE.CODE128, "123456789123456789", Color.Black, Color.White, 300, 50);
                
                br.SaveImage("file.jpg", SaveTypes.JPG);
               
            }
            catch (Exception)
            {
                string sss = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                /*
                string query = "SELECT * FROM journal where jdocdate > '20180200' AND status = '4'";
                DataTable dtPlans = ((IDEXData)toolbox).getQuery(query);
                int rr = 0;
                int reset = 0;
                foreach (DataRow dr in dtPlans.Rows)
                {
                    SimpleXML xml = SimpleXML.LoadXml(dr["data"].ToString());

                    DataTable dtPlansUmData = ((IDEXData)toolbox).getQuery("SELECT count(*) as count from um_data where icc = '"+xml["ICC"].Text+"'");
                    int dd = Convert.ToInt32(dtPlansUmData.Rows[0]["count"].ToString());
                    if (dd > 1)
                    {
                        rr++;

                        // теперь понять, надо ли чинить
                        int ifDynamic = 0;
                        int ifStatic = 0;
                        DataTable dtPlansUmDataCheck = ((IDEXData)toolbox).getQuery("SELECT * from um_data where icc = '" + xml["ICC"].Text + "'");
                        
                        foreach (DataRow drCheck in dtPlansUmDataCheck.Rows)
                        {
                            if (Convert.ToInt32(drCheck["dynamic"].ToString()) == 0) ifStatic++;
                            if (Convert.ToInt32(drCheck["dynamic"].ToString()) == 1) ifDynamic++;
                   
                        }

                        if (ifDynamic == 1 && ifStatic == 1)
                        {
                            
                            reset++;
                            string msisdn;
                            //все, можно чинить
                            foreach (DataRow drCheck in dtPlansUmDataCheck.Rows) 
                            {
                                if (Convert.ToInt32(drCheck["dynamic"].ToString()) == 1)
                                {
                                    //if (xml["OLDNEWDOLMSISDN"].Text == "")
                                    //{
                                    if (xml["icc"].Text == "897019917121743027")
                                    {
                                        string fff = "sdc";
                                    }
                                        xml["OLDNEWDOLMSISDN"].Text = drCheck["msisdn"].ToString();
                                        xml["Dynamic"].Text = "1";
                                        string data = SimpleXML.SaveXml(xml);
                                        ((IDEXData)toolbox).getQuery("update `journal` set data = '"+data+"' where id = '" + dr["id"].ToString() + "'");
                                    //}
                                    //else
                                    //{
                                    //    string rrrrr = "sdc";
                                    //}
                                }
                                else if (Convert.ToInt32(drCheck["dynamic"].ToString()) == 0)
                                {
                                    msisdn = drCheck["msisdn"].ToString();
                                    ((IDEXData)toolbox).getQuery("DELETE from um_data where id = '" + drCheck["id"] + "'");

                                    
                                } 
                            }
                        }
                        else
                        {
                            string rrrr = "sd";
                        }
                    }
                    //if (dtPlansUmData.Rows[0]["count"].ToString())
                }
                

                MessageBox.Show("Все");
                // спишем все сим
                //string query = "SELECT * FROM um_data";
                //DataTable dtPlans = ((IDEXData)toolbox).getQuery(query);
                */

                string pp = "";
            }
            catch (Exception) 
            {
                string fff = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM um_data";
            DataTable dt = ((IDEXData)toolbox).getQuery(query);
            foreach (DataRow dr in dt.Rows)
            {
                string querySearh = "select count(*) as count from um_data where icc='"+dr["icc"].ToString()+"'";
                DataTable dt1 = ((IDEXData)toolbox).getQuery(querySearh);
                if (Convert.ToInt32(dt1.Rows[0]["count"].ToString()) > 1)
                {
                    string fff = "f";
                }
            }
            string dd = "hgj";

            // найдем все проданные сим по справочнику сим и найдем договоры в журнале
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["status"].ToString().Equals("2"))
                {
                    string query1 = "SELECT * FROM journal where data like '%"+dr["icc"].ToString()+"%'";
                    DataTable dt1 = ((IDEXData)toolbox).getQuery(query1);
                    if (dt1 != null && dt.Rows.Count < 1)
                    {
                        string ddd = "sdc";
                    }
                }
            }

            string pp = "sdc";

        }

        private void tbResidenceAddrState_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrState.Text = normName(tbResidenceAddrState.Text);
                    tbResidenceAddrRegion.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrRegion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrRegion.Text = normName(tbResidenceAddrRegion.Text);
                    cbResidenceAddrCityType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbResidenceAddrCityType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrCity.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrCity_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrCity.Text = normName(tbResidenceAddrCity.Text);
                    tbResidenceAddrZip.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrZip_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbResidenceAddrStreetType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbResidenceAddrStreetType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrStreet.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrStreet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrStreet.Text = normName(tbResidenceAddrStreet.Text);
                    tbResidenceAddrHouse.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrHouse_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrHouse.Text = normName(tbResidenceAddrHouse.Text);
                    cbResidenceAddrBuildingType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbResidenceAddrBuildingType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrBuilding.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrBuilding_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbResidenceAddrApartmentType.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cbResidenceAddrApartmentType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    tbResidenceAddrApartment.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbResidenceAddrApartment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    cbDeliveryType.Focus();
                }
            }
            catch (Exception)
            {

            }
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

        private void bAddScanDocument_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OPF = new OpenFileDialog();
                //OPF.Filter = "Files(*.JPG;*.JPEG;*.PNG;*.PDF)|*.JPG;*.JPEG;*.PNG;*.PDF";
                OPF.Filter = "Files(*.JPG;*.JPEG;*.PNG)|*.JPG;*.JPEG;*.PNG";
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
                lbIfScanChange.Text = "Ошибка во время вложения скана документа";
                lbIfScanChange.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void bSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor = "beeline";
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
                        //saveFileDialog1.Filter = "jpeg Image|*.jpg|bitmap Image|*.bmp|gif Image|*.gif|pdf file|*.pdf";
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
                string vendor = "beeline";
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

        private void button4_Click(object sender, EventArgs e)
        {
            string currentBase = ((IDEXUserData)toolbox).dataBase;
            JObject data = new JObject();
            string vendor = "beeline";
            string vendorBase = "";
            IDEXServices idis = (IDEXServices)toolbox;
            JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
            //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
            if (int.Parse(obj["data"]["status"].ToString()) == 1)
            {
                currentBase = ((IDEXUserData)toolbox).dataBase;
            }
            data = new JObject();
            data["base"] = currentBase;
            obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/searchDexToServName?data=" + JsonConvert.SerializeObject(data), 0));
            //так как не dexol, а знать базу, к которой произошло подлкючение, нужно, то узнаем базу
            if (int.Parse(obj["data"]["status"].ToString()) == 1)
            {
                vendorBase = obj["data"]["base"].ToString();
            }

            JObject packet;
            string qstr = "select * from `um_data` where status != '2' AND party_id != '1'";
            DataTable dtPid = ((IDEXData)toolbox).getQuery(qstr);
            int isset = 0;
            if (dtPid != null) 
            {
                //int total = dtPid.Rows.Count();
               
                foreach (DataRow dr in dtPid.Rows) 
                {
                    packet = new JObject();
                    packet["com"] = "dexdealer.adapters.beeline";
                    packet["subcom"] = "apiIfIssetSim";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["vendor"] = "beeline";
                    packet["data"]["base"] = "BEE_STS";
                    packet["data"]["icc"] = dr["icc"].ToString();
                    obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + "" + "&clientType=dexol&dexolUid=" + "", 1));
                    if (Convert.ToInt32(obj["data"]["status"].ToString()) == 0)
                    {
                        ((IDEXData)toolbox).getQuery("DELETE from um_data where id = '" + dr["id"].ToString() + "'");
                        isset++;
                    }
                }
            }

            string dd = "w";


        }


        private void tbDocNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                e.SuppressKeyPress = true;

                btnGetDocNum.Focus();

                //SelectNextControl((Control)sender, true, true, true, false);

            }
        }


        private void btnGetDocNum_KeyDown(object sender, EventArgs e)
        {
            if (!mtbICC.Text.Equals(""))
            {
                string currentBase = "";
                if (!isOnline)
                {
                    currentBase = ((IDEXUserData)toolbox).dataBase;   
                }
                else
                {
                    currentBase = ((IDEXUserData)toolbox).currentBase;
                }
                
                IDEXServices idis = (IDEXServices)toolbox;
                JObject authData = new JObject();
                authData["login"] = ((IDEXUserData)toolbox).adaptersLogin == null ? "admin" : ((IDEXUserData)toolbox).adaptersLogin;
                authData["password"] = ((IDEXUserData)toolbox).adaptersPass == null ? "12473513" : ((IDEXUserData)toolbox).adaptersPass;
                //JObject authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                JObject authObj = new JObject();
                try
                {
                    authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                    adaptersUid = authObj["uid"].ToString();
                }
                catch (Exception) { }

                JObject objInfoBase = new JObject();
                objInfoBase = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/adapters/getDexDexolBase?uid=" + adaptersUid + "&clientType=dexol", 1));

                string pfBase = "";

                if (isOnline)
                {
                    try
                    {

                        foreach (JObject jo in objInfoBase["data"])
                        {
                            if (jo["dex_dexol_base_name"].ToString().Equals(currentBase))
                            {
                                pfBase = jo["tag"].ToString();
                            }
                        }

                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        foreach (JObject jo in objInfoBase["data"])
                        {
                            if (jo["list"].ToString().Equals(currentBase))
                            {
                                pfBase = jo["tag"].ToString();
                                currentBase = jo["dex_dexol_base_name"].ToString();
                            }
                        }
                    }
                    catch (Exception) { }
                }



                JObject packet = new JObject();
                //packet["com"] = "dexdealer.adapters.beeline";
                //packet["subcom"] = "apiGetSimList";
                //packet["client"] = "dexol";
                //packet["data"] = new JObject();
                //packet["data"]["vendor"] = "beeline";
                //packet["data"]["base"] = currentBase;
                //packet["data"]["base"] = "BEE_STS";

                //JObject simList = new JObject();
                //JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol", 1));




                try
                {
                    packet["com"] = "dexdealer.adapters.beeline";
                    packet["subcom"] = "apiGetDocNum";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["vendor"] = "beeline";
                    packet["data"]["base"] = currentBase;
                    packet["data"]["icc"] = mtbICC.Text;
                    if (!isOnline) packet["data"]["ignoreUid"] = 1;
                    else packet["data"]["ignoreUid"] = 0;


                    //string ssss = "37.29.115.178:3000/adapters/getSimInfo?data=" + JsonConvert.SerializeObject(data) + "&uid=" + adaptersUid + "&clientType=dexol&dexolUid=" + dexUid;

                    JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol", 1));
                    string docnumber = o["data"]["docnumber"].ToString();
                    if (docnumber != tbDocNum.Text) tbDocNum.Text = docnumber;
                }
                catch (Exception)
                {
                }


                
            }
            
            cbDocOrgType.Focus();
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (adaptersUid != null && !"".Equals(adaptersUid))
            {
                try
                {
                    IDEXServices idis = (IDEXServices)toolbox;
                    JObject packet = new JObject();
                    packet["com"] = "dexdealer.adapters.login";
                    packet["subcom"] = "logout";
                    JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/end?uid=" + adaptersUid + "&clientType=dexol", 1));
                }
                catch (Exception) { }
            }
        }




   

     

    }

    class CheckListBoxCodeTextHint
    {
        public string text, tag, hint;

        public CheckListBoxCodeTextHint(string text, string tag, string hint)
        {
            this.tag = tag;
            this.text = text;
            this.hint = hint;
        }

        public override string ToString()
        {
            return text;
        }
    }
}
