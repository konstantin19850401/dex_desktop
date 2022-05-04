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

namespace DEXPlugin.Document.Mega.EAD
{
    public class Document : IDEXPluginDocument
    {

        public string ID
        {
            get
            {
                return "DEXPlugin.Document.Mega.EAD.Fiz";
            }
        }
        public string Title
        {
            get
            {
                return "МегаФон: Единый абонентский договор (Физ)";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = { "МегаФон" };
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
                return "Форма ввода данных Единого абонентского договора МегаФон для физ.лиц";
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
            r.AddRightsItem(ID + ".new", "МегаФон ЕАД: Создание документа");
            r.AddRightsItem(ID + ".edit", "МегаФон ЕАД: Изменение документа");
            r.AddRightsItem(ID + ".delete", "МегаФон ЕАД: Удаление документа");
//            r.AddRightsItem(ID + ".approve", "МегаФон ЕАД: Быстрое подтверждение документа");

            IDEXConfig c = (IDEXConfig)toolbox;
            if (!c.isRegisterKeyExists("megafon_icc_mask"))
            {
                try
                {
                    c.createRegisterKey("megafon_icc_mask", "МегаФон: Маска ICC", "00000000000000000");
                }
                catch (Exception)
                {
                }
            }
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
                            string.Format("Пользователь: {0}", ((IDEXUserData)toolbox).Title)
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
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ((IDEXDocumentJournal)toolbox).AddRecord("Содержимое документа изменено");
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

            if (xml.Attributes["ID"] != ID) ret.Add("Не соответствует ID документа");
            int dc = -1;
            try
            {
                dc = int.Parse(xml.GetNodeByPath("DocCategory", true).Text);
            }
            catch (Exception) { }
            if (dc != 0 && dc != 1) ret.Add("Категория абонента не указана");

            if (xml.GetNodeByPath("DocCity", true).Text.Trim().Length < 1)
                ret.Add("Не указан город подписания договора");

            bool docDateOk = true;

            if (!rxdate.IsMatch(xml.GetNodeByPath("DocDate", true).Text.Trim()))
            {
                ret.Add("Некорректная дата подписания договора");
                docDateOk = false;
            }

            /*
            if (xml.GetNodeByPath("DocNum", true).Text.Trim().Length < 1)
                ret.Add("Не указан номер договора");
            */

            string tpt = xml.GetNodeByPath("Plan", true).Text.Trim();
            int tpl = xml.GetNodeByPath("Plan", true).Text.Trim().Length;

            if (xml.GetNodeByPath("Plan", true).Text.Trim().Length < 1)
                ret.Add("Не указан ТП");

            if (xml.GetNodeByPath("ICCCTL", true).Text.Trim().Length < 1)
                ret.Add("Не указан контрольный символ ICC");

            if (xml.GetNodeByPath("ICC", true).Text.Trim().Length < 17)
                ret.Add("Некорректный ICC");

            if (xml.GetNodeByPath("MSISDN", true).Text.Trim().Length < 10)
                ret.Add("Некорректный MSISDN");

            if (!rxdate.IsMatch(xml.GetNodeByPath("Birth", true).Text.Trim()))
            {
                ret.Add("Некорректная дата рождения абонента");
                docDateOk = false;
            }

            if (docDateOk)
            {
                try
                {
                    string dd1 = xml["Birth"].Text.Trim(), dd2 = xml["DocDate"].Text.Trim();
                    DateTime d1 = new DateTime(int.Parse(dd1.Substring(6, 4)), int.Parse(dd1.Substring(3, 2)), int.Parse(dd1.Substring(0, 2)));
                    DateTime d2 = new DateTime(int.Parse(dd2.Substring(6, 4)), int.Parse(dd2.Substring(3, 2)), int.Parse(dd2.Substring(0, 2)));

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

            if (xml.GetNodeByPath("FirstName", true).Text.Trim().Length < 1)
                ret.Add("Не указано имя абонента");

            if (xml.GetNodeByPath("SecondName", true).Text.Trim().Length < 1)
                ret.Add("Не указано отчество абонента");

            if (xml.GetNodeByPath("LastName", true).Text.Trim().Length < 1)
                ret.Add("Не указана фамилия абонента");

            if (!rxdate.IsMatch(xml.GetNodeByPath("FizDocDate", true).Text.Trim()))
                ret.Add("Некорректная дата получения удостоверения личности");

            if (xml.GetNodeByPath("FizDocOrg", true).Text.Trim().Length < 1)
                ret.Add("Не указана организация, выдавшая удостоверение личности");

            bool docok = true;
            
            if (xml.GetNodeByPath("FizDocNumber", true).Text.Trim().Length < 1)
            {
                ret.Add("Не указан номер удостоверения личности");
                docok = false;
            }

            if (xml.GetNodeByPath("FizDocSeries", true).Text.Trim().Length < 1)
            {
                ret.Add("Не указана серия удостоверения личности");
                docok = false;
            }

            if (xml.GetNodeByPath("FizDocType", true).Text.Trim().Length < 1)
            {
                ret.Add("Не указан вид удостоверения личности");
                docok = false;
            }

            if (docok && xml.GetNodeByPath("FizDocType", true).Text.Trim().Equals("паспорт", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    string fds = xml.GetNodeByPath("FizDocSeries", true).Text.Trim();
                    if (fds.Length != 4) throw new Exception();
                    int.Parse(fds);
                }
                catch (Exception)
                {
                    ret.Add("Некорректная серия паспорта");
                }

                try
                {
                    string fdn = xml.GetNodeByPath("FizDocNumber", true).Text.Trim();
                    if (fdn.Length != 6) throw new Exception();
                    int.Parse(fdn);
                }
                catch (Exception)
                {
                    ret.Add("Некорректный номер паспорта");
                }
            }

            if (xml.GetNodeByPath("AddrZip", true).Text.Trim().Length < 1)
                ret.Add("Не указан почтовый индекс адреса абонента");

            if (xml.GetNodeByPath("AddrCity", true).Text.Trim().Length < 1)
                ret.Add("Не указан город адреса абонента");

            if (xml.GetNodeByPath("AddrStreet", true).Text.Trim().Length < 1)
                ret.Add("Не указана улица адреса абонента");


            return ret;
        }

        public ArrayList GetDocumentCriticals(Object toolbox)
        {
            ArrayList ret = new ArrayList();
//            ret.Add("DocNum");
            ret.Add("MSISDN");
            ret.Add("ICC");
            return ret;
        }

        public Dictionary<string, string> GetDocumentFields(Object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret["DocCategory"] = "Категория абонента";
            ret["DocCity"] = "Город подписания договора";
            ret["DocDate"] = "Дата подписания договора";
            ret["DocNum"] = "№ договора";
            ret["Plan"] = "ТП";
            ret["PlanRegionId"] = "Код региона ТП";
            ret["ICCCTL"] = "ICC контрольный";
            ret["ICC"] = "ICC";
            ret["MSISDN"] = "MSISDN";
            ret["Birth"] = "Дата рождения";
            ret["FirstName"] = "Имя";
            ret["SecondName"] = "Отчество";
            ret["LastName"] = "Фамилия";
            ret["FizDocDate"] = "Дата выдачи удостоверения";
            ret["FizDocOrg"] = "Организация выдавшая удостоверение";
            ret["FizDocNumber"] = "Номер удостоверения";
            ret["FizDocSeries"] = "Серия удостоверения";
            ret["FizDocType"] = "Тип удостоверения";
            ret["FizDocScan"] = "Скан документа";
            ret["AddrStreet"] = "Улица";
            ret["AddrZip"] = "Индекс";
            ret["AddrRegion"] = "Район";
            ret["AddrState"] = "Область";
            ret["AddrCity"] = "Город";
            ret["AddrApartment"] = "Квартира";
            ret["AddrBuilding"] = "Корпус";
            ret["AddrHouse"] = "Дом";
            ret["ProfileName"] = "Профиль отправки";
            ret["sbms_paccount"] = "Лицевой счет";

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
            sl["FirstName"] = xml.GetNodeByPath("FirstName", true).Text;
            sl["SecondName"] = xml.GetNodeByPath("SecondName", true).Text;
            sl["LastName"] = xml.GetNodeByPath("LastName", true).Text;
            sl["Birth"] = xml.GetNodeByPath("Birth", true).Text;
            sl["FizDocType"] = xml.GetNodeByPath("FizDocType", true).Text;
            sl["FizDocSeries"] = xml.GetNodeByPath("FizDocSeries", true).Text;
            sl["FizDocNumber"] = xml.GetNodeByPath("FizDocNumber", true).Text;
            sl["FizDocOrg"] = xml.GetNodeByPath("FizDocOrg", true).Text;
            sl["FizDocDate"] = xml.GetNodeByPath("FizDocDate", true).Text;
            sl["FizDocScan"] = xml.GetNodeByPath("FizDocScan", true).Text;
            sl["AddrState"] = xml.GetNodeByPath("AddrState", true).Text;
            sl["AddrRegion"] = xml.GetNodeByPath("AddrRegion", true).Text;
            sl["AddrCity"] = xml.GetNodeByPath("AddrCity", true).Text;
            sl["AddrZip"] = xml.GetNodeByPath("AddrZip", true).Text;
            sl["AddrStreet"] = xml.GetNodeByPath("AddrStreet", true).Text;
            sl["AddrHouse"] = xml.GetNodeByPath("AddrHouse", true).Text;
            sl["AddrBuilding"] = xml.GetNodeByPath("AddrBuilding", true).Text;
            sl["AddrApartment"] = xml.GetNodeByPath("AddrApartment", true).Text;
            sl["sbms_paccount"] = xml.GetNodeByPath("smbs_paccount", true).Text;

            sl["AutoDocRegId"] = xml.GetNodeByPath("AutoDocRegId", true).Text;



            return sl;
        }


    }
}
