using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.Common;
using DEXExtendLib;
using System.Collections;

namespace Kassa3
{
    public partial class NetLoginForm : Form
    {

        Dictionary<string, string> dLogins = new Dictionary<string, string>();
        Dictionary<string, string> dPasswords = new Dictionary<string, string>();


        public NetLoginForm()
        {
            InitializeComponent();
            initForm();
        }

        public void initForm() 
        {
            initLbLiteBases();

            bool loaded2 = false;
            try
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(Tools.instance.dataDir + "login2.xml", Encoding.UTF8));

                try
                {
                    int ix = 0;
                    if (int.TryParse(xml.Attributes["tabSelected"], out ix))
                    {
                        tabControl1.SelectedIndex = ix;
                    }
                }
                catch (Exception) { }

                Dictionary<string, string> attr = xml["MySql"].Attributes;
                
                cbSavePassword.Checked = Convert.ToBoolean(attr["SavePassword"]);
                tbMysqlHost.Text = attr["Host"];
                nudMysqlPort.Value = Convert.ToInt32(attr["Port"]);
                tbMysqlUser.Text = attr["User"];
                try
                {
                    tbMysqlPassword.Text = Crypt.Decrypt(attr["Password"], Tools.KEY);
                }
                catch (Exception) { }

                cbKassaBase.Items.Clear();

                ArrayList aldbs = xml["MySql"]["Databases"].GetChildren("Database");
                foreach (SimpleXML xmldb in aldbs)
                {
                    cbKassaBase.Items.Add(new StringKassaItem(xmldb.Attributes["name"], xmldb.Attributes["title"]));
                }

                string selectedDb = attr.ContainsKey("selected") ? attr["selected"] : "";
                foreach (StringKassaItem ski in cbKassaBase.Items)
                {
                    if (ski.dbname.Equals(selectedDb))
                    {
                        cbKassaBase.SelectedItem = ski;
                        break;
                    }
                }

                tbKassaUser.Text = attr.ContainsKey("KassaUser") ? attr["KassaUser"] : "";
                try
                {
                    tbKassaPassword.Text = Crypt.Decrypt(attr["KassaPassword"], Tools.KEY);
                }
                catch (Exception) { }

                aldbs = xml["Sqlite"]["Databases"].GetChildren("Database");

                foreach (SimpleXML xmlDb in aldbs)
                {
                    try
                    {
                        string dbname = xmlDb.Attributes["name"];
                        int dbix = -1;

                        for(int i = 0; i < lbLiteBases.Items.Count; ++i) {
                            StringTagItem sti = (StringTagItem)lbLiteBases.Items[i];
                            if (sti.Text == dbname)
                            {
                                dbix = i;
                                break;
                            }
                        }

                        if (dbix > -1)
                        {
                            if (xmlDb.Attributes.ContainsKey("user")) dLogins[dbname] = xmlDb.Attributes["user"];
                            if (xmlDb.Attributes.ContainsKey("pass")) dPasswords[dbname] = Crypt.Decrypt(xmlDb.Attributes["pass"], Tools.KEY);
                        }
                    }
                    catch (Exception) { }
                }

                if (xml["Sqlite"].Attributes.TryGetValue("selected", out selectedDb))
                {
                    foreach (StringTagItem sti in lbLiteBases.Items)
                    {
                        if (sti.Text == selectedDb)
                        {
                            lbLiteBases.SelectedItem = sti;
                            lbLiteBases_SelectedIndexChanged(lbLiteBases, null);
                            break;
                        }
                    }
                }

                loaded2 = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }

