using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DEXExtendLib;
using System.Data;
using System.Globalization;

using MySql.Data.MySqlClient;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public class Document : IDEXPluginDocument /*, IDEXPluginSetup*///, IDEXDocumentPlans
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Document.Beeline.DOL2.Contract";
            }
        }
        public string Title
        {
            get
            {
                return "Билайн DOL2: Договор о предоставлении услуг связи";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = { "Билайн DOL2" };
                return ret;
            }
        }

        public int Version
        {
            get
            {
                return 1;
            }
        }

        public string Description
        {
            get
            {
                return "Форма ввода данных Договора о предоставлении услуг связи Билайн (DOL2)";
            }
        }

        public Bitmap getBitmap()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.IO.Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".icon.bmp");
                Bitmap b = new Bitmap(s);
                return b;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public void setJournalMode(DEXJournalType journalType)
        {
            this.journalType = journalType;
        }

        DEXJournalType journalType = DEXJournalType.JOURNAL;

        public void Startup(Object toolbox)
        {
            IDEXRights r = (IDEXRights)toolbox;
            r.AddRightsItem(ID + ".new", "Билайн DOL2: Создание документа");
            r.AddRightsItem(ID + ".edit", "Билайн DOL2: Изменение документа");
            r.AddRightsItem(ID + ".delete", "Билайн DOL2: Удаление документа");
            //            r.AddRightsItem(ID + ".approve", "Билайн " + DocumentForm.BeeRegionRus + ": Быстрое подтверждение документа");

            IDEXConfig c = (IDEXConfig)toolbox;
            if (!c.isRegisterKeyExists("beeline_icc_mask"))
            {
                try
                {
                    c.createRegisterKey("beeline_icc_mask", "Билайн DOL2: Маска ICC", "000000000000000000");
                }
                catch (Exception)
                {
                }
            }
            if (!c.isRegisterKeyExists("beeline_msisdn_mask"))
            {
                try
                {
                    c.createRegisterKey("beeline_msisdn_mask", "Билайн DOL2: Маска MSISDN", "0000000000");
                }
                catch (Exception)
                {
                }
            }
        }

        public bool NewDocument(Object toolbox, IDEXDocumentData document)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".new") || r.IsSuperUser())
            {
                DOL2Data dol2 = new DOL2Data(toolbox);
                if (dol2.dataLoaded)
                {

                    DocumentForm form = new DocumentForm();
                    form.toolbox = toolbox;
                    form.module = this;
                    form.dol2 = dol2;

                    form.InitDocument(null, document, false, false);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        ((IDEXDocumentJournal)toolbox).AddRecord(
                            "Новый документ добавлен в журнал",
                            new string[] 
                            {
                                string.Format("Дайджест: {0}", document.documentDigest),
                                string.Format("ID отделения: {0}", document.documentUnitID),
                                string.Format("Статус: {0}", document.documentStatus),
                                string.Format("Время добавления: {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()),
                                string.Format("Пользователь: {0}", ((IDEXUserData)toolbox).Title),
                                string.Format("ID в системе: {0}", ((IDEXUserData)toolbox).Login)
                            }
                        );
                        ret = true;
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно запустить форму, поскольку не удалось загрузить справочники.");
                }
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права создания документов этого типа");
            }
            return ret;
        }


        public bool CloneDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document)
        {
            bool ret = false;

 // Кажется, не используется
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".new") || r.IsSuperUser())
            {
                DocumentForm form = new DocumentForm();
                form.toolbox = toolbox;
                form.module = this;
                form.InitDocument(source, document, true, false);
                form.tbDynamicIcc.Text = "";
                form.tbVerificationCode.Text = "";
                form.btnCheckInputData.Visible = true;
                form.tbBoxType.Text = "";
                form.tbDynamicOldNumber.Text = "";
                form.tbDocNum.Text = "";
                form.mtbMSISDN.Text = "";
                form.mtbICC.Text = "";
                form.lbScanPath.Text = "";
                form.signatire.Text = "";
                form.lbScanMime.Text = "";
                form.cbScan.SelectedIndex = 0;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    string cd_date = "?";
                    string cd_number = "?";

                    SimpleXML xml = SimpleXML.LoadXml(source.documentText);
                    if (xml != null)
                    {
                        cd_date = xml.GetNodeByPath("DocDate", true).Text;
                        cd_number = xml.GetNodeByPath("DocNum", true).Text;
                    }

                    ((IDEXDocumentJournal)toolbox).AddRecord(
                        "Документ на основе другого документа добавлен в журнал",
                        new string[] 
                        {
                            string.Format("Дайджест: {0}", document.documentDigest),
                            string.Format("ID отделения: {0}", document.documentUnitID),
                            string.Format("Статус: {0}", document.documentStatus),
                            string.Format("Время добавления: {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()),
                            string.Format("Пользователь: {0}", ((IDEXUserData)toolbox).Title),
                            string.Format("Документ-основа:"),
                            string.Format("Дайджест: {0}", source.documentDigest),
                            string.Format("ID отделения: {0}", source.documentUnitID),
                            string.Format("Статус: {0}", source.documentStatus),
                            string.Format("Дата добавления: {0}", cd_date),
                            string.Format("№ договора: {0}", cd_number)
                        }
                    );
                    ret = true;
                }
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права создания документов этого типа");
            }
 
            return ret;
        }
        // убать комментирий ниже
        /*
        public void setPlans(Object toolbox)
        {
            DOL2Data dol2 = new DOL2Data(toolbox);
            IDEXPlans r = (IDEXPlans)toolbox;
            IDEXData t = (IDEXData)toolbox;
            
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            foreach ( KeyValuePair<string, DOL2BillPlan> kvp in dol2.libBillplans )
            {
                Dictionary<string, string> s = new Dictionary<string, string>();
                s.Add("Accept", kvp.Value.Accept.ToString());
                s.Add("CellnetsId", kvp.Value.CellnetsId.ToString());
                s.Add("ChannellensId", kvp.Value.ChannellensId.ToString());
                s.Add("Code", kvp.Value.Code.ToString());
                s.Add("Enable", kvp.Value.Enable.ToString());
                s.Add("id", kvp.Value.id.ToString());
                s.Add("Name", kvp.Value.Name.ToString());
                s.Add("PaySystemsId", kvp.Value.PaySystemsId.ToString());
                s.Add("SOC", kvp.Value.SOC.ToString());
                dic.Add(kvp.Key, s);
                s = null;                
            }
            r.setPlans(dic);            
        }
        
        public void getValuesPlan(Object toolbox, string tag) 
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //KeyValuePair<string, Array> ret;
            //var t = new KeyValuePair<string, object>();
            DOL2Data dol2 = new DOL2Data(toolbox);
            //int i = 0;
            //foreach ( KeyValuePair<string, DOL2BillPlan> kvp in dol2.libBillplans )
            //{
            //    if (kvp.Key == tag) 
            //    {
                    //ret.Key = kvp.Key;
                    //ret.Value = (Array)kvp.Value;
                    //foreach(DOL2BillPlan s in kvp.Value) 
                    //{
            //        dic.Add(kvp.Key, kvp.Value);  
                    
                    //}
            //    }
            //}
            
        }
        */

        public bool EditDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document, IDEXDocumentData changes, bool ReadOnly)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".edit") || r.IsSuperUser())
            {
                DOL2Data dol2 = new DOL2Data(toolbox);
                if (dol2.dataLoaded)
                {
                    DocumentForm form = new DocumentForm();
                    form.toolbox = toolbox;
                    form.module = this;
                    form.dol2 = dol2;
                    form.InitDocument(source, document, false, ReadOnly);
                    List<string> arr = new List<string>();
                    string[] fields;
                    arr.Add(String.Format("Статус: {0}", document.documentStatus));
                    arr.Add(String.Format("Время изменения: {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                    bool isOnline = ((IDEXUserData)toolbox).isOnline;
                    if (isOnline)
                    {
                        arr.Add(String.Format("Договор был отредактирован в DEXOL. ID субдилера: {0}", ((IDEXUserData)toolbox).UID));
                    }
                    else
                    {
                        arr.Add(String.Format("Пользователь: {0}", ((IDEXUserData)toolbox).Title));
                        arr.Add(String.Format("ID в системе: {0}", ((IDEXUserData)toolbox).Login));
                    }
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        SimpleXML xmlSource = SimpleXML.LoadXml(source.documentText);
                        SimpleXML xmlNew = SimpleXML.LoadXml(document.documentText);
                        try
                        {
                            bool ifAdd = false;
                            foreach (var node in xmlSource.Child)
                            {
                                string name = ((SimpleXML)node).Name;
                                string text = ((SimpleXML)node).Text;
                                if (xmlNew.GetNodeByPath(((SimpleXML)node).Name, false).Text != ((SimpleXML)node).Text)
                                {
                                    if (!ifAdd)
                                    {
                                        arr.Add("Затронутые поля поля:");
                                        ifAdd = true;
                                    }
                                    arr.Add(String.Format("{0}=> Старое значение - {1}. Новое значение - {2}", ((SimpleXML)node).Name, ((SimpleXML)node).Text, xmlNew.GetNodeByPath(((SimpleXML)node).Name, false).Text));
                                }
                            }

                            if (!ifAdd) arr.Add("Измененых полей не оказалось");
                        }
                        catch (Exception)
                        {
                            arr.Add("Ошибка в процессе получения измененных полей");
                        }

                        fields = new string[arr.Count];
                        for (int i = 0; i < arr.Count; i++)
                        {
                            fields[i] = arr[i];
                        }
                        ((IDEXDocumentJournal)toolbox).AddRecord(
                            "Документ был отредактирован", fields

                        );

                        ret = true;
                    }
                }
                else
                {
                    MessageBox.Show("Невозможно запустить форму, поскольку не удалось загрузить справочники.");
                }
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права на изменение документов этого типа");
            }
            return ret;
        }

        public bool DeleteDocument(Object toolbox, IDEXDocumentData source, bool batch)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".new") || r.IsSuperUser())
            {
                ret = true;
            }
            else
            {
                if (!batch)
                {
                    MessageBox.Show("У пользователя отсутствуют права на удаление документов этого типа");
                }
            }
            return ret;
        }

        /**
         * Возврат = ArrayList(0) - Ошибок нет. Иначе - список ошибок
         **/
        public ArrayList ValidateDocument(Object toolbox, IDEXDocumentData document)
        {
            ArrayList ret = new ArrayList();
            SimpleXML xml = SimpleXML.LoadXml(document.documentText);

            bool statusPassport = false;

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
            Regex rxokonh = new Regex(@"^\d{2}\.\d{2}\.\d{2}$");
            Regex rxemail = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + 
                @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + 
                @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + 
                @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");
            Regex rxpasser = new Regex(@"^\d{2} \d{2}$");
            Regex rxpasnum = new Regex(@"^\d{6}$");
            Regex rxcontactPhone = new Regex(@"^\d{7}$");
            Regex rxcontactPrefix = new Regex(@"^\d{3}$");
            Regex rxpasogrcode = new Regex(@"^\d{3}-\d{3}$");


            if (xml.Attributes["ID"] != ID) ret.Add("Не соответствует ID документа");

            try 
            {
                if (!rxdate.IsMatch(xml.GetNodeByPath("DocDate", true).Text.Trim())) throw new Exception("1");
                string dck = xml.GetNodeByPath("DocDate", true).Text.Trim();
                DateTime src = new DateTime(int.Parse(dck.Substring(6, 4)), int.Parse(dck.Substring(3, 2)), int.Parse(dck.Substring(0, 2)), 12,0, 0);
                IDEXValidators val = (IDEXValidators)toolbox;
                if (!val.CheckPeriodDate(src)) throw new Exception("2");
            } 
            catch(Exception ex) 
            { 
                ret.Add(string.Format("Некорректная дата подписания договора ({0})", ex.Message));
            }

            if (xml["DocNum"].Text.Trim().Equals("")) ret.Add("Не указан № договора");

            int dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DocOrgType", true).Text);
            }
            catch (Exception) { }
            if (dc != 0 && dc != 1) ret.Add("Не указана форма организации (лицо)");

            int docorgtype = dc;

            if (xml.GetNodeByPath("DocClientType", false) == null || xml["DocClientType"].Text.Trim().Equals("")) ret.Add("Не указан тип клиента");

            /*
            if (xml["isOnline"].Text.Equals("1"))
            {
                if (xml["FizDocType"].Text != "1") ret.Add("В данный момент возможна регистрация только с типом документа 'Паспорт'");
                if (xml["CitizenType"].Text != "1") ret.Add("В данный момент возможна регистрация только на резидентов");
            }
            */

            if (docorgtype == 0)
            {

                try
                {
                    dc = int.Parse(xml.GetNodeByPath("Sex", true).Text);
                }
                catch (Exception) { }
                if (dc != 0 && dc != 1) ret.Add("Не указан пол абонента");

                if (xml["FirstName"].Text.Trim().Length < 1)
                    ret.Add("Не указано имя абонента");
                else if (!DOL2Data.CheckString(xml["FirstName"].Text.Trim(), true, false))
                    ret.Add("Имя абонента не должно содержать цифры");

                if (xml["LastName"].Text.Trim().Length < 1)
                    ret.Add("Не указана фамилия абонента");
                else if (!DOL2Data.CheckString(xml["LastName"].Text.Trim(), true, false))
                    ret.Add("Фамилия абонента не должна содержать цифры");


                if (xml["SecondName"].Text.Trim().Length < 1)
                    ret.Add("Не указано отчество абонента");
                if (!DOL2Data.CheckString(xml["SecondName"].Text.Trim(), true, false))
                    ret.Add("Отчество абонента не должно содержать цифры");

                dc = -1;
                try
                {
                    dc = int.Parse(xml.GetNodeByPath("FizDocType", true).Text);
                }
                catch (Exception) { }
                if (dc < 0) ret.Add("Не указан тип удостоверяющего документа");
                else
                {
                    if (dc == 0)
                    {
                        if (!rxpasser.IsMatch(xml["FizDocSeries"].Text)) ret.Add("Серия удостоверяющего документа заполнена некорректно");
                        if (!rxpasnum.IsMatch(xml["FizDocNumber"].Text)) ret.Add("Номер удостоверяющего документа заполнен некорректно");
                    }
                    else
                    {
                        if (xml["FizDocNumber"].Text.Trim().Length < 4) ret.Add("Номер удостоверяющего документа должен содержать не менее 4-х символов");
                    }
                }

                /*
                dc = -1;
                try
                {
                    dc = int.Parse(xml.GetNodeByPath("FizDocCitizen", true).Text);
                }
                catch (Exception) { }
                if (dc < 0) ret.Add("Не указано гражданство");
                */

                if (Convert.ToInt32(xml["CitizenType"].Text) == 1)
                {
                    if (!rxpasogrcode.IsMatch(xml["FizDocOrgCode"].Text)) ret.Add("Код подразделения заполнен некорректно");

                    
                }
                else
                {
                    
                    //if (xml["MigrationCardSeries"].Text == "") ret.Add("Не указана серия миграционной карты");
                    //if (xml["MigrationCardNumber"].Text == "") ret.Add("Не указан номер миграционной карты");
                    //DateTime dtMigrationCardDateStart = DOL2Data.strtodate(xml["MigrationCardDateStart"].Text);
                    //DateTime dtMigrationCardDateFinish = DOL2Data.strtodate(xml["MigrationCardDateFinish"].Text);
                    //if (dtMigrationCardDateStart == DateTime.MinValue) ret.Add("Не указана дата начала действия миграционной карты");
                    //if (dtMigrationCardDateFinish == DateTime.MinValue) ret.Add("Не указана дата окончания действия миграционной карты");
                }


                //if (xml["DocCity"].Text.Trim().Length == 0) ret.Add("Вы не указали город подключения");
                if (xml["FizDocOrg"].Text.Trim().Length < 4) ret.Add("Учреждение, выдавшее документ: Должен содержать больше четырех символов");
                if (xml["FizInn"].Text.Trim().Length > 0 && !DOL2Data.CheckOrgAttribute(xml["FizInn"].Text.Trim(), true, 12)) ret.Add("ИНН должен содержать только 12 символов");

                if (xml["FizBirthPlace"].Text.Trim().Length == 0) ret.Add("Вы не указали место рождения");

                DateTime dtFizDocDate = DOL2Data.strtodate(xml["FizDocDate"].Text);
                DateTime dtBirth = DOL2Data.strtodate(xml["Birth"].Text);
                if (dtFizDocDate == DateTime.MinValue) ret.Add("Не указана дата выдачи удостоверяющего документа");
                if (dtBirth == DateTime.MinValue) ret.Add("Не указана дата рождения абонента");

                if (dtFizDocDate != DateTime.MinValue && dtBirth != DateTime.MinValue)
                {

                    DateTime dtAge = new DateTime(1);
                    try
                    {
                        dtAge = new DateTime((dtFizDocDate - dtBirth).Ticks);
                        if (dtAge.Year < 14) ret.Add("Дата выдачи удостоверяющего документа должна быть больше даты рождения не меньше чем на 14 лет");
                    }
                    catch (Exception) 
                    {
                        ret.Add("Дата выдачи удостоверяющего документа должна быть не раньше даты рождения");
                    }
                    

                    dtAge = new DateTime(1);
                    
                    try
                    {
                        string dd1 = xml["Birth"].Text.Trim(), dd2 = document.documentDate;
                        DateTime d1 = new DateTime(int.Parse(dd1.Substring(6, 4)), int.Parse(dd1.Substring(3, 2)), int.Parse(dd1.Substring(0, 2)));
                        DateTime d2 = new DateTime(int.Parse(dd2.Substring(0, 4)), int.Parse(dd2.Substring(4, 2)), int.Parse(dd2.Substring(6, 2)));

                        bool older = true;
                        if (d2.Year - d1.Year < 18) older = false;
                        else if (d2.Year - d1.Year == 18)
                        {
                            if (d2.Month < d1.Month) older = false;
                            else if (d2.Month == d1.Month)
                            {
                                if (d2.Day < d1.Day) older = false;
                            }
                        }

                        if (!older) ret.Add("Абонент не должен быть младше 18 лет");
                    }
                    catch (Exception) 
                    {
                        ret.Add("Дата рождения не может быть в будущем");                    
                    }

                    
                }

                

                if (document.documentStatus > 0)
                {

                    StringList sl = new StringList();
                    sl["FirstName"] = xml["FirstName"].Text.Trim();
                    sl["SecondName"] = xml["SecondName"].Text.Trim();
                    sl["LastName"] = xml["LastName"].Text.Trim();
                    sl["Birth"] = xml["Birth"].Text.Trim();
                    sl["FizDocNumber"] = xml["FizDocNumber"].Text.Trim();
                    sl["FizDocSeries"] = xml["FizDocSeries"].Text.Trim();

                    string[] pch = ((IDEXPeopleSearcher)toolbox).checkPassport(sl);

                    if (pch != null) foreach (string sa in pch) ret.Add(sa);
                }
            }

            if (docorgtype == 1)
            {
                if (xml.GetNodeByPath("CompanyTitle", true).Text.Trim().Length < 1)
                    ret.Add("Не указано наименование организации");

                if (!DOL2Data.CheckOrgAttribute(xml["CompanyInn"].Text, true, 10))
                    ret.Add("ИНН должен содержать только 10 цифр");
                if (!DOL2Data.CheckOrgAttribute(xml["CompanyOkonh"].Text, true, 5) && !rxokonh.IsMatch(xml["CompanyOkonh"].Text.Trim()))
                    ret.Add("Код ОКОНХ должен содержать только 5 цифр или иметь формат '00.00.00'");
                if (!DOL2Data.CheckOrgAttribute(xml["CompanyOkpo"].Text, true, 8))
                    ret.Add("Код ОКПО должен содержать только 8 цифр");
                if (!DOL2Data.CheckOrgAttribute(xml["CompanyKpp"].Text, true, 9))
                    ret.Add("Код КПП должен содержать только 9 цифр");

                if (xml.GetNodeByPath("OrgBank", true).Text.Trim().Length < 1)
                    ret.Add("Не указано наименование банка организации");
                if (!DOL2Data.CheckOrgAttribute(xml["OrgRs"].Text, true, 20))
                    ret.Add("Расчётный счёт должен содержать только 20 цифр");
                if (!xml["OrgKs"].Text.Trim().Equals("") && !DOL2Data.CheckOrgAttribute(xml["OrgKs"].Text, true, 20))
                    ret.Add("Корр.счёт должен содержать только 20 цифр");
                if (!DOL2Data.CheckOrgAttribute(xml["OrgBik"].Text, true, 9))
                    ret.Add("БИК должен содержать только 9 цифр");
            }

            // проверка на террориста
            try
            {

            }
            catch (Exception) 
            {
            }

            // проверка на террориста
            try
            {
                IDEXServices idis = (IDEXServices)toolbox;
                string firstName = xml.GetNodeByPath("FirstName", true).Text.Trim();
                string secondName = xml.GetNodeByPath("SecondName", true).Text.Trim();
                string lastName = xml.GetNodeByPath("LastName", true).Text.Trim();
                string birth = xml.GetNodeByPath("Birth", true).Text.Trim();


                JObject packet = new JObject();
                packet["com"] = "dexdealer.adapters.checkForTerrorists";
                packet["subcom"] = "checkForTerrorists";
                packet["client"] = "dexol";
                packet["data"] = new JObject();
                packet["data"]["lastname"] = lastName;
                packet["data"]["firstname"] = firstName;
                packet["data"]["secondname"] = secondName;
                packet["data"]["birth"] = birth;
                string packetFromServer = idis.checkForTerrorists(JsonConvert.SerializeObject(packet));
                JObject jo = JObject.Parse(packetFromServer);

                bool ifIsset = false;
                foreach (JObject j in jo["data"]["coincidenceWithBirth"])
                {
                    ifIsset = true;
                }
                if (ifIsset) ret.Add("Абонент находится в списке террористов!");


            }
            catch (Exception)
            {
            }

            //проверка серии и номера паспорта на корректность по базе fms

            IDEXData d = (IDEXData)toolbox;

            try
            {
                



                Regex rgx = new Regex("\\s+");
                string FizDocSeries = rgx.Replace(xml["FizDocSeries"].Text, "");
                string FizDocNumber = xml["FizDocNumber"].Text;


                IDEXServices idis = (IDEXServices)toolbox;
                statusPassport = idis.checkPassport(FizDocSeries, FizDocNumber);

                /*
                if ( d.AccessRemoteServer )
                {
                    //string conn = "server=192.168.0.64;port=3306;user id=passport;Password=12473513;" +
                    //                  "persist security info=True;database=passports;charset=cp1251;" +
                    //                  "Default Command Timeout=60";


                    string conn = "server={$db_server$};user id={$db_user$};Password={$db_pass$};" +
                              "persist security info=True;database={$db_name$}; charset=cp1251;" +
                              "Default Command Timeout=60";
                    StringBuilder sb = new StringBuilder(conn);
                    sb.Replace("{$db_server$}", d.PasspHostDb);
                    sb.Replace("{$db_name$}", d.PasspNameDb);
                    sb.Replace("{$db_user$}", d.PasspUserDb);
                    sb.Replace("{$db_pass$}", d.PasspPassDb);
                    conn = sb.ToString();


                    MySqlConnection con_test = new MySqlConnection(conn);
                    MySqlCommand cmd;
                    con_test.Open();
                    
                    cmd = new MySqlCommand("select count(value) from `list_of_expired_passports` where value=@number", con_test);
                    cmd.Parameters.AddWithValue("@number", Convert.ToInt64(FizDocSeries + FizDocNumber));
                    cmd.ExecuteNonQuery();
                    MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                    DataTable t = new DataTable();
                    ada.Fill(t);

                    try
                    {
                        cmd = new MySqlCommand("select count(value) from `list_of_expired_passports2` where value=@number", con_test);
                        cmd.Parameters.AddWithValue("@number", Convert.ToInt64(FizDocSeries + FizDocNumber));
                        cmd.ExecuteNonQuery();
                        MySqlDataAdapter ada1 = new MySqlDataAdapter(cmd);
                        DataTable t1 = new DataTable();
                        ada1.Fill(t1);
                        if ( Convert.ToInt64(t1.Rows[0].ItemArray[0]) > 0 )
                        {
                            statusPassport = false;
                        }
                    }
                    catch ( Exception )
                    {
                    }
                    con_test.Close();
                    if ( Convert.ToInt64(t.Rows[0].ItemArray[0]) > 0 )
                    {
                        statusPassport = false;
                    }
                }
                else
                {
                    DataTable tu = d.getQuery("select SQL_CALC_FOUND_ROWS value from `list_of_expired_passports` where value='{0}'", Convert.ToInt64(FizDocSeries + FizDocNumber));
                    //if ( tu != null && tu.Rows.Count > 0 && Convert.ToInt32(tu.Rows[0]) > 0)
                    if ( tu != null )
                    {
                        statusPassport = false;
                    }
                }
                 */

            }
            catch ( Exception )
            {
            }
            





            if ( !statusPassport )
                ret.Add("Серия и номер паспорта запрещены для заполнения");
