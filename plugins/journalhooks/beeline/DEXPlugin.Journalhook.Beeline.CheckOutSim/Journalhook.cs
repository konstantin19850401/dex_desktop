using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Drawing;
using System.Data;
using MySql.Data.MySqlClient;

namespace DEXPlugin.Journalhook.Beeline.CheckOutSim
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.Beeline.CheckOutSim";
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
                    DataTable t = null;
                    if ("1".Equals(xml["Dynamic"].Text))
                    {
                        t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}' and msisdn = '{1}'",
                            d.EscapeString(xml["ICC"].Text), d.EscapeString(xml["MSISDN"].Text)));
                        if (t == null || t.Rows.Count == 0)
                        {
                            t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}' and msisdn = '{1}'",
                            d.EscapeString(xml["ICC"].Text), d.EscapeString(xml["OLDNEWDOLMSISDN"].Text)));
                        }
                    }
                    else
                    {
                        t = d.getQuery(string.Format("select * from `um_data` where msisdn = '{0}'",
                            d.EscapeString(xml["MSISDN"].Text)));
                    }


                    t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}' and msisdn like '%{1}%'",
                            d.EscapeString(xml["ICC"].Text), d.EscapeString(xml["checkCode"].Text)));
                    if (t == null || t.Rows.Count == 0)
                    {
                        t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}' and msisdn = '{1}'",
                            d.EscapeString(xml["ICC"].Text), d.EscapeString(xml["MSISDN"].Text)));
                    }



                    string icc = xml["ICC"].Text;
                    
                    if (t != null && t.Rows.Count == 1)
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

                        try{
                            // динамическая ли сим, то в справочнике сим-карт сменим номер на новый
                            /*
                            if ("1".Equals(xml["Dynamic"].Text))
                            {
                                if (t.Rows[0]["msisdn"].ToString().Equals(xml["OLDNEWDOLMSISDN"].Text))
                                {
                                    d.runQuery(
                                       "update `um_data` set msisdn = '{0}' where msisdn = '{1}' and icc = '{2}'",
                                       d.EscapeString(xml["MSISDN"].Text), d.EscapeString(xml["OLDNEWDOLMSISDN"].Text), d.EscapeString(icc)
                                    );
                                }
                            }
                            */
                            if ("1".Equals(t.Rows[0]["dynamic"].ToString()))
                            {
                                //if (t.Rows[0]["msisdn"].ToString().Equals(xml["OLDNEWDOLMSISDN"].Text))
                                //{
                                d.runQuery(
                                   "update `um_data` set msisdn = '{0}', dynamic = '0' where msisdn like '%{1}%' and icc = '{2}'",
                                   d.EscapeString(xml["MSISDN"].Text), d.EscapeString(xml["checkCode"].Text), d.EscapeString(icc)
                                );
                                //}
                            }
                            
                        } catch (Exception) {}
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
                            "icc = '{2}', comment = '{3}' where msisdn = '{4}'",
                            doc.documentUnitID, d.EscapeString(doc.documentDate.Substring(0, 8)),
                            d.EscapeString(icc), d.EscapeString(remark), d.EscapeString(xml["MSISDN"].Text)
                            );
                    }
                    else if (t != null && t.Rows.Count > 1)
                    {
                        MessageBox.Show("В справочнике сим карт обнаружено более одной записи");
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
                        //t = d.getQuery(string.Format("select * from `um_data` where icc = '{0}' and msisdn = '{1}'",
                        //    d.EscapeString(xml["ICC"].Text), d.EscapeString(xml["MSISDN"].Text)));
                 

                        d.runQuery(
                            "insert into `um_data` (status, msisdn, icc, date_in, owner_id, date_own, " +
                            "date_sold, region_id, party_id, plan_id) values (2, '{0}', '{1}', '{2}', " +
                            "{3}, '{4}', '{5}', '-', 1, '-')",
                            d.EscapeString(xml["MSISDN"].Text), d.EscapeString(icc),
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
