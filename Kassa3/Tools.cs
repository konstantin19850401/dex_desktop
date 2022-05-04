using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Drawing;
using System.Data.Common;
using DEXExtendLib;
using System.Net;
using System.Globalization;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Data;
using System.Security.Cryptography;

namespace Kassa3
{
    /// <summary>
    /// Класс с утилитами и объектами, общими для всего приложения/
    /// Синглетон.
    /// </summary>
    public class Tools
    {
        public static string KASSAPASS = "6ac7b34b9e06993e3d49b5600620650d"; // Tools.StringToMD5("I like to complicate");
        public static string KEY = ",lf42l2`cwe2lnrjwiogr32jto3fmkel32tm3kl2gf43m2kl3";
        public static string DEF_CURRENCY = "DEF_CURRENCY";
        public static string SQLITE_DB_EXTENSION = "s3db";

        // NONE = Не авторизован
        // LOCAL = подключён к локальной БД
        // NETWORK = подключён к сетевой БД
        public enum DbMode { NONE, NETWORK };

        private static Tools m_tools = null;

        /// <summary>
        /// Получение рабочей копии Tools.
        /// Для уничтожения единственного экземпляра рабочей копии и выполнения всех сопутствующих действий,
        /// необходимо сделать Tools.instance = null
        /// </summary>
        public static Tools instance
        {
            get
            {
                if (m_tools == null) m_tools = new Tools();
                return m_tools;
            }
            set
            {
                if (value == null)
                {
                    m_tools = null;
                }
            }
        }

        public bool isBoss { 
            get { 
                string[] cl = Environment.GetCommandLineArgs();
            
                foreach (string cs in cl) {
                    if (cs != null && cs.ToLowerInvariant().Contains("boss")) return true;
                }
                
                return false; 
            } 
        }

        private string m_applicationDir = "";
        public string appDir { get { return m_applicationDir; } }

        public string dataDir
        {
            get
            {
                string ret = appDir + "data\\";
                if (!Directory.Exists(ret)) Directory.CreateDirectory(ret);
                return ret;
            }
        }

        public string localDbDir
        {
            get
            {
                string ret = appDir + "local_db\\";
                if (!Directory.Exists(ret)) Directory.CreateDirectory(ret);
                return ret;
            }
        }

        public string localDbConnectionString(string dbname)
        {
            return "Data Source=" + localDbDir + dbname + "." + SQLITE_DB_EXTENSION + ";Password=" + KASSAPASS +
                ";Pooling=false"; //";Pooling=true;Max Pool Size=5;Incr Pool Size=2";
        }

        public string networkDbConnectionString(string host, int port, string user, string pass, string dbname)
        {
            return
                "server=" + host +
                ";port=" + port +
                ";user id=" + user +
                ";Password=" + pass +
                ";Database=" + dbname +
                ";persist security info=True;charset=cp1251" +
                //";Pooling=false"; //";Pooling=true;Max Pool Size=5";        
                ";Pooling=false;Default Command Timeout=28800"; //";Pooling=true;Max Pool Size=5";        
        }

        public User currentUser = null;
        public TableMonitor tmDataChanges;

        public Tools()
        {
            m_applicationDir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
        }

