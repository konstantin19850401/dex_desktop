using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public class CDEXDocumentData : IDEXDocumentData
    {
        string fsignature;
        string fdocumentdate;
        string fdocumenttext;
        string fdocumentdigest;
        int funitid;
        int fstatus;
        string fdocidjor;


        public string signature
        {
            get
            {
                return fsignature;
            }

            set
            {
                fsignature = value;
            }
        }

        public string documentDate
        {
            get
            {
                return fdocumentdate;
            }

            set
            {
                fdocumentdate = value;
            }
        }

        public string documentText
        {
            get
            {
                return fdocumenttext;
            }

            set
            {
                fdocumenttext = value;
            }
        }

        public string documentDigest
        {
            get
            {
                return fdocumentdigest;
            }

            set
            {
                fdocumentdigest = value;
            }
        }

        public int documentUnitID
        {
            get
            {
                return funitid;
            }
            set
            {
                funitid = value;
            }
        }

        public int documentStatus
        {
            get
            {
                return fstatus;
            }
            set
            {
                fstatus = value;
            }
        }

        public string documentIdJournal 
        {
            get 
            {
                return fdocidjor;
            }
            set 
            {
                fdocidjor = value;
            }
        }

        public CDEXDocumentData()
        {
            documentStatus = -1;
            documentText = "";
            documentUnitID = -1;
        }
    }
}
