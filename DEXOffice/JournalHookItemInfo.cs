using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEXExtendLib;

namespace DEXOffice
{
    public class JournalHookItemInfo
    {
        public IDEXPluginJournalhook Module;
        public string FunctionName, SubFunctionName;

        public JournalHookItemInfo(IDEXPluginJournalhook aModule, string aFunctionName, string aSubFunctionName)
        {
            Module = aModule;
            FunctionName = aFunctionName;
            SubFunctionName = aSubFunctionName;
        }
    }
}
