using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.Text.RegularExpressions;

namespace DEXPlugin.Function.Archive
{
    public partial class ArchiveMain : Form
    {
        object toolbox;

        public ArchiveMain(object toolbox)
        {
            InitializeComponent();

            this.toolbox = toolbox;
            IDEXConfig cfg = (IDEXConfig)toolbox;

            deStart.Value = cfg.getDate(this.Name, "deStart", DateTime.Now);
            deEnd.Value = cfg.getDate(this.Name, "deEnd", DateTime.Now);

            try
            {
                for (int i = 0; i < clbStatus.Items.Count; ++i) clbStatus.SetItemChecked(i, false);

                string[] sts = cfg.getStr(this.Name, "clbStatus", "").Split('|');
                foreach (string st in sts)
                {
                    try
                    {
                        clbStatus.SetItemChecked(int.Parse(st), true);
                    }
                    catch (Exception) { }
                }

            }
            catch (Exception) { }
        }

        public void SaveParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "deStart", deStart.Value);
            cfg.setDate(this.Name, "deEnd", deEnd.Value);
            string st = "";
            for (int i = 0; i < clbStatus.Items.Count; ++i)
            {
                if (clbStatus.GetItemChecked(i))
                {
                    if (st != "") st += "|";
                    st += i.ToString();
                }
            }
            cfg.setStr(this.Name, "clbStatus", st);

        }

        bool commonCheckFields()
        {
            lDocCount.Text = "-";
            string er = "";

            Regex rxdate = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");

            if (!rxdate.IsMatch(deStart.Text)) er += "* Некорректная дата начала интервала\n";
            if (!rxdate.IsMatch(deEnd.Text)) er += "* Некорректная дата конца интервала\n";

            if (clbStatus.SelectedIndices.Count < 1) er += "* Не выделено ни одного статуса\n";

            if (er.Equals(""))
            {
                try
                {
                    string st = "";
                    for (int i = 0; i < clbStatus.Items.Count; ++i)
                    {
                        if (clbStatus.GetItemChecked(i))
                        {
                            if (st != "") st += " or ";
                            st += "status = " + i.ToString();
                        }

                    }


                    IDEXData dd = (IDEXData)toolbox;
                    DataTable dt = dd.getQuery(
                        "select count(id) cid from `journal` where jdocdate >= '{0}000000000' and jdocdate <= '{1}235959999' and ({2})",
                        deStart.Value.ToString("yyyyMMdd"), deEnd.Value.ToString("yyyyMMdd"), st
                        );
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lDocCount.Text = dt.Rows[0]["cid"].ToString();
                    }
                }
                catch (Exception) { }

                if (lDocCount.Text.Equals("-") || lDocCount.Text.Equals("0"))
                {
                    er += "* В указанном периоде нет ни одного документа\n";
                }
            }

            if (!er.Equals("")) MessageBox.Show("Ошибки:\n\n" + er);
            return er.Equals("");
        }

        private void bCheckCount_Click(object sender, EventArgs e)
        {
            commonCheckFields();
        }

        private void bMakeArchive_Click(object sender, EventArgs e)
        {
            if (commonCheckFields())
            {
                string msg = string.Format("Вы уверены в том, что хотите перенести в архив документы за период с {0} по {1}?\n\nЭто действие нельзя будет отменить.",
                    deStart.Text, deEnd.Text);
                if (MessageBox.Show(msg, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {

                    string ret = WaitMessage.Execute(new WaitMessageEvent(MakeDocsArchive));
                    if (!ret.Equals(""))
                    {
                        MessageBox.Show("Ошибки:\n\n" + ret);
                    }
                    else
                    {
                        MessageBox.Show("Архивирование произведено.");
                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }

        public string MakeDocsArchive(IWaitMessageEventArgs wmea)
        {
            string ret = "";

            try
            {
                string st = "";
                for (int i = 0; i < clbStatus.Items.Count; ++i)
                {
                    if (clbStatus.GetItemChecked(i))
                    {
                        if (st != "") st += " or ";
                        st += "status = " + i.ToString();
                    }

                }

                IDEXData dd = (IDEXData)toolbox;
                DataTable dt = dd.getQuery(
                    "select count(id) cid from `journal` where jdocdate >= '{0}000000000' and jdocdate <= '{1}235959999' and ({2})",
                    deStart.Value.ToString("yyyyMMdd"), deEnd.Value.ToString("yyyyMMdd"), st
                    );

                int docsToExport = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    docsToExport = int.Parse(dt.Rows[0]["cid"].ToString());
                }

                if (docsToExport > 0)
                {

                    wmea.canAbort = true;
                    wmea.minValue = 0;
                    wmea.maxValue = docsToExport;
                    wmea.textMessage = "Архивирование документов";
                    wmea.progressValue = 0;
                    wmea.progressVisible = true;


                    dt = dd.getQuery(
                        "select * from `journal` where jdocdate >= '{0}000000000' and jdocdate <= '{1}235959999'  and ({2})",
                        deStart.Value.ToString("yyyyMMdd"), deEnd.Value.ToString("yyyyMMdd"), st
                    );

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        CDEXDocumentData cdoc = new CDEXDocumentData();
                        cdoc.documentStatus = 100;
                        int pw = 0;
                        foreach (DataRow r in dt.Rows)
                        {
                            try
                            {
                                string sql =
                                    "insert into `archive` (locked, locktime, userid, status, signature, " +
                                    "jdocdate, unitid, docid, digest, data, journal, importhash) values (" +
                                    "'', '', '" + dd.EscapeString(r["userid"].ToString()) + "', " +
                                    r["status"].ToString() + ", '" + dd.EscapeString(r["signature"].ToString()) +
                                    "', '" + dd.EscapeString(r["jdocdate"].ToString()) + "', " +
                                    r["unitid"].ToString() + ", '" + dd.EscapeString(r["docid"].ToString()) +
                                    "', '" + dd.EscapeString(r["digest"].ToString()) + "', '" +
                                    dd.EscapeString(r["data"].ToString()) + "', '" +
                                    dd.EscapeString(r["journal"].ToString()) + "', '" +
                                    dd.EscapeString(r["importhash"].ToString()) + "')";

                                dd.runQuery(sql);

                                sql = string.Format("delete from `journal` where id={0}", r["id"].ToString());

                                dd.runQuery(sql);

                                cdoc.signature = r["signature"].ToString();
                                dd.setDocumentCriticals(null, cdoc, true);
                            }
                            catch (Exception) { }

                            wmea.progressValue = pw++;
                            if (wmea.isAborted) break;
                        }
                    }
                }
                else
                {
                    ret = "В указанном интервале нет документов";
                }
            }
            catch (Exception) { ret = "Внутренняя ошибка"; }

            return ret;
        }

    }
}
