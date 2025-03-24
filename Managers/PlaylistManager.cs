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

        //private void GetSeries(List<Series> shuffledList)
        //{
        //    if (shuffledList == null)
        //        return;


        //}
        //public void PopulatePlaylist()
        //{
        //    try
        //    {
        //        Debug.WriteLine("FullSeriesList.Count" + _fileManager.FullSeriesList.Count);
        //        if(_fileManager.FullSeriesList.Count > 0)
        //        {
        //            ShuffleList(_fileManager.FullSeriesList);
        //            Debug.WriteLine($"PopulatePlaylist - SeriesList.Count: {_shuffledList.Count}");
        //            if (_shuffledList.Count == 0 || _shuffledList == null)
        //                return;

        //            int totalEpisodes = _fileManager.FullSeriesList.Sum(s => s.TotalEpisodes);
        //            Debug.WriteLine($"Populate Playlist\nTotal Episodes: {totalEpisodes}\nseriesList.Count: {_shuffledList.Count}\n");
        //            //while (Playlist.Count < 50)
        //            //{
        //            //    Debug.WriteLine($"Playlist Count: {Playlist.Count}");
        //            //    GetSeries(totalEpisodes);
        //            //}

        //            int itemsPerCycle = 50;
        //            while(Playlist.Count < totalEpisodes)
        //            {
        //                Debug.WriteLine($"While - Playlist.Count");
        //                int itemsToAdd = Math.Min(itemsPerCycle, totalEpisodes - Playlist.Count);
        //                for(int i = 0; i < itemsPerCycle; i++)
        //                {
        //                    GetSeries(totalEpisodes);
        //                    Debug.WriteLine($"For - latest item in Playlist: {Playlist[Playlist.Count - 1]}");
        //                }
        //                if(itemsToAdd == 0 || Playlist.Count % itemsToAdd != 0)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"There was a problem populating the playlist: {ex.Message}\n" +
        //            $"StackTrace: {ex.StackTrace}\n" +
        //            $"Source: {ex.Source}\n" +
        //            $"Data: {ex.Data}");
        //    }
        //}

        //public void GetSeries(int totalEpisodes)
        //{
        //    Debug.WriteLine($"_shuffledList.Count: {_shuffledList.Count}");
        //    if (_shuffledList == null)
        //        return;
        //    foreach(Series item in _shuffledList)
        //    {
        //        Debug.WriteLine($"GetSereis - seriesName: {item.SeriesName}");
        //    }

        //    if(_shuffledList.Count == 0)
        //    {
        //        ShuffleList(_fileManager.FullSeriesList);
        //    }

        //    GetSeason(_shuffledList[0]);
        //    _shuffledList.RemoveAt(0);
        //    //return;
        //}

        //private void GetSeason(Series series)
        //{
        //    List<Season> availableSeasons = series.Seasons
        //        .Where(season => !season.Episodes.All(ep => ep.IsRerun) &&
        //        season.Episodes.Any(ep => !Playlist.Contains(ep)))
        //        .ToList();

        //    Debug.WriteLine($"availableSeasons.Count: {availableSeasons.Count}");
        //    if (availableSeasons.Count > 0)
        //    {
        //        //foreach(Season season in availableSeasons)
        //        //{
        //        //    Debug.WriteLine($"{season.SeasonName}");
        //        //    foreach(Episode ep in season.Episodes)
        //        //    {
        //        //        Debug.WriteLine($"Episode Title: {ep.EpisodeTitle}");
        //        //    }
        //        //}
                
        //        if (series.IsEpisodic)
        //        {
        //            int rand = new Random().Next(availableSeasons.Count - 1);
        //            Debug.WriteLine($"Random: {rand}");
        //            GetEpisode(availableSeasons, rand, true);
        //        }
        //        else
        //        {
        //            Debug.WriteLine($"availableSeasons: {availableSeasons.Count}");
        //            GetEpisode(availableSeasons, 0, false);
        //        }
        //    }
        //    else
        //    {

        //        Debug.WriteLine("No Episodes In AvailableSeasons");
        //        return;
        //    }
            
        //}
        //private int TestEpisode(Episode ep, ref int attempts)
        //{
        //    bool isSelected = Playlist.Any(e => e.EpisodeTitle == ep.EpisodeTitle);

        //    if (isSelected)
        //    {
        //        return attempts++;
        //    }
        //    else
        //    {
        //        return attempts;
        //    }
        //}

        //private void GetEpisode(List<Season> availableSeasons, int index, bool isEpisodic)
        //{
        //    //if (Playlist.Count >= 50)
        //    //    return;
        //    Season season = availableSeasons[index];

        //    List<Episode> unwatched = season.Episodes
        //        .Where(ep => !ep.IsRerun)
        //        .ToList();

        //    if (unwatched.Count == 0)
        //        return;

        //    if (isEpisodic)
        //    {
        //        int attempts = 0;

        //        while(attempts < unwatched.Count)
        //        {
        //            int rand = new Random().Next(unwatched.Count);
        //            Episode candidate = unwatched[rand];
        //            Debug.WriteLine($"Season Name: {candidate.EpisodeTitle}");
        //            if(!Playlist.Any(ep => ep.EpisodeTitle == candidate.EpisodeTitle))
        //            {
        //                Playlist.Add(candidate);
        //                return;
        //            }
        //            else
        //            {
        //                attempts++;
        //            }


        //            if (attempts > season.Episodes.Count)
        //            {
        //                Debug.WriteLine($"Season Name2: {candidate.EpisodeTitle}");
        //                season = availableSeasons[index + 1];
        //                unwatched = season.Episodes
        //                    .Where(ep => !ep.IsRerun)
        //                    .ToList();
        //                Debug.WriteLine($"Season Name3: {candidate.EpisodeTitle}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for(int i = 0; i < season.Episodes.Count; i++)
        //        {
        //            Episode candidate = unwatched[i];
        //            Debug.WriteLine($"Season Name(Else): {candidate.EpisodeTitle}");
        //            if (!Playlist.Any(ep => ep.EpisodeTitle == candidate.EpisodeTitle))
        //            {
        //                Playlist.Add(candidate);
        //                return;
        //            }
        //            else
        //            {
        //                candidate = unwatched[i + 1];

        //                Debug.WriteLine($"Candidate Failed: {candidate.EpisodeTitle}");
        //            }
        //        }
        //    }
        //}

        public void MarkAsRerun(LibVLCSharp.Shared.MediaPlayer player, Episode episode)
        {
            TimeSpan watchedTime = TimeSpan.FromSeconds((double)player.Time);
            TimeSpan maxTime = TimeSpan.FromSeconds(episode.Duration.TotalSeconds * 0.92);

            if (player.Media != null && watchedTime >= maxTime)
                episode.IsRerun = true;
            _fileManager.SaveFullList(_fileManager.FullSeriesList, FileManager.SeriesFile);
        }
    }
}
