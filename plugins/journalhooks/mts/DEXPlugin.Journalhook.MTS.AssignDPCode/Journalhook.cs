using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;

namespace DEXPlugin.Journalhook.MTS.AssignDPCode
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.MTS.AssignDPCode";
            }
        }
        public string Title
        {
            get
            {
                return "Присвоить код точки продаж МТС";
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
                return "Присваивает код точки продаж выделенным документам для отправки в УД МТС.";
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
                (DocStatus < 6 && DocStatus != 4) &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (hookVisible) ret["assignmtsdpcode"] = "Присвоить код точки продаж";
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
                if (FunctionName.Equals("assignmtsdpcode") &&
                    docId.StartsWith("DEXPlugin.Document.MTS.Jeans") &&
                    (doc.documentStatus < 6 && doc.documentStatus != 4) &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    if (newcode == null)
                    {
                        JHFormDPCode dpcf = new JHFormDPCode();
                        if (dpcf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            newcode = dpcf.resultCode;
                        }
                        else
                        {
                            docChanged = false;
                            return true;
                        }
                    }

                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                    xml["AssignedDPCode"].Text = newcode;
                    //xml["AllDpCode"].Text = newcode;

                    string DPCodeKind = "НП";

                    IDEXData d = (IDEXData)toolbox;

                    DataTable dt = d.getQuery("select kind from `mts_units_dp` where dpcode = '" + d.EscapeString(newcode) + "' and uid = " + doc.documentUnitID);
                    if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["kind"]) == 1)
                    {
                        DPCodeKind = "П";
                    }


                    xml["DPCodeKind"].Text = DPCodeKind;
                    doc.documentText = SimpleXML.SaveXml(xml);
                    docChanged = true;
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
