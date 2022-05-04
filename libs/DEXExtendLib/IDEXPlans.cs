using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;

namespace DEXExtendLib
{
    public interface IDEXPlans
    {
        Dictionary<string, Dictionary<string, string>> Tpls
        {
            get;
        }
        void setPlans(Dictionary<string, Dictionary<string, string>> dt);
        
    }
}
