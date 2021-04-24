using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarsPhotoFetcher
{
    public class ApiHelper
    {
        private const string BASE_URL = "https://api.nasa.gov/mars-photos/api/v1/";

        private readonly HttpClient client = new();

        private readonly string key;

        public ApiHelper(string key)
        {
            this.key = key ?? throw new ArgumentOutOfRangeException(nameof(key));
        }

        public async Task<List<Manifest>> GetManifests(Rover rover)
        {
            var url = new StringBuilder();

            url.Append(BASE_URL + "manifests/");
            url.Append(rover.ToString().ToLower());
            url.Append("?api_key=");
            url.Append(key);

            var json = await client.GetStringAsync(url.ToString());

            return ManifestParser.Parse(json);
        }

        public async Task<List<Photo>> GetPhotos(Rover rover, DateTime earthDate)
        {
            var url = new StringBuilder();

            url.Append(BASE_URL + "rovers/");
            url.Append(rover.ToString().ToLower());
            url.Append("/photos?earth_date=");
            url.Append(earthDate.ToString("yyyy-MM-dd"));
            url.Append("&api_key=");
            url.Append(key);

            var json = await client.GetStringAsync(url.ToString());

            return PhotosParser.GetPhotos(rover, earthDate, json);
        }
    }
}
