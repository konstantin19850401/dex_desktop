using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPluginFunction : IDEXPluginInfo
    {
        bool Execute(Object toolbox);
    }
}
