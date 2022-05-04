using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DEXExtendLib;
using System.Data;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public class DOL2Data // Загружает Links
    {
        object toolbox;

        private bool m_dataLoaded = true;

        public bool dataLoaded { get { return m_dataLoaded; } }

        public List<StringTagItem> spheres = null; // IdName
        public List<StringTagItem> doctypes = null; // IdName
        public List<StringTagItem> placetypes = null; // IdNameShortname
        public List<StringTagItem> streettypes = null; // IdNameShortname
        public List<StringTagItem> buildingtypes = null; // IdNameShortname
        public List<StringTagItem> roomtypes = null; // IdNameShortname
        public List<StringTagItem> deliverytypes = null; // IdName
        public List<StringTagItem> billcycles = null; // IdName
        public List<StringTagItem> channeltypes = null; // IdNameCode
        public List<StringTagItem> companytypes = null; // IdNameShortname
        public List<StringTagItem> persontypes = null; // IdNameShortname
        public List<StringTagItem> paysystems = null; // IdName

        public List<StringTagItem> libCountries;


        public Dictionary<int, string> libCellnets;
        public Dictionary<int, string> libChannellens;
//        public Dictionary<int, string> libPaysystems;
        public Dictionary<int, NameCode> libServices;
        public Dictionary<string, DOL2BillPlan> libBillplans;

        public DOL2Data(object toolbox)
        {
            this.toolbox = toolbox;
            IDEXData d = (IDEXData)toolbox;

            try
            {
                spheres = loadIdName2(libname("spheres"));
                doctypes = loadIdName2(libname("doctypes"));
                placetypes = loadIdName2(libname("placetypes"));
                streettypes = loadIdName2(libname("streettypes"));
                buildingtypes = loadIdName2(libname("buildingtypes"));
                roomtypes = loadIdName2(libname("roomtypes"));
                deliverytypes = loadIdName2(libname("deliverytypes"));
                billcycles = loadIdName2(libname("billcycles"));
                channeltypes = loadIdName2(libname("channeltypes"));
                companytypes = loadIdName2(libname("companytypes"));
                persontypes = loadIdName2(libname("persontypes"));
                paysystems = loadIdName2(libname("paysystems"));

                libCellnets = loadIdName(libname("cellnets"));
                libChannellens = loadIdName(libname("channellens"));
//                libPaysystems = loadIdName(libname("paysystems"));
                libServices = loadIdNameCode(libname("services"));
                libCountries = loadIdNameCode2(libname("countries"));
                

                libBillplans = new Dictionary<string, DOL2BillPlan>();

                Dictionary<int, DOL2BillPlan> idPlans = new Dictionary<int, DOL2BillPlan>();

//                DataTable tbPlans = d.getQuery(string.Format("select * from `{0}` order by SOC", d.EscapeString(libname("billplans"))));
                DataTable tbPlans = d.getTable(d.EscapeString(libname("billplans")));
                tbPlans.DefaultView.Sort = "SOC";
                
                if (tbPlans != null && tbPlans.Rows.Count > 0)
                {
                    foreach (DataRow row in tbPlans.Rows)
                    {
                        try 
                        {
                            DOL2BillPlan plan = new DOL2BillPlan(int.Parse(row["id"].ToString()), row["name"].ToString(), row["code"].ToString(),
                                int.Parse(row["paysystemsid"].ToString()), int.Parse(row["cellnetsid"].ToString()), int.Parse(row["channellensid"].ToString()),
                                Convert.ToBoolean(row["enable"]), Convert.ToBoolean(row["accept"]), row["soc"].ToString(), new List<DOL2Service>());

                            libBillplans[plan.SOC] = plan;
                            idPlans[plan.id] = plan;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                // изменение тарифного плана (костя) 08.10.2015 начало
                //d.setTp(tbPlans);
                // изменение тарифного плана (костя) 08.10.2015 конец
//                DataTable tbBPS = d.getQuery(string.Format("select * from `{0}`", d.EscapeString(libname("bplanservices"))));
                DataTable tbBPS = d.getTable(d.EscapeString(libname("bplanservices")));
                if (tbBPS != null && tbBPS.Rows.Count > 0)
                {
                    foreach(DataRow brow in tbBPS.Rows)
                    {
                        try
                        {
                            int billplansid = int.Parse(brow["billplansid"].ToString());
                            int servicesid = int.Parse(brow["servicesid"].ToString());
                            bool mandatory = Convert.ToBoolean(brow["mandatory"]);
                            if (libServices.ContainsKey(servicesid) && idPlans.ContainsKey(billplansid))
                            {
                                DOL2Service d2s = new DOL2Service(servicesid, libServices[servicesid].Name, libServices[servicesid].Code, mandatory);
                                if (!serviceInList(idPlans[billplansid].services, d2s))
                                {
                                    idPlans[billplansid].services.Add(d2s);
                                }
                            }
                        }
                        catch(Exception)
                        {
                        }
                    }
                }


                m_dataLoaded = true;
            }
            catch (Exception)
            {
                m_dataLoaded = false;
            }
        }

        bool serviceInList(List<DOL2Service> sl, DOL2Service svc)
        {
            if (svc != null)
            {
                foreach (DOL2Service cd2s in sl)
                {
                    if (cd2s != null && cd2s.Code == svc.Code) return true;
                }
            }
            return false;
        }

        public string libname(string namepart) 
        {
            return "beeline_" + namepart;
        }

        Dictionary<int, string> loadIdName(string tableName)
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();

            IDEXData d = (IDEXData)toolbox;
            //DataTable tb = d.getQuery(string.Format("select * from `{0}` order by name", d.EscapeString(tableName)));
            DataTable tb = d.getTable(d.EscapeString(tableName));
            tb.DefaultView.Sort = "name";
            if (tb != null && tb.Rows.Count > 0)
            {
                foreach (DataRow r in tb.Rows)
                {
                    ret[int.Parse(r["id"].ToString())] = r["name"].ToString();
                }
            }

            return ret;
        }

        List<StringTagItem> loadIdName2(string tableName)
        {
            List<StringTagItem> ret = new List<StringTagItem>();

            IDEXData d = (IDEXData)toolbox;
//            DataTable tb = d.getQuery(string.Format("select * from `{0}` order by name", d.EscapeString(tableName)));
            DataTable tb = d.getTable(d.EscapeString(tableName));
            tb.DefaultView.Sort = "name";
            if (tb != null && tb.Rows.Count > 0)
            {
                foreach (DataRow r in tb.Rows)
                {
                    ret.Add(new StringTagItem(r["name"].ToString(), r["id"].ToString()));
                }
            }

            return ret;
        }

        List<StringTagItem> loadIdNameCode2(string tableName)
        {
            List<StringTagItem> ret = new List<StringTagItem>();
            IDEXData d = (IDEXData)toolbox;
            DataTable tb = d.getTable(d.EscapeString(tableName));
            tb.DefaultView.Sort = "translation";
            if (tb != null && tb.Rows.Count > 0)
            {
                foreach (DataRow r in tb.Rows)
                {
                    ret.Add(new StringTagItem(r["translation"].ToString(), r["id"].ToString()));
                }
            }

            return ret;
        }

        Dictionary<int, NameCode> loadIdNameCode(string tableName)
        {
            Dictionary<int, NameCode> ret = new Dictionary<int, NameCode>();

            IDEXData d = (IDEXData)toolbox;
//            DataTable tb = d.getQuery(string.Format("select * from `{0}` order by name", d.EscapeString(tableName)));
            DataTable tb = d.getTable(d.EscapeString(tableName));
            tb.DefaultView.Sort = "name";
            if (tb != null && tb.Rows.Count > 0)
            {
                foreach (DataRow r in tb.Rows)
                {
                    int iid = int.Parse(r["id"].ToString());
                    ret[iid] = new NameCode(iid, r["name"].ToString(), r["code"].ToString());
                }
            }

            return ret;
        }

        public StringTagItem findByTag(List<StringTagItem> src, string search)
        {
            try
            {
                foreach (StringTagItem sti in src)
                {
                    if (sti.Tag.Equals(search, StringComparison.CurrentCultureIgnoreCase)) return sti;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public static bool CheckOrgAttribute(string value, bool onlyDigit, int len)
        {
            try
            {
                if (value.Equals("")) return false;
                if (onlyDigit)
                {
                    for (int f = 0; f < value.Length; ++f)
                    {
                        if (value[f] < '0' || value[f] > '9') return false;
                    }
                }
                return value.Length == len;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool CheckOrgAttribute(string value, int minlen, int maxlen)
        {
            try
            {
                int i = int.Parse(value);
                int len = i.ToString().Length;
                return len >= minlen && len <= maxlen;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool CheckString(string src, bool noDigits, bool noSymbols)
        {
            try
            {
                for (int f = 0; f < src.Length; ++f)
                {
                    bool isDigit = src[f] >= '0' && src[f] <= '9';
                    if (isDigit && noDigits) return false;
                    if (!isDigit && noSymbols) return false;
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        public static DateTime strtodate(string src) 
        {
            DateTime ret;
            if (!DateTime.TryParse(src, out ret)) ret = DateTime.MinValue;
            return ret;
        }
    }

    public class NameCode
    {
        public int id;
        public string Name, Code;
        public NameCode(int id, string Name, string Code)
        {
            this.id = id;
            this.Name = Name;
            this.Code = Code;
        }
    }

    public class DOL2Service
    {
        public int id;
        public string Name, Code;
        public bool Mandatory;
        public DOL2Service(int id, string Name, string Code, bool Mandatory)
        {
            this.id = id;
            this.Name = Name;
            this.Code = Code;
            this.Mandatory = Mandatory;
        }
    }

    public class DOL2BillPlan
    {
        public int id;
        public string Name, Code;
        public int PaySystemsId, CellnetsId, ChannellensId;
        public bool Enable, Accept;
        public string SOC;
        public List<DOL2Service> services;

        public DOL2BillPlan(int id, string Name, string Code, int PaySystemsId, int CellnetsId, int ChannellensId, bool Enable, bool Accept, string SOC, List<DOL2Service> services)
        {
            this.id = id;
            this.Name = Name;
            this.Code = Code;
            this.PaySystemsId = PaySystemsId;
            this.CellnetsId = CellnetsId;
            this.ChannellensId = ChannellensId;
            this.Enable = Enable;
            this.Accept = Accept;
            this.SOC = SOC;
            this.services = services;
        }

    }
}
