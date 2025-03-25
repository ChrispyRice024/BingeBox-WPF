using WPF_BingeBox.Models;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Windows.Media.Playlists;

namespace WPF_BingeBox.Managers
{
    public class PlaylistManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<Series> _shuffledList;

        private FileManager _fileManager;
        
        private ObservableCollection<Episode> _playlist = new ObservableCollection<Episode>();
        public ObservableCollection<Episode> Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlist)));
            }
        }

        public PlaylistManager(FileManager fileManager)
        {
            _fileManager = fileManager;
            ShuffleList(_fileManager.FullSeriesList);
        }

        private List<T> ShuffleList<T>(List<T> list)
        {
            List<T> seriesList = new List<T>(list);
            Random rng = new Random();
            for (int i = seriesList.Count - 1; i > 0; --i)
            {
                int rand = rng.Next(i + 1);
                (seriesList[i], seriesList[rand]) = (seriesList[rand], seriesList[i]);
            }
            return seriesList;
        }

        public void PopulatePlaylist(List<Series> seriesList)
        {
            if (seriesList == null)
                return;

            var shuffledList = ShuffleList(seriesList);

            if (shuffledList == null)
                return;

            int index = 0;
            List<Episode> unfinishedEpisodes;
            int totalEpisodes = seriesList.Sum(s => s.TotalEpisodes);
            
            Random rand = new Random();
            if (Playlist.Count < 50)
            {
                while (Playlist.Count < 50 && Playlist.Count < totalEpisodes)
                {
                    if (shuffledList[index].IsEpisodic)
                    {
                        unfinishedEpisodes = shuffledList[index].Seasons
                            .SelectMany(s => s.Episodes)
                            .Where(ep => !ep.IsRerun).ToList();
                        
                        List<Episode> shuffledEpisodes = ShuffleList(unfinishedEpisodes);
                        int randomIndex = rand.Next(shuffledEpisodes.Count);
                        if (!Playlist.Any(ep => ep == shuffledEpisodes[randomIndex]))
                        {
                            Playlist.Add(shuffledEpisodes[randomIndex]);
                        }
                    }
                    else
                    {
                        GetSeason(shuffledList[index]);
                        Debug.WriteLine($"totalEpisodes: {totalEpisodes}");
                        Debug.WriteLine($"While Playlist.Count: {Playlist.Count}");
                    }
                    ++index;
                    if (index > shuffledList.Count - 1)
                    {
                        shuffledList = ShuffleList(seriesList);
                        index = 0;
                    }
                }
            }
        }

        private void GetSeason(Series series)
        {
            List<Season> availableSeasons = series.Seasons
                .Where(season => season.Episodes
                .Any(ep => !ep.IsRerun)).ToList();


            if (availableSeasons.Count == 0)
                return;
            for(int i = 0; i < availableSeasons.Count; i++)
            {
                List<Episode> availableEpisodes = availableSeasons[i].Episodes
                .Where(ep => !ep.IsRerun && !Playlist.Contains(ep))
                .ToList();

                if(availableEpisodes.Count > 0)
                {
                    Playlist.Add(availableEpisodes[0]);
                    return;
                }
            }
            Debug.WriteLine("no more unwatched episodes");
        }

        public void MarkAsRerun(LibVLCSharp.Shared.MediaPlayer player, Episode episode)
        {
            //TimeSpan watchedTime = TimeSpan.FromSeconds((double)player.Time);
            //TimeSpan maxTime = TimeSpan.FromSeconds(episode.Duration.TotalSeconds * 0.92);

            //if (player.Media != null && watchedTime >= maxTime)
            episode.IsRerun = true;
            _fileManager.SaveFullList(_fileManager.FullSeriesList, FileManager.SeriesFile);
        }
    }
}
