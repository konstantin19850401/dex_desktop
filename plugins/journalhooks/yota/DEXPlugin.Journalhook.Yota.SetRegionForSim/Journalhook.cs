using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DEXSIM;

namespace DEXPlugin.Journalhook.Yota.SetRegionForSim
{
    public class Journalhook: IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Yota.SetRegionForSim";
            }
        }
        public string Title
        {
            get
            {
                return "Отписать сим-карты на регион";
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
                return "Отписывает сим-карты выбранному региону";
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
            catch ( Exception )
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
            hookVisible = DocType.StartsWith("DEXPlugin.Document.");
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if ( hookVisible )
                ret["changefieldvalue"] = "Отписать выбранное на регион ...";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            
            Dictionary<string, string> ret = new Dictionary<string, string>();

            DataTable dt = ( (IDEXData)toolbox ).getQuery("select * from `um_regions` order by title");

            if ( dt != null && dt.Rows.Count > 0 )
            {
                foreach ( DataRow r in dt.Rows )
                {
                    ret[r["region_id"].ToString()] = r["title"].ToString();
                }
            }
            return ret;
        }
        public string libname(string namepart)
        {
            return "yota_" + namepart;
        }
        
        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            
            try
            {
                SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                if ( xml == null )
                    return false;

                if ( FunctionName.Equals("changefieldvalue") && docId.StartsWith("DEXPlugin.Document.") )
                {
                    IDEXData d = (IDEXData)toolbox;
                    d.runQuery(
                           "update `um_data` set region_id = '{0}' where icc = '{1}'",
                           SubFunctionName, xml["ICC"].Text);
                    docChanged = false;
                }
            }
            catch ( Exception )
            {
            }

            return false;
        }
    }
}
