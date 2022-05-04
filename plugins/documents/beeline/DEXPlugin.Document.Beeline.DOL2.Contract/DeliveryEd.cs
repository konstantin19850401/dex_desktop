using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DEXExtendLib;

namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    public partial class DeliveryEd : Form
    {
        public DeliveryEd()
        {
            InitializeComponent();
        }

        public bool prepareForm(string fn)
        {
            try
            {
                SimpleXML xml = SimpleXML.LoadXml(File.ReadAllText(fn, Encoding.GetEncoding(1251)));
                ArrayList offices = xml.GetChildren("OFFICE");
                lbOffice.Items.Clear();
                foreach (SimpleXML office in offices)
                {
                    lbOffice.Items.Add(new StringObjTagItem(office["name"].Text, office));
                }

                return true;
            }
            catch (Exception) { }
            return false;
        }

        private void DeliveryEd_Shown(object sender, EventArgs e)
        {
            try
            {
                lbOffice.SelectedIndex = 0;
            }
            catch (Exception) { }

            lbOffice.Focus();
        }

        private void lbOffice_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
