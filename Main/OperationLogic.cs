using Microsoft.WindowsAPICodePack.Dialogs;
using PhotosCategorier.Algorithm;
using PhotosCategorier.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using static PhotosCategorier.Algorithm.FileTool;

namespace PhotosCategorier
{
    public partial class MainWindow : INotifyPropertyChanged
    {

        private List<Album> allClassifyFolder;

        public bool IsIncludeSubfolder
        {
            get => Properties.Settings.Default.IncludeSubfolder;
            set
            {
                var settings = Properties.Settings.Default;

                if (settings.IncludeSubfolder != value)
                {
                    settings.IncludeSubfolder = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsIncludeSubfolder)));
                }
            }
        }

        private int remainingFiles = 0;

        public int RemainingFiles
        {
            get => remainingFiles;
            set
            {
                if (remainingFiles != value)
                {
                    remainingFiles = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingFiles)));
                }
            }
        }

        /// <summary>
        /// 设置需分类的文件夹
        /// 使 删除 和 跳过 按钮可用
        /// </summary>
        private void SetClassifyFolderWithSelection()
        {
            var directories = SelectFolders(Properties.Resources.SettingClassifyFolder);
            SetClassifyFolder(directories);
        }

        private void SetClassifyFolder(DirectoryInfo[] directories)
        {
            if (directories != null)
            {
                photographs.Clear();
                allClassifyFolder = new List<Album>();

                foreach (var dir in directories)
                {
                    SetClassifyFolderFunc(dir, IsIncludeSubfolder);
                }

                if (photographs.Count > 0)
                {
                    DeleteThis.IsEnabled = SkipThis.IsEnabled = true;
                    AddingClassifyFolder.IsEnabled = true;
                    RefreshButton.IsEnabled = ClearButton.IsEnabled = true;
                    InitImage();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                }
            }

            void SetClassifyFolderFunc(DirectoryInfo dir, bool isIncludeSubfolder)
            {
                var curAlbum = new Album(dir);
                allClassifyFolder.Add(curAlbum);
                var curAlbumImages = curAlbum.GetAllPhotographs();
                if (curAlbumImages != null)
                {
                    photographs.AddRange(curAlbumImages);
                }

                if (isIncludeSubfolder)
                {
                    var subFolders = dir.GetDirectories();
                    foreach (var sub in subFolders)
                    {
                        SetClassifyFolderFunc(sub, isIncludeSubfolder);
                    }
                }
            }
        }

        private void AddClassifyFolderWithSelection()
        {
            var directories = SelectFolders(Properties.Resources.AddingClassifyFolder);
            AddClassifyFolder(directories);
        }

        private void AddClassifyFolder(DirectoryInfo[] directories)
        {
            if (directories != null)
            {
                if (allClassifyFolder == null)
                {
                    SetClassifyFolder(directories);
                }
                else
                {
                    foreach (var dir in directories)
                    {
                        AddClassifyFolderFunc(dir, IsIncludeSubfolder);
                    }

                    UpdateRemainingFileCounter();
                }
            }

            void AddClassifyFolderFunc(DirectoryInfo dir, bool isIncludeSubfolder)
            {
                var curAlbum = new Album(dir);

                if (NotHas())
                {
                    allClassifyFolder.Add(curAlbum);
                    var curAlbumImages = curAlbum.GetAllPhotographs();
                    if (curAlbumImages != null)
                    {
                        photographs.AddRange(curAlbumImages);
                    }
                }

                if (isIncludeSubfolder)
                {
                    var subFolders = dir.GetDirectories();
                    foreach (var sub in subFolders)
                    {
                        AddClassifyFolderFunc(sub, isIncludeSubfolder);
                    }
                }

                bool NotHas()
                {
                    bool notHas = true;
                    foreach (var folder in allClassifyFolder)
                    {
                        if (curAlbum.Equals(folder))
                        {
                            notHas = false;
                        }
                    }
                    return notHas;
                }
            }
        }

        private void ClearDuplicates()
        {
            var r = MessageBox.Show(Properties.Resources.ConfirmClearingDuplicates, Properties.Resources.Warnning, MessageBoxButton.OKCancel);
            if (r == MessageBoxResult.OK)
            {
                if (photographs.IsEmpty)
                {
                    MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                    return;
                }
                Refresh();
                var window = new ClearDuplicatesWindow(photographs.AllPhotographs);
                window.ShowDialog();
                Refresh();
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
            DropFolder(allPath);
            DropFile(allPath);
        }

        private void DropFolder(string[] allPath)
        {
            var allDirs = (from item in allPath where new DirectoryInfo(item).Exists select new DirectoryInfo(item)).ToArray();
            AddClassifyFolder(allDirs);
        }

        private void DropFile(string[] allPath)
        {
            var allPhotos = (from item in allPath where new FileInfo(item).IsPhotograph() select new Photograph(item)).ToList();
            photographs.AddRange(allPhotos);
        }

        private void Refresh()
        {
            photographs.Clear();
            var needRemove = new List<Album>();
            foreach (var album in allClassifyFolder)
            {
                var ps = album.GetAllPhotographs();
                if (ps == null)
                {
                    needRemove.Add(album);
                }
                else
                {
                    photographs.AddRange(ps);

                }
            }
            foreach (var album in needRemove)
            {
                allClassifyFolder.Remove(album);
            }
            InitImage();
        }

        private void Clear()
        {
            photographs.Clear();
            allClassifyFolder = new List<Album>();
            ClearCurImage();
        }

        private void SetLeftFolder()
        {
            leftArrow = SelectFolder(Properties.Resources.SelectLeftFolder);
            if (leftArrow != null)
            {
                this.LeftArrowPointedToFolder.Content = leftArrow.Name;
                ToLeft.IsEnabled = true;
            }
        }
        private void SetRightFolder()
        {
            rightArrow = SelectFolder(Properties.Resources.SelectRightFolder);
            if (rightArrow != null)
            {
                this.RightArrowPointedToFolder.Content = rightArrow.Name;
                ToRight.IsEnabled = true;
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

        /// <summary>
        /// It'll reset <see cref="curPhoto"/> to 0 and update current Image which is displayed and file counter.
        /// </summary>
        private void InitImage()
        {
            photographs.Reset();
            UpdateImage();
            UpdateRemainingFileCounter();
        }

        private void NextImage()
        {
            if (photographs.MoveNext())
            {
                UpdateImage();
            }
            UpdateRemainingFileCounter();
        }

        private void UpdateRemainingFileCounter()
        {
            RemainingFiles = photographs.RemainingFiles;
        }

        private void RenderImage(Photograph photo)
        {
            try
            {
                curImage.Source = photo.GetImageSource();
            }
            catch
            {
                ClearCurImage();
                throw;
            }
        }

        private void ClearCurImage()
        {
            curImage.Source = null;
        }

        private enum Arrow { LEFT_ARROW, RIGHT_ARROW }

        /// <summary>
        /// Checking whether current photo is the last one
        /// </summary>
        /// <returns>If the current photo is the last one or it hasn't arrived yet,it would return true.Otherwise,it had ended and return false.</returns>
        private bool CheckLastOne()
        {
            if (!photographs.HasNext)
            {
                ClearCurImage();
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
                return false;
            }
            return true;
        }
        private void MoveThisTo(Arrow arrow)
        {
            if (CheckLastOne())
            {
                var photo = photographs.Current;
                try
                {
                    switch (arrow)
                    {
                        case Arrow.LEFT_ARROW:
                            photo.FilePath.MoveTo(leftArrow);
                            break;
                        case Arrow.RIGHT_ARROW:
                            photo.FilePath.MoveTo(rightArrow);
                            break;
                    }
                    NextImage();
                }
                catch (DirectoryNotFoundException)
                {
                    DirectoryInfo target = null;
                    switch (arrow)
                    {
                        case Arrow.LEFT_ARROW:
                            target = leftArrow;
                            break;
                        case Arrow.RIGHT_ARROW:
                            target = rightArrow;
                            break;
                    }
                    MessageBox.Show($"{Properties.Resources.DirectoryNotFound}\n{target.FullName}", Properties.Resources.Error);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show($"{Properties.Resources.UnauthorizedAccess}\n{photo.FilePath}", Properties.Resources.Error);
                }
                catch (IOException)
                {
                    MessageBox.Show($"{Properties.Resources.FileHasOccupiedOrHasDeleted}\n{photo.FilePath}", Properties.Resources.Error);
                }
            }
        }

        private void SkipThisPhoto()
        {
            if (CheckLastOne())
            {
                NextImage();
            }
        }

        private void DeleteThisPhoto()
        {
            if (CheckLastOne())
            {
                var photo = photographs.Current;
                var file = photo.FilePath;
                var r = MessageBox.Show($"{Properties.Resources.ConfirmDeletion}\n{file.GetLastName()}",
                    Properties.Resources.Warnning, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    try
                    {
                        file.DeleteFileToRecycleBin();
                    }
                    catch (IOException)
                    {
                        MessageBox.Show($"{Properties.Resources.FileHasOccupied}\n{file}", Properties.Resources.Error);
                    }
                    NextImage();
                }
            }

        }

        private void UpdateImage()
        {
            if (photographs.IsEmpty)
            {
                ClearCurImage();
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
                return;
            }
            var curPhoto = photographs.Current;
            try
            {
                RenderImage(curPhoto);
            }
            catch
            {
                var r = MessageBox.Show($"{Properties.Resources.CannotOpen}\n{curPhoto.FilePath.GetLastName()}\n{Properties.Resources.ConfirmDeletion}", Properties.Resources.Error, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    curPhoto.FilePath.DeleteFileToRecycleBin();
                }
            }
        }
    }
}
