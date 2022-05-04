using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DEXExtendLib;

namespace DEXExtendLib
{
    public class StringObjTagItem
    {
        public string Text;
        public Object Tag;

        public StringObjTagItem(string text, Object tag)
        {
            Text = text;
            Tag = tag;
        }

        public override string ToString()
        {
            return Text;
        }

        public static void SelectByTag(ComboBox cb, Object tag, bool reset)
        {
            try
            {
                bool found = false;

                foreach (StringObjTagItem i in cb.Items)
                {
                    if (i.Tag.Equals(tag))
                    {
                        cb.SelectedItem = i;
                        found = true;
                        break;
                    }
                }

                if (!found && reset)
                {
                    cb.SelectedIndex = -1;
                    cb.SelectedItem = null;
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
