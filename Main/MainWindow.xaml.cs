﻿using PhotosCategorier.Algorithm;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;

namespace PhotosCategorier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Layout.Layout layout;
        public MainWindow()
        {
            InitializeComponent();
            layout = (Layout.Layout)App.Current.Resources["CurLayout"];
            InitPhotograph();
        }
        public void InitPhotograph()
        {
            Photograph.GetScaleSize = ScaleAlgorithm.折半缩放法;
            Photograph.BackgroundBrush = BackgroundBrush;
            Photograph.SetSize(layout.WINDOW_WIDTH, layout.WINDOW_HEIGHT);
        }

        private List<Photograph> photographs;
        private int curPhoto = 0;

        private DirectoryInfo leftArrow, rightArrow;

        private SolidBrush BackgroundBrush = new SolidBrush(Color.FromArgb(122, 240, 240, 240));

        private void ResetSize(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            Photograph.SetSize(Width, Height);
            UpdateImage();
        }
    }
}
