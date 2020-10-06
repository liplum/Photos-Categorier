using System.Drawing;

namespace PhotosCategorier.Render
{
    public interface IRenderer
    {
        Bitmap GetCurrentRender();
        Bitmap Init();
    }
}
