using Microsoft.WindowsAPICodePack.Dialogs;
using PhotosCategorier.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using static PhotosCategorier.Algorithm.FileAlgorithm;

namespace PhotosCategorier
{
    public partial class MainWindow
    {

        private List<Album> allClassifyFolder;

        /// <summary>
        /// 设置需分类的文件夹
        /// 使 删除 和 跳过 按钮可用
        /// </summary>
        private void SetClassifyFolder()
        {
            var directories = SelectFolders(Properties.Resources.SettingClassifyFolder);

            if (directories != null)
            {
                photographs = new List<Photograph>();
                allClassifyFolder = new List<Album>();

                foreach (var dir in directories)
                {
                    var curAlbum = new Album(dir);
                    allClassifyFolder.Add(curAlbum);
                    var curAlbumImages = curAlbum.GetAllPhotographs();
                    if (curAlbumImages != null)
                    {
                        photographs.AddRange(curAlbumImages);
                    }
                }

                if (photographs.Count > 0)
                {
                    DeleteThis.IsEnabled = SkipThis.IsEnabled = true;
                    AddingClassifyFolder.IsEnabled = true;
                    InitImage();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                }
            }
        }

        private void AddClassifyFolder()
        {
            var directories = SelectFolders(Properties.Resources.AddingClassifyFolder);

            if (directories != null)
            {
                foreach (var dir in directories)
                {
                    var curAlbum = new Album(dir);

                    if (NotHas())
                    {
                        allClassifyFolder.Add(curAlbum);
                        var curAlbumImages = curAlbum.GetAllPhotographs();
                        if (curAlbumImages != null)
                        {
                            photographs.AddRange(curAlbumImages);
                            RemainingFileCounter.Content = $"{photographs.Count - curPhoto}";
                        }
                    }

                    bool NotHas()
                    {
                        foreach (var folder in allClassifyFolder)
                        {
                            if (curAlbum.Equals(folder))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }


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

        private void InitImage()
        {
            curPhoto = 0;
            UpdateImage();
            RemainingFileCounter.Content = $"{photographs.Count}";
        }

        private void NextImage()
        {
            ++curPhoto;
            if (curPhoto < photographs.Count)
            {
                RemainingFileCounter.Content = $"{photographs.Count - curPhoto}";
                UpdateImage();
            }
            else
            {
                RemainingFileCounter.Content = "0";
            }
        }

        private enum Arrow { LEFT_ARROW, RIGHT_ARROW }

        private void MoveThisTo(Arrow arrow)
        {
            if (curPhoto >= photographs.Count)
            {
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
            }
            else
            {
                if (curPhoto == photographs.Count - 1)
                    MessageBox.Show(Properties.Resources.IsLastPhoto, Properties.Resources.Tip);
                var photo = photographs[curPhoto];
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
            if (curPhoto >= photographs.Count)
            {
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
            }
            else
            {
                if (curPhoto == photographs.Count - 1)
                    MessageBox.Show(Properties.Resources.IsLastPhoto, Properties.Resources.Tip);
                NextImage();
            }
        }

        private void DeleteThisPhoto()
        {
            if (curPhoto >= photographs.Count)
            {
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
            }
            else
            {
                if (curPhoto == photographs.Count - 1)
                    MessageBox.Show(Properties.Resources.IsLastPhoto, Properties.Resources.Tip);

                var photo = photographs[curPhoto];
                var file = photo.FilePath;
                var r = MessageBox.Show($"{Properties.Resources.ConfirmDeletion}\n{GetNameFromPath(file)}",
                    Properties.Resources.Warnning, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    try
                    {
                        file.DeleteFile();
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
            try
            {
                curImage.Source = photographs?[curPhoto].GetImageSource();
            }
            catch
            {
                var file = photographs[curPhoto].FilePath;
                var r = MessageBox.Show($"{Properties.Resources.CannotOpen}\n{FileAlgorithm.GetNameFromPath(file)}\n{Properties.Resources.ConfirmDeletion}", Properties.Resources.Error, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    file.DeleteFile();
                }
                NextImage();
            }
        }
    }
}
