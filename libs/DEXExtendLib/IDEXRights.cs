using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXRights
    {
        bool IsSuperUser();
        void AddRightsItem(string AKey, string ATitle);
        void SetRightsItem(string AKey, bool AValue);
        bool GetRightsItem(string AKey);
        ArrayList GetRightsTable();
    }
}
