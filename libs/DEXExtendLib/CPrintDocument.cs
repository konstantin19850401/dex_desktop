using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Drawing.Drawing2D;
using BarcodeLib;

namespace DEXExtendLib
{
    public class CPrintDocument
    {
        SimpleXML doc, scheme;
        PrintDocument pd;
        CstPrintPreviewDialog ppdlg;
        PrintDialog pdlg;
        Object toolbox;
        PrinterSettings settings;

        SimpleXML _node(SimpleXML s1, string name)
        {
            if (s1 != null && s1.GetNodeByPath(name, false) != null) return s1[name];
            return null;
        }

        string _txt(SimpleXML s1, string name, string def)
        {
            if (s1 != null && s1.GetNodeByPath(name, false) != null) return s1[name].Text;
            return def;
        }

        string _attr(SimpleXML s1, string name, string def)
        {
            if (s1 != null && s1.Attributes.ContainsKey(name)) return s1.Attributes[name];
            return def;
        }

        public CPrintDocument(SimpleXML doc, SimpleXML scheme, PrinterSettings settings, Object toolbox)
        {
            this.doc = doc;
            this.scheme = scheme;
            this.settings = settings;
            this.toolbox = toolbox;


            //добавление полей для описания параметров дилера
            if (doc != null)
            {
                Dictionary<string, string> dRegisterstest = new Dictionary<string, string>();
                IDEXData ddtest = (IDEXData)toolbox;
                using (DataTable dtrtest = ddtest.getQuery("select rname, rvalue from `registers`"))
                {
                    if (dtrtest != null && dtrtest.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtrtest.Rows)
                        {
                            dRegisterstest[row["rname"].ToString()] = row["rvalue"].ToString();
                        }
                    }
                }

                foreach (SimpleXML field in doc.Child)
                {
                    if (field.Name == "MainDealerFIO" || field.Name == "MainDealerName" || field.Name == "MainDealerPowAt" || field.Name == "MainDealerDatePowAt")
                    {
                        foreach (var s in dRegisterstest)
                        {
                            if (s.Key == field.Name) field.Text = s.Value;
                        }
                    }
                }
            }
            
            
            



            IDEXPrinter prn = (IDEXPrinter)toolbox;

            pd = new PrintDocument();

            if (settings != null) pd.PrinterSettings = settings;

            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            pd.DefaultPageSettings.Landscape = false;

            pd.DefaultPageSettings.Landscape = _txt(scheme, "Orientation", "landscape").Trim().ToLower().Equals("landscape");


            
            


            ppdlg = new CstPrintPreviewDialog();
            ppdlg.Owner = null;
            pdlg = new PrintDialog();
        }

        public void SetupPrinter()
        {
            pdlg.Document = pd;
            if (pdlg.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = pdlg.PrinterSettings;
            }

        }

        public void Preview()
        {
            IDEXConfig cfg = (IDEXConfig)toolbox;

            ppdlg.Document = pd;

            ppdlg.Width = Screen.PrimaryScreen.Bounds.Width;
            ppdlg.Height = Screen.PrimaryScreen.Bounds.Height;
            ppdlg.PrintPreviewControl.Zoom = 1.0;

            ppdlg.ShowDialog();
        }

        public void Print(bool showSetup)
        {
            if (showSetup) SetupPrinter();
            using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
            {
                try
                {
                    pd.Print();
                }
                finally
                {
                    wic.Undo();
                }
            }
        }

