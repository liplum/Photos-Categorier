using PhotosCategorier.Photo;
using PhotosCategorier.Render;
using PhotosCategorier.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;

namespace PhotosCategorier.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly Layout.Layout layout;
        public MainWindow()
        {
            InitializeComponent();
            layout = (Layout.Layout)App.Current.Resources["CurLayout"];
            InitPhotograph();
        }
        public void InitPhotograph()
        {
            renderer = new DoubleBufferRenderer(photographs);
            Photograph.Init(layout.WINDOW_WIDTH, layout.WINDOW_HEIGHT, BackgroundBrush, ScaleAlgorithm.比例缩放法);
        }

        private readonly PhotographsGenerator photographs = new PhotographsGenerator();

        private List<Album> allClassifyFolder = new List<Album>();

        private DoubleBufferRenderer renderer;

        private DirectoryInfo leftArrow, rightArrow;

        private readonly SolidBrush BackgroundBrush = new SolidBrush(Color.FromArgb(122, 240, 240, 240));

        public event PropertyChangedEventHandler PropertyChanged;

        private void ResetSize(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            App.SetWidth_Height(Width, Height);
            Photograph.SetSize(Width, Height);
            if (photographs.HasNext)
            {
                UpdateImage();
            }
        }
    }
}
