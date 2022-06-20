using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace DEXOL_NEW
{
    class Handler
    {
        Comet comet;
        Toolbox tb;
        Dictionary<string, Form> winds = new Dictionary<string, Form>();
        public Handler()
        {
            tb = Toolbox.getToolBox();
            comet = tb.comet;
        }
        public Dictionary<string, Form>  getWinds { get { return winds; } }
        public void AddWind(string key, Form form)
        {
            winds.Add(key, form);
        }
        public void DeleteWindByHash(string hash) {
            if (winds.ContainsKey(hash))
            {
                winds.Remove(hash);
            }
        }
        public Form GetFormById(string hash)
        {
            Form form = null;
            if (winds.ContainsKey(hash))
            {
                form = winds[hash];
            }
            return form;
        }
        public async void SelectApp(string appId)
        {
            JObject packet = new JObject();
            packet["com"] = "skyline.core.apps";
            packet["subcom"] = "select";
            packet["data"] = new JObject();
            packet["data"]["appid"] = appId;
            await comet.Get(packet);
        }
       

        #region Команды для сервера
        public async void LaunchBase(string baseId)
        {
            JObject packet = new JObject();
            packet["com"] = "skyline.apps.adapters";
            packet["subcom"] = "appApi";
            packet["data"] = new JObject();
            packet["data"]["action"] = "list";
            packet["data"]["subaction"] = "period";
            packet["data"]["base"] = baseId;
            await comet.Get(packet);
        }
        public async void OpenDict(string dictId)
        {
            JObject packet = new JObject();
            packet["com"] = "skyline.apps."+tb.AppId;
            packet["subcom"] = "appApi";
            packet["data"] = new JObject();
            packet["data"]["action"] = "getDictsRecordsV1";
            packet["data"]["dict"] = dictId;
            await comet.Get(packet);
        }
        public async Task GetStartData()
        {
            JObject jo = new JObject();
            jo["com"] = "skyline.apps." + tb.AppId;
            jo["subcom"] = "appApi";
            jo["data"] = new JObject();
            jo["data"]["action"] = "startingLocationApp";
            await comet.Get(jo);
        }
        public async Task GetAppDicts()
        {
            JObject jo = new JObject();
            jo["com"] = "skyline.apps." + tb.AppId;
            jo["subcom"] = "appApi";
            jo["data"] = new JObject();
            jo["data"]["action"] = "getNewAllDictsV1";
            comet.Get(jo);
        }
       
        #endregion


        public async Task ProcessDataFromServer(JObject packet)
        {
            //Console.WriteLine("Пакет с сервера " + packet.ToString());
            switch (packet["com"].ToString())
            {
                case "skyline.core.apps":
                    switch (packet["subcom"].ToString())
                    {
                        case "select":
                            if ((int)packet["data"]["status"] == 200)
                            {
                                tb.AppId = packet["data"]["appid"].ToString();
                                // запросим справочники
                                GetAppDicts();
                            }
                            break;
                    }
                    break;
                case "skyline.apps.adapters":
                    switch (packet["subcom"].ToString())
                    {
                        case "appApi":
                            switch (packet["data"]["action"].ToString())
                            {
                                case "startingLocationApp":
                                    Console.WriteLine("Пакет с сервера " + packet.ToString());
                                    // надо бы запросить справочники теперь
                                    if ((int)packet["data"]["status"] == 200)
                                    {
                                        if (packet["data"]["bases"] != null)
                                        {
                                            Dictionary<string, JArray> dict = new Dictionary<string, JArray>();
                                            dict.Add("bases", (JArray)packet["data"]["bases"]);
                                            Main.mainForm.InitMenu(dict);
                                        }
                                    }
                                    break;
                                case "getNewAllDictsV1":
                                    if ((int)packet["data"]["status"] == 200)
                                    {
                                        Task.Factory.StartNew(() =>
                                        {
                                            Console.WriteLine("Пакет с сервера " + packet.ToString());
                                            Dictionary<string, JArray> dict = new Dictionary<string, JArray>();
                                            dict.Add("dicts", (JArray)packet["data"]["list"]);

                                            foreach (JObject jo in packet["data"]["list"])
                                            {
                                                tb.AddDict(jo["name"].ToString(), (JArray)jo["list"]);
                                            }



                                            Main.mainForm.InitMenu(dict);
                                            GetStartData();
                                        });
                                        
                                        // теперь получим стартовые значения для локации

                                    }
                                    break;
                                case "getDictsRecordsV1":
                                    Console.WriteLine("Пакет с сервера " + packet.ToString());
                                    Task.Factory.StartNew(() => {
                                        DictForm df = new DictForm((JObject)packet["data"]);
                                        df.ShowDialog();
                                        Main.mainForm.AddOwnedForm(df);
                                    });
                                    break;
                                case "getDictSingleId":
                                    Console.WriteLine("Пакет с сервера " + packet.ToString());
                                    //if ((int)packet["data"]["status"] == 200)
                                    //{

                                    Task.Factory.StartNew(() =>
                                    {
                                        if (winds[packet["hash"].ToString()] != null)
                                        {
                                            DictForm form = (DictForm)winds[packet["hash"].ToString()];
                                            form.Cmd((JObject)packet["data"]);
                                        }
                                    });


                                        //}

                                        //DictItem di = new DictItem();
                                        //di.Show();
                                        //Task.Factory.StartNew(() => {
                                        //    DictItem di = new DictItem();
                                        //    di.ShowDialog();
                                        //    Main.mainForm.AddOwnedForm(di);
                                        //});
                                break;
                                case "createNewRecordInDictV1":
                                    Console.WriteLine("Пришел пакет createNewRecordInDictV1 " + packet.ToString());
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
    }
}