        /*
        ~Tools()
        {
            DisconnectAll();
        }

        private DbMode m_currentMode = DbMode.NONE;

        public DbMode currentMode
        {
            get { return m_currentMode; }
        }

        private MySqlConnection con = null;
        
        public MySqlConnection connection
        {
            get {
                if (con == null || m_currentMode == DbMode.NONE)
                {
                    ConnectNetworkDb(last_dbhost, last_dbport, last_dbname, last_dbuser, last_dbpass);
                }

                if (con == null || m_currentMode != DbMode.NETWORK) MessageBox.Show("Соединение с БД прервано\n(con = " + con + ", m_currentMode = " + m_currentMode + ")");
                return con; 
            }
        }
        
        private string last_dbhost, last_dbname, last_dbuser, last_dbpass;
        private int last_dbport;

        /// <summary>
        /// Соединение с БД MySql
        /// </summary>
        /// <param name="dbhost"></param>
        /// <param name="dbport"></param>
        /// <param name="dbname"></param>
        /// <param name="dbuser"></param>
        /// <param name="dbpass"></param>
        public void ConnectNetworkDb(string dbhost, int dbport, string dbname, string dbuser, string dbpass)
        {
            // Переделать под пул соединений: http://stackoverflow.com/q/5244126/1983575

            m_currentMode = DbMode.NONE;

            DisconnectAll();

            try
            {
                // Соединение с БД
                string conn =
                    "server=" + dbhost +
                    ";port=" + dbport.ToString() +
                    ";user id=" + dbuser +
                    ";Password=" + dbpass +
                    ";persist security info=True;charset=cp1251";

                MySqlConnection mysqlConnection = new MySqlConnection(conn);
                mysqlConnection.Open();
                mysqlConnection.ChangeDatabase(dbname);

                con = mysqlConnection;
                con.StateChange += new StateChangeEventHandler(OnConnectionStateChange);
                m_currentMode = DbMode.NETWORK;
                last_dbhost = dbhost;
                last_dbname = dbname;
                last_dbpass = dbpass;
                last_dbport = dbport;
                last_dbuser = dbuser;
            }
            catch (MySqlException mex)
            {
                DisconnectAll();
                throw new KassaException("Ошибка БД: " + mex.Message);
            }
            catch (Exception ex)
            {
                DisconnectAll();
                throw new KassaException("Исключение. Класс: " + ex.GetType().ToString() + ", Сообщение: " + ex.Message);
            }

        }

        /// <summary>
        /// Закрывает соединения со всеми БД и переводит Tools в состояние DbMode.NONE
        /// </summary>
        public void DisconnectAll()
        {
            try
            {
                con.Close();
            }
            catch (Exception) { }
            con = null;

            currentUser = null;
            m_currentMode = DbMode.NONE;
        }

        protected void OnConnectionStateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed || e.CurrentState == ConnectionState.Broken)
            {
                m_currentMode = DbMode.NONE;                
            }
        }

        /// <summary>
        /// Инициализация сессии.
        /// </summary>
        /// <param name="login">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public void initSession(string login, string password)
        {
            try
            {
                if (m_currentMode == DbMode.NONE) throw new KassaException("Невозможно войти в БД, т.к. с ней не установлено соединение.");

                DataTable dtu = MySqlFillTable(new MySqlCommand("select * from users", connection));

                if (dtu != null && dtu.Rows.Count > 0)
                { // Пользователи есть. Проверим, присутствует ли пользователь с указанным логином и паролем.
                    DataRow[] rusers = dtu.Select(string.Format("login='{0}'", MySqlHelper.EscapeString(login)));
                    bool userFound = false;
                    string md5pass = Crypt.StringToMD5(password);
                    foreach (DataRow ruser in rusers)
                    {
                        if (ruser["pass"].ToString().Equals(md5pass))
                        { // Пользователь найден
                            //TODO Сохранить параметры пользователя в UserData
                            UserPrefs uprefs = new UserPrefs();
                            uprefs.LoadFromXml(ruser["prefs"].ToString());

                            currentUser = new User(
                                int.Parse(ruser["id"].ToString()), bool.Parse(ruser["active"].ToString()), 
                                ruser["login"].ToString(), ruser["pass"].ToString(), uprefs
                                );

                            if (!currentUser.active) throw new KassaException("Указанный пользователь неактивен.");

                            userFound = true;
                            break;
                        }
                    }

                    if (!userFound) throw new KassaException("В выбранной кассе нет пользователя с таким именем или паролем.");
                }
                else
                { // Пользователей нет. Создадим пользователя
                    currentUser = new User(0, true, login, password, new UserPrefs());
                    currentUser.prefs.dicUsers = AccessMode.WRITE;

                    MySqlCommand cmd = new MySqlCommand(
                        "insert into `users` (active, login, pass, prefs) " +
                        "values (@active, @login, @pass, @prefs)", connection
                        );

                    SetDbParameter(cmd, "active", true);
                    SetDbParameter(cmd, "login", login);
                    SetDbParameter(cmd, "pass", Crypt.StringToMD5(password));
                    SetDbParameter(cmd, "prefs", currentUser.prefs.SaveToXml());

                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand("select id from users where login = @login and pass = @pass", connection);
                    SetDbParameter(cmd, "login", login);
                    SetDbParameter(cmd, "pass", Crypt.StringToMD5(password));

                    DataTable dt = MySqlFillTable(cmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        currentUser.PID = int.Parse(dt.Rows[0]["id"].ToString());
                    }

                    // Показать, что был создан пользователь в новой БД
                    MessageBox.Show(string.Format("Поскольку в выбранной БД не было ни одного пользователя, был создан пользователь <{0}>.", login), "Внимание!");
                }

            }
            catch (KassaException kex) { throw kex; }
            catch (Exception) { throw new KassaException("Ошибка инициализации сессии."); }
        }
        */

