using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXSysData
    {
        string AppDir { get; }
        string DataDir { get; }
        ArrayList DocumentTypes();
        void keybRU();
        void keybEN();
        string[] DocumentStatesText { get; }
    }
}
