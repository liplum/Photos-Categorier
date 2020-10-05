using PhotosCategorier.Layout;
using PhotosCategorier.Main;
using System;
using System.Windows;
using System.Windows.Input;

namespace PhotosCategorier.Main
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
            SetWindowSize();
        }

        private void SettingLanguage_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage();
        }

        private void SettingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            AddClassifyFolderWithSelection(Properties.Resources.SettingClassifyFolder);
        }

        private void AddingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            AddClassifyFolderWithSelection(Properties.Resources.AddingClassifyFolder);
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void MainWin_PreviewDrop(object sender, DragEventArgs e)
        {
            var array = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (array == null)
            {
                e.Handled = true;
            }
            else
            {
                DropFolderOrFile(array);
            }
        }

        private void MainWin_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }


        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.Focus();

            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    {
                        if (!photographs.IsEmpty)
                            SkipThisPhoto();
                        else
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                    }
                    break;
                case Key.S:
                case Key.Down:
                case Key.Delete:
                    {
                        if (!photographs.IsEmpty)
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
                        else if (!photographs.IsEmpty)
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
                        else if (!photographs.IsEmpty)
                            MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                        else
                            MoveThisTo(Arrow.RIGHT_ARROW);
                    }
                    break;
                case Key.F5:
                    {
                        if (allClassifyFolder != null && allClassifyFolder.Count != 0)
                        {
                            Refresh();
                        }
                    }
                    break;
                case Key.C:
                    {
                        Clear();
                    }
                    break;
            }

        }
    }
}