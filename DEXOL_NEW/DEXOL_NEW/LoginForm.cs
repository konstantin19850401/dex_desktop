using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXOL_NEW
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void loginClick(object sender, EventArgs e)
        {
            String login = te_login.Text;
            String password = te_password.Text;
            Toolbox tb = Toolbox.getToolBox();
            Comet comet = tb.comet;
            await comet.Login(login, password, this);
        }

        private void te_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btn_ok.PerformClick();
            }
        }

        private void te_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                te_password.Focus();
            }
        }
    }
}
