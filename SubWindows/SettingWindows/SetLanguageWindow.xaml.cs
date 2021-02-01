using System.Windows;
using System.Windows.Controls;

namespace PhotosCategorier.SubWindows
{
    /// <summary>
    /// SetLanguageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetLanguageWindow : Window
    {
        public SetLanguageWindow()
        {
            InitializeComponent();
            selector = SelectLanguage.ComboBox;
            InitButton();
            InitCombo();
        }

        private readonly ComboBox selector;

        private void InitButton()
        {
            SelectLanguage.OK.Click += OK_Click;
            SelectLanguage.Cancel.Click += Cancel_Click;
        }

        private static readonly string[] Languages = { App.Language_default, App.Language_en, App.Language_zh };

        private void InitCombo()
        {
            selector.ItemsSource = Languages;
            selector.SelectedItem = Properties.Settings.Default.Language;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var selected = selector.SelectedItem;
            DialogResult = App.SetLanguage(selected.ToString());
            Close();
        }
    }

}
