using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Security;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System.Globalization;
using System.Threading;

namespace DEXExtendLib
{
    public class CCMSession
    {
        //const string SBMS_URL = "https://sbms.megafonkavkaz.ru:9443/CLIR_API/";
        //public string oapi_URL = "";//@"https://kvk-dlr-sbms02.megafon.ru:9443/CLIR_API/";
        string[] param_delim = new string[2] { "=", "&" };


        string sessionId = null;
        public string pslpt_id = null;

        public string lastErrorMessage = null;
        public int lastErrorCode = -1;

        public string lastRequest = "";

        

        

        public SimpleXML httpRequest(string command, params string[] args)
        {
            try
            {
                string url = oapi_URL + command + "?";
                //string url = SBMS_URL + "?";

                if (sessionId != null)
                {
                    url += @"SESSION_ID=" + sessionId + @"&";
                }

                Encoding e1251 = Encoding.GetEncoding("windows-1251");
                int dlm = 0;
                if (args != null && args.Length > 0)
                {
                    string sparam = "";
                    foreach (string arg in args)
                    {
                        if (dlm == 0) sparam = arg + "=";
                        else if (dlm == 1 && arg.Trim().Length > 0)
                        {
                            url += sparam + HttpUtility.UrlEncode(arg, e1251) + @"&";
                            //                            url += sparam + arg + @"&";
                        }
                        dlm = 1 - dlm;
                    }
                }



                lastRequest = url;


                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();

                //url = @"https://alldealers.megafon.ru:9443/ps/auth/api/token";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


                
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "application/json";

                var postData = "grant_type=password";
                postData += "&username=FR10_SALPAG_ADMIN_2016";
                postData += "&password=FR10_SALPAG_ADMIN_2016_345";
                var data = Encoding.UTF8.GetBytes(postData);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }

                X509Certificate2 cert = GetCertificateFromStore(oapiSessionCert);
                if (cert == null)
                {
                    Console.WriteLine(oapiSessionCert);
                    Console.ReadLine();
                }         
                request.ClientCertificates.Add(cert);

                // под этим юзером проходим авторизацию на сервере(Тип авторизации Basic)
                string auth = string.Format("{0}:{1}", "ps_sep", "1111");
                string enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));
                string cred = string.Format("{0} {1}", "Basic", enc);
                request.Headers[HttpRequestHeader.Authorization] = cred;
                


