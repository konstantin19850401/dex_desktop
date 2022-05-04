using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using DEXExtendLib;
using System.Threading;
using System.Data;
using System.Collections;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json.Linq;
using DEXSIM;
using System.Management;

using Newtonsoft.Json;
using System.Text.RegularExpressions;

using System.Net;
using System.Web;

namespace dexol
{
    public class DEXToolBox : IDEXPluginSystemData, IDEXUserData, IDEXSysData, IDEXConfig, IDEXData, IDEXCrypt,
                 IDEXServices, /*IDEXTrigger,*/ IDEXRights, IDEXDocumentJournal, IDEXCitySearcher,
                 IDEXPeopleSearcher, /*IDEXScheduler,*/ IDEXPrinter, IDEXValidators, IDEXSim
    {
        public const string SECRETKEY = "klg4tt3g2t3l32t3[t2p1[t32yo2yk";
        public const string CONFIGFILE = "config.xml";
        public const string SUPERUSER_KEY = "SUPERUSER";
        public const string SUPERUSER_TITLE = "Права супер-пользователя";
        public const string RESTRICTED_KEY = "RESTRICTED";
        public const string RESTRICTED_TITLE = "Пользователю доступны только собственные документы";
        public const string DB_DATE_FORMAT = "yyyyMMddhhmmss";
        public const string DB_DATE_FORMAT_MS = DB_DATE_FORMAT + "fff";

        public const int DOCUMENT_NONE = -1;
        public const int DOCUMENT_DRAFT = 0;
        public const int DOCUMENT_UNAPPROVED = 1;
        public const int DOCUMENT_APPROVED = 2;
        public const int DOCUMENT_TOEXPORT = 3;
        public const int DOCUMENT_EXPORTED = 4;
        public const int DOCUMENT_RETURNED = 5;
        public const int DOCUMENT_EXPORTING = 6;
        public const int DOCUMENT_TODELETE = 100;

        public static string[] DOCUMENT_STATE_TEXT = { "Черновик", "На подтверждение", "Подтверждён", "На отправку", "Отправлен", "Возвращён", "Отправляется" };

        #region Получение региона по отделению
        public string getRegionByUid(string uid)
        {
            string ans = "";
            try
            {
                string url = @"http://37.29.115.178:30380/getRegionById.php";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?uid=" + uid);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    ans = stream.ReadToEnd();
                }
                response.Close();
            }
            catch (Exception) { }
            return ans;
        }
        #endregion

