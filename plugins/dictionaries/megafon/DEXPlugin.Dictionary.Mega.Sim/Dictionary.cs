using System;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using DEXExtendLib;
using DEXSIM;

namespace DEXPlugin.Dictionary.Mega.Sim
{
    public class Dictionary: IDEXPluginDictionary, IDEXSim
    {
        #region IDEXPluginDictionary

        public string ID
        {
            get
            {
                return "DEXPlugin.Dictionary.Sim";
            }
        }
        public string Title
        {
            get
            {
                return "Справочник SIM-карт";
            }
        }

        public string[] Path
        {
            get
            {
                string[] ret = null;
                return ret;
            }
        }

        public int Version
        {
            get
            {
                return 1;
            }
        }

        public string Description
        {
            get
            {
                return "Справочник списка SIM-карт";
            }
        }

        public Bitmap getBitmap()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.IO.Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".icon.bmp");
                Bitmap b = new Bitmap(s);
                return b;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public void setJournalMode(DEXJournalType journalType)
        {
            this.journalType = journalType;
        }

        DEXJournalType journalType = DEXJournalType.JOURNAL;

        Object toolbox;

        public void Startup(Object toolbox)
        {
            this.toolbox = toolbox;
            IDEXRights r = (IDEXRights)toolbox;
            r.AddRightsItem(ID + ".access", "Доступ к справочнику SIM-карт");

            IDEXServices s = (IDEXServices)toolbox;
            s.RegisterService("sim", this);
        }

        public void ShowDictionary(Object toolbox)
        {
            IDEXRights r = (IDEXRights)toolbox;
            if (r.GetRightsItem(ID + ".access") || r.IsSuperUser())
            {

                FSimMain main = new FSimMain();
                main.toolbox = toolbox;
                main.InitForm();
                main.ShowDialog();
                main._saveLayout();
                main.toolbox = null;
                main = null;
                GC.Collect();
 
            }
            else
            {
                MessageBox.Show("У пользователя отсутствуют права доступа к справочнику SIM-карт");
            }
        }
        #endregion

        #region IDEXSim

        Dictionary<string, string> intGetSimBy(string field, string value)
        {
            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery(string.Format(
                "select * from `um_data` where {0} = '{1}'",
                field, MySqlHelper.EscapeString(value)));
            if (t != null && t.Rows.Count > 0)
            {
                try
                {
                    Dictionary<string, string> ret = new Dictionary<string, string>();
                    DataRow r = t.Rows[0];
                    foreach(DataColumn c in t.Columns)
                    {
                        ret[c.ColumnName] = r[c.ColumnName].ToString();
                    }

                    int unitid = int.Parse(r["owner_id"].ToString());
                    string planid = r["plan_id"].ToString();
                    string regionid = r["region_id"].ToString();

                    t = d.getQuery(string.Format("select * from `units` where uid = {0}", unitid));
                    if (t != null && t.Rows.Count > 0)
                    {
                        ret["owner_title"] = t.Rows[0]["title"].ToString();
                        ret["owner_desc"] = t.Rows[0]["desc"].ToString();
                        string s = t.Rows[0]["status"].ToString();
                        ret["owner_status"] = t.Rows[0]["status"].ToString();
                        ret["owner_data"] = t.Rows[0]["data"].ToString();
                    }

                    t = d.getQuery(string.Format("select * from `um_plans` where plan_id = '{0}'",
                        MySqlHelper.EscapeString(planid)));
                    if (t != null && t.Rows.Count > 0)
                    {
                        ret["plan_title"] = t.Rows[0]["title"].ToString();
                    }

                    t = d.getQuery(string.Format("select * from `um_regions` where region_id = '{0}'",
                        MySqlHelper.EscapeString(regionid)));
                    if (t != null && t.Rows.Count > 0)
                    {
                        ret["region_title"] = t.Rows[0]["title"].ToString();
                    }

                    ret["region_id"] = regionid;

                    return ret;
                }
                catch (Exception)
                {
                }
            }
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
            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery("select * from `um_data` where status = 0 or status = 1");
            if (t != null && t.Rows.Count > 0)
            {
                try
                {
                    List<string> ret = new List<string>();
                    foreach (DataRow r in t.Rows)
                    {
                        ret.Add(r["msisdn"].ToString());
                    }
                    return ret;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }


        #endregion
    }
}
