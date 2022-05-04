using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXOffice
{
    public partial class ApproveForm : Form
    {
        public ApproveForm()
        {
            InitializeComponent();

            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            /*
            int opt = cfg.getInt(this.Name, "DestDocStatus", 0);
            rbStApproved.Checked = (opt == 0);
            rbStToExport.Checked = (opt == 1);
            cbIgnoreStReturned.Checked = cfg.getBool(this.Name, "cbIgnoreStReturned", false);
            cbStReturnedStSent.Checked = cfg.getBool(this.Name, "cbStReturnedStSent", false);
            cbIgnoreStSent.Checked = cfg.getBool(this.Name, "cbIgnoreStSent", false);
             */
            rbStApproved.Checked = false;
            rbStToExport.Checked = false;
            cbIgnoreStReturned.Checked = false;
            cbStReturnedStSent.Checked = false;
            cbIgnoreStSent.Checked = false;

        }

        public void saveForm()
        {
            IDEXConfig cfg = (IDEXConfig)DEXToolBox.getToolBox();
            int opt = (rbStApproved.Checked) ? 0 : 1;
            cfg.setInt(this.Name, "DestDocStatus", opt);
            cfg.setBool(this.Name, "cbIgnoreStReturned", cbIgnoreStReturned.Checked);
            cfg.setBool(this.Name, "cbStReturnedStSent", cbStReturnedStSent.Checked);
            cfg.setBool(this.Name, "cbIgnoreStSent", cbIgnoreStSent.Checked);

        }

        private void cbIgnoreStReturned_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIgnoreStReturned.Checked) cbStReturnedStSent.Checked = false;
        }

        private void cbStReturnedStSent_CheckedChanged(object sender, EventArgs e)
        {
            if (cbStReturnedStSent.Checked) cbIgnoreStReturned.Checked = false;
        }
    }

}
