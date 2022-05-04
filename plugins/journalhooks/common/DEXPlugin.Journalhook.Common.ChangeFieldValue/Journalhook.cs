using System;
using System.Collections.Generic;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DEXSIM;

namespace DEXPlugin.Journalhook.Common.ChangeFieldValue
{
    public class Journalhook: IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Common.ChangeFieldValue";
            }
        }
        public string Title
        {
            get
            {
                return "Изменение значений полей документов";
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
                return "Изменяет значения некоторых полей в выделенных документах";
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

        String selectedField = null;
        String selectedFieldValue = null;

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            this.toolbox = toolbox;
            selectedField = null;
            selectedFieldValue = null;
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            hookVisible = DocType.StartsWith("DEXPlugin.Document.");
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if ( hookVisible )
                ret["changefieldvalue"] = "Изменить значение поля ...";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret["JDocDate"] = "Дата документа";
            ret["DocDate"] = "Дата заключения договора";
            ret["DocCity"] = "Город подключения";
            ret["FizDocScan"] = "Скан документа";
            ret["DealerName"] = "Ф.И.О. заполнившего договор";
            ret["DealerCode"] = "Код точки продаж";
            ret["UnitID"] = "Отделение продаж";
            // изменение тарифного плана (костя) 08.10.2015 начало
            //ret["Plan"] = "Тарифный план";
            // изменение тарифного плана (костя) 08.10.2015 конец
            return ret;
        }
        public string libname(string namepart)
        {
            return "beeline_" + namepart;
        }
        
        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            
            try
            {
                //ComboBox cbt = new ComboBox();
                if ( FunctionName.Equals("changefieldvalue") && docId.StartsWith("DEXPlugin.Document.") )
                {
                    if ( selectedField == null || selectedFieldValue == null )
                    {
                        // Если форма вернёт DialogResult.Cancel, возвращаем true
                        string ltitle = null, ltype = null;
                        if ( "JDocDate".Equals(SubFunctionName) )
                        {
                            ltitle = "Дата документа";
                            ltype = "d";
                        }
                        else if ( "DocDate".Equals(SubFunctionName) )
                        {
                            ltitle = "Дата заключения договора";
                            ltype = "d";
                        }
                        else if ( "DocCity".Equals(SubFunctionName) )
                        {
                            ltitle = "Город подключения";
                            ltype = "s";
                        }
                        else if ( "FizDocScan".Equals(SubFunctionName) )
                        {
                            ltitle = "Скан документа";
                            ltype = "b";
                        }
                        else if ( "DealerName".Equals(SubFunctionName) )
                        {
                            ltitle = "Ф.И.О. заполнившего договор";
                            ltype = "s";
                        }
                        else if ( "DealerCode".Equals(SubFunctionName) )
                        {
                            ltitle = "Код точки продаж";
                            ltype = "s";
                        }
                        else if ( "UnitID".Equals(SubFunctionName) )
                        {
                            ltitle = "Отделение продаж";
                            ltype = "u";
                        }
                        // изменение тарифного плана (костя) 08.10.2015 начало
                        /*
                        else if ("Plan".Equals(SubFunctionName)) 
                        {
                            ltitle = "Тарифный план";
                            ltype = "t";
                        }
                        */
                        // изменение тарифного плана (костя) 08.10.2015 конец
                        if ( ltitle == null || ltype == null )
                            return true;

                        FieldValueForm fvf = new FieldValueForm(toolbox, ltitle, ltype);

                        

                        if ( fvf.ShowDialog() != DialogResult.OK )
                            return true;

                        selectedField = SubFunctionName;
                        selectedFieldValue = fvf.returnValue;
                        //cbt = fvf.cbl;

                        


                        if ( "JDocDate".Equals(SubFunctionName) )
                        {
                            selectedFieldValue += doc.documentDate.Substring(8);
                        }
                        else if ( "DocDate".Equals(SubFunctionName) )
                        {
                            selectedFieldValue = selectedFieldValue.Substring(6, 2) + "." + selectedFieldValue.Substring(4, 2) + "." + selectedFieldValue.Substring(0, 4);
                        }
                    }

                    if ( "UnitID".Equals(selectedField) )
                    {
                        doc.documentUnitID = Convert.ToInt32(selectedFieldValue);
                    }
                    // изменение тарифного плана (костя) 08.10.2015 начало
                    /*
                    else if ("Plan".Equals(selectedField)) 
                    {
                       //SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                        //xml[selectedField].Text = selectedFieldValue;
                        //IDEXPlans plans = (IDEXPlans)toolbox;
                        IDEXPlans s = (IDEXPlans)toolbox;
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        foreach ( KeyValuePair<string, Dictionary<string, string>> kvp in s.Tpls )
                        {
                            if ( kvp.Key == selectedFieldValue )
                            {
                                SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                                xml[selectedField].Text = selectedFieldValue;
                                if ( xml[selectedField].Attributes.Count > 0 )
                                {
                                        foreach ( KeyValuePair<string, string> ss in kvp.Value )
                                        {
                                            xml[selectedField].Attributes[ss.Key] = kvp.Value[ss.Key];
                                        }
                                }
                                doc.documentText = SimpleXML.SaveXml(xml);

                            }
                        }

                    }
                    */
                    // изменение тарифного плана (костя) 08.10.2015 конец
                    else if ( !"JDocDate".Equals(selectedField) )
                    {
                        SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                        xml[selectedField].Text = selectedFieldValue;
                        doc.documentText = SimpleXML.SaveXml(xml);
                    }
                    else
                    {
                        doc.documentDate = selectedFieldValue;
                    }
                    docChanged = true;
                }
            }
            catch ( Exception )
            {
            }

            return false;
        }
    }
}
