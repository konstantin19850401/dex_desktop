using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXSIM
{
    public interface IDEXSim
    {
        Dictionary<string, string> getSimByMSISDN(string msisdn);
        Dictionary<string, string> getSimByICC(string icc);
        List<string> getFreeSim();
    }
}
