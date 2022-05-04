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




namespace DEXPlugin.Journalhook.Common.DeleteUserInformation
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Common.DeleteUserInformation";
            }
        }
        public string Title
        {
            get
            {
                return "Удалить данные из базы по абоненту";
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
                return "Удаляет данные по абоненты из таблицы autodoc_people.";
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
                (DocStatus == 2 || DocStatus == 4 || DocStatus == 0) &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string,string>();
            if ( hookVisible )
                ret["deluserinfo"] = "Удалить данные из базы автодока по абоненту";
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
                if (MessageBox.Show("Вы действительно хотите удалить данные по абоненту?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes) 
                {
                    
                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);                    
                    IDEXData d = (IDEXData)toolbox;
                    //DataTable t = d.getQuery(string.Format("delete from `autodoc_people_usage`, `autodoc_people` where phash = (select phash from `autodoc_people` where data REGEXP '.*FizDocSeries={0}.*FizDocNumber={1}')",
                    DataTable t = d.getQuery(string.Format("delete from `autodoc_people` where phash = (select phash from `autodoc_people` where data REGEXP '.*FizDocSeries={0}.*FizDocNumber={1}')",
                       xml["FizDocSeries"].Text, xml["FizDocNumber"].Text));
                    docChanged = true;
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
