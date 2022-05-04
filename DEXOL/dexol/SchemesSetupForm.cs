using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using DEXExtendLib;
using dexol;
using System.IO;

namespace dexol
{
    public partial class SchemesSetupForm : Form
    {
        public SchemesSetupForm()
        {
            InitializeComponent();
            
            cbPrinter.Items.Clear();
            foreach (string prname in PrinterSettings.InstalledPrinters)
            {
                cbPrinter.Items.Add(prname);
            }

            cbScheme.Items.Clear();
            DEXToolBox tb = DEXToolBox.getToolBox();
            SimpleXML[] schemes = CPrintDocument.GetSchemesForId(tb.DataDir + @"printing_schemes\", "ALL");
            if (schemes != null && schemes.Length > 0)
            {
                foreach (SimpleXML scheme in schemes)
                {
                    cbScheme.Items.Add(new StringObjTagItem(
                        string.Format("{0} ({1})", scheme["Title"].Text, scheme["GUID"].Text),
                        scheme
                        ));
                }
            }

            string er = "";
            if (cbPrinter.Items.Count < 1) er += "* Не установлено ни одного принтера\n";
            if (cbScheme.Items.Count < 1) er += "* Не установлено ни одной схемы печати\n";

            bSelPrnScheme.Enabled = er.Equals("");
            if (!er.Equals(""))
            {
                MessageBox.Show("Невозможно редактировать свойства печати:\n\n" + er);
            }
        }

        private void bSelPrnScheme_Click(object sender, EventArgs e)
        {
            string er = "";
            if (cbPrinter.SelectedIndex < 0)
            {
                er += "* Не выбран принтер\n";
            }

            if (cbScheme.SelectedIndex < 0)
            {
                er += "* Не выбрана схема\n";
            }

            if (!er.Equals(""))
            {
                MessageBox.Show("Ошибки:\n\n" + er);
                return;
            }

            SimpleXML defscheme = (SimpleXML)((StringObjTagItem)cbScheme.SelectedItem).Tag;
            SimpleXML scheme = SimpleXML.LoadXml(SimpleXML.SaveXml(defscheme));

            string printer = (string)cbPrinter.SelectedItem;

            string guid = scheme["GUID"].Text.Trim();

            DEXToolBox tb = DEXToolBox.getToolBox();
            /*
            DataTable t = tb.getQuery(string.Format(
                "select * from `prnschemes` where printer = '{0}' and guid = '{1}'",
                tb.EscapeString(printer), tb.EscapeString(guid)));
            if (t != null && t.Rows.Count > 0)
            {
                scheme = SimpleXML.LoadXml(t.Rows[0]["data"].ToString());
            }
            */

            string schemeName = tb.DataDir + "scheme-" + tb.StringToMD5(printer) + "-" + scheme["GUID"].Text + ".xml";

            if (File.Exists(schemeName))
            {
                SimpleXML s2 = SimpleXML.LoadXml(File.ReadAllText(schemeName, Encoding.UTF8));
                if (s2 != null) scheme = s2;
            }

            SchemeEditForm sef = new SchemeEditForm((string)cbPrinter.SelectedItem, scheme, defscheme);
            if (sef.ShowDialog() == DialogResult.OK)
            {
                //TODO Сохранение нового или старого шаблона
                /*
                tb.runQuery(string.Format(
                    "delete from `prnschemes` where printer = '{0}' and guid = '{1}'",
                    tb.EscapeString(printer), tb.EscapeString(guid)));
                 */
                string sn = tb.DataDir + "scheme-" + tb.StringToMD5(printer) + "-" + scheme["GUID"].Text + ".xml";
                try
                {
                    File.Delete(sn);
                }
                catch (Exception) { } 

                if (sef.template != null)
                {
                    /*
                    tb.runQuery(string.Format(
                        "insert into `prnschemes` (printer, guid, data) values ('{0}', '{1}', '{2}')",
                        tb.EscapeString(printer), tb.EscapeString(guid), 
                        tb.EscapeString(SimpleXML.SaveXml(sef.template))));
                     */
                    File.WriteAllText(sn, SimpleXML.SaveXml(sef.template), Encoding.UTF8);
                }
            }
        }
    }
}
