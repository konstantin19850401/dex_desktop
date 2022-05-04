using System;

namespace DEXExtendLib
{
    public interface IDEXPluginDictionary : IDEXPluginInfo, IDEXPluginStartup
    {
        void ShowDictionary(Object toolbox);
    }
}
