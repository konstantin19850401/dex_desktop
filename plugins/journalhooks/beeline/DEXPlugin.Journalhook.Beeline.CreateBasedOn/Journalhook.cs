using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

namespace DEXPlugin.Journalhook.Beeline.CreateBasedOn
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Beeline.CreateBasedOn";
            }
        }
        public string Title
        {
            get
            {
                return "Создать документ на основании";
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
                return "Создает новый документ на основании выбранного.";
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
        object toolbox;

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            this.toolbox = toolbox;
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            hookVisible = DocType.StartsWith("DEXPlugin.Document.") &&
                (DocStatus == 1 || DocStatus == 2 || DocStatus == 4 || DocStatus == 5) &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string,string>();
            if (hookVisible) ret["createbasedon"] = "Создать документ на основании";
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
                if (FunctionName.Equals("createbasedon") &&
                    docId.StartsWith("DEXPlugin.Document.") &&
                    (doc.documentStatus == 1 || doc.documentStatus == 2 || doc.documentStatus == 4 || doc.documentStatus == 5) &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    IDEXData d = (IDEXData)toolbox;

                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                    DataTable t = null;
                    /*
                    IDEXPluginDocument dd = new ;
            
                    
                    IDEXDocumentData newdoc = doc;
                    
                    if (dd.CloneDocument(toolbox, doc, newdoc)) 
                    {
                        string dddd = "fff";
                    } 
                    else 
                    {
                        MessageBox.Show("Ошибка клонирования документа");
                    }
                    */



                    docChanged = false;
                }
                else
                {
                    MessageBox.Show("Невозможно создать документ на основании выделенного ввиду неподходящего статуса!");
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
