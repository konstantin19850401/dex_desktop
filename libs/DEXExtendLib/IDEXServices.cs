using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DEXExtendLib
{
    public interface IDEXServices
    {
        void RegisterService(string servicename, Object service);
        void UnregisterService(string servicename);
        Object GetService(string servicename);
        bool checkPassport(string series, string number);
        string checkForTerrorists(string json);
        string checkPassportPacket(string json);
        string getRegionByUid(string uid);
        string sendRequest(string method, string port, string url, string body, int reqStatus);
    }
}
