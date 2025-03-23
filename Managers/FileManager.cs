using WPF_BingeBox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace WPF_BingeBox.Managers
{
    public class FileManager
    {
        public static string CurrentDir = AppDomain.CurrentDomain.BaseDirectory;
        public static string ParentDir = Directory.GetParent(CurrentDir).Parent.Parent.FullName;
        public static string DataFolder = Path.Combine(ParentDir, "Data");
        public static string SeriesFile = Path.Combine(DataFolder, "Series.json");

        public List<Series> FullSeriesList;

        public FileManager()
        {
            VerifyPaths();
            FullSeriesList = LoadFromFile(SeriesFile);
            Debug.WriteLine("SeriesFile" + SeriesFile);
        }

        public void VerifyPaths()
        {
            try
            {
                if (!Directory.Exists(CurrentDir))
                    return;

                if (!Directory.Exists(DataFolder))
                {
                    Directory.CreateDirectory(DataFolder);
                }

                if (!File.Exists(SeriesFile))
                {
                    using (File.Create(SeriesFile)) { }
                }
            }catch(Exception ex)
            {
                Debug.WriteLine($"Could not Find/Create Dirrectory: {ex}");
            }
        }

        public void SaveToFile(Series series, string filePath)
        {
            if (series == null)
            {
                Debug.WriteLine($"Series is Null");
                return;
            }
            if (FullSeriesList == null)
            {
                Debug.WriteLine($"Series List is Null");
                return;
            }

            FullSeriesList.Add(series);

            string listString = JsonSerializer.Serialize(FullSeriesList);

            File.WriteAllText(filePath, listString);
            FullSeriesList = LoadFromFile(SeriesFile);
        }

        public void SaveFullList(List<Series> fullList, string filePath)
        {
            string listString = JsonSerializer.Serialize(FullSeriesList);
            File.WriteAllText(filePath, listString);
            FullSeriesList = LoadFromFile(SeriesFile);
        }

        public static List<Series> LoadFromFile(string filePath)
        {
            string listJson = File.ReadAllText(filePath);

            switch (listJson)
            {
                case null:
                    return new List<Series>();
                case "":
                    return new List<Series>();
                default:
                    List<Series> seriesList = JsonSerializer.Deserialize<List<Series>>(listJson);
                    if (seriesList != null)
                    {
                        seriesList = seriesList.OrderBy(i => i.SeriesName).ToList<Series>();
                        foreach(Series show in seriesList)
                        {
                            Debug.WriteLine($"Series Name: {show.SeriesName}");
                        }
                        return seriesList;
                    }
                    else
                    {
                        return new List<Series>();
                    }
                        
            }
        }
    }
}
