using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

namespace DEXPlugin.Journalhook.Common.CheckOutSim
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Yota.CheckOutSim";
            }
        }
        public string Title
        {
            get
            {
                return "Списать SIM-карту по документу";
            }
        }

        public string[] Path
        {
            get
            {
                return null;
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
                return "Присваивает статус <продано> SIM-карте из выделенного документа.";
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


        bool hookVisible = false;
        object toolbox;

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            this.toolbox = toolbox;
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            hookVisible = DocType.StartsWith("DEXPlugin.Document.") && 
                (DocStatus == 2 || DocStatus == 4) &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string,string>();
            if (hookVisible) ret["checkoutsim"] = "Списать SIM-карту по документу";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            return null;
        }

        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            try
            {
                if (FunctionName.Equals("checkoutsim") &&
                    docId.StartsWith("DEXPlugin.Document.") && 
                    (doc.documentStatus == 2 || doc.documentStatus == 4) &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    IDEXData d = (IDEXData)toolbox;

                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);

                    DataTable t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}'",
                        d.EscapeString(xml["ICC"].Text)));

                    string icc = xml["ICC"].Text;
                    
                    if (t != null && t.Rows.Count > 0)
                    {
                        string remark = t.Rows[0]["comment"].ToString();

                        if (doc.documentUnitID != int.Parse(t.Rows[0]["owner_id"].ToString()))
                        {
                            remark += (remark != "" ? ";" : "") + string.Format("Предыдущий владелец: {0}", t.Rows[0]["owner_id"].ToString());
                        }

                        if (!icc.Equals(t.Rows[0]["icc"].ToString()))
                        {
                            remark += (remark != "" ? ";" : "") + string.Format("Предыдущий ICC: {0}", t.Rows[0]["icc"].ToString());
                        }
                        /*
                        MySqlCommand cmd = new MySqlCommand(
                            "update `um_data` set status = 2, owner_id = @owner_id, date_sold = @date_sold, " +
                            "icc = @icc, comment = @comment where msisdn = @msisdn", d.getConnection()
                            );
                        cmd.Parameters.AddWithValue("owner_id", doc.documentUnitID);
                        cmd.Parameters.AddWithValue("date_sold", doc.documentDate.Substring(0, 8));
                        cmd.Parameters.AddWithValue("icc", icc);
                        cmd.Parameters.AddWithValue("msisdn", xml["MSISDN"].Text);
                        cmd.Parameters.AddWithValue("comment", remark);
                        cmd.ExecuteNonQuery();
                         */

                        d.runQuery(
                            "update `um_data` set status = 2, owner_id = {0}, date_sold = '{1}', " +
                            "icc = '{2}', comment = '{3}' where icc = '{4}'",
                            doc.documentUnitID, d.EscapeString(doc.documentDate.Substring(0, 8)),
                            d.EscapeString(icc), d.EscapeString(remark), d.EscapeString(xml["ICC"].Text)
                            );
                    }
                    else
                    {
                        /*
                        MySqlCommand cmd = new MySqlCommand(
                            "insert into `um_data` (status, msisdn, icc, date_in, owner_id, date_own, " +
                            "date_sold, region_id, party_id, plan_id) values (2, @msisdn, @icc, @date_in, " +
                            "@owner_id, @date_own, @date_sold, '-', @party_id, '-')", d.getConnection()
                            );
                        cmd.Parameters.AddWithValue("msisdn", xml["MSISDN"].Text);
                        cmd.Parameters.AddWithValue("icc", icc);
                        cmd.Parameters.AddWithValue("date_in", doc.documentDate.Substring(0, 8));
                        cmd.Parameters.AddWithValue("owner_id", doc.documentUnitID);
                        cmd.Parameters.AddWithValue("date_own", doc.documentDate.Substring(0, 8));
                        cmd.Parameters.AddWithValue("date_sold", doc.documentDate.Substring(0, 8));
                        cmd.Parameters.AddWithValue("party_id", (int)1);
                        cmd.ExecuteNonQuery();
                         */

                        d.runQuery(
                            "insert into `um_data` (status, msisdn, icc, date_in, owner_id, date_own, " +
                            "date_sold, region_id, party_id, plan_id) values (2, '{0}', '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '-', 1, '-')",
                            "", d.EscapeString(icc),
                            d.EscapeString(doc.documentDate.Substring(0, 8)), doc.documentUnitID,
                            d.EscapeString(doc.documentDate.Substring(0, 8)),
                            d.EscapeString(doc.documentDate.Substring(0, 8))
                            );



//                        _l("Сведения о SIM-карте добавлены успешно");
                    }
                    

                    docChanged = false;
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
