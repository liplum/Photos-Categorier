using System.Windows;

namespace PhotosCategorier.Main
{
    /// <summary>
    /// SetLanguageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetLanguageWindow : Window
    {
        public SetLanguageWindow()
        {
            InitializeComponent();
            InitCombo();
        }



        private void InitCombo()
        {
            //LanguageSelcet.Items.Add(App.Language_en);
            //LanguageSelcet.Items.Add(App.Language_zh);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
