using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Users
{
    public partial class FUsersEd : Form
    {
        DataTable data;
        DataRow row;
        Object toolbox;

        public FUsersEd()
        {
            InitializeComponent();
        }

        public void InitForm(Object AToolBox, DataTable AData, DataRow ARow)
        {
            toolbox = AToolBox;
            data = AData;
            row = ARow;

            IDEXRights r = (IDEXRights)toolbox;
            ArrayList rt = r.GetRightsTable();

            cbDefNewDocState.SelectedIndex = 0;

            clbRights.Items.Clear();

            if (row == null)
            {
                tbLogin.Text = "";
                tbTitle.Text = "";
                tbUID.Text = ((IDEXCrypt)toolbox).StringToMD5(DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                cbStatus.Checked = true;

                foreach (RightsItem ri in rt)
                {
                    clbRights.Items.Add(ri.Title, false);
                }
            }
            else
            {
                tbLogin.Text = row["login"].ToString();
                tbTitle.Text = row["title"].ToString();
                tbUID.Text = row["uid"].ToString();
                cbStatus.Checked = bool.Parse(row["status"].ToString());


                SimpleXML doc = SimpleXML.LoadXml(row["settings"].ToString());
                string srights = doc["rights"].Text;
                try
                {
                    cbDefNewDocState.SelectedIndex = int.Parse(doc[@"Properties\DefaultDocumentState"].Text);
                }
                catch (Exception)
                {
                }

                #region Старый вариант загрузки прав
                /*
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(row["settings"].ToString());
                XmlNodeList rl = doc.GetElementsByTagName("rights");
                if (rl.Count > 0)
                {
                    srights = rl[0].InnerText;
                }
*/
                #endregion

                foreach (RightsItem ri in rt)
                {
                    clbRights.Items.Add(ri.Title, srights.IndexOf("|" + ri.ID + "|") > -1);
                }
            }

            tbLogin.Focus();
        }

        private string applyRightsToSettings(string data)
        {
            SimpleXML ret = SimpleXML.LoadXml(data);
            if (ret == null) ret = new SimpleXML("settings");

            IDEXRights r = (IDEXRights)toolbox;
            ArrayList rt = r.GetRightsTable();

            string srights = "";
            foreach (int checkedIndex in clbRights.CheckedIndices)
            {
                srights += ((RightsItem)rt[checkedIndex]).ID + "|";
            }
            if (srights.Length > 0)
            {
                srights = "|" + srights;
            }
            ret["rights"].Text = srights;
            ret[@"Properties\DefaultDocumentState"].Text = cbDefNewDocState.SelectedIndex.ToString();

            return SimpleXML.SaveXml(ret);
            
            #region Старый вариант процедуры

            /*
            XmlDocument ret = new XmlDocument();
            try
            {
                ret.LoadXml(data);
            }
            catch (Exception)
            {
            }

            XmlNode root = null;

            XmlNodeList xnlSettings = ret.GetElementsByTagName("settings");
            if (xnlSettings.Count > 0)
            {
                root = xnlSettings[0];
            }
            else
            {
                XmlNode n = ret.CreateNode(XmlNodeType.Element, "settings", null);
                ret.AppendChild(n);
                root = n;
            }

            XmlNodeList xnlRights = ret.GetElementsByTagName("rights");
            if (xnlRights.Count > 0)
            {
                root = xnlRights[0];
            }
            else
            {
                XmlNode n = ret.CreateNode(XmlNodeType.Element, "rights", null);
                root.AppendChild(n);
                root = n;
            }

            IDEXRights r = (IDEXRights)toolbox;
            ArrayList rt = r.GetRightsTable();

            string srights = "";
            foreach (int checkedIndex in clbRights.CheckedIndices)
            {
                srights += ((RightsItem)rt[checkedIndex]).ID + "|";
            }
            if (srights.Length > 0)
            {
                srights = "|" + srights;
            }

            root.InnerText = srights;

            TextWriter sw = new StringWriter();
            ret.Save(sw);
            return sw.ToString();
             */
            #endregion
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";

            string clLogin = tbLogin.Text.Trim();

            if (clLogin.Length < 1)
            {
                er += "* Не указан логин пользователя\n";
            }

            if (er == "")
            {
                DataRow[] r = data.Select("login = '" + clLogin.Replace("'", "\'") + "'");
                if (r != null && r.Length > 0 && r[0] != row)
                {
                    er += "* Пользователь с таким логином уже существует\n";
                }
            }

            if (er == "")
            {
                DataRow nr = row;

                if (nr == null)
                {
                    nr = data.NewRow();
                    nr.BeginEdit();
                    nr["settings"] = applyRightsToSettings("");
                    nr["datecreated"] = DateTime.Now.ToString("yyyyMMddhhmmss");
                }
                else
                {
                    nr.BeginEdit();
                    nr["settings"] = applyRightsToSettings(nr["settings"].ToString());
                }

                if (tbPass.Text.Length > 0)
                {
                    nr["pass"] = ((IDEXCrypt)toolbox).StringToMD5(tbPass.Text);
                }

                nr["login"] = tbLogin.Text;
                nr["uid"] = tbUID.Text;
                nr["title"] = tbTitle.Text;
                nr["datechanged"] = DateTime.Now.ToString("yyyyMMddhhmmss");
                nr["status"] = cbStatus.Checked.ToString();
                nr.EndEdit();

                if (row == null)
                {
                    data.Rows.Add(nr);
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Ошибки:\n\n" + er);
            }
        }
    }
}
