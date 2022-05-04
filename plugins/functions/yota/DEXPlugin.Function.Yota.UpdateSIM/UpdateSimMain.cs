using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Function.Yota.UpdateSIM
{
    public partial class UpdateSimMain : Form
    {
/*
        public const int DOCUMENT_NONE = -1;
        public const int DOCUMENT_DRAFT = 0;
        public const int DOCUMENT_UNAPPROVED = 1;
        public const int DOCUMENT_APPROVED = 2;
        public const int DOCUMENT_TOEXPORT = 3;
        public const int DOCUMENT_EXPORTED = 4;
        public const int DOCUMENT_RETURNED = 5;
        public const int DOCUMENT_EXPORTING = 6;
        public const int DOCUMENT_TODELETE = 100;

        public static string[] DOCUMENT_STATE_TEXT = { "Черновик", "На подтверждение", "Подтверждён", "На отправку", "Отправлен", "Возвращён", "Отправляется" };
*/
        object toolbox;


        public UpdateSimMain(object toolbox)
        {
            InitializeComponent();
            this.toolbox = toolbox;

            IDEXConfig cfg = (IDEXConfig)toolbox;
            de1.Value = cfg.getDate(this.Name, "de1", DateTime.Now);
            de2.Value = cfg.getDate(this.Name, "de2", DateTime.Now);

            cb1.Checked = cfg.getBool(this.Name, "cb1", true);
            cb2.Checked = cfg.getBool(this.Name, "cb2", true);
            cb3.Checked = cfg.getBool(this.Name, "cb3", true);
            cb4.Checked = cfg.getBool(this.Name, "cb4", true);
            cb5.Checked = cfg.getBool(this.Name, "cb5", true);
            cb6.Checked = cfg.getBool(this.Name, "cb6", true);

        }

        public void SaveParams()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;
            cfg.setDate(this.Name, "de1", de1.Value);
            cfg.setDate(this.Name, "de2", de2.Value);

            cfg.setBool(this.Name, "cb1", cb1.Checked);
            cfg.setBool(this.Name, "cb2", cb2.Checked);
            cfg.setBool(this.Name, "cb3", cb3.Checked);
            cfg.setBool(this.Name, "cb4", cb4.Checked);
            cfg.setBool(this.Name, "cb5", cb5.Checked);
            cfg.setBool(this.Name, "cb6", cb6.Checked);
        }

        string statusAdd(string src, bool ch, int num)
        {
            if (ch)
            {
                return src + (src != "" ? ", " : "") + num.ToString();
            }

            return src;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string er = "";

            if (!de1.Text.Equals(de1.Value.ToString("dd.MM.yyyy"))) er += "* Некорректная дата начала\n";
            if (!de2.Text.Equals(de2.Value.ToString("dd.MM.yyyy"))) er += "* Некорректная дата окончания\n";

            DateTime d1 = de1.Value, d2 = de2.Value;
            if (d1 > d2)
            {
                DateTime t = d1; 
                d1 = d2; 
                d2 = t;
            }

            statusCondition = statusAdd("", cb1.Checked, 0);
            statusCondition = statusAdd(statusCondition, cb2.Checked, 1);
            statusCondition = statusAdd(statusCondition, cb3.Checked, 2);
            statusCondition = statusAdd(statusCondition, cb4.Checked, 3);
            statusCondition = statusAdd(statusCondition, cb5.Checked, 4);
            statusCondition = statusAdd(statusCondition, cb6.Checked, 5);

            if (statusCondition == "")
            {
                er += "* Не выделено ни одного статуса\n";
            }

            if (er == "")
            {
                sds = d1.ToString("yyyyMMdd") + "000000000";
                sde = d2.ToString("yyyyMMdd") + "235959999";
                er = WaitMessage.Execute(new WaitMessageEvent(UpdateSIM));
            }

            if (er != "")
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        string statusCondition = "";
        string sds, sde;
        const string AB = "* Операция прервана";

        public string UpdateSIM(IWaitMessageEventArgs wmea)
        {
            wmea.canAbort = true;
            wmea.textMessage = "Синхронизация данных SIM";
            wmea.progressVisible = false;

            IDEXData data = ((IDEXData)toolbox);

            int cnt = 0;

            string wh = string.Format("where jdocdate >= '{0}' and jdocdate <= '{1}' and status in ({2})", sds, sde, statusCondition);

            DataTable dt = data.getQuery("select count(id) as cid from `journal` " + wh);
            if (wmea.isAborted) return AB;

            if (dt != null && dt.Rows.Count > 0)
            {
                cnt = Convert.ToInt32(dt.Rows[0]["cid"]);
                if (cnt > 0)
                {
                    wmea.minValue = 0;
                    wmea.maxValue = cnt;
                    wmea.progressValue = 0;
                    wmea.progressVisible = false;

                    dt = data.getQuery("select id, unitid, data from `journal` " + wh);
                    int pw = 0;

                    foreach (DataRow r in dt.Rows)
                    {
                        try {
                            int id = Convert.ToInt32(r["id"]), unitid = Convert.ToInt32(r["unitid"]);
                            SimpleXML xml = SimpleXML.LoadXml(r["data"].ToString());
                            //string msisdn = xml.GetNodeByPath("ICC", false).Text.Trim(), 
                            string icc = xml.GetNodeByPath("ICC", false).Text.Trim();

                            if (icc.Length > 0)
                            {
                                data.runQuery("update `um_data` set icc = '{0}', owner_id = {1} where icc = '{2}'", icc, unitid, icc);
                            }
                        } catch (Exception) { }
                        wmea.progressValue = pw++;
                        if (wmea.isAborted) break;

                    }
                }
            }

            if (cnt == 0) {
                return "* В указанном периоде не было документов\n";
            }

            if (wmea.isAborted) return AB;

            return ""; // Всё в порядке
        }
    }
}
