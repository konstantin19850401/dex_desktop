using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using DEXExtendLib;

namespace DEXPlugin.Report.Mega.Activation
{
    public class ReportP : IDEXPluginReport
    {
        #region IDEXPluginInfo
        public string ID
        {
            get
            {
                return "DEXPlugin.Report.Common.Activation";
            }
        }
        public string Title
        {
            get
            {
                return "Сверка по активации";
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
                return "Создаёт отчёт для сверки с отделениями по количеству активации.";
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
            ActivationMain activationMain = new ActivationMain(toolbox);
            activationMain.ShowDialog();
            activationMain.SaveForm();
            return true;
        }

    }
}
