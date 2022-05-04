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

namespace DEXPlugin.Dictionary.Beeline.Sim
{
    public partial class FAdvancedSearchList : Form
    {
        public Object toolbox;
        string[] separators = { ((char)9).ToString(), ";", ":", "|", ".", ",", "!", "&" };

        public FAdvancedSearchList()
        {
            InitializeComponent();
        }

        public void InitForm(Object toolbox)
        {
            this.toolbox = toolbox;
            dgvPreview.DataSource = null;
        }

        private void LoadSource(string src)
        {
            if (src.Trim().Equals(""))
            {
                MessageBox.Show("Предоставленный источник не содержит никакой информации.");
                return;
            }
            else
            {

                dgvPreview.DataSource = null;

                DataTable t = new DataTable();
                dgvPreview.Columns.Clear();

                string[] fnames = { "msisdn"};
                string[] fcaption = { "MSISDN"};
                string[] ftypes = { "String"};

                int fcnt = fnames.Length;
                for (int f = 0; f < fcnt; ++f)
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
                    dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvc.Visible = (fcaption[f] != null);
                    dgvc.DisplayIndex = f;
                    dgvPreview.Columns.Add(dgvc);
                }

               string[] sep = { separators[0] };

               StringReader sr = new StringReader(src);
               ArrayList strs = new ArrayList();
               while (true)
               {
                   string ln = sr.ReadLine();
                   if (ln == null) break;
                   else
                       if (!ln.Trim().Equals("")) strs.Add(ln);
               }

               foreach (string ln in strs)
               {
                   string rer = "";
                   bool carderr = false;
                   string[] p = ln.Split(sep, StringSplitOptions.None);

                   DataRow r = t.NewRow();

                   r["msisdn"] = p[0];
                   //r["icc"] = p[1];

                   t.Rows.Add(r);
               }
               dgvPreview.DataSource = t;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                LoadSource(Clipboard.GetText());
            }
            else
                MessageBox.Show("В буфере обмена содержится не текстовая информация.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
