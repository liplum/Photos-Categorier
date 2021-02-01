using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace PhotosCategorier.Utils
{
    public static class ImageTool
    {
        /// <summary>
        /// 读取文件路径的图像
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        /// <exception cref="CannotOpenFileException"></exception>
        public static Bitmap ReadBitmap(this string ImagePath)
        {
            try
            {
                using var img = Image.FromFile(ImagePath);
                return new Bitmap(img);
            }
            catch
            {
                throw new CannotOpenFileException($"Can't Open the file at {ImagePath}.");
            }
        }

        public static WriteableBitmap GetImageSource(this Bitmap Bitmap)
        {
            var hBitmap = Bitmap.GetHbitmap();
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();

            return new WriteableBitmap(bitmapSource);
        }

        public class NotInitPhotographException : Exception
        {
            public NotInitPhotographException(string message) : base(message) { }
        }

        public class CannotProcessImageException : Exception
        {
            public CannotProcessImageException(string message) : base(message) { }
        }

        public class CannotOpenFileException : Exception
        {
            public CannotOpenFileException(string message) : base(message) { }
        }

        public class ImageSizeException : Exception
        {
            public ImageSizeException(string message) : base(message) { }
        }

    }
}
