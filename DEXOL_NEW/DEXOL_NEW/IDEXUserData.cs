using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DEXOL_NEW
{
    public interface IDEXUserData
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string SecondName { get; set; }
        string Userpic { get; set; }
        Dictionary<string, JObject> AppsData { get; }
        void AddApp(JObject app);
    }
}
