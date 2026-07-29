using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Jaiz_CSAT_Survey.Services
{
    public class Crypto
    {
        private static byte[] _key;

        private static byte[] IV;

        private readonly static Random GetRandom;

        private readonly static Random Rng;

        private readonly static object syncLock;

        private readonly static Random GetSmallRandom;

        static Crypto()
        {
            Crypto._key = new byte[0];
            Crypto.IV = new byte[] { 18, 52, 86, 120, 144, 171, 205, 239 };
            Crypto.GetRandom = new Random();
            Crypto.Rng = new Random();
            Crypto.syncLock = new object();
            Crypto.GetSmallRandom = new Random();
        }

        public Crypto()
        {
        }

        private static string Create11DigitString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (stringBuilder.Length < 11)
            {
                int num = Crypto.Rng.Next(10);
                stringBuilder.Append(num.ToString());
            }
            return stringBuilder.ToString();
        }

        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            string str;

            try
            {
                byte[] numArray = new byte[stringToDecrypt.Length + 1];
                Crypto._key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                numArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(Crypto._key, Crypto.IV), CryptoStreamMode.Write);
                cryptoStream.Write(numArray, 0, (int)numArray.Length);
                cryptoStream.FlushFinalBlock();
                str = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception exception)
            {
                str = "Invalid encryption " + exception.Message;
            }
            return str;
        }

        public static string DecryptText(string strText)
        {
            return Crypto.Decrypt(strText, "-&;$#@?*");
        }

        private static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {
            string base64String;
            try
            {
                Crypto._key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(Crypto._key, Crypto.IV), CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, (int)bytes.Length);
                cryptoStream.FlushFinalBlock();
                base64String = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception exception)
            {
                base64String = "Invalid encryption " + exception.Message;
            }
            return base64String;
        }

        public static string EncryptText(string strText)
        {
            return Crypto.Encrypt(strText, "-&;$#@?*");
        }

        private static string Generate10PanPart()
        {
            string str = Crypto.Create11DigitString();
            string str1 = str.Substring(5).Substring(1, 2);
            string str2 = str.Substring(0, 6).Substring(1, 5);
            return string.Concat(str1, Crypto.GenerateSmallRamdomNum(), str2);
        }

        private static string Generate12PanPart()
        {
            string str = Crypto.Create11DigitString();
            string str1 = str.Substring(6);
            string str2 = str.Substring(0, 6).Substring(0, 4);
            return string.Concat(str1, Crypto.GenerateSmallRamdomNum(), str2);
        }

        private static string Generate7PanPart()
        {
            string str = Crypto.Create11DigitString();
            string str1 = str.Substring(5).Substring(2, 2);
            string str2 = str.Substring(0, 6).Substring(2, 2);
            return string.Concat(str1, Crypto.GenerateSmallRamdomNum(), str2);
        }

        private static string Generate9PanPart()
        {
            string str = Crypto.Create11DigitString();
            string str1 = str.Substring(5).Substring(0, 3);
            string str2 = str.Substring(0, 6).Substring(1, 3);
            return string.Concat(str1, Crypto.GenerateSmallRamdomNum(), str2);
        }

        private string GeneratePANValue(string bin, string productcode, int panLength)
        {
            string empty = string.Empty;
            string str = string.Empty;
            if (!(!string.IsNullOrEmpty(productcode) ? true : panLength != 16))
            {
                empty = string.Concat(bin, Crypto.Generate9PanPart());
            }
            else if (!(string.IsNullOrEmpty(productcode) ? true : panLength != 16))
            {
                empty = string.Concat(bin, productcode, Crypto.Generate7PanPart());
            }
            else if (!(!string.IsNullOrEmpty(productcode) ? true : panLength != 19))
            {
                empty = string.Concat(bin, Crypto.Generate12PanPart());
            }
            else if ((string.IsNullOrEmpty(productcode) ? false : panLength == 19))
            {
                empty = string.Concat(bin, productcode, Crypto.Generate10PanPart());
            }
            return string.Concat(empty, this.GetLuhnCheckDigit(empty));
        }

        private static string GenerateSmallRamdomNum()
        {
            string str;
            lock (Crypto.syncLock)
            {
                int num = Crypto.GetSmallRandom.Next(100, 999);
                str = num.ToString("000");
            }
            return str;
        }



        public static string GetCrypt(string text)
        {
            SHA512 sHA512 = SHA512.Create();
            byte[] numArray = sHA512.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Encoding.UTF8.GetString(numArray);
        }

        public string GetLuhnCheckDigit(string number)
        {
            string str;
            int num = 0;
            bool flag = true;
            char[] charArray = number.ToCharArray();
            for (int i = (int)charArray.Length - 1; i >= 0; i--)
            {
                int num1 = charArray[i] - 48;
                if (flag)
                {
                    num1 *= 2;
                    if (num1 > 9)
                    {
                        num1 -= 9;
                    }
                }
                num += num1;
                flag = !flag;
            }
            if (num % 10 != 0)
            {
                int num2 = 10 - num % 10;
                str = num2.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                str = "0";
            }
            return str;
        }

        public static string GetMd5Hash(string input)
        {
            string str;
            MD5 mD5 = MD5.Create();
            try
            {
                byte[] numArray = mD5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < (int)numArray.Length; i++)
                {
                    stringBuilder.Append(numArray[i].ToString("x2"));
                }
                str = stringBuilder.ToString();
            }
            finally
            {
                if (mD5 != null)
                {
                    ((IDisposable)mD5).Dispose();
                }
            }
            return str;
        }



        public string MaskPan(string pan, int panlength)
        {
            pan = pan.Trim();
            string str = pan.Substring(0, 6);
            string str1 = pan.Substring(panlength - 4, 4);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < panlength - 10; i++)
            {
                stringBuilder.Append("*");
            }
            return string.Concat(str, stringBuilder.ToString(), str1);
        }



        public static bool VerifyMd5Hash(string input, string hash)
        {
            bool flag;
            MD5 mD5 = MD5.Create();
            try
            {
                string md5Hash = Crypto.GetMd5Hash(input);
                flag = (0 != StringComparer.OrdinalIgnoreCase.Compare(md5Hash, hash) ? false : true);
            }
            finally
            {
                if (mD5 != null)
                {
                    ((IDisposable)mD5).Dispose();
                }
            }
            return flag;
        }

        public static bool VerifyMd5HashWithAnotherHash(string hash1, string hash2)
        {
            bool flag;
            MD5 mD5 = MD5.Create();
            try
            {
                flag = (0 != StringComparer.OrdinalIgnoreCase.Compare(hash1, hash2) ? false : true);
            }
            finally
            {
                if (mD5 != null)
                {
                    ((IDisposable)mD5).Dispose();
                }
            }
            return flag;
        }
    }
}
