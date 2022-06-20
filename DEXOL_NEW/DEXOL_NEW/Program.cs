using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace DEXOL_NEW
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

            Comet comet = new Comet(@"http", @"37.29.115.178", 3021);
            
            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                //await comet.InitSubscriptionAsync();
                Application.Run(new Main());
            }

            //Application.Run(new Main());
            //Application.Run(new LoginForm());
        }
    }
}
