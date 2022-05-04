using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DEXPlugin.Journalhook.Yota.SenderProfile
{
    public partial class ProfileEditEd : Form
    {
        List<string> pcodes;
        string initialPcode;

        public ProfileEditEd(string pname, string pcode, List<string> pcodes)
        {
            InitializeComponent();
            this.pcodes = pcodes;
            tbPname.Text = pname;
            tbPcode.Text = pcode;
            initialPcode = pcode;
            tbPname.Focus();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            string er = "";
            if (tbPname.Text.Trim().Equals("")) er += "* Не указано наименование\n";
            string fpcode = tbPcode.Text.Trim();
            if (fpcode.Equals("")) er += "* Не указан код\n";
            else
            {
                if (pcodes.Contains(fpcode) && !fpcode.Equals(initialPcode))
                {
                    er += "* Указанный код уже использован в другой записи\n";
                }
            }

            if (er == "") DialogResult = DialogResult.OK;
            else MessageBox.Show("Ошибки:\n\n" + er);
        }
    }
}
