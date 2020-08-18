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
        /// <summary>
        /// 设置需分类的文件夹
        /// 使 删除 和 跳过 按钮可用
        /// </summary>
        private void SetClassifyFolder()
        {
            var folder = SelectFolder(Properties.Resources.SettingClassifyFolder);

            if (folder != null)
            {
                var allFiles = folder.GetFiles();
                var selectedFiles = from file in allFiles
                                    where
               file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg") || file.Name.EndsWith(".gif")
               || file.Name.EndsWith(".jpeg")
                                    select file;
                var imageFiles = selectedFiles.ToArray();

                int filesCount = imageFiles.Length;

                if (filesCount > 0)
                {
                    photographs = new List<Photograph>(filesCount);
                    foreach (var file in imageFiles)
                        photographs.Add(new Photograph(file.FullName));

                    DeleteThis.IsEnabled = SkipThis.IsEnabled = true;
                    InitImage();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                }
            }
        }
        private void SetLeftFolder()
        {
            leftArrow = SelectFolder(Properties.Resources.KeepLeftFolder);
            if (leftArrow != null)
            {
                this.LeftArrowPointedToFolder.Content = leftArrow.Name;
                ToLeft.IsEnabled = true;
            }
        }
        private void SetRightFolder()
        {
            rightArrow = SelectFolder(Properties.Resources.KeepRightFolder);
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
                IsFolderPicker = true
            };

            var mode = dialog.ShowDialog();

            if (mode == CommonFileDialogResult.Ok)
            {
                string name = dialog.FileName;

                target = new DirectoryInfo(name);

            }
            return target;
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
                RemainingFileCounter.Content = "0";
        }

        private enum Arrow { LEFT_ARROW,RIGHT_ARROW}

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
                    MessageBox.Show($"{Properties.Resources.UnauthorizedAccess}\n{photo.FilePath}",Properties.Resources.Error);
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
                if(r == MessageBoxResult.OK)
                {
                    file.DeleteFile();
                }
                NextImage();
            }
        }
    }
}
