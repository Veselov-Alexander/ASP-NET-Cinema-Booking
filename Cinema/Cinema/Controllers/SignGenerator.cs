using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace Cinema
{
    public static class SignGenerator
    {
        private static string key = "cinema228";
        public static string GetSign(string text)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] hash = provider.ComputeHash(Encoding.Default.GetBytes(text + key));

            return BitConverter.ToString(hash).ToLower().Replace("-", "");
        }
    }
}