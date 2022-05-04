using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Kassa3
{
    public class ImportRule : IStrColor
    {
        public List<ImportMatch> matches = new List<ImportMatch>();
        public long id;
        public string protocol, title;
        public int op_id;
        public string r_prim;
        public int srctype, src_acc_id, src_client_id, dsttype, dst_acc_id, dst_client_id, status;

        public ImportRule(long id, string protocol, string title, int op_id, string r_prim,
            int srctype, int src_acc_id, int src_client_id,
            int dsttype, int dst_acc_id, int dst_client_id,
            int status, List<ImportMatch> matches)
        {
            this.id = id;
            this.protocol = protocol;
            this.title = title;
            this.op_id = op_id;
            this.r_prim = r_prim;
            this.srctype = srctype;
            this.src_acc_id = src_acc_id;
            this.src_client_id = src_client_id;
            this.dsttype = dsttype;
            this.dst_acc_id = dst_acc_id;
            this.dst_client_id = dst_client_id;
            this.status = status;
            this.matches = matches;
        }

        /// <summary>
        /// Проверяет поля документа на соответствие с правилом импорта.
        /// </summary>
        /// <param name="src">Поля документа</param>
        /// <returns>=0 - документ прошёл проверку, >0 - количество несовпадений в документе, <0 - в правиле не описаны критерии импорта</returns>
        public int check(Dictionary<string, string> src)
        {
            if (matches.Count < 1) return -1;
            int lastUnmatches = matches.Count();
            foreach (ImportMatch match in matches)
            {
                if (match.check(src)) lastUnmatches--;
            }

            return lastUnmatches;
        }

        public override string ToString()
        {
            return (status == 1 ? "" : "[X] ") + title;
        }

        public Color getColor()
        {
            return status == 1 ? Color.Black : Color.Red;
        }

        public string getSignature()
        {
            if (matches.Count < 1) return "";

            string[] ss = new string[matches.Count];
            int c = 0;
            foreach (ImportMatch match in matches) ss[c++] = match.ToString();
            Array.Sort(ss);
            
            StringBuilder sb = new StringBuilder();
            foreach (string s in ss) sb.Append(s);

            return Tools.StringToMD5(sb.ToString());
        }

        public string getR_Prim(Dictionary<string, string> src)
        {
            string ret = r_prim;

            foreach (KeyValuePair<string, string> kvp in src)
            {
                ret = ret.Replace("${" + kvp.Key + "}", kvp.Value);
            }

            return ret;
        }
    }

    public class ImportMatch
    {
        public bool _changed = false;

        public long id;
        public long rule_id;
        public string field_title;
        public string field;
        public int operation;
        public string match;
        Regex regex = null;

        public bool changed = false;

        public ImportMatch(long id, long rule_id, string field, int operation, string match, string field_title)
        {
            this.id = id;
            this.rule_id = rule_id;
            this.field = field;
            this.operation = operation;
            this.match = match;
            if (operation == 4) regex = new Regex(match);
            this.field_title = field_title;
        }

        /// <summary>
        /// Проверка, есть ли соответствие в документе
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool check(Dictionary<string, string> data)
        {
            if (data.ContainsKey(field))
            {
                string fvalue = data[field];

                switch (operation)
                {
                    case 0: return string.Compare(fvalue, match, true) == 0;
                    case 1: return string.Compare(fvalue, match, true) != 0;
                    case 2: return fvalue.Contains(match);
                    case 3: return !fvalue.Contains(match);
                    case 4: return regex.IsMatch(fvalue);
                }
            }

            return false;
        }

        public override string ToString()
        {
            string sop = " ? ";
            switch (operation)
            {
                case 0: sop = " равно "; break; // return string.Compare(fvalue, match, true) == 0;
                case 1: sop = " не равно "; break; // return string.Compare(fvalue, match, true) != 0;
                case 2: sop = " содержит "; break; // return fvalue.Contains(match);
                case 3: sop = " не содержит "; break; // return !fvalue.Contains(match);
                case 4: sop = " regex "; break; // return regex.IsMatch(fvalue);
            }

            return field_title + sop + match;
        }
    }

}
