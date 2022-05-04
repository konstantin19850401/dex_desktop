using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXPlugin.Journalhook.MTS.ExportScanFromApp
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.MTS.ExportScanFromApp";
            }
        }
        public string Title
        {
            get
            {
                return "Приложить скан документа удостоверяющего личность";
            }
        }

        public string[] Path
        {
            get
            {
                return null;
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
                return "Прикладывает сканированное изображение документа удостоверяющего личность";
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

        bool hookVisible = false;
        string newcode = null;

        object toolbox;

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            this.toolbox = toolbox;
            newcode = null;
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            hookVisible = DocType.StartsWith("DEXPlugin.Document.MTS.Jeans") &&
                DocStatus == 4 &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (hookVisible) ret["ExportScanFromApp"] = "Приложить скан документа удостоверяющего личность";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            return null;
        }

        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            try
            {
                if (FunctionName.Equals("ExportScanFromApp") &&
                    docId.StartsWith("DEXPlugin.Document.MTS.Jeans") &&
                    doc.documentStatus == 4 &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    IDEXServices idis = (IDEXServices)toolbox;
                    string currentBase = ((IDEXUserData)toolbox).dataBase;
                    string pfBase = "";
                    JObject objInfoBase = new JObject();

                    DataTable t1 = ((IDEXData)toolbox).getQuery("select rvalue from `registers` where rname = 'nodejsserver'");
                    string nodejsserver = t1.Rows[0]["rvalue"].ToString();
                    objInfoBase = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/adapters/getDexDexolBase?&clientType=dexol", 1));
                    foreach (JObject jo in objInfoBase["data"])
                    {
                        if (jo["list"].ToString().Equals(currentBase))
                        {
                            pfBase = jo["dex_dexol_base_name"].ToString();
                        }
                    }

                    JObject packet = new JObject();
                    packet["com"] = "dexdealer.adapters.mts";
                    packet["subcom"] = "apiExportScanFromApp";
                    packet["client"] = "dexol";
                    packet["data"] = new JObject();
                    packet["data"]["vendor"] = "mts";
                    packet["data"]["base"] = pfBase;
                    packet["data"]["documentid"] = doc.documentIdJournal;
                    JObject obj = JObject.Parse(idis.sendRequest("GET", nodejsserver, "3020", "/dexdealer/beeline/cmd?packet=" + JsonConvert.SerializeObject(packet) +  "&clientType=dexol&", 1));
                    string msg = "";
                    if (Convert.ToInt32(obj["data"]["status"].ToString()) == 1) 
                    {
                        msg = "Статус эскпорта - успешно!";
                        MessageBox.Show("Экспорт скана произведен успешно!");
                    } 
                    else 
                    {
                        msg = "Статус экспорта - ошибка!";
                        MessageBox.Show("Экспорт скана произведен с ошибкой!");
                    }
                    ((IDEXDocumentJournal)toolbox).AddRecord(
                        "Предпринята попытка экспорта скана вручную.",
                        new string[] 
                        {
                            string.Format(msg)
                        }
                    );
                    docChanged = true;
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
