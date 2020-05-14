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
                    缩放后宽度 /= 2;
                    缩放后高度 /= 2;
                }
            }
            else if (缩放后宽度 < 最大宽度 || 缩放后高度 < 最大高度)
            {
                while (缩放后宽度 > 最大宽度 || 缩放后高度 > 最大高度)
                {
                    缩放后宽度 *= 2;
                    缩放后高度 *= 2;
                }
            }
            return (缩放后宽度, 缩放后高度);
        }
    }
}
