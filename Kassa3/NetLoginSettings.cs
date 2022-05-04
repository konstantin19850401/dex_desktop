using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.IO;

namespace Kassa3
{
    public class NetLoginSettings
    {
        public Dictionary<string, string> dicLogins = new Dictionary<string, string>();
        public Dictionary<string, string> dicPasswords = new Dictionary<string, string>();

        public string MysqlHost = "";
        public int MysqlPort = 3306;
        public string MysqlUser = "";
        public string MysqlPassword = "";
        public string MysqlDb = "";
        public string MysqlKassaUser = "";
        public string MysqlKassaPassword = "";
        public bool SaveUser = false;

        public bool loadFromXml(String fileName)
        {
            try
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(fileName, Encoding.UTF8));

                SimpleXML xmlMysql = xml["MySql"];

                MysqlHost = xmlMysql.Attributes["Host"];
                MysqlPort = Convert.ToInt32(xmlMysql.Attributes["Port"]);
                MysqlUser = xmlMysql.Attributes["User"];
                MysqlPassword = xmlMysql.Attributes["User"];
                return true;
            }
            catch (Exception) { }

            return false;
                
        }
    }

    
}