//
            
            if (Convert.ToInt32(xml["CitizenType"].Text) == 1) 
            {
                try
                {
                    DateTime dtFizDocDate = DateTime.ParseExact(xml["FizDocDate"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    DateTime dtBirth = DateTime.ParseExact(xml["Birth"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    DateTime dtDocDate;
                    DateTime dnow = DateTime.Now;

                    try
                    {
                        dtDocDate = DateTime.ParseExact(xml["DocDate"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        dtDocDate = dnow;
                        ret.Add("Дата договора не указана");
                    }

                    int YearsPassed = dtFizDocDate.Year - dtBirth.Year;
                    if (dtFizDocDate.Month < dtBirth.Month || (dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day < dtBirth.Day))
                    {
                        YearsPassed--;
                    }
                    if (YearsPassed < 14) ret.Add("На момент выдачи паспорта, лицу было менее 14 лет");

                    Dictionary<int, int> interval = new Dictionary<int, int>();
                    interval.Add(0, 20);
                    interval.Add(20, 45);
                    interval.Add(45, 200);
                    if (dnow.Year < dtFizDocDate.Year) ret.Add("Дата выдачи документа не может быть больше текущей");


                    // проверка на террориста

                    /*
                    foreach (KeyValuePair<int, int> kvp in interval)
                    {
                        // определим какой промежуток
                        if (dtFizDocDate.Year - dtBirth.Year >= kvp.Key && dtFizDocDate.Year - dtBirth.Year <= kvp.Value)
                        {
                            if (dtFizDocDate.Year - dtBirth.Year == kvp.Value && (dtFizDocDate.Month > dtBirth.Month || (dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day >= dtBirth.Day)))
                            {
                                continue;
                            }
                            // сразу отсеем по году если он отличается
                            if (dtBirth.Year + kvp.Value < dnow.Year)
                            {
                                ret.Add("Паспорт просрочен");
                                break;
                            }
                            // теперь, если год равен, то определим, просрочен ли паспорт
                            if (dtFizDocDate.Year - dtBirth.Year == kvp.Value || dtDocDate.Year - dtBirth.Year == kvp.Value)
                            {
                                if (dtFizDocDate.Year - dtBirth.Year < kvp.Value && dtFizDocDate.Year - dtBirth.Year > 14)
                                {
                                    if (dtBirth.Month < dtDocDate.Month || dtBirth.Month == dtDocDate.Month && dtBirth.Day > dtDocDate.Day)
                                    {

                                        // посчитаем, сколько дней прошло после того, как был просрочен паспорт. Если < 30, то пока Ок

                                        int dif = DateTime.DaysInMonth(dtDocDate.Year, dtBirth.Month) - dtBirth.Day + dtDocDate.Day;
                                        if (((dtBirth.Month == dtDocDate.Month) || dtBirth.Month < dtDocDate.Month && dtBirth.Month + 1 == dtDocDate.Month))
                                        {
                                            //условие если не прошел месяц с момента просрочки паспорта
                                            if (dtBirth.Month == dtDocDate.Month && dtBirth.Day < dtDocDate.Day - 1)
                                            {
                                            }
                                            else if (dtBirth.Month < dtDocDate.Month && dtBirth.Month + 1 == dtDocDate.Month)
                                            {
                                                //int dif = DateTime.DaysInMonth(dtDocDate.Year, dtBirth.Month) - dtBirth.Day + dtDocDate.Day;
                                                if (dif > 29)
                                                {
                                                    ret.Add("Паспорт просрочен");
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ret.Add("Паспорт просрочен");
                                            break;
                                        }

                                    }

                                }

                                if (dtDocDate.Year - dtBirth.Year == kvp.Value)
                                {
                                    if (dtBirth.Month < dtDocDate.Month || dtBirth.Month == dtDocDate.Month && dtBirth.Day < dtDocDate.Day)
                                    {
                                        if (((dtBirth.Month == dtDocDate.Month) || dtBirth.Month < dtDocDate.Month && dtBirth.Month + 1 == dtDocDate.Month) && dtBirth.Day < dtDocDate.Day - 1)
                                        {
                                            //условие если не прошел месяц с момента просрочки паспорта
                                            if (dtBirth.Month == dtDocDate.Month && dtBirth.Day < dtDocDate.Day - 1)
                                            {
                                            }
                                            else if (dtBirth.Month < dtDocDate.Month && dtBirth.Month + 1 == dtDocDate.Month)
                                            {
                                                int dif = DateTime.DaysInMonth(dtDocDate.Year, dtBirth.Month) - dtBirth.Day + dtDocDate.Day;
                                                if (dif > 30)
                                                {
                                                    ret.Add("Паспорт просрочен");
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ret.Add("Паспорт просрочен");
                                            break;
                                        }
                                    }
                                }

                                
                            }
                        }


                    }
                    */

                    string nodejsserver = "";
                    try
                    {
                        DataTable t1 = ((IDEXData)toolbox).getQuery("select rvalue from `registers` where rname = 'nodejsserver'");
                        nodejsserver = t1.Rows[0]["rvalue"].ToString();
                    }
                    catch (Exception)
                    {
                    }

                    bool isOnline = ((IDEXUserData)toolbox).isOnline;
                    string currentBase = isOnline == true ? ((IDEXUserData)toolbox).currentBase : ((IDEXUserData)toolbox).dataBase;
                    IDEXServices idis = (IDEXServices)toolbox;
                    JObject authData = new JObject();
                    string adaptersUid = "";
                    authData["login"] = "admin";
                    authData["password"] = "12473513";
                    //JObject authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3000", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                    JObject authObj = new JObject();
                    try
                    {
                        //authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
                        authObj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/start?data=" + JsonConvert.SerializeObject(authData) + "&clientType=dexol", 1));
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
                                    //currentBase = jo["dex_dexol_base_name"].ToString();
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


                    try
                    {
                        JObject packet = new JObject();
                        packet["com"] = "dexdealer.adapters.beeline";
                        packet["subcom"] = "apiChcPassport";
                        packet["client"] = "dexol";
                        packet["data"] = new JObject();
                        packet["data"]["base"] = currentBase;
                        packet["data"]["vendor"] = "beeline";
                        packet["data"]["birth"] = xml["Birth"].Text;
                        packet["data"]["docDate"] = xml["FizDocDate"].Text;
                        packet["data"]["currentDate"] = xml["DocDate"].Text;
                        packet["data"]["series"] = xml["FizDocSeries"].Text.Replace(" ", "");
                        packet["data"]["number"] = xml["FizDocNumber"].Text;
                        if (!isOnline) packet["data"]["ignoreUid"] = 1;
                        else packet["data"]["ignoreUid"] = 0;
                        JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol", 1));
                        foreach (string jo in (JArray)o["data"]["err"])
                        {
                            ret.Add(jo);
                        }

                    }
                    catch (Exception)
                    {
                        ret.Add("Ошибка пакета сервера при попытке проверить данные паспорта");
                    }

                    string checkDate = dtFizDocDate.Year.ToString();
                    Regex rgxCheck = new Regex("\\s+");
                    string FizDocSeries = rgxCheck.Replace(xml["FizDocSeries"].Text, "");
                    int chDate = int.Parse(checkDate.Substring(2, 2));
                    int chFds = int.Parse(FizDocSeries.Substring(2, 2));
                    if (chDate == 0 && chDate != chFds && 100 - chFds < 4) chDate = 100;
                    else if (100 - chFds < 3 && 100 - chFds < 4 && chDate != chFds) chDate = 100;
                    else if (chDate == 0 && chDate != chFds && 100 - chFds > 3) ret.Add("Паспорт просрочен");

                    if (Math.Abs(chDate - chFds) > 3)
                    {
                        ret.Add("Вы ввели неверные данные для паспорта");
                    }

                }
                catch (Exception) { }
            }


            if (xml["AddrCountry"].Text.Trim().Equals("")) ret.Add("Не указана страна (Адрес регистрации)");

            if (xml["AddrState"].Text.Trim().Equals("")) ret.Add("Не указана область проживания (Адрес регистрации)");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("AddrCityType", true).Text);
            }
            catch (Exception) { }
            if (dc < 0) ret.Add("Не указан тип населенного пункта (Адрес регистрации)");

            if (xml["AddrCity"].Text.Trim().Equals("")) ret.Add("Не указано наименование населенного пункта (Адрес регистрации)");
            if (!DOL2Data.CheckOrgAttribute(xml["AddrZip"].Text, true, 6))
                ret.Add("Индекс населенного пункта должен содержать только 6 цифр (Адрес регистрации)");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("AddrStreetType", true).Text);
            }
            catch (Exception) { }
            if (dc < 0) ret.Add("Не указан тип улицы (Адрес рег.)");
            if (xml["AddrStreet"].Text.Trim().Equals("")) ret.Add("Не указано наименование улицы (Адрес регистрации)");
            bool houseExists = !xml["AddrHouse"].Text.Trim().Equals("");
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("AddrBuildingType", true).Text);
            }
            catch (Exception) { }

            bool buildingExists = (dc >= 0 && !xml["AddrBuilding"].Text.Trim().Equals(""));

            if (!houseExists && !buildingExists) ret.Add("Или дом или строение должны быть заполнены (Адрес рег.)");


            // проверить, заполнены ли поля места пребывания
            if (xml["ResidenceAddrCountry"].Text.Trim().Equals("")) ret.Add("Не указана страна (Адрес пребывания)");
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ResidenceAddrCityType", true).Text);
            }
            catch (Exception) { }
            if (dc < 0) ret.Add("Не указан тип населенного пункта (Адрес пребывания)");
            if (xml["ResidenceAddrCity"].Text.Trim().Equals("")) ret.Add("Не указано наименование населенного пункта (Адрес пребывания)");
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ResidenceAddrStreetType", true).Text);
            }
            catch (Exception) { }
            if (dc < 0) ret.Add("Не указан тип улицы (Адрес пребывания)");
            if (xml["ResidenceAddrStreet"].Text.Trim().Equals("")) ret.Add("Не указано наименование улицы (Адрес пребывания)");
            if (!DOL2Data.CheckOrgAttribute(xml["ResidenceAddrZip"].Text, true, 6))
                ret.Add("Индекс населенного пункта должен содержать только 6 цифр (Адрес пребывания)");
            houseExists = !xml["ResidenceAddrHouse"].Text.Trim().Equals("");
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ResidenceAddrBuildingType", true).Text);
            }
            catch (Exception) { }

            buildingExists = (dc >= 0 && !xml["ResidenceAddrBuilding"].Text.Trim().Equals(""));

            if (!houseExists && !buildingExists) ret.Add("Или дом или строение должны быть заполнены (Адрес пребывания.)");



           
            

            dc = -1;
            buildingExists = false;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("AddrBuildingType", true).Text);
            }
            catch (Exception) { }
            buildingExists = xml["AddrBuilding"].Text.Trim().Equals("") && dc < 0;
            if (!buildingExists)
            {
                if (xml["AddrBuilding"].Text.Trim().Equals("") || dc < 0)
                {
                    ret.Add("'Тип строения' и 'Строение' должны быть либо оба заполнены либо оба пусты (Адрес регистрации)");
                }
            }

            
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("AddrApartmentType", true).Text);
            }
            catch (Exception) { }

            bool apartmentEmpty = xml["AddrApartment"].Text.Trim().Equals("") && dc < 0;

            if (!apartmentEmpty)
            {
                if (xml["AddrApartment"].Text.Trim().Equals("") || dc < 0)
                {
                    ret.Add("'Тип апартаментов' и 'Апартаменты' должны быть либо оба заполнены либо оба пусты (Адрес регистрации)");
                }
            }


            dc = -1;
            buildingExists = false;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ResidenceAddrBuildingType", true).Text);
            }
            catch (Exception) { }
            buildingExists = xml["ResidenceAddrBuilding"].Text.Trim().Equals("") && dc < 0;
            if (!buildingExists)
            {
                if (xml["ResidenceAddrBuilding"].Text.Trim().Equals("") || dc < 0)
                {
                    ret.Add("'Тип строения' и 'Строение' должны быть либо оба заполнены либо оба пусты (Адрес места пребывания)");
                }
            }


            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ResidenceAddrApartmentType", true).Text);
            }
            catch (Exception) { }

            apartmentEmpty = xml["ResidenceAddrApartment"].Text.Trim().Equals("") && dc < 0;

            if (!apartmentEmpty)
            {
                if (xml["ResidenceAddrApartment"].Text.Trim().Equals("") || dc < 0)
                {
                    ret.Add("'Тип апартаментов' и 'Апартаменты' должны быть либо оба заполнены либо оба пусты (Адрес места пребывания)");
                }
            }

