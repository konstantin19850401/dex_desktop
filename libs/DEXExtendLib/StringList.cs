using System;
using System.Collections.Generic;
using System.Text;

namespace DEXExtendLib
{
    public class StringList
    {
        private Dictionary<string, string> sl;

        public string this[string kname]
        {
            get
            {
                try
                {
                    if (sl.ContainsKey(kname)) return sl[kname];
                }
                catch (Exception)
                {
                }

                return "";
            }
            set
            {
                sl[kname] = value;
            }
        }

        public int Count
        {
            get
            {
                return sl.Count;
            }
        }

        public bool ContainsKey(string kname)
        {
            try
            {
                return sl.ContainsKey(kname);
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool ContainsKeys(string[] keys)
        {
            foreach (string k in keys)
            {
                if (!ContainsKey(k)) return false;
            }
            return true;
        }

        public StringList()
        {
            sl = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        }

        public StringList(string text)
        {
            sl = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            LoadFromString(text);
        }

        public Dictionary<string, string> getDictonary()
        {
            return sl;
        }

        public void Remove(string key)
        {
            try
            {
                sl.Remove(key);
            }
            catch (Exception)
            {
            }
        }

        public void LoadFromString(string text)
        {
            string[] st = text.Split(new string[] { "\n", "\r\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
            if (st != null && st.Length > 0)
            {
                foreach (string v in st)
                {
                    if (v != null && v.Trim().Length > 0 && v.IndexOf('=') > 0)
                    {
                        sl[v.Substring(0, v.IndexOf('='))] = v.Substring(v.IndexOf('=') + 1);
                    }
                }
            }
        }

        public string SaveToString()
        {
            string ret = "";
            foreach (KeyValuePair<string, string> kvp in sl)
            {
                if (!ret.Equals("")) ret += "\n";
                ret += string.Format("{0}={1}", kvp.Key, kvp.Value);
            }
            return ret;
        }

    }
}
