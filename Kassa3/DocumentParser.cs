using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Kassa3
{
    class DocumentParser
    {
        const int ST_BEFORE_HEADER = 0, ST_HEADER = 1, ST_LIST = 2, ST_OPERATION = 3;
        // ST_BEFORE_HEADER > ST_HEADER > [ ST_OPERATION > ST_LIST ]

        public static List<Dictionary<string, string>> parse(string[] src, string protocol)
        {
            if ("ibank2".Equals(protocol)) return parseIbank(src);

            return null;
        }

        static List<Dictionary<string, string>> parseIbank(string[] src0)
        {
            try
            {
                // Подобрать строки с переносами
                string[] src = new string[src0.Length];
                Array.Copy(src0, src, src0.Length);

                for (int i = src.Length - 1; i > -1; i--)
                {
                    if (!src[i].StartsWith("$") && src[i].IndexOf('=') < 0 && i > 0)
                    {
                        src[i - 1] += src[i];
                        src[i] = null;
                    }
                }

                List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();

                Dictionary<string, string> hdr = new Dictionary<string, string>() { { "$$BLOCK", "header" } };

                ret.Add(hdr);

                // $OPERS_LIST
                // ... header
                // $OPERATION
                // ... body
                // $OPERATION_END
                // $OPERATION
                // ... body
                // $OPERATION_END
                // $OPERS_LIST_END

                int state = ST_BEFORE_HEADER;

                Dictionary<string, string> body = null;

                foreach (string str in src)
                {
                    if (str != null)
                    {
                        if (state == ST_BEFORE_HEADER)
                        {
                            if ("$OPERS_LIST".Equals(str)) state = ST_HEADER;
                        }
                        else if (state == ST_HEADER || state == ST_LIST)
                        {
                            if ("$OPERATION".Equals(str))
                            {
                                body = new Dictionary<string, string>() { { "$$BLOCK", "body" } };
                                ret.Add(body);
                                state = ST_OPERATION;
                            }
                            else if ("$OPERS_LIST_END".Equals(str))
                            {
                                break;
                            }
                            else
                            {
                                if (state == ST_HEADER)
                                {
                                    KeyValuePair<string, string> kvp;
                                    if (parsePair(str, out kvp)) hdr.Add(kvp.Key, kvp.Value);
                                }
                            }
                        }
                        else if (state == ST_OPERATION)
                        {
                            if ("$OPERATION_END".Equals(str))
                            {
                                state = ST_LIST;
                            }
                            else
                            {
                                KeyValuePair<string, string> kvp;
                                if (parsePair(str, out kvp)) body.Add(kvp.Key, kvp.Value);
                            }
                        }
                    }
                }

                return ret;
            }
            catch (Exception) { }

            return null;
            
        }

        public static bool parsePair(string pair, out KeyValuePair<string, string> outPair)
        {
        
            try
            {
                int pos = pair.IndexOf('=');
                if (pos > -1) {
                    outPair = new KeyValuePair<string, string>(pair.Substring(0, pos), pair.Substring(pos + 1));
                    return true;
                }
            }
            catch (Exception) { }

            outPair = new KeyValuePair<string, string>();

            return false;
        }

        public static string getDocumentId(Dictionary<string, string> src, string protocol)
        {
            try
            {
                if ("ibank2".Equals(protocol) && src.ContainsKey("OPER_ID")) return src["OPER_ID"];
            }
            catch (Exception) { }

            return "?";
        }

        public static string getR_Date(Dictionary<string, string> src, string protocol)
        {
            try
            {
                if ("ibank2".Equals(protocol) && src.ContainsKey("OPER_DATE"))
                {
                    //OPER_DATE=02.12.2013
                    string dd = src["OPER_DATE"];
                    return dd.Substring(6, 4) + dd.Substring(3, 2) + dd.Substring(0, 2);
                }
            }
            catch (Exception) { }

            return "?";
        }

        public static double getR_Sum(Dictionary<string, string> src, string protocol)
        {
            try
            {
                if ("ibank2".Equals(protocol) && src.ContainsKey("AMOUNT"))
                {
                    return Math.Abs(double.Parse(src["AMOUNT"], CultureInfo.InvariantCulture));
                }
            }
            catch (Exception) { }

            return 0;
        }
    }
}
