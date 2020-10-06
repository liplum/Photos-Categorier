using PhotosCategorier.Layout;
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
                if (photographs.IsEmpty)
                {
                    MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
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
            if (res == true)
            {
                MessageBox.Show(Properties.Resources.PleaseReopen, Properties.Resources.Tip);
            }
        }
    }
}