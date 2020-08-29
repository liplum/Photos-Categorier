using PhotosCategorier.Layout;
using PhotosCategorier.Main;
using System.Windows;
using System.Windows.Input;

namespace PhotosCategorier
{
    public partial class MainWindow
    {
        private void LeftArrowPointedToFolder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (leftArrow != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", leftArrow.FullName);
            }
        }

        private void RightArrowPointedToFolder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (rightArrow != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", rightArrow.FullName);
            }
        }

        private void DeleteThis_Click(object sender, RoutedEventArgs e)
        {
            DeleteThisPhoto();
        }

        private void SelectLeftFolder_Click(object sender, RoutedEventArgs e)
        {
            SetLeftFolder();
        }

        private void SelectRightFolder_Click(object sender, RoutedEventArgs e)
        {
            SetRightFolder();
        }

        private void SettingWindowSize_Click(object sender, RoutedEventArgs e)
        {
            new SetSizeWindow().ShowDialog();
            ResetSize(layout.LayoutType.GetWidth(), layout.LayoutType.GetHeight());
        }

        private void SettingLanguage_Click(object sender, RoutedEventArgs e)
        {
            new SetLanguageWindow().ShowDialog();
        }

        private void SettingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            SetClassifyFolder();
        }

        private void AddingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            AddClassifyFolder();
        }

        private void ClearingDuplicates_Click(object sender, RoutedEventArgs e)
        {
            ClearDuplicates();
        }

        private void ToLeft_Click(object sender, RoutedEventArgs e)
        {
            MoveThisTo(Arrow.LEFT_ARROW);
        }

        private void ToRight_Click(object sender, RoutedEventArgs e)
        {
            MoveThisTo(Arrow.RIGHT_ARROW);
        }

        private void SkipThis_Click(object sender, RoutedEventArgs e)
        {
            SkipThisPhoto();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    {
                        if (photographs != null)
                            SkipThisPhoto();
                        else
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                    }
                    break;
                case Key.S:
                case Key.Down:
                    {
                        if (photographs != null)
                            DeleteThisPhoto();
                        else
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                    }
                    break;
                case Key.A:
                case Key.Left:
                    {

                        if (leftArrow == null)
                            MessageBox.Show(Properties.Resources.NotPointLeft, Properties.Resources.Error);
                        else if (photographs == null)
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                        else
                            MoveThisTo(Arrow.LEFT_ARROW);

                    }
                    break;
                case Key.D:
                case Key.Right:
                    {
                        if (rightArrow == null)
                            MessageBox.Show(Properties.Resources.NotPointRight, Properties.Resources.Error);
                        else if (photographs == null)
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                        else
                            MoveThisTo(Arrow.RIGHT_ARROW);
                    }
                    break;
                case Key.F5:
                    {
                        if(allClassifyFolder != null && allClassifyFolder.Count != 0)
                        {
                            Refresh();
                        }
                    }
                    break;
            }
        }
    }
}
