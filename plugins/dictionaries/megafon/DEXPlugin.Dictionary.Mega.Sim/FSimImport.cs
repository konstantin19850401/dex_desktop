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
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Mega.Sim
{
    public partial class FSimImport : Form
    {
        public Object toolbox;
        string[] separators = {((char)9).ToString() , ";", ":", "|", ".", ",", "!", "&" };

        StringTagItem fieldMsisdn, fieldIcc;

        public FSimImport()
        {
            InitializeComponent();
        }

        public void InitForm(Object toolbox)
        {
            this.toolbox = toolbox;

            dgvPreview.DataSource = null;
            dgvPreview.Visible = false;

            IDEXData d = (IDEXData)toolbox;
            IDEXConfig cfg = (IDEXConfig)toolbox;
            // Инициализация справочников

            DataTable t = d.getQuery("select * from `um_plans`");
            StringTagItem.UpdateCombo(cbPlan, t, null, "plan_id", "title", false);

            t = d.getQuery("select * from `um_regions`");
            StringTagItem.UpdateCombo(cbRegion, t, null, "region_id", "title", false);


            // Загрузка сохранённых значений выбора
            cbSeparator.SelectedIndex = cfg.getInt(this.Name, "cbSeparator", 0);

            cbRemoveQuotes.Checked = cfg.getBool(this.Name, "cbRemoveQuotes", true);
//            nudPartyNum.Value = cfg.getInt(this.Name, "nudPartyNum", 1);
            StringTagItem.SelectByTag(cbPlan, cfg.getStr(this.Name, "cbPlan", ""), true);
            StringTagItem.SelectByTag(cbRegion, cfg.getStr(this.Name, "cbRegion", ""), true);

            // новый биллинг
            cbNewBilling.Checked = cfg.getBool(this.Name, "cbNewBilling", true);

            cbBalance.Items.Clear();
            t = d.getQuery("select * from `um_balances` order by title");
            StringTagItem.UpdateCombo(cbBalance, t, null, "title", "title", true);                
            cbBalance.Text = cfg.getStr(this.Name, "tbBalance", "");

            tbSrcFile.Text = cfg.getStr(this.Name, "tbSrcFile", "");

            cbLoadIntoTextEditor.Checked = cfg.getBool(this.Name, "cbLoadIntoTextEditor", false);
            cbOldToCurrent.Checked = cfg.getBool(this.Name, "cbOldToCurrent", false);
            cbDublicateMsisdn.SelectedIndex = cfg.getInt(this.Name, "cbDublicateMsisdn", 0);

            tbCheckMSISDN.Text = cfg.getStr(this.Name, "tbCheckMSISDN", "");
            tbCheckICC.Text = cfg.getStr(this.Name, "tbCheckICC", "");
            cbShowOnlyCorrect.Checked = cfg.getBool(this.Name, "cbShowOnlyCorrect", true);

            Dictionary<string, object> dobjs = new Dictionary<string, object>();
            fieldMsisdn = new StringTagItem("MSISDN", "msisdn");
            dobjs["msisdn"] = fieldMsisdn;
            fieldIcc = new StringTagItem("ICC", "icc");
            dobjs["icc"] = fieldIcc;
            dobjs["region_id"] = new StringTagItem("Регион", "region_id");
            dobjs["plan_id"] = new StringTagItem("Тарифный план", "plan_id");
            dobjs["party_id"] = new StringTagItem("Партия", "party_id");
            dobjs["balance"] = new StringTagItem("Баланс", "balance");

            clbFields.Items.Clear();
            string sk = this.Name + ".clbFields";

            int ccnt = cfg.getInt(sk, "count", 0);
            if (ccnt > 0)
            {
                for (int f = 0; f < ccnt; ++f)
                {
                    string k = cfg.getStr(sk, "field_" + f.ToString(), null);
                    bool b = cfg.getBool(sk, "state_" + f.ToString(), false);
                    if (k != null && dobjs.ContainsKey(k))
                    {
                        int idx = clbFields.Items.Add(dobjs[k]);
                        clbFields.SetItemChecked(idx, b);
                        dobjs.Remove(k);                                                
                    }
                }
            }

            foreach (KeyValuePair<string, object> kvp in dobjs)
            {
                clbFields.Items.Add(kvp.Value);
            }

            bNewParty_Click(null, null);
        }

        public void SaveFormParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            
            cfg.setInt(this.Name, "cbSeparator", cbSeparator.SelectedIndex);
            cfg.setBool(this.Name, "cbRemoveQuotes", cbRemoveQuotes.Checked);
//            cfg.setStr(this.Name, "nudPartyNum", nudPartyNum.Value.ToString());
            
            if (cbPlan.SelectedIndex > -1) cfg.setStr(this.Name, "cbPlan", ((StringTagItem)cbPlan.SelectedItem).Tag);
            if (cbRegion.SelectedIndex > -1) cfg.setStr(this.Name, "cbRegion", ((StringTagItem)cbRegion.SelectedItem).Tag);
            cfg.setStr(this.Name, "tbBalance", cbBalance.Text);

            cfg.setStr(this.Name, "tbSrcFile", tbSrcFile.Text);

            cfg.setBool(this.Name, "cbLoadIntoTextEditor", cbLoadIntoTextEditor.Checked);
            cfg.setBool(this.Name, "cbOldToCurrent", cbOldToCurrent.Checked);
            cfg.setInt(this.Name, "cbDublicateMsisdn", cbDublicateMsisdn.SelectedIndex);

            cfg.setStr(this.Name, "tbCheckMSISDN", tbCheckMSISDN.Text);
            cfg.setStr(this.Name, "tbCheckICC", tbCheckICC.Text);
            cfg.setBool(this.Name, "cbShowOnlyCorrect", cbShowOnlyCorrect.Checked);

            cfg.setBool(this.Name, "cbNewBilling", cbNewBilling.Checked);

            string sk = this.Name + ".clbFields";
            int f = 0;
            foreach (StringTagItem sti in clbFields.Items)
            {
                cfg.setStr(sk, "field_" + f.ToString(), sti.Tag);
                cfg.setBool(sk, "state_" + f.ToString(), clbFields.CheckedItems.Contains(sti));
                ++f;
            }
            cfg.setInt(sk, "count", f);

        }

        private void bNewParty_Click(object sender, EventArgs e)
        {
            IDEXData d = (IDEXData)toolbox;
            DataTable t = d.getQuery("select MAX(party_id) mparty_id from `um_data`");
            if (t != null && t.Rows.Count > 0)
            {
                try
                {
                    nudPartyNum.Value = int.Parse(t.Rows[0]["mparty_id"].ToString()) + 1;
                }
                catch (Exception)
                {
                    nudPartyNum.Value = 1;
                    MessageBox.Show("Невозможно получить код последней партии из БД");
                }
            }
        }

        private void bFieldUp_Click(object sender, EventArgs e)
        {
            if (clbFields.SelectedIndex > 0)
            {
                Object o = clbFields.Items[clbFields.SelectedIndex - 1];
                bool c = clbFields.GetItemChecked(clbFields.SelectedIndex - 1);

                clbFields.Items[clbFields.SelectedIndex - 1] = clbFields.Items[clbFields.SelectedIndex];
                clbFields.SetItemChecked(clbFields.SelectedIndex - 1, clbFields.GetItemChecked(clbFields.SelectedIndex));
                clbFields.Items[clbFields.SelectedIndex] = o;
                clbFields.SetItemChecked(clbFields.SelectedIndex, c);
                clbFields.SelectedIndex--;
            }
        }

        private void bFieldDown_Click(object sender, EventArgs e)
        {
            if (clbFields.SelectedIndex < clbFields.Items.Count - 1)
            {
                Object o = clbFields.Items[clbFields.SelectedIndex + 1];
                bool c = clbFields.GetItemChecked(clbFields.SelectedIndex + 1);
                clbFields.Items[clbFields.SelectedIndex + 1] = clbFields.Items[clbFields.SelectedIndex];
                clbFields.SetItemChecked(clbFields.SelectedIndex + 1, clbFields.GetItemChecked(clbFields.SelectedIndex));
                clbFields.Items[clbFields.SelectedIndex] = o;
                clbFields.SetItemChecked(clbFields.SelectedIndex, c);
                clbFields.SelectedIndex++;
            }
        }

        private void clbFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            StringTagItem sti = (StringTagItem)clbFields.Items[e.Index];

            if (sti.Tag.Equals("party_id"))
            {
                nudPartyNum.Enabled = (e.NewValue == CheckState.Unchecked);
                bNewParty.Enabled = (e.NewValue == CheckState.Unchecked);
            }
            if (sti.Tag.Equals("plan_id")) cbPlan.Enabled = (e.NewValue == CheckState.Unchecked);
            if (sti.Tag.Equals("region_id")) cbRegion.Enabled = (e.NewValue == CheckState.Unchecked);
            if (sti.Tag.Equals("balance")) cbBalance.Enabled = (e.NewValue == CheckState.Unchecked);
        }


        private void LoadSource(string src)
        {
            if (src.Trim().Equals(""))
            {
                MessageBox.Show("Предоставленный источник не содержит никакой информации.");
                return;
            }

            if (cbLoadIntoTextEditor.Checked)
            {
                FSimSrcView sview = new FSimSrcView();
                sview.tb.Text = src;
                sview.ShowDialog();
            }
            else
            {
                // Загрузка из буфера в таблицу
                string er = "";
                if (!clbFields.CheckedItems.Contains(fieldMsisdn) || !clbFields.CheckedItems.Contains(fieldIcc))
                {
                    er += "* Должны быть помечены обязательные поля MSISDN и ICC\n";
                }

                if (cbSeparator.SelectedIndex < 0)
                {
                    er += "* Не указан разделитель значений\n";
                }

                if (nudPartyNum.Enabled && !nudPartyNum.Text.Equals(nudPartyNum.Value.ToString()))
                {
                    er += "* Указано некорректное значение партии\n";
                }

                if (cbPlan.Enabled && (cbPlan.SelectedIndex < 0 || cbPlan.SelectedItem == null))
                {
                    er += "* Не указан тарифный план для SIM-карт\n";
                }

                if (cbRegion.Enabled && (cbRegion.SelectedIndex < 0 || cbRegion.SelectedItem == null))
                {
                    er += "* Не указан регион для SIM-карт\n";
                }

                if (cbBalance.Enabled && (cbBalance.Text.Trim().Equals("")))
                {
                    er += "* Не указан баланс для SIM-карт\n";
                }

                if (er == "")
                {
                    gbTable.Text = "Предпросмотр загружаемых карт";

                    pb.Minimum = 0;
                    pb.Maximum = 100;
                    pb.Value = 0;
                    _ctlstate(false);
                    IDEXData d = (IDEXData)toolbox;
                    DataTable sprt = d.getQuery("select * from `um_plans`");
                    Dictionary<string, string> dicPlan = new Dictionary<string,string>();
                    if (sprt != null && sprt.Rows.Count > 0)
                    {
                        foreach(DataRow r in sprt.Rows)
                        {
                            dicPlan[r["plan_id"].ToString()] = r["title"].ToString();
                        }
                    }

                    sprt = d.getQuery("select * from `um_regions`");
                    Dictionary<string, string> dicRegion = new Dictionary<string,string>();
                    if (sprt != null && sprt.Rows.Count > 0)
                    {
                        foreach(DataRow r in sprt.Rows)
                        {
                            dicRegion[r["region_id"].ToString()] = r["title"].ToString();
                        }
                    }

                    dgvPreview.DataSource = null;

                    DataTable t = new DataTable();
                    dgvPreview.Columns.Clear();

                    string[] fnames =   { "msisdn", "icc",    "party_id", "plan_id", "plan_title", "region_id", "region_title", "balance", "status",    "old" };
                    string[] fcaption = { "MSISDN", "ICC",    "Партия",   null,      "ТП",         null,        "Регион",       "Баланс",  "Состояние", "Существует" };
                    string[] ftypes =   { "String", "String", "Int32",    "String",  "String",     "String",    "String",       "String",  "String",    "String" };

                    int fcnt = fnames.Length;
                    for(int f = 0; f < fcnt; ++f)
                    {
                        DataColumn col = t.Columns.Add();
                        col.ColumnName = fnames[f];
                        col.Caption = (fcaption[f] != null) ? fcaption[f] : fnames[f];
                        col.DataType = Type.GetType("System." + ftypes[f]);
                        DataGridViewColumn dgvc = new DataGridViewTextBoxColumn();
                        dgvc.Name = fnames[f];
                        dgvc.DataPropertyName = fnames[f];
                        dgvc.HeaderText = col.Caption;
                        dgvc.ValueType = col.DataType;
//                        dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                        dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgvc.Visible = (fcaption[f] != null);
                        dgvc.DisplayIndex = f;
                        dgvPreview.Columns.Add(dgvc);
                    
                    }


                    Dictionary<string, object> flds = new Dictionary<string,object>();
                    foreach (StringTagItem sti in clbFields.CheckedItems)
                    {
                        flds[sti.Tag] = sti;
                    }

                    int arbFieldsCount = flds.Count;


                    Dictionary<string, string> amsi = new Dictionary<string, string>();


                    string[] sep = { separators[cbSeparator.SelectedIndex] };
                    
                    StringReader sr = new StringReader(src);
                    ArrayList strs = new ArrayList();
                    while (true)
                    {
                        string ln = sr.ReadLine();
                        if (ln == null) break;
                        else 
                            if (!ln.Trim().Equals("")) strs.Add(ln);
                    }

                    pb.Maximum = strs.Count;

                    int cerrcnt = 0, cgoodcnt = 0, citocnt = 0;

                    foreach(string ln in strs)
                    {
                        string rer = "";

                        bool carderr = false;

                        DataRow r = t.NewRow();

                        string[] p = ln.Split(sep, StringSplitOptions.None);
                        if (p.Length < arbFieldsCount) rer += "Недостаточно полей в строке источника; ";

                        if (rer == "")
                        {
                            int c = 0;
                            foreach (KeyValuePair<string, object> kvp in flds)
                            {
                                r[kvp.Key] = _unq(p[c].Trim());
                                c++;
                            }

                            if (!flds.ContainsKey("plan_id"))
                            {
                                r["plan_id"] = ((StringTagItem)cbPlan.SelectedItem).Tag;
                            }

                            if (!flds.ContainsKey("region_id"))
                            {
                                r["region_id"] = ((StringTagItem)cbRegion.SelectedItem).Tag;
                            }

                            if (!flds.ContainsKey("party_id"))
                            {
                                r["party_id"] = Int32.Parse(nudPartyNum.Value.ToString());
                            }

                            if (!flds.ContainsKey("balance"))
                            {
                                r["balance"] = cbBalance.Text;
                            }

                            if ( dicPlan.ContainsKey(r["plan_id"].ToString()))
                            {
                                r["plan_title"] = dicPlan[r["plan_id"].ToString()].ToString();
                            }
                            else
                            {
                                if (r["plan_id"].ToString() == "") {
                                     r["plan_title"] = "";
                                } else {
                                    rer += "Тарифный план не найден в справочнике; ";
                                    carderr = true;
                                }
                               
                            }

                            if (dicRegion.ContainsKey(r["region_id"].ToString()))
                            {
                                r["region_title"] = dicRegion[r["region_id"].ToString()].ToString();
                            }
                            else
                            {
                                rer += "Регион не найден в справочнике; ";
                                carderr = true;
                            }

                            bool chkdb = true;

                            if (!_maskeq(r["msisdn"].ToString(), tbCheckMSISDN.Text.Trim()))
                            {
                                rer += "Некорректный MSISDN; ";
                                chkdb = false;
                                carderr = true;
                            }

                            if (!_maskeq(r["icc"].ToString(), tbCheckICC.Text.Trim()))
                            {
                                rer += "Некорректный ICC; ";
                                chkdb = false;
                                carderr = true;
                            }

                            if (chkdb)
                            {
                                if (amsi.ContainsKey(r["msisdn"].ToString()))
                                {
                                    rer += "MSISDN дублируется в загружаемом списке; ";
                                    chkdb = false;
                                    carderr = true;
                                }
                                else
                                {
                                    amsi[r["msisdn"].ToString()] = r["icc"].ToString();
                                }
                            }

                            if (chkdb)
                            {
                                DataTable t2 = d.getQuery(string.Format(
                                    "select msisdn, icc, party_id from `um_data` where msisdn='{0}' or icc='{0}'",
                                    d.EscapeString(r["msisdn"].ToString()), d.EscapeString(r["icc"].ToString())
                                    ));
                                bool emsi = false, eicc = false;

                                string opid = "";

                                if (t2 != null && t2.Rows.Count > 0)
                                {
                                    foreach (DataRow r2 in t2.Rows)
                                    {
                                        if (r2["msisdn"].ToString().Equals(r["msisdn"].ToString()))
                                        {
                                            emsi = true;
                                            opid = r2["party_id"].ToString();
                                        }

                                        if (r2["icc"].ToString().Equals(r["icc"].ToString()) && (!(emsi && cbOldToCurrent.Checked))) eicc = true;
                                    }
                                }

                                if (emsi)
                                {
                                    r["old"] = opid;
                                    //rer += "Карта с таким MSISDN уже есть; ";
                                }
                                if (eicc)
                                {
                                    rer += "Карта с таким ICC уже есть; ";
                                    carderr = true;
                                }
                            }
                        }

                        r["status"] = rer;

                        citocnt++;
                        if (carderr) cerrcnt++;
                        else cgoodcnt++;

                        t.Rows.Add(r);
                        pb.Value++;
                        pb.Refresh();
                    }

                    if (t.Rows.Count > 0)
                    {
                        dgvPreview.DataSource = t;
                        dgvPreview.Visible = true;
                        cbShowOnlyCorrect_CheckedChanged(cbShowOnlyCorrect, new EventArgs());
                    }
                    else
                    {
                        dgvPreview.DataSource = null;
                        dgvPreview.Visible = false;
                    }

                    gbTable.Text = string.Format(
                        "Предпросмотр загружаемых карт (Ошибочных: {0}, Корректных: {1}, Всего: {2})",
                        cerrcnt, cgoodcnt, citocnt
                        );


                    _ctlstate(true);
                }
                else
                {
                    MessageBox.Show("Ошибки:\n\n" + er);
                }
            }
        }

        private string _unq(string src)
        {
            string ret = src.Trim();
            if (cbRemoveQuotes.Checked && ret.Length > 0)
            {
                if (ret[0] == '"') ret = ret.Substring(1);
                if (ret.Length > 0)
                {
                    if (ret[ret.Length - 1] == '"')
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                }
            }
            return ret.Trim();
        }

        private bool _maskeq(string src, string mask)
        {
            if (src == null) return src == mask;
            if (mask == null || mask.Length < 1) return true;
            if (src.Length != mask.Length) return false;

            string csrc = src.ToLower(), cmask = mask.ToLower();

            for (int f = 0; f < csrc.Length; ++f)
            {
                char c = cmask[f];
                if (((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z')) && csrc[f] != c) return false;
                if (c == '*' && (csrc[f] < '0' || csrc[f] > '9')) return false;
                if (c == '#' && (csrc[f] < 'a' || csrc[f] > 'z')) return false;
            }
            return true;
        }

        private void bLoadFromClipboard_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                LoadSource(Clipboard.GetText());
            }
            else
                MessageBox.Show("В буфере обмена содержится не текстовая информация.");
        }

        private void bSelectSrcFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbSrcFile.Text = ofd.FileName;
            }
        }

        private void bLoadFromSrcFile_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(tbSrcFile.Text))
            {
                string srcf = null;
                try
                {
                    srcf = System.IO.File.ReadAllText(tbSrcFile.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Невозможно прочесть файл.");
                }
                if (srcf != null) LoadSource(srcf);
            }
            else
                MessageBox.Show("Файл не указан или отсутствует.");
        }

        private void dgvPreview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewRow r = dgvPreview.Rows[e.RowIndex];
                if (r.Cells["status"].Value.ToString().Trim().Equals(""))
                {
                    r.DefaultCellStyle.BackColor = Color.FromArgb(199, 253, 203);

                    if (!r.Cells["old"].Value.ToString().Trim().Equals(""))
                    {
                        r.DefaultCellStyle.BackColor = Color.FromArgb(249, 247, 40);
                    }
                }
                else
                {
                    r.DefaultCellStyle.BackColor = Color.FromArgb(253, 199, 199);
                }
            }
        }

        private void _ctlstate(bool st)
        {
            gbTable.Enabled = st;
            groupBox2.Enabled = st;
            groupBox3.Enabled = st;
            groupBox4.Enabled = st;
            groupBox5.Enabled = st;
            bOk.Enabled = st;
            bCancel.Enabled = st;
            lOpProgress.Visible = !st;
            pb.Visible = !st;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";
            bool warn = false;
            DataTable t = (DataTable)(dgvPreview.DataSource);

            if (t == null || t.Rows.Count < 1)
            {
                er += "* Нет ни одной записи для импорта\n";
            }

            if (er == "")
            {
                int good = 0, bad = 0;
                foreach (DataRow r in t.Rows)
                {
                    if (r["status"].ToString().Trim().Equals("") && (r["old"].ToString().Trim().Equals("") || cbOldToCurrent.Checked))
                    {
                        good++;
                    }
                    else
                    {
                        bad++;
                    }
                }

                if (good == 0)
                {
                    er += "* Нет ни одной записи для импорта\n";
                }
                else
                {
                    warn = bad > 0;
                }
            }

            if (er == "")
            {
                if (!warn || MessageBox.Show("В списке есть записи, помеченные как ошибочные.\n" +
                    "В справочник будут импортированы только корректные SIM-карты.\n\n" +
                    "Продолжить?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    pb.Minimum = 0;
                    pb.Maximum = t.Rows.Count;
                    pb.Value = 0;
                    _ctlstate(false);


                    IDEXData dtb = (IDEXData)toolbox;
                    string datein = dtb.EscapeString(DateTime.Now.ToString("yyyyMMdd"));

                    int impc = 0;
                    int billing = cbNewBilling.Checked ? 1 : 0;
                    foreach (DataRow r in t.Rows)
                    {
                        if (r["status"].ToString().Trim().Equals("") && (r["old"].ToString().Trim().Equals("") || cbOldToCurrent.Checked))
                        {
                            if (!r["old"].ToString().Equals(""))
                            {
                                if (cbDublicateMsisdn.SelectedIndex == 0)
                                {
                                    dtb.runQuery(string.Format(
                                        "insert into `um_data_out` (status, status_j, msisdn, icc, fs, billing, date_in, owner_id, date_own, date_sold, " +
                                        "region_id, party_id, plan_id, balance, comment, data) SELECT status, status_j, msisdn, icc, fs, date_in, owner_id, " +
                                        "date_own, date_sold, region_id, party_id, plan_id, balance, comment, data FROM `um_data` WHERE msisdn = '{0}'",
                                        r["msisdn"].ToString()));
                                    //string ppp = "insert into `um_data_out` (status, status_j, msisdn, icc, fs, date_in, owner_id, date_own, date_sold, " +
                                     //   "region_id, party_id, plan_id, balance, comment, data) SELECT status, status_j, msisdn, icc, fs, date_in, owner_id, " +
                                     //   "date_own, date_sold, region_id, party_id, plan_id, balance, comment, data FROM `um_data` WHERE msisdn = '"+
                                     //   r["msisdn"].ToString()+"'";
                                }

                                dtb.runQuery(string.Format(
                                    "delete from `um_data` where msisdn = '{0}'",
                                    r["msisdn"].ToString()));
                            }

                            int ifs = cbFS.Checked ? 1 : 0;
                            

                            string sql = "insert into `um_data` (status, status_j, msisdn, icc, fs, billing, date_in, owner_id, date_own, date_sold, region_id, party_id, " +
                                         "plan_id, balance, data) values (0, 0,'" +
                                         dtb.EscapeString(r["msisdn"].ToString()) + "', '" + dtb.EscapeString(r["icc"].ToString()) + "', " + ifs.ToString() +
                                         ", " + billing.ToString() + ",'" + datein + "', -1, '', '', '" + dtb.EscapeString(r["region_id"].ToString()) + 
                                         "', " + r["party_id"].ToString() + ", '" + dtb.EscapeString(r["plan_id"].ToString()) + 
                                         "', '" + dtb.EscapeString(r["balance"].ToString()) + "', '')";

                            dtb.runQuery(sql);
                                
                            impc++;
                        }
                        pb.Value++;
                        pb.Refresh();
                    }
                    _ctlstate(true);
                    MessageBox.Show(string.Format("Импортировано карт: {0}", impc));
                    DialogResult = DialogResult.OK;
                }
            }


        }

        private void dgvPreview_Sorted(object sender, EventArgs e)
        {
            dgvPreview.Refresh();
        }

        private void cbShowOnlyCorrect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable t = (DataTable)dgvPreview.DataSource;
                if (t != null) t.DefaultView.RowFilter = cbShowOnlyCorrect.Checked ? "status = ''" : "";
            }
            catch (Exception) { }
        }

    }
}
