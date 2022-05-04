using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXPlugin.Function.MTS.Autodoc
{
    public partial class AutodocIgnorRegForm : Form
    {
        //DEXToolBox toolbox;
        public static List<string> selectedIdReg = new List<string>();
        public AutodocIgnorRegForm()
        {
            InitializeComponent();
            //toolbox = DEXToolBox.getToolBox();

            //IDEXData d = (IDEXData)toolbox;
            Dictionary<string, string> reg = new Dictionary<string, string>();
            reg.Add("414-416,астрахан", "Астраханская область"); //Астраханская область
            reg.Add("344-347,ростов", "Ростовская область"); //Ростовская, Ростовская область
            reg.Add("400-404,волгогр", "Волгоградская область"); //Ростовская, Ростовская область
            reg.Add("350-354,краснодар", "Краснодарский край"); // Краснодарский, Краснодарский край
            reg.Add("355-357,ставропол", "Ставропольский край"); //Ставропольский край, СТАВРОПОЛЬСКИЙ КРАЙ
            reg.Add("385,адыг", "Республика Адыгея (Адыгея)"); // Адыгея республика, Адыгея
            reg.Add("367-368,дагестан", "Республика Дагестан"); //Дагестан республика,Дагестан
            reg.Add("386,ингуш", "Республика Ингушетия"); //Ингушетия республика
            reg.Add("360-361,кабардин", "Кабардино-Балкарская республика"); //Кабардино-балкарская республика
            reg.Add("358-359,калмык", "Республика Калмыкия"); //Калмыкия республика
            reg.Add("369,карачае", "Карачаево-Черкесская республика"); //Карачаево-черкесская республика
            reg.Add("362-363,алан", "Республика Северная Осетия-Алания"); //Северная осетия-алания республика
            reg.Add("364-366,чечен", "Чеченская республика"); //Чеченская республика
            reg.Add("364-366,коми", "Республика Коми"); //Чеченская республика
            clbIgnorReg.Items.Clear();
                foreach (KeyValuePair<string, string> kvp in reg)
                {
                    StringObjTagItem sti = new StringObjTagItem(kvp.Value, kvp.Key);
                    clbIgnorReg.Items.Add(sti, selectedIdReg.Contains(sti.Tag.ToString()));
                }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            selectedIdReg.Clear();
            foreach (StringObjTagItem soti in clbIgnorReg.CheckedItems)
            {
                selectedIdReg.Add(soti.Tag.ToString());
            }
            DialogResult = DialogResult.OK;
        }

        private void clbUnits_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bool nstate = clbIgnorReg.CheckedItems.Count < clbIgnorReg.Items.Count;
                clbIgnorReg.BeginUpdate();
                try
                {
                    for (int i = 0; i < clbIgnorReg.Items.Count; ++i)
                    {
                        clbIgnorReg.SetItemChecked(i, nstate);
                    }
                }
                finally
                {
                    clbIgnorReg.EndUpdate();
                }
            }
        }
    }
}
