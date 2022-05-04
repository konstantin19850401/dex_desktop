using System;
using System.Threading;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using DEXExtendLib;
using MySql.Data.MySqlClient;
using System.Drawing.Printing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Management;
//using System.Data.Common;
//using System.Data.SQLite;
using System.Net;
using System.Web;

namespace DEXOffice
{
    public class DEXToolBox : IDEXPluginSystemData, IDEXUserData, IDEXSysData, IDEXConfig, IDEXData, IDEXMySqlData, 
                 IDEXCrypt, IDEXServices, IDEXTrigger, IDEXRights, IDEXDocumentJournal, IDEXCitySearcher,
                 IDEXPeopleSearcher, IDEXScheduler, IDEXPrinter, IDEXValidators//, IDEXPlans

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

        string locker_id;

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
        #region отправка произвольного запроса на произовольный адрес
        public string sendRequest(string method, string url, string port, string body, int reqStatus)
        {
            byte[] buf = new byte[8192];
            StringBuilder sb = new StringBuilder();
            string str = "{}";
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

        public bool getAccessRemoteServer()
        {
                return accessRemoteServer;
        }

        public static string[] DOCUMENT_STATE_TEXT = { "Черновик", "На подтверждение", "Подтверждён", "На отправку", "Отправлен", "Возвращён", "Отправляется" };

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
            triggers = new Dictionary<string, string>();
            user_rights = new ArrayList();
            AddRightsItem(SUPERUSER_KEY, SUPERUSER_TITLE);
            AddRightsItem(RESTRICTED_KEY, RESTRICTED_TITLE);


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

            /*
            // SQLite test
            string baseName = sDataDir + "CompanyWorkers.db3";


            SQLiteConnection.CreateFile(baseName);

            SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + baseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"CREATE TABLE [workers] (
                                [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                                [name] char(100) NOT NULL,
                                [family] char(100) NOT NULL,
                                [age] int NOT NULL,
                                [profession] char(100) NOT NULL
                                );";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
             */
        }

        #region Получение экземпляра ToolBox
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

        #region Задачи (IDEXSchedule)

        Dictionary<Object, DateTime> schedules = new Dictionary<object, DateTime>();
        object schedulerLocker = new object();
        bool doStopSchedule = false;
        bool doSuspendSchedule = false;
        int idleCounter;
        Thread schedulerThread = null;

        public void AddSchedule(IDEXPluginSchedule schedule, DateTime nextTime)
        {
            if (schedules.ContainsKey(schedule))
            {
                if (nextTime < DateTime.Now)
                {
                    RemoveSchedule(schedule);
                }
                else
                {
                    schedules[schedule] = nextTime;
                }
            }
            else
            {
                if (nextTime >= DateTime.Now)
                {
                    schedules.Add(schedule, nextTime);
                }
            }
        }

        public void AddSchedule(IDEXPluginSchedule schedule, int SecondsAfterNow)
        {
            AddSchedule(schedule, DateTime.Now.AddSeconds(SecondsAfterNow));
        }

        public void RemoveSchedule(IDEXPluginSchedule schedule)
        {
            if (schedules.ContainsKey(schedule))
            {
                try
                {
                    schedules.Remove(schedule);
                }
                catch (Exception)
                {
                }
            }
        }

        public bool IsScheduleIdle(IDEXPluginSchedule schedule)
        {
            return !schedules.ContainsKey(schedule);
        }

        public DateTime GetScheduleTime(IDEXPluginSchedule schedule)
        {
            if (schedules.ContainsKey(schedule)) return schedules[schedule];
            return DateTime.MinValue;
        }


        public void StartSchedule()
        {
            idleCounter = 0;
            if (schedulerThread == null || !schedulerThread.IsAlive)
            {
                schedulerThread = new Thread(DoSchedules);
                schedulerThread.Priority = ThreadPriority.Lowest;
                schedulerThread.Start();
            }
            else
            {
                ContinueSchedule();
            }
        }

        public void SuspendSchedule()
        {
            doSuspendSchedule = true;
        }

