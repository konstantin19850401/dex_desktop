using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXConfig
    {
        bool isSectionExists(string section);
        bool isKeyExists(string section, string key);
        bool isBinary(string section, string key);

        int getInt(string section, string key, int def);
        bool getBool(string section, string key, bool def);
        string getStr(string section, string key, string def);
        float getFloat(string section, string key, float def);
        DateTime getDate(string section, string key, DateTime def);
        byte[] getBinary(string section, string key, byte[] def);

        void setInt(string section, string key, int value);
        void setBool(string section, string key, bool value);
        void setStr(string section, string key, string value);
        void setFloat(string section, string key, float value);
        void setDate(string section, string key, DateTime value);
        void setBinary(string section, string key, byte[] value);

        bool isRegisterKeyExists(string key);
        void createRegisterKey(string key, string title, string value);

        int getRegisterInt(string key, int def);
        bool getRegisterBool(string key, bool def);
        string getRegisterStr(string key, string def);
        float getRegisterFloat(string key, float def);
        DateTime getRegisterDate(string key, DateTime def);

        void setRegisterInt(string key, int value);
        void setRegisterBool(string key, bool value);
        void setRegisterStr(string key, string value);
        void setRegisterFloat(string key, float value);
        void setRegisterDate(string key, DateTime value);
    }
}
