using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXPluginStartup
    {
        void Startup(Object toolbox);
    }
}
