using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.Beeline.Sim
{
    public partial class FSimUnitsEd : Form
    {
        Object toolbox;
        public FSimUnitsEd(Object AToolBox)
        {
            toolbox = AToolBox;
            InitializeComponent();
            DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");

            try
            {

                IDEXData d = (IDEXData)toolbox;
                DataTable dt = d.getQuery("SELECT * FROM units");
                foreach (DataRow dr in t.Rows)
                {
                    foreach (DataRow dtdr in dt.Rows)
                    {
                        if (dtdr["uid"].ToString().Equals(dr["uid"].ToString()))
                        {
                            if (!dtdr["region"].ToString().Equals(""))
                            {
                                dr["title"] = dtdr["title"].ToString() + " [" + dtdr["region"].ToString() + "]";
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception) { }


            StringTagItem.UpdateCombo(cbUnit, t, null, "uid", "title", false);
        }
    }
}
