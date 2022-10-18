using PhotosCategorier.Main.Exceptions;
using System;
using System.Windows;
using System.Windows.Input;

namespace PhotosCategorier.Main
{
    public partial class MainWindow
    {
        private void LeftArrowPointedToFolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (leftArrow != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", leftArrow.FullName);
            }
            e.Handled = true;
        }

        private void RightArrowPointedToFolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rightArrow != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", rightArrow.FullName);
            }
            e.Handled = true;
        }

        private void DeleteThis_Click(object sender, RoutedEventArgs e)
        {
            DeleteThisPhoto();
            e.Handled = true;
        }

        private void SelectLeftFolder_Click(object sender, RoutedEventArgs e)
        {
            SetLeftFolderWithSelection();
            e.Handled = true;
        }

        private void SelectRightFolder_Click(object sender, RoutedEventArgs e)
        {
            SetRightFolderWithSelection();
            e.Handled = true;
        }

        private void SettingWindowSize_Click(object sender, RoutedEventArgs e)
        {
            SetWindowSize();
            e.Handled = true;
        }

        private void SettingLanguage_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage();
            e.Handled = true;
        }

        private void SettingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddClassifyFolderWithSelection(Properties.Resources.SettingClassifyFolder);
            }
            catch (NotHoldPhotoException)
            {
                MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void AddingClassifyFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddClassifyFolderWithSelection(Properties.Resources.AddingClassifyFolder);
            }
            catch (NotHoldPhotoException)
            {
                MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void ClearingDuplicates_Click(object sender, RoutedEventArgs e)
        {
            ClearDuplicates();
            e.Handled = true;
        }

        private void ToLeft_Click(object sender, RoutedEventArgs e)
        {
            MoveThisTo(Arrow.LeftArrow);
            e.Handled = true;
        }

        private void ToRight_Click(object sender, RoutedEventArgs e)
        {
            MoveThisTo(Arrow.RightArrow);
            e.Handled = true;
        }

        private void SkipThis_Click(object sender, RoutedEventArgs e)
        {
            SkipThisPhoto();
            e.Handled = true;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            e.Handled = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            e.Handled = true;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenThisPhoto();
            }
            catch (FileHasOccupiedOrBeenDeletedException)
            {
                MessageBox.Show(Properties.Resources.FileHasOccupiedOrBeenDeleted, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void OpenFolderWhereFileIs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFolderWherePhotoIs();
            }
            catch (FileHasOccupiedOrBeenDeletedException)
            {
                MessageBox.Show(Properties.Resources.FileHasOccupiedOrBeenDeleted, Properties.Resources.Error);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(Properties.Resources.DirectoryNotFound, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void CopyFile_Click(object sender, RoutedEventArgs e)
        {
            CopyCurrent();
            e.Handled = true;
        }


        private void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = allClassifyFolder.Count != 0;
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Refresh();
            e.Handled = true;
        }

        private void ClearCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !photographs.IsEmpty;
        }

        private void ClearCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clear();
            e.Handled = true;
        }

        private void SetLeftArrow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetLeftArrow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetLeftFolderWithSelection();
            e.Handled = true;
        }

        private void SetRightArrow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetRightArrow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetRightFolderWithSelection();
            e.Handled = true;
        }

        private void OpenFolderWhereFileIs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsEnd;
        }

        private void OpenFolderWhereFileIs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFolderWherePhotoIs();
            }
            catch (FileHasOccupiedOrBeenDeletedException)
            {
                MessageBox.Show(Properties.Resources.FileHasOccupiedOrBeenDeleted, Properties.Resources.Error);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(Properties.Resources.DirectoryNotFound, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void OpenFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsEnd;
        }

        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenThisPhoto();
            }
            catch (FileHasOccupiedOrBeenDeletedException)
            {
                MessageBox.Show(Properties.Resources.FileHasOccupiedOrBeenDeleted, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void CopyFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsEnd;
        }

        private void CopyFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                CopyCurrent();
            }
            catch (FileHasOccupiedOrBeenDeletedException)
            {
                MessageBox.Show(Properties.Resources.FileHasOccupiedOrBeenDeleted, Properties.Resources.Error);
            }

            e.Handled = true;
        }

        private void DisplayArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DisplayArea_Drop(object sender, DragEventArgs e)
        {
            if ((e.Effects & DragDropEffects.Link) != 0)

            {
                var array = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (!(array is null))
                {
                    try
                    {
                        DropFolderOrFile(array);
                    }
                    catch (NotHoldPhotoException)
                    {
                        MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                    }
                }
            }

            e.Handled = true;
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            Focus();

            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    {
                        if (!CheckEmptyWithMessage(EmptyMessage.NotSetClassify))
                        {
                            SkipThisPhoto();
                        }
                    }
                    break;
                case Key.S:
                case Key.Down:
                case Key.Delete:
                    {
                        if (!CheckEmptyWithMessage(EmptyMessage.NotSetClassify))
                        {
                            DeleteThisPhoto();
                        }
                    }
                    break;
                case Key.A:
                case Key.Left:
                    {

                        if (leftArrow == null)
                        {
                            MessageBox.Show(Properties.Resources.NotPointLeft, Properties.Resources.Error);
                        }
                        else if (!CheckEmptyWithMessage(EmptyMessage.NotSetClassify))
                        {
                            MoveThisTo(Arrow.LeftArrow);
                        }
                    }
                    break;
                case Key.D:
                case Key.Right:
                    {
                        if (rightArrow == null)
                        {
                            MessageBox.Show(Properties.Resources.NotPointRight, Properties.Resources.Error);
                        }
                        else if (!CheckEmptyWithMessage(EmptyMessage.NotSetClassify))
                        {
                            MoveThisTo(Arrow.RightArrow);
                        }
                    }
                    break;
            }
            e.Handled = true;
        }
    }
}