        /// <summary>
        /// Инициализация сессии (новая).
        /// </summary>
        /// <param name="login">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public void initSession2(string login, string password)
        {
            try
            {
                DbCommand cmd = Db.command("select * from users where login = @login and pass = @pass");
                Db.param(cmd, "login", login);
                Db.param(cmd, "pass", Crypt.StringToMD5(password));

                DataTable dtu = Db.fillTable(cmd);

                if (dtu == null || dtu.Rows.Count < 1) throw new KassaException("Указанный пользователь не найден");
                DataRow ruser = dtu.Rows[0];

                UserPrefs uprefs = new UserPrefs();
                uprefs.LoadFromXml(ruser["prefs"].ToString());

                currentUser = new User(
                    int.Parse(ruser["id"].ToString()), Convert.ToBoolean(ruser["active"]),
                    ruser["login"].ToString(), ruser["pass"].ToString(), uprefs
                    );

                if (!currentUser.active) throw new KassaException("Указанный пользователь неактивен.");


            }
            catch (KassaException kex) { throw kex; }
            catch (Exception) { throw new KassaException("Ошибка инициализации сессии."); }
        }

        /*
        public bool checkConnection(MySqlConnection con, bool silent)
        {
            try
            {
                new MySqlCommand("select count(id) from kassa", con).ExecuteScalar();
                return true;
            }
            catch (Exception) {
                if (!silent) MessageBox.Show("Внимание!\n\nНевозможно соединиться с базой данных!\nПерезапустите приложение.");
            }
            return false;
        }

        public DataTable MySqlFillTable(MySqlCommand cmd)
        {
            DataTable ret = new DataTable();
            using (MySqlDataAdapter ada = new MySqlDataAdapter(cmd))
            {
                ada.Fill(ret);
            }
            return ret;
        }

        public void SetDbParameter(MySqlCommand cmd, string paramName, object paramValue)
        {
            bool oldParam = cmd.Parameters.Contains(paramName);

            MySqlParameter par = oldParam ? cmd.Parameters[paramName] : cmd.CreateParameter();
            par.Value = paramValue;
            if (!oldParam)
            {
                par.ParameterName = paramName;
                cmd.Parameters.Add(par);
            }
        }
        */

        public string DbErrorMsg(Exception ex, string defmsg)
        {
            /*
            try
            {
                if (ex is MySqlException)
                {
                    MySqlException ex2 = (MySqlException)ex;
                    if (ex2.Number == 1451) return "Невозможно удалить запись, так как на неё ссылаются записи в других таблицах";
                }
            }
            catch (Exception ex3) {
                return "Неизвестная ошибка (" + ex3.ToString() + ")";
            }
            */

            return defmsg;
        }

