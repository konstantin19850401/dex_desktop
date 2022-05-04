using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using DEXExtendLib;

namespace DEXPlugin.Report.Yota.Sverka
{
    public class ReportP : IDEXPluginReport
    {
        #region IDEXPluginInfo
        public string ID
        {
            get
            {
                return "DEXPlugin.Report.Common.Sverka";
            }
        }
        public string Title
        {
            get
            {
                return "Сверка по ТП и документам";
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
                return "Создаёт отчёт для сверки с отделениями по количеству ТП подключённых документов.";
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

        #endregion

        public bool Report(Object toolbox)
        {
/*
            PeriodReestrMain main = new PeriodReestrMain();
            main.InitForm(toolbox);
            main.ShowDialog();
            main.SaveForm();
*/

            SverkaMain sverkaMain = new SverkaMain(toolbox);
            sverkaMain.ShowDialog();
            sverkaMain.SaveForm();
            return true;
        }

    }
}
