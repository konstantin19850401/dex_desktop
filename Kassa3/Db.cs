using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Kassa3
{
    class Db
    {
        // http://www.codeproject.com/Articles/17768/ADO-NET-Connection-Pooling-at-a-Glance
        // https://msdn.microsoft.com/ru-ru/library/dd0w4a2z(v=vs.110).aspx
        // http://stackoverflow.com/questions/7354116/net-mysql-connector-conflicting-dbproviderfactories
        
        // https://www.sqlite.org/datatype3.html


        ///////////////////////////////////////////////////////////////////////////////////////////////////


        public static string DBTYPE_MYSQL = "MySql.Data.MySqlClient";
        public static string DBTYPE_SQLITE = "System.Data.SQLite";

        private static string connStr;
        private static string _dbType;

        private static DbProviderFactory factory;
        private static DbConnection _connection = null;

        private static bool _isMysql = false;

        public static void init(string dbType, string connStr)
        {
            _dbType = dbType;
            _isMysql = _dbType == DBTYPE_MYSQL;
            Db.connStr = connStr;
            factory = DbProviderFactories.GetFactory(_dbType);
        }

        public string dbType
        {
            get
            {
                return _dbType;
            }
        }

        public static bool isMysql
        {
            get
            {
                return _isMysql;
            }
        }

        
        public static DbConnection connection {
            get
            {
                if (/*_dbType == DBTYPE_MYSQL ||*/ _connection == null)
                {
                    _connection = factory.CreateConnection();
                    _connection.ConnectionString = connStr;
                    _connection.Open();
                }

                return _connection;
            }
        }

        public static void closeConnection()
        {
            if (_connection != null)
            {
                try
                {
                    _connection.Close();
                }
                catch (Exception) { }
                
                _connection = null;
            }
        }
        

        /*
        public static DbConnection connection
        {
            get
            {
                DbConnection ret = factory.CreateConnection();
                ret.ConnectionString = connStr;
                ret.Open();
                return ret;
            }
        }
        */

        public static DbDataAdapter dataAdapter()
        {
            return factory.CreateDataAdapter();
        }

        public static DbCommand command(DbConnection cn, string commandText)
        {
            DbCommand ret = cn.CreateCommand();
            ret.CommandText = commandText;
            return ret;
        }

        public static DbCommand command(string commandText)
        {
            return command(connection, commandText);
        }

        public static DataTable fillTable(DbCommand cmd)
        {
            DataTable ret = new DataTable();

            try
            {
                using ( DbDataAdapter ada = factory.CreateDataAdapter() )
                {
                    ada.SelectCommand = cmd;
                    ada.Fill(ret);
                }
            } catch( Exception ) {
            }

            return ret;
        }

        public static void param(DbCommand cmd, string paramName, object paramValue)
        {
            bool oldParam = cmd.Parameters.Contains(paramName);

            DbParameter par = oldParam ? cmd.Parameters[paramName] : cmd.CreateParameter();
            par.Value = paramValue;
            if (!oldParam)
            {
                par.ParameterName = paramName;
                cmd.Parameters.Add(par);
            }
        }

        public static string escape(string src)
        {
            return MySqlHelper.EscapeString(src);
        }

        public static long LastInsertedId(DbCommand cmd, string tableName)
        {
            //TODO Тестировать!
            bool isMysql = _dbType == DBTYPE_MYSQL;

            cmd.CommandText = isMysql ?
            "select last_insert_id()" :
            "select last_insert_rowid()";

            long rowid = Convert.ToInt64(cmd.ExecuteScalar());

            if (isMysql) return rowid;

            cmd.CommandText = "select id from " + tableName + " where ROWID = " + rowid;
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public static void createSqliteDb(string connStr, string dbUser, string dbPass, string dbTitle)
        {
            DbConnection cn = DbProviderFactories.GetFactory(DBTYPE_SQLITE).CreateConnection();
            cn.ConnectionString = connStr;
            cn.Open();

            command(cn, "CREATE TABLE accounts (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "firm_id INTEGER NOT NULL," +
                        "curr_id INTEGER NOT NULL," +
                        "title TEXT NOT NULL," +
                        "shortcut TEXT NOT NULL," +
                        "banktag TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX accounts_id ON accounts (id);" +
                        "CREATE INDEX accounts_firm_id ON accounts (firm_id);" +
                        "CREATE INDEX accounts_curr_id ON accounts (curr_id);" +
                        "CREATE INDEX accounts_title ON accounts (title);" +
                        "CREATE INDEX accounts_shortcut ON accounts (shortcut);" +

                        "CREATE TABLE changes (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "title TEXT NOT NULL," +
                        "stamp INTEGER NOT NULL" +
                        ");" +
                        "CREATE INDEX changes_id ON changes (id);" +

                        "CREATE TABLE client_cat (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "parent_id INTEGER NOT NULL," +
                        "cat_title TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX client_cat_id ON client_cat (id);" +
                        "CREATE INDEX client_cat_parent_id ON client_cat (parent_id);" +
                        "CREATE INDEX client_cat_cat_title ON client_cat (cat_title);" +

                        "CREATE TABLE client_data (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "cat_id INTEGER NOT NULL," +
                        "title TEXT NOT NULL," +
                        "shortcut TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX client_data_id ON client_data (id);" +
                        "CREATE INDEX client_data_cat_id ON client_data (cat_id);" +
                        "CREATE INDEX client_data_title ON client_data (title);" +
                        "CREATE INDEX client_data_shortcut ON client_data (shortcut);" +

                        "CREATE TABLE curr_list (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "curr_id TEXT NOT NULL," +
                        "name TEXT NOT NULL," +
                        "active INTEGER NOT NULL," +
                        "code TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX curr_list_id ON curr_list (id);" +
                        "CREATE INDEX curr_list_curr_id ON curr_list (curr_id);" +
                        "CREATE INDEX curr_list_name ON curr_list (name);" +
                        "CREATE INDEX curr_list_active ON curr_list (active);" +
                        "CREATE INDEX curr_list_code ON curr_list (code);" +

                        "CREATE TABLE curr_values (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "currlist_id INTEGER NOT NULL," +
                        "value REAL NOT NULL," +
                        "date TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX curr_values_id ON curr_values (id);" +
                        "CREATE INDEX curr_values_currlist_id ON curr_values (currlist_id);" +
                        "CREATE INDEX curr_values_value ON curr_values (value);" +
                        "CREATE INDEX curr_values_date ON curr_values (date);" +

                        "CREATE TABLE firms (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "title TEXT NOT NULL," +
                        "shortcut TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX firms_id ON firms (id);" +
                        "CREATE INDEX firms_title ON firms (title);" +
                        "CREATE INDEX firms_shortcut ON firms (shortcut);" +

                        "CREATE TABLE import_matches (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "rule_id INTEGER NOT NULL," +
                        "field TEXT NOT NULL," +
                        "operation INTEGER NOT NULL," +
                        "match_value TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX import_matches_id ON import_matches (id);" +

                        "CREATE TABLE import_rules (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "protocol TEXT NOT NULL," +
                        "title TEXT NOT NULL," +
                        "op_id INTEGER NOT NULL," +
                        "r_prim TEXT NOT NULL," +
                        "srctype INTEGER NOT NULL," +
                        "src_acc_id INTEGER NOT NULL," +
                        "src_client_id INTEGER NOT NULL," +
                        "dsttype INTEGER NOT NULL," +
                        "dst_acc_id INTEGER NOT NULL," +
                        "dst_client_id INTEGER NOT NULL," +
                        "status INTEGER NOT NULL" +
                        ");" +
                        "CREATE INDEX import_rules_id ON import_rules (id);" +
                        "CREATE INDEX import_rules_protocol ON import_rules (protocol);" +


                        "CREATE TABLE journal (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "op_id INTEGER NOT NULL," +
                        "r_date TEXT NOT NULL," +
                        "r_sum REAL NOT NULL," +
                        "r_prim TEXT NOT NULL," +
                        "srctype INTEGER NOT NULL," +
                        "src_acc_id INTEGER," +
                        "src_client_id INTEGER," +
                        "src_curr_value REAL," +
                        "dsttype INTEGER NOT NULL," +
                        "dst_acc_id INTEGER," +
                        "dst_client_id INTEGER," +
                        "dst_curr_value REAL," +
                        "user_cr INTEGER NOT NULL," +
                        "user_ch INTEGER," +
                        "locker_id INTEGER," +
                        "lock_till TEXT," +
                        "foreign_id TEXT NOT NULL DEFAULT ''," +
                        "deleted INTEGER NOT NULL DEFAULT 0" +
                        ");" +
                        "CREATE INDEX journal_id ON journal (id);" +
                        "CREATE INDEX journal_op_id ON journal (op_id);" +
                        "CREATE INDEX journal_r_date ON journal (r_date);" +
                        "CREATE INDEX journal_r_sum ON journal (r_sum);" +
                        "CREATE INDEX journal_r_prim ON journal (r_prim);" +
                        "CREATE INDEX journal_srctype ON journal (srctype);" +
                        "CREATE INDEX journal_src_acc_id ON journal (src_acc_id);" +
                        "CREATE INDEX journal_src_client_id ON journal (src_client_id);" +
                        "CREATE INDEX journal_src_curr_value ON journal (src_curr_value);" +
                        "CREATE INDEX journal_dsttype ON journal (dsttype);" +
                        "CREATE INDEX journal_dst_acc_id ON journal (dst_acc_id);" +
                        "CREATE INDEX journal_dst_client_id ON journal (dst_client_id);" +
                        "CREATE INDEX journal_dst_curr_value ON journal (dst_curr_value);" +
                        "CREATE INDEX journal_user_cr ON journal (user_cr);" +
                        "CREATE INDEX journal_user_ch ON journal (user_ch);" +
                        "CREATE INDEX journal_locker_id ON journal (locker_id);" +
                        "CREATE INDEX journal_lock_till ON journal (lock_till);" +
                        "CREATE INDEX journal_foreign_id ON journal (foreign_id);" +
                        "CREATE INDEX journal_deleted ON journal (deleted);" +

                        "CREATE TABLE kassa (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "title TEXT NOT NULL," +
                        "signature TEXT NOT NULL," +
                        "legal_date TEXT NOT NULL DEFAULT '19700207'," +
                        "db_version INTEGER NOT NULL DEFAULT 1" +
                        ");" +

                        "CREATE TABLE new_recs (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "user_id INTEGER NOT NULL," +
                        "rec_id INTEGER NOT NULL" +
                        ");" +
                        "CREATE INDEX new_recs_id ON new_recs (id);" +
                        "CREATE INDEX new_recs_user_id ON new_recs (user_id);" +
                        "CREATE INDEX new_recs_rec_id ON new_recs (rec_id);" +

                        "CREATE TABLE ops (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "title TEXT NOT NULL," +
                        "shortcut TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX ops_id ON ops (id);" +
                        "CREATE INDEX ops_title ON ops (title);" +
                        "CREATE INDEX ops_shortcut ON ops (shortcut);" +

                        "CREATE TABLE reclog (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "op_date TEXT NOT NULL," +
                        "op_user INTEGER NOT NULL," +
                        "op_kind INTEGER NOT NULL," +
                        "op_info TEXT NOT NULL" +
                        ");" +
                        "CREATE INDEX reclog_id ON reclog (id);" +

                        "CREATE TABLE rules (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "priority INTEGER NOT NULL," +
                        "permit INTEGER NOT NULL," +
                        "firm_id INTEGER," +
                        "acc_id INTEGER," +
                        "op_id INTEGER," +
                        "user_id INTEGER," +
                        "cat_id INTEGER," +
                        "client_id INTEGER" +
                        ");" +
                        "CREATE INDEX rules_id ON rules (id);" +
                        "CREATE INDEX rules_priority ON rules (priority);" +
                        "CREATE INDEX rules_permit ON rules (permit);" +
                        "CREATE INDEX rules_firm_id ON rules (firm_id);" +
                        "CREATE INDEX rules_acc_id ON rules (acc_id);" +
                        "CREATE INDEX rules_op_id ON rules (op_id);" +
                        "CREATE INDEX rules_user_id ON rules (user_id);" +
                        "CREATE INDEX rules_cat_id ON rules (cat_id);" +
                        "CREATE INDEX rules_client_id ON rules (client_id);" +

                        "CREATE TABLE users (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "active INTEGER NOT NULL DEFAULT 0," +
                        "login TEXT NOT NULL," +
                        "pass TEXT NOT NULL," +
                        "prefs TEXT NOT NULL," +
                        "mark_new INTEGER NOT NULL DEFAULT 0" +
                        ");" +
                        "CREATE INDEX users_id ON users (id);" +
                        "CREATE INDEX users_active ON users (active);" +
                        "CREATE INDEX users_login ON users (login);" +
                        "CREATE INDEX users_pass ON users (pass);"

                        ).ExecuteNonQuery();

            UserPrefs uprefs = new UserPrefs();
            uprefs.appSettings = SimplePermission.PERMIT;
            uprefs.dicClients = AccessMode.WRITE;
            uprefs.dicCurrency = AccessMode.WRITE;
            uprefs.dicFirmAcc = AccessMode.WRITE;
            uprefs.dicOps = AccessMode.WRITE;
            uprefs.dicUsers = AccessMode.WRITE;
            uprefs.fieldsEdit = SimplePermission.PERMIT;
            uprefs.globalRule = SimplePermission.PERMIT;

            DbCommand cmd = command(cn, 
                "insert into users (active, login, pass, prefs, mark_new) " +
                "values (1, @user, @pass, @prefs, 1)"
                );

            param(cmd, "prefs", uprefs.SaveToXml());
            param(cmd, "user", dbUser);
            param(cmd, "pass", Crypt.StringToMD5(dbPass));

            cmd.ExecuteNonQuery();

            cmd = command(cn, "insert into kassa (title, signature) values (@title, @signature)");
            param(cmd, "title", dbTitle);
            param(cmd, "signature", System.Guid.NewGuid().ToString());
            cmd.ExecuteNonQuery();

            cn.Close();
        }
    }
}
