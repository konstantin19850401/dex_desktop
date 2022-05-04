using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Data;
using Newtonsoft.Json.Linq;
using DEXExtendLib;
using System.IO.Compression;
using System.Web;

namespace dexol
{
    class DexolSession
    {
        private static DexolSession m_inst = null;

        public string sid = null;

        public string user_props = null;
        public string user_name = null;
        public string profile_code = null;

        public JObject lastJsonCall = null;

        string lastExceptionMsg = null;

        public string currentDb = null;

        public string DEXOL_URL = "http://192.168.2.50/dexol/?"; // Просто чтобы не было пусто. Значение берётся на окне логина.
        string[] param_delim = new string[2] { "=", "&" };

        private DexolSession()
        {
            sid = null;
        }

        public static DexolSession inst()
        {
            if (m_inst == null) m_inst = new DexolSession();
            return m_inst;
        }

        public bool isLoggedIn()
        {
            return sid != null;
        }

        /*
        private static byte[] GZipUncompress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var gzip = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }*/

        public JObject jsonHttpRequest(params string[] args)
        {
            return jsonHttpRequest(true, args);
        }

        public JObject jsonHttpRequest(bool doCompress, params string[] args)
        {
            lastExceptionMsg = null;
            string url = "";
            try
            {
                //для мсфон поставим без компрессии
                doCompress = false;

                url = DEXOL_URL + "gzip=" + (doCompress ? "y" : "n");
                string postdata = "";
                string rmethod = "GET";

                int dlm = 0;
                bool doPost = false;
                if (args != null && args.Length > 0)
                {
                    foreach (string arg in args)
                    {
                        if (dlm % 2 == 0)
                        {
                            if (arg.StartsWith("POST:"))
                            {
                                doPost = true;
                                rmethod = "POST";
                                if (!"".Equals(postdata)) postdata += @"&";
                                postdata += HttpUtility.UrlEncode(arg.Substring(5).Trim()) + "=";
                            }
                            else
                            {
                                doPost = false;
                                url += @"&" + arg + "=";
                            }
                        }
                        else
                        {
                            if (doPost)
                            {
                                postdata += HttpUtility.UrlEncode(arg);
                            }
                            else
                            {
                                url += arg;
                            }
                        }
                        dlm = 1 - dlm;
                    }
                }

                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = rmethod;

                if ("POST".Equals(rmethod))
                {
                    request.ContentType = "application/x-www-form-urlencoded";

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] byte1 = encoding.GetBytes(postdata);
                    request.ContentLength = byte1.Length;
                    request.GetRequestStream().Write(byte1, 0, byte1.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                Console.WriteLine("response.ContentLength: {0}", response.ContentLength);

                Stream responseStream = response.GetResponseStream();

                Stream st;
                if (doCompress) st = new GZipStream(responseStream, CompressionMode.Decompress);
                else 
                    st = responseStream;
                
                string ts = null;
                int count = 0;

//                int blen = 0;

                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
//                        blen += count;
                        ts = Encoding.ASCII.GetString(buf, 0, count);
                        sb.Append(ts);
                    }
                } while (count > 0);
                string str = sb.ToString();
                st.Close();

//                Console.WriteLine("blen: {0}", blen);
//                Console.WriteLine("request: " + url);
//                Console.WriteLine("postdata: " + postdata);
//                Console.WriteLine("reply: " + str);

                return JObject.Parse(str);

            }
            catch (Exception ex) {
                lastExceptionMsg = ex.Message;
//                File.WriteAllText("last_request_url.txt", url, Encoding.UTF8);
            }

            return null;
        }

        public bool login(string user, string pass)
        {
            lastJsonCall = jsonHttpRequest("login", user, "pass", DEXToolBox.getToolBox().StringToMD5(pass));
            if (lastJsonCall != null)
            {
                if ((int)lastJsonCall["result"] == 0)
                {
                    sid = lastJsonCall["sid"].ToString();
                    user_props = lastJsonCall["user_props"].ToString();
                    user_name = lastJsonCall["user_name"].ToString();
                    profile_code = lastJsonCall["profile_code"].ToString();
                    return true;
                }
            }
            sid = null;
            return false;
        }

