﻿using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace PhotosCategorier.Utils
{
    public static class FileTool
    {
        public static string GetLastName(this string filePath)
        {
            var strs = filePath.Split('\\');
            return strs[^1];
        }

        public static bool IsPhotograph(this FileInfo file)
        {
            return file.Exists &&( file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg") || file.Name.EndsWith(".gif")
           || file.Name.EndsWith(".jpeg"));
        }

        public static bool IsPhotograph(this string filePath,out FileInfo file)
        {
            var f = new FileInfo(filePath);
            if (f.Exists)
            {
                file = f;
                return f.Name.EndsWith(".png") || f.Name.EndsWith(".jpg") || f.Name.EndsWith(".gif")
           || f.Name.EndsWith(".jpeg");
            }
            file = null;
            return false;
        }
        public static bool IsPhotograph(this string filePath)
        {
            var f = new FileInfo(filePath);
            if (f.Exists)
            {
                return f.Name.EndsWith(".png") || f.Name.EndsWith(".jpg") || f.Name.EndsWith(".gif")
           || f.Name.EndsWith(".jpeg");
            }
            return false;
        }

        public static bool IsExisted(this string filePath,out FileInfo file)
        {
            var f = new FileInfo(filePath);
            if (f.Exists)
            {
                file = f;
                return true;
            }
            file = null;
            return false;
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
                File.Move(filePath, directory.FullName + @"\" + filePath.GetLastName());
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
            catch (IOException)
            {
                //等待被删除的文件夹不存在
                ;
            }
        }

        public static void DeleteFileToRecycleBin(this string filePath)
        {
            try
            {
                FileSystem.DeleteFile(filePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}

