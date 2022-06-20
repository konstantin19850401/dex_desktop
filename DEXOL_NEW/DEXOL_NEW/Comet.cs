using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace DEXOL_NEW
{
    class Comet
    {
        String uid;
        String protocol;
        String url;
        int port;
        Toolbox tb;
        public Comet(String protocol, String url, int port)
        {
            this.protocol = protocol;
            this.url = url;
            this.port = port;
            tb = Toolbox.getToolBox();
            tb.comet = this;
            Handler handler = new Handler();
            tb.handler = handler;
        }
        private async Task InitSubscriptionAsync() 
        {
            try
            {
                byte[] buf = new byte[8192];
                StringBuilder sb = new StringBuilder();
                string str = "{}";
                JObject packet = new JObject();
                packet["uid"] = uid;
                string ur = @"http://" + url + ":" + port + "/subscription?packet=" + JsonConvert.SerializeObject(packet);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ur);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream st = response.GetResponseStream();
                    string ts = null;
                    int count = 0;
                    do
                    {
                        count = st.Read(buf, 0, buf.Length);
                        if (count != 0)
                        {
                            ts = Encoding.GetEncoding("UTF-8").GetString(buf, 0, count);
                            sb.Append(ts);
                        }
                    } while (count > 0);
                    str = sb.ToString();

                }
                response.Close();
                JObject jobject = (JObject)JsonConvert.DeserializeObject(str);

                //await InitSubscriptionAsync();
                await tb.handler.ProcessDataFromServer(jobject);
                await Task.Factory.StartNew(() => InitSubscriptionAsync());
                //await InitSubscriptionAsync();

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.Timeout)
                {
                    await InitSubscriptionAsync();
                    //Task.Factory.StartNew(() => InitSubscriptionAsync());
                    //Task.Factory.StartNew(() => InitSubscriptionAsync());
                }
            }
        }
        public async Task Login(String login, String password, LoginForm form)
        {
            JObject packet = new JObject();
            packet["com"] = "skyline.core.auth";
            packet["subcom"] = "initsession";
            packet["data"] = new JObject();
            packet["data"]["login"] = login;
            packet["data"]["password"] = password;
            JObject response = await Request(packet);
            //Console.Write(response);
            if ((int)response["status"] == 401)
            {
                if (response["data"]["err"] != null)
                {
                    String err = "";
                    foreach (string jo in (JArray)response["data"]["err"])
                    {
                        err += jo + "\n";
                    }
                    MessageBox.Show(err);
                }
            }
            else
            {
                if (response["subcom"] != null)
                {
                    if (response["data"]["uid"] != null)
                    {
                        
                        uid = response["data"]["uid"].ToString();
                        // настройки пользователя!!!

                        Toolbox tb = Toolbox.getToolBox();
                        tb.LastName = response["data"]["lastname"].ToString();
                        tb.FirstName = response["data"]["firstname"].ToString();
                        tb.SecondName = response["data"]["secondname"].ToString();
                        tb.Userpic = response["data"]["userpic"].ToString();
                        foreach (JObject app in response["data"]["availableApps"])
                        {
                            tb.AddApp(app);
                        }
                        new Thread(async () => InitSubscriptionAsync()).Start();
                        //Task.Factory.StartNew(() => InitSubscriptionAsync());
                        //await InitSubscriptionAsync();
                        form.DialogResult = DialogResult.OK;
                    }
                } else
                {
                    MessageBox.Show("В пришедшем пакете отстутствует команда subcom");
                }
            }
        }
        private async Task<JObject> Request(JObject packet)
        {
            byte[] buf = new byte[8192];
            StringBuilder sb = new StringBuilder();
            string str = "{}";
            if (uid != null) packet["uid"] = uid;
            string ur = @"http://" + url + ":" + port + "/cmd?packet=" + JsonConvert.SerializeObject(packet);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ur);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream st = response.GetResponseStream();
                string ts = null;
                int count = 0;
                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        ts = Encoding.GetEncoding("UTF-8").GetString(buf, 0, count);
                        sb.Append(ts);
                    }
                } while (count > 0);
                str = sb.ToString();
            }
            response.Close();
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            return jo;
        }
        public async Task<JObject> Get(JObject packet)
        {
            if (packet != null)
            {
                return await Request(packet);
            }
            else return null;
        }
    }
}