        public void ContinueSchedule()
        {
            doSuspendSchedule = false;
        }

        public void EndSchedule()
        {
            doStopSchedule = true;
        }
        

        /// <summary>
        /// Обработчик заданий планировщика
        /// </summary>
        public void DoSchedules()
        {
            while (!doStopSchedule)
            {
                while (doSuspendSchedule)
                {
                    Thread.Sleep(1000);
                    if (doStopSchedule) break;
                }
                try
                {
                    if (!doStopSchedule && !doSuspendSchedule)
                    {
                        lock (schedulerLocker)
                        {
                            // Отработка запланированных заданий
                            Dictionary<Object, DateTime> sh2 = schedules;
                            schedules = new Dictionary<Object, DateTime>();
                            foreach (KeyValuePair<Object, DateTime> kvp in sh2)
                            {
                                if (kvp.Key is IDEXPluginSchedule)
                                {
                                    if (kvp.Value > DateTime.Now)
                                    {
                                        schedules[kvp.Key] = kvp.Value;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            ((IDEXPluginSchedule)kvp.Key).Schedule(this);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }

                            if (++idleCounter >= 10)
                            {
                                idleCounter = 0;
                                
                                // Отработка заданий, отсутствующих в очереди планировщика
                                ArrayList sh3 = Plugins.getSchedules();
                                foreach (Object shi in sh3)
                                {
                                    if (shi is IDEXPluginSchedule && !schedules.ContainsKey(shi))
                                    {
                                        try
                                        {
                                            ((IDEXPluginSchedule)shi).Idle(this);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion

        #region Соединение с БД (IDEXData) / 06.11.09

        private MySqlConnection connection;

        public MySqlConnection initConnection(string connectionStr)
        {
            connection = new MySqlConnection(connectionStr);
            return getConnection();
        }

        public MySqlConnection getConnection()
        {
            try
            {

                if (connection != null)
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.Open();
                    }
                }
 
                return connection;
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public DataTable getQuery(MySqlCommand cmd)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
            ad.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }

        public DataTable getQuery(string Query, params object[] parameters)
        {
            try
            {
                return getQuery(string.Format(Query, parameters));
            }
            catch (Exception) { }
            return null;
        }

        public DataTable getQuery(string Query)
        {
            MySqlConnection c = getConnection();
            if (c == null) return null;

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter(Query, c);

            try
            {
                ad.Fill(dt);
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }

        public int runQuery(string Query)
        {
            MySqlCommand cmd = new MySqlCommand(Query, getConnection());
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            
            return -1;
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
            MySqlCommand cmd = new MySqlCommand(Query, getConnection());
            try
            {
                cmd.ExecuteNonQuery();
                return cmd.LastInsertedId;
            }
            catch (Exception)
            {
            }

            return -1;
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

            return getQuery("select * from `" + TableName + "`");
        }

        public MySqlDataAdapter getDataAdapter(string query)
        {
            if (query == null || query.Length < 1) return null;

            try
            {
                return new MySqlDataAdapter(query, getConnection());
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public void setDataReference(string TableName, string Key, bool DoIncrement)
        {
            if (TableName == null || TableName.Length < 1) return;
            if (Key == null || Key.Length < 1) return;

            MySqlConnection c = getConnection();
            if (c == null) return;

            MySqlCommand cmd = new MySqlCommand(
                "select id, refcount from `datarefs` where reftable=@reftable and refkey=@refkey", 
                c);
            cmd.Parameters.Add(new MySqlParameter("reftable", TableName));
            cmd.Parameters.Add(new MySqlParameter("refkey", Key));

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);

            bool refUpdated = false;
    
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    int id = int.Parse(dt.Rows[0]["id"].ToString());
                    int refcount = int.Parse(dt.Rows[0]["refcount"].ToString());
                    if (DoIncrement) refcount++;
                    else refcount--;
                    if (refcount < 1)
                    {
                        cmd = new MySqlCommand("delete from `datarefs` where id=@id", c);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand("update `datarefs` set refcount=@refcount where id=@id", c);
                        cmd.Parameters.AddWithValue("@refcount", refcount);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    refUpdated = true;
                }
            }
            catch (Exception)
            {
            
            }

            if (!refUpdated)
            {
                cmd = new MySqlCommand("insert into `datarefs` (reftable, refkey, refcount) values (@reftable, @refkey, 1)", c);
                cmd.Parameters.AddWithValue("@reftable", TableName);
                cmd.Parameters.AddWithValue("@refkey", Key);
                cmd.ExecuteNonQuery();
            }

        }

        public int getDataReference(string TableName, string Key)
        {
            if (TableName == null || TableName.Length < 1) return 0;
            if (Key == null || Key.Length < 1) return 0;

            MySqlConnection c = getConnection();
            if (c == null) return 0;

            MySqlCommand cmd = new MySqlCommand(
                "select id, refcount from `datarefs` where reftable=@reftable and refkey=@refkey", 
                c);
            cmd.Parameters.Add(new MySqlParameter("reftable", TableName));
            cmd.Parameters.Add(new MySqlParameter("refkey", Key));

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                return int.Parse(dt.Rows[0]["refcount"].ToString());
            }

            return 0;
        }

        public void clearDataReference(string TableName, string Key)
        {
            if (TableName == null || TableName.Length < 1) return;
            if (Key == null || Key.Length < 1) return;

            MySqlConnection c = getConnection();
            if (c == null) return;

            MySqlCommand cmd = new MySqlCommand(
                "delete from `datarefs` where reftable=@reftable and refkey=@refkey", 
                c);
            cmd.Parameters.AddWithValue("@reftable", TableName);
            cmd.Parameters.AddWithValue("@refkey", Key);
            cmd.ExecuteNonQuery();
        }

        public void setDataHint(string hintType, string hintValue)
        {
            if (hintType == null || hintType.Trim().Length < 1) return;

            MySqlConnection c = getConnection();
            if (c == null) return;

            MySqlCommand cmd = new MySqlCommand(
                "delete from `hints` where hinttype=@hinttype and hintvalue=@hintvalue", 
                c);
            cmd.Parameters.Add(new MySqlParameter("hinttype", hintType));
            cmd.Parameters.Add(new MySqlParameter("hintvalue", hintValue));
            cmd.ExecuteNonQuery();

            cmd.CommandText = "insert into `hints` (hinttype, hintvalue) values (@hinttype, @hintvalue)";
            cmd.ExecuteNonQuery();
        }

        public string[] getDataHint(string hintType)
        {
            List<string> ret = new List<string>();
            if (hintType == null || hintType.Trim().Length < 1) return ret.ToArray();

            MySqlConnection c = getConnection();
            if (c == null) return ret.ToArray();

            MySqlCommand cmd = new MySqlCommand(
                "select hintvalue from `hints` where hinttype=@hinttype", 
                c);
            cmd.Parameters.Add(new MySqlParameter("hinttype", hintType));

            MySqlDataReader r = cmd.ExecuteReader();
            try
            {
                while (r.Read())
                {
                    ret.Add(r.GetString("hintvalue"));
                }
            }
            finally
            {
                r.Close();
            }

            return ret.ToArray();
        }

        public ArrayList checkDocumentCriticals(ArrayList fields, IDEXDocumentData document)
        {
            SimpleXML doc = SimpleXML.LoadXml(document.documentText);
            MySqlConnection c = getConnection();
            ArrayList ret = new ArrayList();
            foreach (string field in fields)
            {
                SimpleXML node = doc.GetNodeByPath(field, false);
                if (node != null && node.Text.Length > 0)
                {
                    string cvalue = node.Text;
                    MySqlCommand cmd = new MySqlCommand(
                        "select * from `criticals` where cname=@cname and cvalue=@cvalue " +
                        "and signature <> @signature", c
                    );
                    cmd.Parameters.Add(new MySqlParameter("cname", field));
                    cmd.Parameters.Add(new MySqlParameter("cvalue", cvalue));
                    cmd.Parameters.Add(new MySqlParameter("signature", document.signature));

                    try
                    {
                        DataTable t = getQuery(cmd);


                        if (t != null && t.Rows != null && t.Rows.Count > 0)
                        {
                            foreach(DataRow r in t.Rows)
                            {
                                MySqlCommand cmd2 = new MySqlCommand(
                                    "select digest from `journal` where signature=@signature", c
                                );
                                cmd2.Parameters.Add(new MySqlParameter("signature", r["signature"].ToString()));
                                MySqlDataReader r2 = cmd2.ExecuteReader();
                                try
                                {
                                        while (r2.Read())
                                        {
                                            ret.Add(string.Format("{0}: {1} ({2})", field, cvalue, r2.GetString("digest")));
                                        }
                                }
                                finally
                                {
                                    r2.Close();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: {0}", e.Message);
                    }
                }
            }

            return ret;
        }

        public void setDocumentCriticals(ArrayList fields, IDEXDocumentData document, bool reset)
        {
            MySqlConnection c = getConnection();

            if (reset)
            {
                MySqlCommand cmd = new MySqlCommand(
                    "delete from `criticals` where signature=@signature", c
                );
                cmd.Parameters.Add(new MySqlParameter("signature", document.signature));
                cmd.ExecuteNonQuery();
            }

            if (document.documentStatus > DOCUMENT_DRAFT && document.documentStatus != DOCUMENT_TODELETE)
            {
                SimpleXML doc = SimpleXML.LoadXml(document.documentText);
                foreach (string field in fields)
                {
                    SimpleXML node = doc.GetNodeByPath(field, false);
                    if (node != null && node.Text.Length > 0)
                    {
                        string cvalue = node.Text;
                        MySqlCommand cmd = new MySqlCommand(
                            "insert into `criticals` (signature, cname, cvalue) values (@signature, @cname, @cvalue)", c
                        );
                        cmd.Parameters.Add(new MySqlParameter("signature", document.signature));
                        cmd.Parameters.Add(new MySqlParameter("cname", field));
                        cmd.Parameters.Add(new MySqlParameter("cvalue", cvalue));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public string EscapeString(string src)
        {
            return MySqlHelper.EscapeString(src);
        }

        #endregion

        #region Пользовательские данные (IDEXUserData) / 08.01.10

        public string sLogin;
        public string sPassword;
        public string sUID;
        public string sTitle;
        public string sCurrentBase = "";
        public DateTime sDateCreated;
        public DateTime sDateChanged;
        public string sUserData;
        public SimpleXML sProperties;
        public string sDataBase;

        public string Login { get { return sLogin; } }
        public string Password { get { return sPassword; } }
        public string UID { get { return sUID; } }
        public string MAC { get { return locker_id; } }
        public string Title { get { return sTitle; } }
        public DateTime DateCreated { get { return sDateCreated; } }
        public DateTime DateChanged { get { return sDateChanged; } }
        public string dataBase { get { return sDataBase;  } }

        //а вот это нужно для подключения к серверу адаптеров
        public string adaptersLogin { get; set; }
        public string adaptersPass { get; set; }
        public string dexServer { get; set; }

        public bool isOnline { get { return false; } }
        public string currentBase { get { return sCurrentBase; } } //Для DEXOL содержит конкретную подключаемую базу. Для DEX поле пусто
        public SimpleXML UserProperties { get { return sProperties; } }

        // настройки сервера проверки паспортов
        public bool accessRemoteServer = false;
        public string passpHostDb = "";
        public string passpUserDb = "";
        public string passpNameDb = "";
        public string passpPassDb = "";

        public bool AccessRemoteServer { get { return accessRemoteServer; } }
        public string PasspHostDb { get { return passpHostDb; } }
        public string PasspUserDb { get { return passpUserDb; } }
        public string PasspNameDb { get { return passpNameDb; } }
        public string PasspPassDb { get { return passpPassDb; } }
        

        public void ParseUserData(string data)
        {
            //TODO Parse User Data
            try
            {
                SimpleXML doc = SimpleXML.LoadXml(data);
                ParseRights(doc["rights"]);
                sProperties = doc["Properties"];
            }
            catch(Exception) { }

        }

        #endregion

        #region Системные данные (IDEXSysData) / 28.11.09

        string sAppDir;
        string sDataDir;

        public string AppDir { get { return sAppDir; } }
        public string DataDir { get { return sDataDir; } }
        public string[] DocumentStatesText { get { return DEXToolBox.DOCUMENT_STATE_TEXT; } }

        public ArrayList DocumentTypes()
        {
            return Plugins.getDocuments();
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

        #endregion

        #region Триггеры (IDEXTrigger)

        Dictionary<string, string> triggers;

        public void setTrigger(string title, string value)
        {
            if (title == null || title.Length < 1) return;
            triggers[title] = value;
        }

        public string getTrigger(string title)
        {
            if (title != null || title.Length > 0 && triggers.ContainsKey(title)) 
            {
                return triggers[title];
            }
            return "";
        }

        public void clearTriggers()
        {
            triggers.Clear();
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

        // // //

        public bool isRegisterKeyExists(string key)
        {
            if (key == null || key.Length < 1) return false;
            
            MySqlConnection c = getConnection();
            if (c == null) return false;

            MySqlCommand cmd = new MySqlCommand(
                "select * from `registers` where rname=@rname", c
                );
            cmd.Parameters.AddWithValue("rname", key);

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            return dt != null && dt.Rows.Count > 0;
        }

        public void createRegisterKey(string key, string title, string value)
        {
            if (key == null || key.Length < 1) return;

            MySqlConnection c = getConnection();
            if (c == null) return;

            MySqlCommand cmd = new MySqlCommand(
                "select * from `registers` where rname=@rname", c
                );
            cmd.Parameters.AddWithValue("rname", key);

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            if (dt == null || dt.Rows.Count < 1)
            {
                cmd = new MySqlCommand(
                    "insert into `registers` (rname, rtitle, rvalue) values (@rname, @rtitle, @rvalue)", c
                    );
            }
            cmd.Parameters.AddWithValue("rname", key);
            cmd.Parameters.AddWithValue("rvalue", value);
            cmd.Parameters.AddWithValue("rtitle", title);
            cmd.ExecuteNonQuery();
        }

        public string getRegisterStr(string key, string def)
        {
            if (key == null || key.Length < 1) return def;

            MySqlConnection c = getConnection();
            if (c == null) return def;

            MySqlCommand cmd = new MySqlCommand(
                "select * from `registers` where rname=@rname", c
                );
            cmd.Parameters.AddWithValue("rname", key);

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["rvalue"].ToString();
            }
            return def;
        }

        public int getRegisterInt(string key, int def)
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
        public bool getRegisterBool(string key, bool def)
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

        public float getRegisterFloat(string key, float def)
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

        public DateTime getRegisterDate(string key, DateTime def)
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


        public void setRegisterStr(string key, string value)
        {
            if (key == null || key.Length < 1) return;    

            MySqlConnection c = getConnection();
            if (c == null) return;

            MySqlCommand cmd = new MySqlCommand(
                "select * from `registers` where rname=@rname", c
                );
            cmd.Parameters.AddWithValue("rname", key);

            DataTable dt = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                cmd = new MySqlCommand(
                    "update `registers` set rvalue=@rvalue where rname=@rname", c
                    );
            }
            else
            {
                cmd = new MySqlCommand(
                    "insert into `registers` (rname, rvalue) values (@rname, @rvalue)", c
                    );
            }
            cmd.Parameters.AddWithValue("rname", key);
            cmd.Parameters.AddWithValue("rvalue", value);
            cmd.ExecuteNonQuery();
        }

        public void setRegisterInt(string key, int value)
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterBool(string key, bool value)
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterFloat(string key, float value)
        {
            setRegisterStr(key, value.ToString());
        }

        public void setRegisterDate(string key, DateTime value)
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
            return GetRightsItem(SUPERUSER_KEY);
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
                rec.Attributes.Add("time", DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"));
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
                rec.Attributes.Add("time", DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"));
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

        Dictionary<string, string[]> cityValuesList = new Dictionary<string, string[]>();

        public void updateCityValuesList(string field)
        {
            List<string> ret = new List<string>();
            try
            {
                ret.Clear();
                DataTable t = getQuery(string.Format(
                    "select {0} from `city` order by {0}", field
                    ));
                if (t != null && t.Rows.Count > 0)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        ret.Add(r[field].ToString());
                    }
                }
            }
            catch (Exception)
            {
            }

            cityValuesList[field] = ret.ToArray();
        }

        public string[] getCityValuesList(string field)
        {
            if (!cityValuesList.ContainsKey(field)) updateCityValuesList(field);
            return cityValuesList[field];
        }

        public Dictionary<string, string> getCityData(string field, string value)
        {
            Dictionary<string, string> ret = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    string.Format("select * from `city` where {0}=@{0}", field),
                    getConnection()
                );
                cmd.Parameters.Add(new MySqlParameter("@" + field, value));

                DataTable t = getQuery(cmd);
                if (t != null && t.Rows.Count > 0)
                {
                    ret = new Dictionary<string, string>();
                    ret["zip"] = t.Rows[0]["zip"].ToString();
                    ret["city"] = t.Rows[0]["city"].ToString();
                    ret["region"] = t.Rows[0]["region"].ToString();
                }
            }
            catch (Exception)
            {
                ret = null;
            }
            return ret;
        }

        public void setCityData(Dictionary<string, string> values)
        {
            try
            {
                if (values.ContainsKey("zip") && values.ContainsKey("city") &&
                    values.ContainsKey("region"))
                {
                    MySqlCommand cmd = new MySqlCommand(
                        string.Format("select count(id) cid from `city` where zip=@zip"),
                        getConnection()
                    );
                    cmd.Parameters.Add(new MySqlParameter("zip", values["zip"].ToString()));
                    DataTable t = getQuery(cmd);

                    bool doUpdate = false;
                    try
                    {
                        doUpdate = (t != null && t.Rows.Count > 0 & int.Parse(t.Rows[0]["cid"].ToString()) > 0);
                    }
                    catch (Exception)
                    {
                    }

                    if (doUpdate)
                    {
                        cmd = new MySqlCommand(
                            "update `city` set city=@city, region=@region where zip=@zip",
                            getConnection()
                        );
                    }
                    else
                    {
                        cmd = new MySqlCommand(
                            "insert into `city` (zip, city, region) values (@zip, @city, @region)",
                            getConnection()
                        );
                    }
                    cmd.Parameters.Add(new MySqlParameter("city", values["city"]));
                    cmd.Parameters.Add(new MySqlParameter("region", values["region"]));
                    cmd.Parameters.Add(new MySqlParameter("zip", values["zip"]));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Работа со справочником абонентов (IDEXPeopleSearcher) / 08.01.10

        public ArrayList getPeopleHash(string FirstName, string SecondName, string LastName, string Birth)
        {
            try
            {
                string whr = "";
                if (FirstName != null) whr += "firstname like '" + MySqlHelper.EscapeString(FirstName) + "'";
                if (SecondName != null) whr += (whr != "" ? " and " : "") + "secondname like '" + MySqlHelper.EscapeString(SecondName) + "'";
                if (LastName != null) whr += (whr != "" ? " and " : "") + "lastname like '" + MySqlHelper.EscapeString(LastName) + "'";
                if (Birth != null) whr += (whr != "" ? " and " : "") + "birth = '" + MySqlHelper.EscapeString(Birth) + "'";

                if (whr != "") whr = "where " + whr;
                MySqlCommand cmd = new MySqlCommand("select phash from `people` " + whr, getConnection());
                MySqlDataReader dr = cmd.ExecuteReader();

                ArrayList ret = new ArrayList();
                try
                {
                    try
                    {
                        while (dr.Read())
                        {
                            ret.Add(dr.GetString("phash"));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                finally
                {
                    dr.Close();
                }
                if (ret.Count < 1) return null;
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public StringList getPeopleData(string Hash)
        {
            if (Hash == null) return null;
            try
            {
                StringList ret = new StringList();
                MySqlCommand cmd = new MySqlCommand("select * from `people` where phash=@phash", getConnection());
                cmd.Parameters.AddWithValue("phash", Hash);
                MySqlDataReader dr = cmd.ExecuteReader();

                try
                {
                    try
                    {
                        if (dr.Read())
                        {
                            ret.LoadFromString(dr.GetString("data"));
                            ret["suspect"] = Convert.ToString(dr.GetInt16("suspect"));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                finally
                {
                    dr.Close();
                }
                return ret;
            }
            catch (Exception)
            {
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
                    MySqlCommand cmd = new MySqlCommand(
                        "update `people` set data=@data where phash=@phash", getConnection()
                        );
                    cmd.Parameters.AddWithValue("phash", nhash);
                    cmd.Parameters.AddWithValue("data", PData.SaveToString());
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand(
                        "insert into `people` (phash, firstname, secondname, lastname, birth, data) values " +
                        "(@phash, @firstname, @secondname, @lastname, @birth, @data)",
                        getConnection()
                        );
                    cmd.Parameters.AddWithValue("phash", nhash);
                    cmd.Parameters.AddWithValue("firstname", PData["firstname"]);
                    cmd.Parameters.AddWithValue("secondname", PData["secondname"]);
                    cmd.Parameters.AddWithValue("lastname", PData["lastname"]);
                    cmd.Parameters.AddWithValue("birth", PData["birth"]);
                    cmd.Parameters.AddWithValue("data", PData.SaveToString());
                    cmd.ExecuteNonQuery();
                }
                return nhash;
            }
            return null;
        }

        public string[] checkPassport(StringList PData)
        {
            List<string> msgs = new List<string>();

            if (PData.ContainsKeys(new string[] { "firstname", "secondname", "lastname", "birth", "FizDocSeries", "FizDocNumber" }) &&
                //PData["firstname"] != "" && PData["secondname"] != "" && PData["lastname"] != "" && PData["birth"] != "" &&
                PData["firstname"] != "" && PData["lastname"] != "" && PData["birth"] != "" && 
                PData["FizDocSeries"] != "" && PData["FizDocNumber"] != "")
            {
                string nhash = StringToMD5(PData["firstname"] + PData["secondname"] + PData["lastname"] + PData["birth"]);

                // Получаем очищенное проверяемое Ф.И.О. и дату рождения
                string ofn = PData["firstname"].ToLowerInvariant().Trim();
                string osn = PData["secondname"].ToLowerInvariant().Trim();
                string oln = PData["lastname"].ToLowerInvariant().Trim();
                string od = PData["birth"].Trim();
                try
                {
                    // Выгребем всё, что имеет тот же номер и серию, но другие фио или дату рождения.
                    DataTable dt = new DataTable();
                    MySqlDataAdapter ada = new MySqlDataAdapter(
                        string.Format("select * from `people` where data like '%FizDocSeries={0}%' and data like '%FizDocNumber={1}%' and phash<>'{2}'",
                        MySqlHelper.EscapeString(PData["FizDocSeries"].Trim()), MySqlHelper.EscapeString(PData["FizDocNumber"]), MySqlHelper.EscapeString(nhash)),
                        getConnection()
                    );

                    ada.Fill(dt);

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
                            msgs.Add(string.Format("Совпадение пасп.данных: {0} {1} {2}, {3}\nИсточник: ({4} {5} {6}, {7})", 
                                sl["lastname"], sl["firstname"], sl["secondname"], sl["birth"],
                                PData["lastname"], PData["firstname"], PData["secondname"], PData["birth"]
                                ));
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

        // убать комментирий ниже
        /*
        #region Тарифные планы
        Dictionary<string, Dictionary<string, string>> tpls;
        public Dictionary<string, Dictionary<string, string>> Tpls
        {
            get
            {
                //object sender;
                //IDEXPluginDocument doc = (IDEXPluginDocument)sender;
                
                return tpls;
            }

        }

        public void setPlans(Dictionary<string, Dictionary<string, string>> dt)
        {
            tpls = dt;
        }
        #endregion
        */

        
    }
}
