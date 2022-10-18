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

namespace PhotosCategorier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : INotifyPropertyChanged
    {
        private readonly Layout.Layout layout;

        public MainWindow()
        {
            InitializeComponent();
            layout = (Layout.Layout)App.Current.Resources["CurLayout"];
            InitPhotograph();
        }

        private void InitPhotograph()
        {
            renderer = new DoubleBufferRenderer(photographs);
            Photograph.Init(layout.WINDOW_WIDTH, layout.WINDOW_HEIGHT, backgroundBrush, ScaleAlgorithm.ScaleByRatio);
        }

        private readonly PhotographsGenerator photographs = new();

        private List<FileInfo> allImages = new();
        private ImageList imageList = new();
        private IRenderer renderer;

        private DirectoryInfo? leftArrow, rightArrow;

        private readonly SolidBrush backgroundBrush = new(Color.FromArgb(122, 240, 240, 240));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            App.SetWidth_Height(width, height);
            Photograph.SetSize(width, height);
            if (photographs.HasNext)
            {
                UpdateImage();
            }
        }
    }
}