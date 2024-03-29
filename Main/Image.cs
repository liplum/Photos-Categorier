﻿using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System.Windows;
using System.Windows.Media;
using PhotosCategorier.Server;

namespace PhotosCategorier.Main
{
    public partial class MainWindow
    {
        private ImageSource curPhotoSource;

        public ImageSource CurPhotoSource
        {
            get => curPhotoSource;
            set
            {
                if (curPhotoSource == value) return;
                curPhotoSource = value;
                OnPropertyChanged();
            }
        }

        private string curPhotoInfo;

        public string CurPhotoInfo
        {
            get => curPhotoInfo;
            set
            {
                if (curPhotoInfo == value) return;
                curPhotoInfo = value;
                OnPropertyChanged();
            }
        }

        private int remainingFiles = 0;

        public int RemainingFiles
        {
            get => remainingFiles;
            set
            {
                if (remainingFiles == value) return;
                remainingFiles = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// It'll reset <see cref="curPhoto"/> to 0 and update current Image which is displayed and file counter.
        /// </summary>
        private void InitImage()
        {
            photographs.Reset();
            InitRenderPool();
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

        private void RenderImage()
        {
            try
            {
                CurPhotoSource = renderer.GetCurrentRender().GetImageSource();
            }
            catch
            {
                ClearCurImage();
                throw;
            }
        }

        private void ClearCurImage()
        {
            CurPhotoSource = null;
        }

        private void DisplayPhotoInfo(Photograph photo)
        {
            CurPhotoInfo = photo.FilePath;
        }

        private void ResetPhotoInfo()
        {
            CurPhotoInfo = null;
        }

        private void InitRenderPool()
        {
            if (CheckEmptyWithMessage(EmptyMessage.HasNoPhoto))
            {
                ClearCurImage();
                return;
            }
            var curPhoto = photographs.Current;
            try
            {
                CurPhotoSource = renderer.Init().GetImageSource();
                DisplayPhotoInfo(curPhoto);
            }
            catch
            {
                ClearCurImage();
                var r = MessageBox.Show($"{Properties.Resources.CannotOpen}\n{curPhoto.FilePath.GetLastName()}\n{Properties.Resources.ConfirmDeletion}", Properties.Resources.Error, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    curPhoto.FilePath.Deleted();
                }
            }
        }

        private void UpdateImage()
        {
            if (CheckEmptyWithMessage(EmptyMessage.HasNoPhoto))
            {
                ClearCurImage();
                return;
            }
            var curPhoto = photographs.Current;
            try
            {
                RenderImage();
                DisplayPhotoInfo(curPhoto);
            }
            catch
            {
                var r = MessageBox.Show($"{Properties.Resources.CannotOpen}\n{curPhoto.FilePath.GetLastName()}\n{Properties.Resources.ConfirmDeletion}", Properties.Resources.Error, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    curPhoto.FilePath.Deleted();
                }
            }
        }

    }
}
