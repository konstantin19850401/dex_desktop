using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

using MySql.Data.MySqlClient;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Document.Yota.Contract
{
    public class Document : IDEXPluginDocument, IDEXPluginSetup
    {

        public string ID
        {
            get
            {
                return "DEXPlugin.Document.Yota.Contract";
            }
        }
        public string Title
        {
            get
            {
                return "Yota. Публичный договор";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = { "Yota" };
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
                return "Yota. Заполнение публичного договора Yota";
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
            r.AddRightsItem(ID + ".new", "Yota Публичный договор: Создание документа");
            r.AddRightsItem(ID + ".edit", "Yota Публичный договор: Изменение документа");
            r.AddRightsItem(ID + ".delete", "Yota Публичный договор: Удаление документа");
            //            r.AddRightsItem(ID + ".approve", "МегаФон ЕАД: Быстрое подтверждение документа");

            IDEXConfig c = (IDEXConfig)toolbox;
            if (!c.isRegisterKeyExists("yota_icc_mask"))
            {
                try
                {
                    c.createRegisterKey("yota_icc_mask", "Yota: Маска ICC", "00000000000000000");
                }
                catch (Exception)
                {
                }
            }
            /*
            if (!c.isRegisterKeyExists("megafon_msisdn_mask"))
            {
                try
                {
                    c.createRegisterKey("megafon_msisdn_mask", "МегаФон: Маска MSISDN", "0000000000");
                }
                catch (Exception)
                {
                }
            }
            */
        }

        public bool NewDocument(Object toolbox, IDEXDocumentData document)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".new") || r.IsSuperUser())
            {
                DocumentForm form = new DocumentForm();
                form.toolbox = toolbox;
                form.module = this;

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
                MessageBox.Show("У пользователя отсутствуют права создания документов этого типа");
            }
            return ret;
        }

        public bool CloneDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".new") || r.IsSuperUser())
            {
                DocumentForm form = new DocumentForm();
                form.toolbox = toolbox;
                form.module = this;
                form.InitDocument(source, document, true, false);
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
                            string.Format("ID в системе: {0}", ((IDEXUserData)toolbox).Login),
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

        public bool EditDocument(Object toolbox, IDEXDocumentData source, IDEXDocumentData document, IDEXDocumentData changes, bool ReadOnly)
        {
            bool ret = false;
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".edit") || r.IsSuperUser())
            {
                DocumentForm form = new DocumentForm();
                form.toolbox = toolbox;
                form.module = this;
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

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
            Regex rxpasogrcode = new Regex(@"^\d{3}-\d{3}$");

            bool statusPassport = false;

            if (xml.Attributes["ID"] != ID) ret.Add("Не соответствует ID документа");
            //if ("".Equals(xml["DocNum"].Text.Trim())) ret.Add("Не указан номер договора");
            if ("".Equals(xml["DocCategory"].Text.Trim()) || "0".Equals(xml["DocCategory"].Text.Trim())) ret.Add("Категория абонента не указана");
            if ("".Equals(xml["DocClientType"].Text.Trim()) || "0".Equals(xml["DocClientType"].Text.Trim())) ret.Add("Тип клиента не указан");

            //if (xml["MSISDN"].Text.Trim().Length < 10) ret.Add("Некорректный MSISDN");
            if (xml["ICC"].Text.Trim().Length < 10) ret.Add("Некорректный ICC");

            if (xml["isOnline"].Text.Equals("1"))
            {
                //if (xml["FizDocType"].Text != "passport_rf") ret.Add("В данный момент через dexol возможна регистрация только с типом документа 'Паспорт гражданина РФ'");
                //if (xml["DocClientType"].Text != "1") ret.Add("В данный момент через dexol возможна регистрация только на резидентов");
            }

            //if (xml["ICCCTL"].Text.Trim().Length < 1) ret.Add("Не указан контрольный символ ICC");

            //- if ("".Equals(xml["Plan"].Text.Trim())) ret.Add("Не указан ТП");

            if ("".Equals(xml["FirstName"].Text.Trim())) ret.Add("Не указано имя абонента");
            //if ("".Equals(xml["SecondName"].Text.Trim())) ret.Add("Не указано отчество абонента");
            if ("".Equals(xml["LastName"].Text.Trim())) ret.Add("Не указана фамилия абонента");
            if ("".Equals(xml["Sex"].Text.Trim()) || "0".Equals(xml["Sex"].Text.Trim())) ret.Add("Не указан пол абонента");
            //if ("".Equals(xml["FizBirthPlace"].Text.Trim())) ret.Add("Не указано место рождения абонента");

            int dc = -1;
            /*
            try
            {
                dc = int.Parse(xml.GetNodeByPath("FizDocCitizen", true).Text);
            }
            catch (Exception) { }
            if (dc < 1) ret.Add("Не указано гражданство");
            if (dc == 253 && xml["FizDocCitizenOther"].Text.Equals(""))
            {
                ret.Add("Для типа гражданства Другое, вы должны заполнить поле Другое");
            }
            */
            if (!xml["FizDocType"].Text.Equals("passport_rf"))
            {
                //if (xml["FizDocTypeResidence"].Text.Equals("")) ret.Add("Вы не указали тип документа, подтверждающего право пребывания в РФ");
                //if (xml["FizDocResidenceSeries"].Text.Equals("")) ret.Add("Вы не указали серию для документа, подтверждающего право пребывания в РФ");
                //if (xml["FizDocResidenceNumber"].Text.Equals("")) ret.Add("Вы не указали номер для документа, подтверждающего право пребывания в РФ");
                //if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim()))
                //    ret.Add("Некорректная дата начала действия документа, подтверждающего право пребывания в РФ");
                //if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceEnd", true).Text.Trim()))
                //    ret.Add("Некорректная дата окончания действия документа, подтверждающего право пребывания в РФ");
                
            }



            // разберемся с гражданством
            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("FizDocCitizen", true).Text);
            }
            catch (Exception) { }
            if (xml["FizDocType"].Text.Equals("passport_rf") && dc != 171)
            {
                ret.Add("Для ДУЛ типа Паспорт РФ гражданство может быть только РОССИЯ");
            }
            if (xml["FizDocType"].Text.Equals("passport_inostr") || xml["FizDocType"].Text.Equals("diplomatic_passport")) 
            {
                if (dc < 1) ret.Add("Вы не указали гражданство");
            }
            if (dc == 253 && xml["FizDocCitizenOther"].Text.Equals(""))
            {
                ret.Add("Для типа гражданства Другое, вы должны заполнить поле Другое");
            }
            if (xml["FizDocType"].Text.Equals("other"))
            {
                if (xml["FizDocOtherDocTypes"].Text.Equals("soldier_identity_card") || xml["FizDocOtherDocTypes"].Text.Equals("military_ticket") || xml["FizDocOtherDocTypes"].Text.Equals("sailor_identity_card"))
                {
                    if (dc != 171)
                    {
                        ret.Add("'Для документов с типом Удостоверение личности военнослужащего', 'Военный билет солдата, матроса, сержанта, старшины, прапорщика, мичмана', 'Удостоверение личности моряка' гражданство может быть только РОССИЯ)");
                    }
                }

                if (xml["FizDocOtherDocTypes"].Text.Equals("refuge_card") || xml["FizDocOtherDocTypes"].Text.Equals("considering_refuge_status_card") || xml["FizDocOtherDocTypes"].Text.Equals("temp_asylum_card"))
                {
                    if (dc < 1) ret.Add("Вы не указали гражданство");
                }

            }


            // разберемся с типом документа Другой
            if (xml["FizDocType"].Text.Equals("other"))
            {
                if (xml["FizDocOtherDocTypes"].Text.Equals(""))
                {
                    ret.Add("Вы не указали тип документа для ДУЛ Другой");
                }

                if (xml["FizDocOtherDocTypes"].Text.Equals("residence_permit"))
                {
                    //if ()
                }
            }




            
            
            if (!rxdate.IsMatch(xml["Birth"].Text.Trim()))
            {
                ret.Add("Некорректная дата рождения абонента");
            }
            else
            {
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
                catch (Exception) { }
            }


            

            if ( !rxdate.IsMatch(xml.GetNodeByPath("DocDate", true).Text.Trim()) )
                ret.Add("Некорректная дата подписания договора");

            if ((xml["FizDocOtherDocTypes"].Text.Equals("temp_residence_permit") 
                || xml["FizDocOtherDocTypes"].Text.Equals("temp_residence_permit")
                || xml["FizDocOtherDocTypes"].Text.Equals("temp_asylum_card")
                || xml["FizDocOtherDocTypes"].Text.Equals("considering_refuge_status_card")
                || xml["FizDocOtherDocTypes"].Text.Equals("refuge_card")
                ) && xml["FizDocType"].Text.Equals("other"))
            {
                if ("".Equals(xml["FizDocResidenceDocNumber"].Text.Trim())) ret.Add("Не указан номер удостоверения личности");
                if ("".Equals(xml["FizDocResidenceDocSeries"].Text.Trim())) ret.Add("Не указана серия удостоверения личности");

                string ddd = xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim();
                DateTime dtDocDate;
                try
                {
                    dtDocDate = DateTime.ParseExact(xml["FizDocResidenceStart"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    ret.Add("Дата начала действия документа не указана");
                }
                try
                {
                    dtDocDate = DateTime.ParseExact(xml["FizDocResidenceEnd"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    ret.Add("Дата окончания действия документа не указана");
                }

                if ("".Equals(xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim()))
                    ret.Add("Дата начала действия документа не указана");
                if ("".Equals(xml.GetNodeByPath("FizDocResidenceEnd", true).Text.Trim()))
                    ret.Add("Дата окончания действия документа не указана");

                if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim()))
                    ret.Add("Некорректная дата начала действия документа");
                if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceEnd", true).Text.Trim()))
                    ret.Add("Некорректная дата окончания действия документа");
            }
            else
            {
                
                if ("".Equals(xml["FizDocNumber"].Text.Trim())) ret.Add("Не указан номер удостоверения личности");
                if ("".Equals(xml["FizDocSeries"].Text.Trim())) ret.Add("Не указана серия удостоверения личности");
                if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocDate", true).Text.Trim()))
                    ret.Add("Некорректная дата получения удостоверения личности");


                if ("0".Equals(xml["Refugee"]))
                {
                    if ((xml["FizDocType"].Text.Equals("passport_inostr") || xml["FizDocType"].Text.Equals("diplomatic_passport")) &&
                        ("".Equals(xml["FizDocTypeResidence"].Text.Trim())))
                        ret.Add("Вы не указали документ, подтверждающий право пребывания в РФ");
                }

                if ("0".Equals(xml["Refugee"]))
                {
                    if ((xml["FizDocType"].Text.Equals("passport_inostr") || xml["FizDocType"].Text.Equals("diplomatic_passport")))
                    {
                        if ("".Equals(xml["FizDocResidenceDocNumber"].Text.Trim())) ret.Add("Не указан номер документа, подтверждающего право пребывания в РФ");
                        if ("".Equals(xml["FizDocResidenceDocSeries"].Text.Trim())) ret.Add("Не указана серия документа, подтверждающего право пребывания в РФ");

                        if ("01.01.0001".Equals(xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim()))
                            ret.Add("Дата начала действия документа, подтверждающего право пребывания в РФ, не указана");
                        if ("01.01.0001".Equals(xml.GetNodeByPath("FizDocResidenceEnd", true).Text.Trim()))
                            ret.Add("Дата окончания действия документа, подтверждающего право пребывания в РФ, не указана");

                        if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceStart", true).Text.Trim()))
                            ret.Add("Некорректная дата начала действия документа, подтверждающего право пребывания в РФ");
                        if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocResidenceEnd", true).Text.Trim()))
                            ret.Add("Некорректная дата окончания действия документа, подтверждающего право пребывания в РФ");
                    }
                }
            }
            if ("".Equals(xml["FizDocOrg"].Text.Trim())) ret.Add("Не указана организация, выдавшая удостоверение личности");
            

            //проверка серии и номера паспорта на корректность по базе fms
           
            IDEXData d = (IDEXData)toolbox;
            string sss = xml["FizDocType"].Text;
            if ( xml["FizDocType"].Text == "passport_rf" ) {
                try
                {
                    IDEXServices idis = (IDEXServices)toolbox;
                    statusPassport = idis.checkPassport(xml["FizDocSeries"].Text, xml["FizDocNumber"].Text);
                    if (!statusPassport) ret.Add("Серия и номер паспорта запрещены для заполнения");
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
                        cmd.Parameters.AddWithValue("@number", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                        cmd.ExecuteNonQuery();
                        MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                        DataTable t = new DataTable();
                        ada.Fill(t);

                        try
                        {
                            cmd = new MySqlCommand("select count(value) from `list_of_expired_passports2` where value=@number", con_test);
                            cmd.Parameters.AddWithValue("@number", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                            cmd.ExecuteNonQuery();
                            MySqlDataAdapter ada1 = new MySqlDataAdapter(cmd);
                            DataTable t1 = new DataTable();
                            ada1.Fill(t1);
                            if (Convert.ToInt64(t1.Rows[0].ItemArray[0]) > 0 )
                            {
                                statusPassport = false;
                            }
                        }
                        catch ( Exception )
                        {
                        }
                        con_test.Close();
                        if ( Convert.ToInt64(t.Rows[0].ItemArray[0]) > 0)
                        {
                            statusPassport = false;
                        }
                    }
                    else
                    {
                        DataTable tu = d.getQuery("select SQL_CALC_FOUND_ROWS value from `list_of_expired_passports` where value='{0}'", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
                        //if ( tu != null && tu.Rows.Count > 0 && Convert.ToInt32(tu.Rows[0]) > 0)
                        if ( tu != null )
                        {
                            statusPassport = false;
                        }
                    }
                    */

                } catch (Exception) 
                {
                    ret.Add("Проверьте корректность серии и номера паспорта!");
                }
            }

            // проверить гражданство


            if ("passport_rf".Equals(xml["FizDocType"].Text.Trim()))
            {
                if (!rxpasogrcode.IsMatch(xml["FizDocOrgCode"].Text)) ret.Add("Код подразделения заполнен некорректно");
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
         
            if ("".Equals(xml["FizDocType"].Text.Trim()) || "0".Equals(xml["FizDocType"].Text.Trim())) ret.Add("Не указан вид удостоверения личности");
            else if ("passport_rf".Equals(xml["FizDocType"].Text.Trim()))
            {
                //if ("0".Equals(xml["Refugee"])) {
                    try
                    {
                        //int.Parse(xml["FizDocSeries"].Text.Trim());
                        if (xml["FizDocSeries"].Text.Trim().Length != 4) throw new Exception();
                    }
                    catch (Exception)
                    {
                        ret.Add("Серия паспорта должна содержать 4 цифры подряд, без пробелов");
                    }

                    try
                    {
                        int.Parse(xml["FizDocNumber"].Text.Trim());
                        if (xml["FizDocNumber"].Text.Trim().Length != 6) throw new Exception();
                    }
                    catch (Exception)
                    {
                        ret.Add("Номер паспорта должен содержать 6 цифр подряд, без пробелов");
                    }
                //}

                // сверка с базой данных
                try
                {
                    //if (  ) 
                    //{
                    //}
                    //else 
                   // {
                    //}
                }
                catch(Exception) 
                {
                }

                if ("1".Equals(xml["DocClientType"].Text.Trim()))
                {
                    try
                    {
                        //if (!rxpasogrcode.IsMatch(xml["FizDocOrgCode"].Text)) ret.Add("Код подразделения заполнен некорректно");v // Лена попросила убрать

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
                        interval.Add(14, 20);
                        interval.Add(20, 45);
                        interval.Add(45, 200);

                        if (dnow.Year < dtFizDocDate.Year) ret.Add("Дата выдачи документа не может быть больше текущей");


                        /*
                         int bYear = dtBirth.Year; // дата рождения
                         int fdYear = dtFizDocDate.Year; // дата выдачи документа
                         int dogYear = dtDocDate.Year; // дата заключения договора
                         int y20 = 20; // менять в 20 лет
                         int y45 = 45; // менять в 45 лет

                         // определим, сколько лет абоненту
                         int difYear = dnow.Year - bYear;

                         bool f = false;
                         if (difYear <= y20)
                         {
                             // проверяем 
                             f = true;
                         }
                         if (difYear <= y45 && !f)
                         {
                             // Если человеку меньше или ровно 45
                             if (difYear == y45)
                             {
                                 // если ровно, то проверим дату получения документа. Она должна быть не меньше дня рождения
                                
                                
                                 if (dtFizDocDate.Month < dtBirth.Month || dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day < dtBirth.Day)
                                 {
                                     // в этом случае проверим, возраст, когда выдан паспорт
                                     if (dtFizDocDate.Year - dtBirth.Year >= 20)
                                     {
                                         // паспорт выдан после 20
                                     }
                                 }

                             }
                         }
                         */
                        foreach (KeyValuePair<int, int> kvp in interval)
                        {
                            // определим какой промежуток
                            if (dtFizDocDate.Year - dtBirth.Year >= kvp.Key && dtFizDocDate.Year - dtBirth.Year <= kvp.Value)
                            {
                                if (dtFizDocDate.Year - dtBirth.Year == kvp.Value && (dtFizDocDate.Month > dtBirth.Month || (dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day > dtBirth.Day)))
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

                                    /*
                                    if (dtFizDocDate.Month > dtBirth.Month || (dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day > dtBirth.Day))
                                    {
                                        
                                        ret.Add("Паспорт просрочен");
                                        break;
                                    }
                                    */
                                }
                            }


                        }

                        string checkDate = dtFizDocDate.Year.ToString();
                        Regex rgxCheck = new Regex("\\s+");
                        string FizDocSeries = rgxCheck.Replace(xml["FizDocSeries"].Text, "");
                        int chDate = int.Parse(checkDate.Substring(2, 2));
                        int chFds = int.Parse(FizDocSeries.Substring(2, 2));

                        //if (chDate == 0 && chDate != chFds && 100 - chFds < 4) chDate = 100;
                        //else if (100 - chFds < 3 && 100 - chFds < 4 && chDate != chFds) chDate = 100;
                        //else if (chDate == 0 && chDate != chFds && 100 - chFds > 3) ret.Add("Паспорт просрочен");

                        int permissiblePeriod = 4;
                        if (chDate != chFds)
                        {
                            if (chDate <= permissiblePeriod && 100 - chFds <= permissiblePeriod) chDate += 100;
                            else if (chFds <= permissiblePeriod && 100 - chDate <= permissiblePeriod) chFds += 100;
                        }

                        if (Math.Abs(chDate - chFds) > 4)
                        {
                            ret.Add("Вы ввели неверные данные для паспорта. Разница между серией и годом выдачи не соответствует действительности");
                        }

                        /*
                        if (chDate == 0 && chFds > 3)
                        {
                            chDate = 100;
                            if (Math.Abs(chDate - chFds) > 3) 
                            {
                                ret.Add("Вы ввели неверные данные для паспорта");
                            }
                        }
                        else if ( chDate != 0 && Math.Abs(chDate - chFds) > 3) 
                        {
                            ret.Add("Вы ввели неверные данные для паспорта");
                        }
                        */

                    }
                    catch (Exception) { }
                }

                try
                {
                    DateTime dtFizDocDate = DateTime.ParseExact(xml["FizDocDate"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    DateTime dtBirth = DateTime.ParseExact(xml["Birth"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    int YearsPassed = dtFizDocDate.Year - dtBirth.Year;
                    if (dtFizDocDate.Month < dtBirth.Month || (dtFizDocDate.Month == dtBirth.Month && dtFizDocDate.Day < dtBirth.Day))
                    {
                        YearsPassed--;
                    }
                    if (YearsPassed < 14) ret.Add("На момент выдачи паспорта, лицу было менее 14 лет");
                }
                catch (Exception) { }
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

                if ("passport_rf".Equals(xml["FizDocType"].Text.Trim())) {
                    string[] pch = ((IDEXPeopleSearcher)toolbox).checkPassport(sl);
                    if (pch != null) foreach (string sa in pch) ret.Add(sa);
                }
            }

            if (xml["AddrZip"].Text.Trim().Length != 6) ret.Add("Почтовый индекс адреса абонента должен содержать 6 знаков");
            if (xml["AddrCity"].Text.Trim().Length < 1) ret.Add("Не указан город адреса абонента");
            else
            {
                try
                {
                    Dictionary<string, string> dic = ((IDEXCitySearcher)toolbox).getCityData("city", xml["AddrCity"].Text.Trim());
                    if (dic == null) ret.Add("Указан город, которого нет в справочнике городов");
                }
                catch (Exception) { }
            }

            if (xml["AddrStreet"].Text.Trim().Length < 1) ret.Add("Не указана улица адреса абонента");
            if (xml["AddrHouse"].Text.Trim().Length < 1) ret.Add("Не указан дом абонента");
            if (xml["AddrApartment"].Text.Trim().Length < 1) ret.Add("Не указана квартира абонента");

            //if ("".Equals(xml["DeliveryType"].Text.Trim()) || "0".Equals(xml["DeliveryType"].Text.Trim())) ret.Add("Не указан способ доставки");

            return ret;
        }

        public ArrayList GetDocumentCriticals(Object toolbox)
        {
            ArrayList ret = new ArrayList();
            //ret.Add("MSISDN");
            ret.Add("ICC");
            return ret;
        }

        public Dictionary<string, string> GetDocumentFields(Object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret["DocCategoryPrn"] = "Категория абонента";
            ret["DocCity"] = "Город подписания договора";
            //ret["DocNum"] = "№ договора";
            //ret["ICCCTL"] = "ICC контрольный";
            ret["ICC"] = "ICC";
            ret["DocReg"] = "Регион документа";
            //ret["MSISDN"] = "MSISDN";
            //ret["fs"] = "ФС";
//-            ret["PlanPrn"] = "ТП";
            ret["Unittitlesim"] = "Отделение при распределении sim";
           // ret["IntPlanPrn"] = "ТП (внутренний)";
            ret["FizBirthPlace"] = "Место рождения";
            ret["Birth"] = "Дата рождения";
            ret["FirstName"] = "Имя";
            ret["SecondName"] = "Отчество";
            ret["LastName"] = "Фамилия";
            ret["FizDocDate"] = "Дата выдачи удостоверения";
            ret["FizDocOrg"] = "Организация выдавшая удостоверение";
            ret["FizDocOrgCode"] = "Код подразделения";
            ret["FizDocNumber"] = "Номер удостоверения";
            ret["FizDocSeries"] = "Серия удостоверения";
            ret["FizDocTypePrn"] = "Тип удостоверения";
            ret["AddrCountryPrn"] = "Страна";
            ret["AddrStreet"] = "Улица";
            ret["AddrZip"] = "Индекс";
            ret["AddrRegion"] = "Район";
            ret["AddrState"] = "Область";
            ret["AddrCity"] = "Город";
            ret["AddrApartment"] = "Квартира";
            ret["AddrBuilding"] = "Корпус";
            ret["AddrHouse"] = "Дом";
            ret["AddrPhone"] = "Телефон";
           // ret["ProfileName"] = "Профиль отправки";
            ret["DocDate"] = "Дата подписания договора";
            ret["SellerId"] = "Код продавца";
            ret["ProfileCode"] = "Профиль отправки";
            return ret;
        }

        public string GetFieldValueText(Object toolbox, string fieldname, string value)
        {
            if ("fs".Equals(fieldname)) return ((value == "1" || true.ToString().Equals(value)) ? "ФС" : "");

            return value;
        }

        public StringList GetPeopleData(Object toolbox, IDEXDocumentData document)
        { // OK
            SimpleXML xml = SimpleXML.LoadXml(document.documentText);

            StringList sl = new StringList();
            sl["FirstName"] = xml.GetNodeByPath("FirstName", true).Text;
            sl["SecondName"] = xml.GetNodeByPath("SecondName", true).Text;
            sl["LastName"] = xml.GetNodeByPath("LastName", true).Text;

            sl["DocClientType"] = xml["DocClientType"].Text;
            sl["#isResident"] = "1".Equals(xml["DocClientType"].Text) ? "1" : "0";

            sl["Sex"] = xml["Sex"].Text;

            sl["Birth"] = xml.GetNodeByPath("Birth", true).Text;
            sl["FizBirthPlace"] = xml["FizBirthPlace"].Text;

            sl["FizDocType"] = xml.GetNodeByPath("FizDocType", true).Text;
            sl["FizDocSeries"] = xml.GetNodeByPath("FizDocSeries", true).Text;
            sl["FizDocNumber"] = xml.GetNodeByPath("FizDocNumber", true).Text;
            sl["FizDocOrg"] = xml.GetNodeByPath("FizDocOrg", true).Text;
            sl["FizDocOrgCode"] = xml.GetNodeByPath("FizDocOrgCode", true).Text;
            sl["FizDocDate"] = xml.GetNodeByPath("FizDocDate", true).Text;
            sl["DocReg"] = xml.GetNodeByPath("DocReg", true).Text;

            sl["AddrCountry"] = xml["AddrCountry"].Text;
            sl["AddrState"] = xml.GetNodeByPath("AddrState", true).Text;
            sl["AddrCity"] = xml.GetNodeByPath("AddrCity", true).Text;
            sl["AddrZip"] = xml.GetNodeByPath("AddrZip", true).Text;
            sl["AddrRegion"] = xml.GetNodeByPath("AddrRegion", true).Text;
            sl["AddrStreet"] = xml.GetNodeByPath("AddrStreet", true).Text;
            sl["AddrHouse"] = xml.GetNodeByPath("AddrHouse", true).Text;
            sl["AddrBuilding"] = xml.GetNodeByPath("AddrBuilding", true).Text;
            sl["AddrApartment"] = xml.GetNodeByPath("AddrApartment", true).Text;
            sl["AddrPhone"] = xml["AddrPhone"].Text;
            sl["ContactEmail"] = xml["ContactEmail"].Text;
            sl["FizInn"] = xml["FizInn"].Text;

            sl["DeliveryType"] = xml["DeliveryType"].Text;
            sl["DeliveryPhone"] = xml["DeliveryPhone"].Text;
            sl["DeliveryFax"] = xml["DeliveryFax"].Text;

            sl["DeliveryCountry"] = xml["DeliveryCountry"].Text;
            sl["DeliveryState"] = xml.GetNodeByPath("DeliveryState", true).Text;
            sl["DeliveryCity"] = xml.GetNodeByPath("DeliveryCity", true).Text;
            sl["DeliveryZip"] = xml.GetNodeByPath("DeliveryZip", true).Text;
            sl["DeliveryRegion"] = xml.GetNodeByPath("DeliveryRegion", true).Text;
            sl["DeliveryStreet"] = xml.GetNodeByPath("DeliveryStreet", true).Text;
            sl["DeliveryHouse"] = xml.GetNodeByPath("DeliveryHouse", true).Text;
            sl["DeliveryBuilding"] = xml.GetNodeByPath("DeliveryBuilding", true).Text;
            sl["DeliveryApartment"] = xml.GetNodeByPath("DeliveryApartment", true).Text;

            return sl;
        }

        #region IDEXPluginSetup Members

        public void Setup(object toolbox)
        {
            try
            {
                SetupForm sf = new SetupForm();
                IDEXConfig cfg = (IDEXConfig)toolbox;
/*
                IDEXCrypt crypt = (IDEXCrypt)toolbox;
                sf.tbUser.Text = cfg.getStr(ID, "sbms_user", "");
                string dpass = cfg.getStr(ID, "sbms_pass", "");
                if (dpass != "") dpass = crypt.Decrypt(dpass, ID);
                sf.tbPass.Text = dpass;
 */
                sf.cbExpressInput.Checked = cfg.getBool(ID, "express_input", false);
                if (sf.ShowDialog() == DialogResult.OK)
                {
//                    cfg.setStr(ID, "sbms_user", sf.tbUser.Text);
//                    cfg.setStr(ID, "sbms_pass", crypt.Encrypt(sf.tbPass.Text, ID));
                    cfg.setBool(ID, "express_input", sf.cbExpressInput.Checked);
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}
