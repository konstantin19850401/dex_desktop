using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Test
{
    public class Dictionary : IDEXPluginDictionary, IDEXPluginSetup
    {
        public string ID 
        {
            get 
            {
                return "DEXPlugin.Dictionary.Test"; 
            }
        }
        public string Title 
        {
            get
            {
                return "Тестовый справочник";
            }
        }

        public string[] Path 
        {
            get
            {
                string[] ret = { "Тестовый раздел", "Тестовый подраздел" };
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
                return "Это тестовый справочник, который не делает абсолютно ничего.";
            }
        }

        public void Startup(Object toolbox)
        {
            //TODO
        }

        public void ShowDictionary(Object toolbox)
        {
            IDEXUserData udata = (IDEXUserData)toolbox;
            MessageBox.Show(string.Format("ShowDictionary\nLogin: {0}\nPass: {1}", udata.Login, udata.Password));

            IDEXTrigger tr = (IDEXTrigger)toolbox;
            tr.setTrigger("test", "ShowDictionary");
        }

        public void Setup(Object toolbox)
        {
            IDEXUserData udata = (IDEXUserData)toolbox;
            MessageBox.Show(string.Format("Setup Dictionary\nLogin: {0}\nPass: {1}", udata.Login, udata.Password));

            IDEXTrigger tr = (IDEXTrigger)toolbox;
            tr.setTrigger("test", "Setup Dictionary");
        }
    }
}
