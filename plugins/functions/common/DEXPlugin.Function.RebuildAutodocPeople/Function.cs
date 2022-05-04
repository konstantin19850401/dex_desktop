using System;
using System.Collections.Generic;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;

namespace DEXPlugin.Function.RebuildAutodocPeople
{
    public class Function : IDEXPluginFunction
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Function.RebuildAutodocPeople";
            }
        }
        public string Title
        {
            get
            {
                return "Построение базы автодока";
            }
        }

        public string[] Path
        {
            get
            {
//                string[] ret = { "" };
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
                return "Построение базы абонентских данных для функции формирования группы документов";
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

        public bool Execute(Object toolbox)
        {
            RebuildAutodocPeopleMain main = new RebuildAutodocPeopleMain(toolbox);
            main.ShowDialog();
            main.SaveParams();
            return true;
        }
    }
}
