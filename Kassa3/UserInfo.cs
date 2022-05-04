using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Globalization;
using System.Collections;
using System.Data;
//using MySql.Data.MySqlClient;

namespace Kassa3
{
    public class UserPrefs
    {
        // Здесь будут разрешения, настройки и прочее для одного пользователя
        public DateTime createdTime;

        public AccessMode dicUsers;
        public AccessMode dicCurrency;
        public AccessMode dicClients;
        public AccessMode dicOps;
        public AccessMode dicFirmAcc;
        public SimplePermission appSettings;
        public SimplePermission fieldsEdit;

        public SimplePermission globalRule;
        public List<OpRuleItem> opRules;
        Dictionary<OpRuleType, Dictionary<long, SimplePermission>> ruleMap;
        Dictionary<long, long> accountMap, categoryMap, clientMap;
        Dictionary<int, int> dAccToFirm;

        char[] RulePrefix = { 'F', 'A', 'O', 'C', 'P' };

        public bool needRebuildRuleMapping = true;

        public UserPrefs()
        {
            createdTime = DateTime.Now;
            dicUsers = AccessMode.NONE;
            dicCurrency = AccessMode.NONE;
            dicClients = AccessMode.NONE;
            dicOps = AccessMode.NONE;
            dicFirmAcc = AccessMode.NONE;
            appSettings = SimplePermission.PROHIBIT;
            fieldsEdit = SimplePermission.PERMIT;

            opRules = new List<OpRuleItem>();
            globalRule = SimplePermission.PROHIBIT;

            ruleMap = new Dictionary<OpRuleType, Dictionary<long, SimplePermission>>();

            foreach (OpRuleType cur in Enum.GetValues(typeof(OpRuleType)))
            {
                ruleMap[cur] = new Dictionary<long, SimplePermission>();
            }

            accountMap = new Dictionary<long, long>();
            categoryMap = new Dictionary<long, long>();
            clientMap = new Dictionary<long, long>();
            dAccToFirm = new Dictionary<int, int>();
        }

        ~UserPrefs()
        {
            opRules.Clear();
            opRules = null;
            ruleMap.Clear();
            ruleMap = null;
            accountMap.Clear();
            accountMap = null;
            categoryMap.Clear();
            categoryMap = null;
            clientMap.Clear();
            clientMap = null;
        }

        public void LoadFromXml(string src)
        {
            opRules.Clear();

            try
            {
                SimpleXML xml = SimpleXML.LoadXml(src);
                
                //createdTime = 
                DateTime.TryParseExact(xml["createdTime"].Text, "yyyyMMddHHmmssffffff", CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out createdTime);

                int i = 0;
                dicUsers = (AccessMode)(int.TryParse(xml["dicUsers"].Text, out i) ? i : (int)AccessMode.NONE);
                dicCurrency = (AccessMode)(int.TryParse(xml["dicCurrency"].Text, out i) ? i : (int)AccessMode.NONE);
                dicClients = (AccessMode)(int.TryParse(xml["dicClients"].Text, out i) ? i : (int)AccessMode.NONE);
                dicOps = (AccessMode)(int.TryParse(xml["dicOps"].Text, out i) ? i : (int)AccessMode.NONE);
                dicFirmAcc = (AccessMode)(int.TryParse(xml["dicFirmAcc"].Text, out i) ? i : (int)AccessMode.NONE);
                appSettings = (SimplePermission)(int.TryParse(xml["appSettings"].Text, out i) ? i : (int)SimplePermission.PROHIBIT);
                fieldsEdit = (SimplePermission)( int.TryParse(xml["fieldsEdit"].Text, out i) ? i : (int)SimplePermission.PERMIT );

                globalRule = (SimplePermission)(int.TryParse(xml["OpRules"].Attributes["global_rule"], out i) ? i : (int)SimplePermission.PROHIBIT);

                ArrayList alRules = xml["OpRules"].GetChildren("Rule");
                if (alRules != null && alRules.Count > 0)
                {
                    foreach(SimpleXML xmlRule in alRules) 
                    {
                        try
                        {
                            OpRuleItem newRule = new OpRuleItem(
                                (OpRuleType)int.Parse(xmlRule.Attributes["type"]),
                                long.Parse(xmlRule.Attributes["param_id"]),
                                "",
                                true.ToString().Equals(xmlRule.Attributes["permit"])
                                );
                            if (Tools.instance.ValidateRule(newRule)) opRules.Add(newRule);
                        }
                        catch (Exception) { }
                    }
                }
                buildRuleMapping();
            }
            catch (Exception) { }
        }

