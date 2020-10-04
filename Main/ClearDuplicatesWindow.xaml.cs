using PhotosCategorier.Algorithm;
using PhotosCategorier.DataStructure;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static PhotosCategorier.Algorithm.FileTool;

namespace PhotosCategorier.Main
{
    /// <summary>
    /// ClearDuplicatesWindow.xaml 的交互逻辑
    /// </summary>
    public sealed partial class ClearDuplicatesWindow : Window, INotifyPropertyChanged
    {
        private bool HasDuplicates { get => duplicates.Count != 0; }

        private double _Progress;

        public double Progress
        {
            get => _Progress;
            set
            {
                if (_Progress != value)
                {
                    _Progress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private const double MAX_PROGRESS = 100;

        public ClearDuplicatesWindow(List<Photograph> photographs)
        {
            InitializeComponent();
            this.photographs = photographs;
            progress.Maximum = MAX_PROGRESS;
            Cancel.IsEnabled = true;
        }

        private readonly List<Photograph> photographs;

        private readonly List<string> duplicates = new List<string>();

        private void UpdateDisplay()
        {
            var count = duplicates.Count;
            if (count == 0)
            {
                Counter.Content = Properties.Resources.NotFoundDuplicate;
                Cancel.IsEnabled = true;
            }
            else
            {
                Counter.Content = string.Format(Properties.Resources.HowMuchDuplicatesFound, count);
                Counter.FontSize = 15;
                OK.IsEnabled = true;
            }
        }

        private async Task CheckDuplicates()
        {
            await Task.Factory.StartNew(Run);

            void Run()
            {
                var count = photographs.Count;
                Progress = 0;

                var per = MAX_PROGRESS / (count + 1);


                Multimap<string, string> multimap = new Multimap<string, string>();
                foreach (var photo in photographs)
                {
                    multimap.Add(photo.MD5(), photo.FilePath);

                    Progress += per;
                }

                foreach (var item in multimap)
                {
                    var values = item.Value;
                    if (values.Count > 1)
                    {
                        var iterator = values.GetEnumerator();
                        iterator.MoveNext();
                        while (iterator.MoveNext())
                        {
                            duplicates.Add(iterator.Current);
                        }
                    }
                }
                Progress = MAX_PROGRESS;
            }
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            if (HasDuplicates)
            {
                await ClearDuplicates();
                MessageBox.Show(Properties.Resources.ClearDuplicatesSuccessfully, Properties.Resources.Success);
                this.DialogResult = true;
            }
            this.Close();
        }

        private async Task ClearDuplicates()
        {
            await Task.Factory.StartNew(Run);

            void Run()
            {
                var count = duplicates.Count;
                var per = MAX_PROGRESS / (count + 1);
                Progress = 0;

                foreach (var duplcate in duplicates)
                {
                    duplcate.DeleteFileToRecycleBin();
                    Progress += per;
                }
                Progress = MAX_PROGRESS;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckDuplicates();
            UpdateDisplay();
        }
    }
    static class PhotoUtil
    {
        public static string MD5(this Photograph photo)
        {
            return HashAlgorithm.MD5(photo.FilePath);
        }
    }
}
