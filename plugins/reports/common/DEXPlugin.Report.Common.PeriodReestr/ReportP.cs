// Оригинал: P:\NDocs\plugins-2007\rpi\megafon\mega_int_dreestr_period\

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

namespace DEXPlugin.Report.Common.PeriodReestr
{
    public class ReportP : IDEXPluginReport
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Report.Common.PeriodReestr";
            }
        }
        public string Title
        {
            get
            {
                return "Периодичный реестр договоров";
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
                return "Формирует реестр документов за требуемый период";
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

        private DataTable fake()
        {
            return null;
        }

        public bool Report(Object toolbox)
        {
/*
            Form frm = new Form();
            frm.WindowState = FormWindowState.Maximized;


            ReportViewer rv = new ReportViewer();
            rv.Dock = DockStyle.Fill;
            frm.Controls.Add(rv);

            
            rv.LocalReport.ReportEmbeddedResource = "DEXPlugin.Report.Common.PeriodReestr.PeriodReestr.rdlc";
            rv.ProcessingMode = ProcessingMode.Local;
            rv.SetDisplayMode(DisplayMode.PrintLayout);
            
            

            rv.RefreshReport();

            frm.Show();
*/

            PeriodReestrMain main = new PeriodReestrMain();
            main.InitForm(toolbox);
            main.ShowDialog();
            main.SaveForm();

            return true;
        }
    }
}