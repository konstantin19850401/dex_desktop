using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DEXExtendLib
{
    public class CSVParser
    {
        public static ArrayList ParseLine(string source, string separator, bool quotes)
        {
            try
            {
                ArrayList ret = new ArrayList();

                bool inQuotes = false, dblQuote = false;

                string ou = "";

                if (!quotes)
                {
                    ret.AddRange(source.Split(new string[] { separator }, StringSplitOptions.None));
                }
                else
                {
                    for (int f = 0; f < source.Length; ++f)
                    {
                        if (inQuotes)
                        { // В кавычкаж
                            if (source[f] == "\""[0])
                            {
                                if (dblQuote) ou += source[f];
                                dblQuote = !dblQuote;
                            }
                            else if (source[f] == separator[0])
                            {
                                if (dblQuote)
                                {
                                    dblQuote = false;
                                    inQuotes = false;
                                    ret.Add(ou);
                                    ou = "";
                                }
                                else
                                {
                                    ou += source[f];
                                }
                            }
                            else
                            {
                                ou += source[f];
                            }
                        }
                        else
                        { // За кавычками
                            if (source[f] == "\""[0])
                            {
                                if (ou.Trim().Equals(""))
                                {
                                    inQuotes = true;
                                    dblQuote = false;
                                    ou = "";
                                }
                                else
                                {
                                    ou += source[f];
                                }
                            }
                            else if (source[f] == separator[0])
                            {
                                ret.Add(ou);
                                ou = "";
                            }
                            else
                            {
                                ou += source[f];
                            }
                        }
                    }
                }

                if (ou != "") ret.Add(ou);

                return ret;
            }
            catch (Exception)
            {
            }
            return null;
        }
        
        public static DataTable stringToTable(string source, string separator, bool quotes, bool trimEmpty)
        {
            try
            {
                string[] src = source.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                int fldcount = 0;
                ArrayList lines = new ArrayList();
                foreach (string s in src)
                {
                    if (!trimEmpty || s.Trim().Length > 0)
                    {
                        ArrayList line = ParseLine(s, separator, quotes);
                        if (line != null && line.Count > fldcount) fldcount = line.Count;
                        lines.Add(line);
                    }
                }

                if (lines.Count > 0 && fldcount > 0)
                {
                    DataTable t = new DataTable();

                    for (int f = 0; f < fldcount; ++f)
                    {
                        t.Columns.Add("field" + f.ToString(), typeof(string));
                    }

                    foreach (ArrayList ln in lines)
                    {
                        DataRow r = t.NewRow();
                        if (ln != null && ln.Count > 0)
                        {
                            for (int f = 0; f < ln.Count; ++f)
                            {
                                if (ln[f] != null && ln[f] is string)
                                {
                                    string sss = (string)ln[f];
                                    r["field" + f.ToString()] = sss;
                                }
                            }
                        }

                        t.Rows.Add(r);
                    }

                    return t;
                }
            }
            catch (Exception)
            {
            }
            return null;
        
        }

        public static string _q(string src, bool quotes)
        {
            if (!quotes) return src;
            return "\"" + src.Replace("\"", "\"\"") + "\"";
        }

        public static string tableToString(DataTable source, string separator, bool quotes)
        {
            if (source == null || source.Columns.Count < 1 || source.Rows.Count < 1) return null;
            try
            {
                string ret = "";
                foreach (DataRow r in source.Rows)
                {
                    string retstr = "";
                    for (int f = 0; f < source.Columns.Count; ++f)
                    {
                        retstr += (f > 0 ? separator : "") + _q(r[f].ToString(), quotes);
                    }
                    ret += retstr + "\n";
                }
                return ret;
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Недостаточно памяти для выполнения операции");
            }

            return null;
        }

        public static void tableToFile(DataTable source, string separator, bool quotes, string filename, bool genHeaders)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1251));
                if (genHeaders)
                {
                    string hdrs = "";
                    for (int f = 0; f < source.Columns.Count; ++f)
                    {
                        hdrs += (hdrs.Equals("") ? "" : separator) + _q(source.Columns[f].Caption, quotes);
                    }
                    if (!hdrs.Equals("")) sw.WriteLine(hdrs);
                }

                foreach (DataRow r in source.Rows)
                {
                    string retstr = "";
                    for (int f = 0; f < source.Columns.Count; ++f)
                    {
                        retstr += (f > 0 ? separator : "") + _q(r[f].ToString(), quotes);
                    }
                    sw.WriteLine(retstr);
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception) { }
        }
    }
}
