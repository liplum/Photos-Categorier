using PhotosCategorier.Utils;
using System;
using System.Windows;

namespace PhotosCategorier.Servers
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
                if (Properties.Settings.Default.DeleteDirectly)
                {
                    filePath.DeleteFile();
                }
                else
                {
                    filePath.DeleteFileToRecycleBin();
                }
            }
            catch (Exception)
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
                if (Properties.Settings.Default.DeleteDirectly)
                {
                    filePath.DeleteFile();
                }
                else
                {
                    filePath.DeleteFileToRecycleBin();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
