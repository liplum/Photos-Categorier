using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using static PhotosCategorier.Utils.FileTool;

namespace PhotosCategorier.Main
{
    public partial class MainWindow : INotifyPropertyChanged
    {
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
            ClearCurImage();
            UpdateRemainingFileCounter();
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
