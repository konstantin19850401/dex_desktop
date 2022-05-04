using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.MTS.AllDP
{
    public class Dictionary : IDEXPluginDictionary
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Dictionary.MTS.AllDP";
            }
        }
        public string Title
        {
            get
            {
                return "Справочник всех точек продаж";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = null;
                return ret;
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
                return "Справочник всех точек продаж МТС";
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

        public void Startup(Object toolbox)
        {
            IDEXRights r = (IDEXRights)toolbox;
            r.AddRightsItem(ID + ".access", "Доступ к справочнику точек продаж МТС");

            ((IDEXData)toolbox).runQuery(
                "CREATE TABLE IF NOT EXISTS `mts_dp_all` ( " +
                "`id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT, " +
                "`dpcode` varchar(32) NOT NULL, " +
                "`kind` smallint(1) NOT NULL, " +
                "PRIMARY KEY (`id`) " +
                ") ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;"
                );

        }

        public void ShowDictionary(Object toolbox)
        {
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".access") || r.IsSuperUser())
            {
                FUnitsDPMain main = new FUnitsDPMain(toolbox);
                main.ShowDialog();
                main.toolbox = null;
                main = null;
                GC.Collect();
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права доступа к справочнику точек продаж МТС");
            }
        }

    }
}
