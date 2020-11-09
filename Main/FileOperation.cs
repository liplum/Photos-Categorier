using Microsoft.WindowsAPICodePack.Dialogs;
using PhotosCategorier.Main.Exceptions;
using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using static PhotosCategorier.Utils.FileTool;
using My_DirectoryNotFoundException = PhotosCategorier.Main.Exceptions.DirectoryNotFoundException;

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
        /// <summary>
        /// Open this file as a photo via Windows' Photos.
        /// <br/>
        /// If this photo isn't existed ,it throw <see cref="FileHasOccupiedOrBeenDeletedException"/>
        /// </summary>
        /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
        private void OpenThisPhoto()
        {
            var cur = photographs.Current;
            if (!(cur is null))
            {
                var curfile = new FileInfo(cur.FilePath);
                if (curfile.Exists)
                {
                    var path = curfile.FullName;
                    System.Diagnostics.Process.Start("explorer.exe", path);
                }
                else
                {
                    throw new FileHasOccupiedOrBeenDeletedException();
                }
            }
        }
        /// <summary>
        /// Open the folder in which this photo located via Windows' Explorer.
        /// <br/>
        /// If this photo isn't existed ,it throw <see cref="FileHasOccupiedOrBeenDeletedException"/> 
        /// <br/>
        /// If the folder isn't existed ,it throw <see cref="My_DirectoryNotFoundException"/>
        /// </summary>
        /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
        /// <exception cref="My_DirectoryNotFoundException"></exception>
        private void OpenFolderWherePhotoIs()
        {
            var cur = photographs.Current;
            if (!(cur is null))
            {
                var curfile = new FileInfo(cur.FilePath);
                if (curfile.Exists)
                {

                    var dir = curfile.Directory;
                    if (dir.Exists)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", dir.FullName);
                    }
                    else
                    {
                        throw new My_DirectoryNotFoundException();
                    }
                }
                else
                {
                    throw new FileHasOccupiedOrBeenDeletedException();
                }
            }
        }

        /// <summary>
        /// Copy this photo into the Clipboard.
        /// </summary>
        /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
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
                    throw new FileHasOccupiedOrBeenDeletedException();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <exception cref="NotHoldPhotoException"></exception>
        private void AddClassifyFolderWithSelection(string title)
        {
            var directories = SelectFolders(title);

            try
            {
                AddClassifyFolder(directories);
            }
            catch
            {
                throw;
            }
            finally
            {
                UpdateRemainingFileCounter();
                InitComponent();
                InitImage();
            }
        }

        /// <summary>
        /// Add the classify folder.And update <see cref="RemainingFileCounter"/>
        /// <br/>
        /// Its essence is that it'll add all photos which are in these folders into <see cref="photographs"/> and add these folders into <see cref="allClassifyFolder"/>
        /// <br/>
        /// If these folders don't hold the photos or the application can read them ,it throw <see cref="NotHoldPhotoException"/>
        /// </summary>
        /// <param name="directories"></param>
        /// <exception cref="NotHoldPhotoException"></exception>
        private void AddClassifyFolder(DirectoryInfo[] directories)
        {
            if (directories != null)
            {
                var addedRes = AddClassifyFolderFunc(directories);

                if ((addedRes & AddClassifyFolderResult.ACTUALLY_ADDED) != 0)
                {


                }
                else if ((addedRes & AddClassifyFolderResult.REPEATEDLY_ADDED) != 0)
                {

                }
                else
                {
                    throw new NotHoldPhotoException();
                }
            }
        }
        [Flags]
        private enum AddClassifyFolderResult
        {
            ACTUALLY_ADDED = 0b_1_0_0,
            REPEATEDLY_ADDED = 0b_0_1_0,
            NOT_ADDED = 0b_0_0_1
        }
        /// <summary>
        /// Add Classify Folder
        /// </summary>
        /// <param name="directories"></param>
        /// <returns>If it actually added some photos , it'll return true . Otherwise , there are nothing is added , it return false.</returns>
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
            var allTypeGroups = (from path in allPath group path by path.GetTargetFileType()).ToArray();

            bool actuallyAdded = false;

            foreach (var group in allTypeGroups)
            {
                switch (group.Key)
                {
                    case TargetFileType.PHOTO:
                        {
                            var res = DropPhoto(group.ToArray());
                            actuallyAdded = res || actuallyAdded;
                        }
                        break;
                    case TargetFileType.FOLDER:
                        {
                            var res = DropFolder(group.ToArray());
                            actuallyAdded = res || actuallyAdded;
                        }
                        break;
                    case TargetFileType.ANOTHER:
                        {
                            ;
                        }
                        break;
                }
            }

            if (!actuallyAdded)
            {
                throw new NotHoldPhotoException();
            }
        }

        /// <summary>
        /// Adding all the photos (They are included in these folders).
        /// </summary>
        /// <param name="allPath"></param>
        /// <returns>It has actually added some photos.</returns>
        private bool DropFolder(string[] allPath)
        {
            var allDirs = (from item in allPath select new DirectoryInfo(item)).ToArray();
            try
            {
                AddClassifyFolder(allDirs);
                return true;
            }
            catch (NotHoldPhotoException)
            {
                return false;
            }
            finally
            {
                UpdateRemainingFileCounter();
                InitComponent();
                InitImage();
            }
        }
        /// <summary>
        /// Adding all the photos (Themselves are just files).
        /// </summary>
        /// <param name="allPath"></param>
        /// <param name="remains">The remains except all the photos.</param>
        /// <returns>It has actually added some photos.</returns>
        private bool DropPhoto(string[] allPath)
        {
            var allPhotos = (from item in allPath select new Photograph(item)).ToList();

            if (allPhotos.Count != 0)
            {
                photographs.AddRange(allPhotos);
                UpdateRemainingFileCounter();
                InitComponent();
                InitImage();
                return true;
            }
            return false;
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
