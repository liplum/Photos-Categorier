using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PhotosCategorier.Utils
{
    public class HashAlgorithm
    {
        /// <summary>
        /// 对文件流进行MD5加密
        /// </summary>
        public static string MD5(Stream stream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(stream);
            byte[] b = md5.Hash;
            md5.Clear();
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对文件进行MD5加密
        /// </summary>
        public static string MD5(string filePath)
        {
            using FileStream stream = File.Open(filePath, FileMode.Open);

            return MD5(stream);
        }
    }
}
