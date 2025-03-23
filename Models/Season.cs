using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WPF_BingeBox.Models
{
    public class Season
    {
        public long ID { get; set; }
        public string SeasonName { get; set; }
        public string Location { get; set; }
        public List<Episode> Episodes { get; set; }
        public int TotalEpisodes { get; set; }
        public string ParentSeries { get; set; }

        public Season() { }
        public Season(string seasonName, string seasonFolder, string parentSeries)
        {
            SeasonName = seasonName;
            Episodes = new List<Episode>();
            Location = seasonFolder;
            ParentSeries = parentSeries;
            TotalEpisodes = 0;

            string[] extensions = new string[5]
            {
                "*.mp4",
                "*.avi",
                "*.mkv",
                "*.mov",
                "*.wmv"
            };

            DirectoryInfo dirInfo = new DirectoryInfo(seasonFolder);
            foreach(string ex in extensions)
            {
                TotalEpisodes += dirInfo.GetFiles(ex).Length;
            }
        }
    }
}
