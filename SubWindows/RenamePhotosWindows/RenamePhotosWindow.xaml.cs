using PhotosCategorier.SubWindows.RenamePhotosWindows;
using System.Collections.Generic;
using System.Windows;

namespace PhotosCategorier.SubWindows
{
    /// <summary>
    /// RenamePhotosWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RenamePhotosWindow : Window
    {
        public RenamePhotosWindow(/*List<Photograph> photos*/)
        {
            InitializeComponent();
            //photographs = photos;
            AllPresetListBox.ItemsSource = new List<RuleBox> {
                new RuleBox()
            };
        }

        //private readonly List<Photograph> photographs;


    }
}