                // снять комментарий
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                oapiSessionStatus = false;


                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); 
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                Stream st = response.GetResponseStream();
                
                string ts = null;
                int count = 0;

                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        ts = Encoding.GetEncoding(1251).GetString(buf, 0, count);
                        sb.Append(ts);
                    }
                } while (count > 0);
                string str = sb.ToString();

                SimpleXML xml = SimpleXML.LoadXml(str);

                try
                {
                    SimpleXML xmlError = xml.GetNodeByPath("ERROR", false);
                    lastErrorCode = Convert.ToInt32(xmlError["ERROR_ID"].Text);
                    lastErrorMessage = xmlError["ERROR_MESSAGE"].Text;
                }
                catch (Exception)
                {
                    try
                    {
                        SimpleXML xmlError = xml.GetNodeByPath(command, false).GetNodeByPath("ERROR", false);
                        lastErrorCode = Convert.ToInt32(xmlError["ERROR_ID"].Text);
                        lastErrorMessage = xmlError["ERROR_MESSAGE"].Text;
                    }
                    catch (Exception)
                    {
                        lastErrorCode = -1;
                        lastErrorMessage = null;
                    }
                }

                sessionId = null;

                SimpleXML xmlSessionID = xml.GetNodeByPath("SESSION_ID", false);
                if (xmlSessionID != null) sessionId = xmlSessionID.Text;

                return xml;
            }
            catch (Exception){ }

            return null;
        }

        public bool connect(string login, string password, string sbmsCert, string sbmsServer)
        {
            try
            {
                oapiSessionCert = sbmsCert;

                oapi_URL = @"https://" + sbmsServer + ":9443/CLIR_API/";
                //SBMS_URL = @"https://" + sbmsServer + ":9443/ps/auth/api/token";

                //string ln = "FR10_SALPAG_ADMIN_2016";
                //string pswd = "FR10_SALPAG_ADMIN_2016_345"; 

                oapiSessionStatus = true;
                string channel = "WWW";
                SimpleXML xml = httpRequest("CLIR_API_LOGIN", "LOGIN", login, "PASSWORD", password, "CHANNEL", channel);
                //SimpleXML xml = httpRequest("CLIR_API_LOGIN", "grant_type", "password", "username", ln, "password", pswd);
                SimpleXML xmlSessionID = xml.GetNodeByPath("SESSION_ID", false);
                pslpt_id = xml["HAS_GET_USER_ATTRIBUTES"]["P_SLPT_ID"].Text;

                return (sessionId != null);
            }
            catch (Exception) { }

            sessionId = null;
            return false;
        }

        public Dictionary<string, string> xml2dic(SimpleXML xml)
        {
            if (xml == null) return null;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (SimpleXML node in xml.Child)
            {
                try
                {
                    if (node != null && "ROW".Equals(node.Name))
                    {
                        SimpleXML nid = node.GetNodeByPath("ID", false), ndef = node.GetNodeByPath("DEF", false);
                        if (nid != null && ndef != null) ret[nid.Text] = ndef.Text;
                    }
                }
                catch (Exception) { }
            }

            return ret;
        }

        public DataTable xml2table(SimpleXML xml, string prefix)
        {
            if (xml == null) return null;
            if (prefix == null) prefix = "";

            DataTable ret = new DataTable();
            ret.Columns.Add("id", typeof(string));
            ret.Columns.Add("title", typeof(string));
            foreach (SimpleXML node in xml.Child)
            {
                try
                {
                    if (node != null && "ROW".Equals(node.Name))
                    {
                        SimpleXML nid = node.GetNodeByPath(prefix + "ID", false), ndef = node.GetNodeByPath(prefix + "DEF", false);
                        if (nid != null && ndef != null) ret.Rows.Add(nid.Text, ndef.Text);
                    }
                }
                catch (Exception) { }
            }

            return ret;
        }

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // Always accept
                return true;
                
            }
        }












        // новое апи. OAPI
        public static string oapiSessionCert = "";
        public static bool oapiSessionStatus = false;
        public static Dictionary<string, string> token = new Dictionary<string,string>();
        public string oapi_URL = "";

        public static Dictionary<string, string[]> oapiConnectInfo = new Dictionary<string, string[]>();
        
        public static Dictionary<string, JObject> oapiConnections = new Dictionary<string, JObject>();

        public bool oapiConnect(string login, string password, string cert, string server)
        {
            try
            {
                JArray ja = new JArray();
                int s = 0;

                oapiSessionCert = cert;
                oapi_URL = @"https://" + server + ":9443/";
                //string url = oapi_URL + "ps/auth/api/token";
                oapiSessionStatus = true;

                //заголовки
                Dictionary<string,string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/x-www-form-urlencoded");
                headers.Add("Accept", "application/json");
                headers.Add("Authorization", "Basic cHNfc2VwOjExMTE=");

                //тело запроса
                Dictionary<string, string> body = new Dictionary<string, string>();
                body.Add("grant_type", "password");
                body.Add("username", login);
                body.Add("password", password);

                //token["access_token"] = null;

                // проверим, есть ли токен, а если есть, не истек ли он.
                if (oapiConnections.ContainsKey(login))
                {
                    DateTime now = DateTime.Now;
                    DateTime exp = DateTime.Parse(oapiConnections[login]["date_expiration"].ToString());
                    TimeSpan dif = exp - now;
                    if (dif.TotalMinutes < 5) // если жизни у токена не более 5 мин, будем запрашивать
                    {
                        s = -2;
                    }
                } 
                else 
                {
                    s = -1;
                }

                // если нужно получить токен
                if (s < 0)
                {
                    ja = oapiHttpRequest("ps/auth/api/token", "POST", false, headers, body);
                    JObject jo = new JObject();
                    jo["access_token"] = ja[0]["access_token"].ToString();
                    jo["date_creation"] = ja[0]["date_creation"].ToString();
                    jo["date_expiration"] = ja[0]["date_expiration"].ToString();

                    if (s == -2) oapiConnections[login] = jo;
                    else oapiConnections.Add(login, jo);
                    Thread.Sleep(10000);
                    
                }
                return true;
            }
            catch (Exception) { }

            sessionId = null;
            return false;
        }

        public X509Certificate getCertificate(string cert)
        {
            //string thumbprint = @"383CB5B7815A659E0EFD5D504EA1FAA62ADE54A6"; //кчр
            //string thumbprint = @"2967c3964622715dc227601ff056ce4554e2fc19"; //062013
            string thumbprint = cert;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                return col[0];

            }
            catch (Exception) { }

            return null;
        }

        public JArray oapiHttpRequest(string command, string method, bool appjson, Dictionary<string, string> headers, Dictionary<string, string> body)
        {
            try
            {
                string url = oapi_URL + command + "?";
                Encoding utf8 = Encoding.GetEncoding("UTF-8");

                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                string dataParams = "";
               
                foreach (KeyValuePair<string, string> kvp in body)
                {
                    dataParams += kvp.Key + "=" + kvp.Value + "&";
                }
                if (method == "GET") url += dataParams;
               
                if (appjson) url = url.Substring(0, url.Length - 1);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                foreach (KeyValuePair<string, string> kvp in headers)
                {
                    if (kvp.Key == "Content-Type") request.ContentType = kvp.Value;
                    else if (kvp.Key == "Accept") request.Accept = kvp.Value;
                    else request.Headers.Add(kvp.Key, kvp.Value);
                }
                if (method == "POST")
                {
                    byte[] data = null;
                    if (appjson) data = Encoding.UTF8.GetBytes(body["json"]);
                    else data = Encoding.UTF8.GetBytes(dataParams);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                    }
                }
                //X509Certificate2 cert = GetCertificateFromStore(oapiSessionCert);
                X509Certificate cert = getCertificate(oapiSessionCert);
                if (cert == null)
                {
                    Console.WriteLine(oapiSessionCert);
                    Console.ReadLine();
                }
                request.ClientCertificates.Add(cert);
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
               
                    oapiSessionStatus = false;
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
                    string str = sb.ToString();


                    //JObject json;
                    JArray outjson;
                   
                    try
                    {
                        outjson = JArray.Parse(str);
                    }
                    catch (Exception)
                    {
                        //JObject sss = "["+JObject.Parse("["+str+"]");
                        outjson = JArray.Parse("[" + str + "]");
                    }
                    /*
                    try
                    {
                        //json = JObject.Parse(str);
                        outjson = JArray.Parse(str);

                    }
                    catch (Exception)
                    {
                      
                        JArray s = JArray.Parse(str);
                        if (s.Count > 1)
                        {

                        }
                        else
                        {
                            json = (JObject)s[0];
                        }
                        
                    }
                    */
                    //JArray ddd = JArray.Parse(str);
                    //int dd = ddd.Count;

                    return outjson;
                    }
                catch (Exception) 
                {
                    
                }
            }
            catch(Exception) {}
            return null;
        }






































        private static X509Certificate2 GetCertificateFromStore(string certName)
        {

            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }

        }
        /*
        public X509Certificate getCertificate(string cert)
        {
            //string thumbprint = @"383CB5B7815A659E0EFD5D504EA1FAA62ADE54A6"; //кчр
            //string thumbprint = @"2967c3964622715dc227601ff056ce4554e2fc19"; //062013
            //string thumbprint = "ООО Н-Телеком (KVK)";
            


            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint, cert, false);
                Console.WriteLine(col);
                return col[0];

            }
            catch (Exception) { }

            return null;
        }
        */
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
       
    }
   
}
