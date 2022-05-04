using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;
using System.Data;

namespace DEXPlugin.Journalhook.Mega.EFD.SenderProfile
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Mega.EFD.SenderProfile";
            }
        }
        public string Title
        {
            get
            {
                return "Профиль экспорта ЕФД Мегафон";
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
                return "Устанавливает профиль экспорта выделенному документу ЕФД";
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
        Dictionary<string, string> profsubmenu;
        Dictionary<string, string> profdata;
        string editprofiletag;
        object toolbox;

        public void InitReflist(object toolbox)
        {
            this.toolbox = toolbox;
            hookVisible = false;
            profsubmenu = null;
            profdata = null;
            bool needSeparator = false;
            int scnt = 0;

            profsubmenu = new Dictionary<string, string>();
            profdata = new Dictionary<string, string>();

            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery("select * from `efd_profiles` order by pname");
            if (t != null && t.Rows.Count > 0)
            {
                foreach (DataRow r in t.Rows)
                {
                    string pkey = "profilechange" + scnt.ToString();
                    profsubmenu.Add(pkey, r["pname"].ToString());
                    profdata.Add(pkey, r["pcode"].ToString());
                    scnt++;
                }
                needSeparator = scnt > 0;
            }
            if (needSeparator)
            {
                profsubmenu.Add("profilechange" + scnt++.ToString(), "-");
            }
            editprofiletag = "profilechange" + scnt.ToString();
            profsubmenu.Add(editprofiletag, "Редактировать профили");
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            if (DocType.Equals("DEXPlugin.Document.Mega.EFD.Fiz", StringComparison.InvariantCultureIgnoreCase) &&
                journalType == DEXJournalType.JOURNAL &&
                (DocStatus == 0 || DocStatus == 1 || DocStatus == 2 || DocStatus == 3 || DocStatus == 5)) hookVisible = true;

            if (profsubmenu == null || profdata == null) hookVisible = false;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (hookVisible) ret["changeprofile"] = "Задать профиль отправки";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            if (!FunctionName.Equals("changeprofile")) return null;
            return profsubmenu;
        }

        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            try
            {
                if (FunctionName.Equals("changeprofile") && docId.Equals("DEXPlugin.Document.Mega.EFD.Fiz") &&
                    journalType == DEXJournalType.JOURNAL &&
                    (doc.documentStatus == 0 || doc.documentStatus == 1 || doc.documentStatus == 2 || doc.documentStatus == 3 ||
                    doc.documentStatus == 5))
                {
                    if (editprofiletag != null && editprofiletag.Equals(SubFunctionName))
                    {
                        // Редактирование списка профилей
                        ProfileEditForm pef = new ProfileEditForm(toolbox);
                        pef.ShowDialog();
                        InitReflist(toolbox);
                    }
                    else
                    {
                        if (SubFunctionName != null && profdata != null && profdata.ContainsKey(SubFunctionName))
                        {
                            // Добавление в документ профиля
                            SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                            xml.GetNodeByPath("ProfileName", true).Text = profsubmenu[SubFunctionName];
                            xml.GetNodeByPath("ProfileCode", true).Text = profdata[SubFunctionName];
                            doc.documentText = SimpleXML.SaveXml(xml);

                            docChanged = true;
                        }
                    }
                }
            }
            catch (Exception) { }

            return false;
        }

    }
}
