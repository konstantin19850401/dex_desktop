using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Windows.Forms;

namespace DEXPlugin.Schedule.ExportMega
{
    public class ScheduleF : IDEXPluginSchedule, IDEXPluginSetup
    {

        #region IDEXPluginInfo

        public string ID
        {
            get
            {
                return "DEXPlugin.Schedule.ExportMega";
            }
        }
        public string Title
        {
            get
            {
                return "Экспорт Единых абонентских договоров в DS";
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
                return "Экспорт Единых абонентских договоров в DS";
            }
        }

        public Bitmap getBitmap()
        {
            return null;
        }
        #endregion

        public void Setup(Object toolbox)
        {
            MessageBox.Show(ID + " Setup");
        }

        public void Schedule(Object toolbox)
        {
/*
            MegaExport mexport = new MegaExport(toolbox);
            mexport.Execute();
            mexport = null;
 */
            ((IDEXScheduler)toolbox).AddSchedule(this, 5);
        }

        public void Idle(Object toolbox)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + ID + " Idle");
            ((IDEXScheduler)toolbox).AddSchedule(this, 1);
        }
    }
}
