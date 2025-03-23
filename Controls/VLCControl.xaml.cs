﻿using WPF_BingeBox.Managers;
using LibVLCSharp.Shared;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Class = WPF_BingeBox.Classes;
using Input = System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;
using VlcMediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using Windows.Storage;

namespace WPF_BingeBox.Controls
{
    /// <summary>
    /// Interaction logic for VLCControl.xaml
    /// </summary>
    public partial class VLCControl : UserControl
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private static MainWindow _mainWindow;
        public IntPtr hwnd;

        public VlcMediaPlayer Player;
        public LibVLC libVlc;
        public Media Video;

        public PlaylistManager PlaylistManager;
        public FileManager FileManager;
        

        public VLCControl()
        {
            InitializeComponent();
            InitializeLibVLC();
            
            _mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            
            VideoControls.Tag = VideoPlayer;
            VideoControls.Loaded += (s, e) =>
            {
                VideoControls.Parent = this;
                VideoControls.MainWindow = _mainWindow;
                VideoControls.Height = this.Height;
                VideoControls.Width = this.Width;
            };
            
            hwnd = new WindowInteropHelper(_mainWindow).Handle;
        }

        private void InitializeLibVLC()
        {
            //, "--avcodec-hw=none"
            libVlc = new LibVLC("--verbose=2");
            Player = new VlcMediaPlayer(libVlc);
            Player.Volume = 100;
            Player.MediaChanged += Player_MediaChanged;

            Player.EncounteredError += (s, e) =>
            {
                Debug.WriteLine($"Encountered Error in VLC Media Player: {e}");
            };

            VideoPlayer.MediaPlayer = Player;
        }

        public void SetNewVideo(int index)
        {
            Video = new Media(libVlc,
                new Uri(PlaylistManager.Playlist[index].EpisodePath).AbsoluteUri,
                FromType.FromLocation);

            Player.Media = Video;
        }

        private void Player_MediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e)
        {
            if (VideoPlayer.MediaPlayer.Media == null)
                return;

            VideoPlayer.MediaPlayer.Play();
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlaylistManager.MarkAsRerun(VideoPlayer.MediaPlayer, PlaylistManager.Playlist[VideoControls.CurrentIndex]);
            SetNewVideo(VideoControls.CurrentIndex);

            VideoPlayer.MediaPlayer.Play();
        }
    }
}
