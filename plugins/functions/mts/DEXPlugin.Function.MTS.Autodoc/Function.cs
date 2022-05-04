using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;

namespace DEXPlugin.Function.MTS.Autodoc
{
    public class Function : IDEXPluginFunction
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Function.MTS.Autodoc";
            }
        }
        public string Title
        {
            get
            {
                return "Формирование группы документов";
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
                return "Формирует новые документы с указанными MSISDN и ICC";
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
            AutodocMain main = new AutodocMain(toolbox);
            main.ShowDialog();
            main.SaveParams();
            return true;
        }
    }
}
