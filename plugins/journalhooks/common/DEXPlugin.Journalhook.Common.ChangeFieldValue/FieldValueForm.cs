using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Journalhook.Common.ChangeFieldValue
{
    public partial class FieldValueForm : Form
    {
        public string returnValue = null;
        public string vtype = "";
        //public StringObjTagItem cbl;

        public FieldValueForm(object toolbox, string ltitle, string vtype)
        {
            InitializeComponent();
            this.vtype = vtype;
            cbValue.Checked = false;
            cbList.Visible = false;
            tbValue.Text = "";
            deValue.Value = DateTime.Now;
            lFieldTitle.Text = ltitle;
            
            cbValue.Visible = "b".Equals(vtype);
            tbValue.Visible = "s".Equals(vtype);
            deValue.Visible = "d".Equals(vtype);
            if ( "u".Equals(vtype) )
                cbList.Visible = true;
            /*
            if ( "t".Equals(vtype) )
                cbList.Visible = true;
            */
            if ("u".Equals(vtype))
            {
                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery("select uid, title from `units` order by title");
                StringTagItem.UpdateCombo(cbList, dt, null, "uid", "title", false);
            }
            // изменение тарифного плана (костя) 08.10.2015 начало
            /*
            if ( "t".Equals(vtype) )
            {
                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery("select plan_id, title from `um_plans` order by title");
                StringTagItem.UpdateCombo(cbList, dt, null, "plan_id", "title", false);
            }
            */
            // изменение тарифного плана (костя) 08.10.2015 конец
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if ("b".Equals(vtype)) returnValue = cbValue.Checked ? "X" : "-";
            else if ("s".Equals(vtype)) returnValue = tbValue.Text;
            else if ("d".Equals(vtype)) returnValue = deValue.Value.ToString("yyyyMMdd");
            else if ("u".Equals(vtype)) 
            {
                StringTagItem sti = (StringTagItem)cbList.SelectedItem;
                returnValue = sti == null ? null : sti.Tag;
            }
            // изменение тарифного плана (костя) 08.10.2015 начало
            /*    
            else if ("t".Equals(vtype)) 
            {
                StringTagItem sti = (StringTagItem)cbList.SelectedItem;
                returnValue = sti == null ? null : sti.Tag;
                
            }
            */
            // изменение тарифного плана (костя) 08.10.2015 конец
            
            DialogResult = DialogResult.OK;
        }
    }
}
