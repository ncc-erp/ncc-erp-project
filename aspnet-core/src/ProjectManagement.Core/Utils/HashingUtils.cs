using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManagement.Utils
{
    public static class HashingUtils
    {
        public static byte[] HMAC_SHA256(byte[] key, byte[] data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(data);
            }
        }

        public static string MD5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return string.Join(string.Empty, md5
                    .ComputeHash(Encoding.ASCII.GetBytes(input))
                    .Select(s => s.ToString("x2")));
            }
        }

        public static string HEX(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
