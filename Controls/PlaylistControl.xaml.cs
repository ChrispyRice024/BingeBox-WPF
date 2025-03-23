using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_BingeBox.Models;
using WPF_BingeBox.Managers;

namespace WPF_BingeBox.Controls
{
    /// <summary>
    /// Interaction logic for PlaylistControl.xaml
    /// </summary>
    public partial class PlaylistControl : UserControl
    {

        public PlaylistManager IPlaylistManager;

        //public ObservableCollection<Episode> Playlist;

        public MainWindow Parent;


        public PlaylistControl()
        {
            Parent = System.Windows.Application.Current.MainWindow as MainWindow;
            this.DataContext = Parent;

            InitializeComponent();



            //if(Parent != null)
            //{
            //    this.DataContext = Parent;
            //}
            //else
            //{
            //    Debug.WriteLine("Parent Window is null in PlaylistControl");
            //}

        }

        private void PlaylistLst_Loaded(object sender, RoutedEventArgs e)
        {
            IPlaylistManager.PopulatePlaylist(Parent.FileManager.FullSeriesList);
            foreach (Episode item in Parent.Playlist)
            {
                Debug.WriteLine($"Episode Name: {item.EpisodeTitle}");
            }
        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            IPlaylistManager.PopulatePlaylist(Parent.FileManager.FullSeriesList);
            foreach (Episode item in Parent.Playlist)
            {
                Debug.WriteLine($"Episode Name: {item.EpisodeTitle}");
            }
        }
    }
}