        public string SaveToXml()
        {
            SimpleXML xml = new SimpleXML("UserPrefs");
            xml["createdTime"].Text = createdTime.ToString("yyyyMMddHHmmssffffff");
            xml["dicUsers"].Text = ((int)dicUsers).ToString();
            xml["dicCurrency"].Text = ((int)dicCurrency).ToString();
            xml["dicClients"].Text = ((int)dicClients).ToString();
            xml["dicOps"].Text = ((int)dicOps).ToString();
            xml["dicFirmAcc"].Text = ((int)dicFirmAcc).ToString();
            xml["appSettings"].Text = ((int)appSettings).ToString();
            xml["fieldsEdit"].Text = ( (int)fieldsEdit ).ToString();

            SimpleXML xmlRules = xml.CreateChild("OpRules");
            xmlRules.Attributes["global_rule"] = ((int)globalRule).ToString();

            foreach (OpRuleItem rule in opRules)
            {
                SimpleXML newRule = xmlRules.CreateChild("Rule");
                newRule.Attributes["type"] = ((int)rule.RuleType).ToString();
                newRule.Attributes["param_id"] = rule.paramId.ToString();
                newRule.Attributes["permit"] = rule.permit.ToString();
            }

            return SimpleXML.SaveXml(xml);
        }

        void fillSimpleRuleMapItem(OpRuleType rtype, string sqlquery)
        {
            try
            {
                Dictionary<long, SimplePermission> rmItem = ruleMap[rtype];
                rmItem.Clear();

//                DataTable dt = Tools.instance.MySqlFillTable(new MySqlCommand(sqlquery, Tools.instance.connection));
                using (DataTable dt = Db.fillTable(Db.command(sqlquery)))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        rmItem[Convert.ToInt64(r["id"])] = globalRule;
                    }
                }
            }

