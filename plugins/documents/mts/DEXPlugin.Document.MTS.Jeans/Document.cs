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
using System.Globalization;

using MySql.Data.MySqlClient;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Document.MTS.Jeans
{
    public class Document : IDEXPluginDocument
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Document.MTS.Jeans";
            }
        }
        public string Title
        {
            get
            {
                return "Абонентский договор";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = { "МТС" };
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
                return "Форма ввода данных абонентского договора МТС для физ.лиц";
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
            r.AddRightsItem(ID + ".new", "МТС договор: Создание документа");
            r.AddRightsItem(ID + ".edit", "МТС договор: Изменение документа");
            r.AddRightsItem(ID + ".delete", "МТС договор: Удаление документа");

            IDEXConfig c = (IDEXConfig)toolbox;
            if (!c.isRegisterKeyExists("mts_icc_mask"))
            {
                try
                {
                    c.createRegisterKey("mts_icc_mask", "МТС: Маска ICC", "0000000000000000000");
                }
                catch (Exception)
                {
                }
            }
            if (!c.isRegisterKeyExists("mts_msisdn_mask"))
            {
                try
                {
                    c.createRegisterKey("mts_msisdn_mask", "МТС: Маска MSISDN", "0000000000");
                    
                }
                catch (Exception)
                {
                }
            }

            if (!c.isRegisterKeyExists("mts_dpcode_mask"))
            {
                try
                {
                    c.createRegisterKey("mts_dpcode_mask", "МТС: Маска кода точки продажи", "00000");

                }
                catch (Exception)
                {
                }
            }

            if (!c.isRegisterKeyExists("mts_dpcode_prefix"))
            {
                try
                {
                    c.createRegisterKey("mts_dpcode_prefix", "МТС: Префикс кода точки продажи", "0000");

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

            bool statusPassport = false;
            
            //8 = Валидация полей документа
            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
            Regex rxpasogrcode = new Regex(@"^\d{3}-\d{3}$");
            if (xml.Attributes["ID"] != ID) ret.Add("Не соответствует ID документа");

            string dpcode = xml.GetNodeByPath("AssignedDPCode", true).Text;
            if (dpcode.Length != 5)
                ret.Add("Длина кода дилерской точки может быть 5 символов");


            if (!rxdate.IsMatch(xml.GetNodeByPath("DocDate", true).Text.Trim()))
                ret.Add("Некорректная дата подписания договора");

            if (xml.GetNodeByPath("MSISDN", true).Text.Trim().Length < 10)
                ret.Add("Некорректный MSISDN");

            if (xml.GetNodeByPath("ICC", true).Text.Trim().Length < 17)
                ret.Add("Некорректный ICC");

            if (xml.GetNodeByPath("ICCCTL", true).Text.Trim().Length < 1)
                ret.Add("Не указан контрольный символ ICC");

            string tpt = xml.GetNodeByPath("Plan", true).Text.Trim();
            int tpl = xml.GetNodeByPath("Plan", true).Text.Trim().Length;

            if (xml.GetNodeByPath("Plan", true).Text.Trim().Length < 1)
                ret.Add("Не указан ТП");

            if (xml.GetNodeByPath("FizInn", false) != null)
            {
                string finn = xml["FizInn"].Text.Trim();
                if (finn.Length > 0)
                {
                    try
                    {
                        int.Parse(finn);
                        if (finn.Length != 12) throw new Exception();
                    }
                    catch (Exception)
                    {
                        ret.Add("Некорректный ИНН");
                    }
                }
            }

            if (xml["isOnline"].Text.Equals("1"))
            {
                //if (xml["FizDocType"].Text != "21") ret.Add("В данный момент через dexol возможна регистрация только с типом документа 'Паспорт гражданина РФ'");
                //if (xml["DocCategory"].Text != "0") ret.Add("В данный момент через dexol возможна регистрация только на резидентов");
            }

            int dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DocCategory", true).Text);
            }
            catch (Exception) { }
            if (dc != 0 && dc != 1) ret.Add("Не указана категория абонента");

            if (xml.GetNodeByPath("LastName", true).Text.Trim().Length < 1)
                ret.Add("Не указана фамилия абонента");

            //if (dc == 1) ret.Add("В данный момент производить регистрацию SIM-карт только на резидентов");

            if (xml.GetNodeByPath("FirstName", true).Text.Trim().Length < 1)
                ret.Add("Не указано имя абонента");

            //if (xml.GetNodeByPath("SecondName", true).Text.Trim().Length < 1)
            //    ret.Add("Не указано отчество абонента");

            if (!rxdate.IsMatch(xml.GetNodeByPath("Birth", true).Text.Trim()))
                ret.Add("Некорректная дата рождения абонента");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("Sex", true).Text);
            }
            catch (Exception) { }
            if (dc != 0 && dc != 1) ret.Add("Не указан пол абонента");

            if (xml.GetNodeByPath("AddrZip", true).Text.Trim().Length < 1)
                ret.Add("Не указан почтовый индекс адреса абонента");

            if (xml.GetNodeByPath("AddrCity", true).Text.Trim().Length < 1)
                ret.Add("Не указан город адреса абонента");

            if (xml.GetNodeByPath("AddrStreet", true).Text.Trim().Length < 1)
                ret.Add("Не указана улица адреса абонента");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("FizDocCountry", true).Text);
            }
            catch (Exception) { }
            if (dc < 1) ret.Add("Не указана страна выдачи документа");

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("FizDocType", true).Text);
            }
            catch (Exception) { }
            if (dc < 0) ret.Add("Не указан тип документа");

            //if (dc == 10) ret.Add("В данный момент документ 'иностранный паспорт' использовать для оформления договора.");

            if ( xml.GetNodeByPath("DocCategory", true).Text == "0" && xml.GetNodeByPath("FizDocSeries", true).Text.Trim().Length < 1 )
            {
                ret.Add("Не указана серия удостоверения личности");
            }
            else if ( xml.GetNodeByPath("DocCategory", true).Text == "1" && xml.GetNodeByPath("FizDocSeriesNotRez", true).Text.Trim().Length < 1 ) 
            {
                ret.Add("Не указана серия удостоверения личности");
            }

            if ( xml.GetNodeByPath("DocCategory", true).Text == "0" && xml.GetNodeByPath("FizDocNumber", true).Text.Trim().Length < 1 )
            {
                ret.Add("Не указан номер удостоверения личности");
            }
            else if ( xml.GetNodeByPath("DocCategory", true).Text == "1" && xml.GetNodeByPath("FizDocNumberNotRez", true).Text.Trim().Length < 1 )
            {
                ret.Add("Не указан номер удостоверения личности");
            }

            if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocDate", true).Text.Trim()))
                ret.Add("Некорректная дата получения удостоверения личности");


            /*
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
            */

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

                IDEXServices idis = (IDEXServices)toolbox;
                //statusPassport = idis.checkPassport(xml["FizDocSeries"].Text, xml["FizDocNumber"].Text);
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
                    DataTable tu = d.getQuery("select SQL_CALC_FOUND_ROWS value from `list_of_expired_passports` where value='{0}'", Convert.ToInt64(xml["FizDocSeries"].Text + xml["FizDocNumber"].Text));
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

            DateTime dtBirth = new DateTime();
            DateTime dnow = DateTime.Now;
            try
            {
                dtBirth = DateTime.ParseExact(xml["Birth"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                ret.Add("Дата рождения не указана");
            }
            // не более 100 лет абоненту
            if (dnow.Year - dtBirth.Year > 100 || (dnow.Year - dtBirth.Year == 100 && dnow.Month > dtBirth.Month) || (dnow.Year - dtBirth.Year == 100 && dnow.Month == dtBirth.Month && dnow.Day > dtBirth.Day))
            {
                ret.Add("Абоненту не может быть более 100 лет");
            }

            if (int.Parse(xml.GetNodeByPath("FizDocType", true).Text) == 21) //резидент и документ паспорт РФ
            {
                //if (!statusPassport)
                //    ret.Add("Серия и номер паспорта запрещены для заполнения");

                if (!rxpasogrcode.IsMatch(xml["FizDocOrgCode"].Text)) ret.Add("Код подразделения заполнен некорректно");

                DateTime dtFizDocDate = new DateTime();
                try
                {
                    dtFizDocDate = DateTime.ParseExact(xml["FizDocDate"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) 
                {
                    ret.Add("Дата получения документа удостоверяющего личность не указана");
                }
                
                DateTime dtDocDate;
                
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

                /*
                Dictionary<int, int> interval = new Dictionary<int, int>();
                interval.Add(0, 20);
                interval.Add(20, 45);
                interval.Add(45, 200);
                if (dnow.Year < dtFizDocDate.Year) ret.Add("Дата выдачи документа не может быть больше текущей");


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
                authData["password"] =  "12473513";
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
                    packet["com"] = "dexdealer.adapters.mts";
                    packet["subcom"] = "apiChcPassport";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["base"] = currentBase;
                    packet["data"]["vendor"] = "mts";
                    packet["data"]["birth"] = xml["Birth"].Text;
                    packet["data"]["docDate"] = xml["FizDocDate"].Text;
                    packet["data"]["currentDate"] = xml["DocDate"].Text;
                    packet["data"]["series"] = xml["FizDocSeries"].Text;
                    packet["data"]["number"] = xml["FizDocNumber"].Text;
                    if (!isOnline) packet["data"]["ignoreUid"] = 1;
                    else packet["data"]["ignoreUid"] = 0;
                    JObject o = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/mts/cmd?packet=" + JsonConvert.SerializeObject(packet) + "&uid=" + adaptersUid + "&clientType=dexol", 1));
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
                int chFds = -1;
                int chDate = -1;
                string FizDocSeries = rgxCheck.Replace(xml["FizDocSeries"].Text, "");
                try
                {
                    if (FizDocSeries.Equals("")) ret.Add("Вы не указали серию паспорта");
                    else
                    {
                        chFds = int.Parse(FizDocSeries.Substring(2, 2));
                    }
                }
                catch (Exception) 
                {
                    ret.Add("Проверьте правильность заполнения полей для документа удостоверяющего личность");
                }
                if (checkDate.Equals("1")) ret.Add("Вы не указали дату выдачи документа");
                else
                {
                    chDate = int.Parse(checkDate.Substring(2, 2));
                }

                if (chFds == -1 || chDate == -1)
                {
                }
                else
                {
                    //int chFds = int.Parse(FizDocSeries.Substring(2, 2));
                    //if (chDate == 0 && chDate != chFds && 100 - chFds < 4) chDate = 100;
                    //else if (100 - chFds < 3 && 100 - chFds < 4 && chDate != chFds) chDate = 100;
                    //else if (chDate == 0 && chDate != chFds && 100 - chFds > 3) ret.Add("Паспорт просрочен");

                    int permissiblePeriod = 4;
                    if (chDate != chFds)
                    {
                        if (chDate <= permissiblePeriod && 100 - chFds <= permissiblePeriod) chDate += 100;
                        else if (chFds <= permissiblePeriod && 100 - chDate <= permissiblePeriod) chFds += 100;
                    }

                    if (Math.Abs(chDate - chFds) > permissiblePeriod)
                    {
                        ret.Add("Вы ввели неверные данные для паспорта");
                    }
                }
                

                
            }

            dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("FizDocCitizen", true).Attributes["tag"]);
            }
            catch ( Exception )
            {
                if ( int.Parse(xml.GetNodeByPath("FizDocType", true).Text) == 21 )
                {
                    xml["FizDocCitizen"].Text = "Российская федерация";
                    xml["FizDocCitizen"].Attributes["tag"] = "1565";
                    dc = 1565;
                }
            }

       
            if (int.Parse(xml.GetNodeByPath("FizDocType", true).Text) != 21)
            {
                try
                {
                    DateTime fde = DateTime.ParseExact(xml["FizDocExp"].Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    if (fde < DateTime.Now)
                    {
                        ret.Add("Дата истечения документа не может быть меньше текущей даты");
                    }

                }
                catch (Exception)
                {
                    ret.Add("Не указана дата истечения документа");
                }
            }
          
            if ( dc < 1 )
                ret.Add("Не указано гражданство");

            if (xml.GetNodeByPath("FizDocOrg", true).Text.Trim().Length < 1)
                ret.Add("Не указана организация, выдавшая удостоверение личности");
            // комментарий убрать через месяц-два(27.11.2015)
            //if ( xml.GetNodeByPath("FizDocOrgCode", true).Text.Trim().Length < 1)
            //    ret.Add("Не указан код подразделения, выдавшего удостоверение личности");

            /*
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
            */
            
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
            

            try
            {
                string sdt = xml["DocDate"].Text;
                DateTime dtdoc = new DateTime(int.Parse(sdt.Substring(6, 4)), int.Parse(sdt.Substring(3, 2)), int.Parse(sdt.Substring(0, 2)));
                sdt = xml["Birth"].Text;
                DateTime dtbirth = new DateTime(int.Parse(sdt.Substring(6, 4)), int.Parse(sdt.Substring(3, 2)), int.Parse(sdt.Substring(0, 2)));

                TimeSpan tsp = dtdoc - dtbirth;
                if (tsp.TotalDays < 365.25f * 14) ret.Add("Абонент младше 14 лет");
            }
            catch (Exception) { }

            return ret;
        }

        public ArrayList GetDocumentCriticals(Object toolbox)
        {
            ArrayList ret = new ArrayList();
            ret.Add("MSISDN");
            ret.Add("ICC");
            return ret;
        }

        public Dictionary<string, string> GetDocumentFields(Object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret["AssignedDPCode"] = "Присвоенный код точки";
            ret["DPCodeKind"] = "Тип точки";
            ret["DocCategory"] = "Категория абонента";
            ret["DocDate"] = "Дата подписания договора";
            ret["Plan"] = "ТП";
            ret["ICCCTL"] = "ICC контрольный";
            ret["ICC"] = "ICC";
            ret["Unittitlesim"] = "Отделение при распределении sim";
            ret["MSISDN"] = "MSISDN";
            ret["Birth"] = "Дата рождения";
            ret["FirstName"] = "Имя";
            ret["SecondName"] = "Отчество";
            ret["LastName"] = "Фамилия";
            ret["FizDocDate"] = "Дата выдачи удостоверения";
            ret["FizDocOrg"] = "Организация выдавшая удостоверение";
            ret["FizDocOrgCode"] = "Код подразделения выдавшего удостоверение";
            ret["FizDocNumber"] = "Номер удостоверения";
            ret["FizDocSeries"] = "Серия удостоверения";
            ret["FizDocType"] = "Тип удостоверения";
            ret["FizDocScan"] = "Скан документа";
            ret["AddrStreet"] = "Улица";
            ret["AddrZip"] = "Индекс";
            ret["AddrState"] = "Область";
            ret["AddrCity"] = "Город";
            ret["AddrApartment"] = "Квартира";
            ret["AddrHouse"] = "Дом";

            return ret;
        }

        public string GetFieldValueText(Object toolbox, string fieldname, string value)
        {
            try
            {
                if (fieldname.Equals("DocCategory"))
                {
                    string[] dct = { "Резидент", "Нерезидент" };
                    return dct[int.Parse(value)];
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

            sl["FizInn"] = xml.GetNodeByPath("FizInn", true).Text;
            sl["DocCategory"] = xml.GetNodeByPath("DocCategory", true).Text;
            sl["LastName"] = xml.GetNodeByPath("LastName", true).Text;
            sl["FirstName"] = xml.GetNodeByPath("FirstName", true).Text;
            sl["SecondName"] = xml.GetNodeByPath("SecondName", true).Text;
            sl["Birth"] = xml.GetNodeByPath("Birth", true).Text;
            sl["Sex"] = xml.GetNodeByPath("Sex", true).Text;
            sl["AddrZip"] = xml.GetNodeByPath("AddrZip", true).Text;
            sl["AddrState"] = xml.GetNodeByPath("AddrState", true).Text;
            sl["AddrCity"] = xml.GetNodeByPath("AddrCity", true).Text;
            sl["AddrStreet"] = xml.GetNodeByPath("AddrStreet", true).Text;
            sl["AddrHouse"] = xml.GetNodeByPath("AddrHouse", true).Text;
            sl["AddrBuilding"] = xml.GetNodeByPath("AddrBuilding", true).Text;
            sl["AddrApartment"] = xml.GetNodeByPath("AddrApartment", true).Text;
            sl["FizBirthPlace"] = xml.GetNodeByPath("FizBirthPlace", true).Text;
            sl["FizDocCountry"] = xml.GetNodeByPath("FizDocCountry", true).Text;
            sl["FizDocType"] = xml.GetNodeByPath("FizDocType", true).Text;
            sl["FizDocSeries"] = xml.GetNodeByPath("FizDocSeries", true).Text;
            sl["FizDocNumber"] = xml.GetNodeByPath("FizDocNumber", true).Text;
            sl["FizDocDate"] = xml.GetNodeByPath("FizDocDate", true).Text;
            sl["FizDocOrg"] = xml.GetNodeByPath("FizDocOrg", true).Text;
            sl["FizDocOrgCode"] = xml.GetNodeByPath("FizDocOrgCode", true).Text;
            sl["FizDocScan"] = xml.GetNodeByPath("FizDocScan", true).Text;

            return sl;
        }

    }
}
