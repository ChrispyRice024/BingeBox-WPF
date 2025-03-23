using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Packaging;
using System.Windows.Controls;
using System.Windows;
using System.Text.Json.Serialization;

namespace WPF_BingeBox.Models
{
    public class Series
    {
        public long ID { get; set; }
        public string SeriesName { get; set; }
        public List<Season> Seasons { get; set; }
        public int TotalSeasons { get; set; }
        public int TotalEpisodes { get; set; }
        public bool IsEpisodic { get; set; }
        public string Location { get; set; }
        [JsonConstructor]
        public Series() { }

        public Series(string seriesName, bool isEpisodic, string location)
        {
            ID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            SeriesName = seriesName;
            IsEpisodic = isEpisodic;
            Location = location;
            Seasons = new List<Season>();
            TotalSeasons = new DirectoryInfo(location).GetDirectories().Length;
        }

        public static async Task<Series> SeriesCon(string seriesName, bool isEpisodic, string location, IProgress<int> progress)
        {
            Series series = new Series(seriesName, isEpisodic, location);
            await series.GetSeasonsAndEpisodes(progress);
            MessageBox.Show("Series Added Succesfully");
            return series;
        }

        private async Task GetSeasonsAndEpisodes(IProgress<int> progress)
        {
            int processedEpisodes = 0;
            int totalEpisodesCount = 0;

            var directories = Directory.GetDirectories(Location);
            List<Season> tempSeasons = new List<Season>();

            foreach(string dir in directories)
            {
                string fileName = Path.GetFileName(dir);

                if (fileName.StartsWith("Season") && int.TryParse(fileName.Substring(7), out int _))
                {
                    string[] extensions = new string[5]
                    {
                        "*.mp4",
                        "*.avi",
                        "*.mkv",
                        "*.mov",
                        "*.wmv"
                    };

                    foreach(string ext in extensions)
                    {
                        totalEpisodesCount += Directory.GetFiles(dir, ext).Length;
                    }
                }
            }

            await Task.Run(() =>
            {
                foreach(string dir in directories)
                {
                    string fileName = Path.GetFileName(dir);

                    if(fileName.StartsWith("Season") && int.TryParse(fileName.Substring(7), out int _)){
                        Season season = new Season(fileName, dir, SeriesName);

                        string[] extensions = new string[5]
                        {
                            "*.mp4",
                            "*.avi",
                            "*.mkv",
                            "*.mov",
                            "*.wmv"
                        };

                        foreach(string ext in extensions)
                        {
                            foreach(string file in Directory.GetFiles(dir, ext))
                            {
                                Episode episode = new Episode(Path.GetFileNameWithoutExtension(file), file, SeriesName, fileName);
                                season.Episodes.Add(episode);

                                processedEpisodes++;
                                progress.Report((processedEpisodes * 100) / totalEpisodesCount);
                            }
                        }
                        season.Episodes = season.Episodes.OrderBy(e => e.EpisodeTitle).ToList();
                        tempSeasons.Add(season);
                    }
                }
            });
            //Seasons.Clear();
            Seasons.AddRange(tempSeasons);

            TotalEpisodes = Seasons.Sum(s => s.Episodes.Count);
        }
    }
}
