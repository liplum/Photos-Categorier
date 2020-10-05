﻿using PhotosCategorier.Photo;
using System;

namespace PhotosCategorier.Utils
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

        public static (int ScaleWidth, int ScaleHeight) 比例缩放法((int Width, int Height) ImageSize)
        {
            int 原宽度 = ImageSize.Width,
                原高度 = ImageSize.Height;
            int 最大宽度 = Photograph.MaxWidth,
                最大高度 = Photograph.MaxHeight;
            int 新宽度,
                新高度;

            const double 边距 = 5f / 6f;

            double 宽比高 = (double)原宽度 / 原高度;

            if (最大宽度 > 最大高度)
            {
                获取新宽高(较小的宽或高:最大高度);
            }
            else
            {
                获取新宽高(较小的宽或高:最大宽度);
            }
            return (新宽度, 新高度);

            void 获取新宽高(int 较小的宽或高)
            {
                if (原宽度 > 原高度)
                {
                    新宽度 = 有边距化(最大宽度);
                    新高度 = (int)(新宽度 / 宽比高);
                }
                else if (原高度 > 原宽度)
                {
                    新高度 = 有边距化(最大高度);
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
