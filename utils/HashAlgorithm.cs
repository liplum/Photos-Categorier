using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PhotosCategorier.Utils
{
    public static class HashAlgorithm
    {
        /// <summary>
        /// get the md5 of the file
        /// </summary>
        public static string Md5(this Stream stream)
        {
            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(stream);
            var b = md5.Hash;
            md5.Clear();
            var sb = new StringBuilder(32);
            foreach (var t in b)
            {
                sb.Append($"{t:X2}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// get the md5 of the file
        /// </summary>
        public static string Md5(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Open);

            return stream.Md5();
        }
    }
}
