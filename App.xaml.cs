using System.Windows;

namespace PhotosCategorier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string Language_default = "default";
        public const string Language_zh = "zh";
        public const string Language_en = "en";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            switch (PhotosCategorier.Properties.Settings.Default.Language)
            {
                case Language_default:
                    break;
                case Language_zh:
                    PhotosCategorier.Properties.Resources.Culture = new System.Globalization.CultureInfo("zh");
                    break;
                case Language_en:
                    PhotosCategorier.Properties.Resources.Culture = new System.Globalization.CultureInfo("en");
                    break;
                default:
                    break;
            }
        }

        public void ReloadLanguage()
        {
            PhotosCategorier.Properties.Resources.Culture = new System.Globalization.CultureInfo(PhotosCategorier.Properties.Settings.Default.Language);
        }
    }
}
