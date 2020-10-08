using System;
using System.Windows;
using static PhotosCategorier.Layout.LayoutSize;

namespace PhotosCategorier.SubWindows
{
    /// <summary>
    /// SetSizeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetSizeWindow : Window
    {
        public SetSizeWindow()
        {
            InitializeComponent();
            curLayout = (Layout.Layout)App.Current.Resources["CurLayout"];
            InitCombo();
            InitButton();
        }

        private void InitButton()
        {
            SelectSize.OK.Click += OK_Click;
            SelectSize.Cancel.Click += Cancel_Click;
        }

        private readonly Layout.Layout curLayout;
        private LayoutType[] layoutTypes;

        private void InitCombo()
        {
            var all = Enum.GetValues(typeof(LayoutType));
            int len = all.Length;
            layoutTypes = new LayoutType[len];
            var names = new string[len];
            for (int i = 0; i < len; ++i)
            {
                layoutTypes[i] = (LayoutType)all.GetValue(i);
                names[i] = layoutTypes[i].GetName();
            }
            var combo = SelectSize.ComboBox;
            combo.ItemsSource = names;
            combo.SelectedItem = curLayout.LayoutType.GetName();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var combo = SelectSize.ComboBox;
            int index = combo.SelectedIndex;
            if (index >= 0 && index < layoutTypes.Length)
                curLayout.LayoutType = layoutTypes[index];
            this.Close();
        }
    }
}
