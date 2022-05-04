using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using DEXExtendLib;

namespace DEXOffice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("lastconsole.txt");
            Console.SetOut(sw);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.IndexOf("safemode") > -1) SAFE_MODE = true;
                }
            }

            log(" ");
            log("BEGIN " + DateTime.Now.ToString());

            if (Program.CheckNeedUpdate())
            {
                if (MessageBox.Show("Доступна новая версия программы.\nПроизвести обновление?",
                    "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Run updater
                    System.Diagnostics.Process.Start(
                        Path.GetDirectoryName(Application.ExecutablePath) + @"\DEXUpdater.exe"
                    );
                    Application.Exit();
                    return;
                }
            }

            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Program.log("login.ShowDialog() == DialogResult.OK");
                DEXToolBox tb = DEXToolBox.getToolBox();
                Program.log("toolbox got");
                tb.Plugins.ScanPlugins(tb.AppDir + @"\plugins\");
                Program.log("plugins scanned");
                tb.ParseUserData(tb.sUserData);
                Program.log("userdata parsed");

                Program.log("Application.Run(new Main());");
                Application.Run(
                    new Main(
                        login.cbSMIgnoreDateInterval.Checked, 
                        login.cbSMClearSearchSettings.Checked, 
                        login.cbClearPrinterSettings.Checked)
                        );

                tb.EndSchedule();
            }
            else
            {
                Application.Exit();
            }
            sw.Flush();
            sw.Close();
        }

        static bool CheckNeedUpdate()
        {
            bool ret = false;
            string rtdir = Path.GetDirectoryName(Application.ExecutablePath);
            if (File.Exists(rtdir + @"\DEXUpdater.exe") && File.Exists(rtdir + @"\config.xml"))
            {
                try
                {
                    SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(rtdir + @"\config.xml"));
                    if (xml.GetNodeByPath(@"DEXUpdater\UpdateDir", false) != null)
                    {
                        string upath = xml[@"DEXUpdater\UpdateDir"].Text;
                        string newf = Program.FileToMD5(upath + @"\update.xml");
                        string oldf = Program.FileToMD5(rtdir + @"\update.xml");
/*
                        MessageBox.Show(string.Format("{0}: {1}\n{2}: {3}",
                            upath + @"\update.xml", newf, rtdir + @"\update.xml", oldf));
 */
                        ret = !newf.Equals(oldf);
                    }
                }
                catch (Exception)
                {
                }
            }
            
            return ret;
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
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                ret = sBuilder.ToString();
            }
            catch (Exception)
            {
//                MessageBox.Show("Exception: " + ex.Message);
            }
            return ret;
        }

        public static bool SAFE_MODE = false;

        public static void log(string s)
        {
//            if (SAFE_MODE) {
                string fn = Application.ExecutablePath + ".log";
                try
                {

                    FileStream fs = new FileStream(fn, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(s);
                    sw.Close();
                    
                }
                catch (Exception)
                {
                }
//            }
        }

    }
}
