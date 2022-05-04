using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.MTS.UnitsDP
{
    public class Dictionary : IDEXPluginDictionary
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Dictionary.MTS.UnitsDP";
            }
        }
        public string Title
        {
            get
            {
                return "Справочник точек продаж";
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
                return "Справочник точек продаж МТС, где создаётся привязка к отделению и указывается профиль точки";
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
                "CREATE TABLE IF NOT EXISTS `mts_units_dp` ( " +
                "`id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT, " +
                "`uid` mediumint(8) NOT NULL, " +
                "`dpcode` varchar(32) NOT NULL, " +
                "`kind` smallint(1) NOT NULL, " +
                "PRIMARY KEY (`id`), " +
                "UNIQUE KEY `id` (`id`), " +
                "KEY `uid` (`uid`,`dpcode`) " +
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
