using LibVLCSharp.Shared;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_BingeBox.Models
{
    public class Episode
    {
        public long ID { get; set; }
        public bool IsRerun { get; set; }
        public string EpisodeTitle { get; set; }
        public string EpisodePath { get; set; }
        public string ParentSeries { get; set; }
        public string ParentSeason { get; set; }
        public TimeSpan Duration { get; set; }

        public Episode() { }
        public Episode(string episodeName, string location, string parentSeries, string parentSeason)
        {
            ID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            IsRerun = false;
            EpisodePath = location;
            EpisodeTitle = episodeName;
            ParentSeries = parentSeries;
            ParentSeason = parentSeason;
            InitializeMediaAsync(location);
        }

        private async Task InitializeMediaAsync(string location)
        {
            using (LibVLC libVlc = new LibVLC())
            {
                using(new LibVLCSharp.Shared.MediaPlayer(libVlc))
                {
                    Media media = new Media(libVlc, location, FromType.FromPath);

                    TimeSpan duration = TimeSpan.FromMilliseconds((double)media.Duration);

                    Duration = duration;
                }
            }
        }
    }
}
