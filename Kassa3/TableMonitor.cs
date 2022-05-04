using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public class TableMonitor
    {
        public string tableName;
        Int64 chc = -1;

        public TableMonitor(string tableName)
        {
            this.tableName = tableName;
            ensureRecord();
        }

        public bool checkTableChanged()
        {
            Int64 newChc = chc;
            try
            {
                /*
                string sql = "select stamp from `changes` where title = '" + MySqlHelper.EscapeString(tableName) + "'";
                object o = new MySqlCommand(sql, Tools.instance.connection).ExecuteScalar();
                */

                using (DbCommand cmd = Db.command("select stamp from `changes` where title = '" + Db.escape(tableName) + "'"))
                {
                    object o = cmd.ExecuteScalar();

                    if (o == null)
                    {
                        // Нет такой записи
                        newChc = 0;
                        ensureRecord();
                    }
                    else
                    {
                        newChc = Convert.ToInt64(o);
                    }
                }
            }
            catch (Exception) { }
            bool tableChanged = newChc != chc;
            chc = newChc;
            return tableChanged;
        }

        public void markTableChanged()
        {
            ensureRecord();
            try
            {
                /*
                string sql = "update `changes` set stamp = stamp + 1 where title = '" + MySqlHelper.EscapeString(tableName) + "'";
                new MySqlCommand(sql, Tools.instance.connection).ExecuteNonQuery();
                 */
                using (DbCommand cmd = Db.command("update `changes` set stamp = stamp + 1 where title = '" + Db.escape(tableName) + "'"))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { }
        }

        void ensureRecord()
        {
            try
            {
                /*
                string sql = "select count(id) from `changes` where title = '" + MySqlHelper.EscapeString(tableName) + "'";
                object o = new MySqlCommand(sql, Tools.instance.connection).ExecuteScalar();
                 */

                using (DbCommand cmd = Db.command("select count(id) from `changes` where title = '" + Db.escape(tableName) + "'"))
                {
                    object o = cmd.ExecuteScalar();
                    if (o == null || Convert.ToInt32(o) == 0)
                    {
                        /*
                        sql = "insert into `changes` (title, stamp) values ('" + MySqlHelper.EscapeString(tableName) + "', 1)";
                        new MySqlCommand(sql, Tools.instance.connection).ExecuteNonQuery();
                         */
                        using (DbCommand cmd2 = Db.command("insert into `changes` (title, stamp) values ('" + Db.escape(tableName) + "', 1)"))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception) { }
        }

    }
}