        private string _substrCheck(string source, char pred, string key, string value)
        {
            string ret = source.Replace("{" + pred + key + pred + "}", value);

            bool replaced;
            do
            {
                replaced = false;
                int spos = ret.IndexOf(@"{" + pred + key + "["); // {$ ...[..,..]$}
                if (spos > -1)
                {
                    int predlength = (@"{" + pred + key + "[").Length;
                    int epos = ret.IndexOf("]" + pred + "}", spos);
                    if (epos > -1)
                    {
                        string clr = ret.Substring(spos + predlength, epos - spos - predlength);
                        if (clr.Length > 0) // s, l
                        {
                            try
                            {
                                string rval = "";
                                spos = clr.IndexOf(',');
                                if (spos > -1)
                                {
                                    string clr1 = clr.Substring(0, spos);
                                    string clr2 = clr.Substring(spos + 1);
                                    rval = value.Substring(int.Parse(clr1.Trim()), int.Parse(clr2.Trim()));
                                }
                                else
                                {
                                    rval = value.Substring(int.Parse(clr.Trim()));
                                }

                                if (ret.IndexOf(@"{" + pred + key + @"[" + clr + @"]" + pred + "}") > -1)
                                {
                                    replaced = true;
                                    ret = ret.Replace(@"{" + pred + key + @"[" + clr + @"]" + pred + "}", rval);
                                }
                            }
                            catch (Exception) { }
                        }
                    }
                }
            } while (replaced);

            return ret;
        }

