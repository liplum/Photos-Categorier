using System;
using PhotosCategorier.DataStructure;
using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using PhotosCategorier.Server;

namespace PhotosCategorier.SubWindows
{
    public sealed partial class ClearDuplicatesWindow : Window, INotifyPropertyChanged
    {
        private bool HasDuplicates => duplicates.Count != 0;

        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                if (Math.Abs(_progress - value) < 0.0001) return;
                _progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private const double MaxProgress = 100;

        public ClearDuplicatesWindow(List<Photograph> photographs)
        {
            InitializeComponent();
            this.photographs = photographs;
            progress.Maximum = MaxProgress;
        }

        private readonly List<Photograph> photographs;

        private readonly List<string> duplicates = new();

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

                var per = MaxProgress / (count + 1);


                var multimap = new Multimap<string, string>();
                foreach (var photo in photographs)
                {
                    try
                    {
                        multimap.Add(photo.Md5(), photo.FilePath);
                    }
                    catch
                    {
                        
                    }

                    Progress += per;
                }

                foreach (var item in multimap)
                {
                    var values = item.Value;
                    if (values.Count <= 1) continue;
                    using var iterator = values.GetEnumerator();
                    iterator.MoveNext();
                    while (iterator.MoveNext())
                    {
                        duplicates.Add(iterator.Current);
                    }
                }

                Progress = MaxProgress;
            }
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            if (HasDuplicates)
            {
                await ClearDuplicates();
                MessageBox.Show(Properties.Resources.ClearDuplicatesSuccessfully, Properties.Resources.Success);
                DialogResult = true;
            }

            Close();
        }

        private async Task ClearDuplicates()
        {
            await Task.Factory.StartNew(Run);

            void Run()
            {
                var count = duplicates.Count;
                var per = MaxProgress / (count + 1);
                Progress = 0;

                foreach (var duplicate in duplicates)
                {
                    try
                    {
                        duplicate.DeletedWithException();
                    }
                    catch
                    {
                    }

                    Progress += per;
                }

                Progress = MaxProgress;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckDuplicates();
            UpdateDisplay();
        }
    }

    internal static class PhotoUtil
    {
        public static string Md5(this Photograph photo)
        {
            return HashAlgorithm.Md5(photo.FilePath);
        }
    }
}