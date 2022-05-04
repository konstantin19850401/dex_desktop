using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kassa3
{
    public partial class PrimForm : Form
    {
        public PrimForm()
        {
            InitializeComponent();
        }

        private void bSort_Click(object sender, EventArgs e)
        {
            string[] ss = tbPrim.Lines;                
            int len = ss.Length;
            if (len > 1)
            {
                for (int i1 = 0; i1 < len - 1; ++i1)
                {
                    for (int i2 = i1 + 1; i2 < len; ++i2)
                    {
                        if (ss[i1].CompareTo(ss[i2]) > 0)
                        {
                            string s = ss[i1];
                            ss[i1] = ss[i2];
                            ss[i2] = s;
                        }
                    }
                }
                tbPrim.Lines = ss;
            }
        }
    }
}
