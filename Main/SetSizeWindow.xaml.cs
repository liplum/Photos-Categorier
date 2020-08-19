using System;
using System.Windows;
using static PhotosCategorier.Layout.LayoutSize;

namespace PhotosCategorier
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
        }

        private readonly Layout.Layout curLayout;
        private LayoutType[] layoutTypes;

        private void InitCombo()
        {
            var all = Enum.GetValues(typeof(LayoutType));
            int len = all.Length;
            layoutTypes = new LayoutType[len];
            for (int i = 0; i < len; ++i)
            {
                layoutTypes[i] = (LayoutType)all.GetValue(i);
            }
            foreach (LayoutType t in layoutTypes)
            {
                SizeSelcet.Items.Add(t.GetName());
            }
            SizeSelcet.SelectedItem = curLayout.LayoutType.GetName();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            int index = SizeSelcet.SelectedIndex;
            if (index >= 0 && index < layoutTypes.Length)
                curLayout.LayoutType = layoutTypes[index];
            this.Close();
        }
    }
}
