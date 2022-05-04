using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;
using System.IO;

namespace dexol
{
    public partial class TTCExportForm : Form
    {
        object toolbox;
        DateTime dtday;

        public TTCExportForm(object toolbox, DateTime dtday)
        {
            InitializeComponent();

            this.toolbox = toolbox;
            this.dtday = dtday;

            IDEXConfig cfg = (IDEXConfig)toolbox;
            tbTTCDir.Text = cfg.getStr(this.Name, tbTTCDir.Name, "");
        }

        private void bSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbTTCDir.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                IDEXConfig cfg = (IDEXConfig)toolbox;
                tbTTCDir.Text = fbd.SelectedPath;
                cfg.setStr(this.Name, tbTTCDir.Name, tbTTCDir.Text);
            }
        }

        public string requestJournal(IWaitMessageEventArgs wmea)
        {
            DexolSession ds = DexolSession.inst();
            string savedDb = ds.currentDb;

            try
            {
                wmea.progressVisible = false;
                wmea.textMessage = "Получение списка баз";
                wmea.DoEvents();


                Dictionary<string, StringDbItem> diclist = ds.dblist();

                string expf = "";

                int expcnt = 0;

                foreach (KeyValuePair<string, StringDbItem> kvp in diclist) 
                {
                    try {
                        wmea.progressVisible = false;
                        wmea.textMessage = string.Format("Обработка документов в '{0}'", kvp.Value.title);
                        wmea.DoEvents();
                        ds.currentDb = kvp.Key;

                        DataTable dt = ds.queryJournal(dtday);

                        if (dt != null && dt.Rows.Count > 0) {
                            foreach (DataRow row in dt.Rows)
                            {
                                SimpleXML xml = SimpleXML.LoadXml(row["data"].ToString());
                                expf += "msisdn=" + xml["msisdn"].Text + "\r\n" +
                                        "icc=" + xml["icc"].Text + "\r\n" +
                                        "docid=" + xml.Attributes["ID"] + "\r\n" + 
                                        "###\r\n";
                                expcnt++;
                            }
                        }
                    } catch(Exception) { }
                }

                ds.currentDb = savedDb;
                if (expcnt < 1) return "Нет данных на указанную дату";

                File.WriteAllText((string)wmea.args[0], expf, Encoding.GetEncoding(1251));

                return null;                
            }
            catch (Exception) { }

            ds.currentDb = savedDb;
            return "Не удалось получить список документов";
        }

        private void bExport_Click(object sender, EventArgs e)
        {
            string rdir = tbTTCDir.Text.Trim();
            if (!Directory.Exists(rdir))
            {
                MessageBox.Show("Указанный каталог отсутствует");
                return;
            }

            string lfn = rdir + @"\" + dtday.ToString("yyyy-MM-dd") + ".txt";

            string r = WaitMessage.Execute(new WaitMessageEvent(requestJournal), lfn);
            if (r == null)
            {
                r = "Данные успешно выгружены";
                DialogResult = DialogResult.OK;
            }

            MessageBox.Show(r);
        }
    }
}