            // Старый код для совместимости настроек
            if (!loaded2) {
                LoginSettings ls = null;
                try
                {
                    ls = (LoginSettings)Tools.instance.SoapDeserialize(File.ReadAllText(Tools.instance.dataDir + "login.xml", Encoding.UTF8));
                }
                catch (Exception) { }
                if (ls == null) ls = new LoginSettings();

                cbSavePassword.Checked = ls.SaveUser;

                tbMysqlHost.Text = ls.MysqlHost;
                nudMysqlPort.Value = ls.MysqlPort;
                tbMysqlUser.Text = ls.MysqlUser;
                try
                {
                    tbMysqlPassword.Text = Crypt.Decrypt(ls.MysqlPassword, Tools.KEY);
                }
                catch (Exception) { }

                cbKassaBase.Items.Clear();
                try
                {
                    cbKassaBase.Items.AddRange(ls.databases);

                    foreach (StringKassaItem ski in ls.databases)
                    {
                        if (ski.dbname.Equals(ls.MysqlDb))
                        {
                            cbKassaBase.SelectedItem = ski;
                            break;
                        }
                    }
                }
                catch (Exception) { }

                tbKassaUser.Text = ls.KassaUser;
                try
                {
                    tbKassaPassword.Text = Crypt.Decrypt(ls.KassaPassword, Tools.KEY);
                }
                catch (Exception) { }
            }
        }

        void initLbLiteBases()
        {
            lbLiteBases.Items.Clear();
            string[] dbs = Directory.GetFiles(Tools.instance.localDbDir, "*." + Tools.SQLITE_DB_EXTENSION);
            
            foreach (string db in dbs)
            {
                FileInfo fi = new FileInfo(db);
                lbLiteBases.Items.Add(new StringTagItem(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length), db));
            }
        }

        public void saveForm()
        {
            SimpleXML xml = new SimpleXML("Config");
            xml.Attributes["tabSelected"] = "" + tabControl1.SelectedIndex;
            SimpleXML xmlMysql = xml.CreateChild("MySql");
            Dictionary<string, string> attr = xmlMysql.Attributes;

            attr["SavePassword"] = Convert.ToString(cbSavePassword.Checked);
            attr["Host"] = tbMysqlHost.Text;
            attr["Port"] = Convert.ToString(nudMysqlPort.Value);
            attr["User"] = tbMysqlUser.Text;
            attr["Password"] = Crypt.Encrypt(tbMysqlPassword.Text, Tools.KEY);

            SimpleXML xmlDbs = xmlMysql.CreateChild("Databases");
            for (int i = 0; i < cbKassaBase.Items.Count; ++i)
            {
                StringKassaItem sti = (StringKassaItem)cbKassaBase.Items[i];
                SimpleXML xmlDb = xmlDbs.CreateChild("Database");
                xmlDb.Attributes["name"] = sti.dbname;
                xmlDb.Attributes["title"] = sti.kassatitle;
            }

            if (cbKassaBase.SelectedItem != null) attr["selected"] = ((StringKassaItem)cbKassaBase.SelectedItem).dbname;

            attr["KassaUser"] =  cbSavePassword.Checked ? tbKassaUser.Text : "";
            attr["KassaPassword"] = Crypt.Encrypt(cbSavePassword.Checked ? tbKassaPassword.Text : "", Tools.KEY);

            SimpleXML xmlSqlite = xml.CreateChild("Sqlite");

            if (lbLiteBases.SelectedItem != null) xmlSqlite.Attributes["selected"] = ((StringTagItem)lbLiteBases.SelectedItem).Text;

            xmlDbs = xmlSqlite.CreateChild("Databases");
            for (int i = 0; i < lbLiteBases.Items.Count; ++i)
            {
                StringTagItem sti = (StringTagItem)lbLiteBases.Items[i];
                if (dLogins.ContainsKey(sti.Text) || dPasswords.ContainsKey(sti.Text))
                {
                    SimpleXML xmlDb = xmlDbs.CreateChild("Database");
                    xmlDb.Attributes["name"] = sti.Text;
                    if (dLogins.ContainsKey(sti.Text)) xmlDb.Attributes["user"] = dLogins[sti.Text];
                    if (dPasswords.ContainsKey(sti.Text)) xmlDb.Attributes["pass"] = Crypt.Encrypt(dPasswords[sti.Text], Tools.KEY);
                }
            }

            File.WriteAllText(Tools.instance.dataDir + "login2.xml", SimpleXML.SaveXml(xml), Encoding.UTF8);
            if (File.Exists(Tools.instance.dataDir + "login.xml")) File.Move(Tools.instance.dataDir + "login.xml", Tools.instance.dataDir + "login-old.xml");
            
            /*
            LoginSettings ls = new LoginSettings();
            ls.MysqlHost = tbMysqlHost.Text;
            ls.MysqlPort = (int)nudMysqlPort.Value;
            ls.MysqlUser = tbMysqlUser.Text;
            ls.MysqlPassword = Crypt.Encrypt(tbMysqlPassword.Text, Tools.KEY);

            ls.databases = new StringKassaItem[cbKassaBase.Items.Count];
            for (int i = 0; i < cbKassaBase.Items.Count; ++i) ls.databases[i] = (StringKassaItem)cbKassaBase.Items[i];

            if (cbKassaBase.SelectedItem != null) ls.MysqlDb = ((StringKassaItem)cbKassaBase.SelectedItem).dbname;

            ls.SaveUser = cbSavePassword.Checked;
            ls.KassaUser = ls.SaveUser ? tbKassaUser.Text : "";
            ls.KassaPassword = Crypt.Encrypt(ls.SaveUser ? tbKassaPassword.Text : "", Tools.KEY);

            try
            {
                File.Delete(Tools.instance.dataDir + "login.xml");
            }
            catch (Exception) { }

            File.WriteAllText(Tools.instance.dataDir + "login.xml", Tools.instance.SoapSerialize(ls), Encoding.UTF8);
             */
        }

        private void bSearchNetworkDb_Click(object sender, EventArgs e)
        {
            gbKassa.Enabled = false;

            cbKassaBase.Items.Clear();

            MySqlConnection con = null;
            try
            {
                string conn = 
                    "server=" + tbMysqlHost.Text + 
                    ";port=" + nudMysqlPort.Value.ToString() + 
                    ";user id=" + tbMysqlUser.Text + 
                    ";Password=" + tbMysqlPassword.Text +
                ";Database=;persist security info=True;charset=cp1251";

                con = new MySqlConnection(conn);
                con.Open();

                DataTable dt = new DataTable();

                MySqlDataAdapter mysqlDa = new MySqlDataAdapter("show databases", con);
                mysqlDa.Fill(dt);

                foreach (DataRow dbr in dt.Rows)
                {
                    try
                    {
                        string dbname = dbr["Database"].ToString();
                        con.ChangeDatabase(dbname);

                        using (DataTable dt2 = new DataTable())
                        {
                            mysqlDa = new MySqlDataAdapter("select * from `kassa`", con);
                            mysqlDa.Fill(dt2);
                            if (dt2.Rows.Count > 0)
                            {
                                cbKassaBase.Items.Add(new StringKassaItem(dbname, dt2.Rows[0]["title"].ToString()));
                            }
                        }

                    }
                    catch (Exception) { }
                }

            }
            catch (Exception) { }

            try
            {
                con.Close();
            }
            catch (Exception) { }

            gbKassa.Enabled = true;
        }

        private void bConnectNetworkDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbKassaUser.Text.Trim().Equals("")) throw new KassaException("Имя пользователя не может быть пустым.");

                Tools tools = Tools.instance;

                /*
                tools.ConnectNetworkDb(
                    tbMysqlHost.Text, (int)nudMysqlPort.Value, ((StringKassaItem)cbKassaBase.SelectedItem).dbname,
                    tbMysqlUser.Text, tbMysqlPassword.Text
                    );
                tools.initSession(tbKassaUser.Text, tbKassaPassword.Text);
                 */

                Db.init(Db.DBTYPE_MYSQL, Tools.instance.networkDbConnectionString(
                    tbMysqlHost.Text, (int)nudMysqlPort.Value, tbMysqlUser.Text, tbMysqlPassword.Text, 
                    ((StringKassaItem)cbKassaBase.SelectedItem).dbname)
                );

                tools.initSession2(tbKassaUser.Text, tbKassaPassword.Text);

                tools.tmDataChanges = new TableMonitor("data");

                if (cbUnlockUser.Checked)
                {
                    using (DbCommand cmd = Db.command("update `journal` set locker_id = null, lock_till = null"))
                    {
                        cmd.ExecuteNonQuery();
                    }

//                    new MySqlCommand("update `journal` set locker_id = null, lock_till = null", tools.connection).ExecuteNonQuery();
                }
                saveForm();

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("При попытке подключения произошла ошибка:\n" + ex.Message);
            }
        }

        private void bExitFromKassa_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NetLoginForm_Shown(object sender, EventArgs e)
        {
            bConnectNetworkDB.Focus();
        }

        private void lbLiteBases_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbLiteRemember.Checked = false;
            tbLiteUser.Text = "";
            tbLitePass.Text = "";
            StringTagItem sti = (StringTagItem)lbLiteBases.SelectedItem;
            if (sti != null)
            {
                bool hasLogin = dLogins.ContainsKey(sti.Text), hasPassword = dPasswords.ContainsKey(sti.Text);
                if (hasLogin) tbLiteUser.Text = dLogins[sti.Text];
                if (hasPassword) tbLitePass.Text = dPasswords[sti.Text];
                cbLiteRemember.Checked = hasLogin || hasPassword;
            }
        }

        private void bLiteCreate_Click(object sender, EventArgs e)
        {
            NewSqliteDbForm nsdf = new NewSqliteDbForm();
            if (nsdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //TODO рефрешнуть список баз. Найти базу в списке. Установить пользователя и пароль. Нажать bLiteOpen_Click
                string dbid = nsdf.curDbName;
                dLogins[dbid] = nsdf.curUser;
                dPasswords[dbid] = nsdf.curPass;
                cbLiteRemember.Checked = true;
                initLbLiteBases();
                
                foreach (StringTagItem sti in lbLiteBases.Items)
                {
                    if (sti.Text == dbid)
                    {
                        lbLiteBases.SelectedItem = sti;
                        bLiteOpen_Click(bLiteOpen, null);
                        break;
                    }
                }
            }
        }

        private void bLiteOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbLiteUser.Text.Trim().Equals("")) throw new KassaException("Имя пользователя не может быть пустым.");

                StringTagItem sti = (StringTagItem)lbLiteBases.SelectedItem;
                Db.init(Db.DBTYPE_SQLITE, Tools.instance.localDbConnectionString(sti.Text));

                Tools.instance.initSession2(tbLiteUser.Text, tbLitePass.Text);
                Tools.instance.tmDataChanges = new TableMonitor("data");

                if (cbLiteRemember.Checked)
                {
                    dLogins[sti.Text] = tbLiteUser.Text;
                    dPasswords[sti.Text] = tbLitePass.Text;
                }

                saveForm();

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("При попытке подключения произошла ошибка:\n" + ex.Message);
            }
        }

        private void bLiteForget_Click(object sender, EventArgs e)
        {
            try
            {
                StringTagItem sti = (StringTagItem)lbLiteBases.SelectedItem;
                dLogins.Remove(sti.Text);
                dPasswords.Remove(sti.Text);
                tbLiteUser.Text = "";
                tbLitePass.Text = "";
                saveForm();
            }
            catch (Exception) { }
        }

    }

    [Serializable]
    public class StringKassaItem
    {
        public string dbname, kassatitle;

        public StringKassaItem(string dbname, string kassatitle)
        {
            this.dbname = dbname;
            this.kassatitle = kassatitle;
        }

        public override string ToString()
        {
            return kassatitle + " (" + dbname + ")";
        }
    }

    [Serializable]
    public class LoginSettings
    {
        public string MysqlHost = "";
        public int MysqlPort = 3306;
        public string MysqlUser = "";
        public string MysqlPassword = "";
        public string MysqlDb = "";
        public string KassaUser = "";
        public string KassaPassword = "";
        public bool SaveUser = false;

        public StringKassaItem[] databases;
    }
}
