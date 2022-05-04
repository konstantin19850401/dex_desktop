using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Function.ExportDocData
{
    public partial class ExportDocDataMain : Form
    {
        public ExportDocDataMain()
        {
            InitializeComponent();
        }
        
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };
        Object toolbox;

        string lastDocID;
        Dictionary<string, string> dUnits;

        public void InitForm(Object toolbox)
        {
            lastDocID = null;

            this.toolbox = toolbox;
            IDEXSysData sd = (IDEXSysData)toolbox;
            IDEXConfig cfg = (IDEXConfig)toolbox;
            IDEXData dd = (IDEXData)toolbox;

            deStart.Value = cfg.getDate(this.Name, "deStart", DateTime.Now);
            deEnd.Value = cfg.getDate(this.Name, "deEnd", DateTime.Now);
            cbQuotes.Checked = cfg.getBool(this.Name, "cbQuotes", false);
            cbSeparator.SelectedIndex = cfg.getInt(this.Name, "cbSeparator", 0);

            ArrayList docs = sd.DocumentTypes();

            string seld = cfg.getStr(this.Name, "cbDocType", "");
            StringObjTagItem selo = null;

            cbDocType.Items.Clear();
            foreach (IDEXPluginDocument doc in docs)
            {
                StringObjTagItem soti = new StringObjTagItem(doc.Title, doc);
                cbDocType.Items.Add(soti);
                if (seld.Equals(doc.ID)) selo = soti;
            }

            if (selo != null) cbDocType.SelectedItem = selo;

            DataTable dt = dd.getQuery("select uid, title from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, dt, "Любое отделение", "uid", "title", false);
            StringTagItem.SelectByTag(cbUnit, cfg.getStr(this.Name, "cbUnit",StringTagItem.VALUE_ANY), true);

            dUnits = new Dictionary<string, string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    dUnits[row["uid"].ToString()] = row["title"].ToString();
                }
            }

            RefreshCheckListBox();

        }

        public void SaveParams()
        {
            SaveCheckListBox();
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "deStart", deStart.Value);
            cfg.setDate(this.Name, "deEnd", deEnd.Value);
            cfg.setBool(this.Name, "cbQuotes", cbQuotes.Checked);
            cfg.setInt(this.Name, "cbSeparator", cbSeparator.SelectedIndex);
            if (lastDocID != null)
                cfg.setStr(this.Name, "cbDocType", lastDocID);
            if (cbUnit.SelectedItem != null)
                cfg.setStr(this.Name, "cbUnit", ((StringTagItem)cbUnit.SelectedItem).Tag);
        }

        void SaveCheckListBox()
        {
            if (lastDocID != null && clbFields.Items.Count > 0)
            {
                IDEXConfig cfg = (IDEXConfig)toolbox;
                string _sec = this.Name + "_sel_" + lastDocID;
                foreach (StringTagItem sti in clbFields.Items)
                {
                    cfg.setBool(_sec, sti.Tag, clbFields.CheckedItems.Contains(sti));
                }
            }

        }

        void RefreshCheckListBox()
        {
            SaveCheckListBox();
            clbFields.Items.Clear();
            if (cbDocType.Items.Count > 0 && cbDocType.SelectedItem != null && cbDocType.SelectedIndex > -1)
            {
                IDEXPluginDocument d = (IDEXPluginDocument)((StringObjTagItem)cbDocType.SelectedItem).Tag;
                lastDocID = d.ID;
                IDEXConfig cfg = (IDEXConfig)toolbox;
                string _sec = this.Name + "_sel_" + d.ID;
                
                Dictionary<string, string> fields = d.GetDocumentFields(toolbox);
                foreach (KeyValuePair<string, string> kvp in fields)
                {
                    StringTagItem sti = new StringTagItem(kvp.Value, kvp.Key);
                    clbFields.Items.Add(sti, cfg.getBool(_sec, kvp.Key, false));
                }
            }
        }

        private void cbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCheckListBox();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

            if (!rxdate.IsMatch(deStart.Text)) er += "* Некорректная дата начала интервала\n";
            if (!rxdate.IsMatch(deEnd.Text)) er += "* Некорректная дата конца интервала\n";

            if (cbDocType.SelectedItem == null || cbDocType.SelectedIndex < 0 || clbFields.Items.Count < 1)
                er += "* Не указан тип документа для экспорта\n";

            if (er.Equals(""))
            {
                if (deStart.Value > deEnd.Value) er += "* Дата начала интервала позже конечной даты\n";
                if (clbFields.CheckedItems.Count < 1) er += "* Не указаны поля для экспорта\n";
            }

            if (cbSeparator.SelectedItem == null || cbSeparator.SelectedIndex < 0)
                er += "* Не указан разделитель текста\n";

            if (er.Equals(""))
            {
                // TODO Экспорт данных в CSV


                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    gb1.Enabled = false;
                    gb2.Enabled = false;
                    gb3.Enabled = false;
                    bOk.Enabled = false;
                    bCancel.Enabled = false;

                    try
                    {

                        string sheader = _sappend(null, "Отделение");

                        ArrayList fields = new ArrayList();
                        foreach (StringTagItem sti in clbFields.CheckedItems)
                        {
                            fields.Add(sti.Tag);
                            sheader = _sappend(sheader, sti.ToString());
                        }

                        IDEXPluginDocument doc = (IDEXPluginDocument)((StringObjTagItem)cbDocType.SelectedItem).Tag;

                        IDEXData d = (IDEXData)toolbox;

                        string sql = string.Format(
                            "select * from `journal` where docid='{0}' and jdocdate >= '{1}000000000' and jdocdate <= '{2}235959999'",
                            d.EscapeString(doc.ID),
                            deStart.Value.ToString("yyyyMMdd"), deEnd.Value.ToString("yyyyMMdd")
                            );
                        if (cbUnit.SelectedItem != null)
                        {
                            string unittag = ((StringTagItem)cbUnit.SelectedItem).Tag;
                            if (!StringTagItem.VALUE_ANY.Equals(unittag))
                            {
                                try
                                {
                                    int.Parse(unittag);
                                    sql += " and unitid = " + unittag;
                                }
                                catch (Exception) { }
                            }
                        }

                        DataTable t = d.getQuery(sql);

                        if (t != null && t.Rows.Count > 0)
                        {
                            StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.Default);
                            sw.WriteLine(sheader);

                            foreach (DataRow r in t.Rows)
                            {
                                SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());

                                string uname = dUnits.ContainsKey(r["unitid"].ToString()) ? dUnits[r["unitid"].ToString()] : r["unitid"].ToString();

                                string sout = _sappend(null, uname);
                                foreach (string fld in fields)
                                {
                                    sout = _sappend(sout, xml.GetNodeByPath(fld, true).Text);
                                }

                                sw.WriteLine(sout);
                            }

                            sw.Close();
                            MessageBox.Show("Выгрузка завершена");
                        }
                        else
                        {
                            er = "Нет ни одного документа данного типа в указанном интервале.";
                        }
                    }
                    catch (Exception ex)
                    {
                        er = "Системная ошибка: " + ex.Message;
                    }

                    gb1.Enabled = true;
                    gb2.Enabled = true;
                    gb3.Enabled = true;
                    bOk.Enabled = true;
                    bCancel.Enabled = true;

                    if (er.Equals(""))
                        DialogResult = DialogResult.OK;
                    else
                        MessageBox.Show(er);
                }
            }
            else
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
        }

        private string _sappend(string src, string apnd)
        {
            string apnd2 = apnd;
            if (cbQuotes.Checked)
            {
                apnd2 = "\"" + apnd2.Replace("\"", "\\\"") + "\"";
            }

            if (src == null) return apnd2;

            return src + separators[cbSeparator.SelectedIndex] + apnd2;
        }
    }
}
