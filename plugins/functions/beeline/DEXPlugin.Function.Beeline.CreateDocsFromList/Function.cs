using System;
using System.Collections.Generic;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Reflection;

namespace DEXPlugin.Function.Beeline.CreateDocsFromList
{
    public class Function : IDEXPluginFunction
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Function.CreateDocsFromList";
            }
        }
        public string Title
        {
            get
            {
                return "Создание списка договоров из файла данных";
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
                return "Создает список договоров из файла данных.";
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
            CreateDocsFromListMain main = new CreateDocsFromListMain(toolbox);
            main.ShowDialog();

            return true;
        }
    }
}
