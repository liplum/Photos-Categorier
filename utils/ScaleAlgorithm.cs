using System;

namespace PhotosCategorier.Utils
{
    public static class ScaleAlgorithm
    {
        public static (int ScaleWidth, int ScaleHeight) 归一化缩放法((int Width, int Height) size, int maxWidth, int maxHeight)
        {
            int 原宽度 = size.Width,
                原高度 = size.Height;
            var 长度 = 得到长度();
            var 宽高比 = (double)原宽度 / 原高度;
            double 归一化后的宽度 = 归一化(原宽度),
                    归一化后的高度 = 归一化(原高度);

            if (原宽度 < 原高度)
            {
                var w = 归一化后的宽度 * maxWidth * 2 / 3;
                return ((int)w, (int)(w * (1 / 宽高比)));
            }
            else//此时原图的高度小于宽度
            {
                var h = 归一化后的高度 * maxHeight;
                return ((int)(h * 宽高比), (int)h);
            }

            double 得到长度()
            {
                return Math.Sqrt(原宽度 * 原宽度 + 原高度 * 原高度);
            }

            double 归一化(double 值)
            {
                return 值 / 长度;
            }
        }

        public static (int ScaleWidth, int ScaleHeight) ScaleByRatio((int Width, int Height) size, int maxWidth, int maxHeight)
        {
            int 原宽度 = size.Width,
                原高度 = size.Height;
            int 新宽度,
                新高度;

            const double 边距 = 5f / 6f;

            var 宽比高 = (double)原宽度 / 原高度;

            if (maxWidth > maxHeight)
            {
                获取新宽高(较小的宽或高: maxHeight);
            }
            else
            {
                获取新宽高(较小的宽或高: maxWidth);
            }
            return (新宽度, 新高度);

            void 获取新宽高(int 较小的宽或高)
            {
                if (原宽度 > 原高度)
                {
                    新宽度 = 有边距化(maxWidth);
                    新高度 = (int)(新宽度 / 宽比高);
                }
                else if (原高度 > 原宽度)
                {
                    新高度 = 有边距化(maxHeight);
                    新宽度 = (int)(新高度 * 宽比高);
                }
                else
                {
                    新高度 = 新宽度 = 有边距化(较小的宽或高);
                }

                static int 有边距化(int 需要添加边距的高或宽)
                {
                    return (int)(需要添加边距的高或宽 * 边距);
                }
            }
        }
    }
}
