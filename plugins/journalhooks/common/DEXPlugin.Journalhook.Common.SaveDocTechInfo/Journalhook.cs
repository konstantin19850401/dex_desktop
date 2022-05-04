using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

using System.IO;
using System.Windows.Forms;




namespace DEXPlugin.Journalhook.Common.SaveDocTechInfo
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Common.SaveDocTechInfo";
            }
        }
        public string Title
        {
            get
            {
                return "Выгрузить для диагностики";
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
                return "Выгружает для диагностики все поля из таблицы journal.";
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
                (DocStatus == 2 || DocStatus == 4) &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string,string>();
            if (hookVisible) ret["savetechinfo"] = "Выгрузить для диагностики";
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
                    if (FunctionName.Equals("savetechinfo") &&
                    docId.StartsWith("DEXPlugin.Document.")  &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    IDEXData d = (IDEXData)toolbox;
                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                    DataTable tt = d.getQuery(string.Format("select * from `journal` where id = '{0}'",
                        int.Parse(doc.documentIdJournal)));

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = doc.documentIdJournal;
                    sfd.DefaultExt = ".text";
                    sfd.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

                    if (sfd.ShowDialog() == DialogResult.OK) {
                        StreamWriter tw = new StreamWriter(sfd.FileName);
                        tw.WriteLine("===============================================");
                        tw.WriteLine(string.Format("Дата: {0}", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")));
                        tw.WriteLine("===============================================");

                        foreach (DataColumn dc in tt.Columns) {
                            tw.WriteLine("[ {0} ] => {1}", dc.ColumnName, Convert.ToString(tt.Rows[0][dc.ColumnName]));
                        }
                        tw.WriteLine("===============================================");
                        tw.Flush();
                        tw.Close();
                    }
                }
         
            }
            catch (Exception) { }

            return false;
        }
    }
}
