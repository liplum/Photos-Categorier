using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PhotosCategorier.Utils
{
    public static class HashAlgorithm
    {
        /// <summary>
        /// 对文件流进行MD5加密
        /// </summary>
        public static string MD5(this Stream stream)
        {
            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(stream);
            var b = md5.Hash;
            md5.Clear();
            var sb = new StringBuilder(32);
            for (var i = 0; i < b.Length; i++)
            {
                sb.Append($"{b[i]:X2}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对文件进行MD5加密
        /// </summary>
        public static string MD5(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Open);

            return stream.MD5();
        }
    }
}
