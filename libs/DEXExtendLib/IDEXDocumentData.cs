using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXDocumentData
    {
        string signature { get; set; }
        string documentDate { get; set; }
        string documentText { get; set; }
        string documentDigest { get; set; }
        int documentUnitID { get; set; }
        int documentStatus { get; set; }
        string documentIdJournal { get; set; }
    }
}
