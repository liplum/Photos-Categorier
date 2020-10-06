using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using static PhotosCategorier.Utils.ImageTool;

namespace PhotosCategorier.Photo
{
    /// <summary>
    /// 一张图片
    /// </summary>
    public class Photograph
    {
        public string FilePath { get; }

        /// <summary>
        /// 是否居中
        /// </summary>
        public static bool IsCentered { set; get; } = true;

        /// <summary>
        /// 是否缩放
        /// </summary>
        public static bool IsScale { set; get; } = true;

        /// <summary>
        /// 通过图片的地址新建对象
        /// </summary>
        /// <param name="Path">图片的地址</param>
        public Photograph(string Path) => FilePath = Path;

        public override string ToString() => FilePath;

        /// <summary>
        /// 初始化宽度和高度
        /// </summary>
        /// <param name="Width">窗口可容纳的最大宽度</param>
        /// <param name="Height">窗口可容纳的最大高度</param>
        public static void SetSize(int maxWidth, int maxHeight)
        {
            if (maxWidth <= 0 || maxHeight <= 0)
                throw new ImageSizeException($"Width:{maxWidth} or Height:{maxHeight} is invalid.");

            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        public static void Init(int maxWidth, int maxHeight, Brush backgroundBrush, Func<(int OriginalWidth, int OriginalHeight), int, int, (int ScaleWidth, int ScaleHeight)> getScaleSize)
        {
            SetSize(maxWidth, maxHeight);
            GetScaleSize = getScaleSize;
            BackgroundBrush = backgroundBrush;
        }

        private static int MaxWidth { get; set; }
        private static int MaxHeight { get; set; }

        /// <summary>
        /// 背景刷
        /// </summary>
        private static Brush BackgroundBrush { set; get; }

        /// <summary>
        /// 缩放算法
        /// 必须使用<see cref="MaxWidth"/>和<see cref="MaxHeight"/>获取窗口宽度和高度
        /// </summary>
        private static Func<(int OriginalWidth, int OriginalHeight), int, int, (int ScaleWidth, int ScaleHeight)> GetScaleSize { set; get; }


        /// <summary>
        /// 根据图像的大小新建Bitmap
        /// </summary>
        /// <param name="ImageWidth">图像的宽度</param>
        /// <param name="ImageHeight">图像的长度</param>
        /// <returns></returns>
        /// <exception cref="NotInitPhotographException"></exception>
        public static Bitmap CreateBitmap(int ImageWidth, int ImageHeight)
        {
            if (BackgroundBrush == null)
                throw new NotInitPhotographException("Background Brush isn't inited.");

            Bitmap img = new Bitmap(ImageWidth, ImageHeight);

            using Graphics G = Graphics.FromImage(img);
            G.FillRectangle(BackgroundBrush, new Rectangle(0, 0, ImageWidth, ImageHeight));
            return img;
        }

        /// <summary>
        /// 居中图像
        /// </summary>
        /// <param name="NeedCenteredImage"></param>
        /// <returns></returns>
        /// <exception cref="NotInitPhotographException"></exception>
        public static Bitmap CenteredImage(Bitmap NeedCenteredImage)
        {
            if (MaxWidth <= 0 || MaxHeight <= 0)
                throw new NotInitPhotographException("MaxWidth or MaxHeight isn't inited.");

            Bitmap img = CreateBitmap(MaxWidth, MaxHeight);

            using Graphics G = Graphics.FromImage(img);
            int deltaX = (MaxWidth - NeedCenteredImage.Width) / 2;
            int deltaY = (MaxHeight - NeedCenteredImage.Height) / 2;
            G.DrawImage(NeedCenteredImage, new Rectangle(deltaX, deltaY / 2, NeedCenteredImage.Width, NeedCenteredImage.Height),
                new Rectangle(0, 0, NeedCenteredImage.Width, NeedCenteredImage.Height), GraphicsUnit.Pixel);
            return img;
        }

        /// <summary>
        /// 缩放图像
        /// </summary>
        /// <param name="NeedScaleImage">待缩放的图像</param>
        /// <param name="NewWidth">新的宽度</param>
        /// <param name="NewHeight">新的高度</param>
        /// <returns></returns>
        /// <exception cref="CannotProcessImageException"></exception>
        /// <exception cref="ImageSizeException"></exception>
        /// <exception cref="NotInitPhotographException"></exception>
        public static Bitmap ScaleImage(Bitmap NeedScaleImage)
        {
            if (GetScaleSize == null)
                throw new NotInitPhotographException("GetScaleSize function isn't inited.");

            (int ScaleWidth, int ScaleHeight) = GetScaleSize(
                (OriginalWidth: NeedScaleImage.Width, OriginalHeight: NeedScaleImage.Height), MaxWidth, MaxHeight);
            if (ScaleWidth <= 0 || ScaleHeight <= 0)
                throw new ImageSizeException($"ScaleWidth:{ScaleWidth} or ScaleHeight:{ScaleHeight} is invalid.");

            Bitmap img = new Bitmap(ScaleWidth, ScaleHeight);

            try
            {

                using (Graphics g = Graphics.FromImage(img))
                {
                    // 插值算法的质量
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(NeedScaleImage, new Rectangle(0, 0, ScaleWidth, ScaleHeight),
                        new Rectangle(0, 0, NeedScaleImage.Width, NeedScaleImage.Height), GraphicsUnit.Pixel);
                }
                return img;
            }
            catch
            {
                throw new CannotProcessImageException($"{NeedScaleImage} Can't Scale.");
            }
        }


        /// <summary>
        /// 获得图片对应的Source，用于给Image控件使用
        /// </summary>
        /// <returns>可用于Image.Source</returns>
        /// <exception cref="CannotProcessImageException"></exception>
        /// <exception cref="CannotOpenFileException"></exception>
        public Bitmap GetImageSource()
        {
            lock (typeof(Photograph))
            {
                Bitmap img;
                try
                {
                    img = ReadBitmap(FilePath);
                }
                catch (CannotOpenFileException e)
                {
                    throw e;
                }
                try
                {
                    if (IsScale)
                        img = ScaleImage(img);
                }
                catch (CannotProcessImageException e)
                {
                    throw e;
                }
                if (IsCentered)
                    img = CenteredImage(img);
                return img;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is Photograph p)
            {
                if (this.FilePath == p.FilePath)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