            catch (Exception) { }
        }

        void markCategories(long cat_id, SimplePermission perm)
        {
            Dictionary<long, SimplePermission> rmCat = ruleMap[OpRuleType.CATEGORY];
            Dictionary<long, SimplePermission> rmCli = ruleMap[OpRuleType.CLIENT];

            try
            {
                rmCat[cat_id] = perm;
                foreach (KeyValuePair<long, long> kvpcli in clientMap)
                {
                    if (kvpcli.Value == cat_id) rmCli[kvpcli.Key] = perm;
                }

                foreach (KeyValuePair<long, long> kvpcat in categoryMap)
                {
                    if (kvpcat.Value == cat_id) markCategories(kvpcat.Key, perm);
                }
            }
            catch (Exception) { }
        }

        public void buildRuleMapping()
        {
            Tools tools = Tools.instance;

            dAccToFirm.Clear();

            try
            {
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, firm_id from accounts", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select id, firm_id from accounts")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        dAccToFirm[Convert.ToInt32(r["id"])] = Convert.ToInt32(r["firm_id"]);
                    }
                }
            }
            catch (Exception) { }

            fillSimpleRuleMapItem(OpRuleType.FIRM, "select id from firms");
            fillSimpleRuleMapItem(OpRuleType.OPERATION, "select id from ops");

            try
            {
                Dictionary<long, SimplePermission> rmAccounts = ruleMap[OpRuleType.ACCOUNT];
                rmAccounts.Clear();
                accountMap.Clear();

//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, firm_id from accounts", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select id, firm_id from accounts")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        rmAccounts[Convert.ToInt64(r["id"])] = globalRule;
                        accountMap[Convert.ToInt64(r["id"])] = Convert.ToInt64(r["firm_id"]);
                    }
                }
            }
            catch (Exception) { }

            try
            {
                Dictionary<long, SimplePermission> rmCat = ruleMap[OpRuleType.CATEGORY];
                rmCat.Clear();
                categoryMap.Clear();

//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, parent_id from client_cat", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select id, parent_id from client_cat")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        rmCat[Convert.ToInt64(r["id"])] = globalRule;
                        categoryMap[Convert.ToInt64(r["id"])] = Convert.ToInt64(r["parent_id"]);
                    }
                }
            }
            catch (Exception) { }

            try
            {
                Dictionary<long, SimplePermission> rmCli = ruleMap[OpRuleType.CLIENT];
                rmCli.Clear();
                clientMap.Clear();

//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, cat_id from client_data", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select id, cat_id from client_data")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        rmCli[Convert.ToInt64(r["id"])] = globalRule;
                        clientMap[Convert.ToInt64(r["id"])] = Convert.ToInt64(r["cat_id"]);
                    }
                }
            }
            catch (Exception) { }

            foreach (OpRuleItem rule in opRules)
            {
                SimplePermission perm = rule.permit ? SimplePermission.PERMIT : SimplePermission.PROHIBIT;

                if (rule.RuleType == OpRuleType.CATEGORY) markCategories(rule.paramId, perm);
                else
                {
                    ruleMap[rule.RuleType][rule.paramId] = perm;
                    if (rule.RuleType == OpRuleType.FIRM)
                    {
                        foreach (KeyValuePair<long, long> kvpacc in accountMap)
                        {
                            if (kvpacc.Value == rule.paramId) ruleMap[OpRuleType.ACCOUNT][kvpacc.Key] = perm;
                        }
                    }
                }
            }
        }

        void checkRebuildRuleMapping()
        {
            if (needRebuildRuleMapping)
            {
                buildRuleMapping();
                needRebuildRuleMapping = false;
            }
        }

        public SimplePermission getRuleFor(OpRuleType ruleType, long id)
        {
            checkRebuildRuleMapping();

            try
            {
                return ruleMap[ruleType][id];
            }
            catch (Exception) { }

            return globalRule;
        }

        public string getSqlForRuleType(OpRuleType ruleType, string fieldIdentifier)
        {
            checkRebuildRuleMapping();

            string ret = "";
            try
            {

                foreach (KeyValuePair<long, SimplePermission> kvp in ruleMap[ruleType])
                {
                    if (kvp.Value == SimplePermission.PERMIT)
                    {
                        if (ret != "") ret += " OR ";
                        ret += fieldIdentifier + " = " + kvp.Key.ToString();
                    }
                }
            }
            catch (Exception) { }

            if (ret == "") ret = fieldIdentifier + " = -1";

            return ret;
        }

        public string getIdListForRuleType(OpRuleType ruleType)
        {
            return getIdListForRuleType(ruleType, -1);
        }

        public string getIdListForRuleType(OpRuleType ruleType, int firmId)
        {
            checkRebuildRuleMapping();

            string ret = "";
            try
            {

                foreach (KeyValuePair<long, SimplePermission> kvp in ruleMap[ruleType])
                {
                    if (kvp.Value == SimplePermission.PERMIT && (firmId < 0 || 
                        (dAccToFirm != null && dAccToFirm.ContainsKey((int)kvp.Key) && dAccToFirm[(int)kvp.Key] == firmId)))
                    {
                        if (ret != "") ret += ", ";
                        ret += kvp.Key.ToString();
                    }
                }
            }
            catch (Exception) { }

            if (ret == "") ret = "-1";

            return ret;
        }

        public string getRuleMappingText()
        {
            checkRebuildRuleMapping();

            Tools tools = Tools.instance;
            string ret = "";

            // Фирмы и счета
            // ----------------------
            // ! + ! Петров : Счёт 1 
            // ! - ! Петров : Счёт 2 
            // ! - ! Иванов : Счёт 1 
            // ----------------------
            try
            {
                string tbldata = "";
                int maxwidth = 0;

                string SCONCAT = Db.isMysql ? "concat(fi.title, ' : ', ac.title, ' (', cl.name, ')')" : "fi.title || ' : ' || ac.title || ' (' || cl.name || ')'";

                string sql = 
                    "select ac.id as ac_id, " + SCONCAT + " as title " +
                    "from accounts as ac, firms as fi, curr_list as cl " +
                    "where ac.firm_id = fi.id and ac.curr_id = cl.id " +
                    "order by fi.title, ac.title, cl.name";

//                DataTable dt = tools.MySqlFillTable(new MySqlCommand(sql, tools.connection));
                using (DataTable dt = Db.fillTable(Db.command(sql)))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SimplePermission perm = getRuleFor(OpRuleType.ACCOUNT, Convert.ToInt64(r["ac_id"]));
                        string scurr = "! " + (perm == SimplePermission.PERMIT ? "+" : "-") + " ! " + r["title"].ToString() + " ";
                        if (scurr.Length > maxwidth) maxwidth = scurr.Length;
                        tbldata += scurr + "\r\n";
                    }
                }

                string pad = "".PadRight(maxwidth, '-') + "\r\n";
                ret += "Фирмы и счета\r\n" + pad + tbldata + pad + "\r\n";
            }
            catch (Exception) {
                ret += "(!!!) Не удалось собрать карту разрешений для фирм и счетов\r\n\r\n";
            }

            // Операции
            // -----------------------
            // ! + ! Выплата зарплаты 
            // ! - ! Деление на ноль  
            // -----------------------
            try
            {
                string tbldata = "";
                int maxwidth = 0;
//                DataTable dt = tools.MySqlFillTable(new MySqlCommand("select id, title from ops order by title", tools.connection));
                using (DataTable dt = Db.fillTable(Db.command("select id, title from ops order by title")))
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SimplePermission perm = getRuleFor(OpRuleType.OPERATION, Convert.ToInt64(r["id"]));
                        string scurr = "! " + (perm == SimplePermission.PERMIT ? "+" : "-") + " ! " + r["title"].ToString() + " ";
                        if (scurr.Length > maxwidth) maxwidth = scurr.Length;
                        tbldata += scurr + "\r\n";
                    }
                }
                string pad = "".PadRight(maxwidth, '-') + "\r\n";
                ret += "Операции\r\n" + pad + tbldata + pad + "\r\n";
            }
            catch (Exception)
            {
                ret += "(!!!) Не удалось собрать карту разрешений для операций\r\n\r\n";
            }


            // Категории и клиенты
            // ------------------------------
            // !   ! Важные клиенты
            // !   !  Очень Важные клиенты
            // ! - !   Вася
            // ! + !   Петя

            try
            {
                string tbldata = "";
                int maxwidth = 0;

//                DataTable dtcat = tools.MySqlFillTable(new MySqlCommand("select * from client_cat order by parent_id, cat_title", tools.connection));

                using (DataTable dtcat = Db.fillTable(Db.command("select * from client_cat order by parent_id, cat_title")))
                {
//                    DataTable dtcli = tools.MySqlFillTable(new MySqlCommand("select * from client_data order by cat_id, title", tools.connection));

                    using (DataTable dtcli = Db.fillTable(Db.command("select * from client_data order by cat_id, title")))
                    {

                        DataRow[] rows = dtcat.Select("parent_id = -1");
                        foreach (DataRow r in rows)
                        {
                            addCatText(dtcat, dtcli, r["cat_title"].ToString(), Convert.ToInt64(r["id"]), ref maxwidth, ref tbldata, "");
                        }
                    }
                }
                string pad = "".PadRight(maxwidth, '-') + "\r\n";
                ret += "Категории и клиенты\r\n" + pad + tbldata + pad + "\r\n";
            }
            catch (Exception)
            {
                ret += "(!!!) Не удалось собрать карту разрешений для категорий и клиентов\r\n\r\n";
            }

            return ret;
        }

        void addCatText(DataTable dtcat, DataTable dtcli, string cat_title, long id, ref int maxwidth, ref string tbldata, string indent)
        {
            try
            {

                string scurr = "!   ! " + indent + cat_title;
                if (scurr.Length > maxwidth) maxwidth = scurr.Length;
                tbldata += scurr + "\r\n";

                DataRow[] rows = dtcli.Select("cat_id = " + id.ToString());
                foreach (DataRow r in rows)
                {
                    SimplePermission perm = getRuleFor(OpRuleType.CLIENT, Convert.ToInt64(r["id"]));
                    scurr = "! " + (perm == SimplePermission.PERMIT ? "+" : "-") + " ! " + indent + "> " + r["title"].ToString();
                    if (scurr.Length > maxwidth) maxwidth = scurr.Length;
                    tbldata += scurr + "\r\n";
                }

                DataRow[] rows2 = dtcat.Select("parent_id = " + id.ToString());
                foreach (DataRow r in rows2)
                {
                    addCatText(dtcat, dtcli, r["cat_title"].ToString(), Convert.ToInt64(r["id"]), ref maxwidth, ref tbldata, indent + " ");
                }
            }
            catch (Exception) { }
        }

    }

    public enum AccessMode { NONE = 0, READ = 1, WRITE = 2 };
    public enum SimplePermission { PROHIBIT = 0, PERMIT = 1 };

    public class User
    {
        public int PID;
        public bool active;
        public string Login;
        public string Password;
        public UserPrefs prefs;

        public User(int PID, bool active, string Login, string Password, UserPrefs prefs)
        {
            this.PID = PID;
            this.active = active;
            this.Login = Login;
            this.Password = Password;
            this.prefs = prefs;
        }
    }

    public enum OpRuleType
    {
        FIRM = 0, ACCOUNT = 1, OPERATION = 2, CATEGORY = 3, CLIENT = 4
    }

    public class OpRuleItem
    {
        string[] RuleTypeName = new string[] { "Фирма", "Счёт", "Операция", "Категория", "Контрагент" };

        public OpRuleType RuleType;
        public long paramId;
        public bool permit;
        public string paramTitle;

        public OpRuleItem(OpRuleType RuleType, long paramId, string paramTitle, bool permit)
        {
            this.RuleType = RuleType;
            this.paramId = paramId;
            this.permit = permit;
            this.paramTitle = paramTitle;
        }

        public override string ToString()
        {
            return (permit ? "[Разрешено] " : "[Запрещено] ") + RuleTypeName[(int)RuleType] + " " + paramTitle;
        }

    }

}
