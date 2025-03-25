using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using SysWin = System.Windows;
using System.Windows.Controls;
using Thread = System.Windows.Threading;
using Class = WPF_BingeBox.Classes;
using Windows.ApplicationModel.VoiceCommands;
using VlcMediaPlayer = LibVLCSharp.Shared.MediaPlayer;


namespace WPF_BingeBox.Controls
{
    /// <summary>
    /// Interaction logic for PlaybackControls.xaml
    /// </summary>
    public partial class PlaybackControls : UserControl
    {
        //pass down mainwindow from the constructor or do this
        //System.Windows.Application.Current.MainWindow as MainWindow
        public MainWindow MainWindow;

        public VLCControl Parent;
        public VideoView VideoParent;
        public VlcMediaPlayer Player;

        Thread.DispatcherTimer FadeTimer;
        public bool IsFullscreen = false;
        public int CurrentIndex = 0;
        public Thread.DispatcherTimer VideoTimer;
        

        public PlaybackControls()
        {
            InitializeComponent();

            VideoTimer = new Thread.DispatcherTimer();
            VideoTimer.Interval = TimeSpan.FromMilliseconds(100);
            VideoTimer.Tick += VideoTimer_Tick;
            
            //TrackBar.Value = 0;
            //TrackBar.Maximum = 100;

            Parent = this.Parent as VLCControl;

            SetEvents();
        }

        public PlaybackControls(VLCControl parent, MainWindow mainWindow)
        {
            InitializeComponent();

            TrackBar.Value = 0;
            TrackBar.Maximum = 100;

            Parent = parent;
            MainWindow = mainWindow;
            Player = Parent.VideoPlayer.MediaPlayer;

            VideoTimer = new Thread.DispatcherTimer();
            VideoTimer.Interval = TimeSpan.FromMilliseconds(100);
            VideoTimer.Tick += VideoTimer_Tick;

            this.Height = Parent.Height;
            this.Width = Parent.Width;

            SetEvents();
        }

        private void SetEvents()
        {
            FadeTimer = new Thread.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            FadeTimer.Tick += (s, e) =>
            {
                ControlPanel.Visibility = SysWin.Visibility.Collapsed;
                FadeTimer.Stop();
            };

            this.MouseMove += (s, e) =>
            {
                ControlPanel.Visibility = SysWin.Visibility.Visible;
                FadeTimer.Stop();
                FadeTimer.Start();
            };
        }

        public void PlayVideo()
        {
            Parent.VideoPlayer.Content = this;
            Parent.VideoPlayer.MediaPlayer.Play();
            VideoTimer.Start();
        }

        
        private void VideoTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine($"TimeLeft: {Parent.VideoPlayer.MediaPlayer.Time}");

            if (Parent.VideoPlayer.MediaPlayer != null &&
                Parent.VideoPlayer.MediaPlayer.Media.Duration != 0 &&
                Parent.VideoPlayer.MediaPlayer.IsPlaying)
            {
                TrackBar.Maximum = Parent.VideoPlayer.MediaPlayer.Media.Duration;
                TrackBar.Value = Parent.VideoPlayer.MediaPlayer.Time;
            }
                
        }
        public void PrevBtn_Click(object sender, SysWin.RoutedEventArgs e)
        {
            if (CurrentIndex == 0)
                return;

            Parent.VideoPlayer.MediaPlayer.Stop();
            CurrentIndex--;

            Parent.SetNewVideo(CurrentIndex);

            PlayVideo();
        }

        public void PlayBtn_Click(object sender, SysWin.RoutedEventArgs e)
        {
            VlcMediaPlayer player = Parent.VideoPlayer.MediaPlayer;
            if (Parent.PlaylistManager == null)
                return;

            if (Parent.FileManager.FullSeriesList.Count == 0)
                return;

            if(player.Media != null)
            {
                if(player != null && player.Media != null)
                {
                    if (Parent.VideoPlayer.MediaPlayer.IsPlaying)
                    {
                        player.Pause();
                        //VideoTimer.Stop();
                    }
                    else
                    {
                        player.Play();
                        VideoTimer.Start();
                    }
                }
                else
                {
                    Parent.SetNewVideo(CurrentIndex);
                    VideoTimer.Start();
                    player.Play();
                }
            }
            else
            {
                Parent.SetNewVideo(CurrentIndex);
            }
        }

        public void NextBtn_Click(object sender, SysWin.RoutedEventArgs e)
        {
            VlcMediaPlayer player = Parent.VideoPlayer.MediaPlayer;
            player.Stop();
            CurrentIndex++;
            Parent.SetNewVideo(CurrentIndex);
        }

        public void FullScreenBtn_Click(object sender, SysWin.RoutedEventArgs e)
        {
            if (!IsFullscreen)
            {
                if(ControlPanel != null && Parent != null && IsFullscreen != null)
                {
                    MainWindow.MakeFullscreen(this, IsFullscreen, Parent);
                }
            }
            else
            {
                MainWindow.RemoveFullscreen(Parent);
                Parent.PlayerGrid.Height = MainWindow.PreviousPlayerHeight;
                Parent.PlayerGrid.Width = MainWindow.PreviousPlayerWidth;
            }
            IsFullscreen = !IsFullscreen;
        }
    }
}
