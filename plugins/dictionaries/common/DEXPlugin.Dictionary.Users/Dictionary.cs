using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Users
{
    public class Dictionary : IDEXPluginDictionary
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Dictionary.Users";
            }
        }
        public string Title
        {
            get
            {
                return "Справочник пользователей";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = null;
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
                return "Справочник пользователей системы";
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
            //TODO
            IDEXRights r = (IDEXRights)toolbox;
            r.AddRightsItem(ID + ".access", "Доступ к справочнику пользователей");
        }

        public void ShowDictionary(Object toolbox)
        {
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".access") || r.IsSuperUser())
            {

                FUsersMain main = new FUsersMain();
                main.toolbox = toolbox;
                main.InitForm();
                main.ShowDialog();
                main.toolbox = null;
                main = null;
                GC.Collect();
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права доступа к справочнику пользователей");
            }
        }
    }


}
