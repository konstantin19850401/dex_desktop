using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXUserData
    {
        string Login { get; }
        string Password { get; }
        string UID { get; }
        string currentBase { get; }
        string MAC { get; }
        string Title { get; }
        DateTime DateCreated { get; }
        DateTime DateChanged { get; }
        bool isOnline { get; }
        SimpleXML UserProperties { get; }
        string dataBase {get;}
        string adaptersLogin { get; set; }
        string adaptersPass { get; set; }
        string dexServer { get; set; }
    }
}
