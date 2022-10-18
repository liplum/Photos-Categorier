using PhotosCategorier.Layout;
using PhotosCategorier.SubWindows;
using System.Windows;

namespace PhotosCategorier.Main
{
    public sealed partial class MainWindow
    {
        private void ClearDuplicates()
        {
            var r = MessageBox.Show(Properties.Resources.ConfirmClearingDuplicates, Properties.Resources.Warnning, MessageBoxButton.OKCancel);
            if (r == MessageBoxResult.OK)
            {
                if (CheckEmptyWithMessage(EmptyMessage.NotSetClassify))
                {
                    return;
                }
                Refresh();
                var window = new ClearDuplicatesWindow(photographs.AllPhotographs);
                window.ShowDialog();
                Refresh();
            }
        }
        private void SetWindowSize()
        {
            new SetSizeWindow().ShowDialog();
            ResetSize(layout.LayoutType.GetWidth(), layout.LayoutType.GetHeight());
        }

        private static void SetLanguage()
        {
            var res = new SetLanguageWindow().ShowDialog();
        }
    }
}