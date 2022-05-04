using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.IO;
using System.Security.Cryptography;
using System.Text;
using DEXExtendLib;
using System.Net;

namespace Kassa3
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
//            Application.Run(new NetLoginForm());
            IfNeedUpdate();
        }
        static public void IfNeedUpdate()
        {
            try
            {

                string rootDir = Path.GetDirectoryName(Application.ExecutablePath);
                if ( File.Exists(rootDir + @"\DEXUpdater.exe") && File.Exists(rootDir + @"\config.xml") )
                {
                    SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(rootDir + @"\config.xml"));
                    if ( xml.GetNodeByPath(@"\DEXUpdater\TypeRepo", false) == null || xml.GetNodeByPath(@"\DEXUpdater\TypeSW", false) == null || xml.GetNodeByPath(@"\DEXUpdater\UpdateDirRem", false) == null || xml.GetNodeByPath(@"\DEXUpdater\UpdateDir", false) == null )
                    {
                        MessageBox.Show("Пожалуйста, заполните данные для обновления и запустите программу заново");
                        try
                        {
                            System.Diagnostics.Process.Start(
                                Path.GetDirectoryName(Application.ExecutablePath) + @"\DEXUpdater.exe"
                            );
                            Application.Exit();
                        }
                        catch (Exception e)
                        {
                            string s = "";
                        }
                        return;
                    }
                    else
                    {
                        try
                        {
                            int typeRepo = Convert.ToInt32(xml[@"DEXUpdater\TypeRepo"].Text, 10);
                            bool update = false;
                            if ( typeRepo == 0 )
                            {
                                string updir = xml[@"DEXUpdater\UpdateDirRem"].Text;
                                if ( CheckUpdateFromRemote(rootDir, updir) )
                                {
                                    update = true;
                                }
                            }
                            else
                            {
                                string updir = xml[@"DEXUpdater\UpdateDir"].Text;
                                if ( CheckUpdateFromLocal(rootDir, updir) )
                                {
                                    update = true;
                                }
                            }
                            if ( update )
                            {
                                if ( MessageBox.Show("Доступна новая версия программы.\nПроизвести обновление?",
                                        "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes )
                                {
                                    System.Diagnostics.Process.Start(
                                        Path.GetDirectoryName(Application.ExecutablePath) + @"\DEXUpdater.exe"
                                    );
                                    Application.Exit();
                                    return;
                                }
                            }
                            else
                            {
                                
                                Application.Run(new MainForm());
                            }
                        }
                        catch ( Exception )
                        {
                        }
                    }
                }
                else
                {
                    Application.Run(new MainForm());
                }
            }
            catch ( Exception )
            {
                string s = "";
            }
        }
        private static bool CheckUpdateFromLocal(string rootdir, string updir)
        {
            try
            {

                string oldf = FileToMD5(rootdir + @"\update.xml");
                string newf = FileToMD5(updir + @"\update.xml");
                if ( oldf.Equals(newf) )
                    return false;
                else
                    return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }
        private static bool CheckUpdateFromRemote(string rootdir, string updir)
        {
            try
            {
                WebRequest request = WebRequest.Create(updir + @"\update.xml");
                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                if ( res.StatusDescription == "OK" )
                {
                    using ( StreamReader stream = new StreamReader(
                        res.GetResponseStream(), Encoding.UTF8) )
                    {
                        string ss = stream.ReadToEnd();
                        MD5 md5 = MD5.Create();
                        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(ss));
                        StringBuilder sBuilder = new StringBuilder();
                        for ( int i = 0; i < data.Length; i++ )
                        {
                            sBuilder.Append(data[i].ToString("x2"));
                        }
                        string newf = sBuilder.ToString();
                        string oldf = FileToMD5(rootdir + @"\update.xml");
                        if ( oldf.Equals(newf) )
                            return false;
                        else
                            return true;
                    }
                }
                else
                {
                    MessageBox.Show("Указанный каталог не является каталогом обновления");
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Проверьте правильность удаленного адреса");
                return true;
            }
        }
        public static string FileToMD5(string source)
        {
            string ret = "";
            try
            {
                MD5 md5 = MD5.Create();
                FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read);
                byte[] data = md5.ComputeHash(fs);
                fs.Close();
                StringBuilder sBuilder = new StringBuilder();
                for ( int i = 0; i < data.Length; i++ )
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                ret = sBuilder.ToString();
            }
            catch ( Exception )
            {
                //MessageBox.Show("Exception: " + ex.Message);
            }
            return ret;
        }
    }
}
