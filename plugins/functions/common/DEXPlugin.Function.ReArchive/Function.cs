using System;
using System.Collections.Generic;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;

namespace DEXPlugin.Function.ReArchive
{
    public class Function : IDEXPluginFunction
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Function.ReArchive";
            }
        }
        public string Title
        {
            get
            {
                return "Перенесение документов в случае ошибочного архивирования";
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
                return "Перенос документов из архива в журнал";
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
            ReArchiveMain main = new ReArchiveMain(toolbox);
            main.ShowDialog();
            main.SaveParams();
            return true;
        }
    
    }
}
