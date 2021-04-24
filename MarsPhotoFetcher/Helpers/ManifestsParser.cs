using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MarsPhotoFetcher
{
    public static class ManifestParser
    {
        private class Root
        {
            public PhotoManifest Photo_Manifest { get; set; }
        }

        private class PhotoManifest
        {
            public string Name { get; set; }
            public string Landing_date { get; set; }
            public string Launch_date { get; set; }
            public string Status { get; set; }
            public int Max_Sol { get; set; }
            public string Max_Date { get; set; }
            public int Total_Photos { get; set; }
            public PhotoInfo[] Photos { get; set; }
        }

        private class PhotoInfo
        {
            public int Sol { get; set; }
            public string Earth_date { get; set; }
            public int Total_Photos { get; set; }
            public string[] Cameras { get; set; }
        }

        public static List<Manifest> Parse(string json)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<Root>(json, options);

            var manifests = new List<Manifest>();

            foreach (var photo in root.Photo_Manifest.Photos)
            {
                manifests.Add(new Manifest()
                {
                    Cameras = photo.Cameras.Select(c => c.ToCamera()).ToList(),
                    EarthDate = DateTime.Parse(photo.Earth_date),
                    Sol = photo.Sol,
                    TotalPhotos = photo.Total_Photos
                });
            }

            return manifests;
        }
    }
}
