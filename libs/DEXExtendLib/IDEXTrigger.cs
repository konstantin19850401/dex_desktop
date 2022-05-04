using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXTrigger
    {
        void setTrigger(string title, string value);
        string getTrigger(string title);
        void clearTriggers();
    }
}
