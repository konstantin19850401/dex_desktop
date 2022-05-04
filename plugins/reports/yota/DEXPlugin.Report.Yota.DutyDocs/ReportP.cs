using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;

namespace DEXPlugin.Report.Yota.DutyDocs
{
    public class ReportP : IDEXPluginReport
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Report.Yota.DutyDocs";
            }
        }
        public string Title
        {
            get
            {
                return "Отчёт по долгам";
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
                return "Формирует список документов, созданных с помощью функции формирования группы документов";
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

        public bool Report(Object toolbox)
        {
            DutyDocsMain main = new DutyDocsMain(toolbox);
            main.ShowDialog();
            main.SaveForm();

            return true;
        }
    }
}
