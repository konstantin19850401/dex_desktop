using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXExtendLib
{
    public interface IWaitMessageEventArgs
    {
        object[] args { get; }
        bool canAbort { set; }
        bool isAborted { get; }
        int progressValue { set; }
        int minValue { set; }
        int maxValue { set; }
        bool progressVisible { get; set; }
        string textMessage { set; }
        void DoEvents();
    }
    
    public delegate string WaitMessageEvent (IWaitMessageEventArgs e);

    public partial class WaitMessage : Form, IWaitMessageEventArgs
    {
        object[] fargs;
        bool fisAborted;
        public WaitMessageEvent wme;
        string ret;

        public WaitMessage()
        {
            InitializeComponent();
            fisAborted = false;
            bCancel.Enabled = false;
            lText.Visible = false;
            this.Text = "";
            progressBar1.Visible = false;
        }

        public object[] args {
            get
            {
                return fargs;
            }
        }

        public bool canAbort
        {
            set
            {
                bCancel.Enabled = value;
                this.Refresh();
            }
        }

        public bool isAborted
        {
            get
            {
                return fisAborted; 
            }
        }

        public int progressValue
        {
            set
            {
                progressBar1.Value = value;
                this.Refresh();
                Application.DoEvents();
            }
        }

        public int minValue
        {
            set
            {
                progressBar1.Minimum = value;
                this.Refresh();
                Application.DoEvents();
            }
        }

        public int maxValue
        {
            set
            {
                progressBar1.Maximum = value;
                this.Refresh();
                Application.DoEvents();
            }
        }

        public bool progressVisible
        {
            get
            {
                Application.DoEvents();
                return progressBar1.Visible;
            }
            set
            {
                progressBar1.Visible = value;
                Application.DoEvents();
            }
        }

        public string textMessage
        {
            set
            {
                lText.Text = value;
                lText.Visible = true;
                this.Refresh();
                Application.DoEvents();
            }
        }

        public void DoEvents()
        {
            Application.DoEvents();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
//            if (MessageBox.Show("Прервать операцию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
            Application.DoEvents();
            fisAborted = true;
//            }
        }

        private void WaitMessage_Shown(object sender, EventArgs e)
        {
            ret = "";
            try
            {
                ret = wme.Invoke(this);
                DialogResult = DialogResult.OK;
            } catch(Exception ex)
            {
                DialogResult = DialogResult.Abort;
                ret += "* Ошибка выполнения функции\nПолный текст сообщения об ошибке в буфере обмена.";
                Clipboard.SetText(ex.ToString());
            }
            if (isAborted)


            {
                DialogResult = DialogResult.Abort;
                ret += "* Выполнение функции прервано\n";
            }
        }

        public static string Execute(WaitMessageEvent wme, params object[] args)
        {
            WaitMessage form = new WaitMessage();
            form.wme = wme;
            form.fargs = args;
            form.ShowDialog();
            return form.ret;            
        }
    }
}
