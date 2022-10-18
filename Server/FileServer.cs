using System;
using System.Windows;
using PhotosCategorier.Utils;

namespace PhotosCategorier.Server
{
    public static class FileServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public static void Deleted(this string filePath)
        {
            try
            {
                filePath.DeletedWithException();
            }
            catch
            {
                MessageBox.Show(Properties.Resources.FileHasOccupied, Properties.Resources.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public static void DeletedWithException(this string filePath)
        {
            try
            {
                if (Properties.Settings.Default.DeleteToRecycleBin)
                {
                    filePath.DeleteFileToRecycleBin();
                }
                else
                {
                    filePath.DeleteFile();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
