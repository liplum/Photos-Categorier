using PhotosCategorier.Layout;
using System.Windows;
using static PhotosCategorier.Layout.LayoutSize;

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

        public App()
        {
             
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CheckLanguage();
            CheckLayout();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveProperties();
        }

        private void SaveProperties()
        {
            PhotosCategorier.Properties.Settings.Default.Save();
        }

        private static void CheckLanguage()
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

        private void CheckLayout()
        {
            var layout = (Layout.Layout)Current.Resources["CurLayout"];
            var settings = PhotosCategorier.Properties.Settings.Default;
            if (settings.IsFirstOpen)
            {
                settings.Width = layout.LayoutType.GetWidth();
                settings.Height = layout.LayoutType.GetHeight();
                settings.IsFirstOpen = false;
            }
            else
            {
                try
                { 
                    var settingLayout = LayoutSize.GenerateLayout(settings.Width, settings.Height);
                    layout.SetLayout(settingLayout);
                }
                catch (GenrateLayoutFailedException)
                {
                    ;
                }
            }

        }

        public static bool SetWidth_Height(int width,int height)
        {
            if (width <= 0 || height <= 0)
            {
                return false;
            }
            var settings = PhotosCategorier.Properties.Settings.Default;
            settings.Width = width;
            settings.Height = height;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lan"></param>
        /// <returns>It returns True when it sucessfully sets language .It returns False only when the original setting is the same as new setting.</returns>
        public static bool SetLanguage(string lan)
        {
            var settings = PhotosCategorier.Properties.Settings.Default;
            if (!settings.Language.Equals(lan))
            {
                settings.Language = lan;
                return true;
            }
            return false;
        }
    }
}