        public bool ValidateRule(OpRuleItem item)
        {
            try
            {
                if (item.RuleType == OpRuleType.FIRM)
                {
//                    item.paramTitle = Convert.ToString(new MySqlCommand("select title from firms where id = " + item.paramId.ToString(), connection).ExecuteScalar());
                    using (DbCommand cmd = Db.command("select title from firms where id = " + item.paramId.ToString()))
                    {
                        item.paramTitle = Convert.ToString(cmd.ExecuteScalar());
                    }
                    return item.paramTitle != string.Empty;
                } else
                if (item.RuleType == OpRuleType.ACCOUNT)
                {
                    /*
                    item.paramTitle = Convert.ToString(new MySqlCommand(
                        "select concat('[', fi.title, '] ', ac.title) as account_title from accounts as ac, firms as fi where ac.firm_id = fi.id and ac.id = " + 
                        item.paramId.ToString(), connection).ExecuteScalar());
                    */

                    string SCONCAT = Db.isMysql ? "concat('[', fi.title, '] ', ac.title)" : "'[' || fi.title || '] ' || ac.title";

                    using(DbCommand cmd = Db.command(
                        "select " + SCONCAT + " as account_title from accounts as ac, firms as fi where ac.firm_id = fi.id and ac.id = " + 
                        item.paramId.ToString()))
                    {
                        item.paramTitle = Convert.ToString(cmd.ExecuteScalar());
                    }

                    return item.paramTitle != string.Empty;
                } else
                if (item.RuleType == OpRuleType.OPERATION)
                {
//                    item.paramTitle = Convert.ToString(new MySqlCommand("select title from ops where id = " + item.paramId.ToString(), connection).ExecuteScalar());
                    using (DbCommand cmd = Db.command("select title from ops where id = " + item.paramId.ToString()))
                    {
                        item.paramTitle = Convert.ToString(cmd.ExecuteScalar());
                    }
                    return item.paramTitle != string.Empty;
                } else
                if (item.RuleType == OpRuleType.CATEGORY)
                {
//                    item.paramTitle = Convert.ToString(new MySqlCommand("select cat_title from client_cat where id = " + item.paramId.ToString(), connection).ExecuteScalar());
                    using (DbCommand cmd = Db.command("select cat_title from client_cat where id = " + item.paramId.ToString()))
                    {
                        item.paramTitle = Convert.ToString(cmd.ExecuteScalar());
                    }
                    return item.paramTitle != string.Empty;
                } else
                if (item.RuleType == OpRuleType.CLIENT)
                {
//                    item.paramTitle = Convert.ToString(new MySqlCommand("select title from client_data where id = " + item.paramId.ToString(), connection).ExecuteScalar());
                    using (DbCommand cmd = Db.command("select title from client_data where id = " + item.paramId.ToString()))
                    {
                        item.paramTitle = Convert.ToString(cmd.ExecuteScalar());
                    }
                    return item.paramTitle != string.Empty;
                }
            }
            catch (Exception) { }

            return false;
        }

