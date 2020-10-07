using PhotosCategorier.Photo;
using PhotosCategorier.Render;
using PhotosCategorier.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PhotosCategorier.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
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
        [NotNull]
        private readonly PhotographsGenerator photographs = new PhotographsGenerator();
        [NotNull]
        private List<Album> allClassifyFolder = new List<Album>();
        [NotNull]
        private DoubleBufferRenderer renderer;

        private DirectoryInfo leftArrow, rightArrow;

        private readonly SolidBrush BackgroundBrush = new SolidBrush(Color.FromArgb(122, 240, 240, 240));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