        #region проверка данных паспорта. Отправка одного паспорта на проверку
        public bool checkPassport(string series, string number)
        {
            bool ans = false;
            try
            {
                string url = @"http://37.29.115.178:30380/checkPassp.php";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?series=" + series + "&number=" + number);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ans = true;
                }
                response.Close();
            }
            catch (Exception) { }
            return ans;
        }
        #endregion

        #region проверка данных списку на терроризм. Отправка пакетом нескольких данных на проверку
        public string checkForTerrorists(string json)
        {
            string ans = "";
            try
            {
                string url = @"http://37.29.115.178:3020/dexdealer/checkForTerrorists";
                //string url = @"http://192.168.0.159:3020/dexdealer/checkForTerrorists";
                //string url = @"http://192.168.0.158:3020/dexdealer/checkForTerrorists";
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(url + "?packet=" + json);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    ans = stream.ReadToEnd();
                }
                response.Close();
            }
            catch (Exception) { }
            return ans;
        }
        #endregion 

        #region отправка произвольного запроса на произовольный адрес
        public string sendRequest(string method, string url, string port, string body, int reqStatus)
        {
            string str = "{}";
            try
            {
                byte[] buf = new byte[8192];
                StringBuilder sb = new StringBuilder();
                
                string ur = @"http://" + url + ":" + port + body;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ur);
                request.Method = method;
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

                if (reqStatus == 2)
                {
                    str = str.Replace("\"", "").Replace("\\", "\"").Replace("\"r\"", "").Replace("\"n\"", "");
                }
                //str = str.Replace("\"r\"", "").Replace("\"n\"", "");

                //str = Regex.Replace(str, "\"{\\\"com", "");
                //str = Regex.Replace(str, @"\", @"");
            }
            catch (WebException e) 
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        // text is the response body
                        str = reader.ReadToEnd();
                    }
                }
            }
            return str;
        }

        #endregion

        #region проверка данных паспорта. Отправка пакетом нескольких данных на проверку
        public string checkPassportPacket(string json)
        {
            string ans = "";
            try
            {
                string url = @"http://37.29.115.178:30380/checkPassp.php";
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(url + "?packet=true&json=" + json);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    ans = stream.ReadToEnd();
                }
                response.Close();
            }
            catch (Exception) { }
            return ans;
        }
        #endregion

        #region Инициализация и получение экземпляра ToolBox
        public DEXToolBox()
        {
            sAppDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            sDataDir = sAppDir + @"\data\";
            if (!Directory.Exists(sDataDir))
            {
                try
                {
                    Directory.CreateDirectory(sDataDir);
                }
                catch (Exception)
                {
                }
            }
            sOutDir = sAppDir + @"\out\";
            if (!Directory.Exists(sOutDir))
            {
                try
                {
                    Directory.CreateDirectory(sOutDir);
                }
                catch (Exception)
                {
                }
            }

            user_rights = new ArrayList();
            AddRightsItem(SUPERUSER_KEY, SUPERUSER_TITLE);
            AddRightsItem(RESTRICTED_KEY, RESTRICTED_TITLE);

            RegisterService("sim", this);

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    string mac = mo["MACAddress"].ToString();
                    locker_id = StringToMD5(mac);
                    break;
                }
            }

        }

        private string locker_id = null;

        private static DEXToolBox pToolBox = null;

        public static DEXToolBox getToolBox()
        {
            if (pToolBox == null)
            {
                pToolBox = new DEXToolBox();
                pToolBox.FPlugins = PluginFramework.getFramework(pToolBox);
            }
            return pToolBox;
        }
        // Получение экземпляра ToolBox /
        #endregion

        #region Плагины (PluginFramework)
        
        private PluginFramework FPlugins;

        public PluginFramework Plugins
        {
            get
            {
                return FPlugins;
            }
        }

        // IDEXPluginSystemData
        public PluginFramework getPlugins() { return FPlugins; }
        // IDEXPluginSystemData /
        
        #endregion

        #region Соединение с БД (IDEXData) / 06.11.09

        public DataTable getQuery(string Query, params object[] parameters)
        {
            try
            {
                return getQuery(string.Format(Query, parameters));
            }
            catch (Exception) { }
            return null;
        }

        public DataTable getQuery(string Query) //dexol
        {
            return DexolSession.inst().getQuery(Query);
        }

        public int runQuery(string Query)
        {
            return DexolSession.inst().runQuery(Query);
        }

        public int runQuery(string Query, params object[] parameters)
        {
            try
            {
                return runQuery(string.Format(Query, parameters));
            }
            catch (Exception) { }
            return -1;
        }

        public long runQueryReturnLastInsertedId(string Query)
        {
            return DexolSession.inst().runQueryReturnLastInsertedId(Query);
        }

        public long runQueryReturnLastInsertedId(string Query, params object[] parameters)
        {
            try
            {
                return runQueryReturnLastInsertedId(string.Format(Query, parameters));
            }
            catch (Exception) { }
            return -1;
        }


        public DataTable getTable(string TableName)
        {
            if (TableName == null || TableName.Length < 1) return null;
            DexolSession ds = DexolSession.inst();
            string jsontable = DataDir + ds.currentDb + @"\" + TableName + ".json";
            if (File.Exists(jsontable))
            {
                try
                {
                    return ds.jsonToDataTable(File.ReadAllText(jsontable, Encoding.UTF8));
                }
                catch (Exception) { }
            }
            return getQuery("select * from `" + TableName + "`");
        }

        public void setDataReference(string TableName, string Key, bool DoIncrement) //dexol
        {
            DexolSession.inst().setDataReference(TableName, Key, DoIncrement);
        }

        public int getDataReference(string TableName, string Key) //dexol
        {
            return DexolSession.inst().getDataReference(TableName, Key);
        }

        public void clearDataReference(string TableName, string Key) //dexol
        {
            DexolSession.inst().clearDataReference(TableName, Key);
        }

        Dictionary<string, List<string>> localHints = null;

        void checkLocalHints(string hintType)
        {
            if (localHints == null)
            {
                localHints = new Dictionary<string, List<string>>();
            }

            if (!localHints.ContainsKey(hintType))
            {
                List<string> ht = new List<string>();
                localHints.Add(hintType, ht);
                if (File.Exists(sDataDir + hintType + ".hints.json"))
                {
                    try
                    {
                        JArray json = JArray.Parse(File.ReadAllText(sDataDir + hintType + ".hints.json", Encoding.UTF8));
                        for (int i = 0; i < json.Count; ++i)
                        {
                            ht.Add((string)json[i]);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        void saveLocalHints(string hintType)
        {
            if (localHints != null && localHints.ContainsKey(hintType))
            {
                JArray json = new JArray();
                List<string> ht = localHints[hintType];
                foreach (string s in ht)
                {
                    json.Add(s);
                }

                try
                {
                    File.WriteAllText(sDataDir + hintType + ".hints.json", json.ToString(), Encoding.UTF8);
                }
                catch (Exception) { } 
            }
        }

        public void setDataHint(string hintType, string hintValue) //dexol
        {
            if (hintType == null || hintType.Trim().Length < 1) return;
            checkLocalHints(hintType);
            List<string> ht = localHints[hintType];
            
            if (!ht.Contains(hintValue)) {
                ht.Add(hintValue);
                saveLocalHints(hintType);
            }
        }

        public string[] getDataHint(string hintType) //dexol
        {
            if (hintType == null || hintType.Trim().Length < 1) return null;
            checkLocalHints(hintType);

            if (localHints.ContainsKey(hintType))
            {
                return localHints[hintType].ToArray();
            }

            return null;
        }

        public ArrayList checkDocumentCriticals(ArrayList fields, IDEXDocumentData document) //dexol
        {
            return DexolSession.inst().checkDocumentCriticals(fields, document);
        }

        public void setDocumentCriticals(ArrayList fields, IDEXDocumentData document, bool reset) //dexol
        {
            DexolSession.inst().setDocumentCriticals(fields, document, reset);
        }

        string zero = new string((char)0, 1);
        string char13 = new string((char)13, 1);
        string char10 = new string((char)10, 1);
        string chardq = new string('"', 1);

        public string EscapeString(string src) //dexol
        {
            if (src != null)
            {
                return src.Replace("\\", @"\\").Replace(zero, @"\0").Replace(char13, @"\r").Replace(char10, @"\n").Replace(chardq, "\\\"").Replace("'", "\\'");
            }
            return null;
            // The exact characters that are escaped by this function are the null byte (0), 
            // newline (\n), carriage return (\r), backslash (\), single quote ('), 
            // double quote (") and substiture (SUB, or \032).
        }

        #endregion

        #region Пользовательские данные (IDEXUserData) / 08.01.10

        public string sTitle, sUID = "",sCurrentBase = "";
        public string sDexServer = "";
        public SimpleXML sProperties;

        // Это всё нафиг не нужно в клиенте
        public string Login { get { return ""; } }
        public string Password { get { return ""; } }
        public string UID { get { return sUID; } }
        public string MAC { get { return locker_id; } }
        public DateTime DateCreated { get { return new DateTime(0); } }
        public DateTime DateChanged { get { return new DateTime(0); } }
        // Это всё нафиг не нужно в клиенте /

        //а вот это нужно для подключения к серверу адаптеров
        public string adaptersLogin { get; set; }
        public string adaptersPass {get; set;}
        public string dataBase { get; set; }
        public string dexServer { get; set; }

        public string Title { get { return sTitle; } }
        public bool isOnline { get { return true; } } // Для DEXOL всегда true
        public string currentBase { get { return sCurrentBase; } } //Для DEXOL содержит конкретную подключаемую базу. Для DEX поле пусто
        public SimpleXML UserProperties { get { return sProperties; } }

        // User properties file
        //<?xml version="1.0" encoding="Utf-8"?><settings><rights>|SUPERUSER|</rights><Properties><DefaultDocumentState>1</DefaultDocumentState></Properties></settings>
        //TODO С правами разбираться надо

        public void ParseUserData(string data)
        {
            //TODO Parse User Data
            try
            {
                sProperties = SimpleXML.LoadXml(data);
            }
            catch (Exception) { }

        }

        #endregion

        #region Системные данные (IDEXSysData) / 28.11.09

        string sAppDir;
        string sDataDir;
        string sOutDir;

        public string AppDir { get { return sAppDir; } }
        public string DataDir { get { return sDataDir; } }
        public string OutDir { get { return sOutDir; } } // Не описано в интерфейсе 
        public string[] DocumentStatesText { get { return DEXToolBox.DOCUMENT_STATE_TEXT; } }

        public string[] currentDocumentTypes = null;

        public ArrayList DocumentTypes()
        {
            if (currentDocumentTypes != null) return new ArrayList(currentDocumentTypes);
            return null;
            // Можно брать значения из StringDbItem в main
//            return Plugins.getDocuments();
        }

        public void keybRU()
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo(0x419));
        }

        public void keybEN()
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo(0x409));
        }

        public IFormatProvider getCulture()
        {
            return new System.Globalization.CultureInfo("ru-RU", true);
        }

        public string checkDir(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        #endregion

        #region Получение хэша MD5 строки (IDEXCrypt)

        public string StringToMD5(string source)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        #region Шифрование / дешифрование строк методом Rijndael (IDEXCrypt)
        // (c) RSDN

        public byte[] Encrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        public string Encrypt(string data, string password)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), password));
        }

        public byte[] Decrypt(byte[] data, string password)
        {
            BinaryReader br = new BinaryReader(InternalDecrypt(data, password));
            return br.ReadBytes((int)br.BaseStream.Length);
        }

        public string Decrypt(string data, string password)
        {
            CryptoStream cs = InternalDecrypt(Convert.FromBase64String(data), password);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        CryptoStream InternalDecrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }

        public string Encrypt(string data)
        {
            return Encrypt(data, DEXToolBox.SECRETKEY);
        }

        public string Decrypt(string data)
        {
            return Decrypt(data, DEXToolBox.SECRETKEY);
        }
        #endregion

        #region Работа с произвольными службами модулей (IDEXServices)

        Dictionary<string, object> dicServices = new Dictionary<string, object>();

        public void RegisterService(string servicename, Object service)
        {
            if (dicServices.ContainsKey(servicename))
            {
                UnregisterService(servicename);
            }
            dicServices[servicename] = service;
        }

        public void UnregisterService(string servicename)
        {
            dicServices.Remove(servicename);
        }

        public Object GetService(string servicename)
        {
            if (dicServices.ContainsKey(servicename)) return dicServices[servicename];
            return null;
        }

        #endregion

        #region Работа с конфигурационным файлом и регистрами (IDEXConfig) / 29.12.09

        SimpleXML _xml = null;

        string _getIDEXConfigName()
        {
            string fn = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location
            ) + "\\" + DEXToolBox.CONFIGFILE;

            return fn;
        }

        void _loadIDEXConfig()
        {
            try
            {
                if (_xml == null)
                {
                    _xml = SimpleXML.LoadXml(File.ReadAllText(_getIDEXConfigName()));
                }
            }
            catch (Exception)
            {
                _xml = new SimpleXML("Config");
            }
        }

        void _saveIDEXConfig()
        {
            if (_xml == null) _loadIDEXConfig();
            try
            {
                File.WriteAllText(_getIDEXConfigName(), SimpleXML.SaveXml(_xml));
            }
            catch (Exception)
            {
            }
        }

        public bool isSectionExists(string section)
        {
            _loadIDEXConfig();
            try
            {
                return _xml.GetNodeByPath(section, false) != null;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool isKeyExists(string section, string key)
        {
            _loadIDEXConfig();
            try
            {
                return _xml.GetNodeByPath(section + @"\" + key, false) != null;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool isBinary(string section, string key)
        {
            _loadIDEXConfig();
            try
            {
                return _xml.GetNodeByPath(section + @"\" + key, false).isBinary();
            }
            catch (Exception)
            {
            }
            return false;
        }

        public string getStr(string section, string key, string def)
        {
            _loadIDEXConfig();
            try
            {
                SimpleXML sv = _xml.GetNodeByPath(section + @"\" + key, false);

                if (sv != null) return sv.Text;
            }
            catch (Exception)
            {
            }

            return def;
        }

        public int getInt(string section, string key, int def)
        {
            try
            {
                return int.Parse(getStr(section, key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public bool getBool(string section, string key, bool def)
        {
            try
            {
                return bool.Parse(getStr(section, key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public float getFloat(string section, string key, float def)
        {
            try
            {
                return float.Parse(getStr(section, key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public DateTime getDate(string section, string key, DateTime def)
        {
            try
            {
                return DateTime.Parse(getStr(section, key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public byte[] getBinary(string section, string key, byte[] def)
        {
            _loadIDEXConfig();
            try
            {
                SimpleXML sv = _xml.GetNodeByPath(section + @"\" + key, false);

                if (sv != null && sv.isBinary()) return sv.Binary;
            }
            catch (Exception)
            {
            }

            return def;
        }

        public void setStr(string section, string key, string value)
        {
            _loadIDEXConfig();
            _xml.GetNodeByPath(section + @"\" + key, true).Text = value;
            _saveIDEXConfig();
        }

        public void setInt(string section, string key, int value)
        {
            setStr(section, key, value.ToString());
        }

        public void setBool(string section, string key, bool value)
        {
            setStr(section, key, value.ToString());
        }

        public void setFloat(string section, string key, float value)
        {
            setStr(section, key, value.ToString());
        }

        public void setDate(string section, string key, DateTime value)
        {
            setStr(section, key, value.ToString());
        }

        public void setBinary(string section, string key, byte[] value)
        {
            _loadIDEXConfig();
            _xml.GetNodeByPath(section + @"\" + key, true).Binary = value;
            _saveIDEXConfig();
        }

        Dictionary<string, string> dRegisters = null;

        void checkRegisters()
        {
            // Если регистры не открыты - открыть с сервера и сохранить на диск.
            // Если на сервер не пускают - открыть с диска последние регистры.
            if (dRegisters == null)
            {
                dRegisters = new Dictionary<string, string>();

                try
                {
                    DataTable t = DexolSession.inst().jsonToDataTable(
                        File.ReadAllText(sDataDir + DexolSession.inst().currentDb + @"\registers.json", Encoding.UTF8)
                        );
                    if (t != null && t.Rows.Count > 0)
                    {
                        foreach (DataRow row in t.Rows)
                        {
                            dRegisters[row["rname"].ToString()] = row["rvalue"].ToString();
                        }
                    }
                    /*
                    JObject json = JObject.Parse(File.ReadAllText(sDataDir + "registers.json", Encoding.UTF8));
                    foreach (JProperty jprop in json.Properties())
                    {
                        dRegisters[jprop.Name] = (string)jprop.Value;
                    }
                     */
                }
                catch (Exception) { }
            }
        }

        public bool isRegisterKeyExists(string key) //dexol
        {
            checkRegisters();
            if (dRegisters != null) return dRegisters.ContainsKey(key);
            return false;
        }

        public void createRegisterKey(string key, string title, string value) //dexol
        {
            // В клиенте не создаются регистры                
        }

        public string getRegisterStr(string key, string def) //dexol
        {
            checkRegisters();
            if (dRegisters != null && dRegisters.ContainsKey(key)) return dRegisters[key];
            return def;
        }

        public int getRegisterInt(string key, int def) //dexol
        {
            try
            {
                return int.Parse(getRegisterStr(key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }
        public bool getRegisterBool(string key, bool def) //dexol
        {
            try
            {
                return bool.Parse(getRegisterStr(key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public float getRegisterFloat(string key, float def) //dexol
        {
            try
            {
                return float.Parse(getRegisterStr(key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }

        public DateTime getRegisterDate(string key, DateTime def) //dexol
        {
            try
            {
                return DateTime.Parse(getRegisterStr(key, def.ToString()));
            }
            catch (Exception)
            {
            }
            return def;
        }


        public void setRegisterStr(string key, string value) //dexol
        {
            // В клиенте не создаются регистры                
        }

        public void setRegisterInt(string key, int value) //dexol
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterBool(string key, bool value) //dexol
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterFloat(string key, float value) //dexol
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterDate(string key, DateTime value) //dexol
        {
            setRegisterStr(key, value.ToString());
        }

        #endregion

        #region Работа с правами пользователя (IDEXRights) / 08.01.10

        ArrayList user_rights;

        private RightsItem GetRightsItemObj(string AKey)
        {
            try
            {
                if (user_rights != null && user_rights.Count > 0)
                {
                    foreach (RightsItem ritem in user_rights)
                    {
                        if (ritem.ID.CompareTo(AKey) == 0)
                        {
                            return ritem;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public void AddRightsItem(string AKey, string ATitle)
        {
            RightsItem item = GetRightsItemObj(AKey);
            if (item == null)
            {
                item = new RightsItem(AKey, ATitle, false);
                user_rights.Add(item);
            }
        }

        public void SetRightsItem(string AKey, bool AValue)
        {
            RightsItem item = GetRightsItemObj(AKey);
            if (item != null) item.CanAccess = AValue;
        }

        public bool GetRightsItem(string AKey)
        {
            try
            {
                return GetRightsItemObj(AKey).CanAccess;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public ArrayList GetRightsTable()
        {
            return user_rights;
        }

        public bool IsSuperUser()
        {
            return true; // Пусть модули документов считают нас супер-юзером
            //return GetRightsItem(SUPERUSER_KEY);
        }

        public void ParseRights(SimpleXML rnode)
        {
            if (rnode != null)
            {
                string srights = rnode.Text;
                ArrayList rt = GetRightsTable();
                foreach (RightsItem ri in rt)
                {
                    ri.CanAccess = (srights.IndexOf("|" + ri.ID + "|") > -1);
                }
            }
        }

        #endregion

        #region Работа с журналом документа (IDEXDocumentJournal) / 02.01.10

        SimpleXML currentJournal = null;

        public SimpleXML getCurrentJournal()
        {
            return currentJournal;
        }

        public void setCurrentJournal(SimpleXML journal)
        {
            currentJournal = journal;
        }

        public void AddRecord(SimpleXML journal, string text)
        {
            if (journal != null)
            {
                SimpleXML rec = journal.CreateChild("record");
                rec.Attributes.Add("time", DateTime.Now.ToString("hh:mm:ss dd:MM:yyyy"));
                rec.CreateChild("text").Text = text;
            }
        }

        public void AddRecord(string text)
        {
            AddRecord(currentJournal, text);
        }

        public void AddRecord(SimpleXML journal, string text, Array subdata)
        {
            if (journal != null)
            {
                SimpleXML rec = journal.CreateChild("record");
                rec.Attributes.Add("time", DateTime.Now.ToString("hh:mm:ss dd:MM:yyyy"));
                rec.CreateChild("text").Text = text;
                try
                {
                    foreach (string s in subdata)
                    {
                        rec.CreateChild("subdata").Text = s;
                    }
                }
                catch (Exception)
                {
                }
            }

        }


        public void AddRecord(string text, Array subdata)
        {
            AddRecord(currentJournal, text, subdata);
        }

        public void AddRecord(string text, ArrayList subdata)
        {
            try
            {
                AddRecord(currentJournal, text, subdata.ToArray());
            }
            catch (Exception)
            {
            }
        }


        #endregion

        #region Работа со справочником городов (IDEXCitySearcher)

        public DataTable dtCity = null;

        void checkCityValuesList()
        {
            if (dtCity == null)
            {
                try
                {
                    dtCity = DexolSession.inst().jsonToDataTable(File.ReadAllText(sDataDir + DexolSession.inst().currentDb + @"\city.json", Encoding.UTF8));
                }
                catch (Exception) { }
            }
        }

        public void updateCityValuesList(string field) // dexol
        {
        }

        public string[] getCityValuesList(string field)
        {
            checkCityValuesList();
            string[] ret = new string[0];

            try
            {
                int cur = 0, cnt = dtCity.Rows.Count;
                ret = new string[cnt];
                foreach (DataRow row in dtCity.Rows)
                {
                    ret[cur++] = row[field].ToString();
                }
            }
            catch (Exception) { } 

            return ret;
        }

        public Dictionary<string, string> getCityData(string field, string value)
        {
            checkCityValuesList();

            try
            {
                DataRow[] rows = dtCity.Select(string.Format("{0}='{1}'", field, EscapeString(value)));
                if (rows != null && rows.Length > 0)
                {
                    Dictionary<string, string> ret = new Dictionary<string, string>();
                    ret["zip"] = rows[0]["zip"].ToString();
                    ret["city"] = rows[0]["city"].ToString();
                    ret["region"] = rows[0]["region"].ToString();
                    return ret;
                }
            }
            catch (Exception) { }

            return null;
        }

        public void setCityData(Dictionary<string, string> values)
        {
            // Клиент не добавляет новые записи в общие справочники
        }

        #endregion

        #region Работа со справочником абонентов (IDEXPeopleSearcher) / 08.01.10

        public ArrayList getPeopleHash(string FirstName, string SecondName, string LastName, string Birth)
        {
            try
            {
                string whr = "";
                if (FirstName != null) whr += "firstname like '" + EscapeString(FirstName) + "'";
                if (SecondName != null) whr += (whr != "" ? " and " : "") + "secondname like '" + EscapeString(SecondName) + "'";
                if (LastName != null) whr += (whr != "" ? " and " : "") + "lastname like '" + EscapeString(LastName) + "'";
                if (Birth != null) whr += (whr != "" ? " and " : "") + "birth = '" + EscapeString(Birth) + "'";
                if (whr != "") whr = "where " + whr;

                DataTable t = getQuery("select phash from `people` " + whr);
                if (t != null && t.Rows.Count > 0)
                {
                    ArrayList ret = new ArrayList();
                    foreach (DataRow row in t.Rows)
                    {
                        ret.Add(row["phash"].ToString());
                    }
                    return ret;
                }
            }
            catch (Exception) { }

            return null;
        }

        public StringList getPeopleData(string Hash)
        {
            if (Hash != null)
            {
                DataTable t = getQuery("select * from `people` where phash='{0}'", EscapeString(Hash));
                StringList ret = new StringList();
                if (t != null && t.Rows.Count > 0)
                {
                    ret.LoadFromString(t.Rows[0]["data"].ToString());
                    ret["suspect"] = Convert.ToString(t.Rows[0]["suspect"]);
                }

                return ret;
            }

            return null;
        }

        public string setPeopleData(StringList PData)
        {
            if (PData.ContainsKey("firstname") && PData.ContainsKey("secondname") &&
                PData.ContainsKey("lastname") && PData.ContainsKey("birth"))
            {
                string nhash = StringToMD5(PData["firstname"] + PData["secondname"] + PData["lastname"] + PData["birth"]);

                ArrayList hashes = getPeopleHash(PData["firstname"], PData["secondname"], PData["lastname"], PData["birth"]);
                if (hashes != null && hashes.Count > 0)
                {
                    runQuery("update `people` set data='{0}' where phash='{1}'", EscapeString(PData.SaveToString()), EscapeString(nhash));
                }
                else
                {
                    runQuery(
                        "insert into `people` (phash, firstname, secondname, lastname, birth, data) values " +
                        "('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        EscapeString(nhash), EscapeString(PData["firstname"]), EscapeString(PData["secondname"]),
                        EscapeString(PData["lastname"]), EscapeString(PData["birth"]), EscapeString(PData.SaveToString())
                        );
                }

                return nhash;
            }
            return null;
        }

        public string[] checkPassport(StringList PData)
        {
            List<string> msgs = new List<string>();

            if (PData.ContainsKeys(new string[] { "firstname", "secondname", "lastname", "birth", "FizDocSeries", "FizDocNumber" }) &&
                PData["firstname"] != "" && (PData["secondname"] != "" || PData["secondname"] == "") && PData["lastname"] != "" && PData["birth"] != "" &&
                PData["FizDocSeries"] != "" && PData["FizDocNumber"] != "")
            {
                string nhash = StringToMD5(PData["firstname"] + PData["secondname"] + PData["lastname"] + PData["birth"]);

                // Получаем очищенное проверяемое Ф.И.О. и дату рождения
                string ofn = PData["firstname"].ToLowerInvariant().Trim();
                string osn = PData["secondname"].ToLowerInvariant().Trim();
                string oln = PData["lastname"].ToLowerInvariant().Trim();
                string od = PData["birth"].Trim();

                DexolSession ds = DexolSession.inst();
                try
                {
                    
                    // Выгребем всё, что имеет тот же номер и серию, но другие фио или дату рождения.
                    string sql = string.Format(
                        "select * from `people` where data like '%FizDocSeries={0}%' and data like '%FizDocNumber={1}%' and phash<>'{2}'",
                        EscapeString(PData["FizDocSeries"]), EscapeString(PData["FizDocNumber"]), EscapeString(nhash)
                        );

                    DataTable dt = DexolSession.inst().getQuery(sql);
                    
                    if (dt == null || dt.Rows.Count < 1) return null; // Всё хорошо, нет двойников

                    foreach (DataRow r in dt.Rows)
                    {
                        StringList sl = new StringList(r["data"].ToString());
                        string nfn = sl["firstname"].ToLowerInvariant().Trim();
                        string nsn = sl["secondname"].ToLowerInvariant().Trim();
                        string nln = sl["lastname"].ToLowerInvariant().Trim();
                        string nd = sl["birth"].Trim();

                        if (nfn.CompareTo(ofn) != 0 || nsn.CompareTo(osn) != 0 || nln.CompareTo(oln) != 0 || nd.CompareTo(od) != 0)
                        {
                            msgs.Add(string.Format("Совпадение пасп.данных: {0} {1} {2}, {3}", sl["lastname"], sl["secondname"], sl["firstname"], sl["birth"]));
                        }
                    }
                }
                catch (Exception)
                {
                    msgs.Add("Системная ошибка при проверке паспорта");
                }
            }
            else
            {
                msgs.Add("Не указаны паспортные данные для проверки");
            }

            if (msgs.Count == 0) return null;
            return msgs.ToArray();
         }

        #endregion

        #region Работа с принтером (IDEXPrinter) / 15.01.10

        public void ClearPrinterSettings()
        {
            try
            {
                setBinary("printers", "printer", null);
            }
            catch (Exception) { }
        }

        public PrinterSettings LoadPrinterSettings()
        {
            try
            {
                if (isBinary("printers", "printer"))
                {
                    byte[] pss = getBinary("printers", "printer", null);
                    if (pss != null)
                    {
                        MemoryStream ms = new MemoryStream(pss);
                        BinaryFormatter bf = new BinaryFormatter();

                        return (PrinterSettings)bf.Deserialize(ms);
                    }
                }
            }
            catch (Exception)
            {
            }

            return new PrinterSettings();
        }

        public void SavePrinterSettings(PrinterSettings ps)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, ps.Clone());
                setBinary("printers", "printer", ms.ToArray());
            }
            catch (Exception)
            {
            }
        }


        #endregion

        #region Валидаторы (IDEXValidators) / 13.01.11

        public bool CheckPeriodDate(DateTime src)
        {
            try
            {
                string d = getRegisterStr("startperiod", "-");
                if (!d.Equals("-") && d.Length >= 8)
                {
                    DateTime dst = new DateTime(int.Parse(d.Substring(0, 4)), int.Parse(d.Substring(4, 2)), int.Parse(d.Substring(6, 2)), 0, 0, 0);
                    return src >= dst;
                }
            }
            catch (Exception) { }

            return true;
        }

        #endregion

        #region IDEXSim

        public DataTable simTable = null;
        public DataTable simRegions = null;
        public DataTable simPlans = null;


        Dictionary<string, string> intGetSimBy(string field, string value)
        {
            DexolSession ds = DexolSession.inst();

            if (simPlans == null)
            {
                try
                {
                    simPlans = ds.jsonToDataTable(File.ReadAllText(DataDir + ds.currentDb + @"\um_plans.json", Encoding.UTF8));
                }
                catch (Exception) { }
            }

            if (simRegions == null)
            {
                try
                {
                    simRegions = ds.jsonToDataTable(File.ReadAllText(DataDir + ds.currentDb + @"\um_regions.json", Encoding.UTF8));
                }
                catch (Exception) { }
            }

            if (simTable == null)
            {
                try
                {
                    simTable = ds.jsonToDataTable(File.ReadAllText(DataDir + ds.currentDb + @"\um_data.json", Encoding.UTF8));
                }
                catch (Exception) { }
            }

            try
            {
                DataRow r = simTable.Select(string.Format("{0} = '{1}'", field, EscapeString(value)))[0];
                Dictionary<string, string> ret = new Dictionary<string, string>();

                foreach (DataColumn c in simTable.Columns)
                {
                    ret[c.ColumnName] = r[c.ColumnName].ToString();
                }

                int unitid = int.Parse(r["owner_id"].ToString());
                string planid = r["plan_id"].ToString();
                string regionid = r["region_id"].ToString();
                ret["owner_title"] = "";
                ret["owner_desc"] = "";
                ret["owner_status"] = "";
                ret["owner_data"] = "";

                if (simPlans != null)
                {
                    DataRow[] rows = simPlans.Select(string.Format("plan_id = '{0}'", EscapeString(planid)));
                    if (rows != null && rows.Length > 0)
                    {
                        ret["plan_title"] = rows[0]["title"].ToString();
                    }
                }

                if (simRegions != null)
                {
                    DataRow[] rows = simRegions.Select(string.Format("region_id = '{0}'", EscapeString(regionid)));
                    if (rows != null && rows.Length > 0)
                    {
                        ret["region_title"] = rows[0]["title"].ToString();
                    }
                }

                return ret;
            }
            catch (Exception) { }

            return null;
        }

        public Dictionary<string, string> getSimByMSISDN(string msisdn)
        {
            return intGetSimBy("msisdn", msisdn);
        }

        public Dictionary<string, string> getSimByICC(string icc)
        {
            return intGetSimBy("icc", icc);
        }

        public List<string> getFreeSim()
        {
            DexolSession ds = DexolSession.inst();

            if (simTable == null)
            {
                try
                {
                    simTable = ds.jsonToDataTable(File.ReadAllText(DataDir + ds.currentDb + @"\um_data.json", Encoding.UTF8));
                }
                catch (Exception) {
                    return null;
                }
            }

            try
            {
                List<string> ret = new List<string>();
                foreach (DataRow r in simTable.Rows)
                {
                    ret.Add(r["msisdn"].ToString());
                }
                return ret;
            }
            catch (Exception) { }

            return null;
        }

        #endregion

        #region IDEXPrinter Members

        PrinterSettings IDEXPrinter.LoadPrinterSettings()
        {
            try
            {
                if (isBinary("printers", "printer"))
                {
                    byte[] pss = getBinary("printers", "printer", null);
                    if (pss != null)
                    {
                        MemoryStream ms = new MemoryStream(pss);
                        BinaryFormatter bf = new BinaryFormatter();

                        return (PrinterSettings)bf.Deserialize(ms);
                    }
                }
            }
            catch (Exception)
            {
            }

            return new PrinterSettings();
        }

        void IDEXPrinter.SavePrinterSettings(PrinterSettings ps)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, ps.Clone());
                setBinary("printers", "printer", ms.ToArray());
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region  настройки сервера проверки паспортов
        
        public bool accessRemoteServer = false;
        public string passpHostDb = "";
        public string passpUserDb = "";
        public string passpNameDb = "";
        public string passpPassDb = "";

        public bool AccessRemoteServer
        {
            get
            {
                return accessRemoteServer;
            }
        }
        public string PasspHostDb
        {
            get
            {
                return passpHostDb;
            }
        }
        public string PasspUserDb
        {
            get
            {
                return passpUserDb;
            }
        }
        public string PasspNameDb
        {
            get
            {
                return passpNameDb;
            }
        }
        public string PasspPassDb
        {
            get
            {
                return passpPassDb;
            }
        }
        #endregion
    }
}
