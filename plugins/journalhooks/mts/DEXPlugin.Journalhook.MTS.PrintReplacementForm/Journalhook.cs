using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using DEXExtendLib;
using System.Drawing;
using System.Data;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DEXPlugin.Journalhook.MTS.PrintReplacementForm
{
    public class Journalhook : IDEXPluginJournalhook
    {
        public string ID
        {
            get
            {
                return "DEXPlugin.Journalhook.MTS.PrintReplacementForm";
            }
        }
        public string Title
        {
            get
            {
                return "Печать формы для замены SIM";
            }
        }

        public string[] Path
        {
            get
            {
                return null;
            }
        }

        public int Version
        {
            get
            {
                return 1;
            }
        }

        public string Description
        {
            get
            {
                return "Печатает форму по замене SIM";
            }
        }

        public Bitmap getBitmap()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.IO.Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".icon.bmp");
                Bitmap b = new Bitmap(s);
                return b;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public void setJournalMode(DEXJournalType journalType)
        {
            this.journalType = journalType;
        }

        DEXJournalType journalType = DEXJournalType.JOURNAL;


        bool hookVisible = false;
        object toolbox;

        public void InitReflist(object toolbox)
        {
            hookVisible = false;
            this.toolbox = toolbox;
        }

        public void AddReferenceVisibility(object toolbox, string DocType, int DocStatus)
        {
            hookVisible = DocType.StartsWith("DEXPlugin.Document.") &&
                journalType == DEXJournalType.JOURNAL;
        }

        public Dictionary<string, string> getVisibleFunctionsList(object toolbox)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (hookVisible) ret["printreplacementform"] = "Печать формы для замены SIM";
            return ret;
        }

        public Dictionary<string, string> getVisibleSubFunctionsList(string FunctionName)
        {
            return null;
        }

        public bool RunFunctionForDocument(string FunctionName, string SubFunctionName, string docId, IDEXDocumentData doc, out bool docChanged)
        {
            docChanged = false;
            try
            {
                if (FunctionName.Equals("printreplacementform") &&
                    docId.StartsWith("DEXPlugin.Document.") &&
                    journalType == DEXJournalType.JOURNAL)
                {
                    IDEXData d = (IDEXData)toolbox;


                    string existingFile = ((IDEXSysData)toolbox).AppDir + "\\mtsReplacementForm.pdf";
                    string newFile = ((IDEXSysData)toolbox).AppDir + "\\mtsOutReplacementForm.pdf";
                    SimpleXML xml = SimpleXML.LoadXml(doc.documentText);
                    /*
                    using (FileStream fs = new FileStream(existingFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Document docpdf = new Document(PageSize.LETTER))
                        {
                            using (PdfWriter writer = PdfWriter.GetInstance(docpdf, fs))
                            {
                                docpdf.Open();

                                docpdf.Add(new Paragraph("This is a test"));

                                docpdf.Close();
                            }
                        }
                    }
                    */

                    //Bind a PdfReader to our first document
                    PdfReader reader = new PdfReader(existingFile);
                    //Create a new stream for our output file (this could be a MemoryStream, too)
                    using (FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        //Use a PdfStamper to bind our source file with our output file
                        using (PdfStamper stamper = new PdfStamper(reader, fs))
                        {
                            BaseFont baseFont = BaseFont.CreateFont(((IDEXSysData)toolbox).AppDir + "\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                            //In case of conflict we want our new text to be written "on top" of any existing content
                            //Get the "Over" state for page 1

                            PdfContentByte cb = stamper.GetOverContent(1);


                            //Begin text command
                            //cb.BeginText();

                            //Set the font information
                            //cb.SetFontAndSize(BaseFont.CreateFont(baseFont, BaseFont.CP1250, false), 16f);
                            //Position the cursor for drawing
                            //cb.MoveText(300, 500); // первый - горизонталь
                            //Write some text
                            //string ss = xml["LastName"].Text.ToString() + " " + xml["FirstName"].Text.ToString() + " " + xml["SecondName"].Text.ToString();
                            //cb.ShowText(ss);
                            //End text command
                            //cb.EndText();

                            //Create a new ColumnText object to write to
                            ColumnText from = new ColumnText(cb);
                            from.SetSimpleColumn(285, 795, 500, 200);
                            from.AddElement(new Paragraph(String.Format("{0} {1} {2}", xml["LastName"].Text, xml["FirstName"].Text, xml["SecondName"].Text), font));
                            from.Go();

                            char[] msdch = String.Format("{0}", xml["MSISDN"].Text).ToCharArray();
                            string msd = "";
                            foreach (char ch in msdch) msd = string.Format(msd + "{0}    ", ch);
                            ColumnText msisdn = new ColumnText(cb);
                            msisdn.SetSimpleColumn(363, 743, 600, 400);
                            msisdn.AddElement(new Paragraph(msd, font));
                            msisdn.Go();

                            string sn = String.Format("{0} {1}", xml["FizDocSeries"].Text.Replace(" ", ""), xml["FizDocNumber"].Text);
                            ColumnText fizDocSN = new ColumnText(cb);
                            fizDocSN.SetSimpleColumn(384, 724, 500, 200);
                            fizDocSN.AddElement(new Paragraph(sn, font));
                            fizDocSN.Go();

                            ColumnText birth = new ColumnText(cb);
                            birth.SetSimpleColumn(333, 702, 500, 200);
                            birth.AddElement(new Paragraph(String.Format("{0}", xml["Birth"].Text), font));
                            birth.Go();

                            ColumnText addr = new ColumnText(cb);
                            addr.SetSimpleColumn(322, 683, 600, 400);
                            addr.AddElement(new Paragraph(String.Format("{0}, {1}, {2}", xml["AddrCity"].Text, xml["AddrStreet"].Text, xml["AddrHouse"].Text), font));
                            addr.Go();

                            string prod = "Алиева А.А.";
                            ColumnText prod1 = new ColumnText(cb);
                            prod1.SetSimpleColumn(100, 163, 300, 100);
                            prod1.AddElement(new Paragraph(String.Format("{0}", prod), font));
                            prod1.Go();

                            ColumnText prod2 = new ColumnText(cb);
                            prod2.SetSimpleColumn(163, 101, 300, 50);
                            prod2.AddElement(new Paragraph(String.Format("{0}", prod), font));
                            prod2.Go();


                            //AssignedDPCode
                            DataTable t = d.getQuery("select * from `registers`");
                            if (t != null && t.Rows.Count > 0)
                            {
                                string pref = "";
                                string dealer = "";
                                foreach (DataRow dr in t.Rows)
                                {
                                    if (dr["rname"].ToString() == "mts_dpcode_prefix") pref = dr["rvalue"].ToString();
                                    if (dr["rname"].ToString() == "dealer_code") dealer = dr["rvalue"].ToString();
                                }

                                char[] codech = String.Format("{0}{1}", pref, xml["AssignedDPCode"].Text).ToCharArray();
                                string code = "";
                                foreach (char ch in codech) code = string.Format(code + "{0}    ", ch);
                                ColumnText adc = new ColumnText(cb);
                                adc.SetSimpleColumn(154, 125, 400, 80);
                                adc.AddElement(new Paragraph(String.Format("{0}", code), font));
                                adc.Go();

                                ColumnText dealerct = new ColumnText(cb);
                                dealerct.SetSimpleColumn(175, 76, 500, 50);
                                dealerct.AddElement(new Paragraph(String.Format("{0}", dealer), font));
                                dealerct.Go();
                            }

                            string filePath = ((IDEXSysData)toolbox).AppDir + "\\mtsOutReplacementForm.pdf";
                            System.Diagnostics.Process.Start(filePath);
                        }
                    }

                    docChanged = false;
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
