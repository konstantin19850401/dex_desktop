using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Dictionary.Sim
{
    public partial class FChangeSimType : Form
    {
        object toolbox;
        int oldTypeSim;
        bool selectedCards;

        public FChangeSimType(object toolbox, int oldTypeSim, bool selectedCards)
        {
            this.toolbox = toolbox;
            this.oldTypeSim = oldTypeSim;
            this.selectedCards = selectedCards;

            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            /*
            if (!nudPartyId.Value.ToString().Equals(nudPartyId.Text))
            {
                MessageBox.Show("Указано некорректное значение кода партии");
                return;
            }
            */
            /*
            if (selectedCards)
            {
                if (cbTypeSim.SelectedIndex == oldTypeSim)
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
            */
            DialogResult = DialogResult.OK;
        }
    }
}