        public void logout()
        {
            if (isLoggedIn())
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "logout");
                sid = null;
            }
        }

        public bool ping()
        {
            if (!isLoggedIn()) return false;

            lastJsonCall = jsonHttpRequest("sid", sid, "f", "ping");
            if (lastJsonCall != null)
            {
                if ((int)lastJsonCall["result"] == 0)
                {
                    return true;
                }
            }
            sid = null;
            return false;
        }

        public Dictionary<string, StringDbItem> dblist()
        {
            lastJsonCall = jsonHttpRequest("sid", sid, "f", "dblist");
            if (lastJsonCall != null)
            {
                if ((int)lastJsonCall["result"] == 0)
                {
                    Dictionary<string, StringDbItem> ret = new Dictionary<string, StringDbItem>();
                    JArray dblist = (JArray)lastJsonCall["dblist"];
                    for (int i = 0; i < dblist.Count; ++i)
                    {
                        JArray jdd = (JArray)dblist[i]["doctypes"];
                        string[] doctypes = new string[jdd.Count];
                        for (int j = 0; j < jdd.Count; ++j) doctypes[j] = jdd[j].ToString();
                        JObject unit = (JObject)dblist[i]["unit"];

                        ret[dblist[i]["db_name"].ToString()] = 
                            new StringDbItem(dblist[i]["db_title"].ToString(), dblist[i]["db_name"].ToString(), doctypes, 
                                Convert.ToString(unit["uid"]), Convert.ToString(unit["foreign_id"]), Convert.ToString(unit["title"]), 
                                Convert.ToString(unit["documentstate"]), Convert.ToString(unit["data"]));
                    }
                    return ret;
                }
            }

            return null;
        }

        public JObject jsonFromDataTable(DataTable dtSrc)
        {
            JObject ret = new JObject();
            try
            {
                JArray jaFields = new JArray();
                ret["fields"] = jaFields;
                foreach (DataColumn col in dtSrc.Columns)
                {
                    string ftype = "string";
                    if (col.DataType == typeof(double)) ftype = "real";
                    else if (col.DataType == typeof(int)) ftype = "int";
                    else if (col.DataType == typeof(DateTime)) ftype = "timestamp";
                    JObject joCol = new JObject();
                    joCol["name"] = col.ColumnName;
                    joCol["type"] = ftype;
                    joCol["max_length"] = col.MaxLength;
                    jaFields.Add(joCol);
                }

                JArray jaData = new JArray();
                ret["data"] = jaData;
                foreach (DataRow row in dtSrc.Rows)
                {
                    JArray jaRow = new JArray();
                    jaData.Add(jaRow);
                    foreach (DataColumn col in dtSrc.Columns)
                    {
                        jaRow.Add(row[col].ToString());
                    }
                }

                return ret;
            }
            catch (Exception) { }
            return null;
        }

        public DataTable jsonToDataTable(string sjson)
        {
            DataTable ret = new DataTable();
            JObject json = JObject.Parse(sjson);

            JArray jdFields = (JArray)json["fields"];
            int fieldsCount = jdFields.Count;
            string[] fieldNames = new string[fieldsCount];
            Type[] fieldTypes = new Type[fieldsCount];
            for (int i = 0; i < fieldsCount; ++i)
            {
                fieldNames[i] = jdFields[i]["name"].ToString();
                string ftype = jdFields[i]["type"].ToString().ToLowerInvariant();
                if (ftype.Equals("real"))
                {
                    fieldTypes[i] = typeof(double);
                }
                else if (ftype.Equals("int"))
                {
                    fieldTypes[i] = typeof(int);
                }
                else if (ftype.Equals("timestamp"))
                {
                    fieldTypes[i] = typeof(DateTime);
                }
                else if (ftype.Equals("string") || ftype.Equals("blob") || ftype.Equals("binary"))
                {
                    fieldTypes[i] = typeof(string);
                }

                DataColumn ncol = ret.Columns.Add(fieldNames[i], fieldTypes[i]);
                /*
                try
                {
                    ncol.MaxLength = Convert.ToInt32(jdFields[i]["max_length"].ToString());
                }
                catch (Exception) { }
                 */
            }

            JArray jdData = (JArray)json["data"];
            int rowsCount = jdData.Count;
            for (int i = 0; i < rowsCount; ++i)
            {
                DataRow nr = ret.NewRow();
                JArray jdrow = (JArray)jdData[i];

                for (int j = 0; j < fieldsCount; ++j)
                {
                    try
                    {
                        if (fieldTypes[j] == typeof(double)) nr[fieldNames[j]] = Convert.ToDouble(jdrow[j].ToString());
                        else if (fieldTypes[j] == typeof(int)) nr[fieldNames[j]] = Convert.ToInt32(jdrow[j].ToString());
                        else if (fieldTypes[j] == typeof(string)) nr[fieldNames[j]] = jdrow[j].ToString();
                        else if (fieldTypes[j] == typeof(DateTime))
                        {
                            string s = jdrow[j].ToString();
                            nr[fieldNames[j]] = new DateTime(
                                Convert.ToInt32(s.Substring(0, 4)),
                                Convert.ToInt32(s.Substring(5, 2)),
                                Convert.ToInt32(s.Substring(8, 2)),
                                Convert.ToInt32(s.Substring(11, 2)),
                                Convert.ToInt32(s.Substring(14, 2)),
                                Convert.ToInt32(s.Substring(17, 2))
                                );
                        }
                    }
                    catch (Exception) { }
                }
                try
                {
                    ret.Rows.Add(nr);
                }
                catch (Exception) { }
            }

            return ret;
        }

        public DataTable queryJournal(DateTime date)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "queryjournal", "db", currentDb, "date", date.ToString("yyyyMMdd"));
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        return jsonToDataTable(lastJsonCall.ToString());
                    }
                }
            }
            return null;
        }

        public DataTable getQuery(string sql)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "getquery", "db", currentDb, "sql", sql);
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        return jsonToDataTable(lastJsonCall.ToString());
                    }
                }
            }
            return null;
        }

        public int tableRecordsCount(string tableName)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "tablerecordscount", "db", currentDb, "table", tableName);
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        try
                        {
                            int ret = Convert.ToInt32((string)lastJsonCall["count"]);
                            return ret;
                        }
                        catch (Exception) { }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns> -1 = запрос не удался по причине клиентской ошибки, 0 и выше = статус, возвращаемый сервером</returns>
        public int runQuery(string sql)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "getquery", "db", currentDb, "sql", sql);
                if (lastJsonCall != null)
                {
                    return (int)lastJsonCall["result"];
                }
            }
            return -1;
        }

        public long runQueryReturnLastInsertedId(string sql)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "getquery", "db", currentDb, "sql", sql);
                if (lastJsonCall != null)
                {
                    return (int)lastJsonCall["mysql_insert_id"];
                }
            }
            return -1;
        }

        public void setDataReference(string TableName, string Key, bool DoIncrement)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest(
                    "sid", sid, "f", "setdatareference", "db", currentDb, "tablename", TableName,
                    "key", Key, "doincrement", DoIncrement ? "1" : "0"
                    );
            }
        }

        public int getDataReference(string TableName, string Key)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest(
                    "sid", sid, "f", "getdatareference", "db", currentDb, "tablename", TableName, "key", Key
                    );
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        return (int)lastJsonCall["refcount"];
                    }
                }
            }

            return 0;
        }

        public void clearDataReference(string TableName, string Key)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest(
                    "sid", sid, "f", "cleardatareference", "db", currentDb, "tablename", TableName,
                    "key", Key
                    );
            }
        }

        public ArrayList checkDocumentCriticals(ArrayList fields, IDEXDocumentData document)
        {
            ArrayList ret = new ArrayList();

            if (currentDb != null)
            {
                JObject json = new JObject();
                SimpleXML doc = SimpleXML.LoadXml(document.documentText);
                foreach (string field in fields)
                {
                    SimpleXML node = doc.GetNodeByPath(field, false);
                    if (node != null && node.Text.Length > 0)
                    {
                        json[field] = node.Text;
                    }
                }

                lastJsonCall = jsonHttpRequest(
                    "sid", sid, "f", "checkdocumentcriticals", "db", currentDb, 
                    "signature", document.signature , "fields", json.ToString()
                    );
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        JArray jdata = (JArray)lastJsonCall["data"];
                        for (int i = 0; i < jdata.Count; ++i)
                        {
                            ret.Add((string)jdata[i]);
                        }
                    }
                }
            }

            return ret;
        }

        public JObject jsonDocumentCriticals(ArrayList fields, IDEXDocumentData document)
        {
            JObject json = new JObject();
            if (fields != null && document != null)
            {
                SimpleXML doc = SimpleXML.LoadXml(document.documentText);
                foreach (string field in fields)
                {
                    SimpleXML node = doc.GetNodeByPath(field, false);
                    if (node != null && node.Text.Length > 0)
                    {
                        json[field] = node.Text;
                    }
                }
            }

            return json;
        }

        public Dictionary<string, string> getDicList()
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "diclist", "db", currentDb);
                if (lastJsonCall != null)
                {
                    try
                    {
                        Dictionary<string, string> ret = new Dictionary<string, string>();
                        JObject jdata = (JObject)lastJsonCall["data"];

                        foreach (JProperty jprop in jdata.Properties())
                        {
                            ret[jprop.Name] = jprop.Value.ToString();
                        }
                        return ret;
                    }
                    catch (Exception) { } 
                }
            }
            return null;
        }

        public string getUpdateKey()
        {
            lastJsonCall = jsonHttpRequest("sid", sid, "f", "updatekey");
            if (lastJsonCall != null)
            {
                try
                {
                    return (string)lastJsonCall["key"];
                }
                catch (Exception) { }
            }
            return null;
        }

        public bool downloadUpdate(string outfn)
        {
            try
            {
                string url = DEXOL_URL + string.Format(@"sid={0}&f=update", sid);

                try
                {
                    File.Delete(outfn);
                }
                catch (Exception) { } 

                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream st = response.GetResponseStream();
                FileStream outfs = new FileStream(outfn, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                int count = 0;

                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        outfs.Write(buf, 0, count);
                    }
                } while (count > 0);

                outfs.Flush();
                outfs.Close();

                return true;

            }
            catch (Exception ex)
            {
                lastExceptionMsg = ex.Message;
            }

            return false;
        }

        public void setDocumentCriticals(ArrayList fields, IDEXDocumentData document, bool reset) 
        {
            if (currentDb != null)
            {
                JObject json = jsonDocumentCriticals(fields, document);

                if (document.documentStatus > DEXToolBox.DOCUMENT_DRAFT && document.documentStatus != DEXToolBox.DOCUMENT_TODELETE)
                {
                    lastJsonCall = jsonHttpRequest(
                        "sid", sid, "f", "setdocumentcriticals", "db", currentDb,
                        "signature", document.signature, "reset", reset ? "1" : "0",
                        "fields", json.ToString()
                        );
                }
                else
                {
                    lastJsonCall = jsonHttpRequest(
                        "sid", sid, "f", "setdocumentcriticals", "db", currentDb,
                        "signature", document.signature, "reset", reset ? "1" : "0"
                        );
                }
            }
        }

        public string commitDocument(JObject document)
        {
            if (currentDb != null)
            {
                                    
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "commitdocument", "db", currentDb, "POST:document", document.ToString());
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        return null;
                    }
                    string ler = lastErrorMessage();
                    if (ler == null) ler = "Ошибка получения ответа от сервера";
                    return ler;
                }
                return "Ошибка обращения к серверу";
            }

            return "Не выбрана БД";
        }

        public string deleteDocument(int id)
        {
            if (currentDb != null)
            {
                lastJsonCall = jsonHttpRequest("sid", sid, "f", "deletedocument", "db", currentDb, "id", id.ToString());
                if (lastJsonCall != null)
                {
                    if ((int)lastJsonCall["result"] == 0)
                    {
                        return null;
                    }
                    string ler = lastErrorMessage();
                    if (ler == null) ler = "Ошибка получения ответа от сервера";
                    return ler;
                }
                return "Ошибка обращения к серверу";
            }

            return "Не выбрана БД";
        }

        public string lastErrorMessage()
        {
            try
            {
                return lastJsonCall["message"].ToString();
            }
            catch (Exception) { }

            return lastExceptionMsg;
        }

    }
}
