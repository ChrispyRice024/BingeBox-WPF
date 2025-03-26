using LibVLCSharp.Platforms.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VlcMediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using WPF_BingeBox.Controls;
using WPF_BingeBox.Managers;
using WPF_BingeBox.Models;
using System.IO;

namespace WPF_BingeBox;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public FileManager FileManager;
    public PlaylistManager PlaylistManager;

    public double PreviousPlayerHeight;
    public double PreviousPlayerWidth;

    public double PreviousWindowHeight;
    public double PreviousWindowWidth;

    private ObservableCollection<Episode> _playlist;

    public ObservableCollection<Episode> Playlist
    {
        get { return _playlist; }
        set
        {
            _playlist = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlist)));
        }
    }
    public MainWindow()
    {
        FileManager = new FileManager();
        PlaylistManager = new PlaylistManager(FileManager);

        this.KeyDown += (s, e) =>
        {
            if(e.Key == Key.Escape && VideoControl.VideoControls.IsFullscreen)
            {
                RemoveFullscreen(VideoControl);
            }
        };

        Playlist = PlaylistManager.Playlist;

        PlaylistManager.PropertyChanged += (s, e) =>
        {
            Playlist = PlaylistManager.Playlist;
            PlaylistControl.PlaylistLst.Items.Refresh();
        };

        //DataContext = this;
        InitializeComponent();
    }



    private void PlaylistControl_Loaded(object sender, RoutedEventArgs e)
    {
        PlaylistControl.IPlaylistManager = PlaylistManager;

        PlaylistControl.DataContext = this;
        PlaylistControl.Parent = this;

        //PlaylistControl.IPlaylistManager.Playlist.CollectionChanged += (s, e) =>
        //{
        //    PlaylistControl.PlaylistLst.Items.Refresh();
        //};
        //PlaylistManager.PopulatePlaylist();
    }

    private void ToolBar_Loaded(object sender, RoutedEventArgs e)
    {
        ToolBar.FileManager = FileManager;
    }
    private void VideoControl_Loaded(object sender, RoutedEventArgs e)
    {
        VideoControl.PlaylistManager = PlaylistManager;
        VideoControl.FileManager = FileManager;
        
    }

    public void MakeFullscreen(PlaybackControls controlPanel, bool isFullscreen, VLCControl control)
    {
        PreviousPlayerHeight = control.Height;
        PreviousPlayerWidth = control.Width;

        PreviousWindowHeight = this.Height;
        PreviousWindowWidth = this.Width;

        this.WindowStyle = WindowStyle.None;
        this.WindowState = WindowState.Maximized;

        //Grid.SetRowSpan(VideoControl.VideoPlayer, 2);

        VideoControlWrapper.Children.Remove(VideoControl);
        VideoControlWrapper.UpdateLayout();

        OuterGrid.Children.Add(VideoControl);
        //VideoControl.VideoPlayer.Content = controlPanel;
        controlPanel.Height = this.Height;
        controlPanel.Width = this.Width;

        Grid.SetZIndex(OuterGrid, 11);
        OuterGrid.Background = Brushes.Black;
        VideoControl.VideoPlayer.Background = Brushes.Black;
        OuterGrid.UpdateLayout();
    }
    public void RemoveFullscreen(VLCControl control)
    {
        if(OuterGrid.Children.Count > 0)
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;

            control.Height = PreviousPlayerHeight;
            control.Width = PreviousPlayerWidth;

            this.Height = PreviousWindowHeight;
            this.Width = PreviousWindowWidth;

            OuterGrid.Children.Remove(VideoControl);
            OuterGrid.Background = Brushes.Transparent;

            VideoControl.VideoPlayer.Content = control.VideoControls;

            VideoControlWrapper.Children.Add(VideoControl);

            Grid.SetZIndex(OuterGrid, 0);
            //Grid.SetZIndex(VideoControlWrapper, 8);
            //Grid.SetZIndex(PlaylistParent, 9);
            //Grid.SetZIndex(ToolBar, 10);
        }
        this.UpdateLayout();
    }
}