using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PhotosCategorier.Main
{
    public partial class MainWindow
    {
        private void Refresh()
        {
            photographs.CleanNotExisted();
            var needRemove = new List<Album>();
            var needAdd = new List<Photograph>();
            foreach (var album in allClassifyFolder)
            {
                var ps = album.GetAllPhotographs();
                if (ps == null)
                {
                    needRemove.Add(album);
                }
                else
                {
                    needAdd.AddRange(ps);

                }
            }
            foreach (var album in needRemove)
            {
                allClassifyFolder.Remove(album);
            }
            photographs.AddRange(needAdd);
            InitImage();
        }

        private void Clear()
        {
            photographs.Clear();
            allClassifyFolder.Clear();
            ResetComponent();
            UpdateRemainingFileCounter();
        }

        private void InitComponent()
        {
            DeleteThis.IsEnabled = SkipThis.IsEnabled = true;
            AddingClassifyFolder.IsEnabled = true;
            RefreshButton.IsEnabled = ClearButton.IsEnabled = true;
            Inited = true;
        }

        private void ResetComponent()
        {
            DeleteThis.IsEnabled = SkipThis.IsEnabled = false;
            AddingClassifyFolder.IsEnabled = false;
            RefreshButton.IsEnabled = ClearButton.IsEnabled = false;
            ClearCurImage();
            Inited = false;
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

        private enum EmptyMessage { HAS_NO_PHOTO, NOT_SET_CLASSIFY, NOT_HOLD_PHOTO }
        /// <summary>
        /// Checking whether the photographs is empty or not . If it's empty , it would pop a message box with what you want to display
        /// </summary>
        /// <param name="">What you want to display</param>
        /// <returns>If photographs is empty , it would return ture.</returns>
        private bool CheckEmptyWithMessage(EmptyMessage emptyMessage)
        {
            if (photographs.IsEmpty)
            {
                switch (emptyMessage)
                {
                    case EmptyMessage.HAS_NO_PHOTO:
                        MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
                        break;
                    case EmptyMessage.NOT_SET_CLASSIFY:
                        MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                        break;
                    case EmptyMessage.NOT_HOLD_PHOTO:
                        MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                        break;
                }
                return true;
            }

            return false;
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
    }
}
