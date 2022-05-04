using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DEXExtendLib;

using System.Text.RegularExpressions;

namespace DEXPlugin.Dictionary.MTS.UnitsDP
{
    public partial class FCheckDP : Form
    {
        object toolbox;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };
        BindingSource bs;
        DataTable dt;
        Dictionary<string, string> dDid = new Dictionary<string, string>();
        public FCheckDP(object toolbox)
        {
            try
            {
                InitializeComponent();
                this.toolbox = toolbox;
                ArrayList dcs = ( (IDEXPluginSystemData)toolbox ).getPlugins().getDocuments();
                if ( dcs != null && dcs.Count > 0 )
                {
                    foreach ( IDEXPluginDocument dci in dcs )
                    {
                        dDid[dci.ID] = dci.Title;
                    }
                }

                dt = null;
                bs = new BindingSource();
                bs.DataSource = dt;
                dgv.DataSource = bs;

                IDEXConfig cfg = (IDEXConfig)toolbox;
                cbEnc.SelectedIndex = cfg.getInt(this.Name, "cbEnc", 0);
                cbSeparator.SelectedIndex = cfg.getInt(this.Name, "cbSeparator", 0);
                cbQuotes.Checked = cfg.getBool(this.Name, "cbQuotes", false);
            }
            catch ( Exception )
            {
            }
        }
        DataTable srcdt;
        public string FillDtSource(IWaitMessageEventArgs wmea)
        {
            wmea.textMessage = "Загрузка данных";
            wmea.canAbort = true;
            wmea.minValue = 0;
            try
            {
                DataTable t = srcdt;
                if (t != null && t.Columns.Count > 0 && t.Rows.Count > 0)
                {
                    int pv = 0;
                    wmea.maxValue = t.Rows.Count;
                    wmea.progressValue = pv;
                    wmea.progressVisible = true;
                    dt = new DataTable();
                    dt.Columns.Add("Наименование отделения", typeof(string)).Caption = "Наименование отделения";
                    dt.Columns.Add("Код точки МТС", typeof(string)).Caption = "Код точки МТС";
                    foreach (DataRow r in t.Rows)
                    {
                        DataRow dtr = dt.NewRow();
                        dtr["Наименование отделения"] = r[0].ToString();
                        dtr["Код точки МТС"] = r[1].ToString();
                        dt.Rows.Add(dtr);
                        wmea.progressValue = pv++;
                        if (pv % 50 == 0) wmea.DoEvents();
                        if (wmea.isAborted) return "Загрузка данных прервана";
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "Исключение <" + ex.Message + ">";
            }
            return "Не удалось загрузить данные, либо файл не содержит пригодные данные.";        
        } 
        private void bSrcFromBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                cms.Items.Clear();
                dt = null;
                btnAddDp.Enabled = false;
                srcdt = CSVParser.stringToTable(Clipboard.GetText(), separators[cbSeparator.SelectedIndex], cbQuotes.Checked, true);
                string ret = WaitMessage.Execute(new WaitMessageEvent(FillDtSource));
                srcdt = null;
                if (!ret.Equals(""))
                {
                    dt = null;
                    MessageBox.Show(ret);
                }
            }
            catch (Exception)
            {

            }
            bs.DataSource = dt;
        }
        private void bClearTable_Click(object sender, EventArgs e)
        {
            try {
                dt = null;
                bs.DataSource = null;
                cms.Items.Clear();
                btnAddDp.Enabled = false;
            } 
            catch (Exception) 
            {
                
            }
        }
        private void bStartCheck_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCheck = (DataTable)dt;
                if ( dtCheck != null )
                {
                    IDEXData d = (IDEXData)toolbox;
                    DataTable newTable = new DataTable();
                    cms.Items.Clear();
                    newTable.Columns.Add("Наименование отделения", typeof(string)).Caption = "Наименование отделения";
                    newTable.Columns.Add("Код точки МТС", typeof(string)).Caption = "Код точки МТС";
                    newTable.Columns.Add("Профиль", typeof(bool)).Caption = "Профиль";
                    newTable.Columns.Add("Будет добавлено в базу", typeof(bool)).Caption = "Будет добавлено в базу";
                    DataTable dtDpBase = d.getQuery("select t1.uid, t1.dpcode, t2.title from `mts_units_dp` as t1, `units` as t2 where  t1.uid = t2.uid");
                    int i;
                    foreach ( DataRow dr in dtCheck.Rows )
                    {
                        i = 0;
                        foreach ( DataRow drbase in dtDpBase.Rows )
                        {
                            i++;
                            if ( dr["Код точки МТС"].ToString() == drbase["dpCode"].ToString() )
                            {
                                break;
                            }
                            if ( dr["Код точки МТС"].ToString() != drbase["dpCode"].ToString() && ( dtDpBase.Rows.Count == i ) )
                            {
                                DataRow s = newTable.NewRow();
                                s["Наименование отделения"] = dr["Наименование отделения"].ToString();
                                s["Код точки МТС"] = dr["Код точки МТС"].ToString();
                                s["Профиль"] = false;
                                s["Будет добавлено в базу"] = false;
                                newTable.Rows.Add(s);
                            }
                        }
                    }
                    bs.DataSource = newTable;
                    if ( newTable.Rows.Count > 0 )
                        btnAddDp.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
        }
        private void selectMenu(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                cms.Visible = false;
                cms.Items.Clear();
                ( (DataRowView)bs.Current ).Row["Наименование отделения"] = sender.ToString();
                ( (DataRowView)bs.Current ).Row["Будет добавлено в базу"] = true;
            }
            catch ( Exception )
            {
            }
            
        }
        private void checkBoxClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cms.Items.Clear();
                if ( ( (DataRowView)bs.Current ).Row.Table.Columns.Count > 2 )
                {
                    if (!(bool)( (DataRowView)bs.Current ).Row["Будет добавлено в базу"])
                    {
                        IDEXData d = (IDEXData)toolbox;
                        string str = ( (DataRowView)bs.Current ).Row["Наименование отделения"].ToString();
                        string s = "";
                        string[] arrPatterns = new string[2] {@"(\S+)\s+(\S)\.\s*(\S)(\.|,)", @"([А-ЯЁ][а-яё]+[\-\s]?){3,}"};
                        for(int i = 0; i < arrPatterns.Length; i++)
                        {
                            Regex regex = new Regex(arrPatterns[i]);
                            if (regex.Match(str).Length != 0) 
                            {
                                Match mc = regex.Match(str);
                                if ( i == 0 ) 
                                {
                                    s = mc.Groups[1].Value;
                                }
                                else if ( i == 1 ) 
                                {
                                    string[] ss = mc.Groups[0].Value.ToString().Split(' ');
                                    s = ss[0];
                                }
                                break;
                            }
                        }

                        DataTable dtDpBase = d.getQuery("select title from `units` where title REGEXP '{0}' ", s);
                        if ( dtDpBase != null )
                        {
                            foreach ( DataRow dtloc in dtDpBase.Rows )
                            {
                                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                                tsmi.Text = dtloc["title"].ToString();
                                tsmi.MouseDown += new MouseEventHandler(this.selectMenu);
                                cms.Items.Add(tsmi);
                            }
                        }
                    }
                    if ( e.ColumnIndex == 2 )
                    {
                        DataRowView sss = ( (DataRowView)bs.Current );
                        sss.Row["Профиль"] = !(bool)sss.Row["Профиль"];
                    }
                }
            }
            catch ( Exception )
            {
            }
        }
        private void dataGridView1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    cms.Show(dgv, new Point(e.X, e.Y));
                }
            }
            catch ( Exception )
            {
            }
        }
        private void btnAddDp_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable ss = (DataTable)bs.DataSource;
                IDEXData d = (IDEXData)toolbox;
                int i = 0;
                while ( i < ss.Rows.Count )
                {
                    bool s = (bool)ss.Rows[i]["Будет добавлено в базу"];
                    if ( (bool)ss.Rows[i]["Будет добавлено в базу"] )
                    {
                        int kind = Convert.ToInt32((bool)ss.Rows[i]["Профиль"]);
                        string dpcode = ss.Rows[i]["Код точки МТС"].ToString();
                        DataTable dtDpBase = d.getQuery("select uid from `units` where  title = '{0}'", ss.Rows[i]["Наименование отделения"].ToString());
                        if ( dtDpBase.Rows.Count == 1 )
                        {
                            int uid = Convert.ToInt32(dtDpBase.Rows[0]["uid"]);
                            d.runQuery("insert into `mts_units_dp` (uid, dpcode, kind) values (" + uid + ", '" + dpcode + "', " + kind + ")");
                        }
                        ss.Rows[i].Delete();
                        ss.AcceptChanges();
                        i--;
                    }
                    i++;
                }
                cms.Items.Clear();
            }
            catch ( Exception )
            {
            }
        } 
    }
}
