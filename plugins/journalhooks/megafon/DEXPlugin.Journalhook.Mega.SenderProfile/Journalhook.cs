using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;

namespace DEXPlugin.Journalhook.Mega.SenderProfile
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Mega.SenderProfile";
            }
        }
        public string Title
        {
            get
            {
                return "Установка профиля экспорта документу";
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
                return "Устанавливает профиль отправки выделенному документу";
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
        Dictionary<string, ProfileInfo> profdata;
        

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            profsubmenu = null;
            profdata = null;

            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery("select * from `sendprofiles` order by pname");
            if (t != null && t.Rows.Count > 0)
            {
                profsubmenu = new Dictionary<string, string>();
                profdata = new Dictionary<string, ProfileInfo>();
                int scnt = 0;
                foreach (DataRow r in t.Rows)
                {
                    string pkey = "profilechange" + scnt.ToString();
                    profsubmenu[pkey] = r["pname"].ToString();
                    profdata[pkey] = new ProfileInfo(r["plogin"].ToString(), r["ppassword"].ToString(), r["plink"].ToString(), bool.Parse(r["psubscribers"].ToString()));
                    scnt++;
                }
            }
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            if (DocType.Equals("DEXPlugin.Document.Mega.EAD.Fiz", StringComparison.InvariantCultureIgnoreCase) &&
                journalType == DEXJournalType.JOURNAL &&
                (DocStatus == 0 || DocStatus == 1 || DocStatus == 2 || DocStatus == 3 || DocStatus == 5)) hookVisible = true;
            
            if (profsubmenu == null || profdata == null) hookVisible = false;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string,string>();
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
                if (FunctionName.Equals("changeprofile") && docId.Equals("DEXPlugin.Document.Mega.EAD.Fiz") &&
                    journalType == DEXJournalType.JOURNAL &&
                    (doc.documentStatus == 0 || doc.documentStatus == 1 || doc.documentStatus == 2 || doc.documentStatus == 3 ||
                    doc.documentStatus == 5))
                {
                    if (SubFunctionName != null && profdata != null && profdata.ContainsKey(SubFunctionName))
                    {
                        // Добавление в документ профиля
                        ProfileInfo pfi = profdata[SubFunctionName];
                        SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                        xml.GetNodeByPath("ProfileName", true).Text = profsubmenu[SubFunctionName];
                        xml.GetNodeByPath("ProfileLogin", true).Text = pfi.login;
                        xml.GetNodeByPath("ProfilePassword", true).Text = pfi.password;
                        xml.GetNodeByPath("ProfileLink", true).Text = pfi.link;
                        xml.GetNodeByPath("ProfileSubscribers", true).Text = pfi.subscribers ? "1" : "0";
                        doc.documentText = SimpleXML.SaveXml(xml);

                        docChanged = true;
                    }
                }
            }
            catch (Exception) { }

            return false;
        }

    }

    class ProfileInfo
    {
        public string login, password, link;
        public bool subscribers;
        public ProfileInfo(string alogin, string apassword, string alink, bool asubscribers)
        {
            login = alogin;
            password = apassword;
            link = alink;
            subscribers = asubscribers;
        }
    }
}
