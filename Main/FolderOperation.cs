using Microsoft.WindowsAPICodePack.Dialogs;
using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        private void AddClassifyFolderWithSelection(string title)
        {
            var directories = SelectFolders(title);
            AddClassifyFolder(directories);
        }

        public bool Inited
        {
            get; set;
        } = false;

        private void AddClassifyFolder(DirectoryInfo[] directories)
        {
            if (directories != null)
            {
                if (Inited)
                {
                    AddClassifyFolderFunc(directories);
                    InitImage();
                }
                else
                {
                    AddClassifyFolderFunc(directories);

                    if (!CheckEmptyWithMessage(EmptyMessage.NOT_HOLD_PHOTO))
                    {
                        InitComponent();
                        InitImage();
                    }
                }

                UpdateRemainingFileCounter();
            }

            void AddClassifyFolderFunc(DirectoryInfo[] directories)
            {
                var isIncludeSubfolder = IsIncludeSubfolder;
                AddFunc(directories);

                allClassifyFolder = allClassifyFolder.Distinct(albumComparer).ToList();
                var allPhotos = new List<Photograph>();
                var needRemove = new List<Album>();
                foreach (var album in allClassifyFolder)
                {
                    var all = album.GetAllPhotographs();
                    if (all is null)
                    {
                        needRemove.Add(album);
                        continue;
                    }
                    allPhotos.AddRange(all);
                }
                allClassifyFolder.RemoveAll(item => needRemove.Contains(item));
                photographs.AddRange(allPhotos);

                void AddFunc(DirectoryInfo[] dirs)
                {
                    foreach (var dir in dirs)
                    {
                        allClassifyFolder.Add(new Album(dir));
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
        }

        private readonly AlbumComparer albumComparer = new AlbumComparer();

        private class AlbumComparer : IEqualityComparer<Album>
        {
            public bool Equals([AllowNull] Album x, [AllowNull] Album y)
            {
                return x.Directory.FullName.Equals(y.Directory.FullName);
            }

            public int GetHashCode([DisallowNull] Album obj)
            {
                return obj.Directory.FullName.GetHashCode();
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
                this.LeftArrowPointedToFolder.Content = leftArrow.Name;
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
                this.RightArrowPointedToFolder.Content = rightArrow.Name;
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