        #region Сериализация / Десериализация объектов
        public string SoapSerialize(object obj)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    SoapFormatter sf = new SoapFormatter();
                    sf.Serialize(ms, obj);
                    return Encoding.UTF8.GetString(ms.GetBuffer());
                }
            }
            catch (Exception) { }
            return "";
        }

        public object SoapDeserialize(string src)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(src)))
                {
                    SoapFormatter sf = new SoapFormatter();
                    return sf.Deserialize(ms);
                }
            }
            catch (Exception) { }

            return null;
        }
        #endregion

        #region Утилиты
        /// <summary>
        /// Получение значения DateTime из 20-значного значения, принятого в Кассе3
        /// </summary>
        /// <param name="src">20-значное текстовое значение даты</param>
        /// <returns>Дата</returns>
        public DateTime GetDbDate(string src)
        {
            //YYYYMMDDHHmmSSFFFRRR
            return new DateTime(int.Parse(src.Substring(0, 4)), int.Parse(src.Substring(4, 2)), int.Parse(src.Substring(6, 2)),
                                int.Parse(src.Substring(8, 2)), int.Parse(src.Substring(10, 2)), int.Parse(src.Substring(12, 2)),
                                int.Parse(src.Substring(14, 3))
                                );
        }

        /// <summary>
        /// Получение 20-значного значения даты, принятого в Кассе3 из DateTime
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>20-значное текстовое значение даты</returns>
        public string CreateDbDate(DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssffffff");        
        }

        public string CreateDbDate()
        {
            return CreateDbDate(DateTime.Now);
        }

        public void MarkRecordDeleted(int pid, string table)
        {
            //TODO Если происходит работа в локальной БД, указанная запись добавляется в таблицу удалений.
        }

        public string makeWebRequest(string url, Encoding pageEnc)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, pageEnc);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        public string updateCurrencyValues(DateTime reqDate, bool overwriteDbValues) 
        {
            try
            {

                string sReqDate = reqDate.ToString("dd/MM/yyyy");
                string request = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + sReqDate;
                SimpleXML xml = SimpleXML.LoadXml(makeWebRequest(request, Encoding.GetEncoding(1251)));

                Dictionary<string, int> dCur = new Dictionary<string, int>();
                //DataTable dt = MySqlFillTable(new MySqlCommand("select id, curr_id from curr_list", connection));

                using (DataTable dt = Db.fillTable(Db.command("select id, curr_id from curr_list")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        try
                        {
                            dCur[r["curr_id"].ToString()] = int.Parse(r["id"].ToString());
                        }
                        catch (Exception) { }
                    }
                }

//                DataTable dtcurr = MySqlFillTable(new MySqlCommand("select curr_id, code from curr_list where code = ''", connection));
                using (DataTable dtcurr = Db.fillTable(Db.command("select curr_id, code from curr_list where code = ''")))
                {
                    /*
                    using (MySqlDataAdapter ada = new MySqlDataAdapter("select cv.*, cl.curr_id from curr_values as cv, curr_list as cl where cv.date = '" +
                        reqDate.ToString("yyyyMMdd") + "' and cv.currlist_id = cl.id", connection))
                    {
                        dt = new DataTable();
                        ada.Fill(dt);
                    */

                    using(DataTable dt = Db.fillTable(Db.command(
                        "select cv.*, cl.curr_id from curr_values as cv, curr_list as cl where cv.date = '" +
                        reqDate.ToString("yyyyMMdd") + "' and cv.currlist_id = cl.id")))
                    {
/*
                        MySqlCommand cmdUpdate = new MySqlCommand("update curr_values set value = @value where currlist_id = @currlist_id and curr_values.date = @date", connection);
                        MySqlCommand cmdInsert = new MySqlCommand("insert into curr_values (currlist_id, curr_values.date, value) values (@currlist_id, @date, @value)", connection);
                        MySqlCommand cmdUpdateCode = new MySqlCommand("update curr_list set code = @code where curr_id = @curr_id", connection);
*/
                        DbCommand cmdUpdate = Db.command("update curr_values set value = @value where currlist_id = @currlist_id and curr_values.date = @date");
                        DbCommand cmdInsert = Db.command("insert into curr_values (currlist_id, curr_values.date, value) values (@currlist_id, @date, @value)");
                        DbCommand cmdUpdateCode = Db.command("update curr_list set code = @code where curr_id = @curr_id");

                        ArrayList alValute = xml.GetChildren("Valute");
                        foreach (SimpleXML xmlValute in alValute)
                        {
                            try
                            {
                                string valId = xmlValute.Attributes["ID"];
                                string sValValue = xmlValute["Value"].Text.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                                float valValue = float.Parse(sValValue) / int.Parse(xmlValute["Nominal"].Text);
                                string charcode = xmlValute["CharCode"].Text;

//                                MySqlCommand cmde = null;
                                DbCommand cmde = null;

//                                DataRow[] sv = dt.Select("curr_id = '" + MySqlHelper.EscapeString(valId) + "' and date = '" + reqDate.ToString("yyyyMMdd") + "'");
                                DataRow[] sv = dt.Select("curr_id = '" + Db.escape(valId) + "' and date = '" + reqDate.ToString("yyyyMMdd") + "'");
                                if (sv != null && sv.Length > 0)
                                {
                                    if (overwriteDbValues)
                                    {
                                        cmde = cmdUpdate;
                                    }
                                }
                                else
                                {
                                    cmde = cmdInsert;
                                }

                                if (cmde != null)
                                {
                                    /*
                                    SetDbParameter(cmde, "currlist_id", dCur[valId]);
                                    SetDbParameter(cmde, "date", reqDate.ToString("yyyyMMdd"));
                                    SetDbParameter(cmde, "value", valValue);
                                    */

                                    Db.param(cmde, "currlist_id", dCur[valId]);
                                    Db.param(cmde, "date", reqDate.ToString("yyyyMMdd"));
                                    Db.param(cmde, "value", valValue);

                                    cmde.ExecuteNonQuery();
                                }

//                                sv = dtcurr.Select("curr_id = '" + MySqlHelper.EscapeString(valId) + "'");
                                sv = dtcurr.Select("curr_id = '" + Db.escape(valId) + "'");
                                if (sv != null && sv.Length > 0)
                                {
                                    if (!sv[0]["code"].Equals(charcode))
                                    {
                                        sv[0]["code"] = charcode;
                                        /*
                                        SetDbParameter(cmdUpdateCode, "code", charcode);
                                        SetDbParameter(cmdUpdateCode, "curr_id", valId);
                                         */
                                        Db.param(cmdUpdateCode, "code", charcode);
                                        Db.param(cmdUpdateCode, "curr_id", valId);
                                        cmdUpdateCode.ExecuteNonQuery();
                                    }
                                }
                            }

                            catch (Exception) { }
                        }
                    }
                }
/*
                <ValCurs Date="04/02/2011" name="Foreign Currency Market">
                    <Valute ID="R01010">
	                    <NumCode>036</NumCode>
	                    <CharCode>AUD</CharCode>
	                    <Nominal>1</Nominal>
	                    <Name>Австралийский доллар</Name>
	                    <Value>29,7392</Value>
                    </Valute>
*/

            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
            return "";
        }

        /// <summary>
        /// Смотрит, имеется ли в таблице валют позиция "Российский рубль" и, если такой позиции нет, она создаётся.
        /// Эта позиция необходима для возможности создания рублёвых счетов.
        /// </summary>
        public void fixCurrencyRouble()
        {
            bool needCreate = true;

            /*
            MySqlCommand cmd = new MySqlCommand("select count(id) cid from curr_list where curr_id = @curr_id", connection);
            SetDbParameter(cmd, "curr_id", DEF_CURRENCY);
             */
            try
            {
                using (DbCommand cmd = Db.command("select count(id) cid from curr_list where curr_id = @curr_id"))
                {
                    Db.param(cmd, "curr_id", DEF_CURRENCY);
                    int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    needCreate = cnt == 0;
                }
            }
            catch (Exception) { }

            if (needCreate)
            {
                /*
                cmd = new MySqlCommand("insert into curr_list (curr_id, name, active, code) values (@curr_id, @curr_name, 1, @curr_code)", connection);
                SetDbParameter(cmd, "curr_id", DEF_CURRENCY);
                SetDbParameter(cmd, "curr_name", "Российский рубль");
                SetDbParameter(cmd, "curr_code", "RUR");
                cmd.ExecuteNonQuery();
                */

                long curr_id;

                using (DbCommand cmd = Db.command("insert into curr_list (curr_id, name, active, code) values (@curr_id, @curr_name, 1, @curr_code)"))
                {
                    Db.param(cmd, "curr_id", DEF_CURRENCY);
                    Db.param(cmd, "curr_name", "Российский рубль");
                    Db.param(cmd, "curr_code", "RUR");
                    cmd.ExecuteNonQuery();
                    curr_id = Db.LastInsertedId(cmd, "curr_list");
                }

                /*
                long curr_id = cmd.LastInsertedId;
                cmd = new MySqlCommand("insert into curr_values (currlist_id, value, date) values (@currlist_id, @value, @date)", connection);
                SetDbParameter(cmd, "currlist_id", curr_id);
                SetDbParameter(cmd, "value", 1.0f);
                SetDbParameter(cmd, "date", "19700101");
                cmd.ExecuteNonQuery();
                 */

                using (DbCommand cmd = Db.command("insert into curr_values (currlist_id, value, date) values (@currlist_id, @value, @date)"))
                {
                    Db.param(cmd, "currlist_id", curr_id);
                    Db.param(cmd, "value", 1.0f);
                    Db.param(cmd, "date", "19700101");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SimpleXML saveBandedGridState(BandedGridView bgv)
        {
            SimpleXML ret = new SimpleXML("BandedGridView");
            try
            {
                foreach (BandedGridColumn bgc in bgv.Columns)
                {
                    SimpleXML node = ret.CreateChild("Column");
                    node.Attributes["AbsoluteIndex"] = Convert.ToString(bgc.AbsoluteIndex);
                    node.Attributes["ColVIndex"] = Convert.ToString(bgc.ColVIndex);
                    node.Attributes["Name"] = bgc.Name;
                    node.Attributes["OwnerBand_Name"] = bgc.OwnerBand.Name;
                    node.Attributes["SortOrder"] = Convert.ToString((int)bgc.SortOrder);
//                    node.Attributes["Visible"] = Convert.ToString(bgc.Visible);
                    node.Attributes["VisibleIndex"] = Convert.ToString(bgc.VisibleIndex);
                    node.Attributes["Width"] = Convert.ToString(bgc.Width);
                }

                foreach (GridBand gb in bgv.Bands)
                {
                    SimpleXML node = ret.CreateChild("Band");
                    node.Attributes["Name"] = gb.Name;
//                    node.Attributes["Visible"] = Convert.ToString(gb.Visible);
                    node.Attributes["Width"] = Convert.ToString(gb.Width);
                }

            }
            catch (Exception) { }

            return ret;
        }

        public void loadBandedGridState(BandedGridView bgv, SimpleXML xml)
        {
            try
            {
                bgv.GridControl.SuspendLayout();
                ArrayList alBands = xml.GetChildren("Band");
                foreach (SimpleXML item in alBands)
                {
                    try
                    {
                        GridBand gb = bgv.Bands[item.Attributes["Name"]];
//                        gb.Visible = Convert.ToBoolean(item.Attributes["Visible"]);
                        gb.Width = Convert.ToInt32(item.Attributes["Width"]);
                    }
                    catch (Exception) { }
                }

                ArrayList alColumns = xml.GetChildren("Column");
                foreach (SimpleXML item in alColumns)
                {
                    try
                    {
                        BandedGridColumn bgc = bgv.Columns[item.Attributes["Name"]];
                        bgc.AbsoluteIndex = Convert.ToInt32(item.Attributes["AbsoluteIndex"]);
                        bgc.ColVIndex = Convert.ToInt32(item.Attributes["ColVIndex"]);
                        bgc.SortOrder = (ColumnSortOrder)Convert.ToInt32(item.Attributes["SortOrder"]);
//                        bgc.Visible = Convert.ToBoolean(item.Attributes["Visible"]);
                        bgc.VisibleIndex = Convert.ToInt32(item.Attributes["VisibleIndex"]);
                        bgc.Width = Convert.ToInt32(item.Attributes["Width"]);

                        GridBand gb = bgv.Bands[item.Attributes["OwnerBand_Name"]];
                        bgc.OwnerBand = gb;
                    }
                    catch (Exception) { }
                }

                bgv.GridControl.ResumeLayout();
            }
            catch (Exception) { }
        }

        public static string StringToMD5(string source)
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

    }

    /// <summary>
    /// Общее исключение. Выскакивает во всех случаях, не относящихся к специальным.
    /// </summary>
    public class KassaException : Exception
    {
        public KassaException(string message) : base(message) { }
    }

    /// <summary>
    /// Класс для сохранения формы в хмл
    /// </summary>
    [Serializable]
    public class FormState
    {
        public Size size;
        public Point location;
        public FormWindowState windowState;
        public Dictionary<string, string> values;
        bool empty = true;

        public FormState(Form src)
        {
            windowState = src.WindowState;
            src.WindowState = FormWindowState.Normal;
            location = src.Location;
            size = src.Size;
            src.WindowState = windowState;
            values = new Dictionary<string, string>();
            empty = false;
        }

        public FormState(string fn)
        {
            values = new Dictionary<string, string>();
            try
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(fn, Encoding.UTF8));
                int iws = (int)FormWindowState.Normal;
                if (int.TryParse(xml["WindowState"].Text, out iws)) windowState = (FormWindowState)iws;
                location = new Point(int.Parse(xml["Location"].Attributes["x"]), int.Parse(xml["Location"].Attributes["y"]));
                size = new Size(int.Parse(xml["Size"].Attributes["width"]), int.Parse(xml["Size"].Attributes["height"]));
                if (windowState == FormWindowState.Minimized) windowState = FormWindowState.Maximized;                    

                SimpleXML xmlv = xml.GetNodeByPath("Values", false);
                if (xmlv != null)
                {
                    ArrayList alv = xmlv.GetChildren("Value");
                    foreach (SimpleXML vi in alv)
                    {
                        if (vi.Attributes.ContainsKey("name"))
                        {
                            values[vi.Attributes["name"]] = vi.Text;
                        }
                    }
                }
                empty = false;
            }
            catch (Exception)
            {
                empty = true;
            }
        }

        public void SaveValue(string key, object value)
        {
            if (value is CheckBox)
            {
                values[key] = ((CheckBox)value).Checked.ToString();
            }
            else if (value is TabControl)
            {
                values[key] = ((TabControl)value).SelectedIndex.ToString();
            }
            else if (value is DateEdit)
            {
                values[key] = ((DateEdit)value).Text;
            }
            else if (value is NumericUpDown)
            {
                values[key] = ((NumericUpDown)value).Value.ToString();
            }
            else if (value is Panel)
            {
                values[key] = ((Panel)value).Width.ToString();
            }
            else
            {
                values[key] = value.ToString();
            }
        }

        public bool LoadValue(string key, object value)
        {
            if (!values.ContainsKey(key)) return false;

            try
            {
                if (value is CheckBox)
                {
                    ((CheckBox)value).Checked = bool.Parse(values[key]);
                    return true;
                }
                else if (value is TabControl)
                {
                    ((TabControl)value).SelectedIndex = int.Parse(values[key]);
                }
                else if (value is DateEdit)
                {
                    ((DateEdit)value).Text = values[key];
                }
                else if (value is NumericUpDown)
                {
                    ((NumericUpDown)value).Value = Convert.ToDecimal(values[key]);
                }
                else if (value is Panel)
                {
                    ((Panel)value).Width = int.Parse(values[key]);
                }
            }
            catch (Exception) { }
            return false;
        }

        public string getValue(string key, string defValue)
        {
            try
            {
                return values[key];
            }
            catch (Exception) { }

            return defValue;
        }

        public void ApplyToForm(Form dest) 
        {
            if (!empty)
            {
                dest.Size = size;
                dest.Location = location;
                dest.WindowState = windowState;
            }
        }

        public void UpdateFromForm(Form src)
        {
            if (windowState != FormWindowState.Minimized) size = src.Size;
            location = src.Location;
            windowState = src.WindowState;
            //if (windowState == FormWindowState.Minimized) windowState = FormWindowState.Normal;
            empty = false;
        }

        public void SaveToFile(string fn)
        {
            try
            {
                File.Delete(fn);
            }
            catch (Exception) { }

            try
            {
                SimpleXML xml = new SimpleXML("Form");
                xml["WindowState"].Text = ((int)windowState).ToString();
                SimpleXML loc = xml["Location"];
                loc.Attributes["x"] = location.X.ToString();
                loc.Attributes["y"] = location.Y.ToString();
                SimpleXML sz = xml["Size"];
                sz.Attributes["width"] = size.Width.ToString();
                sz.Attributes["height"] = size.Height.ToString();

                if (values != null && values.Count > 0)
                {
                    SimpleXML xmlv = xml.CreateChild("Values");
                    foreach (KeyValuePair<string, string> kvp in values)
                    {
                        SimpleXML iv = xmlv.CreateChild("Value");
                        iv.Attributes["name"] = kvp.Key;
                        iv.Text = kvp.Value;
                    }
                }

                File.WriteAllText(fn, SimpleXML.SaveXml(xml), Encoding.UTF8);

            }
            catch (Exception) { }
        }
    }

    public interface IStrColor
    {
        Color getColor();
    }

    public class ColorString : IStrColor
    {
        string str;
        Color color;

        public ColorString(string str, Color color)
        {
            this.str = str;
            this.color = color;
        }

        public Color getColor()
        {
            return color;
        }

        public override string ToString()
        {
            return str;
        }
    }

}
