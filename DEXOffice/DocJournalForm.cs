using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXOffice
{
    public partial class DocJournalForm : Form
    {
        DataTable t;
        BindingSource bs;

        public DocJournalForm(SimpleXML jrn)
        {
            InitializeComponent();

            dgv.AutoGenerateColumns = false;
            time.DataPropertyName = "time";
            text.DataPropertyName = "text";

            bs = new BindingSource();
            dgv.DataSource = bs;

            t = new DataTable();
            t.Columns.Add("time", typeof(string));
            t.Columns.Add("text", typeof(string));
            t.Columns.Add("color", typeof(Color));

            Color[] clrs = { Color.White, Color.LightGray };
            int clr = 0;

            try
            {
                ArrayList recs = jrn.GetChildren("record");
                foreach (SimpleXML rec in recs)
                {
                    DataRow r = t.NewRow();
                    try
                    {
                        r["time"] = rec.Attributes["time"];
                    }
                    catch (Exception)
                    {
                    }
                    if (rec.GetNodeByPath("text", false) != null)
                        r["text"] = rec["text"].Text;
                    else
                        r["text"] = "(Нет текста)";

                    r["color"] = clrs[clr];
                    t.Rows.Add(r);

                    ArrayList sds = rec.GetChildren("subdata");
                    if (sds != null && sds.Count > 0)
                    {
                        foreach (SimpleXML sd in sds)
                        {
                            r = t.NewRow();
                            r["time"] = "";
                            r["text"] = sd.Text;
                            r["color"] = clrs[clr];
                            t.Rows.Add(r);
                        }
                    }

                    clr = 1 - clr;
                }
            }
            catch (Exception)
            {
            }

            bs.DataSource = t;
        }

        private void dgv_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DataRowView row = bs[dgv.Rows[e.RowIndex].Index] as DataRowView;
                Color cc = (Color)row["color"];
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = cc;
            }
            catch (Exception)
            {
            }
        }
    }
}
