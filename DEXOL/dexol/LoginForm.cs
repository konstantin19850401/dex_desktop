using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DEXExtendLib;

namespace dexol
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            cbDexolUrl.Items.Clear();
            try
            {
                JArray jaServers = JArray.Parse(File.ReadAllText("servers.json", Encoding.UTF8));
                foreach (JObject joItem in jaServers)
                {
                    JToken sName, sUrl;
                    if (joItem.TryGetValue("name", out sName) && joItem.TryGetValue("url", out sUrl))
                    {
                        cbDexolUrl.Items.Add(new StringTagItem((string)sName, (string)sUrl));
                    }
                }
            }
            catch (Exception) { }

            tbPass.Text = "";
            tbUser.Text = "";
            if (File.Exists("auth.json"))
            {
                try
                {
                    authdata auth = JsonConvert.DeserializeObject<authdata>(File.ReadAllText("auth.json", Encoding.UTF8));
                        //JsonMapper.ToObject<authdata>(File.ReadAllText("auth.json", Encoding.UTF8));
                    cbRemember.Checked = auth.saveAuth;
                    if (auth.saveAuth)
                    {
                        tbUser.Text = auth.login;
                        if (auth.password.Length > 0) tbPass.Text = DEXToolBox.getToolBox().Decrypt(auth.password);
                        StringTagItem.SelectByTag(cbDexolUrl, auth.serverpath, true);
                    }
                }
                catch (Exception) { }
            }
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            DexolSession ds = DexolSession.inst();

//            if (tbDexolUrl.Text.Trim().Length > 0)
            if (cbDexolUrl.SelectedItem != null)
            {
//                string surl = tbDexolUrl.Text.Trim();
                string surl = ((StringTagItem)cbDexolUrl.SelectedItem).Tag.Trim();
                if (!'/'.Equals(surl[surl.Length - 1])) surl += @"/";
                surl += "index.php?";
                ds.DEXOL_URL = surl;
                if (ds.login(tbUser.Text, tbPass.Text))
                {
                    authdata auth = new authdata();
                    auth.saveAuth = cbRemember.Checked;
                    auth.login = auth.saveAuth ? tbUser.Text : "";

                    auth.serverpath = ((StringTagItem)cbDexolUrl.SelectedItem).Tag;

                    DEXToolBox.getToolBox().adaptersLogin = tbUser.Text;
                    DEXToolBox.getToolBox().adaptersPass = tbPass.Text;
                    Uri uri = new Uri(((StringTagItem)cbDexolUrl.SelectedItem).Tag.Trim());

                    auth.password = auth.saveAuth ? DEXToolBox.getToolBox().Encrypt(tbPass.Text) : "";
                    try
                    {
                        File.WriteAllText("auth.json", JsonConvert.SerializeObject(auth), Encoding.UTF8);
                    }
                    catch (Exception) {  }
                    DialogResult = DialogResult.OK;
                    
                }
                else
                {
                    string msg = ds.lastErrorMessage();
                    if (msg != null) MessageBox.Show("Ошибка:\n\n" + msg);
                }
            }
            else
            {
                MessageBox.Show("Не указан адрес сервера");
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
        }
    }

    public class authdata
    {
        public string login = "", password = "", serverpath = "";
        public bool saveAuth = false;
    }

    
}