        private string _expr(SimpleXML doc, string docvalue, string srcvalue)
        {

            string ret = srcvalue.Replace(@"{$docvalue$}", docvalue);

            try
            {
                foreach (SimpleXML cnode in doc.Child)
                {
                    string nvalue = cnode.Text;
                    if (cnode.Attributes.ContainsKey("printable")) nvalue = cnode.Attributes["printable"];

                    ret = _substrCheck(ret, '$', cnode.Name, nvalue);

                    /* // Старый вариант, не обрабатывающий подстроки
                    string nname = @"{$" + cnode.Name + @"$}";
                    ret = ret.Replace(nname, nvalue);
                    */

                }
            }
            catch (Exception) { }

            return ret;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (scheme != null)
            {
                IDEXSysData sd = (IDEXSysData)toolbox;
                string schemes_dir = sd.DataDir + @"printing_schemes\";

                Dictionary<string, string> dRegisters = new Dictionary<string, string>();


                IDEXData dd = (IDEXData)toolbox;
                using (DataTable dtr = dd.getQuery("select rname, rvalue from `registers`"))
                {
                    if (dtr != null && dtr.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtr.Rows)
                        {
                            dRegisters[row["rname"].ToString()] = row["rvalue"].ToString();
                        }
                    }
                }

                

                Graphics g = e.Graphics;
                g.PageUnit = GraphicsUnit.Millimeter;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                float pofsx = pd.DefaultPageSettings.PrintableArea.X / 10 * 2.54f;
                float pofsy = pd.DefaultPageSettings.PrintableArea.Y / 10 * 2.54f;
                float pofsw = pd.DefaultPageSettings.PrintableArea.Width / 10 * 2.54f;
                float pofsh = pd.DefaultPageSettings.PrintableArea.Height / 10 * 2.54f;
                float fofsx = 0, fofsy = 0;

                SimpleXML frot = _node(scheme, "Rotate");
                if (frot != null && (frot.Text.Trim().Equals("yes") || frot.Text.Trim().Equals("1")))
                {

                    float paw = pd.DefaultPageSettings.Bounds.Width / 10 * 2.54f,
                          pah = pd.DefaultPageSettings.Bounds.Height / 10 * 2.54f;

                    g.ResetTransform();
                    g.TranslateTransform(paw / 2, pah / 2);
                    g.RotateTransform(180.0f);
                    g.TranslateTransform(-paw / 2, -pah / 2);
                }

                SimpleXML fofs = _node(scheme, "FieldsOffset");

                if (fofs != null)
                {
                    
                    try
                    {
                        fofsx = float.Parse(_attr(fofs, "x", "0"));
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        fofsy = float.Parse(_attr(fofs, "y", "0"));
                    }
                    catch (Exception)
                    {
                    }
                }

                
                

                // Шрифт
                Font prnFont = new Font("Courier New", 10);

                SimpleXML ffnt = _node(scheme, "Font");

                if (ffnt != null)
                {
                    string foname = _attr(ffnt, "name", "Courier New");
                    string fosize = _attr(ffnt, "size", "10");
                    try
                    {
                        prnFont = new Font(foname, float.Parse(fosize));
                    
                    }
                    catch (Exception)
                    {
                    }
                }

                // Разберемся с подложкой

                bool usecover = false;

                SimpleXML uc = _node(scheme, "UseCover");

                if (uc != null)
                {
                    string cval = uc.Text.Trim().ToLower();
                    usecover = (cval.Equals("1") || cval.Equals("yes") || cval.Equals("true"));
                }


                if (usecover)
                {
                    SimpleXML scvr = _node(scheme, "Cover");
                    if (scvr != null)
                    {
                        Image cvr = Image.FromFile(schemes_dir + scvr.Text);

                        float cvrox = pofsx, cvroy = pofsy,
                            cvrow = cvr.Width / cvr.HorizontalResolution /* / g.DpiX*/,
                            cvroh = cvr.Height / cvr.VerticalResolution /*/ g.DpiY*/;

                        SimpleXML cofs = _node(scheme, "CoverOffset");

                        if (cofs != null)
                        {
                            try
                            {
                                cvrox += (float)(float.Parse(cofs.Attributes["x"].ToString()));
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                cvroy += (float)(float.Parse(cofs.Attributes["y"].ToString()));
                            }
                            catch (Exception)
                            {
                            }
                        }

                        string v = _txt(scheme, "CoverScale", "100").Trim().ToLower();
                        if (v.Equals("auto"))
                        {
                            
                            float sx = pofsw / cvr.Width,
                                   sy = pofsh / cvr.Height;

                            if (sx < sy) sy = sx;
                            else sx = sy;

                            cvrow = (float)Math.Round(cvr.Width * sx);
                            cvroh = (float)Math.Round(cvr.Height * sy);
                        }
                        else
                        {
                            try
                            {
                                float i = (float)(float.Parse(v));
                                cvrow = cvrow * 10 * i / 100;
                                cvroh = cvroh * 10 * i / 100;
                            }
                            catch (Exception)
                            {
                            }
                        }

                        g.DrawImage(cvr, new RectangleF(cvrox, cvroy, cvrow, cvroh));


                        
                        // штрих-код для Билайн
                        
                        if (doc != null)
                        {
                            if (doc.Attributes["ID"].Equals("DEXPlugin.Document.Beeline.DOL2.Contract"))
                            {
                                Barcode b = new Barcode();
                                b.IncludeLabel = true;
                                string ICC = "";
                                foreach (SimpleXML field in doc.Child)
                                {
                                    if (field.Name == "ICC")
                                    {
                                        ICC = field.Text; 
                                        break;
                                    }
                                }
                                //string rrrr = doc.Child.GetChildren("ICC").ToString();// GetNodeByPath("ICC", false);
                                const double milimetresPerInch = 25.4; // as one inch is 25.4 mm
                                Image r = b.Encode(TYPE.CODE128, ICC, Color.Black, Color.White, 270, 50);
                                double lengthInMilimeter = r.Width / 10 * milimetresPerInch;
                                double heightInMilimeter = r.Height / 10 * milimetresPerInch;
                                Rectangle rectCropArea;
                                //rectCropArea = new Rectangle(50, 3, Convert.ToInt32(klengthInMilimeter), Convert.ToInt32(heightInMilimeter));
                                //g.DrawImage(r, new Rectangle(Convert.ToInt32(cvrow) - 80, 20, Convert.ToInt32(lengthInMilimeter), Convert.ToInt32(heightInMilimeter)), rectCropArea, GraphicsUnit.Millimeter);
                                rectCropArea = new Rectangle(0, 0, Convert.ToInt32(lengthInMilimeter), Convert.ToInt32(heightInMilimeter));
                                g.DrawImage(r, new Rectangle(Convert.ToInt32(cvrow) - 78, 1, Convert.ToInt32(lengthInMilimeter)- 85, Convert.ToInt32(heightInMilimeter) + 10), rectCropArea, GraphicsUnit.Millimeter);
                            }
                            else if (doc.Attributes["ID"].Equals("DEXPlugin.Document.MTS.Jeans")) 
                            {
                                Barcode b = new Barcode();
                                b.IncludeLabel = true;
                                string ICC = "", MSISDN = "", ICCCTL = "";
                                foreach (SimpleXML field in doc.Child)
                                {
                                    if (field.Name == "ICC")
                                    {
                                        ICC = field.Text;
                                    }
                                    else if (field.Name == "MSISDN") 
                                    {
                                        MSISDN = field.Text;
                                    }
                                    else if (field.Name == "ICCCTL") 
                                    {
                                        ICCCTL = field.Text;
                                    }
                                }
                                string result = string.Format("{0}{1}{2}", ICC, ICCCTL, MSISDN);// ICC + ICCCTL + MSISDN;
                                //string result = "897010112345678901239101234567";
                                const double milimetresPerInch = 25.4; // as one inch is 25.4 mm
                                b.Alignment = AlignmentPositions.LEFT;
                                //b.BackColor = Color.Black;
                                b.IncludeLabel = false;
                                Image r = b.Encode(TYPE.CODE128, result, Color.Black, Color.White, 400, 40);
                               
                                double lengthInMilimeter = r.Width *0.05* milimetresPerInch;
                                double heightInMilimeter = r.Height *0.04* milimetresPerInch;
                                Rectangle rectCropArea;
                                rectCropArea = new Rectangle(0, 0, Convert.ToInt32(lengthInMilimeter) + 200, Convert.ToInt32(heightInMilimeter));
                                //g.DrawImage(r, new Rectangle(Convert.ToInt32(cvrow) - 78, 1, Convert.ToInt32(lengthInMilimeter) - 85, Convert.ToInt32(heightInMilimeter) + 10), rectCropArea, GraphicsUnit.Millimeter);
                                g.DrawImage(r, new Rectangle(Convert.ToInt32(cvrow) -130 , 23, Convert.ToInt32(lengthInMilimeter) -100, Convert.ToInt32(heightInMilimeter) - 10), rectCropArea, GraphicsUnit.Millimeter);

                            }
                        }
                        
                        
                    }
                }

                


                float fscalex = 100, fscaley = 100;
                SimpleXML fsc = _node(scheme, "FieldsScale");

                if (fsc != null)
                {
                    try
                    {
                        fscalex = float.Parse(fsc.Attributes["x"]);
                    }
                    catch (Exception)
                    {
                        fscalex = 100;
                    }

                    try
                    {
                        fscaley = float.Parse(fsc.Attributes["y"]);
                    }
                    catch (Exception)
                    {
                        fscaley = 100;
                    }
                }
                
                // Вывод полей на экран



                SimpleXML nodeFields = _node(scheme, "Fields");

                if (nodeFields != null)
                {
                    Dictionary<string, string> fDefaults = new Dictionary<string, string>();
                    foreach (SimpleXML field in nodeFields.Child)
                    {
                        if (field.Attributes.ContainsKey("default")) fDefaults[field.Name] = field.Attributes["default"].ToString();
                    }

                    foreach (SimpleXML field in nodeFields.Child)
                    {
                        string fvalue = field.Name;
                        

                        float fx = -1, fy = -1, fw = -1, fh = -1;
                        int maxLength = -1;
                        try
                        {
                            fx = (float)(float.Parse(field.Attributes["x"].ToString()) - pofsx);
                        }
                        catch (Exception)
                        {
                        }

                        if (fscalex != 100) fx = fx * fscalex / 100;

                        try
                        {
                            fy = (float)(float.Parse(field.Attributes["y"].ToString()) - pofsy);
                        }
                        catch (Exception)
                        {
                        }

                        if (fscaley != 100) fy = fy * fscaley / 100;

                        try
                        {
                            fw = (float)(float.Parse(field.Attributes["w"].ToString()));
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            fh = (float)(float.Parse(field.Attributes["h"].ToString()));
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            maxLength = int.Parse(field.Attributes["maxlength"].ToString());
                            if (maxLength < 1) maxLength = -1;
                        }
                        catch (Exception)
                        {
                            maxLength = -1;
                        }

                        bool fenabled = false;
                        try
                        {
                            string sen = field.Attributes["enabled"].ToString().Trim().ToLower();
                            fenabled = (sen.Equals("1") || sen.Equals("true") || sen.Equals("yes"));
                        }
                        catch (Exception)
                        {
                        }

                        if (doc != null/* && doc.GetNodeByPath(field.Name, false) != null*/)
                        {
                            fvalue = null;
                            string fDefValue = null;
                            if (field.Attributes.ContainsKey("default")) fDefValue = field.Attributes["default"].ToString();

                            bool nullDocValue = doc.GetNodeByPath(field.Name, false) == null;

                            string docvalue = doc.GetNodeByPath(field.Name, true).Text.Trim();
                            string ftype = "text";
                            if (field.Attributes.ContainsKey("type")) ftype = field.Attributes["type"].ToString();
                            ftype = ftype.Trim().ToLower();

                            //Types:

                            //enum
                            if (ftype.Equals("enum"))
                            {
                                ArrayList enodes = field.GetChildren("Value");
                                foreach (SimpleXML enumvalue in enodes)
                                {
                                    if (enumvalue.Attributes.ContainsKey("id") && enumvalue.Attributes["id"].ToString().Equals(docvalue))
                                    {
                                        fvalue = enumvalue.Text.Trim();
                                        break;
                                    }
                                }
                            }
                            //regexp
                            else if (ftype.Equals("regexp") &&
                                     field.GetNodeByPath("Pattern", false) != null &&
                                    field.GetNodeByPath("Replacement", false) != null)
                            {
                                Regex rx = new Regex(field.GetNodeByPath("Pattern", false).Text.Trim());
                                fvalue = rx.Replace(docvalue, field.GetNodeByPath("Replacement", false).Text.Trim());
                            }
                            // multitext
                            else if (ftype.Equals("multitext") && field.GetNodeByPath("Expression", false) != null)
                            {
                                string fexpr = field["Expression"].Text;
                                fvalue = field["Expression"].Text;
                                foreach (SimpleXML docnode in doc.Child)
                                {
                                    fvalue = _substrCheck(fvalue, '$', docnode.Name, docnode.Text);
                                }

                                foreach (KeyValuePair<string, string> kvp in fDefaults)
                                {
                                    fvalue = _substrCheck(fvalue, '$', kvp.Key, kvp.Value);
                                }
                            }
                            //db
                            else if (ftype.Equals("db"))
                            {
                                if (field.GetNodeByPath("Table", false) != null)
                                {
                                    Dictionary<string, string> fdic = field.GetNodeByPath("Table", false).Attributes;
                                    if (fdic != null && fdic.ContainsKey("name") &&
                                        fdic.ContainsKey("index") && fdic.ContainsKey("value"))
                                    {
                                        IDEXData d = (IDEXData)toolbox;
                                        try
                                        {
                                            DataTable t = d.getQuery(string.Format(
                                                "select {0} from `{1}` where {2} = \"{3}\"",
                                                fdic["value"].ToString(), fdic["name"].ToString(),
                                                fdic["index"].ToString(), d.EscapeString(docvalue)));

                                            if (t != null && t.Rows.Count > 0)
                                            {
                                                //fvalue = t.Rows[0][fdic["value"].ToString()].ToString();
                                                if ( field.GetNodeByPath("Condition", false) == null )
                                                {
                                                    fvalue = t.Rows[0][fdic["value"].ToString()].ToString();
                                                }
    
                                                if ( field.GetNodeByPath("Condition", false) != null )
                                                {



                                                    ArrayList fcond = field.GetChildren("Condition");

                                                    bool isCondition = false;
                                                    bool notCondition = false;

                                                    bool atLeastOneIs = false;

                                                    foreach ( SimpleXML fcondv in fcond )
                                                    {
                                                        bool part = fcondv.Attributes.ContainsKey("part");

                                                        // Обработка IS
                                                        if ( fcondv.Attributes.ContainsKey("is") )
                                                        {
                                                            if ( fcondv.Attributes["is"].ToString().Equals(docvalue) ||
                                                                ( part && docvalue.IndexOf(fcondv.Attributes["is"].ToString()) > -1 ) )
                                                            {
                                                                isCondition = true;
                                                            }
                                                            atLeastOneIs = true;
                                                        }

                                                        // Обработка NOT
                                                        if ( fcondv.Attributes.ContainsKey("not") &&
                                                            ( fcondv.Attributes["not"].ToString().Equals(docvalue) ||
                                                            ( part && docvalue.IndexOf(fcondv.Attributes["not"].ToString()) > -1 )
                                                            ) )
                                                        {
                                                            notCondition = true;
                                                        }
                                                    }
                                                    if ( !atLeastOneIs )
                                                        isCondition = true;

                                                    if ( isCondition && !notCondition )
                                                    {
                                                        //fvalue = _expr(doc, docvalue, field.Attributes["checkvalue"].ToString());
                                                        fvalue = t.Rows[0][fdic["value"].ToString()].ToString();
                                                        fenabled = true;
                                                    }
                                                    else
                                                    {
                                                        fvalue = "";
                                                    }


                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            fvalue += " ERR";
                                        }
                                    }
                                }
                            }
                            // check
                            else if ( ftype.Equals("check") && field.Attributes.ContainsKey("checkvalue") )
                            {
                                fenabled = false;
                                ArrayList fcond = field.GetChildren("Condition");

                                bool iscondition = false;
                                bool notcondition = false;

                                bool atLeastOneIs = false;

                                bool parseAttr = false;
                                if (field.Attributes.ContainsKey("parseAttr"))
                                {
                                    parseAttr = true;
                                }

                                foreach (SimpleXML fcondv in fcond)
                                {
                                    bool part = fcondv.Attributes.ContainsKey("part");

                                   
                                    if (parseAttr)
                                    {
                                        // этот блок добавлен для обработки атрибутов, так как ее не было. 
                                        // Обработка IS
                                        ArrayList node = doc.GetChildren(field.Attributes["node"]);
                                        string attr = ((SimpleXML)node[0]).Attributes[field.Attributes["attr"]];
                                        if (fcondv.Attributes.ContainsKey("is"))
                                        {
                                            if (fcondv.Attributes["is"].ToString().Equals(attr))
                                            {
                                                iscondition = true;
                                            }
                                            atLeastOneIs = true;
                                        }

                                        // Обработка NOT
                                        if (fcondv.Attributes.ContainsKey("not") &&
                                            (fcondv.Attributes["not"].ToString().Equals(attr))
                                            )
                                        {
                                            notcondition = true;
                                        }
                                    } 
                                    else
                                    {
                                        // Обработка IS
                                        if (fcondv.Attributes.ContainsKey("is"))
                                        {
                                            if (fcondv.Attributes["is"].ToString().Equals(docvalue) ||
                                                (part && docvalue.IndexOf(fcondv.Attributes["is"].ToString()) > -1))
                                            {
                                                iscondition = true;
                                            }
                                            atLeastOneIs = true;
                                        }

                                        // Обработка NOT
                                        if (fcondv.Attributes.ContainsKey("not") &&
                                            (fcondv.Attributes["not"].ToString().Equals(docvalue) ||
                                            (part && docvalue.IndexOf(fcondv.Attributes["not"].ToString()) > -1)
                                            ))
                                        {
                                            notcondition = true;
                                        }
                                    }
                                }

                                if (!atLeastOneIs) iscondition = true;

                                if (iscondition && !notcondition)
                                {
                                    fvalue = _expr(doc, docvalue, field.Attributes["checkvalue"].ToString());
                                    fenabled = true;
                                }
                            }
                            //text (default)
                            if (fvalue == null)
                            {
                                fvalue = docvalue;
                                if ((nullDocValue || fvalue.Equals("")) && fDefValue != null) fvalue = fDefValue;
                            }

                            if (fvalue != null && fvalue.Length > 0)
                            {
                                foreach (KeyValuePair<string, string> kvp in dRegisters)
                                {
                                    fvalue = _substrCheck(fvalue, '#', kvp.Key, kvp.Value);
//                                    fvalue = fvalue.Replace(@"{#" + kvp.Key + @"#}", kvp.Value);
                                }
                            }

                            if (fvalue != null && maxLength > 0 && fvalue.Length > maxLength)
                            {
                                fvalue = fvalue.Substring(0, maxLength);
                            }

                        }

                        if (fw < 0) fw = g.MeasureString(fvalue, prnFont).Width;
                        if (fh < 0) fh = prnFont.GetHeight(g);

                        fx += fofsx;
                        fy += fofsy;

                        if (fenabled)
                        {
                            SolidBrush prnBrush = new SolidBrush(Color.Black);
                            RectangleF prnRect = new RectangleF(fx, fy, fw, fh);

                            try
                            {
                                float intv = 0;
                                if (field.Attributes.ContainsKey("interval"))
                                {
                                    try
                                    {
                                        intv = float.Parse(field.Attributes["interval"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                if (intv == 0)
                                {
                                    g.DrawString(fvalue, prnFont, prnBrush, prnRect);
                                    if (doc == null)
                                    {
                                        Pen pn = new Pen(Color.FromArgb(255, 0, 0, 255), 0.2f);
                                        g.DrawRectangle(pn, prnRect.X, prnRect.Y, prnRect.Width, prnRect.Height);
                                    }
                                }
                                else
                                {
                                    for (int f = 0; f < fvalue.Length; ++f)
                                    {
                                        g.DrawString(fvalue.Substring(f, 1), prnFont, prnBrush, prnRect);

                                        if (doc == null)
                                        {
                                            Pen pn = new Pen(Color.FromArgb(255, 0, 0, 255), 0.2f);
                                            g.DrawRectangle(pn, prnRect.X, prnRect.Y, intv, prnRect.Height);
                                        }

                                        prnRect.X += intv;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получение схем печати указанного типа из указанного каталога
        /// </summary>
        /// <param name="schemesdir">Каталог с файлами схем печати</param>
        /// <param name="id">Идентификатор документа или ALL, если нужно получить все имеющиеся схемы</param>
        /// <returns>Массив описателей схем печати</returns>
        public static SimpleXML[] GetSchemesForId(string schemesdir, string id)
        {
            SimpleXML[] ret = null;
            try
            {
                List<SimpleXML> schemes = new List<SimpleXML>();

                string[] files = Directory.GetFiles(schemesdir, "*.scheme", SearchOption.TopDirectoryOnly);
                foreach (string s in files)
                {
                    SimpleXML scheme = SimpleXML.LoadXml(System.IO.File.ReadAllText(s));
                    if (scheme.GetNodeByPath("ID", false) != null && (scheme["ID"].Text.Equals(id) || id.Equals("ALL")))
                    {
                        schemes.Add(scheme);
                    }
                }
                ret = schemes.ToArray();
            }
            catch (Exception)
            {
            }
            
            return ret;
        }
    }
}
