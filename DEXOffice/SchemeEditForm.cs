using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using DEXExtendLib;

namespace DEXOffice
{
    public partial class SchemeEditForm : Form
    {
        public SimpleXML template, deftemplate;
        DataTable dtp;
        string printerTitle;
        bool changed;

        public SchemeEditForm(string printerTitle, SimpleXML template, SimpleXML deftemplate)
        {
            InitializeComponent();
            this.template = template;
            this.deftemplate = deftemplate;

            this.printerTitle = printerTitle;
            lPrinterTitle.Text = printerTitle;

            loadTemplate();
        }

        private void loadTemplate()
        {
            lDocTitle.Text = this.template["Title"].Text;

            DEXToolBox tb = DEXToolBox.getToolBox();
            string schemesdir = tb.DataDir + @"\printing_schemes\";

            string[] files = Directory.GetFiles(schemesdir, "*.bmp", SearchOption.TopDirectoryOnly);
            cbCover.Items.Clear();
            if (files != null && files.Length > 0)
            {
                foreach (string fname in files)
                {
                    cbCover.Items.Add(Path.GetFileName(fname));
                }

                string ucover = this.template["Cover"].Text;
                int ucoverid = cbCover.Items.IndexOf(ucover);
                cbCover.SelectedIndex = ucoverid;

                cbUseCover.Checked = this.template["UseCover"].Text.Equals("1");
                if (ucoverid < 0) cbUseCover.Checked = false;

                cbCover.Enabled = true;
                cbUseCover.Enabled = true;
            }
            else
            {
                cbCover.Enabled = false;
                cbUseCover.Checked = false;
                cbUseCover.Enabled = false;
            }

            nudCoverOffsetX.Value = _nudval((decimal)_float(this.template["CoverOffset"].SafeAttribute("x", "0"), 0), nudCoverOffsetX);
            nudCoverOffsetY.Value = _nudval((decimal)_float(this.template["CoverOffset"].SafeAttribute("y", "0"), 0), nudCoverOffsetY);
            nudCoverScale.Value = _nudval((decimal)_float(this.template["CoverScale"].Text.Trim(), 100), nudCoverScale);

            cbOrientation.SelectedIndex = this.template["Orientation"].Text.Equals("landscape") ? 1 : 0;
            cbRotate.Checked = this.template["Rotate"].Text.Equals("1") || this.template["Rotate"].Text.Equals("yes");

            Font f;

            try
            {
                f = new Font(this.template["Font"].SafeAttribute("name", "Arial"),
                    float.Parse(this.template["Font"].SafeAttribute("size", "11")),
                    (FontStyle)int.Parse(this.template["Font"].SafeAttribute("style", "0"))
                    );
            }
            catch (Exception)
            {
                f = new Font("Arial", 11, FontStyle.Regular);
            }

            fd.Font = f;
            UpdateFontButtonText();

            nudFieldsOffsetX.Value = _nudval((decimal)_float(this.template["FieldsOffset"].SafeAttribute("x", "0"), 0), nudFieldsOffsetX);
            nudFieldsOffsetY.Value = _nudval((decimal)_float(this.template["FieldsOffset"].SafeAttribute("y", "0"), 0), nudFieldsOffsetY);
            nudFieldsScaleX.Value = _nudval((decimal)_float(this.template["FieldsScale"].SafeAttribute("x", "100"), 100), nudFieldsScaleX);
            nudFieldsScaleY.Value = _nudval((decimal)_float(this.template["FieldsScale"].SafeAttribute("y", "100"), 100), nudFieldsScaleY);

            dtp = new DataTable();
            dtp.Columns.Add("enabled", typeof(bool));
            dtp.Columns.Add("title", typeof(string));
            dtp.Columns.Add("x", typeof(string));
            dtp.Columns.Add("y", typeof(string));
            dtp.Columns.Add("w", typeof(string));
            dtp.Columns.Add("h", typeof(string));
            dtp.Columns.Add("interval", typeof(string));
            dtp.Columns.Add("node", typeof(SimpleXML));

            dgv.AutoGenerateColumns = false;

            dgv.Columns.Clear();
            dgv.Columns.Add(_newcol(new DataGridViewCheckBoxColumn(false), "enabled", "Вкл.", false));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "title", "Наименование поля", true));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "x", "X", false));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "y", "Y", false));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "w", "Ширина", false));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "h", "Высота", false));
            dgv.Columns.Add(_newcol(new DataGridViewTextBoxColumn(), "interval", "Интервал", false));

            dgv.DataSource = dtp;

            foreach (SimpleXML fld in this.template["Fields"].Child)
            {
                DataRow nr = dtp.NewRow();
                nr["enabled"] = fld.SafeAttribute("enabled", "false").Equals("true");
                nr["title"] = fld.SafeAttribute("title", "-");
                nr["x"] = fld.SafeAttribute("x", "");
                nr["y"] = fld.SafeAttribute("y", "");
                nr["w"] = fld.SafeAttribute("w", "");
                nr["h"] = fld.SafeAttribute("h", "");
                nr["interval"] = fld.SafeAttribute("interval", "");
                nr["node"] = fld;
                dtp.Rows.Add(nr);
            }

            changed = false;
        }

        private SimpleXML getTemplate()
        {
            SimpleXML ret = new SimpleXML("Scheme");
            ret["ID"].Text = deftemplate["ID"].Text;
            ret["Title"].Text = deftemplate["Title"].Text;
            ret["GUID"].Text = deftemplate["GUID"].Text;
            ret["Cover"].Text = (cbCover.SelectedIndex > -1 && cbCover.SelectedItem != null) ? (string)cbCover.SelectedItem : "";
            ret["UseCover"].Text = (cbUseCover.Checked ? "1" : "0");
            ret["Orientation"].Text = (cbOrientation.SelectedIndex == 1 ? "landscape" : "portrait");
            ret["Rotate"].Text = (cbRotate.Checked ? "yes" : "no");

            SimpleXML cofs = ret["CoverOffset"];
            cofs.Attributes["x"] = nudCoverOffsetX.Value.ToString();
            cofs.Attributes["y"] = nudCoverOffsetY.Value.ToString();

            ret["CoverScale"].Text = nudCoverScale.Value.ToString();
            
            SimpleXML cfnt = ret["Font"];
            cfnt.Attributes["Name"] = fd.Font.Name;
            cfnt.Attributes["Size"] = fd.Font.Size.ToString();

            SimpleXML cfofs = ret["FieldsOffset"];
            cfofs.Attributes["x"] = nudFieldsOffsetX.Value.ToString();
            cfofs.Attributes["y"] = nudFieldsOffsetY.Value.ToString();

            SimpleXML cfsc = ret["FieldsScale"];
            cfsc.Attributes["x"] = nudFieldsScaleX.Value.ToString();
            cfsc.Attributes["y"] = nudFieldsScaleY.Value.ToString();

            SimpleXML flds = ret["Fields"];
            foreach (DataRow r in dtp.Rows)
            {
                SimpleXML fld = ((SimpleXML)r["node"]).Clone(true);
                fld.Attributes["x"] = r["x"].ToString();
                fld.Attributes["y"] = r["y"].ToString();
                fld.Attributes["w"] = r["w"].ToString();
                fld.Attributes["h"] = r["h"].ToString();
                try
                {
                    fld.Attributes["enabled"] = (bool.Parse(r["enabled"].ToString()) ? "true" : "false");
                }
                catch (Exception)
                {
                    fld.Attributes["enabled"] = "false";
                }
                fld.Parent = flds;
            }

            return ret;
        }

        private DataGridViewColumn _newcol(DataGridViewColumn src, string colname, string coltitle, bool ro)
        {
            DataGridViewColumn ret = src;
            ret.Name = colname;
            ret.HeaderText = coltitle;
            ret.DataPropertyName = colname;
            ret.ReadOnly = ro;
            return ret;

        }

        decimal _nudval(decimal value, NumericUpDown nud)
        {
            decimal ret = value;
            if (ret < nud.Minimum) ret = nud.Minimum;
            if (ret > nud.Maximum) ret = nud.Maximum;

            return ret;
        }

        float _float(string s, float def)
        {
            try
            {
                return float.Parse(s);
            }
            catch (Exception)
            {
            }

            return def;
        }

        private void UpdateFontButtonText()
        {
            bFont.Text = string.Format("Шрифт: {0}, {1}, {2}", fd.Font.Name, fd.Font.Style.ToString(), fd.Font.SizeInPoints);
        }

        private void bFont_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog() == DialogResult.OK)
            {
                UpdateFontButtonText();
                changed = true;
            }
        }

        private void bPreview_Click(object sender, EventArgs e)
        {
            try
            {
                SimpleXML t = getTemplate();

                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = printerTitle;

                CPrintDocument doc = new CPrintDocument(null, t, ps, DEXToolBox.getToolBox());
                doc.Preview();

            }
            catch (Exception)
            {
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F3)
            {
                bPreview_Click(bPreview, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void bRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Восстановить шаблон по умолчанию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                template = SimpleXML.LoadXml(SimpleXML.SaveXml(deftemplate));
                loadTemplate();
                changed = true;
            }
        }

        private void bUpdateTemplate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Обновить шаблон?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SimpleXML tfields = template["Fields"];
                List<string> sfields = new List<string>();

                // Сначала соберем хеши полей в текущем шаблоне
                foreach (SimpleXML tfield in tfields.Child)
                {
                    try
                    {
                        sfields.Add(tfield.Name + tfield.Attributes["title"] + tfield.Attributes["type"]);
                    }
                    catch (Exception) { }
                }


                List<string> dfields = new List<string>();

                SimpleXML dtfields = deftemplate["Fields"];

                // Соберем хеши полей в шаблоне по умолчанию и дополним текущий шаблон отсутствующими полями
                foreach (SimpleXML tfield in dtfields.Child)
                {
                    try
                    {
                        string shash = tfield.Name + tfield.Attributes["title"] + tfield.Attributes["type"];
                        dfields.Add(shash);
                        if (!sfields.Contains(shash))
                        {
                            sfields.Add(shash);
                            tfield.Clone(true).Parent = tfields;
                        }
                    }
                    catch (Exception) { }
                }

                // Проверим, нет ли лишних полей в текущем шаблоне
                List<SimpleXML> delfields = new List<SimpleXML>();

                foreach (SimpleXML tfield in tfields.Child)
                {
                    try
                    {
                        string shash = tfield.Name + tfield.Attributes["title"] + tfield.Attributes["type"];
                        if (!dfields.Contains(shash)) delfields.Add(tfield);
                    }
                    catch (Exception) { }
                }

                // Грохнем в текущем шаблоне найденные лишние поля
                foreach (SimpleXML delfield in delfields) delfield.Parent = null;

                loadTemplate();
                changed = true;
            }
        }

        private void cbCover_SelectedIndexChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void cbUseCover_CheckedChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void nudCoverOffsetX_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void cbOrientation_SelectedIndexChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;
        }

        private void SchemeEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (changed && MessageBox.Show("Выйти без сохранения схемы?", "Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            template = getTemplate();
            changed = false;
            DialogResult = DialogResult.OK;
        }

        private void cbRotate_CheckedChanged(object sender, EventArgs e)
        {
            changed = true;
        }

    }
}
