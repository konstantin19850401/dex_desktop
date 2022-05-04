using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;


namespace DEXExtendLib
{
    public class StringTagItem
    {
        public static string VALUE_ANY = "@ANY#";
        
        
        public string Text;
        public string Tag;

        public StringTagItem(string text, string tag)
        {
            Text = text;
            Tag = tag;
        }

        public override string ToString()
        {
            return Text;
        }

        public static void UpdateCombo(ComboBox cb, DataTable t, string anyName,
            string tagField, string titleField, bool saveCurrent)
        {
            string sfi = (saveCurrent &&
                          cb.Items.Count > 0 &&
                          cb.SelectedIndex > -1) ?
                          ((StringTagItem)cb.Items[cb.SelectedIndex]).Tag : VALUE_ANY;

            cb.Items.Clear();

            if (anyName != null)
            {
                cb.Items.Add(new StringTagItem(anyName, VALUE_ANY));
            }

            if (t != null && t.Rows.Count > 0)
            {
                foreach (DataRow r in t.Rows)
                {
                    cb.Items.Add(new StringTagItem(r[titleField].ToString(), r[tagField].ToString()));
                }
                cb.SelectedIndex = 0;
            }

            int c = 0;
            foreach (StringTagItem sf in cb.Items)
            {
                if (sfi.Equals(sf.Tag))
                {
                    cb.SelectedIndex = c;
                    break;
                }
                c++;
            }
        }

        public static void SelectByTag(ComboBox cb, string tag, bool reset)
        {
            try
            {
                bool found = false;
                foreach (StringTagItem i in cb.Items)
                {
                    if (i.Tag.Equals(tag))
                    {
                        cb.SelectedItem = i;
                        found = true;
                        break;
                    }
                }
                if (reset && !found)
                {
                    cb.SelectedIndex = -1;
                    cb.SelectedItem = null;
                }
            }
            catch (Exception)
            {
            }
        }

        public static string getSelectedTag(ComboBox cb, string _default)
        {
            try
            {
                return ((StringTagItem)(cb.SelectedItem)).Tag;
            }
            catch (Exception) { }

            return _default;
        }
    }
}
