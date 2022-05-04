using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Windows.Forms;

namespace DEXPlugin.Dictionary.Mega.Sim
{
    public partial class FSimUnitsEd : Form
    {
        Object toolbox;
        public FSimUnitsEd(Object AToolBox)
        {
            toolbox = AToolBox;
            InitializeComponent();
            DataTable t = ((IDEXData)toolbox).getQuery("select * from `units` order by title");
            StringTagItem.UpdateCombo(cbUnit, t, null, "uid", "title", false);
        }
    }
}
