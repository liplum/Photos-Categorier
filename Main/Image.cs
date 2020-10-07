using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System.Windows;
using System.Windows.Media;

namespace PhotosCategorier.Main
{
    public partial class MainWindow
    {
        private ImageSource curPhotoSource = null;

        public ImageSource CurPhotoSource
        {
            get => curPhotoSource;
            set
            {
                if (curPhotoSource != value)
                {
                    curPhotoSource = value;
                    OnPropertyChanged();
                }
            }
        }

        private string curPhotoInfo = null;

        public string CurPhotoInfo
        {
            get => curPhotoInfo;
            set
            {
                if (curPhotoInfo != value)
                {
                    curPhotoInfo = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
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
            if (RemainingFiles == 0)
            {
                RemainingFileCounter.Visibility = Visibility.Hidden;
            }
            else
            {
                RemainingFileCounter.Visibility = Visibility.Visible;
            }
        }

        private void RenderImage()
        {
            try
            {
                var res = renderer.GetCurrentRender();
                CurPhotoSource = ImageTool.ToImageSource(res);
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
            CurImageInfo.Content = photo.FilePath;
        }

        private void InitRenderPool()
        {
            if (CheckEmptyWithMessage(EmptyMessage.HAS_NO_PHOTO))
            {
                ClearCurImage();
                return;
            }
            var curPhoto = photographs.Current;
            try
            {
                var res = renderer.Init();
                CurPhotoSource = ImageTool.ToImageSource(res);
                DisplayPhotoInfo(curPhoto);
            }
            catch
            {
                ClearCurImage();
                var r = MessageBox.Show($"{Properties.Resources.CannotOpen}\n{curPhoto.FilePath.GetLastName()}\n{Properties.Resources.ConfirmDeletion}", Properties.Resources.Error, MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.OK)
                {
                    curPhoto.FilePath.DeleteFileToRecycleBin();
                }
            }
        }

        private void UpdateImage()
        {
            if (CheckEmptyWithMessage(EmptyMessage.HAS_NO_PHOTO))
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
                    curPhoto.FilePath.DeleteFileToRecycleBin();
                }
            }
        }

    }
}
