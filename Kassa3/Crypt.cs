using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Kassa3
{
    public class Crypt
    {
        #region Получение хэша MD5 строки (IDEXCrypt)

        public static string StringToMD5(string source)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        #region Шифрование / дешифрование строк методом Rijndael (IDEXCrypt)
        // (c) RSDN

        public static byte[] Encrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        public static string Encrypt(string data, string password)
        {
            return Convert.ToBase64String(Crypt.Encrypt(Encoding.UTF8.GetBytes(data), password));
        }

        public static byte[] Decrypt(byte[] data, string password)
        {
            BinaryReader br = new BinaryReader(Crypt.InternalDecrypt(data, password));
            return br.ReadBytes((int)br.BaseStream.Length);
        }

        public static string Decrypt(string data, string password)
        {
            CryptoStream cs = Crypt.InternalDecrypt(Convert.FromBase64String(data), password);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        static CryptoStream InternalDecrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }

        #endregion

    }
}
