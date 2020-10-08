using Microsoft.WindowsAPICodePack.Dialogs;
using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;

namespace PhotosCategorier.Main
{
    public sealed partial class MainWindow
    {
        public bool IsIncludeSubfolder
        {
            get => Properties.Settings.Default.IncludeSubfolder;
            set
            {
                var settings = Properties.Settings.Default;

                if (settings.IncludeSubfolder != value)
                {
                    settings.IncludeSubfolder = value;
                    OnPropertyChanged();
                }
            }
        }

        private string leftArrowContent = Properties.Resources.Left;
        public string LeftArrowContent
        {
            get => leftArrowContent;
            set
            {
                if (leftArrowContent != value)
                {
                    leftArrowContent = value;
                    OnPropertyChanged();
                }
            }
        }
        private string rightArrowContent = Properties.Resources.Right;

        public string RightArrowContent
        {
            get => rightArrowContent;
            set
            {
                if (rightArrowContent != value)
                {
                    rightArrowContent = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isEnd = true;

        public bool IsEnd
        {
            get => isEnd;
            set
            {
                if (isEnd != value)
                {
                    isEnd = value;
                    OnPropertyChanged();
                }
            }
        }

        private enum OpenMode { FILE, DIRECTORY }
        private void OpenFileBy(OpenMode mode)
        {
            var cur = photographs.Current;
            if (!(cur is null))
            {
                var curfile = new FileInfo(cur.FilePath);
                if (curfile.Exists)
                {
                    var path = "";
                    switch (mode)
                    {
                        case OpenMode.FILE:
                            {
                                path = curfile.FullName;
                            }
                            break;
                        case OpenMode.DIRECTORY:
                            {
                                var dir = curfile.Directory;
                                if (dir.Exists)
                                {
                                    path = dir.FullName;
                                }
                                else
                                {
                                    MessageBox.Show(Properties.Resources.DirectoryNotFound, Properties.Resources.Error);
                                    return;
                                }
                            }
                            break;
                    }
                    System.Diagnostics.Process.Start("explorer.exe", path);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.FileHasOccupiedOrHasDeleted, Properties.Resources.Error);
                }
            }
        }

        private void CopyCurrent()
        {
            var cur = photographs.Current;
            if (!(cur is null))
            {
                var curfile = new FileInfo(cur.FilePath);
                if (curfile.Exists)
                {
                    var file = new StringCollection
                    {
                        curfile.FullName
                    };
                    Clipboard.SetFileDropList(file);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.FileHasOccupiedOrHasDeleted, Properties.Resources.Error);
                }
            }
        }

        private void AddClassifyFolderWithSelection(string title)
        {
            var directories = SelectFolders(title);
            AddClassifyFolder(directories);
        }

        private void AddClassifyFolder(DirectoryInfo[] directories)
        {
            if (directories != null)
            {
                var addedRes = AddClassifyFolderFunc(directories);

                if ((addedRes & AddClassifyFolderResult.ACTUALLY_ADDED) != 0)
                {
                    InitComponent();
                    InitImage();
                }
                else if ((addedRes & AddClassifyFolderResult.REPEATEDLY_ADDED) != 0)
                {

                }
                else
                {
                    MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                }
            }

            UpdateRemainingFileCounter();
        }
        [Flags]
        private enum AddClassifyFolderResult
        {
            ACTUALLY_ADDED = 0b100,
            REPEATEDLY_ADDED = 0b010,
            NOT_ADDED = 0b001
        }
        /// <summary>
        /// Add Classify Folder
        /// </summary>
        /// <param name="directories"></param>
        /// <returns>If it actually added some photos , it would return true . Otherwise , there are nothing is added , it return false.</returns>
        private AddClassifyFolderResult AddClassifyFolderFunc(DirectoryInfo[] directories)
        {
            var isIncludeSubfolder = IsIncludeSubfolder;
            var folderNeedAdd = new List<Album>();
            AddFunc(directories);

            var allPhotos = new List<Photograph>();
            var needRemove = new List<Album>();
            var addedResult = AddClassifyFolderResult.NOT_ADDED;
            foreach (var album in folderNeedAdd)
            {
                var all = album.GetAllPhotographs();
                if (all is null)
                {
                    needRemove.Add(album);
                }
                else
                {
                    if (allClassifyFolder.Contains(album))
                    {
                        addedResult |= AddClassifyFolderResult.REPEATEDLY_ADDED;//equals actuallyAdded += AddClassifyFolderResult.REPEATEDLY_ADDED
                    }
                    else
                    {
                        addedResult |= AddClassifyFolderResult.ACTUALLY_ADDED;//equals actuallyAdded += AddClassifyFolderResult.ACTUALLY_ADDED
                    }
                    allPhotos.AddRange(all);
                }
            }
            allClassifyFolder = allClassifyFolder.Distinct().ToList();
            allClassifyFolder.AddRange(folderNeedAdd);
            allClassifyFolder.RemoveAll(item => needRemove.Contains(item));
            photographs.AddRange(allPhotos);
            return addedResult;

            void AddFunc(DirectoryInfo[] dirs)
            {
                foreach (var dir in dirs)
                {
                    folderNeedAdd.Add(new Album(dir));
                    if (isIncludeSubfolder)
                    {
                        var allSub = dir.GetDirectories();
                        if (allSub.Length != 0)
                        {
                            AddFunc(allSub);
                        }
                    }
                }
            }
        }

        private void DropFolderOrFile(Array array)
        {
            var len = array.Length;
            var allPath = new string[len];
            for (int i = 0; i < len; i++)
            {
                allPath[i] = array.GetValue(i).ToString();
            }
            var rest = DropFile(allPath);
            DropFolder(rest);
        }

        /// <summary>
        /// Adding all the photos in these folders
        /// </summary>
        /// <param name="allPath"></param>
        /// <returns>The remain except all the folders.</returns>
        private string[] DropFolder(string[] allPath)
        {
            var allDirs = (from item in allPath where new DirectoryInfo(item).Exists select new DirectoryInfo(item)).ToArray();
            AddClassifyFolder(allDirs);
            var allDirPaths = (from item in allDirs select item.FullName).ToArray();
            return allPath.Except(allDirPaths).ToArray();
        }
        /// <summary>
        /// Adding all the photos
        /// </summary>
        /// <param name="allPath"></param>
        /// <returns>The remain except all the files.</returns>
        private string[] DropFile(string[] allPath)
        {
            var allPhotos = (from item in allPath where new FileInfo(item).IsPhotograph() select new Photograph(item)).ToList();
            photographs.AddRange(allPhotos);
            var allPhotosPaths = (from item in allPhotos select item.FilePath).ToArray();
            return allPath.Except(allPhotosPaths).ToArray();
        }
        private void SetLeftFolderWithSelection()
        {
            leftArrow = SelectFolder(Properties.Resources.SelectLeftFolder);
            SetLeftFolder();
        }

        private void SetLeftFolder()
        {
            if (leftArrow != null)
            {
                LeftArrowContent = leftArrow.Name;
                ToLeft.IsEnabled = true;
                LeftArrowPointedToFolder.Visibility = Visibility.Visible;
            }
        }

        private void SetRightFolderWithSelection()
        {
            rightArrow = SelectFolder(Properties.Resources.SelectRightFolder);
            SetRightFolder();
        }

        private void SetRightFolder()
        {
            if (rightArrow != null)
            {
                RightArrowContent = rightArrow.Name;
                ToRight.IsEnabled = true;
                RightArrowPointedToFolder.Visibility = Visibility;
            }
        }

        /// <summary>
        /// 打开选择文件夹窗口
        /// </summary>
        /// <param name="Caption">窗口标题</param>
        /// <returns></returns>
        private static DirectoryInfo SelectFolder(string Caption)
        {
            DirectoryInfo target = null;

            using var dialog = new CommonOpenFileDialog(Caption)
            {
                IsFolderPicker = true,
            };

            var mode = dialog.ShowDialog();

            if (mode == CommonFileDialogResult.Ok)
            {
                string name = dialog.FileName;

                target = new DirectoryInfo(name);

            }
            return target;
        }

        /// <summary>
        /// 打开选择文件夹窗口
        /// </summary>
        /// <param name="Caption">窗口标题</param>
        /// <returns></returns>
        private static DirectoryInfo[] SelectFolders(string Caption)
        {

            using var dialog = new CommonOpenFileDialog(Caption)
            {
                IsFolderPicker = true,
                Multiselect = true
            };

            var mode = dialog.ShowDialog();

            if (mode == CommonFileDialogResult.Ok)
            {
                string[] names = dialog.FileNames.ToArray();
                int len = names.Length;
                DirectoryInfo[] dirs = new DirectoryInfo[len];
                for (int i = 0; i < len; i++)
                {
                    var name = names[i];
                    dirs[i] = new DirectoryInfo(name);
                }

                return dirs;
            }
            return null;
        }
    }
}
