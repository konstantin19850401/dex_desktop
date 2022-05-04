using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXDocumentJournal
    {
        void AddRecord(string text);
        void AddRecord(string text, Array subdata);
        void AddRecord(string text, ArrayList subdata);
        void AddRecord(SimpleXML journal, string text);
        void AddRecord(SimpleXML journal, string text, Array subdata);
    }
}
