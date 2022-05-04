using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DEXExtendLib
{
    public class DEXTools
    {
        /**
         * Функция позволяет проверить, верен ли ICC код сим-карты
         * Если код верен, возвращается 0
         **/
/*
        public static int GetLuhnSecureDigital(string Num)
        {
            //Num[0..N-1] - card number
            //N - card number len
            //Num[N-1] - check digit 

            int p = 0;
            int sum = 0;
            int N = Num.Length;

            for (int i = 1; i < N; i++)
            {
                p = Num[(N - 1) - i] - '0';
                if (i % 2 != 0)
                {
                    p *= 2;
                    if (p > 9)
                    {
                        p -= 9;
                    }
                }
                sum += p;
            }
            sum = (sum % 10) == 0 ? 0 : 10 - (sum % 10);
            return sum;
        }
        */

        public static int calcIccCtl(string icc)
        {
            int ret = -1;
            if (icc != null && icc.Length == 17)
            {
                int j = icc.Length - 1, cc = 0;
                for (int i = 0; i < icc.Length; ++i)
                {
                    if (i % 2 == 0)
                    {
                        int r = (icc[j] - '0') * 2;
                        if (r > 9) r -= 9;
                        cc += r;
                    }
                    else
                    {
                        cc += (icc[j] - '0');
                    }
                    j--;
                }

                ret = cc % 10;
                if (ret > 0) ret = 10 - ret;
            }

            return ret;
        }

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

    }
}
