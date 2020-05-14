using System;
using System.ComponentModel;
using System.Reflection;
using static PhotosCategorier.Layout.LayoutSize;

namespace PhotosCategorier.Layout
{
    public class Layout : INotifyPropertyChanged
    {
        public int WINDOW_WIDTH
        {
            get
            {
                return LayoutType.GetWidth();
            }
        }

        public int WINDOW_HEIGHT
        {
            get
            {
                return LayoutType.GetHeight();
            }
        }

        private LayoutType layoutType;
        public LayoutType LayoutType
        {
            get => layoutType;
            set
            {
                if (layoutType != value)
                {
                    layoutType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                }
            }
        }

        public float FontSize
        {
            get => LayoutType.GetFontSize();
        }

        public float ItemFontSize
        {
            get => FontSize * 7 / 10;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void SetLayout(LayoutType type = LayoutType.Size1080x720) => layoutType = type;

        public Layout() => SetLayout();
    }

    public static class LayoutSize
    {
        public enum LayoutType
        {
            [Size(2560, 1440), FontSize(35)]
            Size2560x1440,
            [Size(1920, 1440), FontSize(32)]
            Size1920x1440,
            [Size(1920, 1080), FontSize(30)]
            Size1920x1080,
            [Size(1660, 1200), FontSize(28)]
            Size1660x1200,
            [Size(1600, 900), FontSize(27)]
            Size1600x900,
            [Size(1280, 960), FontSize(26)]
            Size1280x960,
            [Size(1080, 720), FontSize(25)]
            Size1080x720,
            [Size(960, 640), FontSize(23)]
            Size960x640,
            [Size(720, 480), FontSize(22)]
            Size720x480,
            [Size(640, 480), FontSize(20)]
            Size640x480,
        }
        /// <summary>
        /// 获得当前的宽度
        /// </summary>
        /// <param name="type">LayoutType中定义的各种窗口尺寸</param>
        /// <returns></returns>
        /// <exception cref="NotDefineException"></exception>
        public static int GetWidth(this LayoutType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            object[] attributes = fi.GetCustomAttributes(typeof(Size), false);
            if (attributes.Length > 0)
            {
                return ((Size)attributes[0]).Width;
            }
            throw new NotDefineException($"{type} undefine Width");
        }
        /// <summary>
        /// 获得当前的高度
        /// </summary>
        /// <param name="type">LayoutType中定义的各种窗口尺寸</param>
        /// <returns></returns>
        /// <exception cref="NotDefineException"></exception>
        public static int GetHeight(this LayoutType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            object[] attributes = fi.GetCustomAttributes(typeof(Size), false);
            if (attributes.Length > 0)
            {
                return ((Size)attributes[0]).Height;
            }
            throw new NotDefineException($"{type} undefine Height");
        }

        /// <summary>
        /// 获得当前的字体大小
        /// </summary>
        /// <param name="type">LayoutType中定义的各种窗口尺寸</param>
        /// <returns></returns>
        /// <exception cref="NotDefineException"></exception>
        public static float GetFontSize(this LayoutType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            object[] attributes = fi.GetCustomAttributes(typeof(FontSize), false);
            if (attributes.Length > 0)
            {
                return ((FontSize)attributes[0]).Size;
            }
            throw new NotDefineException($"{type} undefine FontSize");
        }
        /// <summary>
        /// 获得当前的完整名称
        /// </summary>
        /// <param name="type">LayoutType中定义的各种窗口尺寸</param>
        /// <returns></returns>
        /// <exception cref="NotDefineException"></exception>
        public static string GetName(this LayoutType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            object[] attributes = fi.GetCustomAttributes(typeof(Size), false);
            if (attributes.Length > 0)
            {
                var size = (Size)attributes[0];
                return $"{size.Width}x{size.Height}";
            }
            throw new NotDefineException($"{type} undefine Size");
        }

        public class Size : Attribute
        {
            public int Width { set; get; }
            public int Height { set; get; }

            public Size(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }

        public class FontSize : Attribute
        {
            public float Size { set; get; }
            public FontSize(float size) => Size = size;
        }

        public class NotDefineException : Exception
        {
            public NotDefineException(string message) : base(message) { }
        }
    }
}
