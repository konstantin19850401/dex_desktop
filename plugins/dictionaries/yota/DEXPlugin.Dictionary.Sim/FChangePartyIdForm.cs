using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Yota.Sim
{
    public partial class Form1 : Form
    {
        object toolbox;
        int oldParty;
        bool selectedCards;

        public Form1(object toolbox, int oldParty, bool selectedCards)
        {
            this.toolbox = toolbox;
            this.oldParty = oldParty;
            this.selectedCards = selectedCards;

            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (!nudPartyId.Value.ToString().Equals(nudPartyId.Text))
            {
                MessageBox.Show("Указано некорректное значение кода партии");
                return;
            }

            if (!selectedCards)
            {
                if ((int)nudPartyId.Value == oldParty)
                {
                    MessageBox.Show("Указанное значение идентично текущему коду партии");
                    return;
                }

                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery(string.Format(
                    "select count(id) as cid from `um_data` where party_id = {0}", nudPartyId.Value.ToString()
                    ));
                if (dt != null && dt.Rows.Count > 0 && int.Parse(dt.Rows[0]["cid"].ToString()) > 0)
                {
                    MessageBox.Show("Партия с таким кодом уже существует");
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
