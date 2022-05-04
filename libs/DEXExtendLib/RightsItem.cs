using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public class RightsItem
    {
        public string ID;
        public string Title;
        public bool CanAccess;

        public RightsItem(string AID, string ATitle, bool ACanAccess)
        {
            ID = AID;
            Title = ATitle;
            CanAccess = ACanAccess;
        }
    }
}
