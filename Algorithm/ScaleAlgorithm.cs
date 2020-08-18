using System;

namespace PhotosCategorier.Algorithm
{
    public static class ScaleAlgorithm
    {
        public static (int ScaleWidth, int ScaleHeight) 折半缩放法((int Width, int Height) ImageSize)
        {
            int 缩放后宽度 = ImageSize.Width,
                缩放后高度 = ImageSize.Height;
            int 最大宽度 = Photograph.MaxWidth,
                最大高度 = Photograph.MaxHeight;

            if (缩放后宽度 > 最大宽度 || 缩放后高度 > 最大高度)
            {
                while (缩放后宽度 > 最大宽度 || 缩放后高度 > 最大高度)
                {
                    缩放后宽度 = 缩放后宽度 * 2 / 3;
                    缩放后高度 = 缩放后高度 * 2 / 3;
                }
            }
            else if (缩放后宽度 < 最大宽度 || 缩放后高度 < 最大高度)
            {
                while (缩放后宽度 > 最大宽度 || 缩放后高度 > 最大高度)
                {
                    缩放后宽度 = 缩放后宽度 * 3 / 2;
                    缩放后高度 = 缩放后高度 * 3 / 2;
                }
            }
            return (缩放后宽度, 缩放后高度);
        }

        public static (int ScaleWidth, int ScaleHeight) 归一化缩放法((int Width, int Height) ImageSize)
        {
            int 原宽度 = ImageSize.Width,
                原高度 = ImageSize.Height;
            var 长度 = 得到长度();
            double 宽高比 = (double)原宽度 / 原高度;
            double 归一化后的宽度 = 归一化(原宽度),
                    归一化后的高度 = 归一化(原高度);

            if (原宽度 < 原高度)
            {
                var w = 归一化后的宽度 * Photograph.MaxWidth * 2 / 3;
                return ((int)w, (int)(w * (1 / 宽高比)));
            }
            else//此时原图的高度小于宽度
            {
                var h = 归一化后的高度 * Photograph.MaxHeight;
                return ((int)(h * 宽高比), (int)h);
            }

            double 得到长度() => Math.Sqrt(原宽度 * 原宽度 + 原高度 * 原高度);

            double 归一化(double 值) => 值 / 长度;
        }
    }
}
