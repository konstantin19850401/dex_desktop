// Оригинал: P:\NDocs\plugins-2008\rpi\common\common_int_docdates\

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;
using System.Data;

namespace DEXPlugin.Report.Common.DocDates
{
    public class ReportP : IDEXPluginReport
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Report.Common.DocDates";
            }
        }
        public string Title
        {
            get
            {
                return "Дата документа и отгрузки SIM-карты";
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
                return "Формирует список документов с датой отгрузки SIM-карты и датой документа для списка MSISDN из буфера обмена.";
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
            DocDatesMain main = new DocDatesMain(toolbox);
            main.ShowDialog();
            main.SaveForm();

            return true;
        }

    }
}
