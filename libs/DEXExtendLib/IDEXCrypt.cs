using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEXExtendLib
{
    public interface IDEXCrypt
    {
        string StringToMD5(string source);
        byte[] Encrypt(byte[] data, string password);
        string Encrypt(string data, string password);
        byte[] Decrypt(byte[] data, string password);
        string Decrypt(string data, string password);
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
