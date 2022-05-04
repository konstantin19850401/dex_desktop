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
    public partial class TextViewer : Form
    {
        string tvId;
        FormState fs;

        public TextViewer(string tvId, string title, string text)
        {
            InitializeComponent();
            this.tvId = tvId;
            this.Text = title;
            tbText.Text = text;
            fs = new FormState(Tools.instance.dataDir + @"\" + tvId + ".fs");
        }

        ~TextViewer()
        {
            fs = null;
        }

        private void TextViewer_Shown(object sender, EventArgs e)
        {
            fs.ApplyToForm(this);
        }

        private void TextViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            fs.UpdateFromForm(this);
            fs.SaveToFile(Tools.instance.dataDir + @"\" + tvId + ".fs");
        }
    }
}
