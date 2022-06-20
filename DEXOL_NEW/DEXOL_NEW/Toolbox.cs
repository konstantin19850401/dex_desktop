using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXOL_NEW
{
    class Toolbox : IDEXUserData
    {
        private static Toolbox pToolBox = null;
        private string appId = null;
        public Toolbox() { }


        public static Toolbox getToolBox()
        {
            if (pToolBox == null)
            {
                pToolBox = new Toolbox();
            }
            return pToolBox;
        }
        #region Данные приложения
        private string appName = "DEXOL";
        private string appVer = "0.0.1";
        public string AppName { get { return appName; } }
        public string AppVer { get { return appVer; } }
        public string AppId { get { return appId; } set { appId = value; } }

        #endregion
        #region Comet
        public Comet comet { get; set; }
        public static void initComet(Comet comet)
        {
            if (comet == null)
            {
                Toolbox tb = Toolbox.getToolBox();
                tb.comet = comet;
            }
        }
        #endregion
        #region Handler
        public Handler handler { get; set; }
        public void initHandler(Handler handler)
        {
            if (handler == null)
            {
                Toolbox tb = Toolbox.getToolBox();
                tb.handler = handler;
            }
        }
        #endregion
        #region UserData
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Userpic { get; set; }
        Dictionary<string, JObject> appsData = new Dictionary<string, JObject>();
        public Dictionary<string, JObject> AppsData { get { return appsData; } }
        public void AddApp(JObject app)
        {
            Dictionary<string, JObject> s = appsData;

            if (!appsData.ContainsKey(app["name"].ToString()))
            {
                appsData.Add(app["name"].ToString(), app);
            }
        }
        #endregion
        #region Справочники
        Dictionary<string, JArray> dicts = new Dictionary<string, JArray>();
        public Dictionary<string, JArray> Dictionaries()
        {
            return dicts;
        }
        public void AddDict(string dictname, JArray items)
        {
            if (!dicts.ContainsKey(dictname))
            {
                dicts.Add(dictname, items);
            }
        }

        #endregion
        #region Методы
        // генерация случайной строки длиной 16 символов из букв лат. алфавита
        public string GenerateHash()
        {
            int len = 16;
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            char[] chars = new char[len];

            for (int i = 0; i < len; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
            #endregion
        }
    }
}
