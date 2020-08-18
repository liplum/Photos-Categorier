using System;
using System.IO;

namespace PhotosCategorier.Algorithm
{
    public static class FileAlgorithm
    {
        public static string GetNameFromPath(string filePath)
        {
            var strs = filePath.Split('\\');
            return strs[^1];
        }

        /// <summary>
        /// 移动到指定文件夹
        /// </summary>
        /// <param name="filePath">需要移动的路径</param>
        /// <param name="directory">目标文件夹</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        public static void MoveTo(this string filePath, DirectoryInfo directory)
        {
            /*//目标文件夹不存在，文件被占用
            if (File.Exists(filePath))
                throw new FolderNonexistentOrHasOccupiedException($"{directory.FullName} isn't exists Or {filePath} has occupied");*/
            try
            {
                //文件不存在
                File.Move(filePath, directory.FullName + @"\" + GetNameFromPath(filePath));
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
        }

        public static void DeleteFile(this string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch(IOException)
            {
                throw;
            }
        }
        /*/// <summary>
        /// 目标文件夹不存在，文件被占用
        /// </summary>
        public class FolderNonexistentOrHasOccupiedException : Exception
        {
            public FolderNonexistentOrHasOccupiedException(string message) : base(message) { }
        }
        /// <summary>
        /// 文件不存在
        /// </summary>
        public class FileNonexistentException : Exception
        {
            public FileNonexistentException(string message) : base(message) { }
        }*/
    }
}

