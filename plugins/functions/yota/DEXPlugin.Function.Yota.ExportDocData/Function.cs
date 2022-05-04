using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;

namespace DEXPlugin.Function.Yota.ExportDocData
{
    public class Function : IDEXPluginFunction
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Function.Yota.ExportDocData";
            }
        }
        public string Title
        {
            get
            {
                return "Выгрузка полей документов журнала";
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
                return "Выгрузка в CSV полей документов журнала";
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
            ExportDocDataMain main = new ExportDocDataMain();
            main.InitForm(toolbox);
            main.ShowDialog();
            main.SaveParams();
            return true;
        }
    }
}
