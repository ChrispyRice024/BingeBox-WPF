using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_BingeBox.Managers;
using WPF_BingeBox.Models;

namespace WPF_BingeBox
{
    /// <summary>
    /// Interaction logic for AddSeries.xaml
    /// </summary>
    public partial class AddSeries : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private OpenFolderDialog _folderDialog;
        private FileManager _fileManager;

        private Series NewSeries;
        private bool IsEpisodic;
        private string _seriesName;
        private string _location;
        private string _loadingTxt;

        public string SeriesName
        {
            get { return _seriesName; }
            set
            {
                _seriesName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesName"));
            }
        }
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Location"));
            }
        }
        public string LoadingTxt
        {
            get { return _loadingTxt; }
            set
            {
                _loadingTxt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LoadingTxt"));
            }
        }

        public AddSeries(FileManager fileManager)
        {
            DataContext = this;
            InitializeComponent();
            _fileManager = fileManager;
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            _folderDialog = new OpenFolderDialog();
            _folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            _folderDialog.ShowDialog();

            string selectedFolder = _folderDialog.FolderName;

            Location = selectedFolder;
        }

        private async void AddSeriesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var progress = new Progress<int>(value =>
                {
                    ProgressBar.Value = value;
                    LoadingTxt = $"{value}% Completed";
                });
                NewSeries = await Series.SeriesCon(_seriesName, IsEpisodic, Location, progress);

                _fileManager.SaveToFile(NewSeries, FileManager.SeriesFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"New Exception When Creating Series: {ex}");
            }
        }

        private void IsEpisodicChkBx_Click(object sender, RoutedEventArgs e)
        {
            IsEpisodic = IsEpisodicChkBx.IsChecked ?? false;
        }
    }
}
