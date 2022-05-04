using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using DEXExtendLib;


using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace DEXUpdater
{
    public partial class UpdMain : Form
    {
        string dcfg;
        string md5Remote;
        public UpdMain()
        {
            dcfg = Path.GetDirectoryName(Application.ExecutablePath) + @"\config.xml";
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            dcfg = Path.GetDirectoryName(Application.ExecutablePath) + @"\config.xml";
            tbSelectUpDir.Text = "";
            if (File.Exists(dcfg))
            {
                try
                {
                    SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                    if (xml.GetNodeByPath(@"\DEXUpdater\TypeRepo", false) != null && xml[@"DEXUpdater\TypeRepo"].Text != "-1")
                    {
                        setCheckRadio(gbTypeRepo, Convert.ToInt32(xml[@"DEXUpdater\TypeRepo"].Text, 10));
                        if (rbLoc.Checked)
                        {
                            tbSelectUpDir.Text = xml[@"DEXUpdater\UpdateDir"].Text;
                        }
                        else if (rbRem.Checked)
                        {
                            tbSelectUpDir.Text = xml[@"DEXUpdater\UpdateDirRem"].Text;
                        } 
                        if (xml[@"DEXUpdater\TypeRepo"].Text == "0")
                        {
                            bSelectUpDir.Visible = false;
                        }
                    }
                    if (xml.GetNodeByPath(@"DEXUpdater\TypeSW", false) != null && xml[@"DEXUpdater\TypeSW"].Text != "-1")
                    {
                        setCheckRadio(gbTypeSW, Convert.ToInt32(xml[@"DEXUpdater\TypeSW"].Text, 10));
                    }
                }
                catch (Exception)
                {
                }
            }
            lbLog.Items.Clear();
        }

        public string getAppRun()
        {
            string[] arr = new string[] { "dex_mega_efd_bridge", "DEXOffice", "dexol", "Kassa3" };
            dcfg = Path.GetDirectoryName(Application.ExecutablePath) + @"\config.xml";
            SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
            return arr[Convert.ToInt32(xml[@"DEXUpdater\TypeSW"].Text, 10)];
        }
 
        private void bSelectUpDir_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(fbd.SelectedPath))
                {
                    tbSelectUpDir.Text = fbd.SelectedPath;
                    if (!File.Exists(fbd.SelectedPath + @"\update.xml"))
                    {
                        MessageBox.Show("Внимание!\n\nВ указанном каталоге отсутствует файл \"update.xml\"");
                    }
                }
            }
        }

        private void UpdMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SimpleXML xml = new SimpleXML("config");
                if (File.Exists(dcfg))
                {
                    xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                }

                xml[@"DEXUpdater\TypeRepo"].Text = getCheckRadio(gbTypeRepo);
                xml[@"DEXUpdater\TypeSW"].Text = getCheckRadio(gbTypeSW);

                if (rbLoc.Checked)
                {
                    xml[@"DEXUpdater\UpdateDir"].Text = tbSelectUpDir.Text;
                }
                else if (rbRem.Checked)
                {
                    xml[@"DEXUpdater\UpdateDirRem"].Text = tbSelectUpDir.Text;
                } 

                File.WriteAllText(dcfg, SimpleXML.SaveXml(xml));
            }
            catch (Exception)
            {
            }
        }

        public void setCheckRadio(GroupBox name, int number)
        {
            int i = 0;
            //int number = Convert.ToInt32(str, 10);
            foreach (Control control in name.Controls)
            {
                if (control.GetType() == typeof(System.Windows.Forms.RadioButton) && i == number)
                {
                    RadioButton rbControl = (RadioButton)control;
                    rbControl.Checked = true;
                }
                i++;

            }
        }

        public string getCheckRadio(GroupBox name)
        {
            int i = 0;
            foreach (Control control in name.Controls)
            {
                if (control.GetType() == typeof(System.Windows.Forms.RadioButton))
                {
                    RadioButton rbControl = (RadioButton)control;
                    if (rbControl.Checked)
                    {
                        return i.ToString();
                    }
                }
                i++;

            }
            return "-1";
        }

        public string FileToMD5(string source)
        {
            string ret = "";
            try
            {
                MD5 md5 = MD5.Create();
                FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read);
                byte[] data = md5.ComputeHash(fs);
                fs.Close();
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                ret = sBuilder.ToString();
            }
            catch (Exception)
            {
            }
            return ret;
        }

        string _l(string msg)
        {
            try
            {
                int i = lbLog.Items.Add(msg);
                lbLog.SelectedIndex = i;
                Application.DoEvents();
            }
            catch (Exception)
            {
            }
            return msg;
        }

        private bool CheckUpdateFromRemote(string rootdir, string updir)
        {
            try
            {
                WebRequest request = WebRequest.Create(updir + @"\update.xml");
                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                if (res.StatusDescription == "OK")
                {
                    using (StreamReader stream = new StreamReader(
                        res.GetResponseStream(), Encoding.UTF8))
                    {
                        string ss = stream.ReadToEnd();
                        MD5 md5 = MD5.Create();
                        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(ss));
                        StringBuilder sBuilder = new StringBuilder();
                        for (int i = 0; i < data.Length; i++)
                        {
                            sBuilder.Append(data[i].ToString("x2"));
                        }
                        string newf = sBuilder.ToString();
                        md5Remote = newf;
                        string oldf = FileToMD5(rootdir + @"\update.xml");
                        if (oldf.Equals(newf)) return false;
                        else return true;
                    }
                }
                else
                {
                    MessageBox.Show(_l("Указанный каталог не является каталогом обновления"));
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Проверьте правильность удаленного адреса");
                return true;
            }
            
            
        }
       
        private void bDoUpdate_Click(object sender, EventArgs e)
        {
            ControlBox = false;
            tbSelectUpDir.Enabled = false;
            bSelectUpDir.Enabled = false;
            bDoUpdate.Enabled = false;
            cbForceUpdate.Enabled = false;

            gbTypeSW.Enabled = false;
            gbTypeRepo.Enabled = false;
            btn_saveconf.Enabled = false;

            lbLog.Items.Clear();
            string rootdir = Path.GetDirectoryName(Application.ExecutablePath);
            string updir = tbSelectUpDir.Text;
            string oldf = FileToMD5(rootdir + @"\update.xml");

            bool flagUdateXml = true;
            bool flagUpdate = true;
            int typeRepo = Convert.ToInt32(getCheckRadio(gbTypeRepo), 10);
            try
            {
                if ( typeRepo == 1 )
                {
                    if ( !File.Exists(updir + @"\update.xml") )
                    {
                        flagUdateXml = false;
                    }
                    else
                    {
                        string newf = FileToMD5(updir + @"\update.xml");
                        if ( !cbForceUpdate.Checked && oldf.Equals(newf) )
                        {
                            flagUpdate = false;
                        }
                    }
                }
                else
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(updir + @"/update.xml");
                        HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                        if ( res.StatusDescription != "OK" )
                        {
                            flagUdateXml = false;
                        }
                        else
                        {
                            string newf = md5Remote;
                            if ( !cbForceUpdate.Checked && !CheckUpdateFromRemote(rootdir, updir) )
                            {
                                flagUpdate = false;
                            }
                        }
                    } 
                    catch(Exception) 
                    {
                        MessageBox.Show("Ошибка запроса на удаленный сервер");
                    }
                }
                if ( !flagUdateXml )
                {
                    MessageBox.Show(_l("Указанный каталог не является каталогом обновления"));
                }
                else
                {
                    if ( !flagUpdate )
                    {
                        MessageBox.Show(_l("Обновление не требуется"));
                    }
                    else
                    {
                        SimpleXML xml;
                        bool flagErrorRequest = false;
                        if ( typeRepo == 1 )
                        {
                            xml = SimpleXML.LoadXml(File.ReadAllText(updir + @"\update.xml"));
                        }
                        else
                        {
                            WebRequest request = WebRequest.Create(updir + @"\update.xml");
                            HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                            if ( res.StatusDescription == "OK" )
                            {
                                using ( StreamReader stream = new StreamReader(
                                    res.GetResponseStream(), Encoding.UTF8) )
                                {
                                    string ss = stream.ReadToEnd();
                                    xml = SimpleXML.LoadXml(ss);
                                }
                            }
                            else
                            {
                                xml = SimpleXML.LoadXml(File.ReadAllText(rootdir + @"\update.xml"));
                                flagErrorRequest = true;
                            }
                        }
                        if ( !flagErrorRequest && xml.GetNodeByPath("signatures", false) == null )
                        {
                            MessageBox.Show(_l("Ошибка: дескриптор обновления не содержит информации (1)"));
                        }
                        else
                        {
                            ArrayList nodes = xml["signatures"].GetChildren("signature");
                            if ( nodes == null || nodes.Count < 1 )
                            {
                                MessageBox.Show(_l("Ошибка: дескриптор обновления не содержит информации (2)"));
                            }
                            int copiedCount = 0, copyErrorsCount = 0,
                                   deletedCount = 0, deleteErrorsCount = 0,
                                   keepCount = 0, allCount = 0;
                            pb.Minimum = 0;
                            pb.Maximum = nodes.Count;
                            pb.Value = allCount;
                            foreach ( SimpleXML node in nodes )
                            {
                                if ( node.Attributes.ContainsKey("file") && node.Attributes.ContainsKey("signature") )
                                {
                                    string action = node.Attributes["signature"];
                                    string fn = node.Attributes["file"];

                                    lMsg.Text = _l("Обработка: " + fn);
                                    pb.Value = allCount;
                                    Application.DoEvents();
                                    if ( action.Equals("keep") )
                                    {
                                        // Ничего не делаем
                                        _l("Оставлен");
                                        keepCount++;
                                    }
                                    else if ( action.Equals("delete") )
                                    {
                                        try
                                        {
                                            File.Delete(rootdir + @"\" + fn);
                                            deletedCount++;
                                            _l("Удалён");
                                        }
                                        catch ( Exception )
                                        {
                                            deleteErrorsCount++;
                                            _l("Ошибка удаления");
                                        }
                                    }
                                    else
                                    {
                                        if ( typeRepo == 1 )
                                        {
                                            try
                                            {
                                                if ( File.Exists(updir + @"\" + fn) )
                                                {
                                                    string fdir = Path.GetDirectoryName(rootdir + @"\" + fn);
                                                    if ( !Directory.Exists(fdir) )
                                                    {
                                                        Directory.CreateDirectory(fdir);
                                                    }
                                                    File.Copy(updir + @"\" + fn, rootdir + @"\" + fn, true);
                                                    copiedCount++;
                                                    _l("Скопирован");
                                                }
                                            }
                                            catch ( Exception )
                                            {
                                                copyErrorsCount++;
                                                _l("Ошибка копирования");
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                WebClient myWebClient = new WebClient();
                                                string myStringWebResource = updir + @"\" + fn;
                                                string str = updir + Regex.Replace(fn, @"\\", @"/");

                                                string fdir = Path.GetDirectoryName(rootdir + @"\" + fn);
                                                if ( !Directory.Exists(fdir) )
                                                {
                                                    Directory.CreateDirectory(fdir);
                                                }
                                                myWebClient.DownloadFile(str, rootdir + @"\" + fn);
                                                copiedCount++;
                                                _l("Скопирован");
                                            }
                                            catch ( Exception )
                                            {
                                                copyErrorsCount++;
                                                _l("Ошибка копирования");
                                            }
                                        }
                                    }
                                    allCount++;
                                }
                            }
                            string summary = string.Format("Итог обновления:\n\n" +
                                "Скопировано: {0}\nУдалено: {1}\nОставлено: {2}\nОшибок удаления: {3}\n" +
                                "Ошибок копирования: {4}\nВсего операций: {5}\n\n",
                                copiedCount, deletedCount, keepCount, deleteErrorsCount,
                                copyErrorsCount, allCount);

                            if ( copyErrorsCount + deleteErrorsCount == 0 )
                            {
                                summary += _l("Обновление прошло успешно");
                                if ( typeRepo == 1 )
                                {
                                    File.Copy(updir + @"\update.xml", rootdir + @"\update.xml", true);
                                }
                                else
                                {
                                    try
                                    {
                                        WebClient myWebClient = new WebClient();
                                        myWebClient.DownloadFile(updir + @"/update.xml", rootdir + @"\update.xml");
                                    }
                                    catch ( Exception )
                                    {
                                        MessageBox.Show("Попытка скачать файл update.xml окончилась неудачей");
                                    }
                                }
                            }
                            else
                            {
                                summary += _l("Не удалось произвести обновление.") +
                                              "\nПопробуйте перезагрузить компьютер и произвести повторное обновление.";
                            }
                            pb.Value = 0;
                            lMsg.Text = "";

                            if ( MessageBox.Show(summary, "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes )
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(getAppRun() + ".exe");
                                    Environment.Exit(0);
                                }
                                catch ( Exception )
                                {
                                    MessageBox.Show("Не могу запустить " + getAppRun() + ".exe. Проверьте наличие исполняемого файла");
                                }
                            }
                        }

                    }
                }
            }
            catch ( Exception )
            {
                MessageBox.Show(_l("Системная ошибка"));
            }
            try
            {
                FileStream fs;
                if ( File.Exists(rootdir + @"\updatelog.txt") )
                {
                    fs = new FileStream(rootdir + @"\updatelog.txt", FileMode.Append);
                }
                else
                {
                    fs = new FileStream(rootdir + @"\updatelog.txt", FileMode.Create);
                }

                TextWriter tw = new StreamWriter(fs);
                tw.WriteLine("===============================================");
                tw.WriteLine(string.Format("Дата: {0}", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")));
                foreach ( string s in lbLog.Items )
                {
                    tw.WriteLine(s);
                }
                tw.Flush();
                tw.Close();
                fs.Close();
            }
            catch ( Exception )
            {
            }
            ControlBox = true;
            tbSelectUpDir.Enabled = true;
            bSelectUpDir.Enabled = true;
            bDoUpdate.Enabled = true;
            cbForceUpdate.Enabled = true;

            gbTypeSW.Enabled = true;
            gbTypeRepo.Enabled = true;
            btn_saveconf.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //выбор локального
            bSelectUpDir.Visible = true;
            if (File.Exists(dcfg)) 
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                if (xml.GetNodeByPath(@"DEXUpdater\UpdateDir", false) != null)
                {
                    tbSelectUpDir.Text = xml[@"DEXUpdater\UpdateDir"].Text;
                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            // выбор удаленного
            bSelectUpDir.Visible = false;
            if (File.Exists(dcfg))
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                if (xml.GetNodeByPath(@"DEXUpdater\UpdateDirRem", false) != null) tbSelectUpDir.Text = xml[@"DEXUpdater\UpdateDirRem"].Text;
            }
        }

        private void SaveConfigugation(object sender, EventArgs e)
        {
            try
            {
                SimpleXML xml = new SimpleXML("config");
                if ( File.Exists(dcfg) )
                {
                    xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                }

                xml[@"DEXUpdater\TypeRepo"].Text = getCheckRadio(gbTypeRepo);
                xml[@"DEXUpdater\TypeSW"].Text = getCheckRadio(gbTypeSW);

                if (rbLoc.Checked)
                {
                    xml[@"DEXUpdater\UpdateDir"].Text = tbSelectUpDir.Text;
                }
                else if (rbRem.Checked)
                {
                    xml[@"DEXUpdater\UpdateDirRem"].Text = tbSelectUpDir.Text;
                }

                File.WriteAllText(dcfg, SimpleXML.SaveXml(xml));
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
       
    }
}