/*
            else if (docorgtype == 0)
            {
                ret.Add("Не указано помещение (Адрес рег.)");
            }
 */

//

            bool faxMandatory = false, emailMandatory = false;

            dc = -5;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DeliveryType", true).Text);
            }
            catch (Exception) { }

            if (dc == 1 || dc == 2 || dc == 3 || dc == 4 || dc == 5 || dc == 10)
            { // Проверяем адрес доставки
                if (xml["DeliveryCountry"].Text.Trim().Equals("")) ret.Add("Не указана страна (Адрес доставки)");
                
                int dc2 = -1;
                try
                {
                    dc2 = int.Parse(xml.GetNodeByPath("DeliveryCityType", true).Text);
                }
                catch (Exception) { }
                if (dc2 < 0) ret.Add("Не указан тип населенного пункта (Адрес доставки)");

                if (xml["DeliveryCity"].Text.Trim().Equals("")) ret.Add("Не указано наименование населенного пункта (Адрес доставки)");
                if (!DOL2Data.CheckOrgAttribute(xml["DeliveryZip"].Text, true, 6))
                    ret.Add("Индекс населенного пункта должен содержать только 6 цифр (Адрес доставки)");

                dc2 = -1;
                try
                {
                    dc2 = int.Parse(xml.GetNodeByPath("DeliveryStreetType", true).Text);
                }
                catch (Exception) { }
                if (dc2 < 0) ret.Add("Не указан тип улицы (Адрес рег.)");
                if (xml["DeliveryStreet"].Text.Trim().Equals("")) ret.Add("Не указано наименование улицы (Адрес доставки)");

                houseExists = !xml["DeliveryHouse"].Text.Trim().Equals("");

                /*
                dc2 = -1;
                try
                {
                    dc2 = int.Parse(xml.GetNodeByPath("DeliveryBuildingType", true).Text);
                }
                catch (Exception) { }

                buildingExists = (dc2 >= 0 && !xml["DeliveryBuilding"].Text.Trim().Equals(""));

                if (!houseExists && !buildingExists) ret.Add("Или дом или строение должны быть заполнены (Адрес доставки)");
                */
                dc = -1;
                buildingExists = false;
                try
                {
                    dc = int.Parse(xml.GetNodeByPath("DeliveryBuildingType", true).Text);
                }
                catch (Exception) { }
                buildingExists = xml["DeliveryBuilding"].Text.Trim().Equals("") && dc < 0;
                if (!buildingExists)
                {
                    if (xml["DeliveryBuilding"].Text.Trim().Equals("") || dc < 0)
                    {
                        ret.Add("'Тип строения' и 'Строение' должны быть либо оба заполнены либо оба пусты (Адрес доставки корреспонденции)");
                    }
                }


                dc2 = -1;
                try
                {
                    dc2 = int.Parse(xml.GetNodeByPath("DeliveryApartmentType", true).Text);
                }
                catch (Exception) { }

                apartmentEmpty = xml["DeliveryApartment"].Text.Trim().Equals("") && dc2 < 0;

                if (!apartmentEmpty)
                {
                    if (xml["DeliveryApartment"].Text.Trim().Equals("") || dc2 < 0)
                    {
                        ret.Add("'Тип помещения' и 'Помещение' должны быть либо оба заполнены либо оба пусты (Адрес доставки корреспонденции)");
                    }
                }
/*
                else if ((dc == 1 || dc == 2 || dc == 5 || dc == 10) && docorgtype == 0)
                {
                    ret.Add("Не указано помещение (Адрес доставки)");
                }
 */
            } else if (dc == 6)
            { // Проверяем факс
                faxMandatory = true;
            }
            else if (dc == 7)
            { // Проверяем E-Mail
                emailMandatory = true;
            } // Иначе доставка не указана
            else
            {
                ret.Add("'Способ доставки' не может быть пустым! (Адрес доставки корреспонденции)");
            }

            //сфера деятельности дожна быть выбрана и должно содержать поле "Другие"
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DocSphere", true).Text);
            }
            catch (Exception) { }
            if (dc != 12) ret.Add("'Сфера деятельности' не может быть пустым или не быть равным 'Другие'. (Учетные данные абонента)");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DocClientType", true).Text);
            }
            catch (Exception) { }
            if (dc != 0) ret.Add("'Тип клиента' не может быть пустым или не быть равным 'ЧАСТНОЕ ЛИЦО'. (Учетные данные абонента)");


            bool faxEmpty = xml["ContactFaxPrefix"].Text.Trim().Length < 1 && xml["ContactFax"].Text.Trim().Length < 1;

            if (!faxEmpty)
            {
                if (!DOL2Data.CheckOrgAttribute(xml["ContactFaxPrefix"].Text, 3, 6))
                    ret.Add("Факс: префикс должен содержать от 3 до 6 цифр (Контактное лицо)");
                if (!DOL2Data.CheckOrgAttribute(xml["ContactFax"].Text, 4, 7))
                    ret.Add("Факс: номер должен содержать от 4 до 7 цифр (Контактное лицо)");
            }
            else
            {
                if (faxMandatory) ret.Add("Не указан факс (Контактное лицо)");
            }


            //if (!DOL2Data.CheckOrgAttribute(xml["ContactPhonePrefix"].Text, 3, 6))
            //    ret.Add("Телефон: префикс должен содержать от 3 до 6 цифр (Контактное лицо)");
            if (!rxcontactPrefix.IsMatch(xml["ContactPhonePrefix"].Text)) ret.Add("Телефон: префикс должен содержать 3 цифры (Контактное лицо)");
            if (!rxcontactPhone.IsMatch(xml["ContactPhone"].Text)) ret.Add("Телефон: номер должен содержать 7 цифр (Контактное лицо)");
            //if (!DOL2Data.CheckOrgAttribute(xml["ContactPhone"].Text, 4, 7))
            //    ret.Add("Телефон: номер должен содержать от 4 до 7 цифр (Контактное лицо)");

            if (xml["ContactEmail"].Text.Trim().Length > 0)
            {
                if (!rxemail.IsMatch(xml["ContactEmail"].Text.Trim())) ret.Add("Указан некорректный E-mail (Контактное лицо)");
            }
            else
            {
                if (emailMandatory) ret.Add("Не указан E-Mail (Контактное лицо)");
            }

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("ContactSex", true).Text);
            }
            catch (Exception) { }
            if (dc != 0 && dc != 1) ret.Add("Не указан пол (Контактное лицо)");

            if (xml["ContactFio"].Text.Trim().Length < 2) ret.Add("Ф.И.О. должно содержать более 2-х символов (Контактное лицо)");

            if (!DOL2Data.CheckOrgAttribute(xml["MSISDN"].Text, true, 10)) ret.Add("MSISDN должен содержать только 10 цифр");
            else if (!DOL2Data.CheckString(xml["MSISDN"].Text, false, true)) ret.Add("MSISDN должен содержать только цифры");

            if (!DOL2Data.CheckOrgAttribute(xml["ICC"].Text, false, 18)) ret.Add("ICC должен содержать только 18 символов");

            if (xml.GetNodeByPath("Plan", false) == null) ret.Add("Не выбран ТП документа");

            //if ("1".Equals(xml["Dynamic"].Text)) {
                //if (!DOL2Data.CheckOrgAttribute(xml["MSISDN"].Text, true, 10)) ret.Add("Присвоенный номер должен содержать только 10 цифр");
                //if (!DOL2Data.CheckString(xml["NEWMSISDN"].Text, false, true)) ret.Add("Присвоенный номер должен содержать только цифры");
            //}

            return ret;
        }

        public ArrayList GetDocumentCriticals(Object toolbox)
        {
            ArrayList ret = new ArrayList();
            ret.Add("DocNum");
            ret.Add("MSISDN");
            ret.Add("ICC");
            return ret;
        }

        public Dictionary<string, string> GetDocumentFields(Object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret["AssignedDPCode"] = "Присвоенный код точки";
            ret["DocDate"] = "Дата подписания договора";
            ret["DocCity"] = "Город подписания договора";
            ret["DocNum"] = "№ договора";
            ret["ABSCode"] = "Код в АБС";
            ret["PlanPrn"] = "ТП";
            ret["ICC"] = "ICC";
            ret["Unittitlesim"] = "Отделение при распределении sim";
            ret["MSISDN"] = "MSISDN";
            ret["OLDNEWDOLMSISDN"] = "OLDNEWDOLMSISDN";
            ret["Dynamic"] = "Динамическая";
            ret["DocClientType"] = "Тип клиента";                                
            ret["Birth"] = "Дата рождения";
            ret["Sex"] = "Пол";
            ret["BirthPlace"] = "Место рождения";
            ret["FirstName"] = "Имя";
            ret["SecondName"] = "Отчество";
            ret["LastName"] = "Фамилия";
            ret["FizDocDate"] = "Дата выдачи удостоверения";
            ret["FizDocOrg"] = "Организация выдавшая удостоверение";
            ret["FizDocOrgCode"] = "Код организации выдавшей удостоверение личности";
            ret["FizDocScan"] = "Скан документа";
            ret["FizDocNumber"] = "Номер удостоверения";
            ret["FizDocSeries"] = "Серия удостоверения";
            ret["FizDocType"] = "Тип удостоверения"; // <Doctype printable="" />
            ret["FizDocCitizen"] = "Гражданство"; //
            ret["FizInn"] = "ИНН физ.лица";
            ret["CompanyTitle"] = "Компания";
            ret["CompanyInn"] = "Компания: ИНН";
            ret["CompanyOkonh"] = "Компания: ОКОНХ";
            ret["CompanyOkpo"] = "Компания: ОКПО";
            ret["CompanyKpp"] = "Компания: КПП";
            ret["OrgBank"] = "Банк";
            ret["OrgRs"] = "Банк: Р/С";
            ret["OrgKs"] = "Банк: К/С";
            ret["OrgBik"] = "Банк: БИК";
            ret["AddrCountry"] = "Страна";
            ret["AddrZip"] = "Индекс";
            ret["AddrRegion"] = "Район";
            ret["AddrState"] = "Область";
            ret["AddrCity"] = "Город";
            ret["AddrStreet"] = "Улица";
            ret["AddrHouse"] = "Дом";
            ret["AddrBuilding"] = "Корпус";
            ret["AddrApartment"] = "Квартира";
            ret["DeliveryCountry"] = "Страна (Д)";
            ret["DeliveryZip"] = "Индекс (Д)";
            ret["DeliveryRegion"] = "Район (Д)";
            ret["DeliveryState"] = "Область (Д)";
            ret["DeliveryCity"] = "Город (Д)";
            ret["DeliveryStreet"] = "Улица (Д)";
            ret["DeliveryHouse"] = "Дом (Д)";
            ret["DeliveryBuilding"] = "Корпус (Д)";
            ret["DeliveryApartment"] = "Квартира (Д)";
            ret["DeliveryComment"] = "Комментарий (Д)";

            //ret["StartBalance"] = "Стартовый баланс";

            return ret;
        }

        public string GetFieldValueText(Object toolbox, string fieldname, string value)
        {
            try
            {
                if ("Dynamic".Equals(fieldname)) {
                        return "1".Equals(value) ? "Да" : "";
                } else if (fieldname.Equals("FizDocType"))
                {
                    string[] dct = { "", "Паспорт", "Иностранный паспорт", "Военный билет" };
                    return dct[int.Parse(value)];
                } 
                else
                if (fieldname.Equals("DocClientType"))
                {
                    string[] dct2 = { "Физ.лицо", "Юр.лицо" };
                    return dct2[int.Parse(value)];
                } 

                return value;
            }
            catch (Exception)
            {
            }
            return "-";
        }

        public StringList GetPeopleData(Object toolbox, IDEXDocumentData document)
        {
            SimpleXML xml = SimpleXML.LoadXml(document.documentText);

            StringList sl = new StringList();

            sl["Sex"] = xml["Sex"].Text;
            sl["FirstName"] = xml["FirstName"].Text;
            sl["SecondName"] = xml["SecondName"].Text;
            sl["LastName"] = xml["LastName"].Text;
            sl["Birth"] = xml["Birth"].Text;
            sl["FizDocType"] = xml["FizDocType"].Text;
            sl["FizDocCitizen"] = xml["FizDocCitizen"].Text;
            sl["FizDocSeries"] = xml["FizDocSeries"].Text;
            sl["FizDocNumber"] = xml["FizDocNumber"].Text;
            sl["FizDocOrg"] = xml["FizDocOrg"].Text;
            if (xml.GetNodeByPath("FizDocOrgCode", false) != null) sl["FizDocOrgCode"] = xml["FizDocOrgCode"].Text;
            sl["FizDocScan"] = xml["FizDocScan"].Text;
            sl["FizDocDate"] = xml["FizDocDate"].Text;
            sl["FizBirthPlace"] = xml["FizBirthPlace"].Text;
            sl["FizInn"] = xml["FizInn"].Text;
            sl["OrgBank"] = xml["OrgBank"].Text;
            sl["OrgRs"] = xml["OrgRs"].Text;
            sl["OrgKs"] = xml["OrgKs"].Text;
            sl["OrgBik"] = xml["OrgBik"].Text;
            sl["AddrCountry"] = xml["AddrCountry"].Text;
            sl["AddrState"] = xml["AddrState"].Text;
            sl["AddrRegion"] = xml["AddrRegion"].Text;
            if (xml.GetNodeByPath("AddrCityType", false) != null) sl["AddrCityType"] = xml["AddrCityType"].Text;
            sl["AddrCity"] = xml["AddrCity"].Text;
            sl["AddrZip"] = xml["AddrZip"].Text;
            if (xml.GetNodeByPath("AddrStreetType", false) != null) sl["AddrStreetType"] = xml["AddrStreetType"].Text;
            sl["AddrStreet"] = xml["AddrStreet"].Text;
            sl["AddrHouse"] = xml["AddrHouse"].Text;
            if (xml.GetNodeByPath("AddrBuildingType", false) != null) sl["AddrBuildingType"] = xml["AddrBuildingType"].Text;
            sl["AddrBuilding"] = xml["AddrBuilding"].Text;
            if (xml.GetNodeByPath("AddrApartmentType", false) != null) sl["AddrApartmentType"] = xml["AddrApartmentType"].Text;
            sl["AddrApartment"] = xml["AddrApartment"].Text;
            if (xml.GetNodeByPath("AutoDocRegId", false) != null) sl["AutoDocRegId"] = xml["AutoDocRegId"].Text;

            if (xml.GetNodeByPath("FizDocScan", false) != null) sl["FizDocScan"] = xml["FizDocScan"].Text;
            if (xml.GetNodeByPath("FizDocScanMime", false) != null) sl["FizDocScanMime"] = xml["FizDocScanMime"].Text;



            return sl;
        }

       
        /*
        #region IDEXPluginSetup Members

        void IDEXPluginSetup.Setup(object toolbox)
        {
            
            try 
            {
                SetupForm sf = new SetupForm();
                IDEXConfig cfg = (IDEXConfig)toolbox;
                sf.cbSuggestFIO.Checked = cfg.getBool(ID, "suggest_fio", false);
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    cfg.setBool(ID, "suggest_fio", sf.cbSuggestFIO.Checked);
                }
            }
            catch (Exception) { }
                            
        }

        #endregion
         */
    }
}
