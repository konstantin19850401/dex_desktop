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

namespace DEXUpdater
{
    public partial class UpdMain : Form
    {
        string dcfg;

        public UpdMain()
        {
            InitializeComponent();
            dcfg = Path.GetDirectoryName(Application.ExecutablePath) + @"\config.xml";
            tbSelectUpDir.Text = "";
            if (File.Exists(dcfg))
            {
                try
                {
                    SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(dcfg));
                    if (xml.GetNodeByPath(@"DEXUpdater\UpdateDir", false) != null)
                    {
                        tbSelectUpDir.Text = xml[@"DEXUpdater\UpdateDir"].Text;
                    }
                }
                catch (Exception)
                {
                }
            }
            lbLog.Items.Clear();
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
                xml[@"DEXUpdater\UpdateDir"].Text = tbSelectUpDir.Text;
                File.WriteAllText(dcfg, SimpleXML.SaveXml(xml));
            }
            catch (Exception)
            {
            }
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

        private void bDoUpdate_Click(object sender, EventArgs e)
        {
            ControlBox = false;
            tbSelectUpDir.Enabled = false;
            bSelectUpDir.Enabled = false;
            bDoUpdate.Enabled = false;
            cbForceUpdate.Enabled = false;

            lbLog.Items.Clear();
            string rootdir = Path.GetDirectoryName(Application.ExecutablePath);

            try
            {
                string updir = tbSelectUpDir.Text;
                if (!File.Exists(updir + @"\update.xml"))
                {
                    MessageBox.Show(_l("Указанный каталог не является каталогом обновления"));
                }
                else
                {

                    string oldf = FileToMD5(rootdir + @"\update.xml");
                    string newf = FileToMD5(updir + @"\update.xml");
                    if (!cbForceUpdate.Checked && oldf.Equals(newf))
                    {
                        MessageBox.Show(_l("Обновление не требуется"));
                    }
                    else
                    {
                        SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(updir + @"\update.xml"));
                        if (xml.GetNodeByPath("signatures", false) == null)
                        {
                            MessageBox.Show(_l("Ошибка: дескриптор обновления не содержит информации (1)"));
                        }
                        else
                        {
                            ArrayList nodes = xml["signatures"].GetChildren("signature");
                            if (nodes == null || nodes.Count < 1)
                            {
                                MessageBox.Show(_l("Ошибка: дескриптор обновления не содержит информации (2)"));
                            }
                            else
                            {
                                int copiedCount = 0, copyErrorsCount = 0,
                                    deletedCount = 0, deleteErrorsCount = 0, 
                                    keepCount = 0, allCount = 0;
                                pb.Minimum = 0;
                                pb.Maximum = nodes.Count;
                                pb.Value = allCount;
                                foreach (SimpleXML node in nodes)
                                {
                                    if (node.Attributes.ContainsKey("file") && node.Attributes.ContainsKey("signature"))
                                    {
                                        string action = node.Attributes["signature"];
                                        string fn = node.Attributes["file"];

                                        lMsg.Text = _l("Обработка: " + fn);
                                        pb.Value = allCount;
                                        Application.DoEvents();

                                        if (action.Equals("keep"))
                                        {
                                            // Ничего не делаем
                                            _l("Оставлен");
                                            keepCount++;
                                        }
                                        else if (action.Equals("delete"))
                                        {
                                            try
                                            {
                                                File.Delete(rootdir + @"\" + fn);
                                                deletedCount++;
                                                _l("Удалён");
                                            }
                                            catch (Exception)
                                            {
                                                deleteErrorsCount++;
                                                _l("Ошибка удаления");
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (File.Exists(updir + @"\" + fn))
                                                {
                                                    string fdir = Path.GetDirectoryName(rootdir + @"\" + fn);
                                                    if (!Directory.Exists(fdir))
                                                    {
                                                        Directory.CreateDirectory(fdir);
                                                    }
                                                    File.Copy(updir + @"\" + fn, rootdir + @"\" + fn, true);
                                                    copiedCount++;
                                                    _l("Скопирован");
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                copyErrorsCount++;
                                                _l("Ошибка копирования");
                                            }
                                        }
                                    }

                                    allCount++;
                                }

                                string summary = string.Format("Итог обновления:\n\n" +
                                    "Скопировано: {0}\nУдалено: {1}\nОставлено: {2}\nОшибок удаления: {3}\n" +
                                    "Ошибок копирования: {4}\nВсего операций: {5}\n\n",
                                    copiedCount, deletedCount, keepCount, deleteErrorsCount,
                                    copyErrorsCount, allCount);


                                if (copyErrorsCount + deleteErrorsCount == 0)
                                {
                                    summary += _l("Обновление прошло успешно");
                                    File.Copy(updir + @"\update.xml", rootdir + @"\update.xml", true);
                                }
                                else
                                {
                                    summary += _l("Не удалось произвести обновление.") +
                                               "\nПопробуйте перезагрузить компьютер и произвести повторное обновление.";
                                }

                                pb.Value = 0;
                                lMsg.Text = "";

                                MessageBox.Show(summary);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_l("Системная ошибка"));
            }

            try
            {
                FileStream fs;
                if (File.Exists(rootdir + @"\updatelog.txt"))
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
                foreach (string s in lbLog.Items)
                {
                    tw.WriteLine(s);
                }
                tw.Flush();
                tw.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }

            ControlBox = true;
            tbSelectUpDir.Enabled = true;
            bSelectUpDir.Enabled = true;
            bDoUpdate.Enabled = true;
            cbForceUpdate.Enabled = true;
        }


    }
}
